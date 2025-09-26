namespace PongGame.Game;

public class GameRenderer
{
    private readonly int width;
    private readonly int height;
    private readonly char[,] buffer;

    public GameRenderer(int width, int height)
    {
        this.width = width;
        this.height = height;
        buffer = new char[height, width];
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                buffer[y, x] = ' ';
            }
        }
    }

    public void DrawPaddle(Paddle paddle)
    {
        for (int i = 0; i < Paddle.HEIGHT; i++)
        {
            int y = paddle.Y + i;
            if (y >= 0 && y < height && paddle.X >= 0 && paddle.X < width)
            {
                buffer[y, paddle.X] = '█';
            }
        }
    }

    public void DrawBall(Ball ball)
    {
        int x = (int)Math.Round(ball.X);
        int y = (int)Math.Round(ball.Y);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            buffer[y, x] = 'O';
        }
    }

    public void DrawCenterLine()
    {
        int centerX = width / 2;
        for (int y = 0; y < height; y++)
        {
            if (centerX >= 0 && centerX < width)
            {
                buffer[y, centerX] = y % 2 == 0 ? '|' : ' ';
            }
        }
    }

    public void DrawScore(int leftScore, int rightScore)
    {
        string leftScoreText = $"Player 1: {leftScore}";
        string rightScoreText = $"Player 2: {rightScore}";

        DrawText(leftScoreText, 2, 1);
        DrawText(rightScoreText, width - rightScoreText.Length - 2, 1);
    }

    public void DrawControls()
    {
        string controls = "W/S: Left | ↑/↓: Right | R: Reset | ESC: Quit";
        int x = (width - controls.Length) / 2;
        DrawText(controls, x, height - 2);
    }

    private void DrawText(string text, int x, int y)
    {
        for (int i = 0; i < text.Length; i++)
        {
            int posX = x + i;
            if (posX >= 0 && posX < width && y >= 0 && y < height)
            {
                buffer[y, posX] = text[i];
            }
        }
    }

    public void Display()
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(buffer[y, x]);
            }
            if (y < height - 1)
                Console.WriteLine();
        }
    }
}