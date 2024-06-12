using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercisePage : IExercisePage
    {
        public ExercisePage()
        {
            InitializeComponent();
        }
    }
}