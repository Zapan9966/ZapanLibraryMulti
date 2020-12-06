using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ZapanControls.Libraries;

namespace ZapanControls.Helpers
{
    public static class ColorHelper
    {
        private static readonly Random _rand = new Random();
        private static readonly IList<SolidColorBrush> _currentColors = new List<SolidColorBrush>();
        
        public static Brush RandomLightColor()
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(
                (byte)_rand.Next(150, 255),
                (byte)_rand.Next(75, 255),
                (byte)_rand.Next(75, 255)));

            if (_currentColors.Contains(brush, new SolidColorBrushComparer()))
            {
                return RandomLightColor();
            }
            else
            {
                _currentColors.Add(brush);
                return brush;
            }
        }

        public static Brush RandomDarkColor()
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(
                (byte)_rand.Next(200, 255),
                (byte)_rand.Next(150, 255),
                (byte)_rand.Next(150, 255))
            );

            if (_currentColors.Contains(brush, new SolidColorBrushComparer()))
            {
                return RandomDarkColor();
            }
            else
            {
                _currentColors.Add(brush);
                return brush;
            }
        }
    }
}
