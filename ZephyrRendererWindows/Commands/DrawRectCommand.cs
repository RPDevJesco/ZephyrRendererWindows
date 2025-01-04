using ZephyrRenderer;
using ZephyrRenderer.Platform;

namespace ZephyrRendererWindows
{
    public class DrawRectCommand : RenderCommand
    {
        private readonly Color color;

        public DrawRectCommand(int zIndex, RECT bounds, Color color) 
            : base(zIndex, bounds)
        {
            this.color = color;
        }

        public override void Execute(Framebuffer target)
        {
            target.DrawRect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, color);
        }
    }
}