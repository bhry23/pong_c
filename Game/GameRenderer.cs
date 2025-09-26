namespace PongGame.Game;

public class GameRenderer
{
    private readonly int width;
    private readonly int height;
    private readonly char[,] buffer;
    private readonly ConsoleColor[,] colorBuffer;
    private readonly ConsoleColor[,] backgroundBuffer;

    private const ConsoleColor PADDLE_COLOR = ConsoleColor.Cyan;
    private const ConsoleColor BALL_COLOR = ConsoleColor.Yellow;
    private const ConsoleColor CENTER_LINE_COLOR = ConsoleColor.DarkGray;
    private const ConsoleColor SCORE_COLOR = ConsoleColor.White;
    private const ConsoleColor BORDER_COLOR = ConsoleColor.Blue;
    private const ConsoleColor CONTROLS_COLOR = ConsoleColor.Gray;

    public GameRenderer(int width, int height)
    {
        this.width = width;
        this.height = height;
        buffer = new char[height, width];
        colorBuffer = new ConsoleColor[height, width];
        backgroundBuffer = new ConsoleColor[height, width];
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                buffer[y, x] = ' ';
                colorBuffer[y, x] = ConsoleColor.Gray;
                backgroundBuffer[y, x] = ConsoleColor.Black;
            }
        }
        DrawBorder();
    }

    public void DrawPaddle(Paddle paddle)
    {
        DrawPaddle(paddle, PADDLE_COLOR);
    }

    public void DrawPaddle(Paddle paddle, ConsoleColor color)
    {
        for (int i = 0; i < Paddle.HEIGHT; i++)
        {
            int y = paddle.Y + i;
            if (y >= 1 && y < height - 1 && paddle.X >= 1 && paddle.X < width - 1)
            {
                buffer[y, paddle.X] = '█';
                colorBuffer[y, paddle.X] = color;
                backgroundBuffer[y, paddle.X] = ConsoleColor.Black;
            }
        }
    }

    public void DrawBall(Ball ball)
    {
        int x = (int)Math.Round(ball.X);
        int y = (int)Math.Round(ball.Y);

        if (x >= 1 && x < width - 1 && y >= 1 && y < height - 1)
        {
            buffer[y, x] = '●';
            colorBuffer[y, x] = BALL_COLOR;
            backgroundBuffer[y, x] = ConsoleColor.Black;
        }
    }

    public void DrawCenterLine()
    {
        int centerX = width / 2;
        for (int y = 1; y < height - 1; y++)
        {
            if (centerX >= 0 && centerX < width)
            {
                if (y % 3 == 0)
                {
                    buffer[y, centerX] = '┊';
                    colorBuffer[y, centerX] = CENTER_LINE_COLOR;
                    backgroundBuffer[y, centerX] = ConsoleColor.Black;
                }
            }
        }
    }

    public void DrawScore(int leftScore, int rightScore)
    {
        string leftScoreText = $"Player 1: {leftScore}";
        string rightScoreText = $"Player 2: {rightScore}";

        DrawText(leftScoreText, 2, 0, SCORE_COLOR);
        DrawText(rightScoreText, width - rightScoreText.Length - 2, 0, SCORE_COLOR);
    }

    public void DrawControls()
    {
        string controls = "W/S: Left | ↑/↓: Right | R: Reset | ESC: Quit";
        int x = (width - controls.Length) / 2;
        DrawText(controls, x, height - 1, CONTROLS_COLOR);
    }

    private void DrawText(string text, int x, int y)
    {
        DrawText(text, x, y, ConsoleColor.Gray);
    }

    private void DrawText(string text, int x, int y, ConsoleColor color)
    {
        for (int i = 0; i < text.Length; i++)
        {
            int posX = x + i;
            if (posX >= 0 && posX < width && y >= 0 && y < height)
            {
                buffer[y, posX] = text[i];
                colorBuffer[y, posX] = color;
                backgroundBuffer[y, posX] = ConsoleColor.Black;
            }
        }
    }

    private void DrawBorder()
    {
        for (int x = 0; x < width; x++)
        {
            buffer[1, x] = '═';
            colorBuffer[1, x] = BORDER_COLOR;
            backgroundBuffer[1, x] = ConsoleColor.Black;

            buffer[height - 2, x] = '═';
            colorBuffer[height - 2, x] = BORDER_COLOR;
            backgroundBuffer[height - 2, x] = ConsoleColor.Black;
        }

        for (int y = 1; y < height - 1; y++)
        {
            buffer[y, 0] = '║';
            colorBuffer[y, 0] = BORDER_COLOR;
            backgroundBuffer[y, 0] = ConsoleColor.Black;

            buffer[y, width - 1] = '║';
            colorBuffer[y, width - 1] = BORDER_COLOR;
            backgroundBuffer[y, width - 1] = ConsoleColor.Black;
        }

        buffer[1, 0] = '╔';
        buffer[1, width - 1] = '╗';
        buffer[height - 2, 0] = '╚';
        buffer[height - 2, width - 1] = '╝';

        colorBuffer[1, 0] = BORDER_COLOR;
        colorBuffer[1, width - 1] = BORDER_COLOR;
        colorBuffer[height - 2, 0] = BORDER_COLOR;
        colorBuffer[height - 2, width - 1] = BORDER_COLOR;

        backgroundBuffer[1, 0] = ConsoleColor.Black;
        backgroundBuffer[1, width - 1] = ConsoleColor.Black;
        backgroundBuffer[height - 2, 0] = ConsoleColor.Black;
        backgroundBuffer[height - 2, width - 1] = ConsoleColor.Black;
    }

    public void Display()
    {
        Console.SetCursorPosition(0, 0);
        ConsoleColor currentForeground = ConsoleColor.Gray;
        ConsoleColor currentBackground = ConsoleColor.Black;

        Console.ForegroundColor = currentForeground;
        Console.BackgroundColor = currentBackground;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (colorBuffer[y, x] != currentForeground || backgroundBuffer[y, x] != currentBackground)
                {
                    currentForeground = colorBuffer[y, x];
                    currentBackground = backgroundBuffer[y, x];
                    Console.ForegroundColor = currentForeground;
                    Console.BackgroundColor = currentBackground;
                }
                Console.Write(buffer[y, x]);
            }
            if (y < height - 1)
                Console.WriteLine();
        }

        Console.ResetColor();
    }

    public void DrawScoringEffect(int x, int y)
    {
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int posX = x + dx;
                int posY = y + dy;
                if (posX >= 1 && posX < width - 1 && posY >= 2 && posY < height - 2)
                {
                    buffer[posY, posX] = '★';
                    colorBuffer[posY, posX] = ConsoleColor.Green;
                    backgroundBuffer[posY, posX] = ConsoleColor.Black;
                }
            }
        }
    }

    public void DrawBallTrail(Ball ball, double previousX, double previousY)
    {
        int prevX = (int)Math.Round(previousX);
        int prevY = (int)Math.Round(previousY);

        if (prevX >= 1 && prevX < width - 1 && prevY >= 2 && prevY < height - 2)
        {
            if (buffer[prevY, prevX] == ' ')
            {
                buffer[prevY, prevX] = '·';
                colorBuffer[prevY, prevX] = ConsoleColor.DarkYellow;
                backgroundBuffer[prevY, prevX] = ConsoleColor.Black;
            }
        }
    }
}