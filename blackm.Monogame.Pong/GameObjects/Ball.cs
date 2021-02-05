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
    public class Ball : SpriteImage
    {

        private Paddle _player1;
        private Paddle _player2;
        private Vector2 _startingPosition;
        private Vector2 _defaultVelocity = new Vector2(15f);

        bool _isAbove1, _isAbove2, _prevAbove1, _prevAbove2;

        public Ball(ContentManager content, Texture2D texture, Vector2 position, Vector2 size, Paddle player1, Paddle player2)
            : base(content, texture, position, size)
        {
            _player1 = player1;
            _player2 = player2;
            Velocity = _defaultVelocity;
            CollisionThreshold = new Vector2(0, 2.5f);
            _startingPosition = position;
        }

        private void SetPosition(CameraWindowCollisionStatus cameraEdge)
        {
            Position = _startingPosition;
            //_defaultVelocity.Y = (Game1._rando.Next(100) > 50) ? _defaultVelocity.Y : -_defaultVelocity.Y;
            Vector2 newVelocity = Vector2.Zero;
            newVelocity.X = _defaultVelocity.X;
            newVelocity.Y = Game1._rando.Next(100) / 100.0f * _defaultVelocity.Y;
            switch (cameraEdge)
            {
                case CameraWindowCollisionStatus.CollideLeft:
                    Velocity = new Vector2(newVelocity.X, newVelocity.Y);
                    break;
                case CameraWindowCollisionStatus.CollideRight:
                    Velocity = new Vector2(-newVelocity.X, newVelocity.Y);
                    break;
            }
        }

        private void CollidePlayer(Paddle player)
        {
            float diff = (Position.Y - player.Position.Y);
            _velocity.Y = diff * _defaultVelocity.Y;

            float newX = ((Math.Abs(diff) + 1) * _defaultVelocity.X / 2);

            if (_velocity.X < 0)
                _velocity.X = newX;
            else if (_velocity.X > 0)
                _velocity.X = -newX;



        }

        public override void Update(GameTime gameTime)
        {
            if (Velocity.X > 0 && Collide(_player2))
            {
                //_velocity.X *= -1;
                CollidePlayer(_player2);
            }

            if (Velocity.X < 0 && Collide(_player1))
            {
                //_velocity.X *= -1;
                CollidePlayer(_player1);
            }

            switch (Camera.CollideWithCamerWindow(this))
            {
                case CameraWindowCollisionStatus.CollideTop:
                case CameraWindowCollisionStatus.CollideBottom:
                    _velocity.Y *= -1;
                    break;
                case CameraWindowCollisionStatus.CollideLeft:
                    _player2.Scored();
                    SetPosition(CameraWindowCollisionStatus.CollideLeft);
                    break;
                case CameraWindowCollisionStatus.CollideRight:
                    _player1.Scored();
                    SetPosition(CameraWindowCollisionStatus.CollideRight);
                    break;
            }

            base.Update(gameTime);

            if (_player1.IsAI)
            {
                Vector2 deltaPlayerTranslate = Vector2.Zero;

                if (Velocity.X < 0)
                {

                    _prevAbove1 = _isAbove1;

                    if (_player1.Position.Y < Position.Y)
                        _isAbove1 = true;

                    if (_player1.Position.Y > Position.Y)
                        _isAbove1 = false;

                    if (_isAbove1 && _prevAbove1)
                        deltaPlayerTranslate.Y = 1.0f;

                    if (!_isAbove1 && !_prevAbove1)
                        deltaPlayerTranslate.Y = -1.0f;

                    _player1.Update(deltaPlayerTranslate, gameTime);
                }
                else
                {
                    _player1.ResetPosition(gameTime);
                }




            }

            if (_player2.IsAI)
            {
                Vector2 deltaPlayerTranslate = Vector2.Zero;
                if (Velocity.X > 0)
                {
                    _prevAbove2 = _isAbove2;

                    if (_player2.Position.Y < Position.Y)
                        _isAbove2 = true;

                    if (_player2.Position.Y > Position.Y)
                        _isAbove2 = false;

                    if (_isAbove2 && _prevAbove2)
                        deltaPlayerTranslate.Y = 1.0f;

                    if (!_isAbove2 && !_prevAbove2)
                        deltaPlayerTranslate.Y = -1.0f;

                    _player2.Update(deltaPlayerTranslate, gameTime);
                }
                else
                {
                    _player2.ResetPosition(gameTime);
                }




            }
        }
    }
}
