namespace ZeroGravity.Mobile.Contract.Helper
{
    /// <summary>
    /// Hilfsklasse zur Verhinderung mehrfacher Navigierung zur NoInternetConnectionPage.
    /// </summary>
    public static class InternetConnectionHelper
    {
        /* Da durch die verwendete TabView mehrere ViewModels bei App-Start initialisiert werden, werden deshalb auch mehrere Abragen zur Internetverbindung gemacht
           dies führt dazu, dass das NoInternetConnectionEvent in ViewModelBase mehrfach ausgelöst wird, was auch korrekt ist, 
           da beim Aufruf von OnNavigatedTo jedes ViewModels OnNavigatedTo in der Basisklasse ViewModelBase aufgerufen wird 
           (wo die Prüfung der Internetverbindung statt findet) und ggf. das NoInternetConnectionEvent gepublished wird
        */
        public static bool IsOnNoInternetConnectionPage { get ; set; }
    }
}
