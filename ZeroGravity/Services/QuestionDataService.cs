using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class QuestionDataService : IQuestionDataService
    {
        private readonly ILogger<QuestionDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public QuestionDataService(ILogger<QuestionDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<QuestionData> AddAsync(QuestionData questionData, bool saveChanges = true)
        {
            return await _repository.AddAsync(questionData, saveChanges);
        }

        public async Task<QuestionData> GetByIdAsync(int id)
        {
            return await _repository.Execute(new GetQuestionDataById(id));
        }

        public async Task<IEnumerable<QuestionData>> GetQuestionsAsync()
        {
            return await _repository.Execute(new GetAllQuestionData());
        }

        public async Task<QuestionData> UpdateAsync(QuestionData questionData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetQuestionDataById(questionData.Id));
            return await _repository.UpdateAsync(entityToUpdate, questionData, saveChanges);
        }
    }
}