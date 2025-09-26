namespace PongGame.Game;

public class Pong
{
    private const int CONSOLE_WIDTH = 80;
    private const int CONSOLE_HEIGHT = 25;

    private readonly Paddle leftPaddle;
    private readonly Paddle rightPaddle;
    private readonly Ball ball;
    private readonly GameRenderer renderer;
    private readonly SoundManager soundManager;
    private int leftScore;
    private int rightScore;
    private bool gameRunning;
    private int leftPaddleHighlight;
    private int rightPaddleHighlight;
    private int scoringEffectTimer;

    public Pong()
    {
        if (OperatingSystem.IsWindows())
        {
            Console.SetWindowSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
            Console.SetBufferSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
        }
        Console.CursorVisible = false;
        Console.Title = "Pong Game - C#";

        leftPaddle = new Paddle(3, CONSOLE_HEIGHT / 2 - 2);
        rightPaddle = new Paddle(CONSOLE_WIDTH - 4, CONSOLE_HEIGHT / 2 - 2);
        ball = new Ball(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2);
        renderer = new GameRenderer(CONSOLE_WIDTH, CONSOLE_HEIGHT);
        soundManager = new SoundManager();

        leftScore = 0;
        rightScore = 0;
        gameRunning = true;
        leftPaddleHighlight = 0;
        rightPaddleHighlight = 0;
        scoringEffectTimer = 0;

        soundManager.PlayGameStart();
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

        if (ball.Y <= 1 || ball.Y >= CONSOLE_HEIGHT - 3)
        {
            ball.BounceVertical();
            soundManager.PlayWallHit();
        }

        if (ball.CheckPaddleCollision(leftPaddle))
        {
            ball.BounceHorizontal();
            leftPaddleHighlight = 5;
            soundManager.PlayPaddleHit();
        }
        else if (ball.CheckPaddleCollision(rightPaddle))
        {
            ball.BounceHorizontal();
            rightPaddleHighlight = 5;
            soundManager.PlayPaddleHit();
        }

        if (ball.X <= 1)
        {
            rightScore++;
            scoringEffectTimer = 10;
            soundManager.PlayScoreSequence();
            ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, -1);
        }

        if (ball.X >= CONSOLE_WIDTH - 2)
        {
            leftScore++;
            scoringEffectTimer = 10;
            soundManager.PlayScoreSequence();
            ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, 1);
        }

        if (leftPaddleHighlight > 0) leftPaddleHighlight--;
        if (rightPaddleHighlight > 0) rightPaddleHighlight--;
        if (scoringEffectTimer > 0) scoringEffectTimer--;
    }

    private void Render()
    {
        renderer.Clear();
        renderer.DrawBallTrail(ball, ball.PreviousX, ball.PreviousY);

        ConsoleColor leftColor = leftPaddleHighlight > 0 ? ConsoleColor.White : ConsoleColor.Cyan;
        ConsoleColor rightColor = rightPaddleHighlight > 0 ? ConsoleColor.White : ConsoleColor.Cyan;

        renderer.DrawPaddle(leftPaddle, leftColor);
        renderer.DrawPaddle(rightPaddle, rightColor);
        renderer.DrawBall(ball);
        renderer.DrawCenterLine();
        renderer.DrawScore(leftScore, rightScore);
        renderer.DrawControls();

        if (scoringEffectTimer > 0)
        {
            renderer.DrawScoringEffect(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2);
        }

        renderer.Display();
    }

    private void Reset()
    {
        leftScore = 0;
        rightScore = 0;
        leftPaddleHighlight = 0;
        rightPaddleHighlight = 0;
        scoringEffectTimer = 0;
        leftPaddle.Reset(3, CONSOLE_HEIGHT / 2 - 2);
        rightPaddle.Reset(CONSOLE_WIDTH - 4, CONSOLE_HEIGHT / 2 - 2);
        ball.Reset(CONSOLE_WIDTH / 2, CONSOLE_HEIGHT / 2, 1);
    }
}