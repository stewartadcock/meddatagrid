#region MIT License
//
// Filename: Header.cs
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
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// Header VisualModel
  /// </summary>
  [ComVisible(false)]
  public class Header : Common
  {
    /// <summary>
    /// Represents a default HeaderCell, with a 3D border and a LightGray BackColor
    /// </summary>
    public new static readonly Header Default = new Header(true);

    /// <summary>
    /// Represents a Column HeaderCell with the ability to draw an Image in the right to indicates the sort operation. You must use this model with a cell of type ICellSortableHeader.
    /// </summary>
    public static readonly Header ColumnHeader;

    /// <summary>
    /// Represents a Row HeaderCell.
    /// </summary>
    public static readonly Header RowHeader;

    private Color headerShadowColor = Color.FromKnownColor(KnownColor.ControlDark);
    private Color headerLightColor = Color.White; // Color.FromKnownColor(KnownColor.ControlLight);
    private int headerTopLeftWidth = 4;
    private int headerBottomRightWidth = 3;
    private CommonBorderStyle borderStyle = CommonBorderStyle.Raised;

    #region Constructors
    /// <summary>
    /// Initializes the <see cref="Header"/> class.
    /// </summary>
    static Header()
    {
      ColumnHeader = new Header(false);
      ColumnHeader.MakeReadOnly();

      RowHeader = new Header(false);
      RowHeader.TextAlignment = ContentAlignment.MiddleCenter;
      RowHeader.MakeReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Header"/> class.
    /// Use default setting and construct a read and write VisualProperties
    /// </summary>
    public Header()
      : this(false)
    {
      this.ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Header"/> class.
    /// Use default setting
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public Header(bool isReadOnly)
    {
      this.BackColor = Color.FromKnownColor(KnownColor.Control);
      this.SyncBorders();
      this.isReadOnly = isReadOnly;
      this.ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Header"/> class.
    /// </summary>
    /// <remarks>
    /// Copy Constructor.  This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </remarks>
    /// <param name="source">The source.</param>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public Header(Header source, bool isReadOnly)
      : base(source, isReadOnly)
    {
      this.borderStyle = source.borderStyle;
      this.headerTopLeftWidth = source.headerTopLeftWidth;
      this.headerBottomRightWidth = source.headerBottomRightWidth;
      this.headerLightColor = source.headerLightColor;
      this.headerShadowColor = source.headerShadowColor;
      this.ExpandedCell = false;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the specified dark color of this cell for 3D effects (BorderStyle)
    /// </summary>
    /// <value>The color of the header shadow.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color HeaderShadowColor
    {
      get
      {
        return this.headerShadowColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.headerShadowColor = value;
        SyncBorders();
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the Cell Border Style
    /// </summary>
    /// <value>The border style.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public virtual CommonBorderStyle BorderStyle
    {
      get
      {
        return this.borderStyle;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.borderStyle = value;
        SyncBorders();

        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the specified light color of this cell for 3D effects (BorderStyle)
    /// </summary>
    /// <value>The color of the header light.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color HeaderLightColor
    {
      get
      {
        return this.headerLightColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.headerLightColor = value;
        SyncBorders();

        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the width of the header light border.
    /// This is the pecified the width of the border for 3D effects (BorderStyle)
    /// </summary>
    /// <value>The width of the header light border.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public int HeaderLightBorderWidth
    {
      get
      {
        return this.headerTopLeftWidth;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.headerTopLeftWidth = value;
        SyncBorders();

        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the width of the header shadow border.
    /// this is the specified the width of the border for 3D effects (BorderStyle)
    /// </summary>
    /// <value>The width of the header shadow border.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public int HeaderShadowBorderWidth
    {
      get
      {
        return this.headerBottomRightWidth;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.headerBottomRightWidth = value;
        SyncBorders();

        OnChange();
      }
    }
    #endregion

    /// <summary>
    /// Returns the minimum required size of the current cell, calculating using the
    /// current DisplayString, Image and Borders information.
    /// </summary>
    /// <param name="p_Graphics">GDI+ drawing surface</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <returns></returns>
    public override SizeF GetRequiredSize(Graphics p_Graphics,
      Cells.ICellVirtual p_Cell,
      Position p_CellPosition)
    {
      SizeF s = base.GetRequiredSize(p_Graphics, p_Cell, p_CellPosition);
      s.Width += IconUtility.SortUp.Width; // add the width of the sort image
      return s;
    }

    private void SyncBorders()
    {
      Border = new RectangleBorder(new Border(HeaderLightColor, HeaderLightBorderWidth),
                    new Border(HeaderShadowColor, HeaderShadowBorderWidth),
                    new Border(HeaderLightColor, HeaderLightBorderWidth),
                    new Border(HeaderShadowColor, HeaderShadowBorderWidth));
    }

    #region Clone
    /// <summary>
    /// Clone this object. This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </summary>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    public override object Clone(bool isReadOnly)
    {
      return new Header(this, isReadOnly);
    }
    #endregion

    /// <summary>
    /// Draw the borders of the specified cell using DrawGradient3DBorder
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_Border(Cells.ICellVirtual p_Cell, Position p_CellPosition, PaintEventArgs e, Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      Color l_BackColor;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_BackColor = FocusBackColor;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_BackColor = SelectionBackColor;
      }
      else
      {
        l_BackColor = BackColor;
      }

      if (p_CellPosition == p_Cell.Grid.MouseDownPosition)
      {
        DrawGradient3DBorder(e.Graphics, p_ClientRectangle, l_BackColor, HeaderShadowColor, HeaderLightColor, HeaderShadowBorderWidth, HeaderLightBorderWidth, Gradient3DBorderStyle.Sunken);
      }
      else
      {
        DrawGradient3DBorder(e.Graphics, p_ClientRectangle, l_BackColor, HeaderShadowColor, HeaderLightColor, HeaderShadowBorderWidth, HeaderLightBorderWidth, Gradient3DBorderStyle.Raised);
      }
    }

    /// <summary>
    /// Draw the background of the specified cell with gradient color. It changes the color when the mouse is upper the cell
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_Background(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status)
    {
      if (p_ClientRectangle.Width == 0 || p_ClientRectangle.Height == 0)
      {
        return;
      }

      Color menuItemBackColorStart;
      Color menuItemBackColorEnd;

      Position mouseCellPosition = p_Cell.Grid.MouseCellPosition;
      Brush backBrush;

      if ((p_Cell.Grid.FixedColumns > 0 && p_CellPosition.Column > p_Cell.Grid.FixedColumns - 1 && p_Cell.Grid.Columns[0].Width > 0)
       || (p_Cell.Grid.FixedRows > 0 && p_CellPosition.Row > p_Cell.Grid.FixedRows - 1 && p_Cell.Grid.Rows[0].Height > 0))
      {
        if (mouseCellPosition == p_CellPosition)
        {
          menuItemBackColorStart = Color.WhiteSmoke;
          menuItemBackColorEnd = Color.Orange;
        }
        else
        {
          menuItemBackColorStart = Color.WhiteSmoke;
          menuItemBackColorEnd = Color.Gray;
        }
      }
      else
      {
        Point mousePoint = p_Cell.Grid.PointToClient(Grid.MousePosition);  // peut etre source d'erreurs

        if ((mousePoint.X > p_ClientRectangle.Left && mousePoint.X < p_ClientRectangle.Right &&
          mousePoint.Y > p_ClientRectangle.Top && mousePoint.Y < p_ClientRectangle.Bottom) || mouseCellPosition == p_CellPosition)
        {
          menuItemBackColorStart = Color.WhiteSmoke;
          menuItemBackColorEnd = Color.Orange;
        }
        else
        {
          menuItemBackColorStart = Color.WhiteSmoke;
          menuItemBackColorEnd = Color.Gray;
        }
      }

      backBrush = new LinearGradientBrush(
        p_ClientRectangle,
        menuItemBackColorStart, menuItemBackColorEnd, LinearGradientMode.Vertical);
      e.Graphics.FillRectangle(backBrush, p_ClientRectangle);
    }

    /// <summary>
    /// Draw the image and the displaystring of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(Cells.ICellVirtual p_Cell, Position p_CellPosition, PaintEventArgs e, Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      base.DrawCell_ImageAndText(p_Cell, p_CellPosition, e, p_ClientRectangle, p_Status);

      if (p_Cell is ICellSortableHeader)
      {
        ICellSortableHeader l_Header = (ICellSortableHeader)p_Cell;
        SortStatus l_Status = l_Header.GetSortStatus(p_CellPosition);

        if (l_Status.EnableSort)
        {
          if (l_Status.Mode == GridSortMode.Ascending)
          {
            VisualModelBase.PaintImageAndText(e.Graphics, p_ClientRectangle, IconUtility.SortUp, ContentAlignment.MiddleRight, false, null, null, false, Border, Color.Black, null, false, false);
          }
          else if (l_Status.Mode == GridSortMode.Descending)
          {
            VisualModelBase.PaintImageAndText(e.Graphics, p_ClientRectangle, IconUtility.SortDown, ContentAlignment.MiddleRight, false, null, null, false, Border, Color.Black, null, false, false);
          }
        }
      }
    }

    #region Gradient 3D border drawing methods
    /// <summary>
    /// Draw a 3D border inside the specified rectangle using a linear gradient border color.
    /// </summary>
    /// <param name="g">The grapics.</param>
    /// <param name="p_HeaderRectangle">The header rectangle.</param>
    /// <param name="p_BackColor">Color of the back.</param>
    /// <param name="p_DarkColor">Color of the dark.</param>
    /// <param name="p_LightColor">Color of the light.</param>
    /// <param name="p_DarkGradientNumber">The width of the dark border</param>
    /// <param name="p_LightGradientNumber">The width of the light border</param>
    /// <param name="p_Style">The style.</param>
    private static void DrawGradient3DBorder(Graphics g,
      Rectangle p_HeaderRectangle,
      Color p_BackColor,
      Color p_DarkColor,
      Color p_LightColor,
      int p_DarkGradientNumber,
      int p_LightGradientNumber,
      Gradient3DBorderStyle p_Style)
    {
      Color l_TopLeft, l_BottomRight;
      int l_TopLeftWidth, l_BottomRightWidth;
      if (p_Style == Gradient3DBorderStyle.Raised)
      {
        l_TopLeft = p_LightColor;
        l_TopLeftWidth = p_LightGradientNumber;
        l_BottomRight = p_DarkColor;
        l_BottomRightWidth = p_DarkGradientNumber;
      }
      else
      {
        l_TopLeft = p_DarkColor;
        l_TopLeftWidth = p_DarkGradientNumber;
        l_BottomRight = p_LightColor;
        l_BottomRightWidth = p_LightGradientNumber;
      }

      // TopLeftBorder
      Color[] l_TopLeftGradient = CalculateColorGradient(p_BackColor, l_TopLeft, l_TopLeftWidth);
      using (Pen l_Pen = new Pen(l_TopLeftGradient[0]))
      {
        for (int i = 0; i < l_TopLeftGradient.Length; i++)
        {
          l_Pen.Color = l_TopLeftGradient[l_TopLeftGradient.Length - (i + 1)];
          // top
          g.DrawLine(l_Pen, p_HeaderRectangle.Left + i, p_HeaderRectangle.Top + i, p_HeaderRectangle.Right - (i + 1), p_HeaderRectangle.Top + i);
          // Left
          g.DrawLine(l_Pen, p_HeaderRectangle.Left + i, p_HeaderRectangle.Top + i, p_HeaderRectangle.Left + i, p_HeaderRectangle.Bottom - (i + 1));
        }
      }

      // BottomRightBorder
      Color[] l_BottomRightGradient = CalculateColorGradient(p_BackColor, l_BottomRight, l_BottomRightWidth);
      using (Pen l_Pen = new Pen(l_BottomRightGradient[0]))
      {
        for (int i = 0; i < l_BottomRightGradient.Length; i++)
        {
          l_Pen.Color = l_BottomRightGradient[l_BottomRightGradient.Length - (i + 1)];
          // bottom
          g.DrawLine(l_Pen, p_HeaderRectangle.Left + i, p_HeaderRectangle.Bottom - (i + 1), p_HeaderRectangle.Right - (i + 1), p_HeaderRectangle.Bottom - (i + 1));
          // right
          g.DrawLine(l_Pen, p_HeaderRectangle.Right - (i + 1), p_HeaderRectangle.Top + i, p_HeaderRectangle.Right - (i + 1), p_HeaderRectangle.Bottom - (i + 1));
        }
      }
    }

    /// <summary>
    /// Interpolate the specified number of times between start and end color.
    /// Must pass at least two gradients.
    /// </summary>
    /// <param name="p_StartColor">Start color.</param>
    /// <param name="p_EndColor">End color.</param>
    /// <param name="p_NumberOfGradients">The number of gradients.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thown if invalid number of gradients passed.</exception>
    private static Color[] CalculateColorGradient(Color p_StartColor, Color p_EndColor, int p_NumberOfGradients)
    {
      if (p_NumberOfGradients < 2)
      {
        throw new ArgumentException("Invalid Number of gradients, must be 2 or more");
      }

      Color[] l_Colors = new Color[p_NumberOfGradients];
      l_Colors[0] = p_StartColor;
      l_Colors[l_Colors.Length - 1] = p_EndColor;

      float l_IncrementR = ((float)(p_EndColor.R - p_StartColor.R)) / (float)p_NumberOfGradients;
      float l_IncrementG = ((float)(p_EndColor.G - p_StartColor.G)) / (float)p_NumberOfGradients;
      float l_IncrementB = ((float)(p_EndColor.B - p_StartColor.B)) / (float)p_NumberOfGradients;

      for (int i = 1; i < (l_Colors.Length - 1); i++)
      {
        l_Colors[i] = Color.FromArgb((int)(p_StartColor.R + l_IncrementR * (float)i),
          (int)(p_StartColor.G + l_IncrementG * (float)i),
          (int)(p_StartColor.B + l_IncrementB * (float)i));
      }

      return l_Colors;
    }

    [ComVisible(false)]
    private enum Gradient3DBorderStyle
    {
      Raised = 1,
      Sunken = 2
    }
    #endregion
  }
}