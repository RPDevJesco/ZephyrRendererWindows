using ZephyrRenderer.Platform;

namespace ZephyrRendererWindows
{
    public class DirtyRegionTracker
    {
        private readonly List<RECT> dirtyRegions = new();
        private bool fullRedrawNeeded;
    
        public void MarkDirty(RECT region)
        {
            if (fullRedrawNeeded) return;
        
            // Try to merge with existing regions
            bool merged = false;
            for (int i = dirtyRegions.Count - 1; i >= 0; i--)
            {
                if (ShouldMergeRegions(dirtyRegions[i], region))
                {
                    dirtyRegions[i] = MergeRegions(dirtyRegions[i], region);
                    merged = true;
                    break;
                }
            }
        
            if (!merged)
            {
                dirtyRegions.Add(region);
            }
        
            // If we have too many regions, just do a full redraw
            if (dirtyRegions.Count > 10)
            {
                fullRedrawNeeded = true;
                dirtyRegions.Clear();
            }
        }
    
        public IEnumerable<RECT> GetDirtyRegions()
        {
            if (fullRedrawNeeded)
            {
                yield return new RECT(0, 0, double.MaxValue, double.MaxValue);
            }
            else
            {
                foreach (var region in dirtyRegions)
                {
                    yield return region;
                }
            }
        }
    
        public void Clear()
        {
            dirtyRegions.Clear();
            fullRedrawNeeded = false;
        }
    
        private static bool ShouldMergeRegions(RECT a, RECT b)
        {
            // If regions are close enough, merge them
            const int threshold = 32;
            return Math.Abs(a.X - b.X) < threshold && 
                   Math.Abs(a.Y - b.Y) < threshold;
        }
    
        private static RECT MergeRegions(RECT a, RECT b)
        {
            double x = Math.Min(a.X, b.X);
            double y = Math.Min(a.Y, b.Y);
            double right = Math.Max(a.X + a.Width, b.X + b.Width);
            double bottom = Math.Max(a.Y + a.Height, b.Y + b.Height);
        
            return new RECT(x, y, right - x, bottom - y);
        }
    }
}