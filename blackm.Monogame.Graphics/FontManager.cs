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
    static public class FontManager
    {
        static private Dictionary<string, Font> _textFields = new Dictionary<string, Font>();
        //TODO: Make sure this is not null
        static private ContentManager Content { get; set; }

        /// <summary>
        /// Loads a new Font for use in the game
        /// </summary>
        /// <param name="description">Content Filename for the font to use (will use existing spritefont if one already is loaded)</param>
        /// <param name="name">Name of the Message to display eg. 'ScorePlayer1'</param>
        /// <param name="color">Color of the Message<param>
        /// <param name="position">Location within the Camera window to display the Message</param>
        static public void LoadFont(string description, string name, Color color, Vector2 position, HorizontalTextAlignment horizontalAlignment, VerticalTextAlignment verticalAlignment)
        {
            Font font = _textFields.Values.FirstOrDefault(f => f.Description.Equals(description));
            SpriteFont spriteFont = (font != null) ? font.SpriteFont : Content.Load<SpriteFont>(description);
            Font newFont = new Font(description, position, spriteFont, color, horizontalAlignment, verticalAlignment);
            _textFields.Add(name, newFont);
        }

        static public void Initialize(ContentManager content)
        {
            Content = content;
        }

        static public void DrawFont(string name, string msg, SpriteBatch spriteBatch)
        {
            Font font = (_textFields.ContainsKey(name)) ? _textFields[name] : null;

            if (font != null)
            {
                Vector2 fontSize = MeasureStringSize(font, msg);
                Rectangle destRect = Camera.ComputePixelRectangle(font.Position, fontSize);

                switch (font.HorizontalTextAlignment)
                {
                    case HorizontalTextAlignment.LeftJustified:
                        destRect.X += destRect.Width / 2;
                        break;
                    case HorizontalTextAlignment.RightJustified:
                        destRect.X -= destRect.Width / 2;
                        break;
                    case HorizontalTextAlignment.CenterJustified:
                        break;
                }

                switch (font.VerticalTextAlignment)
                {
                    case VerticalTextAlignment.TopJustified:
                        destRect.Y += destRect.Height / 2;
                        break;
                    case VerticalTextAlignment.BottonJustified:
                        destRect.Y -= destRect.Height / 2;
                        break;
                    case VerticalTextAlignment.CenterJustified:
                        break;
                }

                spriteBatch.DrawString(font.SpriteFont, msg, new Vector2(destRect.X, destRect.Y), font.Color);
            }
        }

        static private Vector2 MeasureStringSize(Font font, string msg)
        {
            Vector2 pixelSize = font.SpriteFont.MeasureString(msg);
            return new Vector2(pixelSize.X / Camera.Ratio, pixelSize.Y / Camera.Ratio);

        }

    }
}
