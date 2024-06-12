using Prism.Events;

namespace ZeroGravity.Mobile.Events
{
    public class SaveWizardDataEvent : PubSubEvent<string>
    {
        public bool RequiresSave { get; set; }
    }
}