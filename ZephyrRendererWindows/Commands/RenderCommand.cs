using ZephyrRenderer;
using ZephyrRenderer.Platform;

namespace ZephyrRendererWindows
{
    public abstract class RenderCommand
    {
        public int ZIndex { get; }
        public RECT Bounds { get; }
    
        protected RenderCommand(int zIndex, RECT bounds)
        {
            ZIndex = zIndex;
            Bounds = bounds;
        }
    
        public abstract void Execute(Framebuffer target);
    }
}