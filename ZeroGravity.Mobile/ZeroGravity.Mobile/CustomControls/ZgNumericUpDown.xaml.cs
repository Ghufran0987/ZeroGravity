using System;
using System.Collections.Generic;
using Syncfusion.SfNumericUpDown.XForms;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public partial class ZgNumericUpDown : ContentView
    {
        SfNumericUpDown upDown;
        Grid incrementGrid, decrementGrid;
        UpDownButtonSettings incSettings, decrementSettings;
        Image incrementImage, decrementImage;
        public ZgNumericUpDown()
        {
            InitializeComponent();
            incrementGrid = new Grid
            {
                HeightRequest = 40,
                WidthRequest = 40,
                BackgroundColor = Color.Blue
            };
            incSettings = new UpDownButtonSettings
            {
                ButtonView = incrementGrid,
                ButtonHeight = 45,
                ButtonWidth = 45
            };
            incrementImage = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = (FileImageSource)ImageSource.FromFile("up.png"),
                Aspect = Aspect.AspectFit
            };
            decrementSettings = new UpDownButtonSettings
            {
                ButtonView = decrementGrid,
                ButtonHeight = 45,
                ButtonWidth = 45
            };
            decrementGrid = new Grid
            {
                HeightRequest = 40,
                WidthRequest = 40,
                BackgroundColor = Color.Red

            };
            decrementImage = new Image
            {
                Source = (FileImageSource)ImageSource.FromFile("down.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            upDown = new SfNumericUpDown
            {
                SpinButtonAlignment = SpinButtonAlignment.Left,
                IncrementButtonSettings = incSettings,
                DecrementButtonSettings = decrementSettings
            };
            incrementGrid.Children.Add(incrementImage);
            decrementGrid.Children.Add(decrementImage);
            this.Content = upDown;
        }
    }
}
