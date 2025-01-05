using ZephyrRenderer.Platform;
using ZephyrRenderer.UI;
using ZephyrRenderer.UI.Layout;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UIElement
{
    public class Button : UI.UIElement
    {
        public string Text { get; set; }
        public Color TextColor { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Color HoverColor { get; private set; }
        public Color PressedColor { get; private set; }
        public Color BorderColor { get; private set; }
        public Color BorderHoverColor { get; private set; }
        public bool IsEnabled { get; private set; } = true;

        public event Action? OnClick;
        public event Action? OnMouseEnter;
        public event Action? OnMouseLeave;

        private bool isHovered;
        private bool isPressed;
        private bool wasPressed;
        private RECT localBounds;

        protected override bool IsInteractive => true;

        public Button(RECT bounds, string text, Color textColor, Color backgroundColor,
            Color hoverColor, Color pressedColor, Color borderColor, Color borderHoverColor)
        {
            Console.WriteLine(
                $"Creating button '{text}' with bounds {bounds.X},{bounds.Y} {bounds.Width}x{bounds.Height}");
            Bounds = bounds;
            Text = text;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            HoverColor = hoverColor;
            PressedColor = pressedColor;
            BorderColor = borderColor;
            BorderHoverColor = borderHoverColor;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Return minimum size for the button
            return new Size(
                Math.Min(availableSize.Width, Math.Max(35, LayoutProperties.Width.Value)),
                Math.Min(availableSize.Height, Math.Max(35, LayoutProperties.Height.Value))
            );
        }

        protected override void ArrangeOverride(RECT finalRect)
        {
            Console.WriteLine(
                $"Button '{Text}' arranging with rect {finalRect.X},{finalRect.Y} {finalRect.Width}x{finalRect.Height}");
            localBounds = finalRect;
            base.ArrangeOverride(finalRect);
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            // Handled by other systems.
        }

        public override void CollectRenderCommands(RenderQueue queue)
        {
            if (!IsVisible) return;

            var bounds = GetAbsoluteBounds();
            Console.WriteLine(
                $"Button '{Text}' collecting render commands at {bounds.X},{bounds.Y} {bounds.Width}x{bounds.Height}");
            Console.WriteLine(
                $"Button '{Text}' local bounds: {localBounds.X},{localBounds.Y} {localBounds.Width}x{localBounds.Height}");

            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // Get current background color based on state
            var backgroundColor = isPressed ? PressedColor :
                isHovered ? HoverColor :
                BackgroundColor;

            // Add the background fill command
            queue.Enqueue(new FillRectCommand(100, bounds, backgroundColor));

            // Add the border command
            var borderColor = isHovered ? BorderHoverColor : BorderColor;
            queue.Enqueue(new DrawRectCommand(101, bounds, borderColor));

            // Add the text command if we have text
            if (!string.IsNullOrEmpty(Text))
            {
                double textWidth = FontRenderer.MeasureText(Text);
                double textX = bounds.X + (bounds.Width - textWidth) / 2;
                double textY = bounds.Y + (bounds.Height - 7) / 2;

                if (isPressed)
                {
                    textX += 1;
                    textY += 1;
                }

                queue.Enqueue(new DrawTextCommand(102, new RECT(textX, textY, textWidth, 7),
                    Text, IsEnabled ? TextColor : new Color(120, 120, 120)));
            }

            // Let children add their render commands
            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    child.CollectRenderCommands(queue);
                }
            }
        }

        public override bool HandleMouseEvent(double x, double y, bool isDown)
        {
            if (!IsEnabled) return false;

            // Do hit testing in absolute coordinates
            var bounds = GetAbsoluteBounds();
            var isInBounds = x >= bounds.X &&
                             x < bounds.X + bounds.Width &&
                             y >= bounds.Y &&
                             y < bounds.Y + bounds.Height;

            if (!isInBounds)
            {
                // Handle mouse leave if we were previously hovered
                if (isHovered)
                {
                    isHovered = false;
                    isPressed = false;
                    wasPressed = false;
                    OnMouseLeave?.Invoke();
                }

                return false;
            }

            // Convert to local coordinates and handle the event
            return OnMouseEvent(x - bounds.X, y - bounds.Y, isDown);
        }

        protected override bool OnMouseEvent(double x, double y, bool isDown)
        {
            // Handle hover state
            if (!isHovered)
            {
                isHovered = true;
                OnMouseEnter?.Invoke();
            }

            // Handle click state
            if (isDown)
            {
                isPressed = true;
                wasPressed = true;
            }
            else if (wasPressed && isPressed)
            {
                OnClick?.Invoke();
                isPressed = false;
            }

            wasPressed = isDown;
            return true; // Button always handles its events
        }

        public void SetEnabled(bool enabled)
        {
            if (!enabled)
            {
                isHovered = false;
                isPressed = false;
            }

            IsEnabled = enabled;
        }
    }
}