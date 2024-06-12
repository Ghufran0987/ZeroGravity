using System.Windows.Input;

namespace ZeroGravity.Mobile.Base.Interfaces
{
    public interface IPage
    {
        void InitPage(object obj);

        ICommand CloseOverlayCommand { get; }

        void OpenOverlay();
        void CloseOverlay();
    }
}
