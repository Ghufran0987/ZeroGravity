using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class UserQueryDataDto
    {
        public UserQueryDataDto()
        {
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Feedback { get; set; }

        public DateTime Created { get; set; }
    }
}