using blackm.Monogame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.Pong.GameObjects
{
    public class Paddle : SpriteImage
    {
        public int Score { get; private set; }
        public Vector2 ScorePosition { get; }

        private const float _MAXSPEED = 250.0f;
        private float _accelleration = 100.0f;
        public Vector2 StartingPosition { get; private set; }

        public bool IsAI { get; set; }

        public Paddle(ContentManager content, Texture2D texture, Vector2 paddlePosition, Vector2 size, Vector2 scorePosition)
            : base(content, texture, paddlePosition, size)
        {
            Score = 0;
            ScorePosition = scorePosition;
            StartingPosition = paddlePosition;
        }

        public void Scored()
        { 
            Score++;
        }

        public void ResetPosition(GameTime gameTime)
        {
            Vector2 deltaTranslate = Vector2.Zero;
            deltaTranslate.Y = (Position.Y > StartingPosition.Y) ? -1.0f : 1.0f;

            if (Position.Y - StartingPosition.Y < 1.0 && Position.Y - StartingPosition.Y > -1.0)
                deltaTranslate.Y = 0;

            Update(deltaTranslate, gameTime);
        }

        public void Update(Vector2 deltaTranslate, GameTime gameTime)
        {
            deltaTranslate.X = 0;

            if (deltaTranslate.Y > 0)
            {
                Velocity += new Vector2(0, deltaTranslate.Y * (_accelleration * (float)gameTime.ElapsedGameTime.TotalSeconds));
                if (Velocity.Y >= _MAXSPEED)
                    Velocity = new Vector2(0, _MAXSPEED);
            }
            else if (deltaTranslate.Y < 0)
            {
                Velocity -= new Vector2(0, deltaTranslate.Y * (-_accelleration * (float)gameTime.ElapsedGameTime.TotalSeconds));
                if (Velocity.Y <= -_MAXSPEED)
                    Velocity = new Vector2(0, -_MAXSPEED);
            }
            else
            {
                Velocity = Vector2.Zero;
            }

            if (Velocity.Y > 0 && Camera.CollideWithCamerWindow(this) == CameraWindowCollisionStatus.CollideTop || Velocity.Y < 0 && Camera.CollideWithCamerWindow(this) == CameraWindowCollisionStatus.CollideBottom)
            {
                Velocity = Vector2.Zero;
                Position = new Vector2((float)Math.Round(Position.X, 0),(float) Math.Round(Position.Y, 0));
            }



            base.Update(gameTime);

        }



    }
}
