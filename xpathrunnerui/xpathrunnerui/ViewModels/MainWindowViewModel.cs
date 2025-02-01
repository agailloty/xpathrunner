using CommunityToolkit.Mvvm.ComponentModel;

namespace xpathrunnerui.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                SetProperty(ref _isBusy, value);
            }
        }
    }
}