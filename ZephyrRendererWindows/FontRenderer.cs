namespace ZephyrRenderer.UI
{
    /// <summary>
    /// Provides functionality for rendering text using a bitmap-based font system.
    /// </summary>
    public static class FontRenderer
    {
        /// <summary>
        /// Specifies alignment options for rendering text.
        /// </summary>
        [Flags]
        public enum Alignment
        {
            Left = 0,
            Right = 1,
            CenterHorizontally = 2,
            Top = 0,
            Bottom = 4,
            CenterVertically = 8
        }
        
        private static readonly Dictionary<char, bool[,]> CharacterBitmaps = new()
        {
            ['A'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true }
            },
            ['B'] = new bool[,] {
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, false }
            },
            ['C'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['D'] = new bool[,] {
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, false }
            },
            ['E'] = new bool[,] {
                { true, true, true, true, true },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, true }
            },
            ['F'] = new bool[,] {
                { true, true, true, true, true },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false }
            },
            ['G'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, false },
                { true, false, true, true, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['H'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true }
            },
            ['I'] = new bool[,] {
                { false, true, true, true, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, true, true, true, false }
            },
            ['J'] = new bool[,] {
                { true, true, true, true, true },
                { false, false, false, false, true },
                { false, false, false, false, true },
                { false, false, false, false, true },
                { false, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['K'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, true, false },
                { true, false, true, false, false },
                { true, true, false, false, false },
                { true, false, true, false, false },
                { true, false, false, true, false },
                { true, false, false, false, true }
            },
            ['L'] = new bool[,] {
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, true }
            },
            ['M'] = new bool[,] {
                { true, false, false, false, true },
                { true, true, false, true, true },
                { true, false, true, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true }
            },
            ['N'] = new bool[,] {
                { true, false, false, false, true },
                { true, true, false, false, true },
                { true, false, true, false, true },
                { true, false, false, true, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true }
            },
            ['O'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['P'] = new bool[,] {
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, false, false, false, false }
            },
            ['Q'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, true, false, true },
                { false, true, true, true, true },
                { false, false, false, false, true }
            },
            ['R'] = new bool[,] {
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true, true, true, false },
                { true, false, true, false, false },
                { true, false, false, true, false },
                { true, false, false, false, true }
            },
            ['S'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, false },
                { false, true, true, true, false },
                { false, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['T'] = new bool[,] {
                { true, true, true, true, true },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false }
            },
            ['U'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['V'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, false, true, false },
                { false, false, true, false, false }
            },
            ['W'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, true, false, true },
                { true, false, true, false, true },
                { true, true, false, true, true },
                { true, false, false, false, true }
            },
            ['X'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, false, true, false },
                { false, false, true, false, false },
                { false, true, false, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true }
            },
            ['Y'] = new bool[,] {
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, false, true, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false }
            },
            ['Z'] = new bool[,] {
                { true, true, true, true, true },
                { false, false, false, false, true },
                { false, false, false, true, false },
                { false, false, true, false, false },
                { false, true, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, true }
            },
            ['0'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, true, true },
                { true, false, true, false, true },
                { true, true, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['1'] = new bool[,] {
                { false, false, true, false, false },
                { false, true, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, false, true, false, false },
                { false, true, true, true, false }
            },
            ['2'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { false, false, false, false, true },
                { false, false, false, true, false },
                { false, false, true, false, false },
                { false, true, false, false, false },
                { true, true, true, true, true }
            },
            ['3'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { false, false, false, false, true },
                { false, false, true, true, false },
                { false, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['4'] = new bool[,] {
                { false, false, false, true, false },
                { false, false, true, true, false },
                { false, true, false, true, false },
                { true, false, false, true, false },
                { true, true, true, true, true },
                { false, false, false, true, false },
                { false, false, false, true, false }
            },
            ['5'] = new bool[,] {
                { true, true, true, true, true },
                { true, false, false, false, false },
                { true, true, true, true, false },
                { false, false, false, false, true },
                { false, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['6'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, false },
                { true, false, false, false, false },
                { true, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['7'] = new bool[,] {
                { true, true, true, true, true },
                { false, false, false, false, true },
                { false, false, false, true, false },
                { false, false, true, false, false },
                { false, true, false, false, false },
                { false, true, false, false, false },
                { false, true, false, false, false }
            },
            ['8'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            ['9'] = new bool[,] {
                { false, true, true, true, false },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, true },
                { false, false, false, false, true },
                { true, false, false, false, true },
                { false, true, true, true, false }
            },
            [' '] = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            }
        };

        /// <summary>
        /// Draws a text string onto the specified framebuffer with custom character and line spacing.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="text">The text string to render.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="charSpacing">The spacing between characters in pixels.</param>
        /// <param name="lineSpacing">The spacing between lines in pixels.</param>
        public static void DrawText(Framebuffer framebuffer, string text, int x, int y, Color color)
        {
            int currentX = x;
            foreach (char c in text.ToUpper())
            {
                if (CharacterBitmaps.TryGetValue(c, out bool[,] bitmap))
                {
                    int height = bitmap.GetLength(0);
                    int width = bitmap.GetLength(1);
            
                    for (int py = 0; py < height; py++)
                    {
                        for (int px = 0; px < width; px++)
                        {
                            if (bitmap[py, px])
                            {
                                framebuffer.SetPixel(currentX + px, y + py, color);
                            }
                        }
                    }
                    currentX += 6; // Fixed spacing between characters
                }
            }
        }

        // Keep double version for backwards compatibility, but handle conversion explicitly
        public static void DrawText(Framebuffer framebuffer, string text, double x, double y, Color color)
        {
            DrawText(framebuffer, text, (int)x, (int)y, color);
        }
        
        /// <summary>
        /// Draws a text string aligned within the specified area of the framebuffer.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="text">The text string to render.</param>
        /// <param name="x">The x-coordinate of the alignment area.</param>
        /// <param name="y">The y-coordinate of the alignment area.</param>
        /// <param name="width">The width of the alignment area.</param>
        /// <param name="height">The height of the alignment area.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="alignment">The alignment options for the text.</param>
        public static void DrawTextAligned(Framebuffer framebuffer, string text, int x, int y, int width, int height, Color color, Alignment alignment)
        {
            int textWidth = MeasureText(text);
            int startX = x;
            int startY = y;

            if (alignment.HasFlag(Alignment.CenterHorizontally))
            {
                startX += (width - textWidth) / 2;
            }
            if (alignment.HasFlag(Alignment.CenterVertically))
            {
                startY += (height - 7) / 2; // Character height is 7 pixels
            }

            DrawText(framebuffer, text, startX, startY, color);
        }
        
        /// <summary>
        /// Draws a single character onto the framebuffer.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="bitmap">The bitmap representation of the character.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="color">The color of the character.</param>
        private static void DrawCharacter(Framebuffer framebuffer, bool[,] bitmap, int x, int y, Color color)
        {
            int height = bitmap.GetLength(0);
            int width = bitmap.GetLength(1);

            for (int py = 0; py < height; py++)
            {
                for (int px = 0; px < width; px++)
                {
                    if (bitmap[py, px])
                    {
                        framebuffer.SetPixel(x + px, y + py, color);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a single character onto the framebuffer.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="bitmap">The bitmap representation of the character.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="color">The color of the character.</param>
        private static void DrawCharacter(Framebuffer framebuffer, bool[,] bitmap, double x, double y, Color color)
        {
            DrawCharacter(framebuffer, bitmap, (int)x, (int)y, color);
        }

        /// <summary>
        /// Draws a single character onto the framebuffer with custom spacing.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="bitmap">The bitmap representation of the character.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="color">The color of the character.</param>
        /// <param name="charSpacing">Additional spacing between character columns.</param>
        /// <param name="lineSpacing">Additional spacing between character rows.</param>
        private static void DrawCharacter(Framebuffer framebuffer, bool[,] bitmap, int x, int y, Color color, int charSpacing = 1, int lineSpacing = 2)
        {
            int height = bitmap.GetLength(0);
            int width = bitmap.GetLength(1);

            for (int py = 0; py < height + lineSpacing; py++)
            {
                for (int px = 0; px < width + charSpacing; px++)
                {
                    if (py < height && px < width && bitmap[py, px])
                    {
                        framebuffer.SetPixel(x + px, y + py, color);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a single character onto the framebuffer with custom spacing.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="bitmap">The bitmap representation of the character.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="color">The color of the character.</param>
        /// <param name="charSpacing">Additional spacing between character columns.</param>
        /// <param name="lineSpacing">Additional spacing between character rows.</param>
        private static void DrawCharacter(Framebuffer framebuffer, bool[,] bitmap, double x, double y, Color color, double charSpacing = 1, double lineSpacing = 2)
        {
            DrawCharacter(framebuffer, bitmap, (int)x, (int)y, color, charSpacing, lineSpacing);
        }

        /// <summary>
        /// Draws a text string with line wrapping.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="text">The text string to render.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="maxWidth">The maximum width for a line before wrapping occurs.</param>
        /// <param name="color">The color of the text.</param>
        public static void DrawTextWrapped(Framebuffer framebuffer, string text, int x, int y, int maxWidth, Color color)
        {
            int currentX = x;
            int currentY = y;

            foreach (char c in text.ToUpper())
            {
                if (c == '\n' || currentX + 6 > maxWidth)
                {
                    currentX = x;
                    currentY += 8; // Move to the next line
                    if (c == '\n') continue;
                }

                if (CharacterBitmaps.TryGetValue(c, out bool[,] bitmap))
                {
                    DrawCharacter(framebuffer, bitmap, currentX, currentY, color);
                }
                currentX += 6; // 5 pixels wide + 1 pixel spacing
            }
        }

        /// <summary>
        /// Draws a text string with line wrapping.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to draw on.</param>
        /// <param name="text">The text string to render.</param>
        /// <param name="x">The x-coordinate to start rendering.</param>
        /// <param name="y">The y-coordinate to start rendering.</param>
        /// <param name="maxWidth">The maximum width for a line before wrapping occurs.</param>
        /// <param name="color">The color of the text.</param>
        public static void DrawTextWrapped(Framebuffer framebuffer, string text, double x, double y, double maxWidth, Color color)
        {
           DrawTextWrapped(framebuffer, text, (int)x, (int)y, maxWidth, color);
        }

        /// <summary>
        /// Measures the size of a text string in pixels.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <returns>A tuple containing the width and height of the text.</returns>
        public static (double width, double height) MeasureTextSize(string text)
        {
            double lines = text.Split('\n').Length;
            double width = MeasureText(text);
            double height = lines * 7 + (lines - 1) * 2; // Character height + line spacing
            return (width, height);
        }

        /// <summary>
        /// Measures the width of a text string in pixels.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <returns>The width of the text string in pixels.</returns>
        public static int MeasureText(string text)
        {
            return text.Length * 6; // 5 pixels wide + 1 pixel spacing
        }
    }
}