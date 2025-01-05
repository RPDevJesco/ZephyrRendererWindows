using ZephyrRenderer.Platform;
using ZephyrRenderer.UI;
using ZephyrRenderer.UI.Layout;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UIElement
{
    public class Panel : UI.UIElement
    {
        public Color BackgroundColor { get; set; } = new Color(128, 128, 128);

        protected override bool IsInteractive => false;

        public Panel()
        {
            Console.WriteLine("Creating new Panel");
        }

        public void SetStackLayout(LayoutDirection direction, double spacing = 0)
        {
            Console.WriteLine($"Setting StackLayout with direction {direction} and spacing {spacing}");
            LayoutManager = new StackLayout(this, direction, spacing);
        }

        public void SetGridLayout()
        {
            Console.WriteLine("Setting GridLayout");
            LayoutManager = new GridLayout(this);
        }

protected override Size MeasureOverride(Size availableSize)
    {
        Console.WriteLine($"Panel MeasureOverride with size: {availableSize.Width}x{availableSize.Height}");

        Size desiredSize;
        
        if (LayoutManager != null)
        {
            desiredSize = LayoutManager.MeasureOverride(availableSize);
        }
        else
        {
            // Use layout properties to determine size
            double width = LayoutProperties.Width.Mode switch
            {
                SizeMode.Fixed => LayoutProperties.Width.Value,
                SizeMode.Star => availableSize.Width * LayoutProperties.Width.Value,
                SizeMode.Percentage => availableSize.Width * (LayoutProperties.Width.Value / 100.0),
                _ => availableSize.Width
            };

            double height = LayoutProperties.Height.Mode switch
            {
                SizeMode.Fixed => LayoutProperties.Height.Value,
                SizeMode.Star => availableSize.Height * LayoutProperties.Height.Value,
                SizeMode.Percentage => availableSize.Height * (LayoutProperties.Height.Value / 100.0),
                _ => availableSize.Height
            };

            desiredSize = new Size(width, height);
        }

        Console.WriteLine($"Panel desired size: {desiredSize.Width}x{desiredSize.Height}");
        return desiredSize;
    }

    protected override void ArrangeOverride(RECT finalRect)
    {
        Console.WriteLine($"Panel ArrangeOverride with rect: {finalRect.X},{finalRect.Y} {finalRect.Width}x{finalRect.Height}");

        // Ensure we have valid dimensions
        finalRect.Width = Math.Max(1, finalRect.Width);
        finalRect.Height = Math.Max(1, finalRect.Height);

        // Store the bounds
        Bounds = new RECT(finalRect.X, finalRect.Y, finalRect.Width, finalRect.Height);

        if (LayoutManager != null)
        {
            LayoutManager.ArrangeOverride(finalRect);
        }
        else
        {
            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    // Create child rect based on alignment
                    var childRect = GetAlignedRect(child, finalRect);
                    child.Arrange(childRect);
                }
            }
        }
    }

    private RECT GetAlignedRect(UI.UIElement child, RECT parentRect)
    {
        var childSize = child.DesiredSize;
        var rect = parentRect;

        // Apply horizontal alignment
        switch (child.LayoutProperties.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
                rect.Width = childSize.Width;
                break;
            case HorizontalAlignment.Center:
                rect.X += (parentRect.Width - childSize.Width) / 2;
                rect.Width = childSize.Width;
                break;
            case HorizontalAlignment.Right:
                rect.X = parentRect.X + parentRect.Width - childSize.Width;
                rect.Width = childSize.Width;
                break;
            case HorizontalAlignment.Stretch:
                // Use full width
                break;
        }

        // Apply vertical alignment
        switch (child.LayoutProperties.VerticalAlignment)
        {
            case VerticalAlignment.Top:
                rect.Height = childSize.Height;
                break;
            case VerticalAlignment.Center:
                rect.Y += (parentRect.Height - childSize.Height) / 2;
                rect.Height = childSize.Height;
                break;
            case VerticalAlignment.Bottom:
                rect.Y = parentRect.Y + parentRect.Height - childSize.Height;
                rect.Height = childSize.Height;
                break;
            case VerticalAlignment.Stretch:
                // Use full height
                break;
        }

        return rect;
    }

    public new void AddChild(UI.UIElement child)
    {
        Console.WriteLine($"Panel adding child at {Children.Count}");
        Children.Add(child);
        child.Parent = this;
        InvalidateLayout();
    }

        public override void CollectRenderCommands(RenderQueue queue)
        {
            if (!IsVisible) return;

            var bounds = GetAbsoluteBounds();
            Console.WriteLine($"Panel bounds: {bounds.X},{bounds.Y} {bounds.Width}x{bounds.Height}");
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            queue.Enqueue(new FillRectCommand(-1, bounds, BackgroundColor));

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    child.CollectRenderCommands(queue);
                }
            }
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            // Drawing is handled through CollectRenderCommands
        }
    }
}