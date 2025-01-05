using ZephyrRenderer.Platform;
using ZephyrRenderer.Renderer;
using ZephyrRenderer.UI;
using ZephyrRenderer.UI.Layout;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UIElement
{
    public class Window : UI.UIElement
    {
        private readonly IRenderer renderer;
        private readonly RenderQueue renderQueue = new();
        private readonly ClipStack clipStack = new();
        private Framebuffer framebuffer;
        private double currentWidth;
        private double currentHeight;
        private bool isMouseDown;
        private bool needsLayout = true;

        public Window(string title, double width, double height)
        {
            Console.WriteLine($"Creating window with size {width}x{height}");
            currentWidth = width;
            currentHeight = height;
            renderer = CreatePlatformRenderer();
            framebuffer = new Framebuffer((int)width, (int)height);
        
            // Set explicit layout properties for the window
            LayoutProperties.Width = LayoutSize.Fixed(width);
            LayoutProperties.Height = LayoutSize.Fixed(height);
            LayoutProperties.HorizontalAlignment = HorizontalAlignment.Stretch;
            LayoutProperties.VerticalAlignment = VerticalAlignment.Stretch;
        
            // Initialize bounds
            Bounds = new RECT(0, 0, width, height);

            IntPtr windowHandle = renderer.CreateWindow(title, (int)width, (int)height);
            if (windowHandle == IntPtr.Zero)
                throw new Exception("Failed to create window.");

            if (renderer is WindowsRenderer winRenderer)
            {
                winRenderer.OnMouseMove += (x, y) => { EventManager.Instance.HandleMouseEvent(x, y, isMouseDown); };
                winRenderer.OnMouseButton += (x, y, isDown) =>
                {
                    isMouseDown = isDown;
                    EventManager.Instance.HandleMouseEvent(x, y, isDown);
                };
                
                winRenderer.OnResize += HandleResize;
            }

            EventManager.Instance.SetRootElement(this);
        }
        
        protected override Size MeasureOverride(Size availableSize)
        {
            Console.WriteLine($"Window MeasureOverride with size: {availableSize.Width}x{availableSize.Height}");

            // Store window size for children to use
            Bounds = new RECT(0, 0, availableSize.Width, availableSize.Height);

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    // Pass the full window size to children
                    child.Measure(new Size(availableSize.Width, availableSize.Height));
                }
            }

            return availableSize;
        }

        protected override void ArrangeOverride(RECT finalRect)
        {
            Console.WriteLine($"Window ArrangeOverride with rect: {finalRect.X},{finalRect.Y} {finalRect.Width}x{finalRect.Height}");

            // Store window size
            Bounds = finalRect;

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    // Pass full window rect to children
                    child.Arrange(finalRect);
                }
            }
        }

        private void PerformLayout()
        {
            Console.WriteLine("Performing layout pass");
            var size = new Size(Bounds.Width, Bounds.Height);
            Measure(size);
            Arrange(new RECT(0, 0, Bounds.Width, Bounds.Height));
            needsLayout = false;
        }

        private void HandleResize(int width, int height)
        {
            Console.WriteLine($"Window resize: {width}x{height}");
        
            // Update window dimensions
            Bounds = new RECT(0, 0, width, height);
            LayoutProperties.Width = LayoutSize.Fixed(width);
            LayoutProperties.Height = LayoutSize.Fixed(height);
        
            // Update framebuffer
            framebuffer.Dispose();
            framebuffer = new Framebuffer(width, height);
        
            // Force layout update
            InvalidateLayout();
            PerformLayout();
        }
        
        private IRenderer CreatePlatformRenderer()
        {
            if (OperatingSystem.IsWindows())
                return new WindowsRenderer();
            
            throw new PlatformNotSupportedException(
                $"Platform not supported: {Environment.OSVersion.Platform}"
            );
        }

        public void Run()
        {
            PerformLayout(); // Initial layout

            while (!renderer.ShouldClose)
            {
                // Perform layout if needed
                if (needsLayout)
                {
                    PerformLayout();
                }

                // Clear the framebuffer
                framebuffer.Clear(new Color(240, 240, 240));

                // Collect render commands
                renderQueue.Clear();
                CollectRenderCommands(renderQueue);

                // Execute all render commands
                renderQueue.Execute(framebuffer);

                // Present the framebuffer
                renderer.Present(framebuffer);
                renderer.ProcessEvents();
                Thread.Sleep(1000 / 60);
            }
        }
        
        public override void InvalidateLayout()
        {
            base.InvalidateLayout();
            needsLayout = true;
        }

        public override void CollectRenderCommands(RenderQueue queue)
        {
            // Add window background as a render command
            var bounds = GetAbsoluteBounds();
            Console.WriteLine($"Window bounds: {bounds.X},{bounds.Y} {bounds.Width}x{bounds.Height}");
            queue.Enqueue(new FillRectCommand(-100, bounds, new Color(240, 240, 240)));

            // Collect commands from all children
            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    child.CollectRenderCommands(queue);
                }
            }
        }

        public new void AddChild(UI.UIElement child)
        {
            Console.WriteLine($"Adding child to window");
            base.Children.Add(child);
            child.Parent = this;
            InvalidateLayout();
            PerformLayout(); // Immediate layout pass when adding children
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            // Drawing is handled through CollectRenderCommands
        }
    }
}