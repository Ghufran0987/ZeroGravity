namespace ZeroGravity.Models.Accounts
{
    public class UpdateWizardRequest
    {
        public int AccountId { get; set; }

        public bool CompletedFirstUseWizard { get; set; }
    }
}