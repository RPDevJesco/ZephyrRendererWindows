using ZephyrRenderer;
using ZephyrRenderer.Platform;
using ZephyrRenderer.UIElement;

class Program
{
    static void Main()
    {
        // Create our main window
        var window = new Window("Zephyr UI Demo - Batched Rendering", 1200, 1200);
        
        // Create a test scene with different UI elements to demonstrate the batched rendering
        CreateTestScene(window);

        window.Run();
    }

    static void CreateTestScene(Window window)
    {
        // Main panel covering most of the window
        var mainPanel = new Panel
        {
            Bounds = new RECT(50, 50, 700, 500), // Margin from window edges
            BackgroundColor = new Color(30, 30, 40)
        };
        window.AddChild(mainPanel);

        // Add a row of buttons at the top
        double buttonWidth = 120;
        double buttonHeight = 40;
        double buttonSpacing = 20;
        double startX = 20;
        double startY = 20;

        // Common button colors
        var buttonTextColor = new Color(240, 240, 240);
        var buttonBgColor = new Color(60, 60, 70);
        var buttonHoverColor = new Color(70, 70, 80);
        var buttonPressedColor = new Color(50, 50, 60);
        var buttonBorderColor = new Color(80, 80, 90);
        var buttonBorderHoverColor = new Color(100, 100, 110);

        // Create several buttons to test batch rendering
        for (int i = 0; i < 5; i++)
        {
            var button = new Button(
                bounds: new RECT(
                    startX + (buttonWidth + buttonSpacing) * i,
                    startY,
                    buttonWidth,
                    buttonHeight
                ),
                text: $"Button {i + 1}",
                textColor: buttonTextColor,
                backgroundColor: buttonBgColor,
                hoverColor: buttonHoverColor,
                pressedColor: buttonPressedColor,
                borderColor: buttonBorderColor,
                borderHoverColor: buttonBorderHoverColor
            );

            int buttonIndex = i; // Capture for lambda
            button.OnClick += () => Console.WriteLine($"Button {buttonIndex + 1} clicked!");
            mainPanel.AddChild(button);
        }

        // Add some nested panels to test clipping and bounds calculation
        var nestedPanel1 = new Panel
        {
            Bounds = new RECT(20, 80, 320, 380),
            BackgroundColor = new Color(40, 40, 50)
        };
        mainPanel.AddChild(nestedPanel1);

        var nestedPanel2 = new Panel
        {
            Bounds = new RECT(360, 80, 320, 380),
            BackgroundColor = new Color(40, 40, 50)
        };
        mainPanel.AddChild(nestedPanel2);

        // Add some buttons to the nested panels
        for (int i = 0; i < 3; i++)
        {
            var button1 = new Button(
                bounds: new RECT(
                    20,
                    20 + (buttonHeight + buttonSpacing) * i,
                    buttonWidth,
                    buttonHeight
                ),
                text: $"Left {i + 1}",
                textColor: buttonTextColor,
                backgroundColor: buttonBgColor,
                hoverColor: buttonHoverColor,
                pressedColor: buttonPressedColor,
                borderColor: buttonBorderColor,
                borderHoverColor: buttonBorderHoverColor
            );
            int button1Index = i; // Capture for lambda
            button1.OnClick += () => Console.WriteLine($"Left {button1Index + 1} clicked!");
            nestedPanel1.AddChild(button1);

            var button2 = new Button(
                bounds: new RECT(
                    20,
                    20 + (buttonHeight + buttonSpacing) * i,
                    buttonWidth,
                    buttonHeight
                ),
                text: $"Right {i + 1}",
                textColor: buttonTextColor,
                backgroundColor: buttonBgColor,
                hoverColor: buttonHoverColor,
                pressedColor: buttonPressedColor,
                borderColor: buttonBorderColor,
                borderHoverColor: buttonBorderHoverColor
            );
            int button2Index = i; // Capture for lambda
            button2.OnClick += () => Console.WriteLine($"Right {button2Index + 1} clicked!");
            nestedPanel2.AddChild(button2);
        }

        // Add some buttons at the bottom of the main panel
        var bottomButton1 = new Button(
            bounds: new RECT(20, 480 - buttonHeight, 200, buttonHeight),
            text: "Test Bounds",
            textColor: buttonTextColor,
            backgroundColor: buttonBgColor,
            hoverColor: buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        bottomButton1.OnClick += () => Console.WriteLine("Bounds Test!");
        mainPanel.AddChild(bottomButton1);

        var bottomButton2 = new Button(
            bounds: new RECT(480, 480 - buttonHeight, 200, buttonHeight),
            text: "Test Rendering",
            textColor: buttonTextColor,
            backgroundColor: buttonBgColor,
            hoverColor: buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        bottomButton2.OnClick += () => Console.WriteLine("Render Test!");
        mainPanel.AddChild(bottomButton2);
    }
}

public class AnimatedPanel : Panel
    {
        private double xPos = 0;
        private int xDirection = 1;
        private Color ballColor = new Color(255, 165, 0);
        private Color lineColor = new Color(0, 255, 0);
        private int speed = 2;

        protected override void OnDraw(Framebuffer framebuffer)
        {
            var bounds = GetAbsoluteBounds();
            
            // Draw panel background
            base.OnDraw(framebuffer);

            // Draw border
            framebuffer.DrawRect(
                bounds.X, 
                bounds.Y, 
                bounds.Width, 
                bounds.Height, 
                new Color(100, 100, 100)
            );

            // Draw cross pattern
            framebuffer.DrawLine(
                bounds.X, 
                bounds.Y, 
                bounds.X + bounds.Width, 
                bounds.Y + bounds.Height, 
                lineColor
            );
            framebuffer.DrawLine(
                bounds.X + bounds.Width, 
                bounds.Y, 
                bounds.X, 
                bounds.Y + bounds.Height, 
                lineColor
            );

            // Draw bouncing ball at vertical center
            double ballSize = 40;
            double ballY = bounds.Y + (bounds.Height - ballSize) / 2;
            framebuffer.FillRect(bounds.X + xPos, ballY, ballSize, ballSize, ballColor);

            // Update ball position
            xPos += speed * xDirection;
            if (xPos >= bounds.Width - ballSize || xPos <= 0)
            {
                xDirection *= -1;
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
                (byte)System.Random.Shared.Next(128, 255),
                (byte)System.Random.Shared.Next(128, 255),
                (byte)System.Random.Shared.Next(128, 255)
            );

            lineColor = new Color(
                (byte)System.Random.Shared.Next(128, 255),
                (byte)System.Random.Shared.Next(128, 255),
                (byte)System.Random.Shared.Next(128, 255)
            );
        }

        public void ToggleSpeed()
        {
            speed = speed == 2 ? 4 : 2;
        }
    }