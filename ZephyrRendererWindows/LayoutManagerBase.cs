using ZephyrRenderer.Platform;

namespace ZephyrRenderer.UI.Layout
{
    /// <summary>
    /// Provides the base functionality for implementing layout managers.
    /// </summary>
    public abstract class LayoutManagerBase : ILayoutManager
    {
        protected readonly ILayoutable Owner;
        private bool needsLayout = true;

        public LayoutManagerBase(ILayoutable owner)
        {
            Owner = owner;
        }

        public bool NeedsLayout
        {
            get => needsLayout;
            protected set => needsLayout = value;
        }

        public void InvalidateLayout()
        {
            needsLayout = true;
            Owner.InvalidateLayout();
        }

        public abstract Size MeasureOverride(Size availableSize);
        public abstract void ArrangeOverride(RECT finalRect);

        protected Size ApplySizingMode(LayoutSize size, double availableSize, double contentSize)
        {
            return size.Mode switch
            {
                SizeMode.Fixed => new Size(size.Value, size.Value),
                SizeMode.Auto => new Size(contentSize, contentSize),
                SizeMode.Star => new Size(availableSize * size.Value, availableSize * size.Value),
                SizeMode.Percentage => new Size(availableSize * (size.Value / 100), availableSize * (size.Value / 100)),
                _ => Size.Zero
            };
        }

        protected RECT ApplyAlignment(RECT elementBounds, RECT containerBounds,
            HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var result = elementBounds;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    result.X = containerBounds.X + (containerBounds.Width - elementBounds.Width) / 2;
                    break;
                case HorizontalAlignment.Right:
                    result.X = containerBounds.X + containerBounds.Width - elementBounds.Width;
                    break;
                case HorizontalAlignment.Stretch:
                    result.X = containerBounds.X;
                    result.Width = containerBounds.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Center:
                    result.Y = containerBounds.Y + (containerBounds.Height - elementBounds.Height) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    result.Y = containerBounds.Y + containerBounds.Height - elementBounds.Height;
                    break;
                case VerticalAlignment.Stretch:
                    result.Y = containerBounds.Y;
                    result.Height = containerBounds.Height;
                    break;
            }

            return result;
        }
    }
}