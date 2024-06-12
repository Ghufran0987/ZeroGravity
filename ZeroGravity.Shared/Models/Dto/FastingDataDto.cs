using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FastingDataDto
    {
        public FastingDataDto()
        {
            Start = DateTime.Now;
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Created { get; set; }
    }
}
