using ZephyrRenderer;

namespace ZephyrRendererWindows
{
    public class RenderQueue
    {
        private readonly List<RenderCommand> commands = new();
        private readonly Dictionary<Type, List<RenderCommand>> commandsByType = new();
    
        public int CommandCount => commands.Count;  // Add this propert
    
        public void Enqueue(RenderCommand command)
        {
            commands.Add(command);
        
            var type = command.GetType();
            if (!commandsByType.TryGetValue(type, out var typeList))
            {
                typeList = new List<RenderCommand>();
                commandsByType[type] = typeList;
            }
            typeList.Add(command);
        }
    
        public void Execute(Framebuffer target)
        {
            // Sort commands by Z-index
            commands.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
        
            // Execute all commands
            foreach (var cmd in commands)
            {
                cmd.Execute(target);
            }
        
            Clear();
        }

        public void Clear()
        {
            commands.Clear();
            foreach (var list in commandsByType.Values)
            {
                list.Clear();
            }
        }
    }
}