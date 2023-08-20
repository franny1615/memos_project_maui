using System.ComponentModel;
using System.Globalization;
using memos_project_maui.Resources.Localization;

namespace memos_project_maui.Utilities;

public class LanguageManager : INotifyPropertyChanged
{
    private LanguageManager()
    {
        AppLanguage.Culture = CultureInfo.CurrentCulture;
    }

    public static LanguageManager Instance { get; } = new();

    public string this[string resourceKey]
        => AppLanguage.ResourceManager.GetObject(resourceKey, AppLanguage.Culture).ToString();

    public event PropertyChangedEventHandler PropertyChanged;

    public void SetCulture(CultureInfo culture)
    {
        AppLanguage.Culture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}

