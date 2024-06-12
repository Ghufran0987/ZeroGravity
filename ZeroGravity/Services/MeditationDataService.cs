using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class MeditationDataService : IMeditationDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public MeditationDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }
        public async Task<MeditationData> AddAsync(MeditationData meditationData, bool saveChanges = true)
        {
            return await _repository.AddAsync(meditationData, saveChanges);
        }

        public async Task<MeditationData> GetByIdAsync(int id)
        {
            var meditation = await _repository.Execute(new GetMeditationByMeditationId(id));
            return meditation;
        }

        public async Task<List<MeditationData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var meditationDataList = await _repository.Execute(new GetAllMeditationsByAccountIdAndDate(accountId, dateTime));
            return meditationDataList;
        }
    }
}
