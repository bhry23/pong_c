namespace PongGame.Game;

public class Pong
{
    private const int CONSOLE_WIDTH = 80;
    private const int CONSOLE_HEIGHT = 25;

    private Paddle leftPaddle;
    private Paddle rightPaddle;
    private Ball ball;
    private GameRenderer renderer;
    private int leftScore;
    private int rightScore;
    private bool gameRunning;

    public Pong()
    {
        if (OperatingSystem.IsWindows())
        {
            Console.SetWindowSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
            Console.SetBufferSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
        }
        Console.CursorVisible = false;
        Console.Title = "Pong Game - C#";

        leftPaddle = new Paddle(2, CONSOLE_HEIGHT / 2 - 2);
        rightPaddle = new Paddle(CONSOLE_WIDTH - 3, CONSOLE_HEIGHT / 2 - 2);
        ball = new Ball(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2);
        renderer = new GameRenderer(CONSOLE_WIDTH, CONSOLE_HEIGHT);

        leftScore = 0;
        rightScore = 0;
        gameRunning = true;
    }

    public void Run()
    {
        while (gameRunning)
        {
            HandleInput();
            Update();
            Render();
            Thread.Sleep(50);
        }

        Console.Clear();
        Console.WriteLine("Thanks for playing Pong!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private void HandleInput()
    {
        try
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        leftPaddle.MoveUp();
                        break;
                    case ConsoleKey.S:
                        leftPaddle.MoveDown(CONSOLE_HEIGHT);
                        break;
                    case ConsoleKey.UpArrow:
                        rightPaddle.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        rightPaddle.MoveDown(CONSOLE_HEIGHT);
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        break;
                    case ConsoleKey.R:
                        Reset();
                        break;
                }
            }
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void Update()
    {
        ball.Update();

        if (ball.Y <= 0 || ball.Y >= CONSOLE_HEIGHT - 1)
        {
            ball.BounceVertical();
        }

        if (ball.CheckPaddleCollision(leftPaddle) || ball.CheckPaddleCollision(rightPaddle))
        {
            ball.BounceHorizontal();
        }

        if (ball.X <= 0)
        {
            rightScore++;
            ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, -1);
        }

        if (ball.X >= CONSOLE_WIDTH - 1)
        {
            leftScore++;
            ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, 1);
        }
    }

    private void Render()
    {
        renderer.Clear();
        renderer.DrawPaddle(leftPaddle);
        renderer.DrawPaddle(rightPaddle);
        renderer.DrawBall(ball);
        renderer.DrawCenterLine();
        renderer.DrawScore(leftScore, rightScore);
        renderer.DrawControls();
        renderer.Display();
    }

    private void Reset()
    {
        leftScore = 0;
        rightScore = 0;
        leftPaddle.Reset(2, CONSOLE_HEIGHT / 2 - 2);
        rightPaddle.Reset(CONSOLE_WIDTH - 3, CONSOLE_HEIGHT / 2 - 2);
        ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, 1);
    }
}