namespace ZephyrRenderer
{
    public interface IRenderer : IDisposable
    {
        void Initialize(string title, double width, double height);
        void Present(Framebuffer framebuffer);
        bool ShouldClose { get; }
        void ProcessEvents();
        IntPtr CreateWindow(string title, int width, int height); // Added
        IntPtr GetWindowHandle(); // Added
    }
}