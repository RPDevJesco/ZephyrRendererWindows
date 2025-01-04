namespace ZephyrRenderer.UIElement
{
    public class Panel : UI.UIElement
    {
        public Color BackgroundColor { get; set; } = new Color(128, 128, 128);

        protected override bool IsInteractive => false; // Panels shouldn't block events

        public override void Draw(Framebuffer framebuffer)
        {
            if (!IsVisible) return;

            // First draw this panel's background
            OnDraw(framebuffer);

            // Then draw all children on top
            foreach (var child in Children)
            {
                child.Draw(framebuffer);
            }
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            var bounds = GetAbsoluteBounds();
            
            // Draw panel background using absolute coordinates
            framebuffer.FillRect(
                bounds.X, 
                bounds.Y, 
                bounds.Width, 
                bounds.Height, 
                BackgroundColor
            );
        }

        public override bool HandleMouseEvent(double x, double y, bool isDown)
        {
            // Always return false to allow events to pass through to children
            return false;
        }
    }
}