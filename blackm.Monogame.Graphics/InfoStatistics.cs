using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace blackm.Monogame.Graphics
{
    static public class InfoStatistics
    {
        static float FPSUPDATEDDISPLAY = 0.1f;
        static float _fpsUpdateDisplay = FPSUPDATEDDISPLAY;

        static float _fps = 0f;
        static float _updates = 0f;
        static int _NUMBERSAMPLES = 50;
        static int[] _fpssamples = new int[_NUMBERSAMPLES];
        static int _currentFPSSample = 0;
        static int _ticksFPSAggregate = 0;
        static int _secondSinceStart = 0;

        static int[] _updateSamples = new int[_NUMBERSAMPLES];
        static int _currentUDSample = 0;
        static int _ticksUDAggregate = 0;

        static public void LoadStatisticsModule(string font, Vector2 position, Color color)
        {
            FontManager.LoadFont(font, "info", color, position, HorizontalTextAlignment.LeftJustified, VerticalTextAlignment.TopJustified);
        }

        static public void Update(GameTime gameTime)
        {
            _updateSamples[_currentUDSample++] = (int)gameTime.ElapsedGameTime.Ticks;
            _ticksUDAggregate += (int)gameTime.ElapsedGameTime.Ticks;
            if (_ticksUDAggregate > TimeSpan.TicksPerSecond)
            {
                _ticksUDAggregate -= (int)TimeSpan.TicksPerSecond;
            }   
            if (_currentUDSample == _NUMBERSAMPLES)
            {
                float averageUpdateTime = Sum(_updateSamples) / _NUMBERSAMPLES;
                _updates = TimeSpan.TicksPerSecond / averageUpdateTime;
                _currentUDSample = 0;
            }    
        }

        static private string FormatData()
        {
            return String.Format("FPS: {0} \nUPDATES: {1}", _fps.ToString(), _updates.ToString());
        }


        static private float Sum(int[] samples)
        {
            float ret = 0f;
            for (int i = 0; i < samples.Length; i++)
                ret += (float)samples[i];
            return ret;
        }

        static public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _fpssamples[_currentFPSSample++] = (int)gameTime.ElapsedGameTime.Ticks;
            _ticksFPSAggregate += (int)gameTime.ElapsedGameTime.Ticks;
            if (_ticksFPSAggregate > TimeSpan.TicksPerSecond)
            {
                _ticksFPSAggregate -= (int)TimeSpan.TicksPerSecond;
                _secondSinceStart += 1;
            }
            if (_currentFPSSample == _NUMBERSAMPLES)
            {
                float averageFrameTime = Sum(_fpssamples) / _NUMBERSAMPLES;
                _fps = TimeSpan.TicksPerSecond / averageFrameTime;
                _currentFPSSample = 0;
            }

            FontManager.DrawFont("info", FormatData(), spriteBatch);

        }
    }
}
