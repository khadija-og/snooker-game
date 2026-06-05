using System;
using System.Drawing;

namespace SnookerGame
{
    /// <summary>
    /// Abstract base class representing a ball in the snooker game
    /// </summary>
    public abstract class Ball
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Radius { get; set; }
        public Color BallColor { get; set; }
        public bool IsPocketed { get; set; }

        private const float FRICTION = 0.985f;
        private const float MIN_VELOCITY = 0.1f;

        public Ball(float x, float y, float radius, Color color)
        {
            X = x;
            Y = y;
            Radius = radius;
            BallColor = color;
            VelocityX = 0;
            VelocityY = 0;
            IsPocketed = false;
        }

        /// <summary>
        /// Updates ball position and applies friction
        /// </summary>
        public virtual void Update(float tableWidth, float tableHeight)
        {
            if (IsPocketed) return;

            // Update position
            X += VelocityX;
            Y += VelocityY;

            // Apply friction
            VelocityX *= FRICTION;
            VelocityY *= FRICTION;

            // Stop ball if velocity is too low
            if (Math.Abs(VelocityX) < MIN_VELOCITY)
                VelocityX = 0;
            if (Math.Abs(VelocityY) < MIN_VELOCITY)
                VelocityY = 0;

            // Wall collision (bounce)
            if (X - Radius < 0)
            {
                X = Radius;
                VelocityX *= -0.8f; // Bounce with energy loss
            }
            if (X + Radius > tableWidth)
            {
                X = tableWidth - Radius;
                VelocityX *= -0.8f;
            }
            if (Y - Radius < 0)
            {
                Y = Radius;
                VelocityY *= -0.8f;
            }
            if (Y + Radius > tableHeight)
            {
                Y = tableHeight - Radius;
                VelocityY *= -0.8f;
            }
        }

        /// <summary>
        /// Checks if this ball collides with another ball
        /// </summary>
        public bool IsCollidingWith(Ball other)
        {
            if (other.IsPocketed || this.IsPocketed) return false;

            float dx = other.X - this.X;
            float dy = other.Y - this.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            float minDistance = this.Radius + other.Radius;

            return distance < minDistance;
        }

        /// <summary>
        /// Handles collision with another ball
        /// </summary>
        public virtual void CollideWith(Ball other)
        {
            if (other.IsPocketed || this.IsPocketed) return;

            float dx = other.X - this.X;
            float dy = other.Y - this.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) distance = 0.1f;

            // Normalize collision vector
            float nx = dx / distance;
            float ny = dy / distance;

            // Relative velocity
            float dvx = other.VelocityX - this.VelocityX;
            float dvy = other.VelocityY - this.VelocityY;

            // Relative velocity in collision normal direction
            float dvn = dvx * nx + dvy * ny;

            // Don't collide if balls are moving apart
            if (dvn >= 0) return;

            // Impulse magnitude (assuming equal mass)
            float impulse = dvn / 2;

            // Apply impulse
            this.VelocityX += impulse * nx * 0.9f;
            this.VelocityY += impulse * ny * 0.9f;
            other.VelocityX -= impulse * nx * 0.9f;
            other.VelocityY -= impulse * ny * 0.9f;

            // Separate balls to prevent overlap
            float overlap = (this.Radius + other.Radius) - distance;
            float separationX = (nx * overlap) / 2;
            float separationY = (ny * overlap) / 2;

            this.X -= separationX;
            this.Y -= separationY;
            other.X += separationX;
            other.Y += separationY;
        }

        /// <summary>
        /// Draws the ball on the graphics surface
        /// </summary>
        public virtual void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(BallColor))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }

            // Draw border
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawEllipse(pen, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }
        }

        /// <summary>
        /// Checks if ball is moving
        /// </summary>
        public bool IsMoving()
        {
            return Math.Abs(VelocityX) > MIN_VELOCITY || Math.Abs(VelocityY) > MIN_VELOCITY;
        }

        /// <summary>
        /// Stops the ball
        /// </summary>
        public void Stop()
        {
            VelocityX = 0;
            VelocityY = 0;
        }
    }
}
