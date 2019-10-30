namespace System {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            Console.WriteLine("Enter to start.");
            Console.ReadLine();
            Game.Start();
        }
    }

    public static class Game {
        static int ballX, ballY, leftPaddleX, leftPaddleY, rghtPaddleX, rghtPaddleY, leftScore, rghtScore, ballMotionX, ballMotionY, leftPaddleHeight, rghtPaddleHeight;
        static int PlayWid => Console.WindowWidth;
        static int PlayHgt => Console.WindowHeight - 1;

        public static void Start() {
            ballX = PlayHgt / 2; ballY = PlayHgt / 2;
            ballMotionX = 1; ballMotionY = 1;
            leftPaddleX = 2; leftPaddleY = PlayHgt / 2; leftPaddleHeight = 10; leftScore = 0;
            rghtPaddleX = PlayWid - 3; rghtPaddleY = PlayHgt / 2; rghtPaddleHeight = 10; rghtScore = 0;

            while (true) {
                Move();
                Collide();
                Draw();
                Threading.Thread.Sleep(1000 / 30);
            }
        }

        static void Move() {
            if ((Windows.Input.Keyboard.GetKeyStates(Windows.Input.Key.S) & Windows.Input.KeyStates.Down) > 0) leftPaddleY++;
            if ((Windows.Input.Keyboard.GetKeyStates(Windows.Input.Key.W) & Windows.Input.KeyStates.Down) > 0) leftPaddleY--;
            if ((Windows.Input.Keyboard.GetKeyStates(Windows.Input.Key.Down) & Windows.Input.KeyStates.Down) > 0) rghtPaddleY++;
            if ((Windows.Input.Keyboard.GetKeyStates(Windows.Input.Key.Up) & Windows.Input.KeyStates.Down) > 0) rghtPaddleY--;

            ballX += ballMotionX;
            ballY += ballMotionY;
        }

        static void Collide() {
            if (leftPaddleY <= 0) leftPaddleY = 1;
            if (rghtPaddleY <= 0) rghtPaddleY = 1;
            if (leftPaddleY + leftPaddleHeight >= PlayHgt) leftPaddleY = PlayHgt - leftPaddleHeight;
            if (rghtPaddleY + rghtPaddleHeight >= PlayHgt) rghtPaddleY = PlayHgt - rghtPaddleHeight;
            if (ballX <= leftPaddleX + 1 && ballY > leftPaddleY && ballY < leftPaddleY + leftPaddleHeight) { ballMotionX *= -1; }
            if (ballX >= rghtPaddleX - 1 && ballY > rghtPaddleY && ballY < rghtPaddleY + rghtPaddleHeight) { ballMotionX *= -1; }
            if (ballY <= 0) { ballMotionY *= -1; ballY = 1; }
            if (ballY + 1 >= PlayHgt) { ballMotionY *= -1; ballY = PlayHgt - 1; }
            if (ballX < 0) { ballX = PlayWid / 2; ballY = PlayHgt / 2; ballMotionX = 1; ballMotionY = 1; rghtScore++; }
            if (ballX > PlayWid) { ballX = PlayWid / 2; ballY = PlayHgt / 2; ballMotionX = 1; ballMotionY = 1; leftScore++; }
        }

        static void Draw() {
            var screen = new Text.StringBuilder(new string(' ', PlayWid * PlayHgt));
            for (int i = 0; i < leftPaddleHeight; i++) screen[(leftPaddleY + i) * PlayWid + leftPaddleX] = '█';
            for (int i = 0; i < rghtPaddleHeight; i++) screen[(rghtPaddleY + i) * PlayWid + rghtPaddleX] = '█';
            screen[ballY * PlayWid + ballX] = '█';

            string s = leftScore.ToString();
            screen.Replace(new string(' ', s.Length), s, 1, s.Length);
            string t = rghtScore.ToString();
            screen.Replace(new string(' ', t.Length), t, PlayWid - t.Length - 1, t.Length);

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(screen);
        }
    }
}