using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SnookerGame
{
    /// <summary>
    /// Manages the game state and logic
    /// </summary>
    public class GameManager
    {
        public CueBall CueBall { get; private set; }
        public List<SolidBall> SolidBalls { get; private set; }
        public Cue Cue { get; private set; }
        public int Score { get; private set; }
        public float TableWidth { get; private set; }
        public float TableHeight { get; private set; }
        public GameState State { get; set; }

        private const float BALL_RADIUS = 8f;
        private const float TABLE_MARGIN = 20f;

        public enum GameState
        {
            Aiming,
            Shooting,
            Playing
        }

        public GameManager(float tableWidth, float tableHeight)
        {
            TableWidth = tableWidth;
            TableHeight = tableHeight;
            Score = 0;
            State = GameState.Aiming;
            SolidBalls = new List<SolidBall>();

            InitializeGame();
        }

        /// <summary>
        /// Initializes the game with balls and cue
        /// </summary>
        private void InitializeGame()
        {
            // Create cue ball at bottom middle
            float cueBallX = TableWidth / 2;
            float cueBallY = TableHeight - 50;
            CueBall = new CueBall(cueBallX, cueBallY, BALL_RADIUS);

            // Create cue
            Cue = new Cue(cueBallX, cueBallY);

            // Create solid balls in triangle formation
            CreateTriangleFormation();
        }

        /// <summary>
        /// Creates triangular pyramid of balls
        /// </summary>
        private void CreateTriangleFormation()
        {
            float pyramidX = TableWidth / 2;
            float pyramidY = TableHeight / 4;
            float spacing = BALL_RADIUS * 2.2f;

            Color[] colors = new Color[]
            {
                Color.Red, Color.Red, Color.Red, Color.Red, Color.Red,
                Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow,
                Color.Blue, Color.Blue, Color.Blue, Color.Blue,
                Color.Black, Color.Black
            };

            int ballNumber = 1;
            int colorIndex = 0;

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col <= row; col++)
                {
                    float x = pyramidX + (col * spacing) - (row * spacing / 2);
                    float y = pyramidY + (row * spacing);

                    if (colorIndex < colors.Length)
                    {
                        SolidBall ball = new SolidBall(x, y, BALL_RADIUS, ballNumber, colors[colorIndex]);
                        SolidBalls.Add(ball);
                        colorIndex++;
                        ballNumber++;
                    }
                }
            }
        }

        /// <summary>
        /// Updates game logic
        /// </summary>
        public void Update()
        {
            // Update cue ball
            CueBall.Update(TableWidth, TableHeight);

            // Update solid balls
            foreach (var ball in SolidBalls)
            {
                if (!ball.IsPocketed)
                    ball.Update(TableWidth, TableHeight);
            }

            // Handle collisions between cue ball and solid balls
            foreach (var ball in SolidBalls)
            {
                if (CueBall.IsCollidingWith(ball))
                {
                    CueBall.CollideWith(ball);
                    Score += 10;
                }
            }

            // Handle collisions between solid balls
            for (int i = 0; i < SolidBalls.Count; i++)
            {
                for (int j = i + 1; j < SolidBalls.Count; j++)
                {
                    if (SolidBalls[i].IsCollidingWith(SolidBalls[j]))
                    {
                        SolidBalls[i].CollideWith(SolidBalls[j]);
                    }
                }
            }

            // Check if all balls have stopped
            bool allBallsStopped = !CueBall.IsMoving() && 
                                   SolidBalls.All(b => !b.IsMoving() || b.IsPocketed);

            if (State == GameState.Shooting && allBallsStopped)
            {
                State = GameState.Aiming;
                Cue.ResetPower();
            }

            // Remove pocketed balls
            RemovePocketedBalls();
        }

        /// <summary>
        /// Strikes the cue ball
        /// </summary>
        public void StrikeBall()
        {
            if (State != GameState.Aiming) return;

            Cue.GetStrikeVelocity(out float velocityX, out float velocityY);
            CueBall.Strike(velocityX, velocityY);
            State = GameState.Shooting;
        }

        /// <summary>
        /// Removes balls that have been pocketed
        /// </summary>
        private void RemovePocketedBalls()
        {
            // Check if cue ball is in pocket
            if (CueBall.X < TABLE_MARGIN || CueBall.X > TableWidth - TABLE_MARGIN ||
                CueBall.Y < TABLE_MARGIN || CueBall.Y > TableHeight - TABLE_MARGIN)
            {
                if (CueBall.IsMoving())
                {
                    ResetCueBall();
                }
            }

            // Check if solid balls are in pockets
            foreach (var ball in SolidBalls)
            {
                if (ball.X < TABLE_MARGIN || ball.X > TableWidth - TABLE_MARGIN ||
                    ball.Y < TABLE_MARGIN || ball.Y > TableHeight - TABLE_MARGIN)
                {
                    if (!ball.IsPocketed)
                    {
                        ball.IsPocketed = true;
                        Score += 50;
                    }
                }
            }
        }

        /// <summary>
        /// Resets cue ball to starting position
        /// </summary>
        private void ResetCueBall()
        {
            CueBall.X = TableWidth / 2;
            CueBall.Y = TableHeight - 50;
            CueBall.Stop();
            Cue.X = CueBall.X;
            Cue.Y = CueBall.Y;
        }

        /// <summary>
        /// Draws all game elements
        /// </summary>
        public void Draw(Graphics g)
        {
            // Draw table
            DrawTable(g);

            // Draw cue ball
            CueBall.Draw(g);

            // Draw solid balls
            foreach (var ball in SolidBalls)
            {
                if (!ball.IsPocketed)
                    ball.Draw(g);
            }

            // Draw cue if aiming
            if (State == GameState.Aiming)
                Cue.Draw(g);

            // Draw score
            DrawScore(g);

            // Draw game state
            DrawGameState(g);
        }

        /// <summary>
        /// Draws the table
        /// </summary>
        private void DrawTable(Graphics g)
        {
            // Table background
            using (Brush tableBrush = new SolidBrush(Color.DarkGreen))
            {
                g.FillRectangle(tableBrush, 0, 0, TableWidth, TableHeight);
            }

            // Table border
            using (Pen borderPen = new Pen(Color.Black, 3))
            {
                g.DrawRectangle(borderPen, 0, 0, TableWidth, TableHeight);
            }

            // Draw pockets
            DrawPockets(g);
        }

        /// <summary>
        /// Draws pocket indicators
        /// </summary>
        private void DrawPockets(Graphics g)
        {
            float pocketRadius = 15f;
            using (Brush pocketBrush = new SolidBrush(Color.Black))
            {
                // Corners
                g.FillEllipse(pocketBrush, TABLE_MARGIN - pocketRadius, TABLE_MARGIN - pocketRadius, pocketRadius * 2, pocketRadius * 2);
                g.FillEllipse(pocketBrush, TableWidth - TABLE_MARGIN - pocketRadius, TABLE_MARGIN - pocketRadius, pocketRadius * 2, pocketRadius * 2);
                g.FillEllipse(pocketBrush, TABLE_MARGIN - pocketRadius, TableHeight - TABLE_MARGIN - pocketRadius, pocketRadius * 2, pocketRadius * 2);
                g.FillEllipse(pocketBrush, TableWidth - TABLE_MARGIN - pocketRadius, TableHeight - TABLE_MARGIN - pocketRadius, pocketRadius * 2, pocketRadius * 2);

                // Side pockets
                g.FillEllipse(pocketBrush, TABLE_MARGIN - pocketRadius, TableHeight / 2 - pocketRadius, pocketRadius * 2, pocketRadius * 2);
                g.FillEllipse(pocketBrush, TableWidth - TABLE_MARGIN - pocketRadius, TableHeight / 2 - pocketRadius, pocketRadius * 2, pocketRadius * 2);
            }
        }

        /// <summary>
        /// Draws the score
        /// </summary>
        private void DrawScore(Graphics g)
        {
            using (Font font = new Font("Arial", 18, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.White))
            {
                g.DrawString($"Score: {Score}", font, brush, 20, 20);
            }
        }

        /// <summary>
        /// Draws game state information
        /// </summary>
        private void DrawGameState(Graphics g)
        {
            string stateText = State == GameState.Aiming ? "AIMING - Arrow Keys: Power | Click: Strike" : "PLAYING";
            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.Yellow))
            {
                g.DrawString(stateText, font, brush, 20, TableHeight - 40);
            }
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public void ResetGame()
        {
            Score = 0;
            SolidBalls.Clear();
            State = GameState.Aiming;
            InitializeGame();
        }
    }
}
