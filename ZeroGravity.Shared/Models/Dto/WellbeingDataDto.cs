using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class WellbeingDataDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int Rating { get; set; }

        public DateTime Created { get; set; }
    }
}
