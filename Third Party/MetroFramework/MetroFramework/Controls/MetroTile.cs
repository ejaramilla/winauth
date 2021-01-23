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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    [Designer("MetroFramework.Design.Controls.MetroTileDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(Button))]
    public class MetroTile : Button, IContainerControl, IMetroControl
    {
        #region Constructor

        public MetroTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            TextAlign = ContentAlignment.BottomLeft;
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion

        #region Interface

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;

        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null) CustomPaintBackground(this, e);
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;

        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null) CustomPaint(this, e);
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;

        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null) CustomPaintForeground(this, e);
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default) return metroStyle;

                if (StyleManager != null && metroStyle == MetroColorStyle.Default) return StyleManager.Style;
                if (StyleManager == null && metroStyle == MetroColorStyle.Default) return MetroDefaults.Style;

                return metroStyle;
            }
            set => metroStyle = value;
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default) return metroTheme;

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default) return StyleManager.Theme;
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default) return MetroDefaults.Theme;

                return metroTheme;
            }
            set => metroTheme = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager { get; set; } = null;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor { get; set; } = false;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor { get; set; } = false;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors { get; set; } = false;

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get => GetStyle(ControlStyles.Selectable);
            set => SetStyle(ControlStyles.Selectable, value);
        }

        [Browsable(false)] public Control ActiveControl { get; set; }

        public bool ActivateControl(Control ctrl)
        {
            if (Controls.Contains(ctrl))
            {
                ctrl.Select();
                ActiveControl = ctrl;
                return true;
            }

            return false;
        }

        #endregion

        #region Fields

        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool PaintTileCount { get; set; } = true;

        [DefaultValue(0)] public int TileCount { get; set; }

        [DefaultValue(ContentAlignment.BottomLeft)]
        public new ContentAlignment TextAlign
        {
            get => base.TextAlign;
            set => base.TextAlign = value;
        }

        [DefaultValue(null)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public Image TileImage { get; set; } = null;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseTileImage { get; set; } = false;

        [DefaultValue(ContentAlignment.TopLeft)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TileImageAlign { get; set; } = ContentAlignment.TopLeft;

        private MetroTileTextSize tileTextFontSize = MetroTileTextSize.Medium;

        [DefaultValue(MetroTileTextSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTileTextSize TileTextFontSize
        {
            get => tileTextFontSize;
            set
            {
                tileTextFontSize = value;
                Refresh();
            }
        }

        private MetroTileTextWeight tileTextFontWeight = MetroTileTextWeight.Light;

        [DefaultValue(MetroTileTextWeight.Light)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTileTextWeight TileTextFontWeight
        {
            get => tileTextFontWeight;
            set
            {
                tileTextFontWeight = value;
                Refresh();
            }
        }

        private bool isHovered;
        private bool isPressed;
        private bool isFocused;

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var backColor = BackColor;

                if (!UseCustomBackColor) backColor = MetroPaint.GetStyleColor(Style);

                if (backColor.A == 255)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint)) OnPaintBackground(e);

                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color foreColor;

            if (isHovered && !isPressed && Enabled)
                foreColor = MetroPaint.ForeColor.Tile.Hover(Theme);
            else if (isHovered && isPressed && Enabled)
                foreColor = MetroPaint.ForeColor.Tile.Press(Theme);
            else if (!Enabled)
                foreColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
            else
                foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);

            if (UseCustomForeColor) foreColor = ForeColor;

            if (isPressed)
            {
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            if (UseTileImage)
                if (TileImage != null)
                {
                    Rectangle imageRectangle;
                    switch (TileImageAlign)
                    {
                        case ContentAlignment.BottomLeft:
                            imageRectangle = new Rectangle(new Point(0, Height - TileImage.Height),
                                new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.BottomCenter:
                            imageRectangle =
                                new Rectangle(new Point(Width / 2 - TileImage.Width / 2, Height - TileImage.Height),
                                    new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.BottomRight:
                            imageRectangle =
                                new Rectangle(new Point(Width - TileImage.Width, Height - TileImage.Height),
                                    new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleLeft:
                            imageRectangle = new Rectangle(new Point(0, Height / 2 - TileImage.Height / 2),
                                new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleCenter:
                            imageRectangle =
                                new Rectangle(
                                    new Point(Width / 2 - TileImage.Width / 2, Height / 2 - TileImage.Height / 2),
                                    new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleRight:
                            imageRectangle =
                                new Rectangle(new Point(Width - TileImage.Width, Height / 2 - TileImage.Height / 2),
                                    new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopLeft:
                            imageRectangle = new Rectangle(new Point(0, 0),
                                new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, 0),
                                new Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, 0),
                                new Size(TileImage.Width, TileImage.Height));
                            break;

                        default:
                            imageRectangle = new Rectangle(new Point(0, 0),
                                new Size(TileImage.Width, TileImage.Height));
                            break;
                    }

                    e.Graphics.DrawImage(TileImage, imageRectangle);
                }

            if (TileCount > 0 && PaintTileCount)
            {
                var countSize = TextRenderer.MeasureText(TileCount.ToString(), MetroFonts.TileCount);

                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                TextRenderer.DrawText(e.Graphics, TileCount.ToString(), MetroFonts.TileCount,
                    new Point(Width - countSize.Width, 0), foreColor);
                e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            }

            var textSize = TextRenderer.MeasureText(Text, MetroFonts.Tile(tileTextFontSize, tileTextFontWeight));

            var flags = MetroPaint.GetTextFormatFlags(TextAlign) | TextFormatFlags.LeftAndRightPadding |
                        TextFormatFlags.EndEllipsis;
            var textRectangle = ClientRectangle;
            if (isPressed) textRectangle.Inflate(-2, -2);

            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Tile(tileTextFontSize, tileTextFontWeight),
                textRectangle, foreColor, flags);

            if (false && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion
    }
}