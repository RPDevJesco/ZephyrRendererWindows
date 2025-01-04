using ZephyrRenderer.Platform;
using ZephyrRenderer.Renderer;
using ZephyrRenderer.UI;
using ZephyrRendererWindows;

namespace ZephyrRenderer.UIElement
{
    public class Window : UI.UIElement
    {
        private readonly IRenderer renderer;
        private readonly Framebuffer framebuffer;
        private bool isMouseDown;
        private double lastMouseX;
        private double lastMouseY;

        public Window(string title, double width, double height)
        {
            renderer = CreatePlatformRenderer();
            framebuffer = new Framebuffer(PrecisionHelper.ToInt(width), PrecisionHelper.ToInt(height));

            IntPtr windowHandle = renderer.CreateWindow(title, PrecisionHelper.ToInt(width), PrecisionHelper.ToInt(height));
            if (windowHandle == IntPtr.Zero)
                throw new Exception("Failed to create window.");
            Bounds = new RECT { Width = width, Height = height };

            if (renderer is WindowsRenderer winRenderer)
            {
                winRenderer.OnMouseMove += HandleMouseMove;
                winRenderer.OnMouseButton += HandleMouseButton;
            }

            // Set this window as the root element for event handling
            EventManager.Instance.SetRootElement(this);
        }

        private IRenderer CreatePlatformRenderer()
        {
            if (OperatingSystem.IsWindows())
                return new WindowsRenderer();
            
            throw new PlatformNotSupportedException(
                $"Platform not supported: {Environment.OSVersion.Platform}. " +
                "Currently supported platforms is Windows."
            );
        }

        private void HandleMouseMove(int x, int y)
        {
            lastMouseX = x;
            lastMouseY = y;
            EventManager.Instance.HandleMouseEvent(x, y, isMouseDown);
        }

        private void HandleMouseButton(int x, int y, bool isDown)
        {
            isMouseDown = isDown;
            lastMouseX = x;
            lastMouseY = y;
            EventManager.Instance.HandleMouseEvent(x, y, isDown);
        }

        protected override void OnDraw(Framebuffer framebuffer)
        {
            framebuffer.FillRect(
                Bounds.X,
                Bounds.Y,
                Bounds.Width,
                Bounds.Height,
                new Color(240, 240, 240) // Light gray background
            );
        }

        public void Run()
        {
            while (!renderer.ShouldClose)
            {
                Draw(framebuffer);
                renderer.Present(framebuffer);
                renderer.ProcessEvents();
                Thread.Sleep(1000 / 60);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            EventManager.Instance.Clear();
            renderer.Dispose();
            framebuffer.Dispose();
        }
    }
}