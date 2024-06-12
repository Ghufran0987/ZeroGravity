using System;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public Account Account => (Account)HttpContext.Items["Account"];

        protected void VerifyAccountId(int? accountId)
        {
            if (!accountId.HasValue)
            {
                throw new UnauthorizedAccessException(MessageType.Unauthorized);
            }

            if (Account.Id != accountId.Value)
            {
                throw new UnauthorizedAccessException(MessageType.Unauthorized);
            }
        }
    }
}
