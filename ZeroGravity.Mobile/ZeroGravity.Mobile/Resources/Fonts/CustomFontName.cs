using Xamarin.Forms;

namespace ZeroGravity.Mobile.Resources.Fonts
{
    public static class CustomFontName
    {
        public const string AvenirNextDemi = "AvenirDemi";
        public const string AvenirNextMedium = "AvenirMedium";
        public const string Samantha = "Samantha";
        public const string FaLight300 = "FaLight300";
        public const string FaBrands400 = "FaBrands400";
        //public const string GorditaRegular = "GorditaRegular";
        //public const string GorditaMedium = "GorditaMedium";
        //public const string GorditaBold = "GorditaBold";

        public const string OpenSanRegular = "OpenSan-Regular";
        public const string OpenSanMedium = "OpenSans-SemiBold";
        public const string OpenSanBold = "OpenSans-Bold";
        public const string OpenSanLight = "OpenSans-Light";

        public const string PlayfairDisplaynBold = "PlayfairDisplaynBold";
        public const string PlayfairDisplaynRegular = "PlayfairDisplaynRegular";
    }

    public static class Typography
    {
        public static TypographyItem HeadlineXl { get; } = new TypographyItem(CustomFontName.PlayfairDisplaynRegular, 42, 1, CustomColors.TextColorRegular);
        public static TypographyItem HeadlineL = new TypographyItem(CustomFontName.OpenSanBold, 27, 1, CustomColors.TextColorRegular);
        public static TypographyItem HeadlineM = new TypographyItem(CustomFontName.OpenSanBold, 21, 1, CustomColors.TextColorRegular);
        public static TypographyItem HeadlinePM = new TypographyItem(CustomFontName.PlayfairDisplaynBold, 21, 1, CustomColors.TextColorRegular);
        public static TypographyItem HeadlineS = new TypographyItem(CustomFontName.OpenSanMedium, 16, 1, CustomColors.TextColorRegular);
        public static TypographyItem Byline = new TypographyItem(CustomFontName.OpenSanLight, 14, 1, CustomColors.LineColor);
        public static TypographyItem ByBoldline = new TypographyItem(CustomFontName.OpenSanBold, 14, 1.7, CustomColors.TextColorLight);
        public static TypographyItem BylineIcon = new TypographyItem(CustomFontName.FaLight300, 16, 1, CustomColors.TextColorRegular);
        public static TypographyItem Paragraph = new TypographyItem(CustomFontName.OpenSanRegular, 14, 1.7, CustomColors.TextColorLight);
        public static TypographyItem ParagraphMedium = new TypographyItem(CustomFontName.OpenSanMedium, 14, 1.7, CustomColors.TextColorRegular);

        public static TypographyItem InputLayoutLabel = new TypographyItem(CustomFontName.OpenSanRegular, 14, 1.7, CustomColors.TextColorLight);
        public static TypographyItem InputLayoutEntry = new TypographyItem(CustomFontName.OpenSanMedium, 14, 1.7, CustomColors.TextColorLight);

