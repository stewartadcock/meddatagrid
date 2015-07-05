#region MIT License
//
// Filename: ResizeBehaviorModel.cs
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

using System.Drawing;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// Implement the mouse resize features of a cell.
  /// </summary>
  /// <remarks>
  /// This behavior can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class ResizeBehaviorModel : BehaviorModelGroup
  {
    #region Constants
    private const int MouseDelta = 4;
    #endregion

    #region Class members
    /// <summary>
    /// Resize both width nd height behavior
    /// </summary>
    public static readonly ResizeBehaviorModel ResizeBoth = new ResizeBehaviorModel(CellResizeModes.Both);

    /// <summary>
    /// Resize width behavior
    /// </summary>
    public static readonly ResizeBehaviorModel ResizeWidth = new ResizeBehaviorModel(CellResizeModes.Width);

    /// <summary>
    /// Resize height behavior
    /// </summary>
    public static readonly ResizeBehaviorModel ResizeHeight = new ResizeBehaviorModel(CellResizeModes.Height);

    /// <summary>
    /// Whether left mouse button is currently down.
    /// </summary>
    private static bool isMouseLDown = false;

    private CellResizeModes resizeMode = CellResizeModes.Both;

    private CursorBehaviorModel cursorBehaviorModel = new CursorBehaviorModel();

    // variables indicating whether the resize has to be done
    private bool isWidthResizing = false;
    private bool isHeightResizing = false;
    #endregion

    #region Constrcutor
    /// <summary>
    /// Initializes a new instance of the <see cref="ResizeBehaviorModel"/> class.
    /// </summary>
    /// <param name="p_Mode">The mode.</param>
    public ResizeBehaviorModel(CellResizeModes p_Mode)
    {
      resizeMode = p_Mode;
      SubModels.Add(cursorBehaviorModel);
    }
    #endregion

    #region IBehaviorModel Members
    public override void OnMouseDown(PositionMouseEventArgs e)
    {
      base.OnMouseDown(e);

      isHeightResizing = false;
      isWidthResizing = false;

      Rectangle l_CellRect = e.Grid.PositionToDisplayRect(e.Position);
      Point l_MousePoint = new Point(e.MouseEventArgs.X, e.MouseEventArgs.Y);

      if (((ResizeMode & CellResizeModes.Width) == CellResizeModes.Width) &&
            IsInHorizontalResizeRegion(l_CellRect, l_MousePoint))
      {
        isWidthResizing = true;
      }
      else if (((ResizeMode & CellResizeModes.Height) == CellResizeModes.Height) &&
            IsInVerticalResizeRegion(l_CellRect, l_MousePoint))
      {
        isHeightResizing = true;
      }

      if (e.MouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left)
      {
        isMouseLDown = true;
      }
    }

    public override void OnMouseUp(PositionMouseEventArgs e)
    {
      base.OnMouseUp(e);

      isWidthResizing = false;
      isHeightResizing = false;

      if (e.MouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left)
      {
        isMouseLDown = true;
      }
    }

    public override void OnMouseMove(PositionMouseEventArgs e)
    {
      base.OnMouseMove(e);

      Rectangle l_CellRect = e.Grid.PositionToDisplayRect(e.Position);
      Point l_MousePoint = new Point(e.MouseEventArgs.X, e.MouseEventArgs.Y);

      Point l_AbsPoint = e.Grid.PointRelativeToAbsolute(l_MousePoint);
      if (e.Position.Row < e.Grid.FixedRows)
      {
        l_AbsPoint.Y = l_MousePoint.Y;
      }
      if (e.Position.Column < e.Grid.FixedColumns)
      {
        l_AbsPoint.X = l_MousePoint.X;
      }

      // Resize the column or the row
      if (e.Grid.MouseDownPosition == e.Position)
      {
        if (isWidthResizing)
        {
          if (e.Grid.Columns[e.Position.Column].Resizable == true)
          {
            int l_NewWidth = l_AbsPoint.X - e.Grid.Columns[e.Position.Column].Left;

            if (l_NewWidth > 0)
            {
              e.Grid.Columns[e.Position.Column].Width = l_NewWidth + MouseDelta;
            }
          }
        }
        else if (isHeightResizing)
        {
          int l_NewHeight = l_AbsPoint.Y - e.Grid.Rows[e.Position.Row].Top;

          if (l_NewHeight > 0)
          {
            e.Grid.Rows[e.Position.Row].Height = l_NewHeight + MouseDelta;
          }
        }
      }

      // mis a jour avec le curseur
      if ((IsInHorizontalResizeRegion(l_CellRect, l_MousePoint) || (e.MouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left && isMouseLDown == true)) && (ResizeMode & CellResizeModes.Width) == CellResizeModes.Width)
      {
        e.Cell.Grid.GridCursor = System.Windows.Forms.Cursors.VSplit;
      }
      else if ((IsInVerticalResizeRegion(l_CellRect, l_MousePoint) || (e.MouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left && isMouseLDown == true)) && (ResizeMode & CellResizeModes.Height) == CellResizeModes.Height)
      {
        e.Cell.Grid.GridCursor = System.Windows.Forms.Cursors.HSplit;
      }
      else
      {
        e.Cell.Grid.GridCursor = null;
      }
    }

    public override void OnMouseLeave(PositionEventArgs e)
    {
      base.OnMouseLeave(e);

      e.Cell.Grid.GridCursor = null;
      isWidthResizing = false;
      isHeightResizing = false;
    }

    public override void OnDoubleClick(PositionEventArgs e)
    {
      base.OnDoubleClick(e);

      Point l_Current = e.Grid.PointToClient(System.Windows.Forms.Control.MousePosition);
      Rectangle l_CellRect = e.Grid.PositionToDisplayRect(e.Position);

      if ((ResizeMode & CellResizeModes.Width) == CellResizeModes.Width &&
        IsInHorizontalResizeRegion(l_CellRect, l_Current))
      {
        e.Grid.AutoSizeColumn(e.Position.Column, e.Grid.AutoSizeMinWidth);
      }
      else if ((ResizeMode & CellResizeModes.Height) == CellResizeModes.Height &&
        IsInVerticalResizeRegion(l_CellRect, l_Current))
      {
        e.Grid.AutoSizeRow(e.Position.Row, e.Grid.AutoSizeMinHeight);
      }
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the resize mode of the cell
    /// </summary>
    /// <value>The resize mode.</value>
    public CellResizeModes ResizeMode
    {
      get { return this.resizeMode; }
    }

    /// <summary>
    /// Gets a value indicating whether the behavior is currently resizing a cell width
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is width resizing; otherwise, <c>false</c>.
    /// </value>
    public bool IsWidthResizing
    {
      get { return this.isWidthResizing; }
    }

    /// <summary>
    /// Gets a value indicating whether the behavior is currently resizing a cell height
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is height resizing; otherwise, <c>false</c>.
    /// </value>
    public bool IsHeightResizing
    {
      get { return this.isHeightResizing; }
    }
    #endregion

    #region Support Functions
    /// <summary>
    /// Determines whether the specified cell rectangle is in horizontal resize region.
    /// </summary>
    /// <param name="p_CellRectangle">A grid relative rectangle</param>
    /// <param name="p">The p.</param>
    /// <returns>
    ///   <c>true</c> if the specified cell rectangle is in horizontal resize region; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInHorizontalResizeRegion(Rectangle p_CellRectangle, Point p)
    {
      return p.X >= p_CellRectangle.Right - MouseDelta * 1.5
        && p.X <= p_CellRectangle.Right + MouseDelta * 1.5;
    }

    /// <summary>
    /// Determines whether the specified cell rectangle is in vertical resize region.
    /// </summary>
    /// <param name="p_CellRectangle">A grid relative rectangle</param>
    /// <param name="point">The point.</param>
    /// <returns>
    ///   <c>true</c> if the specified cell rectangle is in vertical resize region; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsInVerticalResizeRegion(Rectangle p_CellRectangle, Point point)
    {
      return point.Y >= p_CellRectangle.Bottom - MouseDelta && point.Y <= p_CellRectangle.Bottom;
    }
    #endregion
  }
}