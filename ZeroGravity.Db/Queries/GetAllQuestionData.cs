using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetAllQuestionData : IDbQuery<List<QuestionData>, ZeroGravityContext>
    {
        public GetAllQuestionData()
        {

        }

        public async Task<List<QuestionData>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.Questions.Include(x => x.Answers).OrderBy(x => x.Id).ToListAsync();
        }
    }
}