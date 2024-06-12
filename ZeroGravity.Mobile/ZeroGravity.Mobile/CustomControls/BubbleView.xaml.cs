using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BubbleView : ContentView
    {
        #region Button
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BubbleView));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty ButtonContentProperty = BindableProperty.Create(nameof(ButtonContent), typeof(View), typeof(BubbleView));

        public View ButtonContent
        {
            get => (View)GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(BubbleView));

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        public static readonly BindableProperty ButtonRotationAngleProperty = BindableProperty.Create(nameof(ButtonRotationAngle), typeof(double), typeof(BubbleView));

        public double ButtonRotationAngle
        {
            get => (double)GetValue(ButtonRotationAngleProperty);
            set => SetValue(ButtonRotationAngleProperty, value);
        }

        public static readonly BindableProperty ButtonRadiusXProperty = BindableProperty.Create(nameof(ButtonRadiusX), typeof(double), typeof(BubbleView));

        public double ButtonRadiusX
        {
            get => (double)GetValue(ButtonRadiusXProperty);
            set => SetValue(ButtonRadiusXProperty, value);
        }

        public static readonly BindableProperty ButtonRadiusYProperty = BindableProperty.Create(nameof(ButtonRadiusY), typeof(double), typeof(BubbleView));

        public double ButtonRadiusY
        {
            get => (double)GetValue(ButtonRadiusYProperty);
            set => SetValue(ButtonRadiusYProperty, value);
        }
        #endregion

        #region Badge
        public static readonly BindableProperty BadgeContentProperty = BindableProperty.Create(nameof(BadgeContent), typeof(View), typeof(BubbleView));

        public View BadgeContent
        {
            get => (View)GetValue(BadgeContentProperty);
            set => SetValue(BadgeContentProperty, value);
        }

        public static readonly BindableProperty BadgeColorProperty = BindableProperty.Create(nameof(BadgeColor), typeof(Color), typeof(BubbleView));

        public Color BadgeColor
        {
            get => (Color)GetValue(BadgeColorProperty);
            set => SetValue(BadgeColorProperty, value);
        }

        public static readonly BindableProperty ShowBadgeProperty = BindableProperty.Create(nameof(ShowBadge), typeof(bool), typeof(BubbleView));

        public bool ShowBadge
        {
            get => (bool)GetValue(ShowBadgeProperty);
            set => SetValue(ShowBadgeProperty, value);
        }

        public static readonly BindableProperty BadgeBoundsProperty = BindableProperty.Create(nameof(BadgeBounds), typeof(Rectangle), typeof(BubbleView));

        public Rectangle BadgeBounds
        {
            get => (Rectangle)GetValue(BadgeBoundsProperty);
            set => SetValue(BadgeBoundsProperty, value);
        }

        public static readonly BindableProperty BadgePositionXProperty = BindableProperty.Create(nameof(BadgePositionX), typeof(double), typeof(BubbleView));

        public double BadgePositionX
        {
            get => (double)GetValue(BadgePositionXProperty);
            set => SetValue(BadgePositionXProperty, value);
        }

        public static readonly BindableProperty BadgePositionYProperty = BindableProperty.Create(nameof(BadgePositionY), typeof(double), typeof(BubbleView));

        public double BadgePositionY
        {
            get => (double)GetValue(BadgePositionYProperty);
            set => SetValue(BadgePositionYProperty, value);
        }

        public static readonly BindableProperty BadgeWidthProperty = BindableProperty.Create(nameof(BadgeWidth), typeof(double), typeof(BubbleView));

        public double BadgeWidth
        {
            get => (double)GetValue(BadgeWidthProperty);
            set => SetValue(BadgeWidthProperty, value);
        }

        public static readonly BindableProperty BadgeHeightProperty = BindableProperty.Create(nameof(BadgeHeight), typeof(double), typeof(BubbleView));

        public double BadgeHeight
        {
            get => (double)GetValue(BadgeHeightProperty);
            set => SetValue(BadgeHeightProperty, value);
        }

        public static readonly BindableProperty BadgeCornerRadiusProperty = BindableProperty.Create(nameof(BadgeCornerRadius), typeof(double), typeof(BubbleView));

        public double BadgeCornerRadius
        {
            get => (double)GetValue(BadgeCornerRadiusProperty);
            set => SetValue(BadgeCornerRadiusProperty, value);
        }
        #endregion


        public BubbleView()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //AbsoluteLayout.SetLayoutBounds(Button, new Rectangle(0.5, 0.5, width, height));
            //ButtonEllipse.Center = new Point(width/2, height/2);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(BadgePositionX) ||
                propertyName == nameof(BadgePositionY) ||
                propertyName == nameof(BadgeWidth) ||
                propertyName == nameof(BadgeHeight))
            {
                AbsoluteLayout.SetLayoutBounds(BadgeFrame, new Rectangle(BadgePositionX, BadgePositionY, BadgeWidth, BadgeHeight));
                BadgeFrame.CornerRadius = (float)(BadgeWidth / 2);
            }
        }

        private void OnButtonSizeChanged(object sender, EventArgs e)
        {
            var width = Root.Width;
            var height = Root.Height;
            AbsoluteLayout.SetLayoutBounds(Button, new Rectangle(0.5, 0.5, width, height));
            ButtonEllipse.Center = new Point(width / 2, height / 2);
        }
    }
}