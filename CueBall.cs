using System;
using System.Drawing;

namespace SnookerGame
{
    /// <summary>
    /// Represents the white cue ball - player controls this ball
    /// </summary>
    public class CueBall : Ball
    {
        private const float MAX_VELOCITY = 20f;

        public CueBall(float x, float y, float radius) 
            : base(x, y, radius, Color.White)
        {
        }

        /// <summary>
        /// Strikes the cue ball with power
        /// </summary>
        public void Strike(float powerX, float powerY)
        {
            VelocityX = Math.Min(Math.Max(powerX, -MAX_VELOCITY), MAX_VELOCITY);
            VelocityY = Math.Min(Math.Max(powerY, -MAX_VELOCITY), MAX_VELOCITY);
        }

        /// <summary>
        /// Draws the cue ball with a special highlight
        /// </summary>
        public override void Draw(Graphics g)
        {
            base.Draw(g);

            // Draw highlight on cue ball
            using (Brush highlightBrush = new SolidBrush(Color.FromArgb(100, Color.Yellow)))
            {
                g.FillEllipse(highlightBrush, X - Radius + 3, Y - Radius + 3, Radius - 2, Radius - 2);
            }
        }
    }
}
