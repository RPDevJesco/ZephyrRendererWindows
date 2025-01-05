using ZephyrRenderer.Platform;
using ZephyrRenderer.UI;
using ZephyrRenderer.UI.Grid;
using ZephyrRenderer.UI.Layout;
using ZephyrRenderer.UIElement;
using ZephyrRendererWindows;
using Color = ZephyrRenderer.Color;

class Program
{
    static void Main()
    {
        // Create our main window
        var window = new Window("Zephyr UI Demo - Grid & Stack Layout", 800, 600);
        
        CreateTestScene(window);
        window.Run();
    }

    static void CreateTestScene(Window window)
    {
        double windowWidth = window.Bounds.Width;
        double windowHeight = window.Bounds.Height;
        double margin = 10;

        // Main panel
        var mainPanel = new Panel
        {
            BackgroundColor = new Color(30, 30, 40)
        };
        mainPanel.LayoutProperties.Width = LayoutSize.Star(1);
        mainPanel.LayoutProperties.Height = LayoutSize.Star(1);
        mainPanel.LayoutProperties.Margin = new Thickness(margin);
        window.AddChild(mainPanel);

        // Top panel with grid layout
        var topPanel = new Panel
        {
            BackgroundColor = new Color(40, 40, 50)
        };
        topPanel.SetGridLayout();
        topPanel.LayoutProperties.Width = LayoutSize.Star(1);
        topPanel.LayoutProperties.Height = LayoutSize.Fixed(40);
        mainPanel.AddChild(topPanel);
        
        // Configure top panel grid
        var gridLayout = topPanel.LayoutManager as GridLayout;
        if (gridLayout != null)
        {
            gridLayout.ColumnSpacing = 10;
            for (int i = 0; i < 5; i++)
            {
                gridLayout.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star(1) });
            }
            gridLayout.AddRowDefinition(new RowDefinition { Height = GridLength.Fixed(35) });
        }

        // Common button colors
        var buttonTextColor = new Color(240, 240, 240);
        var buttonBgColor = new Color(60, 60, 70);
        var buttonHoverColor = new Color(70, 70, 80);
        var buttonPressedColor = new Color(50, 50, 60);
        var buttonBorderColor = new Color(80, 80, 90);
        var buttonBorderHoverColor = new Color(100, 100, 110);

        // Add buttons to grid
        string[] buttonTexts = { "Button 1", "Button 2", "Button 3", "Button 4", "Button 5" };
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            var button = new Button(
                new RECT(0, 0, 0, 35),
                buttonTexts[i],
                new Color(240, 240, 240),
                new Color(60, 60, 70),
                new Color(70, 70, 80),
                new Color(50, 50, 60),
                new Color(80, 80, 90),
                new Color(100, 100, 110)
            );
            button.GridColumn = i;
            button.GridRow = 0;
            button.LayoutProperties.HorizontalAlignment = HorizontalAlignment.Stretch;
            topPanel.AddChild(button);
        }

        double sideWidth = ((windowWidth - margin * 3) / 2);
        double sideHeight = windowHeight - margin * 2 - 40 - margin; // Full height minus margins and top panel

        // Left content panel
        var leftPanel = new Panel
        {
            BackgroundColor = new Color(40, 40, 50)
        };
        leftPanel.SetStackLayout(LayoutDirection.Vertical, margin);
        leftPanel.LayoutProperties.Width = LayoutSize.Fixed(sideWidth);
        leftPanel.LayoutProperties.Height = LayoutSize.Fixed(sideHeight);
        leftPanel.LayoutProperties.Margin = new Thickness(0, margin * 2, margin/2, 0);
        mainPanel.AddChild(leftPanel);

        // Right content panel
        var rightPanel = new Panel
        {
            BackgroundColor = new Color(40, 40, 50)
        };
        rightPanel.SetStackLayout(LayoutDirection.Vertical, margin);
        rightPanel.LayoutProperties.Width = LayoutSize.Fixed(sideWidth);
        rightPanel.LayoutProperties.Height = LayoutSize.Fixed(sideHeight);
        rightPanel.LayoutProperties.Margin = new Thickness(margin/2, margin * 2, 0, 0);
        mainPanel.AddChild(rightPanel);

        // Animation panel - use percentages
        var animationPanel = new AnimatedPanel
        {
            BackgroundColor = new Color(20, 20, 30)
        };
        animationPanel.LayoutProperties.Width = LayoutSize.Percentage(95);
        animationPanel.LayoutProperties.Height = LayoutSize.Percentage(85);
        animationPanel.LayoutProperties.Margin = new Thickness(10);
        rightPanel.AddChild(animationPanel);

        // Control buttons panel - update to use LayoutProperties
        var controlPanel = new Panel();
        controlPanel.SetStackLayout(LayoutDirection.Vertical, 5);
        controlPanel.LayoutProperties.Width = LayoutSize.Fixed(sideWidth - margin * 2);
        controlPanel.LayoutProperties.Height = LayoutSize.Fixed(sideHeight - margin * 2);
        controlPanel.LayoutProperties.Margin = new Thickness(margin);
        leftPanel.AddChild(controlPanel);
        
        // Control buttons
        string[] controlButtonText = { "Reset Animation", "Randomize Colors", "Toggle Speed" };
        Action[] buttonActions =
        {
            () => animationPanel.ResetAnimation(),
            () => animationPanel.RandomizeColors(),
            () => animationPanel.ToggleSpeed()
        };

        for (int i = 0; i < controlButtonText.Length; i++)
        {
            var button = new Button(
                new RECT(0, 0, 0, 35),
                text: controlButtonText[i],
                textColor: buttonTextColor,
                backgroundColor: buttonBgColor,
                hoverColor: buttonHoverColor,
                pressedColor: buttonPressedColor,
                borderColor: buttonBorderColor,
                borderHoverColor: buttonBorderHoverColor
            );

            button.LayoutProperties.Height = LayoutSize.Fixed(35);
            button.LayoutProperties.Width = LayoutSize.Star(1);  // Use Star sizing
            button.LayoutProperties.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.LayoutProperties.VerticalAlignment = VerticalAlignment.Top;
            button.LayoutProperties.Margin = new Thickness(0, 0, 0, 5);

            int index = i;
            button.OnClick += buttonActions[index];
            controlPanel.Children.Add(button);
        }

        // Bottom test buttons - update to use LayoutProperties
        var testBoundsButton = new Button(
            new RECT(0, 0, 0, 35),
            "Test Bounds",
            buttonTextColor,
            buttonBgColor,
            buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        testBoundsButton.LayoutProperties.Width = LayoutSize.Fixed(sideWidth - margin * 2);
        testBoundsButton.LayoutProperties.Height = LayoutSize.Fixed(35);
        testBoundsButton.LayoutProperties.VerticalAlignment = VerticalAlignment.Bottom;
        testBoundsButton.LayoutProperties.Margin = new Thickness(margin, 0, margin, margin);
        leftPanel.AddChild(testBoundsButton);

        var testRenderingButton = new Button(
            new RECT(0, 0, 0, 35),
            "Test Rendering",
            buttonTextColor,
            buttonBgColor,
            buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        testRenderingButton.LayoutProperties.Width = LayoutSize.Fixed(sideWidth - margin * 2);
        testRenderingButton.LayoutProperties.Height = LayoutSize.Fixed(35);
        testRenderingButton.LayoutProperties.VerticalAlignment = VerticalAlignment.Bottom;
        testRenderingButton.LayoutProperties.Margin = new Thickness(margin, 0, margin, margin);
        rightPanel.AddChild(testRenderingButton);

        // Set stack layout for left and right panels to properly arrange their children
        leftPanel.SetStackLayout(LayoutDirection.Vertical, margin);
        rightPanel.SetStackLayout(LayoutDirection.Vertical, margin);
    }
}

