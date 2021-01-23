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
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Localization;

namespace MetroFramework.Controls
{
    [Designer("MetroFramework.Design.Controls.MetroToggleDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(CheckBox))]
    public class MetroToggle : CheckBox, IMetroControl
    {
        #region Constructor

        public MetroToggle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            Name = "MetroToggle";
            metroLocalize = new MetroLocalize(this);
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

        #endregion

        #region Fields

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus { get; set; } = false;

        private readonly MetroLocalize metroLocalize;

        [DefaultValue(MetroLinkSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLinkSize FontSize { get; set; } = MetroLinkSize.Small;

        [DefaultValue(MetroLinkWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLinkWeight FontWeight { get; set; } = MetroLinkWeight.Regular;

        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayStatus { get; set; } = true;

        [Browsable(false)]
        public override Font Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [Browsable(true)] public string OnText { get; set; }

        [Browsable(true)] public string OffText { get; set; }

        [Browsable(false)]
        public override string Text
        {
            get
            {
                if (Checked)
                    return string.IsNullOrEmpty(OnText) == false ? OnText : metroLocalize.translate("StatusOn");

                return string.IsNullOrEmpty(OffText) == false ? OffText : metroLocalize.translate("StatusOff");
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

                if (!UseCustomBackColor) backColor = MetroPaint.BackColor.Form(Theme);

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
            Color borderColor, foreColor;

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
            }
            else
            {
                foreColor = !UseStyleColors
                    ? MetroPaint.ForeColor.CheckBox.Normal(Theme)
                    : MetroPaint.GetStyleColor(Style);
                borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
            }

            using (var p = new Pen(borderColor))
            {
                var boxRect = new Rectangle(DisplayStatus ? 30 : 0, 0, ClientRectangle.Width - (DisplayStatus ? 31 : 1),
                    ClientRectangle.Height - 1);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            var fillColor = Checked ? MetroPaint.GetStyleColor(Style) : MetroPaint.BorderColor.CheckBox.Normal(Theme);

            using (var b = new SolidBrush(fillColor))
            {
                var boxRect = new Rectangle(DisplayStatus ? 32 : 2, 2, ClientRectangle.Width - (DisplayStatus ? 34 : 4),
                    ClientRectangle.Height - 4);
                e.Graphics.FillRectangle(b, boxRect);
            }

            var backColor = BackColor;

            if (!UseCustomBackColor) backColor = MetroPaint.BackColor.Form(Theme);

            using (var b = new SolidBrush(backColor))
            {
                var left = Checked ? Width - 11 : DisplayStatus ? 30 : 0;

                var boxRect = new Rectangle(left, 0, 11, ClientRectangle.Height);
                e.Graphics.FillRectangle(b, boxRect);
            }

            using (var b = new SolidBrush(MetroPaint.BorderColor.CheckBox.Hover(Theme)))
            {
                var left = Checked ? Width - 10 : DisplayStatus ? 30 : 0;

                var boxRect = new Rectangle(left, 0, 10, ClientRectangle.Height);
                e.Graphics.FillRectangle(b, boxRect);
            }

            if (DisplayStatus)
            {
                var textRect = new Rectangle(0, 0, 30, ClientRectangle.Height);
                TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Link(FontSize, FontWeight), textRect, foreColor,
                    MetroPaint.GetTextFormatFlags(TextAlign));
            }

            if (DisplayFocus && isFocused)
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

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var preferredSize = base.GetPreferredSize(proposedSize);
            preferredSize.Width = DisplayStatus ? 80 : 50;
            return preferredSize;
        }

        #endregion
    }
}