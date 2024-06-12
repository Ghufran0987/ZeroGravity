using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Resources.Fonts;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

// Set default language to en-US for cultures without a specific resource file
// so the ResourceManager can return values from the default Resx file (AppResources.resx).
// If  the NeutralResourcesLanguage attribute is not being specified,
// the ResourceManager will return null values for any cultures without specific resource file
// reference: https://docs.microsoft.com/de-de/xamarin/xamarin-forms/app-fundamentals/localization/text?pivots=windows
[assembly: NeutralResourcesLanguage("en-US")]

// register custom fonts
[assembly: ExportFont("avenir-next-demi.ttf", Alias = CustomFontName.AvenirNextDemi)]
[assembly: ExportFont("avenir-next-medium.ttf", Alias = CustomFontName.AvenirNextMedium)]
[assembly: ExportFont("samantha.ttf", Alias = CustomFontName.Samantha)]
[assembly: ExportFont("fa-light-300.ttf", Alias = CustomFontName.FaLight300)]
[assembly: ExportFont("fa-brands-400.ttf", Alias = CustomFontName.FaBrands400)]
//[assembly: ExportFont("gordita-regular.otf", Alias = CustomFontName.GorditaRegular)]
//[assembly: ExportFont("gordita-medium.otf", Alias = CustomFontName.GorditaMedium)]
//[assembly: ExportFont("gordita-bold.otf", Alias = CustomFontName.GorditaBold)]

[assembly: ExportFont("OpenSans-Bold.ttf", Alias = CustomFontName.OpenSanBold)]
[assembly: ExportFont("OpenSans-Light.ttf", Alias = CustomFontName.OpenSanMedium)]
[assembly: ExportFont("OpenSans-Regular", Alias = CustomFontName.OpenSanRegular)]

[assembly: ExportFont("PlayfairDisplay-Bold.ttf", Alias = CustomFontName.PlayfairDisplaynBold)]
[assembly: ExportFont("PlayfairDisplay-Regular.ttf", Alias = CustomFontName.PlayfairDisplaynRegular)]
