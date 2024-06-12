using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class LinkedIntegration : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public int IntegrationId { get; set; }

        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}