namespace PongGame.Game;

public class Ball
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double PreviousX { get; private set; }
    public double PreviousY { get; private set; }
    private double speedX;
    private double speedY;
    private const double INITIAL_SPEED = 0.5;
    private const double SPEED_INCREASE = 1.05;

    public Ball(double x, double y)
    {
        Reset(x, y, 1);
    }

    public void Update()
    {
        PreviousX = X;
        PreviousY = Y;
        X += speedX;
        Y += speedY;
    }

    public void BounceVertical()
    {
        speedY = -speedY;
    }

    public void BounceHorizontal()
    {
        speedX = -speedX;
        speedX *= SPEED_INCREASE;
        speedY *= SPEED_INCREASE;
    }

    public bool CheckPaddleCollision(Paddle paddle)
    {
        int ballIntX = (int)Math.Round(X);
        int ballIntY = (int)Math.Round(Y);

        bool isAtPaddleX = (ballIntX == paddle.X - 1 && speedX > 0) ||
                           (ballIntX == paddle.X + 1 && speedX < 0);

        return isAtPaddleX && paddle.IsWithinBounds(ballIntY);
    }

    public void Reset(double x, double y, int direction)
    {
        X = x;
        Y = y;
        PreviousX = x;
        PreviousY = y;
        speedX = INITIAL_SPEED * direction;
        speedY = INITIAL_SPEED * (Random.Shared.NextDouble() - 0.5);
    }
}