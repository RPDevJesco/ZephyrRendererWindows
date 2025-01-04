using ZephyrRenderer.Platform;

namespace ZephyrRendererWindows
{
    public class ClipStack
    {
        private readonly Stack<RECT> clipRegions = new();
    
        public void PushClip(RECT region)
        {
            if (clipRegions.Count > 0)
            {
                // Intersect with current clip region
                region = IntersectRects(region, clipRegions.Peek());
            }
            clipRegions.Push(region);
        }
    
        public void PopClip()
        {
            if (clipRegions.Count > 0)
            {
                clipRegions.Pop();
            }
        }
    
        public bool IsVisible(RECT rect)
        {
            if (clipRegions.Count == 0) return true;
        
            var currentClip = clipRegions.Peek();
            return rect.Intersects(currentClip);
        }
    
        private static RECT IntersectRects(RECT a, RECT b)
        {
            double x = Math.Max(a.X, b.X);
            double y = Math.Max(a.Y, b.Y);
            double right = Math.Min(a.X + a.Width, b.X + b.Width);
            double bottom = Math.Min(a.Y + a.Height, b.Y + b.Height);
        
            return new RECT(x, y, Math.Max(0, right - x), Math.Max(0, bottom - y));
        }
    }
}