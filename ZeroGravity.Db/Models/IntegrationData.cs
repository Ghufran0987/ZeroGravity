using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    public class IntegrationData :  ModelBase
    {
        public string Name { get; set; }

        public IntegrationType IntegrationType { get; set; }
    }
}