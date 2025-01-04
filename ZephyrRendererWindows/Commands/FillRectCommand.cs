using System;
using ZephyrRenderer;
using ZephyrRenderer.Platform;

namespace ZephyrRendererWindows
{
    public class FillRectCommand : RenderCommand
    {
        private readonly Color color;
    
        public FillRectCommand(int zIndex, RECT bounds, Color color) 
            : base(zIndex, bounds)
        {
            this.color = color;
        }
    
        public override void Execute(Framebuffer target)
        {
            target.FillRect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, color);
        }
    
        private void ExecuteFast(Framebuffer target)
        {
            byte[] buffer = target.GetBuffer();
            Span<byte> span = buffer;
            
            int stride = (int)target.Stride;
            int startY = (int)Bounds.Y;
            int endY = (int)(Bounds.Y + Bounds.Height);
            int startX = (int)Bounds.X * 4;
            int width = (int)Bounds.Width * 4;
            
            // Pre-compute the 4-byte color pattern
            byte[] colorPattern = new byte[] { color.B, color.G, color.R, color.A };
            
            for (int y = startY; y < endY; y++)
            {
                int rowOffset = y * stride + startX;
                for (int x = 0; x < width; x += 4)
                {
                    span.Slice(rowOffset + x, 4).CopyTo(colorPattern);
                }
            }
        }
    
        private void ExecuteNormal(Framebuffer target)
        {
            target.FillRect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, color);
        }
    }
}