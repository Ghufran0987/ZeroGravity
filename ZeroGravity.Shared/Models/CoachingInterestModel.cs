using System.Collections.Generic;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models
{
    public class CoachingInterestModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public CoachingType Coaching { get; set; }
        public List<string> CoachingOptions { get; set; }
    }
}