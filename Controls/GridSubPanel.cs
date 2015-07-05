#region MIT License
//
// Filename: GridSubPanel.cs
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// Grid Sub-Panel.
  /// </summary>
  [ComVisible(false), System.ComponentModel.ToolboxItem(false)]
  public class GridSubPanel : UserControl
  {
    #region GridSubPanelType enum
    private enum GridSubPanelType
    {
      TopLeft,
      Top,
      Left,
      Scrollable
    }
    #endregion

    private ControlsRepository controlsRepository;
    private GridVirtual gridContainer;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="GridSubPanel"/> class.
    /// </summary>
    /// <param name="p_GridContainer">The grid container.</param>
    /// <param name="p_CustomDraw">True to use custom draw function.</param>
    public GridSubPanel(GridVirtual p_GridContainer, bool p_CustomDraw)
    {
      this.toolTip = new System.Windows.Forms.ToolTip();

      ToolTipText = string.Empty;

      if (p_CustomDraw)
      {
        // to remove flicker and use custom draw
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.Opaque, true);
        //SetStyle(ControlStyles.ResizeRedraw, true);
      }

      this.gridContainer = p_GridContainer;

      this.controlsRepository = new ControlsRepository(this);
    }
    #endregion

    #region Grid
    /// <summary>
    /// Gets the grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.gridContainer; }
    }
    #endregion

    #region InputKeys
    /// <summary>
    /// Allow the grid to handle specials keys like Arrows and Tab. See also Grid.SpecialKeys
    /// </summary>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values.</param>
    /// <returns>
    /// <c>true</c> if the specified key is a regular input key; otherwise, <c>false</c>.
    /// </returns>
    protected override bool IsInputKey(Keys keyData)
    {
      //serve per poter gestire le freccie e tab

      if ((Grid.SpecialKeys & GridSpecialKeys.Arrows) == GridSpecialKeys.Arrows)
      {
        switch (keyData)
        {
          case Keys.Up:
          case Keys.Down:
          case Keys.Left:
          case Keys.Right:
          case Keys.Up | Keys.Shift:
          case Keys.Down | Keys.Shift:
          case Keys.Left | Keys.Shift:
          case Keys.Right | Keys.Shift:
            return true;
        }
      }

      if ((Grid.SpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab)
      {
        switch (keyData)
        {
          case Keys.Tab:
          case Keys.Tab | Keys.Shift:
            return true;
        }
      }

      return base.IsInputKey(keyData);
    }
    #endregion

    #region ToolTip
    private System.Windows.Forms.ToolTip toolTip;
    /// <summary>
    /// Gets or sets the tool tip text.
    /// </summary>
    /// <value>The tool tip text.</value>
    public string ToolTipText
    {
      get { return toolTip.GetToolTip(this); }
      set { toolTip.SetToolTip(this, value); }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tool tip is active.
    /// </summary>
    /// <value><c>true</c> if the tool tip is active; otherwise, <c>false</c>.</value>
    public bool ToolTipActive
    {
      get { return toolTip.Active; }
      set { toolTip.Active = value; }
    }
    #endregion

    #region Cells Search
    /// <summary>
    /// Returns the cell at the specified grid view relative point (the point must be relative to the grid display region not to the panel display region)
    /// </summary>
    /// <param name="p_RelativeViewPoint">Point</param>
    /// <returns></returns>
    public virtual Position PositionAtPoint(Point p_RelativeViewPoint)
    {
      int l_Row, l_Col;
      GridSubPanelType l_Type = ContainerType;

      if (l_Type == GridSubPanelType.TopLeft)
      {
        l_Row = gridContainer.Rows.RowAtPoint(p_RelativeViewPoint.Y, true);
        l_Col = gridContainer.Columns.ColumnAtPoint(p_RelativeViewPoint.X, true);
        if (l_Row >= gridContainer.FixedRows)
        {
          l_Row = Position.EmptyIndex;
        }
        if (l_Col >= gridContainer.FixedColumns)
        {
          l_Col = Position.EmptyIndex;
        }
      }
      else if (l_Type == GridSubPanelType.Top)
      {
        Point l_AbsPoint = gridContainer.PointRelativeToAbsolute(p_RelativeViewPoint);
        l_Row = gridContainer.Rows.RowAtPoint(p_RelativeViewPoint.Y, true);
        l_Col = gridContainer.Columns.ColumnAtPoint(l_AbsPoint.X, true);
        if (l_Row >= gridContainer.FixedRows)
        {
          l_Row = Position.EmptyIndex;
        }

        if (l_Col < gridContainer.FixedColumns)
        {
          l_Col = Position.EmptyIndex;
        }
      }
      else if (l_Type == GridSubPanelType.Left)
      {
        Point l_AbsPoint = gridContainer.PointRelativeToAbsolute(p_RelativeViewPoint);
        l_Row = gridContainer.Rows.RowAtPoint(l_AbsPoint.Y, true);
        l_Col = gridContainer.Columns.ColumnAtPoint(p_RelativeViewPoint.X, true);
        if (l_Col >= gridContainer.FixedColumns)
        {
          l_Col = Position.EmptyIndex;
        }

        if (l_Row < gridContainer.FixedRows)
        {
          l_Row = Position.EmptyIndex;
        }
      }
      else //scrollable
      {
        Point l_AbsPoint = gridContainer.PointRelativeToAbsolute(p_RelativeViewPoint);
        l_Row = gridContainer.Rows.RowAtPoint(l_AbsPoint.Y, true);
        l_Col = gridContainer.Columns.ColumnAtPoint(l_AbsPoint.X, true);
      }

      if (l_Row == Position.EmptyIndex || l_Col == Position.EmptyIndex)
      {
        return Position.Empty;
      }

      return new Position(l_Row, l_Col);
    }

    /// <summary>
    /// Returns a range of cells from the specified rectangle, relative to the current grid container, using scrolling information.
    /// </summary>
    /// <param name="p_RelativeRect">A grid relative rectangle (not panel relative)</param>
    /// <returns></returns>
    public Range RangeAtDisplayRect(Rectangle p_RelativeRect)
    {
      Rectangle l_Absolute = gridContainer.RectangleRelativeToAbsolute(p_RelativeRect);
      GridSubPanelType l_Type = ContainerType;

      if (l_Type == GridSubPanelType.TopLeft)
      {
        return RangeAtAbsRect(new Rectangle(p_RelativeRect.X, p_RelativeRect.Y, l_Absolute.Width, l_Absolute.Height));
      }
      else if (l_Type == GridSubPanelType.Top)
      {
        return RangeAtAbsRect(new Rectangle(l_Absolute.X, p_RelativeRect.Y, l_Absolute.Width, l_Absolute.Height));
      }
      else if (l_Type == GridSubPanelType.Left)
      {
        return RangeAtAbsRect(new Rectangle(p_RelativeRect.X, l_Absolute.Y, l_Absolute.Width, l_Absolute.Height));
      }
      else//scrollable
      {
        return RangeAtAbsRect(l_Absolute);
      }
    }

    /// <summary>
    /// Returns a range of cells from the specified absolute rectangle. Returns Empty if no valid cells are found.
    /// </summary>
    /// <param name="p_AbsoluteRect">The absolute rectangle.</param>
    /// <returns></returns>
    public Range RangeAtAbsRect(Rectangle p_AbsoluteRect)
    {
      int l_Start_R, l_Start_C, l_End_R, l_End_C;
      GridSubPanelType l_Type = ContainerType;

      Range l_tmp = gridContainer.RangeAtAbsRect(p_AbsoluteRect);
      if (l_tmp.IsEmpty())
      {
        return l_tmp;
      }
      l_Start_R = l_tmp.Start.Row;
      l_Start_C = l_tmp.Start.Column;

      l_End_R = l_tmp.End.Row;
      l_End_C = l_tmp.End.Column;

      if (l_Type == GridSubPanelType.TopLeft)
      {
        if (l_Start_R >= gridContainer.FixedRows)
        {
          l_Start_R = Position.EmptyIndex;
        }

        if (l_Start_C >= gridContainer.FixedColumns)
        {
          l_Start_C = Position.EmptyIndex;
        }

        if (l_End_R >= gridContainer.FixedRows)
        {
          l_End_R = gridContainer.FixedRows - 1;
        }

        if (l_End_C >= gridContainer.FixedColumns)
        {
          l_End_C = gridContainer.FixedColumns - 1;
        }
      }
      else if (l_Type == GridSubPanelType.Top)
      {
        if (l_Start_R >= gridContainer.FixedRows)
        {
          l_Start_R = Position.EmptyIndex;
        }

        if (l_End_R >= gridContainer.FixedRows)
        {
          l_End_R = gridContainer.FixedRows - 1;
        }

        if (l_Start_C < gridContainer.FixedColumns)
        {
          l_Start_C = gridContainer.FixedColumns;
          if (l_Start_C >= gridContainer.ColumnsCount)
          {
            l_Start_C = Position.EmptyIndex;
          }
        }
        if (l_End_C < gridContainer.FixedColumns)
        {
          l_End_C = gridContainer.FixedColumns;
          if (l_End_C >= gridContainer.ColumnsCount)
          {
            l_End_C = Position.EmptyIndex;
          }
        }
      }
      else if (l_Type == GridSubPanelType.Left)
      {
        if (l_Start_C >= gridContainer.FixedColumns)
        {
          l_Start_C = Position.EmptyIndex;
        }

        if (l_End_C >= gridContainer.FixedColumns)
        {
          l_End_C = gridContainer.FixedColumns - 1;
        }

        if (l_Start_R < gridContainer.FixedRows)
        {
          l_Start_R = gridContainer.FixedRows;
          if (l_Start_R >= gridContainer.RowsCount)
          {
            l_Start_R = Position.EmptyIndex;
          }
        }
        if (l_End_R < gridContainer.FixedRows)
        {
          l_End_R = gridContainer.FixedRows;
          if (l_End_R >= gridContainer.RowsCount)
          {
            l_End_R = Position.EmptyIndex;
          }
        }
      }

      if (l_Start_R == Position.EmptyIndex || l_Start_C == Position.EmptyIndex
        || l_End_C == Position.EmptyIndex || l_End_R == Position.EmptyIndex)
      {
        return Range.Empty;
      }

      return new Range(l_Start_R, l_Start_C, l_End_R, l_End_C);
    }
    #endregion

    #region Point and Rectangle Conversion
    /// <summary>
    /// Convert a grid relative point to a panel relative point
    /// </summary>
    /// <param name="p_GridPoint">The grid point.</param>
    /// <returns></returns>
    public Point PointGridToPanel(Point p_GridPoint)
    {
      return new Point(p_GridPoint.X - Left, p_GridPoint.Y - Top);
    }

    /// <summary>
    /// Convert a panel relative point to a grid relative point
    /// </summary>
    /// <param name="p_PanelPoint">The panel point.</param>
    /// <returns></returns>
    public Point PointPanelToGrid(Point p_PanelPoint)
    {
      return new Point(p_PanelPoint.X + Left, p_PanelPoint.Y + Top);
    }

    /// <summary>
    /// Converts a panel coordinate rectangle to Grid coordinate rectangle
    /// </summary>
    /// <param name="p_PanelRectangle">The panel rectangle.</param>
    /// <returns></returns>
    public Rectangle RectanglePanelToGrid(Rectangle p_PanelRectangle)
    {
      return new Rectangle(PointPanelToGrid(p_PanelRectangle.Location), p_PanelRectangle.Size);
    }

    /// <summary>
    /// Converts a grid coordinate rectangle to Panel coordinate rectangle
    /// </summary>
    /// <param name="p_GridRectangle">The grid rectangle.</param>
    /// <returns></returns>
    public Rectangle RectangleGridToPanel(Rectangle p_GridRectangle)
    {
      return new Rectangle(PointGridToPanel(p_GridRectangle.Location), p_GridRectangle.Size);
    }
    #endregion

    #region ContainerType
    private GridSubPanelType ContainerType
    {
      get
      {
        if (gridContainer.LeftPanel == this)
        {
          return GridSubPanelType.Left;
        }
        else if (gridContainer.TopPanel == this)
        {
          return GridSubPanelType.Top;
        }
        else if (gridContainer.TopLeftPanel == this)
        {
          return GridSubPanelType.TopLeft;
        }
        else
        {
          return GridSubPanelType.Scrollable;
        }
      }
    }
    #endregion

    #region Events Dispatcher
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      GridSubPanelType l_Type = ContainerType;

      switch (l_Type)
      {
        case GridSubPanelType.TopLeft:
          gridContainer.OnTopLeftPanelPaint(e);
          break;
        case GridSubPanelType.Top:
          gridContainer.OnTopPanelPaint(e);
          break;
        case GridSubPanelType.Left:
          gridContainer.OnLeftPanelPaint(e);
          break;
        default:
          gridContainer.OnScrollablePanelPaint(e);
          break;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseDown"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      Point l_GridPoint = PointPanelToGrid(new Point(e.X, e.Y));
      MouseEventArgs l_MouseArgs = new MouseEventArgs(e.Button, e.Clicks, l_GridPoint.X, l_GridPoint.Y, e.Delta);
      gridContainer.OnGridMouseDown(l_MouseArgs);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      Point l_GridPoint = PointPanelToGrid(new Point(e.X, e.Y));
      MouseEventArgs l_MouseArgs = new MouseEventArgs(e.Button, e.Clicks, l_GridPoint.X, l_GridPoint.Y, e.Delta);
      gridContainer.OnGridMouseUp(l_MouseArgs);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      Point l_GridPoint = PointPanelToGrid(new Point(e.X, e.Y));
      MouseEventArgs l_MouseArgs = new MouseEventArgs(e.Button, e.Clicks, l_GridPoint.X, l_GridPoint.Y, e.Delta);
      gridContainer.OnGridMouseMove(l_MouseArgs);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Click"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnClick(EventArgs e)
    {
      base.OnClick(e);
      gridContainer.OnGridClick(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnDoubleClick(EventArgs e)
    {
      base.OnDoubleClick(e);
      gridContainer.OnGridDoubleClick(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);
      gridContainer.OnGridKeyDown(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
    protected override void OnKeyUp(KeyEventArgs e)
    {
      base.OnKeyUp(e);
      gridContainer.OnGridKeyUp(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"></see> that contains the event data.</param>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      base.OnKeyPress(e);
      gridContainer.OnGridKeyPress(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"></see> event.
    /// </summary>
    /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
    protected override void OnDragDrop(DragEventArgs drgevent)
    {
      base.OnDragDrop(drgevent);
      gridContainer.OnPanelDragDrop(drgevent);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"></see> event.
    /// </summary>
    /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
    protected override void OnDragEnter(DragEventArgs drgevent)
    {
      base.OnDragEnter(drgevent);
      gridContainer.OnPanelDragEnter(drgevent);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragLeave"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnDragLeave(EventArgs e)
    {
      base.OnDragLeave(e);
      gridContainer.OnPanelDragLeave(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.DragOver"></see> event.
    /// </summary>
    /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
    protected override void OnDragOver(DragEventArgs drgevent)
    {
      base.OnDragOver(drgevent);
      gridContainer.OnPanelDragOver(drgevent);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      gridContainer.OnPanelMouseEnter(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseHover"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnMouseHover(EventArgs e)
    {
      base.OnMouseHover(e);
      gridContainer.OnGridMouseHover(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      gridContainer.OnPanelMouseLeave(e);
    }
    #endregion

    #region ControlsRepository
    /// <summary>
    /// Gets a collection of controls used for editing operations
    /// </summary>
    /// <value>The controls repository.</value>
    public ControlsRepository ControlsRepository
    {
      get { return this.controlsRepository; }
    }
    #endregion
  }
}