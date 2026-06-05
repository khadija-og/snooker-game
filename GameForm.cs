using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnookerGame
{
    /// <summary>
    /// Main game form - handles rendering and user input
    /// </summary>
    public class GameForm : Form
    {
        private GameManager gameManager;
        private Timer gameTimer;
        private bool[] keysPressed;
        private const int GAME_WIDTH = 1000;
        private const int GAME_HEIGHT = 700;
        private const int TIMER_INTERVAL = 16; // ~60 FPS

        public GameForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        /// <summary>
        /// Initializes game components
        /// </summary>
        private void InitializeGame()
        {
            // Form settings
            this.Text = "Snooker Game - OOP";
            this.Size = new System.Drawing.Size(GAME_WIDTH, GAME_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;

            // Initialize key tracking
            keysPressed = new bool[256];

            // Create game manager
            gameManager = new GameManager(GAME_WIDTH - 20, GAME_HEIGHT - 60);

            // Setup game loop timer
            gameTimer = new Timer();
            gameTimer.Interval = TIMER_INTERVAL;
            gameTimer.Tick += GameLoop_Tick;
            gameTimer.Start();

            // Event handlers
            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;
            this.MouseMove += GameForm_MouseMove;
            this.MouseClick += GameForm_MouseClick;
        }

        /// <summary>
        /// Main game loop
        /// </summary>
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            HandleInput();
            gameManager.Update();
            this.Invalidate(); // Trigger paint
        }

        /// <summary>
        /// Handles keyboard and mouse input
        /// </summary>
        private void HandleInput()
        {
            // Arrow keys for power control (only when aiming)
            if (gameManager.State == GameManager.GameState.Aiming)
            {
                if (keysPressed[(int)Keys.Up])
                    gameManager.Cue.IncreasePower();

                if (keysPressed[(int)Keys.Down])
                    gameManager.Cue.DecreasePower();
            }
        }

        /// <summary>
        /// Handles key down events
        /// </summary>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            keysPressed[(int)e.KeyCode] = true;

            // Reset game with R key
            if (e.KeyCode == Keys.R)
                gameManager.ResetGame();

            e.Handled = true;
        }

        /// <summary>
        /// Handles key up events
        /// </summary>
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            keysPressed[(int)e.KeyCode] = false;
            e.Handled = true;
        }

        /// <summary>
        /// Handles mouse movement - rotates cue
        /// </summary>
        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameManager.State == GameManager.GameState.Aiming)
            {
                gameManager.Cue.RotateToCursor(e.X, e.Y);
            }
        }

        /// <summary>
        /// Handles mouse click - strikes cue ball
        /// </summary>
        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameManager.State == GameManager.GameState.Aiming)
            {
                gameManager.StrikeBall();
            }
        }

        /// <summary>
        /// Handles painting/rendering
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Enable high quality rendering
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Draw game
            gameManager.Draw(e.Graphics);

            // Draw title and instructions
            DrawUI(e.Graphics);
        }

        /// <summary>
        /// Draws UI elements
        /// </summary>
        private void DrawUI(Graphics g)
        {
            // Draw instructions
            string instructions = "Arrow Keys: Adjust Power | Click: Strike | R: Reset";
            using (Font font = new Font("Arial", 10, FontStyle.Italic))
            using (Brush brush = new SolidBrush(Color.LimeGreen))
            {
                g.DrawString(instructions, font, brush, 20, GAME_HEIGHT - 25);
            }

            // Draw number of pocketed balls
            int pocketedCount = gameManager.SolidBalls.FindAll(b => b.IsPocketed).Count;
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.Cyan))
            {
                g.DrawString($"Pocketed: {pocketedCount}/{gameManager.SolidBalls.Count}", font, brush, GAME_WIDTH - 220, 20);
            }
        }

        /// <summary>
        /// Cleanup on form close
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            gameTimer?.Stop();
            gameTimer?.Dispose();
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Initialize component (required by designer)
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ResumeLayout(false);
        }
    }
}
