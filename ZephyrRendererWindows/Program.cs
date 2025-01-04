using ZephyrRenderer;
using ZephyrRenderer.Platform;
using ZephyrRenderer.UIElement;

class Program
{
    static void Main()
    {
        // Create our main window
        var window = new Window("Zephyr UI Demo", 800, 600);

        // Calculate center position for main panel
        double mainPanelWidth = 700;
        double mainPanelHeight = 500;
        double mainPanelX = (800 - mainPanelWidth) / 2;
        double mainPanelY = (600 - mainPanelHeight) / 2;

        // Create main panel
        var mainPanel = new Panel
        {
            Bounds = new RECT { 
                X = mainPanelX, 
                Y = mainPanelY, 
                Width = mainPanelWidth, 
                Height = mainPanelHeight 
            },
            BackgroundColor = new Color(30, 30, 40)
        };
        window.AddChild(mainPanel);

        // Calculate animation panel size (60% of main panel width)
        double animPanelWidth = mainPanelWidth * 0.6;
        double animPanelHeight = mainPanelHeight * 0.8;
        double animPanelX = 20; // Margin from left
        double animPanelY = (mainPanelHeight - animPanelHeight) / 2; // Vertical center

        // Add animation panel
        var animationPanel = new AnimatedPanel
        {
            Bounds = new RECT { 
                X = animPanelX, 
                Y = animPanelY, 
                Width = animPanelWidth, 
                Height = animPanelHeight 
            },
            BackgroundColor = new Color(20, 20, 30)
        };
        mainPanel.AddChild(animationPanel);

        // Calculate button panel size
        double buttonPanelWidth = mainPanelWidth * 0.3;
        double buttonPanelHeight = mainPanelHeight * 0.8;
        double buttonPanelX = animPanelX + animPanelWidth + 40; // After animation panel + spacing
        double buttonPanelY = (mainPanelHeight - buttonPanelHeight) / 2; // Vertical center

        // Add button panel
        var buttonPanel = new Panel
        {
            Bounds = new RECT { 
                X = buttonPanelX, 
                Y = buttonPanelY, 
                Width = buttonPanelWidth, 
                Height = buttonPanelHeight 
            },
            BackgroundColor = new Color(40, 40, 50)
        };
        mainPanel.AddChild(buttonPanel);

        // Button properties
        var buttonTextColor = new Color(240, 240, 240);
        var buttonBgColor = new Color(60, 60, 70);
        var buttonHoverColor = new Color(70, 70, 80);
        var buttonPressedColor = new Color(50, 50, 60);
        var buttonBorderColor = new Color(80, 80, 90);
        var buttonBorderHoverColor = new Color(100, 100, 110);

        // Button dimensions
        double buttonWidth = buttonPanelWidth * 0.8;
        double buttonHeight = 40;
        double buttonSpacing = 30;
        double totalButtonsHeight = (buttonHeight * 3) + (buttonSpacing * 2);
        double buttonStartY = (buttonPanelHeight - totalButtonsHeight) / 2;
        double buttonX = (buttonPanelWidth - buttonWidth) / 2;

        // Create buttons
        var resetButton = new Button(
            bounds: new RECT { 
                X = buttonX, 
                Y = buttonStartY, 
                Width = buttonWidth, 
                Height = buttonHeight 
            },
            text: "RESET ANIMATION",
            textColor: buttonTextColor,
            backgroundColor: buttonBgColor,
            hoverColor: buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        resetButton.OnClick += () => animationPanel.ResetAnimation();
        buttonPanel.AddChild(resetButton);

        var colorButton = new Button(
            bounds: new RECT { 
                X = buttonX, 
                Y = buttonStartY + buttonHeight + buttonSpacing, 
                Width = buttonWidth, 
                Height = buttonHeight 
            },
            text: "CHANGE COLORS",
            textColor: buttonTextColor,
            backgroundColor: buttonBgColor,
            hoverColor: buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        colorButton.OnClick += () => animationPanel.RandomizeColors();
        buttonPanel.AddChild(colorButton);

        var speedButton = new Button(
            bounds: new RECT { 
                X = buttonX, 
                Y = buttonStartY + (buttonHeight + buttonSpacing) * 2, 
                Width = buttonWidth, 
                Height = buttonHeight 
            },
            text: "TOGGLE SPEED",
            textColor: buttonTextColor,
            backgroundColor: buttonBgColor,
            hoverColor: buttonHoverColor,
            pressedColor: buttonPressedColor,
            borderColor: buttonBorderColor,
            borderHoverColor: buttonBorderHoverColor
        );
        speedButton.OnClick += () => animationPanel.ToggleSpeed();
        buttonPanel.AddChild(speedButton);

        window.Run();
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