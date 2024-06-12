using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MetabolicHealthTrackingHistoryPage : IMetabolicHealthTrackingHistoryPage
    {
        public MetabolicHealthTrackingHistoryPage()
        {
            InitializeComponent();
        }

        private void SfListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            try
            {
                MetabolicHealthTrackingHistoryPageViewModel obj = this.BindingContext as MetabolicHealthTrackingHistoryPageViewModel;
                if (e.AddedItems.Count > 0)
                {
                    var selectedItem = e.AddedItems[0];
                    if (obj != null && selectedItem != null)
                    {
                        obj.OnEatingSessionTappedCommand((Proxies.SugarBeatEatingSessionProxy)selectedItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}