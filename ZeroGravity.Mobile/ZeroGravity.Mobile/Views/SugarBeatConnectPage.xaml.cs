using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SugarBeatConnectPage : ISugarBeatConnectPage
    {
        public SugarBeatConnectPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var pages = Navigation.NavigationStack.ToList();
                foreach (var page in pages)
                {
                    if (page.GetType() == typeof(SugarBeatScanPage))
                        Navigation.RemovePage(page);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnPass1TextChange(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as Entry;
                if (entry != null && (e.NewTextValue?.Length == 1))
                {
                    pass2?.Focus();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnPass2TextChange(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as Entry;
                if (entry != null && (e.NewTextValue?.Length == 1))
                {
                    pass3?.Focus();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnPass3TextChange(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as Entry;
                if (entry != null && (e.NewTextValue?.Length == 1))
                {
                    pass4?.Focus();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnPass4TextChange(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as Entry;
                if (entry != null && (e.NewTextValue?.Length == 1))
                {
                    if (pass1?.Text.Length == 1 && pass2.Text.Length == 1 && pass3.Text.Length == 1)
                    {
                        var obj = this.BindingContext as SugarBeatConnectPageViewModel;
                        if (obj != null)
                        {
                            obj.LinkDeviceCommand.Execute(null);
                        }
                        entry.Unfocus();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnPassFocus(object sender, FocusEventArgs e)
        {
            try
            {
                Entry entry = sender as Entry;
                if (entry != null && (entry.Text?.Length == 1))
                {
                    entry.Text = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SfListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            try
            {
                SugarBeatConnectPageViewModel obj = this.BindingContext as SugarBeatConnectPageViewModel;
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
