 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ColorSwitchGame
{
    public class Scene
    {
        public Ball Ball { get; private set; }
        public List<Obstacle> Obstacles { get; private set; }
        public int Score { get; private set; }
        private int frameCounter;
        private Random random = new Random();
        public bool IsGameOver { get; private set; }
        public bool IsGameStarted { get; private set; }
        private int obstacleGap = 300; // Gap between obstacles
        private int lastObstacleY = 0; // Y position of the last spawned obstacle
        private int screenOffset = 0; // Offset to control the screen sliding
        private int screenHeight = 800; // Height of the game screen

        public Scene()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            Ball = new Ball(200, 750, Color.Red); // Start ball position below the first obstacle
            Obstacles = new List<Obstacle>();
            Score = 0;
            frameCounter = 0;
            IsGameOver = false;
            IsGameStarted = false;
            lastObstacleY = 0;
            screenOffset = 0;

            // Initial obstacles
            AddObstacle(200);
            AddObstacle(500);
        }

        private void AddObstacle(int yPos)
        {
            Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
            Obstacle obstacle = new Obstacle(new Point(200, yPos - screenOffset), 100, colors); // Adjust Y position based on screen offset
            Obstacles.Add(obstacle);
            lastObstacleY = yPos;
        }

        public void Update()
        {
            if (IsGameOver || !IsGameStarted) return;

            Ball.Update();

            if (frameCounter % 200 == 0) // Change color every 200 frames
            {
                Ball.ChangeColor();
            }

            // Check if it's time to add a new obstacle
            if (Ball.Y + screenOffset < lastObstacleY - obstacleGap + screenHeight)
            {
                AddObstacle(lastObstacleY - obstacleGap);
            }

            foreach (var obstacle in Obstacles)
            {
                obstacle.Update();

                if (obstacle.CheckCollision(Ball))
                {
                    GameOver();
                    return;
                }

                if (obstacle.CheckOutOfBounds(Ball)) // Adjusted based on the window size and screen offset
                {
                    GameOver();
                    return;
                }

                if (obstacle.CheckScoreObjectCollision(Ball))
                {
                    Score++;
                }
            }

            // Remove obstacles that are out of the screen
            Obstacles.RemoveAll(o => o.Center.Y + screenOffset > screenHeight);

            frameCounter++;
        }

        public void Jump()
        {
            if (IsGameOver)
            {
                ResetGame();
                IsGameStarted = true;
            }

            if (!IsGameStarted)
            {
                IsGameStarted = true;
            }

            Ball.Jump();
        }

        public void Draw(Graphics g)
        {
            Ball.Draw(g);
            foreach (var obstacle in Obstacles)
            {
                obstacle.Draw(g);
            }
        }

        private void GameOver()
        {
            IsGameOver = true;
            MessageBox.Show($"Game Over! Your score: {Score}");
        }

        public int GetScreenOffset()
        {
            return screenOffset;
        }

        public void IncreaseScreenOffset(int offset)
        {
            screenOffset += offset;
        }
    }
}