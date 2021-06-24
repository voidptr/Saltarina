using System.Windows.Input;

namespace Saltarina.ViewModels
{
    public interface INotifyIconViewModel
    {
        ICommand ExitApplicationCommand { get; }
        ICommand HideWindowCommand { get; }
        ICommand ShowWindowCommand { get; }
    }
}