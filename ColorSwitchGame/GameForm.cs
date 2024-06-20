using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorSwitchGame
{
    public partial class GameForm : Form
    {
        private Timer gameTimer;
        private Scene scene;
        private int yOffset; // Offset to control the screen sliding
        private bool gameStarted;
        private bool isPaused;

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
           
            StartBtn.Size = new Size(100, 50);
            StartBtn.Location = new Point((this.ClientSize.Width - StartBtn.Width) / 2,
                (this.ClientSize.Height - StartBtn.Height) / 2);
            StartBtn.Text = "Start ▶";
            StartBtn.ForeColor = Color.White;
            StartBtn.BackColor = Color.Orange;
            StartBtn.Click += StartButton_Click;

            PauseBtn.Size = new Size(70, 40);
            PauseBtn.Location = new Point(this.ClientSize.Width - PauseBtn.Width - 10, 10);
            PauseBtn.Text = "❚❚";
            PauseBtn.ForeColor = Color.White;
            PauseBtn.BackColor = Color.FromArgb(40,40,40);
            PauseBtn.FlatStyle = FlatStyle.Flat;
            PauseBtn.Click += PauseButton_Click;
            PauseBtn.Enabled = false;

            
            scoreLabel.Location = new Point(10, 10);
            scoreLabel.Text = "0";
            scoreLabel.ForeColor = Color.White;

            this.Controls.Add(scoreLabel);

            gameStarted = false;
            isPaused = false;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            gameStarted = true;
            scene.ResetGame();
            gameTimer.Start();
            StartBtn.Visible = false;
            PauseBtn.Enabled = true;
            this.ActiveControl = null; // Remove focus from button
            this.Focus(); // Focus back to the form for key events
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!gameStarted) return;

            if (isPaused)
            {
                gameTimer.Start();
                PauseBtn.Text = "❚❚";
                isPaused = false;
            }
            else
            {
                gameTimer.Stop();
                PauseBtn.Text = "▶";
                isPaused = true;
            }

            this.ActiveControl = null; // Remove focus from button
            this.Focus(); // Focus back to the form for key events
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused) return;

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
                StartBtn.Visible = true;
                StartBtn.Text = "Restart";
                PauseBtn.Enabled = false;
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStarted && !isPaused)
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (scene.IsGameOver)
                    {
                        scene.ResetGame(); // Reset the game when space is pressed after game over
                        yOffset = 0; // Reset yOffset
                        gameTimer.Start();
                        StartBtn.Visible = false;
                        PauseBtn.Enabled = true;
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
