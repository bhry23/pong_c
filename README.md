# Pong Game in C#

A console-based implementation of the classic Pong game built with .NET 9.0 for cross-platform compatibility.

## Features

- 🎮 Two-player gameplay
- 🖥️ Cross-platform compatibility (Windows, macOS, Linux)
- ⚡ Real-time physics and collision detection
- 🏆 Score tracking
- 🎨 Console-based graphics with smooth rendering

## Controls

- **Player 1 (Left Paddle)**: W/S keys
- **Player 2 (Right Paddle)**: ↑/↓ arrow keys
- **R**: Reset game
- **ESC**: Quit game

## Requirements

- .NET 9.0 SDK or later

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/bhry23/pong_c.git
   cd pong_c
   ```

2. Build and run the game:
   ```bash
   dotnet run
   ```

## Game Architecture

The game is built with an object-oriented design:

- **`Program.cs`**: Entry point
- **`Game/Pong.cs`**: Main game logic and loop
- **`Game/Paddle.cs`**: Paddle object with movement controls
- **`Game/Ball.cs`**: Ball physics and collision detection
- **`Game/GameRenderer.cs`**: Console rendering system

## Cross-Platform Compatibility

The game uses platform-specific console features only when available, ensuring it runs smoothly on:
- Windows
- macOS
- Linux

Built with .NET 9.0 for optimal cross-platform performance.
