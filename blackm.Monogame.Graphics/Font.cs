using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.Graphics
{
    internal class Font
    {
        private string _description;
        private SpriteFont _font;
        private Color _color;
        private Vector2 _position;
        private HorizontalTextAlignment _horizontalTextAlignment;
        private VerticalTextAlignment _verticalTextAlignment;

        public Font(string description, Vector2 position, SpriteFont font, Color color, HorizontalTextAlignment horizontalAlignment, VerticalTextAlignment verticalAlignment)
        {
            _font = font;
            _color = color;
            _description = description;
            _position = position;
            _horizontalTextAlignment = horizontalAlignment;
            _verticalTextAlignment = verticalAlignment;
        }

        public SpriteFont SpriteFont
        {
            get { return _font; }
            set { _font = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public string Description { get { return _description; } }
        public HorizontalTextAlignment HorizontalTextAlignment { get { return _horizontalTextAlignment; } }
        public VerticalTextAlignment VerticalTextAlignment { get { return _verticalTextAlignment; } }
    }
}
