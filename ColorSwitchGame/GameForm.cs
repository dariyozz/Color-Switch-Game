﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorSwitchGame
{
    public partial class GameForm : Form
    {
        private Timer gameTimer;
        private Scene scene;
        private int yOffset; // Offset to control the screen sliding
        private Button startButton;
        private bool gameStarted;

        public GameForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.DoubleBuffered = true;
            this.ClientSize = new Size(400, 800); // Set your desired window size
            this.Text = "Color Switch Game";

            scene = new Scene();

            gameTimer = new Timer();
            gameTimer.Interval = 20; // 50 FPS
            gameTimer.Tick += GameTimer_Tick;

            this.Paint += GameForm_Paint;
            this.KeyDown += GameForm_KeyDown;
            this.BackColor = Color.FromArgb(40, 40, 40);

            // Initialize UI elements
            startButton = new Button();
            startButton.Size = new Size(100, 50);
            startButton.Location = new Point((this.ClientSize.Width - startButton.Width) / 2, (this.ClientSize.Height - startButton.Height) / 2);
            startButton.Text = "Start";
            startButton.ForeColor = Color.White;
            startButton.BackColor = Color.Orange;
            startButton.Click += StartButton_Click;

         
            scoreLabel.Location = new Point(10, 10);
            scoreLabel.Text = "0";
            scoreLabel.ForeColor = Color.White;
           

            this.Controls.Add(startButton);
            this.Controls.Add(scoreLabel);

            gameStarted = false;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            gameStarted = true;
            scene.ResetGame();
            gameTimer.Start();
            startButton.Visible = false;
            this.Focus();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            scene.Update();

            // Calculate yOffset based on the ball's position relative to the window's center
            int windowHeight = this.ClientSize.Height;
            int ballY = scene.Ball.Y + scene.GetScreenOffset(); // Ball's current position on the screen

            // Center of the window
            int windowCenter = windowHeight / 2;

            // Calculate yOffset to keep the ball centered initially and scroll up as it moves higher
            yOffset = windowCenter - ballY;

            scoreLabel.Text = $"{scene.Score}";

            Invalidate();

            if (scene.IsGameOver)
            {
                gameTimer.Stop();
                startButton.Visible = true;
                startButton.Text = "Restart";
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStarted)
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (scene.IsGameOver)
                    {
                        scene.ResetGame(); // Reset the game when space is pressed after game over
                        yOffset = 0; // Reset yOffset
                        gameTimer.Start();
                        startButton.Visible = false;
                    }
                    else
                    {
                        scene.Jump();
                    }
                }
            }
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw the game scene with yOffset for scrolling effect
            g.TranslateTransform(0, yOffset); // Apply screen offset

            scene.Draw(g); // Draw scene objects

            g.ResetTransform(); // Reset graphics transform
        }
    }
}