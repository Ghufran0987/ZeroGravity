using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using ZeroGravity.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.SugarBeat.Algorithms;
using ZeroGravity.SugarBeat.Algorithms.Models;

namespace ZeroGravity.Services
{
    public class SugarBeatGlucoseDataService : ISugarBeatGlucoseDataService
    {
        private readonly ILogger<SugarBeatGlucoseDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public SugarBeatGlucoseDataService(ILogger<SugarBeatGlucoseDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<SugarBeatGlucoseData> AddAsync(SugarBeatGlucoseData glucoseData, bool saveChanges = true)
        {
            // Check for duplicate
            SugarBeatGlucoseData glucose = await GetGlucoseByTimeId(glucoseData.AccountId, glucoseData.Created);
            if (glucose != null)
            {
                return glucose;
            }

            SugarBeatGlucoseData result = await _repository.AddAsync(glucoseData, saveChanges);
            if (result != null)
            {
                result = await _repository.Execute(new GetSugarBeatGlucoseDataById(result.Id));
                if (result != null && result.SessionId > 0)
                {
                    var jobId = BackgroundJob.Enqueue(() => ComputeGlouceValueAsync(result.AccountId, (int)result.SessionId));
                }
            }
            return result;
        }

        public async Task<List<SugarBeatGlucoseData>> GetGlucoseForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool isGlucoseNull = false)
        {
            var glucoseData = await _repository.Execute(new GetGlucoseForPeriodAsync(accountId, fromDate, toDate, isGlucoseNull));
            return glucoseData;
        }

        public async Task<List<SugarBeatGlucoseData>> GetAllGlucoseForSessionId(int accountId, int sessionId, bool isGlucoseNull = false)
        {
            var glucoseData = await _repository.Execute(new GetAllGlucoseForSessionId(accountId, sessionId, isGlucoseNull));
            return glucoseData;
        }

        public async Task<SugarBeatGlucoseData> GetGlucoseByTimeId(int accountId, DateTime time)
        {
            var glucoseData = await _repository.Execute(new GetGlucoseByTime(accountId, time));
            return glucoseData;
        }

        public async Task<SugarBeatGlucoseData> GetByIdAsync(int id)
        {
            var glucoseData = await _repository.Execute(new GetSugarBeatGlucoseDataById(id));
            return glucoseData;
        }

        public async Task<SugarBeatGlucoseData> UpdateAsync(SugarBeatGlucoseData glucoseData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetSugarBeatGlucoseDataById(glucoseData.Id));
            return await _repository.UpdateAsync(entityToUpdate, glucoseData, saveChanges);
        }

        public async Task ComputeGlouceValueAsync(int accountId, int sessionId)
        {
            try
            {
                var result = await _repository.Execute(new GetAllGlucoseForSessionId(accountId, sessionId, true));

                // update result to filter out all invalid sensor measurements
                result = result.FindAll(entry => isGlucoseDataValid(entry));

                if (result != null && result.Count > 0)
                {
                    var inputs = new List<GlucoseAlgorithmInputEntry>();
                    foreach (var gd in result)
                    {
                        inputs.Add(new GlucoseAlgorithmInputEntry(gd.Id, gd.SensorValue, null));
                    }

                    var alg = new GlucoseAlgorithm();
                    var output = alg.Run(inputs,
                                         null,
                                         SugarBeatAlgorithmSettings.F0_VALUE,
                                         SugarBeatAlgorithmSettings.RECALIBRATE_AFTER);
                    if (output.HasValue)
                    {
                        foreach (var outputGv in output.Value.glucoseAlgorithmOutputEntries)
                        {
                            var find = result.Find(x => x.Id == outputGv.Timestamp);
                            if (find != null)
                            {
                                find.GlucoseValue = outputGv.GlucoseValues;
                            }
                        }
                        // Update IsSensorWarmedUp
                        if (output.Value.warmupIndex.HasValue)
                        {
                            int index = output.Value.warmupIndex.Value;
                            if (index >= 0 && index <= output.Value.glucoseAlgorithmOutputEntries.Count - 1)
                            {
                                // Find Glucose Value for the index and update
                                var g = output.Value.glucoseAlgorithmOutputEntries[index];
                                if (g != null)
                                {
                                    var find = result.Find(x => x.Id == g.Timestamp);
                                    if (find != null)
                                    {
                                        find.IsSensorWarmedUp = true;
                                    }
                                }
                            }
                        }
                        var updatedList = await _repository.UpdateBulkAsync(result, true);
                        if (updatedList != null && result.Count > 0)
                        {
                            _logger.LogInformation("ComputeGlouceValueAsync Job Completed For Account Id:{0}, SessionId:{1}", accountId, sessionId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository:ComputeGlouceValueAsync  failed for Account Id:{0}, SessionId:{1}", accountId, sessionId);
            }
        }

        private Boolean isGlucoseDataValid(SugarBeatGlucoseData data)
        {
            if (data.CE != null)
            {
                if (data.CE < 0 || data.CE > 2900)
                {
                    return false;
                }
            }

            if (data.RE != null)
            {
                if (data.RE < 500 || data.RE > 1500)
                {
                    return false;
                }
            }

            if (data.SensorValue < 1 || data.SensorValue > 420000)
            {
                return false;
            }

            return true;
        }
    }
}