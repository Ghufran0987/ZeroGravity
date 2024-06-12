using System;
using Syncfusion.ListView.XForms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntegrationsPage : IIntegrationsPage
    {
        public IntegrationsPage()
        {
            InitializeComponent();
        }
    }
}