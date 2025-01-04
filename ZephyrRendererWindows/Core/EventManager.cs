namespace ZephyrRenderer.UI
{
    public class EventManager
    {
        private static EventManager? instance;
        private UIElement? rootElement;
        
        public static EventManager Instance
        {
            get
            {
                instance ??= new EventManager();
                return instance;
            }
        }

        private EventManager() { }

        public void SetRootElement(UIElement root)
        {
            rootElement = root;
        }

        public void HandleMouseEvent(double x, double y, bool isDown)
        {
            if (rootElement == null) return;
            
            HandleMouseEventRecursive(rootElement, x, y, isDown, 0);
        }

        private bool HandleMouseEventRecursive(UIElement element, double x, double y, bool isDown, int depth)
        {
            if (!element.IsVisible)
            {
                return false;
            }
            
            var bounds = element.GetAbsoluteBounds();

            // Check children first (in reverse order for proper z-ordering, top to bottom)
            for (int i = element.Children.Count - 1; i >= 0; i--)
            {
                var child = element.Children[i];
                if (HandleMouseEventRecursive(child, x, y, isDown, depth + 1))
                {
                    return true;
                }
            }

            // Then check this element
            var handled = element.HandleMouseEvent(x, y, isDown);
            return handled;
        }
        
        public void Clear()
        {
            rootElement = null;
        }
    }
}