        public static TypographyItem RadioButtonLabel = new TypographyItem(CustomFontName.OpenSanMedium, 16, 1, CustomColors.TextColorRegular);
        public static TypographyItem RadioButtonLDescription = new TypographyItem(CustomFontName.OpenSanRegular, 14, 1.3, CustomColors.TextColorLight);
        public static TypographyItem RadioButtonIconChecked = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.White);
        public static TypographyItem RadioButtonIconUnchecked = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.TextColorLight);

        public static TypographyItem ButtonIcon = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.White);
        public static TypographyItem ButtonText = new TypographyItem(CustomFontName.OpenSanBold, 16, 1, CustomColors.White);
        public static TypographyItem ButtonIconInverted = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.TextColorRegular);
        public static TypographyItem ButtonIconInvertedBrands = new TypographyItem(CustomFontName.FaBrands400, 24, 1, CustomColors.TextColorRegular);
        public static TypographyItem ButtonTextInverted = new TypographyItem(CustomFontName.OpenSanRegular, 16, 1, CustomColors.TextColorRegular);

        public static TypographyItem TabViewText = new TypographyItem(CustomFontName.OpenSanBold, 12, 1, CustomColors.Pink);
        public static TypographyItem TabViewIcon = new TypographyItem(CustomFontName.FaLight300, 36, 1, CustomColors.Pink);

        public static TypographyItem HomeTabViewText = new TypographyItem(CustomFontName.OpenSanBold, 12, 1, CustomColors.VeryLightGray);
        public static TypographyItem HomeTabViewIcon = new TypographyItem(CustomFontName.FaLight300, 24, 1, CustomColors.VeryLightGray);

        public static TypographyItem SliderHeader = new TypographyItem(CustomFontName.OpenSanMedium, 14, 1, CustomColors.TextColorRegular);
        public static TypographyItem SliderValue = new TypographyItem(CustomFontName.OpenSanRegular, 14, 1, CustomColors.TextColorLight);

        public static TypographyItem BubbleIcon = new TypographyItem(CustomFontName.FaLight300, 32, 1, CustomColors.TextColorRegular);
        public static TypographyItem BubbleText = new TypographyItem(CustomFontName.OpenSanMedium, 14, 1, CustomColors.TextColorRegular);
        public static TypographyItem BubbleTextInsteadOfIcon = new TypographyItem(CustomFontName.OpenSanMedium, 38, 1, CustomColors.TextColorRegular);
        public static TypographyItem BubbleBadgeText = new TypographyItem(CustomFontName.OpenSanRegular, 16, 1, CustomColors.White);
        public static TypographyItem BubbleBadgeIcon = new TypographyItem(CustomFontName.FaLight300, 16, 1, CustomColors.White);

        public static TypographyItem ListViewItemIcon = new TypographyItem(CustomFontName.FaLight300, 16, 1, CustomColors.Pink);
        public static TypographyItem ListViewItemText = new TypographyItem(CustomFontName.OpenSanRegular, 14, 1, CustomColors.Pink);

        public static TypographyItem MessageBoxRedIcon = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.Red);
        public static TypographyItem MessageBoxRedText = new TypographyItem(CustomFontName.OpenSanMedium, 16, 1.7, CustomColors.Red);

        public static TypographyItem MessageBoxGrayIcon = new TypographyItem(CustomFontName.FaLight300, 18, 1, CustomColors.DarkGray);
        public static TypographyItem MessageBoxGrayText = new TypographyItem(CustomFontName.OpenSanMedium, 16, 1.7, CustomColors.DarkGray);

        public static TypographyItem NavBarButtonText = new TypographyItem(CustomFontName.OpenSanMedium, 16, 1.2, CustomColors.TextColorRegular);
        public static TypographyItem NavBarButtonIcon = new TypographyItem(CustomFontName.FaLight300, 18, 1.2, CustomColors.TextColorRegular);

        //    public static TypographyItem TileButtonHeaderText = new TypographyItem(CustomFontName.OpenSanLight, 16, 1, CustomColors.TextColorRegular);
    }

    public class TypographyItem
    {
        public TypographyItem(string fontFamily, double fontSize, double lineHeight, Color textColor)
        {
            FontFamily = fontFamily;
            FontSize = fontSize;
            LineHeight = lineHeight;
            TextColor = textColor;
        }

        public string FontFamily { get; }
        public double FontSize { get; }
        public double LineHeight { get; }
        public Color TextColor { get; }
    }

    public static class CustomColors
    {
        //public static Color TextColorRegular = Color.FromHex("072330");
        public static Color TextColorRegular = Color.FromHex("072330");

        public static Color TextColorLight = Color.FromHex("4F4F4F");
        public static Color LineColor = Color.FromHex("4F4F4F");
        public static Color Green = Color.FromHex("00ba85");
        public static Color NewGreen = Color.FromHex("319C8A");
        public static Color Pink = Color.FromHex("FF5869");
        public static Color Purple = Color.FromHex("B996C3");
        public static Color PinkBackground = Color.FromHex("D3007F");
        public static Color White = Color.FromHex("ffffff");
        public static Color Yellow = Color.FromHex("F6D14C");
        public static Color Blue = Color.FromHex("3AA2F2");
        public static Color Red = Color.FromHex("d90000");
        public static Color LightRed = Color.FromHex("FFEFEF");
        public static Color LightGray = Color.FromHex("808080"); //lines, border, separatore1eaee
        public static Color VeryLightGray = Color.FromHex("CCC"); //lines, border, separatore1eaee
        public static Color MediumGray = Color.FromHex("96adb8");
        public static Color Black = Color.FromHex("000000");
        public static Color DarkGray = Color.FromHex("#072330"); // WizardImageSelection background
        public static Color LightBackground = Color.FromHex("F2F2F2");
        public static Color GrayBackgroundColor = Color.FromHex("E7E8EA");
        public static Color Orange = Color.FromHex("FFC658");
        public static Color LightGreen = Color.FromHex("319c8a");

        public static Color VeryLight = Color.FromHex("#98CEC5");
        public static Color Light = Color.FromHex("#319C8A");
        public static Color Medium = Color.FromHex("#FFC658");
        public static Color VeryHeavy = Color.FromHex("FF5869");
        public static Color Heavy = Color.FromHex("#ffacb4");
        public static Color None = Color.FromHex("#FF5869");
    }
}