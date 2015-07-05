#region MIT License
//
// Filename: BitmapCell.cs
//
// Copyright © 2011-2013 Felix Concordia SARL. All rights reserved.
// Felix Concordia SARL, 400 avenue Roumanille, Bat 7 - BP 309, 06906 Sophia-Antipolis Cedex, FRANCE.
// 
// Copyright © 2005-2011 MEDIT S.A. All rights reserved.
// MEDIT S.A., 2 rue du Belvedere, 91120 Palaiseau, FRANCE.
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

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// Cell containing a <see cref="Bitmap"/>.
  /// </summary>
  [ComVisible(false)]
  public class BitmapCell : Common
  {
    #region Constants
    private static readonly BitmapCell defaultBitmapCell = new BitmapCell(true);
    private static readonly BitmapCell middleLeftAlign;

    /// <summary>
    /// Gets a BitmapCell with the BitmapCell image align set to the Middle Right of the cell
    /// </summary>
    /// <value>The middle left align.</value>
    public static BitmapCell MiddleLeftAlign
    {
      get { return BitmapCell.middleLeftAlign; }
    }

    /// <summary>
    /// Gets the default cell.
    /// </summary>
    /// <remarks>
    /// Represents a default BitmapCell with the BitmapCell image aligned to the
    /// Middle Centre of the cell. You must use this VisualModel with a Cell of
    /// type ICellBitmapCell.
    /// </remarks>
    /// <value>The default cell.</value>
    public static new BitmapCell Default
    {
      get { return BitmapCell.defaultBitmapCell; }
    }
    #endregion

    #region Class members
    private ContentAlignment imageAlignment = ContentAlignment.MiddleCenter;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes the <see cref="BitmapCell"/> class.
    /// </summary>
    /// <remarks>
    /// Static constructor for the <see cref="BitmapCell"/> class.
    /// </remarks>
    static BitmapCell()
    {
      middleLeftAlign = new BitmapCell(false);
      middleLeftAlign.BitmapCellAlignment = ContentAlignment.MiddleLeft;
      middleLeftAlign.AlignTextToImage = true;
      middleLeftAlign.TextAlignment = ContentAlignment.MiddleLeft;
      middleLeftAlign.MakeReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapCell"/> class.
    /// </summary>
    /// <remarks>
    /// Use default setting and construct a read and write VisualProperties
    /// </remarks>
    public BitmapCell()
      : this(false)
    {
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapCell"/> class.
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> this cell is read only.</param>
    /// <remarks>
    /// Use default settings.
    /// </remarks>
    public BitmapCell(bool isReadOnly)
    {
      this.isReadOnly = isReadOnly;
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapCell"/> class.
    /// </summary>
    /// <param name="sourceCell">The source cell.</param>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    /// <remarks>
    /// Copy Constructor. This method duplicates all reference fields to create a new instance.
    /// </remarks>
    public BitmapCell(BitmapCell sourceCell, bool isReadOnly)
      : base(sourceCell, isReadOnly)
    {
      this.imageAlignment = sourceCell.imageAlignment;
      ExpandedCell = false;
    }
    #endregion

    /// <summary>
    /// Gets or sets Image Alignment
    /// </summary>
    /// <value>The bitmap cell alignment.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public ContentAlignment BitmapCellAlignment
    {
      get
      {
        return this.imageAlignment;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.imageAlignment = value;

        OnChange();
      }
    }

    #region DrawCell
    /// <summary>
    /// Draw the image and the displaystring of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell,
    /// relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(
      ICellVirtual p_Cell, Position p_CellPosition, System.Windows.Forms.PaintEventArgs e,
      System.Drawing.Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      if (p_ClientRectangle.Width == 0 || p_ClientRectangle.Height == 0)
      {
        return;
      }

      RectangleBorder l_Border = Border;
      Color l_ForeColor = ForeColor;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_Border = FocusBorder;
        l_ForeColor = FocusForeColor;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_Border = SelectionBorder;
        l_ForeColor = SelectionForeColor;
      }

      ICellBitmapCell l_BitmapCell = (ICellBitmapCell)p_Cell;
      Font l_CurrentFont = GetCellFont();

      Image l_Image = (Image)l_BitmapCell.GetBitmap(p_CellPosition);

      bool l_lastExpandedCell = p_Cell is ICell && ((ICell)p_Cell).IsLastExpandedCell;

      // Image and Text
      VisualModelBase.PaintImageAndText(e.Graphics,
        p_ClientRectangle,
        l_Image,
        imageAlignment,
        ImageStretch,
        string.Empty, // p_Cell.GetDisplayText(p_CellPosition),
        StringFormat,
        AlignTextToImage,
        l_Border,
        l_ForeColor,
        l_CurrentFont,
        false,
        l_lastExpandedCell);
    }
    #endregion

    #region Clone
    /// <summary>
    /// Clone this object. This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </summary>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    public override object Clone(bool isReadOnly)
    {
      return new BitmapCell(this, isReadOnly);
    }
    #endregion
  }
}