public class AnimatedPanel : Panel
{
    private double xPos = 0;
    private int xDirection = 1;
    private Color ballColor = new Color(255, 165, 0);  // Orange
    private Color lineColor = new Color(0, 255, 0);    // Green
    private int speed = 2;

    public override void CollectRenderCommands(RenderQueue queue)
    {
        base.CollectRenderCommands(queue);  // Draw panel background first

        var bounds = GetAbsoluteBounds();
        Console.WriteLine($"AnimatedPanel bounds: {bounds.X},{bounds.Y} {bounds.Width}x{bounds.Height}");

        // Draw border
        queue.Enqueue(new DrawRectCommand(1, bounds, new Color(100, 100, 100)));

        // Draw cross pattern
        queue.Enqueue(new DrawLineCommand(2,
            new POINT(bounds.X, bounds.Y),
            new POINT(bounds.X + bounds.Width, bounds.Y + bounds.Height),
            lineColor));

        queue.Enqueue(new DrawLineCommand(2,
            new POINT(bounds.X + bounds.Width, bounds.Y),
            new POINT(bounds.X, bounds.Y + bounds.Height),
            lineColor));

        // Draw bouncing ball
        double ballSize = Math.Min(40, Math.Min(bounds.Width, bounds.Height) * 0.1);  // Scale with panel size
        double ballY = bounds.Y + (bounds.Height - ballSize) / 2;
        queue.Enqueue(new FillRectCommand(3,
            new RECT(bounds.X + xPos, ballY, ballSize, ballSize),
            ballColor));

        // Update ball position for next frame
        UpdateBallPosition(bounds.Width, ballSize);
    }

    private void UpdateBallPosition(double width, double ballSize)
    {
        xPos += speed * xDirection;
        if (xPos >= width - ballSize || xPos <= 0)
        {
            xDirection *= -1;
            xPos = Math.Clamp(xPos, 0, width - ballSize); // Enforce bounds
        }
    }

    public void ResetAnimation()
    {
        xPos = 0;
        xDirection = 1;
        speed = 2;
    }

    public void RandomizeColors()
    {
        ballColor = new Color(
            (byte)Random.Shared.Next(128, 255),
            (byte)Random.Shared.Next(128, 255),
            (byte)Random.Shared.Next(128, 255)
        );

        lineColor = new Color(
            (byte)Random.Shared.Next(128, 255),
            (byte)Random.Shared.Next(128, 255),
            (byte)Random.Shared.Next(128, 255)
        );
    }

    public void ToggleSpeed()
    {
        speed = speed == 2 ? 4 : 2;
    }
}