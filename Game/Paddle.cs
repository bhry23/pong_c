namespace PongGame.Game;

public class Paddle
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public const int HEIGHT = 5;
    public const int WIDTH = 1;

    public Paddle(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void MoveUp()
    {
        if (Y > 1)
            Y--;
    }

    public void MoveDown(int consoleHeight)
    {
        if (Y < consoleHeight - HEIGHT - 1)
            Y++;
    }

    public void Reset(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsWithinBounds(int ballY)
    {
        return ballY >= Y && ballY <= Y + HEIGHT;
    }
}