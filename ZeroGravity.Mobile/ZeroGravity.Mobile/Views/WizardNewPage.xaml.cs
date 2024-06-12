using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardNewPage : IWizardNewPage
    {
        public WizardNewPage()
        {
            InitializeComponent();
        }
    }
}