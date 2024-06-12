using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    public partial class TermsAndPrivacyPage : ITermsAndPrivacyPage
    {
        public TermsAndPrivacyPage()
        {
            InitializeComponent();
        }

        public void SetTab(string tabTitle)
        {
            //var tabItem = TabView.Items.FirstOrDefault(x => x.Title.Equals(tabTitle));

            //if (tabItem != null)
            //{
            //    // verhindert, dass bei nicht vorhandener Internetverbindung (z.B. Flugmodus) bei iOS die App crasht
            //    Task.Run(async () =>
            //    {
            //        await MainThread.InvokeOnMainThreadAsync(() =>
            //        {
            //            TabView.SelectedIndex = TabView.Items.IndexOf(tabItem);
            //        });
            //    });
            //}
        }
    }
}
