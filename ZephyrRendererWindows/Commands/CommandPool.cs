namespace ZephyrRendererWindows
{
    public class CommandPool<T> where T : RenderCommand
    {
        private readonly Stack<T> pool = new();
        private readonly Func<T> factory;
    
        public CommandPool(Func<T> factory)
        {
            this.factory = factory;
        }
    
        public T Rent()
        {
            return pool.Count > 0 ? pool.Pop() : factory();
        }
    
        public void Return(T command)
        {
            pool.Push(command);
        }
    }
}