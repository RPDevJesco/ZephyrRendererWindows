using ZephyrRenderer.Platform;
using ZephyrRenderer.UI.Layout;
using ZephyrRenderer.UI.Grid;

namespace ZephyrRenderer.UI
{
    public class GridLayout : LayoutManagerBase
    {
        private readonly List<RowDefinition> rowDefinitions = new();
        private readonly List<ColumnDefinition> columnDefinitions = new();
        private double[,]? cellSizes;

        public double RowSpacing { get; set; }
        public double ColumnSpacing { get; set; }

        public GridLayout(ILayoutable owner) : base(owner)
        {
            Console.WriteLine("Creating GridLayout");
        }

        public void AddRowDefinition(RowDefinition rowDef)
        {
            Console.WriteLine($"Adding row definition: {rowDef.Height.UnitType}");
            rowDefinitions.Add(rowDef);
            InvalidateLayout();
        }

        public void AddColumnDefinition(ColumnDefinition colDef)
        {
            Console.WriteLine($"Adding column definition: {colDef.Width.UnitType}");
            columnDefinitions.Add(colDef);
            InvalidateLayout();
        }

        private bool IsFixedSize(GridUnitType unitType)
        {
            return unitType == GridUnitType.Fixed || unitType == GridUnitType.Pixel;
        }

        public override Size MeasureOverride(Size availableSize)
    {
        Console.WriteLine($"GridLayout measuring with size {availableSize.Width}x{availableSize.Height}");
        var owner = Owner as UIElement ?? throw new InvalidOperationException("Owner must be a UIElement");

        // Calculate row heights
        double totalHeight = 0;
        foreach (var row in rowDefinitions)
        {
            row.ActualHeight = row.Height.UnitType == GridUnitType.Fixed 
                ? row.Height.Value 
                : availableSize.Height / rowDefinitions.Count;
            totalHeight += row.ActualHeight;
        }

        // Calculate column widths
        double totalColumnSpacing = ColumnSpacing * (columnDefinitions.Count - 1);
        double availableColumnWidth = (availableSize.Width - totalColumnSpacing) / columnDefinitions.Count;

        foreach (var col in columnDefinitions)
        {
            col.ActualWidth = availableColumnWidth;
        }

        // Measure children
        foreach (var child in owner.Children)
        {
            if (!child.IsVisible) continue;

            var col = child.GridColumn;
            var row = child.GridRow;

            // Calculate available size for child
            var childAvailableSize = new Size(
                columnDefinitions[col].ActualWidth,
                rowDefinitions[row].ActualHeight
            );

            child.Measure(childAvailableSize);
        }

        return new Size(availableSize.Width, totalHeight);
    }

        public override void ArrangeOverride(RECT finalRect)
    {
        Console.WriteLine($"GridLayout arranging in rect {finalRect.X},{finalRect.Y} {finalRect.Width}x{finalRect.Height}");
        var owner = Owner as UIElement ?? throw new InvalidOperationException("Owner must be a UIElement");

        // Calculate column positions
        double[] columnX = new double[columnDefinitions.Count];
        double currentX = finalRect.X;
        for (int i = 0; i < columnDefinitions.Count; i++)
        {
            columnX[i] = currentX;
            currentX += columnDefinitions[i].ActualWidth + ColumnSpacing;
        }

        // Calculate row positions
        double[] rowY = new double[rowDefinitions.Count];
        double currentY = finalRect.Y;
        for (int i = 0; i < rowDefinitions.Count; i++)
        {
            rowY[i] = currentY;
            currentY += rowDefinitions[i].ActualHeight + RowSpacing;
        }

        // Arrange children
        foreach (var child in owner.Children)
        {
            if (!child.IsVisible) continue;

            var col = child.GridColumn;
            var row = child.GridRow;

            var childRect = new RECT(
                columnX[col],
                rowY[row],
                columnDefinitions[col].ActualWidth,
                rowDefinitions[row].ActualHeight
            );

            child.Arrange(childRect);
        }
    }
    }
}