using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class UserQueryData : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        public string Feedback { get; set; }

        public DateTime Created { get; set; }
    }
}