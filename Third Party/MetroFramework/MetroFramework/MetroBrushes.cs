/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework
{
    public sealed class MetroBrushes
    {
        private static readonly Dictionary<string, SolidBrush> metroBrushes = new Dictionary<string, SolidBrush>();

        public static SolidBrush Black => GetSaveBrush("Black", MetroColors.Black);

        public static SolidBrush White => GetSaveBrush("White", MetroColors.White);

        public static SolidBrush Silver => GetSaveBrush("Silver", MetroColors.Silver);

        public static SolidBrush Blue => GetSaveBrush("Blue", MetroColors.Blue);

        public static SolidBrush Green => GetSaveBrush("Green", MetroColors.Green);

        public static SolidBrush Lime => GetSaveBrush("Lime", MetroColors.Lime);

        public static SolidBrush Teal => GetSaveBrush("Teal", MetroColors.Teal);

        public static SolidBrush Orange => GetSaveBrush("Orange", MetroColors.Orange);

        public static SolidBrush Brown => GetSaveBrush("Brown", MetroColors.Brown);

        public static SolidBrush Pink => GetSaveBrush("Pink", MetroColors.Pink);

        public static SolidBrush Magenta => GetSaveBrush("Magenta", MetroColors.Magenta);

        public static SolidBrush Purple => GetSaveBrush("Purple", MetroColors.Purple);

        public static SolidBrush Red => GetSaveBrush("Red", MetroColors.Red);

        public static SolidBrush Yellow => GetSaveBrush("Yellow", MetroColors.Yellow);

        private static SolidBrush GetSaveBrush(string key, Color color)
        {
            lock (metroBrushes)
            {
                if (!metroBrushes.ContainsKey(key))
                    metroBrushes.Add(key, new SolidBrush(color));

                return metroBrushes[key].Clone() as SolidBrush;
            }
        }
    }
}