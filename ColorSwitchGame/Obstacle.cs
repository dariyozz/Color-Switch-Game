using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ColorSwitchGame
{
    public class Obstacle
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
        public int RotationAngle { get; set; }
        public Color[] Colors { get; set; }
        private const float PenWidth = 10f; // Width of the pen used to draw obstacle perimeter
        public Rectangle ScoreObject { get; set; }
        public bool ScoreObjectCollected { get; set; }

        public Obstacle(Point center, int radius, Color[] colors)
        {
            Center = center;
            Radius = radius;
            Colors = colors;
            RotationAngle = 0;
            ScoreObject = new Rectangle(Center.X - 10, Center.Y - 10, 20, 20);
            ScoreObjectCollected = false;
        }

        public void Update()
        {
            RotationAngle = (RotationAngle + 2) % 360; // Slow down the rotation speed
        }
        public static GraphicsPath CreateStarPath(float x, float y, float outerRadius, float innerRadius, int points)
        {
            double angle = Math.PI / points;
            GraphicsPath path = new GraphicsPath();
            PointF[] pts = new PointF[2 * points];
            for (int i = 0; i < 2 * points; i++)
            {
                double r = (i % 2 == 0) ? outerRadius : innerRadius;
                pts[i] = new PointF(
                    x + (float)(r * Math.Sin(i * angle)),
                    y - (float)(r * Math.Cos(i * angle)));
            }
            path.AddPolygon(pts);
            return path;
        }
        public void Draw(Graphics g)
        {
            float sweepAngle = 360f / Colors.Length;
            for (int i = 0; i < Colors.Length; i++)
            {
                using (Pen pen = new Pen(Colors[i], PenWidth)) // Use pen instead of brush for drawing the perimeter
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2, RotationAngle + i * sweepAngle, sweepAngle);
                        g.DrawPath(pen, path);
                    }
                }
            }

            if (!ScoreObjectCollected)
            {
                using (GraphicsPath starPath = CreateStarPath(ScoreObject.X + ScoreObject.Width / 2, ScoreObject.Y + ScoreObject.Height / 2, ScoreObject.Width / 2, ScoreObject.Width / 4, 5))
                {
                    using (Brush brush = new SolidBrush(Color.NavajoWhite))
                    {
                        g.FillPath(brush, starPath);
                    }
                }
            }
        }

        public bool CheckCollision(Ball ball)
        {
            // Calculate distance from ball center to obstacle center
            float distance = (float)Math.Sqrt(Math.Pow(ball.X - Center.X, 2) + Math.Pow(ball.Y - Center.Y, 2));

            // Check if the ball is within the obstacle's perimeter
            bool onPerimeter = Math.Abs(distance - Radius) <= PenWidth / 2; // Consider the width of the pen

            if (!onPerimeter)
            {
                return false; // Ball is not touching the perimeter
            }

            // Calculate angle from the center of the obstacle to the ball
            float angleToBall = (float)(Math.Atan2(ball.Y - Center.Y, ball.X - Center.X) * 180 / Math.PI);
            angleToBall = (angleToBall + 360) % 360;

            // Determine the color segment of the obstacle that corresponds to the angle
            float segmentAngle = 360f / Colors.Length;
            int segment = (int)((angleToBall - RotationAngle + 360) % 360 / segmentAngle);

            // Check if the ball's color matches the obstacle's color segment
            bool colorMatch = Colors[segment] == ball.Color;

            return !colorMatch; // Collision occurs if colors do not match
        }

        public bool CheckOutOfBounds(Ball ball)
        {
            // Adjusted based on the window size and screen offset
            if (ball.Y + 10 > 1000)
            {
                 return true;
            }
            return false;
        }

        public bool CheckScoreObjectCollision(Ball ball)
        {
            if (!ScoreObjectCollected && ScoreObject.Contains(ball.X, ball.Y))
            {
                ScoreObjectCollected = true;
                return true;
            }
            return false;
        }
    }
}