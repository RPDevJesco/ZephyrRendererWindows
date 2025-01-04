# FontRenderer

The `FontRenderer` class is a bitmap-based text rendering library designed for use in custom rendering engines. It enables developers to draw text, align it, wrap lines, and customize character and line spacing on a framebuffer.

## Features

- **Text Rendering**: Render text using a bitmap font.
- **Alignment**: Support for horizontal and vertical text alignment.
- **Custom Spacing**: Configure character and line spacing.
- **Line Wrapping**: Automatically wrap text when it exceeds a specified width.
- **Text Measurement**: Calculate the pixel dimensions of rendered text.

---

## Usage

### 1. Rendering Text

To render text on a `Framebuffer`:

```csharp
Framebuffer framebuffer = new Framebuffer(800, 600);
Color white = new Color(255, 255, 255, 255);

FontRenderer.DrawText(framebuffer, "Hello, World!", 50, 50, white);
```

### 2. Customizing Spacing

Use the charSpacing and lineSpacing parameters for custom character and line spacing:

```csharp
FontRenderer.DrawText(framebuffer, "Custom Spacing", 100, 100, white, charSpacing: 2, lineSpacing: 4);
```

### 3. Aligning Text

Align text within a specific area using DrawTextAligned:

```csharp
FontRenderer.DrawTextAligned(
    framebuffer,
    "Centered Text",
    0, 0,
    800, 600,
    white,
    FontRenderer.Alignment.CenterHorizontally | FontRenderer.Alignment.CenterVertically
);
```
Align text with custom spacing:
```csharp
FontRenderer.DrawTextAligned(
    framebuffer,
    "Custom Aligned Text",
    0, 0,
    800, 600,
    white,
    FontRenderer.Alignment.CenterHorizontally | FontRenderer.Alignment.CenterVertically,
    charSpacing: 3,
    lineSpacing: 5
);
```

### 4. Wrapping Text

Render text with line wrapping:

```csharp
FontRenderer.DrawTextWrapped(framebuffer, "This text will wrap to the next line if it exceeds the maximum width.", 50, 200, 300, white);
```

### 5. Measuring Text

Measure the dimensions of text in pixels:

```csharp
(int width, int height) = FontRenderer.MeasureTextSize("Sample Text");
Console.WriteLine($"Text Width: {width}, Height: {height}");
```