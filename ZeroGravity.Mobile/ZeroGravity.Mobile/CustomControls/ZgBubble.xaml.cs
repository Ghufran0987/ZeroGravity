using System;
using System.Windows.Input;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.CustomControls
{
    public enum BubbleBadgePosition
    {
        TopRight = 325,
        BottomRight = 45,
        BottomLeft = 135,
        TopLeft = 225
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgBubble : ContentView
    {
        private readonly SfButton _button;
        private readonly PancakeView _bubblePancake;
        private readonly PancakeView _badgePancake;

        public ZgBubble()
        {
            InitializeComponent();

            _button = GetControl<SfButton>("Button");
            _bubblePancake = GetControl<PancakeView>("Bubble");
            _badgePancake = GetControl<PancakeView>("Badge");
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ZgBubble));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty); 
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty BubbleBackgroundColorProperty = BindableProperty.Create(nameof(BubbleBackgroundColor), typeof(Color), typeof(ZgBubble));

        public Color BubbleBackgroundColor
        {
            get => (Color)GetValue(BubbleBackgroundColorProperty); 
            set => SetValue(BubbleBackgroundColorProperty, value);
        }

        public static readonly BindableProperty BlurRadiusProperty = BindableProperty.Create(nameof(BlurRadius), typeof(float), typeof(ZgBubble));

        public float BlurRadius
        {
            get => (float)GetValue(BlurRadiusProperty); 
            set => SetValue(BlurRadiusProperty, value);
        }

        public static readonly BindableProperty ShowBadgeProperty = BindableProperty.Create(nameof(ShowBadge), typeof(bool), typeof(ZgBubble));

        public bool ShowBadge
        {
            get => (bool)GetValue(ShowBadgeProperty); 
            set => SetValue(ShowBadgeProperty, value);
        }

        public static readonly BindableProperty BadgeSizeProperty = BindableProperty.Create(nameof(BadgeSize), typeof(double), typeof(ZgBubble));

        public double BadgeSize
        {
            get => (double)GetValue(BadgeSizeProperty); 
            set => SetValue(BadgeSizeProperty, value);
        }

        public static readonly BindableProperty BadgeContentProperty = BindableProperty.Create(nameof(BadgeContent), typeof(View), typeof(ZgBubble));

        public View BadgeContent
        {
            get => (View)GetValue(BadgeContentProperty); 
            set => SetValue(BadgeContentProperty, value);
        }

        public static readonly BindableProperty BadgePositionProperty = BindableProperty.Create(nameof(BadgePosition), typeof(BubbleBadgePosition), typeof(ZgBubble));

        public BubbleBadgePosition BadgePosition
        {
            get => (BubbleBadgePosition)GetValue(BadgePositionProperty); 
            set => SetValue(BadgePositionProperty, value);
        }

        public static readonly BindableProperty BadgeBackgroundColorProperty = BindableProperty.Create(nameof(BadgeBackgroundColor), typeof(Color), typeof(ZgBubble));

        public Color BadgeBackgroundColor
        {
            get => (Color)GetValue(BadgeBackgroundColorProperty); 
            set => SetValue(BadgeBackgroundColorProperty, value);
        }

        public static readonly BindableProperty BadgeCornerRadiusProperty = BindableProperty.Create(nameof(BadgeCornerRadius), typeof(double), typeof(ZgBubble));

        public double BadgeCornerRadius
        {
            get => (double)GetValue(BadgeCornerRadiusProperty);
            set => SetValue(BadgeCornerRadiusProperty, value);
        }

        public static readonly BindableProperty BubbleCornerRadiusProperty = BindableProperty.Create(nameof(BadgeCornerRadius), typeof(double), typeof(ZgBubble));

        public double BubbleCornerRadius
        {
            get => (double)GetValue(BubbleCornerRadiusProperty);
            set => SetValue(BubbleCornerRadiusProperty, value);
        }


        public static readonly BindableProperty BadgeBorderColorProperty = BindableProperty.Create(nameof(BadgeBorderColor), typeof(Color), typeof(ZgBubble));

        public Color BadgeBorderColor
        {
            get => (Color)GetValue(BadgeBorderColorProperty);
            set => SetValue(BadgeBorderColorProperty, value);
        }

        public static readonly BindableProperty BadgeBorderThicknessProperty = BindableProperty.Create(nameof(BadgeBorderThickness), typeof(int), typeof(ZgBubble));

        public int BadgeBorderThickness
        {
            get => (int)GetValue(BadgeBorderThicknessProperty);
            set => SetValue(BadgeBorderThicknessProperty, value);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (_bubblePancake != null)
            {
                _bubblePancake.CornerRadius = _bubblePancake.Width / 2;
            }

            if (!ShowBadge) return;

            if (_badgePancake != null)
            {
                CalculateRelativeBadgePosition(width, out var x, out var y);
                AbsoluteLayout.SetLayoutBounds(_badgePancake, new Rectangle(x, y, BadgeSize, BadgeSize));

                _badgePancake.CornerRadius = _badgePancake.Width / 2;
            }
        }

        private void CalculateRelativeBadgePosition(double bubbleSize, out double x, out double y)
        {
            var halfWidth = BadgeSize / bubbleSize / 2;
            var cornerSquare = bubbleSize / 2d * (Math.Sqrt(2) - 1);
            var cornerSquareWidth = cornerSquare / Math.Sqrt(2);
            var offset = cornerSquareWidth / bubbleSize - halfWidth;

            var radian = ConvertToRadians((int)BadgePosition);

            x = 1 * Math.Cos(radian) + 0.5;
            y = 1 * Math.Sin(radian) + 0.5;
            
            if (x > 1) x = 1;
            if (y > 1) y = 1;

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            if (BadgePosition == BubbleBadgePosition.TopRight)
            {
                x -= offset;
                y += offset;
            }
            else if (BadgePosition == BubbleBadgePosition.BottomRight)
            {
                x -= offset;
                y -= offset;
            }
            else if (BadgePosition == BubbleBadgePosition.BottomLeft)
            {
                x += offset;
                y -= offset;
            }
            else
            {
                x += offset;
                y += offset;
            }

        }

        private double ConvertToRadians(double angle)
        {
            var radians = (Math.PI / 180) * angle;
            return radians;
        }

        private T GetControl<T>(string name)
        {
            var child = GetTemplateChild(name);
            return (T) child;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

 

            if (BadgeContent != null)
            {
                SetInheritedBindingContext(BadgeContent, BindingContext);
            }
        }
    }
}