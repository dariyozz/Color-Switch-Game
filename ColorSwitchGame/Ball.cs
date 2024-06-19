using System;
using System.Drawing;

namespace ColorSwitchGame
{
    public class Ball
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public Color Color { get; set; }
        private static readonly Color[] Colors = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        private Random random = new Random();

        public Ball(int startX, int startY, Color startColor)
        {
            X = startX;
            Y = startY;
            Speed = 0;
            Color = startColor;
        }

        public void Update()
        {
            Speed += 1; // Gravity
            Y += Speed;
        }

        public void Jump()
        {
            Speed = -10; // Jump
        }

        public void ChangeColor()
        {
            Color = Colors[random.Next(Colors.Length)];
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, new Rectangle(X - 10, Y - 10, 20, 20));
            }
        }
    }
}