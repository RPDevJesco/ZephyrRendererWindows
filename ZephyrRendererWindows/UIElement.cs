using ZephyrRenderer.Platform;

namespace ZephyrRenderer.UI
{
    public abstract class UIElement
    {
        private RECT localBounds;
        public RECT Bounds 
        { 
            get => localBounds;
            set => localBounds = value;
        }

        public bool IsVisible { get; set; } = true;
        public UIElement? Parent { get; set; }
        public List<UIElement> Children { get; } = new List<UIElement>();
        
        // Flag to indicate if this element should receive mouse events
        protected virtual bool IsInteractive => false;

        public RECT GetAbsoluteBounds()
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

        // Changed to virtual so it can be overridden
        public virtual bool HandleMouseEvent(double x, double y, bool isDown)
        {
            if (!IsVisible) return false;

            var point = new POINT { X = x, Y = y };
            var bounds = GetAbsoluteBounds();
            
            if (bounds.Contains(point))
            {
                double localX = x - bounds.X;
                double localY = y - bounds.Y;
                return OnMouseEvent(localX, localY, isDown);
            }

            return false;
        }

        public virtual bool OnMouseEvent(double x, double y, bool isDown) => false;

        public void AddChild(UIElement child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void RemoveChild(UIElement child)
        {
            child.Parent = null;
            Children.Remove(child);
        }

        public virtual void Dispose()
        {
            foreach (var child in Children.ToList())
            {
                child.Dispose();
                RemoveChild(child);
            }
        }
    }
}