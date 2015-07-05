#region MIT License
//
// Filename: MultiImages.cs
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

using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// MultiImages visual model.
  /// </summary>
  [ComVisible(false)]
  public class MultiImages : Common
  {
    private List<PositionedImage> positionedImages = new List<PositionedImage>();

    #region Constructors
    /// <summary>
    /// Use default setting and construct a read and write VisualProperties
    /// </summary>
    public MultiImages()
      : this(false)
    {
      this.ExpandedCell = false;
    }

    /// <summary>
    /// Use default setting
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> this cell is read only.</param>
    public MultiImages(bool isReadOnly)
      : base(isReadOnly)
    {
      this.ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiImages"/> class.
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="isReadOnly">if true the property is read only</param>
    /// <remarks>
    /// Copy Constructor.
    /// This method duplicates all reference fields (Image, Font, StringFormat) to create a new instance.
    /// </remarks>
    public MultiImages(MultiImages source, bool isReadOnly)
      : base(source, isReadOnly)
    {
      this.positionedImages = new List<PositionedImage>(source.positionedImages);
      this.ExpandedCell = false;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets Images of the cells
    /// </summary>
    /// <value>The subimages.</value>
    public IList<PositionedImage> SubImages
    {
      get { return positionedImages; }
    }
    #endregion

    #region DrawCell
    /// <summary>
    /// Draw the image and the displaystring of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(Cells.ICellVirtual p_Cell, Position p_CellPosition, System.Windows.Forms.PaintEventArgs e, System.Drawing.Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      base.DrawCell_ImageAndText(p_Cell, p_CellPosition, e, p_ClientRectangle, p_Status);

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

      for (int i = 0; i < positionedImages.Count; i++)
      {
        if (positionedImages[i] != null)
        {
          VisualModelBase.PaintImageAndText(e.Graphics,
            p_ClientRectangle,
            positionedImages[i].Image,
            positionedImages[i].Alignment,
            false,
            null, // not used
            null, // not used
            false, // not used
            l_Border,
            l_ForeColor,
            null,//not used
            false,
            false);
        }
      }
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
      return new MultiImages(this, isReadOnly);
    }
    #endregion
  }
}