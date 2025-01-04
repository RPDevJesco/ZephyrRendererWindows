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
    private readonly RenderQueue renderQueue = new();
    private readonly ClipStack clipStack = new();
    private bool isMouseDown;
    
    public Window(string title, double width, double height)
    {
        renderer = CreatePlatformRenderer();
        framebuffer = new Framebuffer((int)width, (int)height);
        Bounds = new RECT { Width = width, Height = height };

        IntPtr windowHandle = renderer.CreateWindow(title, (int)width, (int)height);
        if (windowHandle == IntPtr.Zero)
            throw new Exception("Failed to create window.");

        if (renderer is WindowsRenderer winRenderer)
        {
            winRenderer.OnMouseMove += (x, y) => 
            {
                EventManager.Instance.HandleMouseEvent(x, y, isMouseDown);
            };
            winRenderer.OnMouseButton += (x, y, isDown) => 
            {
                isMouseDown = isDown;
                EventManager.Instance.HandleMouseEvent(x, y, isDown);
            };
        }

        EventManager.Instance.SetRootElement(this);
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
        while (!renderer.ShouldClose)
        {
            // First clear the framebuffer
            framebuffer.Clear(new Color(240, 240, 240)); // Light gray background
            
            // Collect render commands for all elements
            renderQueue.Clear();
            CollectRenderCommands(renderQueue);

            // For debugging
           // Console.WriteLine($"Collected {renderQueue.CommandCount} render commands"); // Add this line

            // Execute all render commands
            renderQueue.Execute(framebuffer);
            
            // Present the framebuffer to the screen
            renderer.Present(framebuffer);
            renderer.ProcessEvents();
            Thread.Sleep(1000 / 60);
        }
    }

    public override void CollectRenderCommands(RenderQueue queue)
    {
        // Add window background as a render command
        queue.Enqueue(new FillRectCommand(0, GetAbsoluteBounds(), new Color(240, 240, 240)));

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
        base.Children.Add(child);
        child.Parent = this;
    }

    protected override void OnDraw(Framebuffer framebuffer)
    {
        // Drawing is now handled through CollectRenderCommands
    }
    }
}