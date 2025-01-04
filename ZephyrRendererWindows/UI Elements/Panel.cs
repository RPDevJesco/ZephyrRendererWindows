using ZephyrRendererWindows;

namespace ZephyrRenderer.UIElement
{
    public class Panel : UI.UIElement
    {
        public Color BackgroundColor { get; set; } = new Color(128, 128, 128);

        protected override bool IsInteractive => false;

        public new void AddChild(UI.UIElement child)
        {
            base.Children.Add(child);
            child.Parent = this;
        }

        public override void CollectRenderCommands(RenderQueue queue)
        {
            // Only add commands if the panel is visible
            if (!IsVisible) return;
        
            var bounds = GetAbsoluteBounds();
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // Add this panel's background as a render command
            queue.Enqueue(new FillRectCommand(0, bounds, BackgroundColor));
        
            // Let children add their render commands
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
            // Drawing is now handled through CollectRenderCommands
        }
    }
}