using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class UpdateRequestDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTimeDisplayType DateTimeDisplayType { get; set; }
        public UnitDisplayType UnitDisplayType { get; set; }
    }
}