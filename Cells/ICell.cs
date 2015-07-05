#region MIT License
//
// Filename: ICell.cs
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

using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.Cells
{
  /// <summary>
  /// Represents a Cell to use with Grid control.
  /// </summary>
  [ComVisible(false)]
  public interface ICell : ICellVirtual
  {
    #region Value, DisplayText, ToolTipText
    /// <summary>
    /// Gets the display text.
    /// </summary>
    /// <value>The display text.</value>
    /// <remarks>
    /// This is te string representation of the Cell.Value property (default Value.ToString())
    /// </remarks>
    string DisplayText
    {
      get;
    }

    /// <summary>
    /// Gets or sets the cell value.
    /// </summary>
    /// <value>The value.</value>
    object Value
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the tooltip text.
    /// </summary>
    /// <value>The tool tip text.</value>
    string ToolTipText
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is last expanded cell.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is last expanded cell; otherwise, <c>false</c>.
    /// </value>
    bool IsLastExpandedCell
    {
      get;
      set;
    }
    #endregion

    #region LinkToGrid
    /// <summary>
    /// Link the cell to the specified grid.
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <param name="position">The position.</param>
    /// <remarks>
    /// To insert a cell in a grid use Grid.InsertCell, this methos is for internal use only.
    /// </remarks>
    void BindToGrid(Grid grid, Position position);
    #endregion

    #region Range and Position
    /// <summary>
    /// Gets the current range.
    /// </summary>
    /// <remarks>
    /// Returns the current Row and Column position. If this cell is not attached to the grid returns Position.Empty. And the range occupied by the current cell.
    /// Returns the Range of the cells occupied by the current cell. If RowSpan and ColSpan = 1 then this method returns a single cell.
    /// </remarks>
    /// <value>The range.</value>
    Range Range
    {
      get;
    }

    /// <summary>
    /// Gets the index of the current row.
    /// </summary>
    /// <value>The row.</value>
    int Row
    {
      get;
    }

    /// <summary>
    /// Gets the index of the current column.
    /// </summary>
    /// <value>The column.</value>
    int Column
    {
      get;
    }
    #endregion

    #region Row/Col Span
    /// <summary>
    /// Gets or sets the column span for merge operation, calculated using the current range.
    /// </summary>
    /// <value>The column span.</value>
    int ColumnSpan
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the row span.
    /// </summary>
    /// <value>The row span.</value>
    /// <remarks>
    /// This is the RowSpan for merge operations, calculated using the current range.
    /// </remarks>
    int RowSpan
    {
      get;
      set;
    }

    /// <summary>
    /// Returns true if the position specified is inside the current cell range (use Range.Contains).
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    ///   <c>true</c> if the cell contains the specified position; otherwise, <c>false</c>.
    /// </returns>
    bool ContainsPosition(Position position);
    #endregion

    #region Selection
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ICell"/> is selected.
    /// </summary>
    /// <value><c>true</c> if select; otherwise, <c>false</c>.</value>
    bool Select
    {
      get;
      set;
    }
    #endregion

    #region Focus
    /// <summary>
    /// Gets a value indicating whether the has the focus
    /// </summary>
    /// <value><c>true</c> if focused; otherwise, <c>false</c>.</value>
    bool Focused
    {
      get;
    }

    /// <summary>
    /// Give the focus at the cell
    /// </summary>
    /// <returns>
    /// Returns if the cell can receive the focus
    /// </returns>
    bool Focus();

    /// <summary>
    /// Remove the focus from the cell
    /// </summary>
    /// <returns>
    /// Returns true if the cell can leave the focus otherwise false
    /// </returns>
    bool LeaveFocus();
    #endregion

    #region Editing
    /// <summary>
    /// True if this cell is currently in edit state; otherwise <c>false</c>.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is editing; otherwise, <c>false</c>.
    /// </returns>
    bool IsEditing();
    #endregion

    #region Invalidate
    /// <summary>
    /// Invalidate this cell
    /// </summary>
    void Invalidate();
    #endregion
  }
}