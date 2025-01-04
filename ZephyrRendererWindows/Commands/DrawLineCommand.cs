using ZephyrRenderer;
using ZephyrRenderer.Platform;
using Color = ZephyrRenderer.Color;

namespace ZephyrRendererWindows
{
    public class DrawLineCommand : RenderCommand
    {
        private readonly POINT startPoint;
        private readonly POINT endPoint;
        private readonly Color color;

        public DrawLineCommand(int zIndex, POINT startPoint, POINT endPoint, Color color)
            : base(zIndex, new RECT(
                Math.Min(startPoint.X, endPoint.X),
                Math.Min(startPoint.Y, endPoint.Y),
                Math.Abs(endPoint.X - startPoint.X),
                Math.Abs(endPoint.Y - startPoint.Y)))
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.color = color;
        }

        public override void Execute(Framebuffer target)
        {
            target.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, color);
        }
    }
}