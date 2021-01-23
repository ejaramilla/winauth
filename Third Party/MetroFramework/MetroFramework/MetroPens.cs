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
    public sealed class MetroPens
    {
        private static readonly Dictionary<string, Pen> metroPens = new Dictionary<string, Pen>();

        public static Pen Black => GetSavePen("Black", MetroColors.Black);

        public static Pen White => GetSavePen("White", MetroColors.White);

        public static Pen Silver => GetSavePen("Silver", MetroColors.Silver);

        public static Pen Blue => GetSavePen("Blue", MetroColors.Blue);

        public static Pen Green => GetSavePen("Green", MetroColors.Green);

        public static Pen Lime => GetSavePen("Lime", MetroColors.Lime);

        public static Pen Teal => GetSavePen("Teal", MetroColors.Teal);

        public static Pen Orange => GetSavePen("Orange", MetroColors.Orange);

        public static Pen Brown => GetSavePen("Brown", MetroColors.Brown);

        public static Pen Pink => GetSavePen("Pink", MetroColors.Pink);

        public static Pen Magenta => GetSavePen("Magenta", MetroColors.Magenta);

        public static Pen Purple => GetSavePen("Purple", MetroColors.Purple);

        public static Pen Red => GetSavePen("Red", MetroColors.Red);

        public static Pen Yellow => GetSavePen("Yellow", MetroColors.Yellow);

        private static Pen GetSavePen(string key, Color color)
        {
            lock (metroPens)
            {
                if (!metroPens.ContainsKey(key))
                    metroPens.Add(key, new Pen(color, 1f));

                return metroPens[key].Clone() as Pen;
            }
        }
    }
}