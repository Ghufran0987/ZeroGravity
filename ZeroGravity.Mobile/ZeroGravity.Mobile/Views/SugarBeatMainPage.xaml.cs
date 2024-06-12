using Syncfusion.SfChart.XForms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SugarBeatMainPage
    {
        public SugarBeatMainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var pages = Navigation.NavigationStack.ToList();
            foreach (var page in pages)
            {
                if (page.GetType() == typeof(SugarBeatScanPage))
                    Navigation.RemovePage(page);
            }
        }


        void TimeAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
        {
            var minutes = int.Parse(e.LabelContent);

            var hours = System.Math.Round(minutes / 60.0);
            if(hours == 5)
            {
                e.LabelContent = "";
            }
            else
            {
                e.LabelContent = hours == 0 ? hours.ToString() : hours + " hour";
            }
        }

    }
}