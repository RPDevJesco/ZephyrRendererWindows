using ZephyrRenderer.Platform;
using ZephyrRenderer.UI.Layout;

namespace ZephyrRenderer.UI
{
    public class StackLayout : LayoutManagerBase
    {
        public LayoutDirection Direction { get; set; }
        public double Spacing { get; set; }

        private readonly List<Size> childSizes = new();

        public StackLayout(ILayoutable owner, LayoutDirection direction = LayoutDirection.Vertical, double spacing = 0)
            : base(owner)
        {
            Direction = direction;
            Spacing = spacing;
        }

        public override Size MeasureOverride(Size availableSize)
        {
            var owner = Owner as UIElement ?? throw new InvalidOperationException("Owner must be a UIElement");
            childSizes.Clear();

            double totalHeight = 0;
            double maxWidth = availableSize.Width;

            foreach (var child in owner.Children)
            {
                if (!child.IsVisible)
                {
                    childSizes.Add(Size.Zero);
                    continue;
                }

                var childSize = child.Measure(new Size(availableSize.Width, availableSize.Height - totalHeight));
                childSizes.Add(childSize);

                totalHeight += childSize.Height + Spacing;
            }

            // Remove last spacing
            if (totalHeight > 0)
                totalHeight -= Spacing;

            return new Size(maxWidth, totalHeight);
        }

        public override void ArrangeOverride(RECT finalRect)
        {
            var owner = Owner as UIElement ?? throw new InvalidOperationException("Owner must be a UIElement");

            if (childSizes.Count != owner.Children.Count)
            {
                MeasureOverride(new Size(finalRect.Width, finalRect.Height));
            }

            double currentY = finalRect.Y;
            for (int i = 0; i < owner.Children.Count; i++)
            {
                var child = owner.Children[i];
                if (!child.IsVisible) continue;

                var size = childSizes[i];
                child.Arrange(new RECT(
                    finalRect.X,
                    currentY,
                    finalRect.Width,
                    size.Height
                ));

                currentY += size.Height + Spacing;
            }
        }
    }
}