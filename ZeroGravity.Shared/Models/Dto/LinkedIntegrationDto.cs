namespace ZeroGravity.Shared.Models.Dto
{
    public class LinkedIntegrationDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int IntegrationId { get; set; }

        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}