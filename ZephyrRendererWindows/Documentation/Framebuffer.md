# Framebuffer Class

The `Framebuffer` class is a lightweight rendering utility designed to handle pixel manipulation, shape drawing, and viewport management. It is particularly useful for custom rendering engines, image generation, and simple graphical operations.

## Features

- **Pixel Manipulation**: Set, blend, and retrieve individual pixels.
- **Shape Drawing**: Draw lines, rectangles (filled and unfilled), and more.
- **Viewport Management**: Restrict rendering to a specific region of the framebuffer.
- **Memory Management**: Efficient handling of pixel buffers using pinned memory.
- **Extensible**: Easily integrates into rendering pipelines.

---

## Usage

### 1. Creating a Framebuffer

```csharp
using ZephyrRenderer;

int width = 800;
int height = 600;

// Initialize the framebuffer
Framebuffer framebuffer = new Framebuffer(width, height);
```

### 2. Clearing the Framebuffer
Fill the entire framebuffer with a specific color.
```csharp
framebuffer.Clear(new Color(0, 0, 0, 255)); // Clear to black
```

### 3. Setting and Getting Pixels
Set individual pixels:
```csharp
framebuffer.SetPixel(100, 100, new Color(255, 0, 0, 255)); // Red pixel
```
Retrieve pixel data:
```csharp
Color pixelColor = framebuffer.GetPixel(100, 100);
Console.WriteLine($"Pixel Color: R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}, A={pixelColor.A}");
```

### 4. Drawing Shapes
Draw a Line:
```csharp
framebuffer.DrawLine(50, 50, 200, 200, new Color(0, 255, 0, 255)); // Green diagonal line
```
Draw a Filled Rectangle:
```csharp
framebuffer.FillRect(300, 300, 100, 50, new Color(0, 0, 255, 255)); // Blue rectangle
```
Draw an Unfilled Rectangle:
```csharp
framebuffer.DrawRect(500, 400, 150, 75, new Color(255, 255, 0, 255)); // Yellow rectangle border
```

### 5. Using Viewports
Constrain rendering to a specific region:
```csharp
framebuffer.SetViewport(100, 100, 400, 300);

// Operations within this region
framebuffer.SetPixel(150, 150, new Color(255, 0, 0, 255));
framebuffer.FillRect(200, 200, 50, 50, new Color(0, 255, 0, 255));

// Disable viewport
framebuffer.ClearViewport();
```

### 6. Disposing the Framebuffer
Always dispose of the framebuffer to free resources:
```csharp
framebuffer.Dispose();
```
Alternatively, use a using block:
```csharp
using (Framebuffer framebuffer = new Framebuffer(800, 600))
{
    framebuffer.Clear(new Color(0, 0, 0, 255));
    // Perform operations
}
```