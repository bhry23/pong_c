using PongGame.Game;

namespace PongGame;

class Program
{
    static void Main(string[] args)
    {
        var game = new Pong();
        game.Run();
    }
}