using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class UpdateWizardRequestDto
    {
        public int AccountId { get; set; }

        public bool CompletedFirstUseWizard { get; set; }

        public DateTime? OnboardedDate { get; set; }

    }
}