# ZephyrRenderer

ZephyrRenderer is a custom-built UI framework designed for lightweight, efficient, and extensible rendering in 2D environments. The project emphasizes modularity, customizability, and performance, allowing developers to create rich user interfaces and graphical elements for applications or games.

## **Project Overview**
ZephyrRenderer provides a foundational system for rendering UI elements, handling events, and managing graphical output on Windows platforms. Built without reliance on external graphics libraries, it implements its own rendering pipeline, event handling, and graphical primitives.

### **Key Features**
- **Custom UI Components**: Includes panels and buttons as the foundational UI elements, with plans for more components like labels, sliders, and more.
- **Batched Rendering**: Optimized rendering pipeline that uses a `RenderQueue` to group and sort draw calls for better performance.
- **Event Management**: Handles mouse events with propagation and prioritization, including support for interactivity and visibility.
- **Cross-Platform Potential**: Modular design to facilitate platform-specific renderers (currently supports Windows).
- **Custom Render Commands**: Core rendering functionality provided via commands like `DrawLineCommand`, `FillRectCommand`, and `DrawTextCommand`.

### **Current Implementation**
#### **Core Components**
1. **Rendering System**
   - `RenderQueue`: Organizes and executes rendering commands in a batched and z-indexed manner.
   - `Framebuffer`: A custom in-memory frame buffer for pixel-level operations.
   - `RenderCommand`: Base class for all rendering operations like drawing shapes, text, and images.

2. **UI System**
   - `Panel`: A container for organizing child UI elements with customizable backgrounds.
   - `Button`: A clickable UI element with hover and press states, supporting custom text and colors.
   - `Window`: The main rendering surface, managing the render loop and events.

3. **Event System**
   - `EventManager`: Propagates mouse events through the UI hierarchy, supporting hit testing and interactivity.
   - `ClipStack`: Manages clipping regions to ensure elements are rendered within bounds.

4. **Platform Integration**
   - `WindowsRenderer`: A platform-specific renderer leveraging Win32 APIs for creating windows and handling graphical output.

#### **Utilities**
- **FontRenderer**: Bitmap-based text rendering system supporting basic alignment and wrapping.
- **PrecisionHelper**: Handles precise coordinate conversions.
- **DirtyRegionTracker**: Tracks regions requiring updates for efficient rendering.
- **Color**: Represents RGBA colors.

### **Current Capabilities**
- Render hierarchical UI elements with parent-child relationships.
- Handle mouse interactions with event bubbling and interactivity.
- Render basic shapes, text, and buttons with visual feedback.
- Display animation through custom panels like `AnimatedPanel`.
- Support for debugging with log output and visual render queues.

### **Development Progress**
#### **Completed**
- Basic rendering pipeline and framebuffer management.
- Foundational UI elements (Panel and Button).
- Event handling and mouse interaction.
- Windows-specific renderer for graphical output.
- Support for batched rendering and basic animations.

#### **In Progress**
- **Advanced UI Elements**: Labels, sliders, and other common controls.
- **Rich Text Support**: Enhanced text rendering with styles and alignment.
- **Layout System**: Flexible layouts for positioning and resizing elements.
- **Multi-Window Support**: Managing multiple rendering surfaces.
- **Styling System**: CSS-like styling for UI elements.

#### **Planned**
- **Sprite Rendering**: Render 2D sprites with animations.
- **2D Physics**: Implement colliders, forces, and collision detection.
- **Bitmap and Image Support**: Enhanced support for rendering images in various formats.

### **Getting Started**
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd ZephyrRenderer
   ```
2. Build the project using your preferred IDE (e.g., Visual Studio, JetBrains Rider).
3. Run the `Program.cs` file to view the demo application showcasing the current capabilities.

### **Future Goals**
ZephyrRenderer aims to become a complete and versatile 2D rendering and UI framework, suitable for both applications and lightweight games. Its modular architecture ensures adaptability and room for extensions, making it a powerful tool for developers.

---


![image](https://github.com/user-attachments/assets/0d12a015-7bac-4053-a0ab-1e638a93c177)

- **All buttons** are clickable, Test and Button will write to the console window as log statements.
- **the bitmap** moves from left to right. 
- **Randomize Colors** will change the color of the square and the lines. 
- **Toggle Speed** will change between the default speed and the max speed set. 
- **Reset animation** will start the animation back from the starting point