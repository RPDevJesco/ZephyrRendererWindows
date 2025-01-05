
namespace ZephyrRenderer.UI.Grid
{
    /// <summary>
    /// Represents a length used in grid layout measurement.
    /// </summary>
    public class GridLength
    {
        public double Value { get; }
        public GridUnitType UnitType { get; }

        private GridLength(double value, GridUnitType unitType)
        {
            Value = value;
            UnitType = unitType;
        }

        /// <summary>
        /// Gets a GridLength that sizes to the content.
        /// </summary>
        public static GridLength Auto => new(0, GridUnitType.Auto);

        /// <summary>
        /// Gets a GridLength that has a fixed size in pixels.
        /// </summary>
        /// <param name="value">The fixed size in pixels.</param>
        public static GridLength Fixed(double value) => new(value, GridUnitType.Fixed);

        /// <summary>
        /// Gets a GridLength that has a fixed size in pixels.
        /// </summary>
        /// <param name="value">The fixed size in pixels.</param>
        public static GridLength Pixel(double value) => Fixed(value);

        /// <summary>
        /// Gets a GridLength that sizes proportionally to available space.
        /// </summary>
        /// <param name="value">The proportional weight.</param>
        public static GridLength Star(double value = 1) => new(value, GridUnitType.Star);

        /// <summary>
        /// Gets a GridLength that sizes to a percentage of available space.
        /// </summary>
        /// <param name="value">The percentage value (0-100).</param>
        public static GridLength Percent(double value) => new(value, GridUnitType.Percent);
    }

    /// <summary>
    /// Defines how a grid row or column is sized.
    /// </summary>
    public enum GridUnitType
    {
        Auto,       // Size based on content
        Fixed,      // Fixed size in pixels (same as Pixel)
        Pixel,      // Fixed size in pixels
        Star,       // Proportional size
        Percent     // Percentage of available space
    }

    /// <summary>
    /// Represents a row definition in a grid.
    /// </summary>
    public class RowDefinition
    {
        public GridLength Height { get; set; } = GridLength.Auto;
        public double MinHeight { get; set; } = 0;
        public double MaxHeight { get; set; } = double.PositiveInfinity;
        public double ActualHeight { get; internal set; }

        public RowDefinition() { }

        public RowDefinition(GridLength height)
        {
            Height = height;
        }
    }

    /// <summary>
    /// Represents a column definition in a grid.
    /// </summary>
    public class ColumnDefinition
    {
        public GridLength Width { get; set; } = GridLength.Auto;
        public double MinWidth { get; set; } = 0;
        public double MaxWidth { get; set; } = double.PositiveInfinity;
        public double ActualWidth { get; internal set; }

        public ColumnDefinition() { }

        public ColumnDefinition(GridLength width)
        {
            Width = width;
        }
    }

    /// <summary>
    /// Attached properties for Grid elements.
    /// </summary>
    public static class Grid
    {
        public static int GetRow(UIElement element)
        {
            return element.GridRow;
        }

        public static void SetRow(UIElement element, int value)
        {
            element.GridRow = value;
            element.InvalidateLayout();
        }

        public static int GetColumn(UIElement element)
        {
            return element.GridColumn;
        }

        public static void SetColumn(UIElement element, int value)
        {
            element.GridColumn = value;
            element.InvalidateLayout();
        }

        public static int GetRowSpan(UIElement element)
        {
            return element.GridRowSpan;
        }

        public static void SetRowSpan(UIElement element, int value)
        {
            element.GridRowSpan = value;
            element.InvalidateLayout();
        }

        public static int GetColumnSpan(UIElement element)
        {
            return element.GridColumnSpan;
        }

        public static void SetColumnSpan(UIElement element, int value)
        {
            element.GridColumnSpan = value;
            element.InvalidateLayout();
        }
    }
}