using blackm.Monogame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using blackm.Monogame.InputManager;
using blackm.Monogame.Pong.GameObjects;
using System;

namespace blackm.Monogame.Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const long _TARGETUPDATES = 200;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        const int _PREFFEREDWIDTH = 1280;
        const int _PREFFEREDHEIGHT = 720;
        const int _CAMERZOOMLEVEL = 80;
        const bool _FULLSCREEN = false;
        private Vector2 _paddleSize = new Vector2(1, 5);

        private Color _player1Color = Color.Azure;
        private Color _player2Color = Color.Yellow;
        private Color _statsColor = Color.Turquoise;


        SpriteImage _court;
        Paddle _player1Paddle;
        Paddle _player2Paddle;
        Ball _ball;
        public static Random _rando = new Random();

        const string _p1PosField = "p1Position";
        const string _p2PosField = "p2Position";
        const string _fontArial = "Arial16Reg";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputManager.InputManager.Initialize();
            FontManager.Initialize(Content);

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = new TimeSpan(TimeSpan.TicksPerSecond / _TARGETUPDATES);
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.PreferredBackBufferWidth = _PREFFEREDWIDTH;
            _graphics.PreferredBackBufferHeight = _PREFFEREDHEIGHT;
            _graphics.IsFullScreen = _FULLSCREEN;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera.SetCameraWindow(_graphics, Vector2.Zero, _CAMERZOOMLEVEL);
            InfoStatistics.LoadStatisticsModule(_fontArial, new Vector2(Camera.CameraWindowLowerLeftPosition.X + 0.1f, Camera.CameraWindowUpperRightPosition.Y - 0.0f), _statsColor);
            //Score Positions
            float p1scoreposX = Camera.CameraWindowLowerLeftPosition.X + 10;
            float p1scoreposY = Camera.CameraWindowUpperRightPosition.Y - 4;
            float p2scoreposX = Camera.CameraWindowUpperRightPosition.X - 10;
            float p2scoreposY = Camera.CameraWindowUpperRightPosition.Y - 4;

            float p1padX = Camera.CameraWindowLowerLeftPosition.X + 5;
            float p2padX = Camera.CameraWindowUpperRightPosition.X - 5;
            float p12padY = (Camera.CameraWindowUpperRightPosition.Y - Camera.CameraWindowLowerLeftPosition.Y) / 2;

            float p1PosDisplayX = Camera.CameraWindowLowerLeftPosition.X + 10;
            float p1PosDisplayY = Camera.CameraWindowLowerLeftPosition.Y + 4;
            float p2PosDisplayX = Camera.CameraWindowUpperRightPosition.X - 10;
            float p2PosDisplayY = Camera.CameraWindowLowerLeftPosition.Y + 4;

            _court = new SpriteImage(Content, "Court", new Vector2(Camera.CameraWindowUpperRightPosition.X / 2, Camera.CameraWindowUpperRightPosition.Y / 2), new Vector2(Camera.CameraWindowUpperRightPosition.X - Camera.CameraWindowLowerLeftPosition.X, Camera.CameraWindowUpperRightPosition.Y - Camera.CameraWindowLowerLeftPosition.Y));
            _player1Paddle = new Paddle(Content, SpriteImage.CreateColoredPixel(_graphics.GraphicsDevice, _player1Color), new Vector2(p1padX, p12padY), _paddleSize, new Vector2(p1scoreposX, p1scoreposY));
            _player2Paddle = new Paddle(Content, SpriteImage.CreateColoredPixel(_graphics.GraphicsDevice, _player2Color), new Vector2(p2padX, p12padY), _paddleSize, new Vector2(p2scoreposX, p2scoreposY));
            _player1Paddle.CollisionThreshold = new Vector2(2.5f);
            _player2Paddle.CollisionThreshold = new Vector2(2.5f);

            _player1Paddle.IsAI = false;
            _player2Paddle.IsAI = true;

            _ball = new Ball(Content, Content.Load<Texture2D>("SpikedBall"), new Vector2(Camera.CameraWindowUpperRightPosition.X / 2, Camera.CameraWindowUpperRightPosition.Y / 2), new Vector2(2), _player1Paddle, _player2Paddle);
            FontManager.LoadFont("28DaysLater72Reg", "ScorePlayer1", _player1Color, _player1Paddle.ScorePosition, HorizontalTextAlignment.LeftJustified, VerticalTextAlignment.TopJustified);
            FontManager.LoadFont("28DaysLater72Reg", "ScorePlayer2", _player2Color, _player2Paddle.ScorePosition, HorizontalTextAlignment.RightJustified, VerticalTextAlignment.TopJustified);
            FontManager.LoadFont(_fontArial, _p1PosField, _player1Color, new Vector2(p1PosDisplayX, p1PosDisplayY), HorizontalTextAlignment.LeftJustified, VerticalTextAlignment.CenterJustified);
            FontManager.LoadFont(_fontArial, _p2PosField, _player2Color, new Vector2(p2PosDisplayX, p2PosDisplayY), HorizontalTextAlignment.RightJustified, VerticalTextAlignment.CenterJustified);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.InputManager.Update();
            InfoStatistics.Update(gameTime);

            if (InputManager.InputManager.Buttons.AnyButton(InputState.Pressed, Buttons.Back))
                Exit();

            if (InputManager.InputManager.Buttons.AnyButton(InputState.Down, Buttons.X))
                _player1Paddle.Scored();
            if (InputManager.InputManager.Buttons.AnyButton(InputState.Down, Buttons.A))
                _player2Paddle.Scored();
            if (!_player1Paddle.IsAI)
                _player1Paddle.Update(InputManager.InputManager.ThumbSticks.Left, gameTime);
            if (!_player2Paddle.IsAI)
                _player2Paddle.Update(InputManager.InputManager.ThumbSticks.Right, gameTime);
            _ball.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _court.Draw(_spriteBatch);
            FontManager.DrawFont("ScorePlayer1", _player1Paddle.Score.ToString(), _spriteBatch);
            FontManager.DrawFont("ScorePlayer2", _player2Paddle.Score.ToString(), _spriteBatch);
            FontManager.DrawFont(_p1PosField, $"X: {_player1Paddle.Position.X} Y: {_player1Paddle.Position.Y}", _spriteBatch);
            FontManager.DrawFont(_p2PosField, $"X: {_player2Paddle.Position.X} Y: {_player2Paddle.Position.Y}", _spriteBatch);
            _player1Paddle.Draw(_spriteBatch);
            _player2Paddle.Draw(_spriteBatch);
            _ball.Draw(_spriteBatch);
            InfoStatistics.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
