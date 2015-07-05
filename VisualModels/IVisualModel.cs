#region MIT License
//
// Filename: IVisualModel.cs
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

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// An interface that represents the visual aspect of a cell. Contains the Draw method and the common properties
  /// </summary>
  [ComVisible(false)]
  public interface IVisualModel : ICloneable
  {
    #region Format
    /// <summary>
    /// Gets or sets the font.
    /// </summary>
    /// <remarks>
    /// If null, the default font is used
    /// </remarks>
    /// <value>The font.</value>
    Font Font
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the color of the cell background.
    /// </summary>
    /// <value>The color of the background.</value>
    Color BackColor
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the color of the cell foreground.
    /// </summary>
    /// <value>The color of the foreground.</value>
    Color ForeColor
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the normal border of a cell
    /// </summary>
    /// <value>The border.</value>
    RectangleBorder Border
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to apply word wrapping.
    /// This property is only a wrapper around StringFormat
    /// </summary>
    /// <value><c>true</c> if word wrap is enabled; otherwise, <c>false</c>.</value>
    bool WordWrap
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the text alignment.
    /// This property is only a wrapper around StringFormat
    /// </summary>
    /// <value>The text alignment.</value>
    ContentAlignment TextAlignment
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this is an expanded cell.
    /// An expanded cell has lines of expanded nodes as in a treeview.
    /// </summary>
    /// <value><c>true</c> if cell is an expanded cell; otherwise, <c>false</c>.</value>
    bool ExpandedCell
    {
      get;
      set;
    }
    #endregion

    /// <summary>
    /// Draw the cell specified
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    void DrawCell(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      System.Windows.Forms.PaintEventArgs e,
      System.Drawing.Rectangle p_ClientRectangle);

    /// <summary>
    /// Returns the minimum required size of the current cell, calculating using the
    /// current DisplayString, Image and Borders information.
    /// </summary>
    /// <param name="p_Graphics">GDI+ drawing surface</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <returns></returns>
    SizeF GetRequiredSize(Graphics p_Graphics,
      Cells.ICellVirtual p_Cell,
      Position p_CellPosition);

    /// <summary>
    /// Clone this object. This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </summary>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    object Clone(bool isReadOnly);

    /// <summary>
    /// Gets a value indicating whether this class is ReadOnly otherwise False.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    bool ReadOnly
    {
      get;
    }

    /// <summary>
    /// Make the current instance readonly. Use this method to prevent unexpected changes.
    /// </summary>
    void MakeReadOnly();

    /// <summary>
    /// Make the current instance not readonly.
    /// </summary>
    void MakeNonReadOnly();
  }
}