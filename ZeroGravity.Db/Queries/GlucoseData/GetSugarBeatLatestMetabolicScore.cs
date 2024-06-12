using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatLatestMetabolicScore : IDbQuery<int, ZeroGravityContext>
    {
        private readonly int _accountId;

        public GetSugarBeatLatestMetabolicScore(int accountId)
        {
            _accountId = accountId;
        }

        public async Task<int> Execute(ZeroGravityContext dbContext)
        {
            int metabolicScore = -1;
            List<SugarBeatSessionData> sessions = await dbContext.SugarBeatSessions
                .Where(s => s.AccountId == _accountId)
                .OrderByDescending(s => s.Created)
                .Take(6)
                .ToListAsync();

            if (sessions.Count > 0)
            {
                // Check sessions for a valid score. Some sessions may not have any eating sessions
                // tracked against them, so check previous sessions for a valid score if necessary...
                foreach(SugarBeatSessionData session in sessions)
                {
                    List<SugarBeatEatingSession> eatingSessions = await dbContext.SugarBeatEatingSessions.Where(
                        e => e.AccountId == _accountId
                        && e.StartTime >= session.Created
                        && e.EndTime <= session.EndTime
                        && e.MetabolicScore > 0
                    ).ToListAsync();

                    if(eatingSessions.Count > 0)
                    {
                        int avg = (int)Math.Round(eatingSessions.Average(s => s.MetabolicScore));
                        if(avg > 0)
                        {
                            metabolicScore = avg;
                            break; // Found latest, bail out
                        }
                    }
                }
            }

            return metabolicScore;
        }
    }
}