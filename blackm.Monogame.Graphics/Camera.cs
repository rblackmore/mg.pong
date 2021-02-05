using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.Graphics
{
    public enum CameraWindowCollisionStatus
    {
        CollideTop = 1,
        CollideBottom = 2,
        CollideLeft = 3,
        CollideRight = 4,
        InsideWindow = 5
    }

    public static class Camera
    {
        static private Vector2 _origin = Vector2.Zero;
        static private float _width = 100f;
        static private float _height = 0f;
        static private float _ratio = -1f;
        static private float _maxWidth = 500f;
        static private float _minWidth = 50f;

        static private GraphicsDeviceManager _graphics;

        //Bounds
        public static Vector2 CameraWindowLowerLeftPosition { get { return _origin; } }
        public static Vector2 CameraWindowUpperRightPosition { get { return _origin + new Vector2(_width, _height); } }

        public static float Ratio { get { return cameralWindowToPixelRatio(); } }

        static private float cameralWindowToPixelRatio()
        {
            if (_ratio < 0f)
                _ratio = _graphics.PreferredBackBufferWidth / _width; _height = _graphics.PreferredBackBufferHeight / _ratio;
            return _ratio;
        }

        static public void SetCameraWindow(GraphicsDeviceManager graphics, Vector2 origin, float width)
        {
            _origin = origin;
            _width = (width < _minWidth) ? _minWidth : width;
            _width = (width > _maxWidth) ? _maxWidth : width;
            _graphics = graphics;
            cameralWindowToPixelRatio();
        }

        static public void ComputePixelPosition(Vector2 entityCameraPosition, out int x, out int y)
        {
            float ratio = cameralWindowToPixelRatio();
            x = (int)(((entityCameraPosition.X - _origin.X) * ratio) + 0.5f);
            y = (int)(((entityCameraPosition.Y - _origin.Y) * ratio) + 0.5f);
            y = _graphics.PreferredBackBufferHeight - y;
        }

        static public Rectangle ComputePixelRectangle(Vector2 position, Vector2 size)
        {
            float ratio = cameralWindowToPixelRatio();
            int width = (int)((size.X * ratio) + 0.5f);
            int height = (int)((size.Y * ratio) + 0.5);
            int x, y;
            ComputePixelPosition(position, out x, out y);
            y -= height / 2;
            x -= width / 2;
            return new Rectangle(x, y, width, height);

        }

        static public CameraWindowCollisionStatus CollideWithCamerWindow(SpriteImage sprite)
        {
            Vector2 min = CameraWindowLowerLeftPosition;
            Vector2 max = CameraWindowUpperRightPosition;

            if (sprite.MaxBound.Y > max.Y - sprite.CollisionThreshold.Y)
                return CameraWindowCollisionStatus.CollideTop;
            if (sprite.MaxBound.X > max.X - sprite.CollisionThreshold.X)
                return CameraWindowCollisionStatus.CollideRight;
            if (sprite.MinBound.Y < min.Y + sprite.CollisionThreshold.Y)
                return CameraWindowCollisionStatus.CollideBottom;
            if (sprite.MinBound.X < min.X + sprite.CollisionThreshold.X)
                return CameraWindowCollisionStatus.CollideLeft;

            return CameraWindowCollisionStatus.InsideWindow;
        }
    }
}
