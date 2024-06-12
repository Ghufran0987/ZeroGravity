using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MeditationDataDto
    {
        public MeditationDataDto()
        {
            Created = DateTime.Now;
        }


        public int Id { get; set; }
        public int AccountId { get; set; }

        public DateTime Created { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
