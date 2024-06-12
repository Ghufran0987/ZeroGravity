using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class IntegrationDataDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public bool IsLinked { get; set; }
    }
}