using ZephyrRenderer;
using ZephyrRenderer.Platform;
using ZephyrRenderer.UI;

namespace ZephyrRendererWindows
{
    public class DrawTextCommand : RenderCommand
    {
        private readonly string text;
        private readonly Color color;

        public DrawTextCommand(int zIndex, RECT bounds, string text, Color color) 
            : base(zIndex, bounds)
        {
            this.text = text;
            this.color = color;
        }

        public override void Execute(Framebuffer target)
        {
            FontRenderer.DrawText(target, text, (int)Bounds.X, (int)Bounds.Y, color);
        }
    }
}