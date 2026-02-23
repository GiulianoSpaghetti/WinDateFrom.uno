namespace WinDateFrom.Presentation;

public sealed partial class MainPage : Page
{
    public static MainPage Current { get; private set; } = null!;
    public MainPage()
    {
        this.InitializeComponent();
        Current = this;
        this.Loaded += OnLoaded;
    }

    //property of giulio sorrentino and giovanna san severino
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        DarkMode.IsOn = SystemThemeHelper.IsRootInDarkMode(this.XamlRoot);
    }

    //property of giovanna san severino
    private void OnDarkModeToggleToggled(object sender, RoutedEventArgs e)
    {
        SystemThemeHelper.SetApplicationTheme(this.XamlRoot, DarkMode.IsOn ?  ElementTheme.Dark : ElementTheme.Light);
    }
}

