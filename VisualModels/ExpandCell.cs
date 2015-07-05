#region MIT License
//
// Filename: ExpandCell.cs
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
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// Description résumée de ExpandCell.
  /// </summary>
  public class ExpandCell : Common
  {
    /// <summary>
    /// This is the default constructor with a plus box in the middle of the cell. You must use this VisualModel with a Cell of type ICellCheckBox.
    /// </summary>
    public new static readonly ExpandCell Default = new ExpandCell(false);

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCell"/> class.
    /// Use default settings.
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> read only.</param>
    public ExpandCell(bool isReadOnly)
    {
      this.isReadOnly = isReadOnly;
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCell"/> class.
    /// </summary>
    /// <remarks>
    /// Construct by copy.
    /// </remarks>
    /// <param name="source">The source.</param>
    /// <param name="isReadOnly">if set to <c>true</c> read only.</param>
    public ExpandCell(ExpandCell source, bool isReadOnly)
      : base(source, isReadOnly)
    {
      ExpandedCell = source.ExpandedCell;
    }

    #region Draw Cell
    /// <summary>
    /// Gets the image depending on the current state of the cell.
    /// </summary>
    /// <param name="p_State">The state.</param>
    /// <returns></returns>
    public static System.Drawing.Image GetImageForState(bool p_State)
    {
      return (p_State == true) ? IconUtility.Plus : IconUtility.Minus;
    }

    /// <summary>
    /// Draw the image and the display string of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(Cells.ICellVirtual p_Cell, Position p_CellPosition, PaintEventArgs e, Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      if (p_ClientRectangle.Width == 0 || p_ClientRectangle.Height == 0)
      {
        return;
      }

      RectangleBorder l_Border = Border;
      Color l_ForeColor = ForeColor;

      IExpandCell l_Cell = (IExpandCell)p_Cell;
      bool l_Status = l_Cell.GetStateValue(p_CellPosition);

      Image l_StateImage = GetImageForState(l_Status);

      Font l_CurrentFont = GetCellFont();

      // Image and Text
      VisualModelBase.PaintImageAndText(e.Graphics,
        p_ClientRectangle,
        l_StateImage,
        ContentAlignment.MiddleCenter,
        false,      // unused
        null,      // unused
        StringFormat,
        false,      // unused
        l_Border,
        l_ForeColor,
        l_CurrentFont,
        false,
        false);
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
      return new ExpandCell(this, isReadOnly);
    }
    #endregion
  }
}