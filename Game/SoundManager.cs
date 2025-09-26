using System.Runtime.Versioning;

namespace PongGame.Game;

public class SoundManager
{
    private const int PADDLE_HIT_FREQUENCY = 800;
    private const int PADDLE_HIT_DURATION = 100;

    private const int WALL_HIT_FREQUENCY = 600;
    private const int WALL_HIT_DURATION = 150;

    private const int SCORE_FREQUENCY = 1000;
    private const int SCORE_DURATION = 300;

    private const int GAME_START_FREQUENCY = 440;
    private const int GAME_START_DURATION = 200;

    private readonly bool soundEnabled;
    private readonly bool isMacOS;
    private readonly bool isWindows;

    public SoundManager()
    {
        isWindows = OperatingSystem.IsWindows();
        isMacOS = OperatingSystem.IsMacOS();
        soundEnabled = isWindows || isMacOS;

        if (isMacOS)
        {
            InitializeMacOSAudio();
        }
    }

    public void PlayPaddleHit()
    {
        PlaySound(PADDLE_HIT_FREQUENCY, PADDLE_HIT_DURATION);
    }

    public void PlayWallHit()
    {
        PlaySound(WALL_HIT_FREQUENCY, WALL_HIT_DURATION);
    }

    public void PlayScore()
    {
        PlaySound(SCORE_FREQUENCY, SCORE_DURATION);
    }

    public void PlayGameStart()
    {
        PlaySound(GAME_START_FREQUENCY, GAME_START_DURATION);
    }

    public void PlayScoreSequence()
    {
        if (!soundEnabled) return;

        Task.Run(() =>
        {
            try
            {
                if (isWindows)
                {
                    PlayWindowsBeep(800, 150);
                    Thread.Sleep(50);
                    PlayWindowsBeep(1000, 150);
                    Thread.Sleep(50);
                    PlayWindowsBeep(1200, 200);
                }
                else if (isMacOS)
                {
                    PlayMacOSBeep(800, 150);
                    Thread.Sleep(50);
                    PlayMacOSBeep(1000, 150);
                    Thread.Sleep(50);
                    PlayMacOSBeep(1200, 200);
                }
            }
            catch (Exception)
            {
            }
        });
    }

    private void PlaySound(int frequency, int duration)
    {
        if (!soundEnabled) return;

        Task.Run(() =>
        {
            try
            {
                if (isWindows)
                {
                    PlayWindowsBeep(frequency, duration);
                }
                else if (isMacOS)
                {
                    PlayMacOSBeep(frequency, duration);
                }
            }
            catch (Exception)
            {
            }
        });
    }

    private void InitializeMacOSAudio()
    {
        try
        {
            Directory.CreateDirectory("/tmp/pong_sounds");
        }
        catch (Exception)
        {
        }
    }

    private void PlayMacOSBeep(int frequency, int durationMs)
    {
        try
        {
            double duration = durationMs / 1000.0;
            string tempFile = $"/tmp/pong_sounds/beep_{frequency}_{durationMs}.wav";

            if (!File.Exists(tempFile))
            {
                GenerateWaveFile(tempFile, frequency, duration);
            }

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "afplay",
                    Arguments = tempFile,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
        }
        catch (Exception)
        {
        }
    }

    private void GenerateWaveFile(string filePath, int frequency, double duration)
    {
        const int sampleRate = 44100;
        const short bitsPerSample = 16;
        const short channels = 1;

        int totalSamples = (int)(sampleRate * duration);
        byte[] waveData = new byte[totalSamples * 2];

        for (int i = 0; i < totalSamples; i++)
        {
            double time = (double)i / sampleRate;
            double amplitude = Math.Sin(2 * Math.PI * frequency * time);
            short sample = (short)(amplitude * short.MaxValue * 0.3);

            byte[] sampleBytes = BitConverter.GetBytes(sample);
            waveData[i * 2] = sampleBytes[0];
            waveData[i * 2 + 1] = sampleBytes[1];
        }

        using var fileStream = new FileStream(filePath, FileMode.Create);
        using var writer = new BinaryWriter(fileStream);

        writer.Write("RIFF".ToCharArray());
        writer.Write(36 + waveData.Length);
        writer.Write("WAVE".ToCharArray());
        writer.Write("fmt ".ToCharArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write(channels);
        writer.Write(sampleRate);
        writer.Write(sampleRate * channels * bitsPerSample / 8);
        writer.Write((short)(channels * bitsPerSample / 8));
        writer.Write(bitsPerSample);
        writer.Write("data".ToCharArray());
        writer.Write(waveData.Length);
        writer.Write(waveData);
    }

    [SupportedOSPlatform("windows")]
    private static void PlayWindowsBeep(int frequency, int duration)
    {
        Console.Beep(frequency, duration);
    }
}