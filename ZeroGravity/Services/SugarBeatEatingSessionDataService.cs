using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.SugarBeat.Algorithms;
using ZeroGravity.SugarBeat.Algorithms.Models;

public class SugarBeatEatingSessionDataService : ISugarBeatEatingSessionDataService
{
    private readonly ILogger<SugarBeatEatingSessionDataService> _logger;
    private readonly IRepository<ZeroGravityContext> _repository;

    public SugarBeatEatingSessionDataService(ILogger<SugarBeatEatingSessionDataService> logger, IRepository<ZeroGravityContext> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<SugarBeatEatingSession> AddAsync(SugarBeatEatingSession session, bool saveChanges = true)
    {
        // Check for duplicate
        var found = await _repository.Execute(new GetSugarBeatEatingSessionByDate(session));
        if (found) return null;
        return await _repository.AddAsync(session, saveChanges);
    }

    public async Task<SugarBeatEatingSession> GetByIdAsync(int id)
    {
        return await _repository.Execute(new GetSugarBeatEatingSessionById(id));
    }

    public async Task<List<SugarBeatEatingSession>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate)
    {
        var data = await _repository.Execute(new GetSugarBeatEatingSessionForPeriod(accountId, fromDate, toDate));
        return data;
    }

    public async Task<SugarBeatEatingSession> GetSessionByDateAsync(int accountId, DateTime startDate)
    {
        var data = await _repository.Execute(new GetSugarBeatEatingSessionForDate(accountId, startDate));
        return data;
    }

    public async Task<SugarBeatEatingSession> UpdateAsync(SugarBeatEatingSession session, bool saveChanges = true)
    {
        var entityToUpdate = await _repository.Execute(new GetSugarBeatEatingSessionById(session.Id));
        SugarBeatEatingSession updatedEatingSession = null;
        if (entityToUpdate != null)
        {
            updatedEatingSession = await _repository.UpdateAsync(entityToUpdate, session, saveChanges);
        }
        return updatedEatingSession;
    }

    public async Task ComputeMetabolicScoreAsync(int sessionId)
    {
        try
        {
            var eatingSession = await _repository.Execute(new GetSugarBeatEatingSessionById(sessionId));
            if (eatingSession != null && eatingSession.IsCompleted == false)
            {
                // Find all Glucose value where GV is Not Null
                var result = await _repository.Execute(new GetGlucoseForPeriodAsync(eatingSession.AccountId, eatingSession.StartTime, eatingSession.EndTime));
                if (result != null && result.Count > 0)
                {
                    var inputs = new List<ActualGlucouseValue>();
                    foreach (var gd in result)
                    {
                        if (gd.GlucoseValue.HasValue)
                        {
                            // Get Time in Minutes
                            var startTime = eatingSession.StartTime;
                            var mins = (int)gd.Created.Subtract(startTime).TotalMinutes;
                            inputs.Add(new ActualGlucouseValue()
                            {
                                Time = mins,
                                Value = gd.GlucoseValue.Value
                            });
                        }
                    }

                    // Check is sufficient data point available before calculation else come back later ?
                    var alg = new MetabolicScoreCalulator(inputs);
                    var score = alg.GetMetabolicScore();
                    if (DateTime.UtcNow > eatingSession.EndTime)
                    {
                        eatingSession.IsCompleted = true;
                    }
                    eatingSession.MetabolicScore = (decimal)score;
                    var save = await _repository.UpdateAsync(eatingSession, true);
                    if (save != null)
                    {
                        if (DateTime.UtcNow < save.EndTime && save.IsCompleted == false)
                        {
                            var jobId = BackgroundJob.Schedule(() => ComputeMetabolicScoreAsync(sessionId), new TimeSpan(0, 6, 0));
                        }
                    }
                    else
                    {
                        // Save failed try again after 10 Mins
                        var jobId = BackgroundJob.Schedule(() => ComputeMetabolicScoreAsync(sessionId), new TimeSpan(0, 10, 0));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository:ComputeMetabolicScoreAsync  failed for Eating SessionId:{1}", sessionId);
        }
    }
}