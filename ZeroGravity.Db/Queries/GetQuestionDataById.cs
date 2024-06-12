using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetQuestionDataById : IDbQuery<QuestionData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetQuestionDataById(int id)
        {
            _id = id;
        }

        public async Task<QuestionData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.Questions.Include(x => x.Answers).FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}