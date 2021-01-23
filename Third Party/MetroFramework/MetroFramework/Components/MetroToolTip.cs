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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
    [ToolboxBitmap(typeof(ToolTip))]
    public class MetroToolTip : ToolTip, IMetroComponent
    {
        #region Constructor

        public MetroToolTip()
        {
            OwnerDraw = true;
            ShowAlways = true;

            Draw += MetroToolTip_Draw;
            Popup += MetroToolTip_Popup;
        }

        #endregion

        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Blue;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set => metroStyle = value;
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Light;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set => metroTheme = value;
        }

        [Browsable(false)] public MetroStyleManager StyleManager { get; set; }

        #endregion

        #region Fields

        [DefaultValue(true)]
        [Browsable(false)]
        public new bool ShowAlways
        {
            get => base.ShowAlways;
            set => base.ShowAlways = true;
        }

        [DefaultValue(true)]
        [Browsable(false)]
        public new bool OwnerDraw
        {
            get => base.OwnerDraw;
            set => base.OwnerDraw = true;
        }

        [Browsable(false)]
        public new bool IsBalloon
        {
            get => base.IsBalloon;
            set => base.IsBalloon = false;
        }

        [Browsable(false)]
        public new Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        [Browsable(false)]
        public new Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [Browsable(false)]
        public new string ToolTipTitle
        {
            get => base.ToolTipTitle;
            set => base.ToolTipTitle = "";
        }

        [Browsable(false)]
        public new ToolTipIcon ToolTipIcon
        {
            get => base.ToolTipIcon;
            set => base.ToolTipIcon = ToolTipIcon.None;
        }

        #endregion

        #region Management Methods

        public new void SetToolTip(Control control, string caption)
        {
            base.SetToolTip(control, caption);

            if (control is IMetroControl)
                foreach (Control c in control.Controls)
                    SetToolTip(c, caption);
        }

        private void MetroToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (e.AssociatedWindow is IMetroForm)
            {
                Style = ((IMetroForm) e.AssociatedWindow).Style;
                Theme = ((IMetroForm) e.AssociatedWindow).Theme;
                StyleManager = ((IMetroForm) e.AssociatedWindow).StyleManager;
            }
            else if (e.AssociatedControl is IMetroControl)
            {
                Style = ((IMetroControl) e.AssociatedControl).Style;
                Theme = ((IMetroControl) e.AssociatedControl).Theme;
                StyleManager = ((IMetroControl) e.AssociatedControl).StyleManager;
            }

            e.ToolTipSize = new Size(e.ToolTipSize.Width + 24, e.ToolTipSize.Height + 9);
        }

        private void MetroToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            var displayTheme = Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;

            var backColor = MetroPaint.BackColor.Form(displayTheme);
            var borderColor = MetroPaint.BorderColor.Button.Normal(displayTheme);
            var foreColor = MetroPaint.ForeColor.Label.Normal(displayTheme);

            using (var b = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            using (var p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p,
                    new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
            }

            var f = MetroFonts.Default(13f);
            TextRenderer.DrawText(e.Graphics, e.ToolTipText, f, e.Bounds, foreColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        #endregion
    }
}