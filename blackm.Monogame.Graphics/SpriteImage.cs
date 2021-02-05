using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.Graphics
{
    public class SpriteImage
    {

        protected Vector2 _velocity;

        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 CollisionThreshold { get; set; }
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public Vector2 MinBound { get { return Position - (0.5f * Size); } }
        public Vector2 MaxBound { get { return Position + (0.5f * Size); } }

        protected ContentManager Content { get; private set; }
        protected Color Color { get; set; }

        /// <summary>
        /// Constructor loads an image from the Content library
        /// </summary>
        /// <param name="content">Project Content Manager</param>
        /// <param name="path">Path to image to load as texture</param>
        /// <param name="position">Starting Position of sprite</param>
        /// <param name="size">Starting Size of the Sprite</param>
        public SpriteImage(ContentManager content, String path, Vector2 position, Vector2 size)
        {
            Content = content;
            Setup(Content.Load<Texture2D>(path), position, size);
        }
        /// <summary>
        /// Constructor that uses a pre-loaded texture image
        /// </summary>
        /// <param name="content">Project Content Manager</param>
        /// <param name="texture">Pre-Loaded Texture</param>
        /// <param name="position">Starting Position of sprite</param>
        /// <param name="size">Starting Size of the Sprite</param>
        public SpriteImage(ContentManager content, Texture2D texture, Vector2 position, Vector2 size)
        {
            Content = content;
            Setup(texture, position, size);
        }

        private void Setup(Texture2D texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Color = Color.White;
            CollisionThreshold = Vector2.Zero;
            Velocity = Vector2.Zero;
        }

        public bool Collide(SpriteImage image)
        {
            Rectangle imagerect = Camera.ComputePixelRectangle(image.Position, image.Size);
            Rectangle thisrect = Camera.ComputePixelRectangle(Position, Size);
            bool ret = imagerect.Intersects(thisrect);
            return ret;
        }

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = Camera.ComputePixelRectangle(Position,Size);
            spriteBatch.Draw(Texture, destRect, Color);
        }


        public static Texture2D CreateColoredPixel(GraphicsDevice graphics, Color color)
        {
            Texture2D rectangle = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);

            Color[] colorData = new Color[1];

            for (int i = 0; i < 1; i++)
                colorData[i] = color;
            rectangle.SetData(colorData);

            return rectangle;
        }
    }
}
