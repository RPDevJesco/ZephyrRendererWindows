using ZephyrRenderer.Platform;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UI
{
    public abstract class UIElement
    {
        private RECT? cachedAbsoluteBounds;
        private bool boundsDirty = true;
        private RECT localBounds;
        
        public RECT Bounds 
        { 
            get => localBounds;
            set
            {
                localBounds = value;
                InvalidateBounds();
            }
        }

        public bool IsVisible { get; set; } = true;
        public UIElement? Parent { get; set; }
        public List<UIElement> Children { get; } = new List<UIElement>();
        protected virtual bool IsInteractive => false;
    
        protected void InvalidateBounds()
        {
            if (!boundsDirty)
            {
                boundsDirty = true;
                cachedAbsoluteBounds = null;
            
                // Invalidate all children
                foreach (var child in Children)
                {
                    child.InvalidateBounds();
                }
            }
        }
    
        public RECT GetAbsoluteBounds()
        {
            if (boundsDirty || cachedAbsoluteBounds == null)
            {
                cachedAbsoluteBounds = CalculateAbsoluteBounds();
                boundsDirty = false;
            }
            return cachedAbsoluteBounds.Value;
        }

        protected RECT CalculateAbsoluteBounds()
        {
            var result = localBounds;
            var current = Parent;
            
            while (current != null)
            {
                result.X += current.Bounds.X;
                result.Y += current.Bounds.Y;
                current = current.Parent;
            }
            
            return result;
        }

        public virtual void Draw(Framebuffer framebuffer)
        {
            if (!IsVisible) return;
            
            OnDraw(framebuffer);
            
            foreach (var child in Children)
            {
                child.Draw(framebuffer);
            }
        }

        protected abstract void OnDraw(Framebuffer framebuffer);

        public virtual bool HandleMouseEvent(double x, double y, bool isDown)
        {
            if (!IsVisible) return false;

            // Check children first (in reverse order for proper z-order)
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];
                // Convert coordinates to child's local space
                var childBounds = child.GetAbsoluteBounds();
                double childX = x - childBounds.X;
                double childY = y - childBounds.Y;
        
                if (childX >= 0 && childX < childBounds.Width &&
                    childY >= 0 && childY < childBounds.Height)
                {
                    if (child.HandleMouseEvent(x, y, isDown))
                        return true;
                }
            }

            // Then check this element
            var bounds = GetAbsoluteBounds();
            if (x >= bounds.X && x < bounds.X + bounds.Width &&
                y >= bounds.Y && y < bounds.Y + bounds.Height)
            {
                return OnMouseEvent(x - bounds.X, y - bounds.Y, isDown);
            }

            return false;
        }

        public virtual bool OnMouseEvent(double x, double y, bool isDown) => false;

        public virtual void CollectRenderCommands(RenderQueue queue)
        {
            // Base implementation does nothing
            // Derived classes should override this to add their render commands
        }
    }
}