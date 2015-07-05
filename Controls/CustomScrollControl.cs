#region MIT License
//
// Filename: CustomScrollControl.cs
//
// Copyright © 2011-2013 Felix Concordia SARL. All rights reserved.
// Felix Concordia SARL, 400 avenue Roumanille, Bat 7 - BP 309, 06906 Sophia-Antipolis Cedex, FRANCE.
// 
// Copyright © 2005-2011 MEDIT S.A. All rights reserved.
// MEDIT S.A., 2 rue du Belvedere, 91120 Palaiseau, FRANCE.
// 
// Copyright © 2005 www.devage.com, Davide Icardi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the
// Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// A control with a custom implementation of a scrollable area
  /// </summary>
  [ToolboxItem(false)]
  public abstract class CustomScrollControl : Panel
  {
    #region Class variables
    private VScrollBar verticalScroll = null;
    private HScrollBar horizontalScroll = null;
    private Panel bottomRightPanel = null;
    private Size customScrollArea = new Size(0, 0);

    private int oldVerticalScrollValue = 0;
    private int oldHorizontalScrollValue = 0;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomScrollControl"/> class.
    /// </summary>
    protected CustomScrollControl()
    {
      SuspendLayout();

      base.AutoScroll = false;

      // to remove flicker and use custom draw
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      //SetStyle(ControlStyles.DoubleBuffer, true);
      //SetStyle(ControlStyles.Opaque, true);
      SetStyle(ControlStyles.ResizeRedraw, true);

      ResumeLayout(false);
    }
    #endregion

    #region override AutoScroll
    /// <summary>
    /// Gets or sets a value indicating whether the default AutoScroll property is disabled because we have a custom implementation
    /// </summary>
    /// <value>AutoScroll</value>
    /// <returns>true if the container enables auto-scrolling; otherwise, <c>false</c>. The default value is false. </returns>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool AutoScroll
    {
      get
      {
        return false;
      }
      set
      {
        if (value)
        {
          throw new MEDDataGridException("Auto Scroll not supported in this control");
        }
        base.AutoScroll = false;
      }
    }
    #endregion

    #region ScrollBars and Panels
    /// <summary>
    /// Gets the vertical scrollbar. Can be null.
    /// </summary>
    /// <value>The Vertical scroll bar.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public VScrollBar VScrollBar
    {
      get { return this.verticalScroll; }
    }

    /// <summary>
    /// Gets the horizontal scrollbar. Can be null.
    /// </summary>
    /// <value>The Horizontal scroll bar.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public HScrollBar HScrollBar
    {
      get { return this.horizontalScroll; }
    }

    /// <summary>
    /// Gets the panel at the bottom right of the control. This panel is valid only if HScrollBar and VScrollBar are valid. Otherwise is null.
    /// </summary>
    /// <value>The bottom right panel.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Panel BottomRightPanel
    {
      get { return this.bottomRightPanel; }
    }

    /// <summary>
    /// Invalidate the scrollable area
    /// </summary>
    protected abstract void InvalidateScrollableArea();
    #endregion

    #region ScrollArea, ScrollPosition, DisplayRectangle
    /// <summary>
    /// Gets or sets the logical area of the control that must be used for scrolling
    /// </summary>
    /// <value>The custom scroll area.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual Size CustomScrollArea
    {
      get
      {
        return this.customScrollArea;
      }
      set
      {
        if (this.customScrollArea != value)
        {
          this.customScrollArea = value;
          RecalcCustomScrollBars();
        }
      }
    }

    /// <summary>
    /// Gets or sets the current scroll position relative to the CustomScrollArea
    /// </summary>
    /// <value>The custom scroll position.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual Point CustomScrollPosition
    {
      get
      {
        int l_x = 0;
        if (horizontalScroll != null)
        {
          l_x = -horizontalScroll.Value;
        }
        int l_y = 0;
        if (verticalScroll != null)
        {
          l_y = -verticalScroll.Value;
        }
        return new Point(l_x, l_y);
      }
      set
      {
        if (horizontalScroll != null)
        {
          horizontalScroll.Value = -value.X;
        }
        if (verticalScroll != null)
        {
          verticalScroll.Value = -value.Y;
        }
      }
    }

    /// <summary>
    /// Gets the display rectangle of the control, without ScrollBars
    /// </summary>
    /// <value>DisplayRectangle</value>
    /// <returns>A <see cref="T:System.Drawing.Rectangle"></see> that represents the display area of the control.</returns>
    public override Rectangle DisplayRectangle
    {
      get
      {
        int l_ScrollH = 0;
        if (horizontalScroll != null)
        {
          l_ScrollH = horizontalScroll.Height;
        }

        int l_ScrollV = 0;
        if (verticalScroll != null)
        {
          l_ScrollV = verticalScroll.Width;
        }

        Rectangle l_Base = base.DisplayRectangle;
        return new Rectangle(l_Base.X, l_Base.Y, l_Base.Width - l_ScrollV, l_Base.Height - l_ScrollH);
      }
    }
    #endregion

    #region Point and Rectangle Relative/Absolute conversion
    /// <summary>
    /// Convert an absolute rectangle from the total scrolling area to the current displayrectangle.
    /// </summary>
    /// <param name="p_AbsoluteRectangle">The absolute rectangle.</param>
    /// <returns></returns>
    public Rectangle RectangleAbsoluteToRelative(Rectangle p_AbsoluteRectangle)
    {
      return new Rectangle(PointAbsoluteToRelative(p_AbsoluteRectangle.Location), p_AbsoluteRectangle.Size);
    }

    /// <summary>
    /// Convert a relative rectangle from the current displayrectangle to an absolute rectangle for the current scrolling area.
    /// </summary>
    /// <param name="p_RelativeRectangle">The relative rectangle.</param>
    /// <returns></returns>
    public Rectangle RectangleRelativeToAbsolute(Rectangle p_RelativeRectangle)
    {
      return new Rectangle(PointRelativeToAbsolute(p_RelativeRectangle.Location), p_RelativeRectangle.Size);
    }

    /// <summary>
    /// Convert a relative point from the current displayrectangle to an absolute point to the total scrolling area.
    /// </summary>
    /// <param name="p_RelativePoint">The relative point.</param>
    /// <returns></returns>
    public Point PointRelativeToAbsolute(Point p_RelativePoint)
    {
      Point l_ScrollPos = CustomScrollPosition;
      return new Point(p_RelativePoint.X - l_ScrollPos.X, p_RelativePoint.Y - l_ScrollPos.Y);
    }

    /// <summary>
    /// Convert an absolute point from an absolute point to the current
    /// displayrectangle of the grid.
    /// </summary>
    /// <param name="p_AbsolutePoint">The absolute point.</param>
    /// <returns></returns>
    public Point PointAbsoluteToRelative(Point p_AbsolutePoint)
    {
      Point l_ScrollPos = CustomScrollPosition;
      return new Point(p_AbsolutePoint.X + l_ScrollPos.X, p_AbsolutePoint.Y + l_ScrollPos.Y);
    }
    #endregion

    #region Add/Remove ScrollBars
    /// <summary>
    /// Remove the horizontal scrollbar
    /// </summary>
    protected virtual void RemoveHScrollBar()
    {
      if (horizontalScroll != null)
      {
        HScrollBar l_tmp = horizontalScroll;
        horizontalScroll = null;
        l_tmp.ValueChanged -= new EventHandler(HScroll_Change);
        Controls.Remove(l_tmp);
        l_tmp.Dispose();
        l_tmp = null;
      }
      oldHorizontalScrollValue = 0;
    }

    /// <summary>
    /// Remove the vertical scrollbar
    /// </summary>
    protected virtual void RemoveVScrollBar()
    {
      if (verticalScroll != null)
      {
        VScrollBar l_tmp = verticalScroll;
        verticalScroll = null;
        l_tmp.ValueChanged -= new EventHandler(VScroll_Change);
        Controls.Remove(l_tmp);
        l_tmp.Dispose();
        l_tmp = null;
      }
      oldVerticalScrollValue = 0;
    }

    /// <summary>
    /// Insert the horizontal scroll bar
    /// </summary>
    protected virtual void InsertHScrollBar()
    {
      if (horizontalScroll == null)
      {
        horizontalScroll = new HScrollBar();
        horizontalScroll.TabStop = false;
        horizontalScroll.ValueChanged += new EventHandler(HScroll_Change);
        Controls.Add(horizontalScroll);
      }
    }
    /// <summary>
    /// Insert the vertical scroll bar
    /// </summary>
    protected virtual void InsertVScrollBar()
    {
      if (verticalScroll == null)
      {
        verticalScroll = new VScrollBar();
        verticalScroll.TabStop = false;
        verticalScroll.ValueChanged += new EventHandler(VScroll_Change);
        Controls.Add(verticalScroll);
      }
    }

    /// <summary>
    /// recalculate the position of the horizontal scrollbar
    /// </summary>
    protected virtual void RecalcHScrollBar()
    {
      if (horizontalScroll != null)
      {
        int l_WidthVScroll = 0;
        if (verticalScroll != null)
        {
          l_WidthVScroll = verticalScroll.Width;
        }

        horizontalScroll.Location = new Point(0, ClientRectangle.Height - horizontalScroll.Height);
        horizontalScroll.Width = ClientRectangle.Width - l_WidthVScroll;
        horizontalScroll.Minimum = 0;
        horizontalScroll.Maximum = customScrollArea.Width > 0 ? customScrollArea.Width : 0;
        horizontalScroll.LargeChange = Math.Max(5, ClientRectangle.Width - l_WidthVScroll);
        horizontalScroll.SmallChange = horizontalScroll.LargeChange / 5;

        if (horizontalScroll.Value > MaximumHScroll)
        {
          horizontalScroll.Value = MaximumHScroll;
        }

        horizontalScroll.BringToFront();
      }
    }

    /// <summary>
    /// Recalculate the position of the vertical scrollbar
    /// </summary>
    protected virtual void RecalcVScrollBar()
    {
      if (verticalScroll != null)
      {
        int l_HeightHScroll = 0;
        if (horizontalScroll != null)
        {
          l_HeightHScroll = horizontalScroll.Height;
        }

        verticalScroll.Location = new Point(ClientRectangle.Width - verticalScroll.Width, 0);
        verticalScroll.Height = ClientRectangle.Height - l_HeightHScroll;
        verticalScroll.Minimum = 0;
        verticalScroll.Maximum = customScrollArea.Height > 0 ? customScrollArea.Height : 0;
        verticalScroll.LargeChange = Math.Max(5, ClientRectangle.Height - l_HeightHScroll);
        verticalScroll.SmallChange = verticalScroll.LargeChange / 5;

        if (verticalScroll.Value > MaximumVScroll)
        {
          verticalScroll.Value = MaximumVScroll;
        }

        verticalScroll.BringToFront();
      }
    }

    /// <summary>
    /// Recalculate the scrollbars position and size
    /// </summary>
    protected virtual void RecalcCustomScrollBars()
    {
      // Check the size of the control
      Size l_BaseDisplaySize = base.DisplayRectangle.Size; // il base non tiene conto delle scrollbar ed è quello che voglio
      if (l_BaseDisplaySize.Height < customScrollArea.Height)
      {
        InsertVScrollBar();
      }
      else
      {
        RemoveVScrollBar();
      }
      if (l_BaseDisplaySize.Width < customScrollArea.Width)
      {
        InsertHScrollBar();
      }
      else
      {
        RemoveHScrollBar();
      }
      // Re-Check with the size of the ClientRectangle (that if before I added a scrollbar is smaller then the Size)
      Size l_DisplaySize = DisplayRectangle.Size;
      if (l_DisplaySize.Height < customScrollArea.Height)
      {
        InsertVScrollBar();
      }
      if (l_DisplaySize.Width < customScrollArea.Width)
      {
        InsertHScrollBar();
      }

      // if there is only one or zero scrollbar remove the BottomRightPanel
      if (horizontalScroll == null || verticalScroll == null)
      {
        if (bottomRightPanel != null)
        {
          Controls.Remove(bottomRightPanel);
          bottomRightPanel.Dispose();
          bottomRightPanel = null;
        }
      }

      RecalcVScrollBar();
      RecalcHScrollBar();

      //se sono tutti e due abilitati aggiungo il BottomRightPanel
      if (horizontalScroll != null && verticalScroll != null)
      {
        if (bottomRightPanel == null)
        {
          bottomRightPanel = new Panel();
          bottomRightPanel.TabStop = false;
          bottomRightPanel.BackColor = Color.FromKnownColor(KnownColor.Control);
          Controls.Add(bottomRightPanel);
        }
        bottomRightPanel.Location = new Point(horizontalScroll.Right, verticalScroll.Bottom);
        bottomRightPanel.Size = new Size(verticalScroll.Width, horizontalScroll.Height);
        bottomRightPanel.BringToFront();
      }

      // forzo un ridisegno
      InvalidateScrollableArea();
    }
    #endregion

    #region Maximum/Minimum Scroll Position
    /// <summary>
    /// Gets the maximum position that can be scrolled
    /// </summary>
    /// <value>The maximum vertical scroll.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int MaximumVScroll
    {
      get
      {
        if (verticalScroll == null || verticalScroll.Maximum - verticalScroll.LargeChange < 1)
        {
          return 0;
        }
        return verticalScroll.Maximum - verticalScroll.LargeChange;
      }
    }

    /// <summary>
    /// Gets the minimum position that can be scrolled
    /// </summary>
    /// <value>The minimum vertical scroll.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int MinimumVScroll
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    /// Gets the minimum position that can be scrolled
    /// </summary>
    /// <value>The minimum horizontal scroll.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int MinimumHScroll
    {
      get { return 0; }
    }

    /// <summary>
    /// Gets the maximum position that can be scrolled
    /// </summary>
    /// <value>The maximum horizontal scroll.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int MaximumHScroll
    {
      get
      {
        if (horizontalScroll == null || horizontalScroll.Maximum - horizontalScroll.LargeChange < 1)
        {
          return 0;
        }
        return horizontalScroll.Maximum - horizontalScroll.LargeChange;
      }
    }
    #endregion

    #region Layout Events
    /// <summary>
    /// OnLayout Method
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.LayoutEventArgs"/> instance containing the event data.</param>
    protected override void OnLayout(LayoutEventArgs e)
    {
      base.OnLayout(e);

      RecalcCustomScrollBars();
    }
    #endregion

    #region ScrollChangeEvent
    private void VScroll_Change(object sender, EventArgs e)
    {
      OnVScrollPositionChanged(new ScrollPositionChangedEventArgs(-verticalScroll.Value, -oldVerticalScrollValue));

      InvalidateScrollableArea();
      oldVerticalScrollValue = verticalScroll.Value;
    }

    private void HScroll_Change(object sender, EventArgs e)
    {
      OnHScrollPositionChanged(new ScrollPositionChangedEventArgs(-horizontalScroll.Value, -oldHorizontalScrollValue));

      InvalidateScrollableArea();
      oldHorizontalScrollValue = horizontalScroll.Value;
    }

    /// <summary>
    /// Fired when the scroll vertical position change
    /// </summary>
    public event ScrollPositionChangedEventHandler VScrollPositionChanged;
    /// <summary>
    /// Fired when the scroll vertical posizion change
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ScrollPositionChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnVScrollPositionChanged(ScrollPositionChangedEventArgs e)
    {
      if (VScrollPositionChanged != null)
      {
        VScrollPositionChanged(this, e);
      }
    }

    /// <summary>
    /// Fired when the scroll horizontal posizion change
    /// </summary>
    public event ScrollPositionChangedEventHandler HScrollPositionChanged;
    /// <summary>
    /// Fired when the scroll horizontal posizion change
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ScrollPositionChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnHScrollPositionChanged(ScrollPositionChangedEventArgs e)
    {
      if (HScrollPositionChanged != null)
      {
        HScrollPositionChanged(this, e);
      }
    }
    #endregion

    #region Scroll PageDown/Up/Right/Left/LineUp/Down/Right/Left
    /// <summary>
    /// Scroll the page down
    /// </summary>
    public virtual void CustomScrollPageDown()
    {
      if (verticalScroll != null)
      {
        verticalScroll.Value = Math.Min(verticalScroll.Value + verticalScroll.LargeChange, verticalScroll.Maximum - verticalScroll.LargeChange);
      }
    }

    /// <summary>
    /// Scroll the page up
    /// </summary>
    public virtual void CustomScrollPageUp()
    {
      if (verticalScroll != null)
      {
        verticalScroll.Value = Math.Max(verticalScroll.Value - verticalScroll.LargeChange, verticalScroll.Minimum);
      }
    }

    /// <summary>
    /// Scroll the page right
    /// </summary>
    public virtual void CustomScrollPageRight()
    {
      if (horizontalScroll != null)
      {
        horizontalScroll.Value = Math.Min(horizontalScroll.Value + horizontalScroll.LargeChange, horizontalScroll.Maximum - horizontalScroll.LargeChange);
      }
    }

    /// <summary>
    /// Scroll the page left
    /// </summary>
    public virtual void CustomScrollPageLeft()
    {
      if (horizontalScroll != null)
      {
        horizontalScroll.Value = Math.Max(horizontalScroll.Value - horizontalScroll.LargeChange, horizontalScroll.Minimum);
      }
    }

    /// <summary>
    /// Scroll the page down one line
    /// </summary>
    public virtual void CustomScrollLineDown()
    {
      if (verticalScroll != null)
      {
        verticalScroll.Value = Math.Min(verticalScroll.Value + verticalScroll.SmallChange, verticalScroll.Maximum);
      }
    }

    /// <summary>
    /// Scroll the page up one line
    /// </summary>
    public virtual void CustomScrollLineUp()
    {
      if (verticalScroll != null)
      {
        verticalScroll.Value = Math.Max(verticalScroll.Value - verticalScroll.SmallChange, verticalScroll.Minimum);
      }
    }

    /// <summary>
    /// Scroll the page right one line
    /// </summary>
    public virtual void CustomScrollLineRight()
    {
      if (horizontalScroll != null)
      {
        horizontalScroll.Value = Math.Min(horizontalScroll.Value + horizontalScroll.SmallChange, horizontalScroll.Maximum);
      }
    }

    /// <summary>
    /// Scroll the page left one line
    /// </summary>
    public virtual void CustomScrollLineLeft()
    {
      if (horizontalScroll != null)
      {
        horizontalScroll.Value = Math.Max(horizontalScroll.Value - horizontalScroll.SmallChange, horizontalScroll.Minimum);
      }
    }
    #endregion
  }
}