using ZephyrRenderer.Platform;

namespace ZephyrRenderer.UI.Layout
{
    /// <summary>
    /// Defines the core functionality needed for an element to participate in the layout system.
    /// </summary>
    public interface ILayoutable
    {
        /// <summary>
        /// Gets the element's layout properties.
        /// </summary>
        LayoutProperties LayoutProperties { get; }

        /// <summary>
        /// Gets the desired size of the element after measurement.
        /// </summary>
        Size DesiredSize { get; }

        /// <summary>
        /// Gets or sets the actual bounds of the element after arrangement.
        /// </summary>
        RECT Bounds { get; set; }

        /// <summary>
        /// Gets whether the element is visible and should participate in layout.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Measures the element and its children.
        /// </summary>
        /// <param name="availableSize">The size available for the element.</param>
        /// <returns>The desired size of the element.</returns>
        Size Measure(Size availableSize);

        /// <summary>
        /// Arranges the element and its children within the given bounds.
        /// </summary>
        /// <param name="finalRect">The final rectangular bounds for the element.</param>
        void Arrange(RECT finalRect);

        /// <summary>
        /// Invalidates the layout of this element, forcing a new measure and arrange pass.
        /// </summary>
        void InvalidateLayout();
    }

    /// <summary>
    /// Defines the functionality for managing the layout of child elements.
    /// </summary>
    public interface ILayoutManager
    {
        /// <summary>
        /// Gets whether this layout manager needs to perform a layout update.
        /// </summary>
        bool NeedsLayout { get; }

        /// <summary>
        /// Measures the child elements to determine their desired sizes.
        /// </summary>
        /// <param name="availableSize">The size available for laying out children.</param>
        /// <returns>The desired size needed to accommodate the children.</returns>
        Size MeasureOverride(Size availableSize);

        /// <summary>
        /// Arranges the child elements within their final bounds.
        /// </summary>
        /// <param name="finalRect">The final rectangular bounds for the children.</param>
        void ArrangeOverride(RECT finalRect);

        /// <summary>
        /// Invalidates the current layout, forcing a new measure and arrange pass.
        /// </summary>
        void InvalidateLayout();
    }
}