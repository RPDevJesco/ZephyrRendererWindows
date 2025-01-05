using ZephyrRenderer.Platform;
using ZephyrRenderer.UI.Layout;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UI
{
    public abstract class UIElement : ILayoutable
    {
        private RECT bounds;
        private Size desiredSize;
        private ILayoutManager? layoutManager;
        private bool layoutInvalid = true;
        private readonly LayoutProperties layoutProperties = new();
        private RECT? cachedAbsoluteBounds;
        private bool boundsDirty = true;

        public RECT Bounds
        {
            get => bounds;
            set
            {
                bounds = value;
                InvalidateLayout();
                InvalidateBounds();
            }
        }
        // Grid attached properties
        private int gridRow;
        private int gridColumn;
        private int gridRowSpan = 1;
        private int gridColumnSpan = 1;

        public  int GridRow
        {
            get => gridRow;
            set
            {
                if (gridRow != value)
                {
                    gridRow = value;
                    InvalidateLayout();
                }
            }
        }

        public  int GridColumn
        {
            get => gridColumn;
            set
            {
                if (gridColumn != value)
                {
                    gridColumn = value;
                    InvalidateLayout();
                }
            }
        }

        public  int GridRowSpan
        {
            get => gridRowSpan;
            set
            {
                if (gridRowSpan != value)
                {
                    gridRowSpan = Math.Max(1, value);
                    InvalidateLayout();
                }
            }
        }

        public  int GridColumnSpan
        {
            get => gridColumnSpan;
            set
            {
                if (gridColumnSpan != value)
                {
                    gridColumnSpan = Math.Max(1, value);
                    InvalidateLayout();
                }
            }
        }

        public Size DesiredSize => desiredSize;
        public LayoutProperties LayoutProperties => layoutProperties;
        public bool IsVisible { get; set; } = true;
        public UIElement? Parent { get; set; }
        public List<UIElement> Children { get; } = new();
        protected virtual bool IsInteractive => false;

        public ILayoutManager? LayoutManager
        {
            get => layoutManager;
            set
            {
                layoutManager = value;
                InvalidateLayout();
            }
        }

        #region Layout System Implementation

        protected virtual Size MeasureOverride(Size availableSize)
        {
            // Ensure we return valid dimensions
            return new Size(
                Math.Max(1, availableSize.Width),
                Math.Max(1, availableSize.Height)
            );
        }

        public Size Measure(Size availableSize)
        {
            if (!IsVisible)
            {
                desiredSize = Size.Zero;
                return desiredSize;
            }

            // Apply layout properties
            double width = LayoutProperties.Width.Mode switch
            {
                SizeMode.Fixed => LayoutProperties.Width.Value,
                SizeMode.Auto => availableSize.Width,
                SizeMode.Star => availableSize.Width * LayoutProperties.Width.Value,
                SizeMode.Percentage => availableSize.Width * (LayoutProperties.Width.Value / 100),
                _ => availableSize.Width
            };

            double height = LayoutProperties.Height.Mode switch
            {
                SizeMode.Fixed => LayoutProperties.Height.Value,
                SizeMode.Auto => availableSize.Height,
                SizeMode.Star => availableSize.Height * LayoutProperties.Height.Value,
                SizeMode.Percentage => availableSize.Height * (LayoutProperties.Height.Value / 100),
                _ => availableSize.Height
            };

            // Apply margin
            width = Math.Max(0, width - LayoutProperties.Margin.Horizontal);
            height = Math.Max(0, height - LayoutProperties.Margin.Vertical);

            // Measure with constrained size
            desiredSize = MeasureOverride(new Size(width, height));

            // Add margin back to desired size
            desiredSize = new Size(
                desiredSize.Width + LayoutProperties.Margin.Horizontal,
                desiredSize.Height + LayoutProperties.Margin.Vertical
            );

            return desiredSize;
        }

        public void Arrange(RECT finalRect)
        {
            if (!IsVisible)
            {
                bounds = new RECT(0, 0, 0, 0);
                return;
            }

            // Apply margin
            finalRect = new RECT(
                finalRect.X + LayoutProperties.Margin.Left,
                finalRect.Y + LayoutProperties.Margin.Top,
                Math.Max(0, finalRect.Width - LayoutProperties.Margin.Horizontal),
                Math.Max(0, finalRect.Height - LayoutProperties.Margin.Vertical)
            );

            // Apply alignment
            if (LayoutProperties.HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                double width = Math.Min(desiredSize.Width - LayoutProperties.Margin.Horizontal, finalRect.Width);
                switch (LayoutProperties.HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        finalRect.X += (finalRect.Width - width) / 2;
                        break;
                    case HorizontalAlignment.Right:
                        finalRect.X += finalRect.Width - width;
                        break;
                }
                finalRect.Width = width;
            }

            if (LayoutProperties.VerticalAlignment != VerticalAlignment.Stretch)
            {
                double height = Math.Min(desiredSize.Height - LayoutProperties.Margin.Vertical, finalRect.Height);
                switch (LayoutProperties.VerticalAlignment)
                {
                    case VerticalAlignment.Center:
                        finalRect.Y += (finalRect.Height - height) / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        finalRect.Y += finalRect.Height - height;
                        break;
                }
                finalRect.Height = height;
            }

            ArrangeOverride(finalRect);
            bounds = finalRect;
            InvalidateBounds();
        }

        protected virtual void ArrangeOverride(RECT finalRect)
        {
            // Default implementation just sets the bounds
            bounds = finalRect;
        }

        public virtual void InvalidateLayout()
        {
            if (!layoutInvalid)
            {
                layoutInvalid = true;
                Parent?.InvalidateLayout();
            }
        }

        #endregion

        #region Bounds Management

        protected void InvalidateBounds()
        {
            if (!boundsDirty)
            {
                boundsDirty = true;
                cachedAbsoluteBounds = null;
            
                // Invalidate all children
                foreach (var child in Children)
                {
                    child.InvalidateBounds();
                }
            }
        }
    
        public RECT GetAbsoluteBounds()
        {
            if (boundsDirty || cachedAbsoluteBounds == null)
            {
                cachedAbsoluteBounds = CalculateAbsoluteBounds();
                boundsDirty = false;
            }
            return cachedAbsoluteBounds.Value;
        }

        private RECT CalculateAbsoluteBounds()
        {
            var result = bounds;
            var current = Parent;
            
            while (current != null)
            {
                result.X += current.bounds.X;
                result.Y += current.bounds.Y;
                current = current.Parent;
            }
            
            return result;
        }

        #endregion

        #region Rendering

        public virtual void Draw(Framebuffer framebuffer)
        {
            if (!IsVisible) return;
            
            OnDraw(framebuffer);
            
            foreach (var child in Children)
            {
                child.Draw(framebuffer);
            }
        }

        protected abstract void OnDraw(Framebuffer framebuffer);

        public virtual void CollectRenderCommands(RenderQueue queue)
        {
            if (!IsVisible) return;

            // Base implementation does nothing
            // Derived classes should override this to add their render commands
        }

        #endregion

        #region Event Handling

        public virtual bool HandleMouseEvent(double x, double y, bool isDown)
        {
            if (!IsVisible) return false;

            // Check children first (in reverse order for proper z-order)
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];
                // Convert coordinates to child's local space
                var childBounds = child.GetAbsoluteBounds();
                double childX = x - childBounds.X;
                double childY = y - childBounds.Y;
        
                if (childX >= 0 && childX < childBounds.Width &&
                    childY >= 0 && childY < childBounds.Height)
                {
                    if (child.HandleMouseEvent(x, y, isDown))
                        return true;
                }
            }

            // Then check this element
            var bounds = GetAbsoluteBounds();
            if (x >= bounds.X && x < bounds.X + bounds.Width &&
                y >= bounds.Y && y < bounds.Y + bounds.Height)
            {
                return OnMouseEvent(x - bounds.X, y - bounds.Y, isDown);
            }

            return false;
        }

        protected virtual bool OnMouseEvent(double x, double y, bool isDown) => false;

        #endregion
    }
}