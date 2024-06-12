using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IQuestionDataService
    {
        Task<QuestionData> AddAsync(QuestionData questionData, bool saveChanges = true);

        Task<QuestionData> GetByIdAsync(int id);

        Task<IEnumerable<QuestionData>> GetQuestionsAsync();

        Task<QuestionData> UpdateAsync(QuestionData questionData, bool saveChanges = true);
    }
}