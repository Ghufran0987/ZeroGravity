using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class SquareAbsoluteLayout : AbsoluteLayout
    {

        public static readonly BindableProperty IsSquareProperty =
              BindableProperty.CreateAttached("IsSquare",
                                              typeof(bool),
                                              typeof(SquareAbsoluteLayout),
                                              defaultValue: true,
                                              defaultBindingMode: BindingMode.OneWay);

        public static bool GetIsSquare(BindableObject view)
        {
            return (bool)view.GetValue(IsSquareProperty);
        }

        public static void SetIsSquare(BindableObject view, bool value)
        {
            view.SetValue(IsSquareProperty, value);
        }

        Dictionary<View, Rectangle> _boundsCache = new Dictionary<View, Rectangle>();
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            foreach (var child in Children)
            {
                var isSquare = GetIsSquare(child);
                if (isSquare)
                {
                    Rectangle bounds;
                    if (!_boundsCache.ContainsKey(child))
                        _boundsCache[child] = bounds = GetLayoutBounds(child);
                    else
                        bounds = _boundsCache[child];

                    var absFlags = GetLayoutFlags(child);

                    var widthIsProportional = (absFlags & AbsoluteLayoutFlags.WidthProportional) != 0;
                    var heightIsProportional = (absFlags & AbsoluteLayoutFlags.HeightProportional) != 0;

                    var childWidth = widthIsProportional ? bounds.Width * width : bounds.Width;
                    var childHeight = heightIsProportional ? bounds.Height * height : bounds.Height;

                    var size = Math.Min(childWidth, childHeight);

                    SetLayoutBounds(
                        child,
                        new Rectangle(
                            bounds.X,
                            bounds.Y,
                            (widthIsProportional ? (size / width) : size),
                            (heightIsProportional ? (size / height) : size)
                         )
                    );
                }
            }

            base.LayoutChildren(x, y, width, height);
        }
    }
}
