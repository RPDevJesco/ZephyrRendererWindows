using ZephyrRenderer.Platform;
using ZephyrRenderer.UI;
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

        protected override bool IsInteractive => true;

        public Button(RECT bounds, string text, Color textColor, Color backgroundColor, 
            Color hoverColor, Color pressedColor, Color borderColor, Color borderHoverColor)
        {
            Bounds = bounds;
            Text = text;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            HoverColor = hoverColor;
            PressedColor = pressedColor;
            BorderColor = borderColor;
            BorderHoverColor = borderHoverColor;
        }

        public override void CollectRenderCommands(RenderQueue queue)
        {
            if (!IsVisible) return;

            var bounds = GetAbsoluteBounds();
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // Get current background color based on state
            var backgroundColor = isPressed ? PressedColor : 
                isHovered ? HoverColor : 
                BackgroundColor;

            // Add the background fill command
            queue.Enqueue(new FillRectCommand(1, bounds, backgroundColor));

            // Add the border command
            var borderColor = isHovered ? BorderHoverColor : BorderColor;
            // We need to implement DrawRectCommand for borders
            queue.Enqueue(new DrawRectCommand(2, bounds, borderColor));

            // Add the text command if we have text
            if (!string.IsNullOrEmpty(Text))
            {
                // We need to implement DrawTextCommand for text
                double textWidth = FontRenderer.MeasureText(Text);
                double textX = bounds.X + (bounds.Width - textWidth) / 2;
                double textY = bounds.Y + (bounds.Height - 7) / 2;

                if (isPressed)
                {
                    textX += 1;
                    textY += 1;
                }

                queue.Enqueue(new DrawTextCommand(3, new RECT(textX, textY, textWidth, 7), 
                    Text, IsEnabled ? TextColor : new Color(120, 120, 120)));
            }
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            
        }

        private Color GetCurrentBackgroundColor()
        {
            if (!IsEnabled) return new Color(150, 150, 150);
            return isPressed ? PressedColor : 
                   isHovered ? HoverColor : 
                   BackgroundColor;
        }

        private void DrawButtonBackground(Framebuffer framebuffer, RECT bounds, Color backgroundColor)
        {
            // Fill the button background
            framebuffer.FillRect(bounds.X, bounds.Y, bounds.Width, bounds.Height, backgroundColor);

            // Draw 3D effect edges
            var lightEdgeColor = new Color(220, 220, 220);
            var darkEdgeColor = new Color(100, 100, 100);

            if (isPressed)
            {
                // Pressed state - inset effect
                DrawEdge(framebuffer, bounds, darkEdgeColor, lightEdgeColor);
            }
            else
            {
                // Normal/Hover state - raised effect
                DrawEdge(framebuffer, bounds, lightEdgeColor, darkEdgeColor);
            }
        }

        private void DrawEdge(Framebuffer framebuffer, RECT bounds, Color topLeftColor, Color bottomRightColor)
        {
            // Top and left edges
            framebuffer.DrawLine(bounds.X, bounds.Y, 
                bounds.X + bounds.Width - 1, bounds.Y, topLeftColor);
            framebuffer.DrawLine(bounds.X, bounds.Y, 
                bounds.X, bounds.Y + bounds.Height - 1, topLeftColor);

            // Bottom and right edges
            framebuffer.DrawLine(bounds.X, bounds.Y + bounds.Height - 1,
                bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1, bottomRightColor);
            framebuffer.DrawLine(bounds.X + bounds.Width - 1, bounds.Y,
                bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1, bottomRightColor);
        }

        private void DrawCenteredText(Framebuffer framebuffer, RECT bounds)
        {
            double textWidth = FontRenderer.MeasureText(Text);
            double textX = bounds.X + (bounds.Width - textWidth) / 2;
            double textY = bounds.Y + (bounds.Height - 7) / 2; // 7 is the font height

            // Offset text when pressed for visual feedback
            if (isPressed)
            {
                textX += 1;
                textY += 1;
            }

            FontRenderer.DrawText(
                framebuffer,
                Text,
                (int)textX,
                (int)textY,
                IsEnabled ? TextColor : new Color(120, 120, 120)
            );
        }

        public override bool OnMouseEvent(double x, double y, bool isDown)
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
                    OnMouseLeave?.Invoke();
                }
                return false;
            }

            // Convert to local coordinates and handle the event
            return OnMouseEvent(x - bounds.X, y - bounds.Y, isDown);
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