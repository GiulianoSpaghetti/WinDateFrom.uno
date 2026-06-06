namespace WinDateFrom.Presentation;

public sealed partial class MainPage : Page
{
    public static MainPage Current { get; private set; } = null!;
    public MainPage()
    {
        this.InitializeComponent();
        Current = this;
    }
}
