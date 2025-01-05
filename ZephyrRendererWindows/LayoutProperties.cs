namespace ZephyrRenderer.UI.Layout
{
    /// <summary>
    /// Specifies the direction in which elements should be arranged in a StackLayout.
    /// </summary>
    public enum LayoutDirection
    {
        /// <summary>
        /// Elements are arranged vertically from top to bottom.
        /// </summary>
        Vertical,

        /// <summary>
        /// Elements are arranged horizontally from left to right.
        /// </summary>
        Horizontal,
        
        /// <summary>
        /// Elements are arranged horizontally from right to left.
        /// </summary>
        RightToLeft,
        
        /// <summary>
        /// Elements are arranged vertically from bottom to top.
        /// </summary>
        BottomToTop
    }
    
    /// <summary>
    /// Specifies how an element should be sized.
    /// </summary>
    public enum SizeMode
    {
        /// <summary>
        /// Element size is fixed to a specific value.
        /// </summary>
        Fixed,
        
        /// <summary>
        /// Element sizes itself based on its content.
        /// </summary>
        Auto,
        
        /// <summary>
        /// Element takes a proportional amount of remaining space.
        /// </summary>
        Star,
        
        /// <summary>
        /// Element takes a percentage of parent's size.
        /// </summary>
        Percentage
    }

    /// <summary>
    /// Represents a size value with its sizing mode.
    /// </summary>
    public readonly struct LayoutSize
    {
        public readonly double Value { get; }
        public readonly SizeMode Mode { get; }

        private LayoutSize(double value, SizeMode mode)
        {
            Value = value;
            Mode = mode;
        }

        public static LayoutSize Fixed(double value) => new(value, SizeMode.Fixed);
        public static LayoutSize Auto => new(0, SizeMode.Auto);
        public static LayoutSize Star(double value = 1) => new(value, SizeMode.Star);
        public static LayoutSize Percentage(double value) => new(value, SizeMode.Percentage);

        public override string ToString() => Mode switch
        {
            SizeMode.Fixed => $"{Value}px",
            SizeMode.Auto => "Auto",
            SizeMode.Star => Value == 1 ? "*" : $"{Value}*",
            SizeMode.Percentage => $"{Value}%",
            _ => base.ToString()
        };
    }

    /// <summary>
    /// Specifies how an element should be aligned horizontally within its parent.
    /// </summary>
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
        Stretch
    }

    /// <summary>
    /// Specifies how an element should be aligned vertically within its parent.
    /// </summary>
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom,
        Stretch
    }

    /// <summary>
    /// Represents a set of distances from the edges of an element.
    /// </summary>
    public readonly struct Thickness
    {
        public readonly double Left { get; }
        public readonly double Top { get; }
        public readonly double Right { get; }
        public readonly double Bottom { get; }

        public Thickness(double uniformSize) 
            : this(uniformSize, uniformSize, uniformSize, uniformSize)
        {
        }

        public Thickness(double horizontal, double vertical) 
            : this(horizontal, vertical, horizontal, vertical)
        {
        }

        public Thickness(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public double Horizontal => Left + Right;
        public double Vertical => Top + Bottom;

        public static Thickness Zero => new(0);
    }

    /// <summary>
    /// Represents the size of an element.
    /// </summary>
    public readonly struct Size
    {
        public readonly double Width { get; }
        public readonly double Height { get; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static Size Zero => new(0, 0);
        public static Size Infinite => new(double.PositiveInfinity, double.PositiveInfinity);

        public Size WithWidth(double width) => new(width, Height);
        public Size WithHeight(double height) => new(Width, height);
    }

    /// <summary>
    /// Contains all layout-related properties for a UI element.
    /// </summary>
    public class LayoutProperties
    {
        public LayoutSize Width { get; set; } = LayoutSize.Auto;
        public LayoutSize Height { get; set; } = LayoutSize.Auto;
        public LayoutSize MinWidth { get; set; } = LayoutSize.Fixed(0);
        public LayoutSize MinHeight { get; set; } = LayoutSize.Fixed(0);
        public LayoutSize MaxWidth { get; set; } = LayoutSize.Fixed(double.PositiveInfinity);
        public LayoutSize MaxHeight { get; set; } = LayoutSize.Fixed(double.PositiveInfinity);
        
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
        
        public Thickness Margin { get; set; } = Thickness.Zero;
        public Thickness Padding { get; set; } = Thickness.Zero;

        public static LayoutProperties Default => new();

        public LayoutProperties Clone() => new()
        {
            Width = Width,
            Height = Height,
            MinWidth = MinWidth,
            MinHeight = MinHeight,
            MaxWidth = MaxWidth,
            MaxHeight = MaxHeight,
            HorizontalAlignment = HorizontalAlignment,
            VerticalAlignment = VerticalAlignment,
            Margin = Margin,
            Padding = Padding
        };
    }
}