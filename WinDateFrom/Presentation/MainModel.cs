using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ilfortunedinumeroneuno.Presentation;

public partial class MainModel : ObservableObject
{
    private INavigator _navigator;
    private readonly IStringLocalizer localizer;
    private readonly ILocalizationService localizationService;
    public MainModel(ILocalizationService localizationService,
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        this.localizationService = localizationService;
        this.localizationService.SetCurrentCultureAsync(localizationService.CurrentCulture);
        _navigator = navigator;
        this.localizer = localizer;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
        Augura = new RelayCommand(EseguiAuguri);
        Calcola = new RelayCommand(EseguiCalcolo);
        int anno, mese, giorno;
        localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        anno = localSettings.Values["anno"] as int? ?? DateTime.Now.Year;
        mese = localSettings.Values["mese"] as int? ?? DateTime.Now.Month;
        giorno = localSettings.Values["giorno"] as int? ?? DateTime.Now.Day; ;

        Data = new DateTime(anno, mese, giorno);
        Nome = localSettings.Values["nome"] as string ?? "";
        NomeEnabled=Nome.Length==0;
        AuguraVisible=Visibility.Collapsed;
        InsertName = $"{localizer["insert_name"]}";
        InsertDate = $"{localizer["select_date"]}";
        Calculate = $"{localizer["calculate"]}";
        Wish = $"{localizer["wish_happy_anniversary"]}";
    }
    public string? Title { get; }
    public string? InsertName { get; }
    public string? InsertDate { get; }
    public string? Calculate { get; }
    public string? Wish { get; }
    public static int MinChars = 3;
    ApplicationDataContainer localSettings;
    public string? Nome { get; set; }
    public bool? NomeEnabled { get; private set; }
    public string? Risultato { get; private set; }
    public string? Anniversario { get; private set; }
    private string ricorrenza;
    public Visibility AuguraVisible { get; private set; }
     
    public DateTimeOffset Data { get; set; }
    public ICommand Augura { get; }
    public ICommand Calcola { get; }

    private void EseguiAuguri()
    {
        Windows.System.Launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Happy%20{ricorrenza}%20my%20love."));
    }

    private void EseguiCalcolo()
    {
        Risultato = "";
        Anniversario = "";
        ricorrenza = "";
        Nome = Nome.Trim();
        AuguraVisible = Visibility.Collapsed;
        NomeEnabled = Nome.Length == 0;
        DateTime d = DateTime.Now;

        TimeSpan differenza = d - Data;
        if (differenza.Milliseconds < 0)
        {
            Risultato = localizer["invalid_rvalue"];
            OnPropertyChanged(nameof(Risultato));
            OnPropertyChanged(nameof(Anniversario));
            OnPropertyChanged(nameof(AuguraVisible));
            OnPropertyChanged(nameof(NomeEnabled));
            return;
        }
        if (differenza.Days > 1 && Nome.Length >= MinChars)
        {
            if (d.Day == Data.Day)
            {
                if (d.Month == Data.Month)
                {
                    Anniversario = localizer["is_your_anniversary"];
                    ricorrenza = "anniversary";
                }
                else
                {
                    Anniversario = localizer["is_your_mesiversary"];
                    ricorrenza = "mesiversary";
                }
            }
        }
        if (Nome.Length < MinChars)
            Risultato = $"{differenza.Days} {localizer["days_are_passed"]}";
        else
            Risultato = $"{localizer["you_met"]} {Nome} {localizer["about"]} {differenza.Days} {localizer["days_ago"]}.";
        if ( ricorrenza.Length==0)
        {
            AuguraVisible = Visibility.Collapsed;
        }
        else
        {
            AuguraVisible = Visibility.Visible;
        }
        MainPage.Current?.DispatcherQueue?.TryEnqueue(() =>
        {
            OnPropertyChanged(nameof(Risultato));
            OnPropertyChanged(nameof(Anniversario));
            OnPropertyChanged(nameof(AuguraVisible));
            OnPropertyChanged(nameof(NomeEnabled));
        });
        localSettings.Values["giorno"] = Data.Day;
        localSettings.Values["mese"] = Data.Month;
        localSettings.Values["anno"] = Data.Year;
        localSettings.Values["nome"] = Nome;


    }
}
