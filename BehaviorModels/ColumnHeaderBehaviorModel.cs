#region MIT License
//
// Filename: ColumnHeaderBehaviorModel.cs
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

using System.Collections;
using System.Runtime.InteropServices;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// A behaviour that supports sort and resize.
  /// </summary>
  /// <remarks>
  /// Once created, <see cref="ColumnHeaderBehaviorModel"/> objects can not be modified.
  /// When calculated automatically the range to sort is all the grid range without
  /// the rows minor of the current row and the range header is all the grid range
  /// with the rows minor or equal of the current row.
  /// </remarks>
  [ComVisible(false)]
  public class ColumnHeaderBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Sortable column header behaviour
    /// </summary>
    public static readonly ColumnHeaderBehaviorModel SortableHeader = new ColumnHeaderBehaviorModel(true);

    /// <summary>
    /// Sortable column header behaviour
    /// </summary>
    public static readonly ColumnHeaderBehaviorModel NotSortableHeader = new ColumnHeaderBehaviorModel(false);

    private ResizeBehaviorModel resize;

    /// <summary>
    /// Range to sort
    /// </summary>
    private IRangeLoader rangeToSort;

    /// <summary>
    /// Header range (can be null).
    /// </summary>
    private IRangeLoader headerRange;

    /// <summary>
    /// Whether sorting is enabled
    /// </summary>
    private bool doEnableSort = true;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeaderBehaviorModel"/> class.
    /// </summary>
    /// <param name="enableSort">if set to <c>true</c> enable sort.</param>
    public ColumnHeaderBehaviorModel(bool enableSort)
      : this(enableSort, null, null, ResizeBehaviorModel.ResizeWidth, ButtonBehaviorModel.Default, UnselectableBehaviorModel.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeaderBehaviorModel"/> class.
    /// </summary>
    /// <param name="p_EnableSort">True to enable sort; otherwise <c>false</c>.</param>
    /// <param name="p_RangeToSort">If null and p_EnableSort is true then the range is automatically calculated.</param>
    /// <param name="p_HeaderRange">If null and p_EnableSort is true then the range is automatically calculated.</param>
    /// <param name="p_BehaviorResize">The resize behaviour.</param>
    /// <param name="p_BehaviorButton">The button behaviour.</param>
    /// <param name="p_BehaviorUnselectable">The behavior unselectable.</param>
    public ColumnHeaderBehaviorModel(
      bool p_EnableSort, IRangeLoader p_RangeToSort, IRangeLoader p_HeaderRange,
      ResizeBehaviorModel p_BehaviorResize, ButtonBehaviorModel p_BehaviorButton, UnselectableBehaviorModel p_BehaviorUnselectable)
    {
      doEnableSort = p_EnableSort;

      if (p_EnableSort == true)
      {
        headerRange = p_HeaderRange;
        rangeToSort = p_RangeToSort;
      }
      else
      {
        headerRange = null;
        rangeToSort = null;
      }

      resize = p_BehaviorResize;

      SubModels.Add(resize);
      SubModels.Add(p_BehaviorButton);
      SubModels.Add(p_BehaviorUnselectable);
    }

    /// <summary>
    /// Fires the focus entering in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public override void OnFocusEntering(PositionCancelEventArgs e)
    {
      // check whether the user is in a resize region
      if (resize != null &&
        resize.IsHeightResizing == false &&
        resize.IsWidthResizing == false &&
        IsSortEnable(e) == false &&
        e.Grid.Selection.SelectionMode != GridSelectionMode.Row)
      {
        // if the sort is disabled use the header as a column selector
        e.Grid.Columns[e.Position.Column].Focus();
        e.Grid.Columns[e.Position.Column].Select = true;
      }

      base.OnFocusEntering(e);
    }
    #endregion

    #region IBehaviorModel Members
    /// <summary>
    /// Handles the click event on the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);

      if (IsSortEnable(e) &&
        (resize == null || (resize.IsHeightResizing == false && resize.IsWidthResizing == false)))
      {
        ICellSortableHeader sortableHeaderCell = (ICellSortableHeader)e.Cell;
        SortStatus sortStatus = sortableHeaderCell.GetSortStatus(e.Position);
        if (sortStatus.EnableSort)
        {
          SortColumn(e, sortStatus.Mode != GridSortMode.Ascending, sortStatus.Comparer);
        }
      }
    }

    /// <summary>
    /// Handles the mouse enter event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnMouseEnter(PositionEventArgs e)
    {
      base.OnMouseEnter(e);

      e.Cell.Invalidate(e.Position); // Invalidate the cell to refresh the header
    }

    /// <summary>
    /// Handles the mouse leave event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnMouseLeave(PositionEventArgs e)
    {
      base.OnMouseLeave(e);

      e.Cell.Invalidate(e.Position); // Invalidate the cell to refresh the header
    }
    #endregion

    #region Sort Methods
    #region Status Properties
    /// <summary>
    /// Gets range to sort. If null and EnableSort is true the range is automatically calculated.
    /// </summary>
    /// <value>The range to sort.</value>
    public IRangeLoader RangeToSort
    {
      get { return rangeToSort; }
    }

    /// <summary>
    /// Gets header range. If null and EnableSort is true the range is automatically calculated.
    /// </summary>
    /// <value>The range header.</value>
    public IRangeLoader RangeHeader
    {
      get { return headerRange; }
    }

    /// <summary>
    /// Gets a value indicating whether sorting is enabled for column.
    /// True to enable sort otherwise false. Default is true.
    /// </summary>
    /// <value><c>true</c> if sorting is enabled; otherwise, <c>false</c>.</value>
    public bool EnableSort
    {
      get { return doEnableSort; }
    }
    #endregion

    #region Support Function
    /// <summary>
    /// Indicates whether sorting is enabled for the specified cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    /// <returns>
    ///   <c>true</c> if is sort enabled for the specified cell; otherwise, <c>false</c>.
    /// </returns>
    private bool IsSortEnable(PositionEventArgs e)
    {
      if (e.Cell is ICellSortableHeader && doEnableSort)
      {
        ICellSortableHeader sortableHeaderCell = (ICellSortableHeader)e.Cell;
        SortStatus sortStatus = sortableHeaderCell.GetSortStatus(e.Position);
        return sortStatus.EnableSort;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Sort the current column
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    /// <param name="doAscending">if set to <c>true</c> sort in ascending order.</param>
    /// <param name="comparer">The comparer.</param>
    private void SortColumn(PositionEventArgs e, bool doAscending, IComparer comparer)
    {
      if (IsSortEnable(e) && e.Position.Row < e.Grid.RowsCount && e.Grid.ColumnsCount > 0)
      {
        Range l_RangeToSort;
        Range l_RangeHeader;
        if (rangeToSort != null)
        {
          l_RangeToSort = rangeToSort.GetRange(e.Grid);
        }
        else
        {
          // the range to sort is all the grid range without the rows < of the current row
          l_RangeToSort = new Range(e.Position.Row + 1, 0, e.Grid.RowsCount - 1, e.Grid.ColumnsCount - 1);
        }

        if (headerRange != null)
        {
          l_RangeHeader = headerRange.GetRange(e.Grid);
        }
        else
        {
          // the range header is all the grid range with the rows <= of the current row
          l_RangeHeader = new Range(0, 0, e.Position.Row, e.Grid.ColumnsCount - 1);
        }

        if (e.Grid.RowsCount > e.Grid.FixedRows && e.Grid.ColumnsCount > e.Grid.FixedColumns)
        {
          e.Grid.SortRangeRows(l_RangeToSort, e.Position.Column, doAscending, comparer);
          ICellSortableHeader l_CellSortable = (ICellSortableHeader)e.Cell;
          l_CellSortable.SetSortMode(e.Position, doAscending == true ? GridSortMode.Ascending : GridSortMode.Descending);

          // Remove the image from others
          for (int r = l_RangeHeader.Start.Row; r <= l_RangeHeader.End.Row; r++)
          {
            for (int c = l_RangeHeader.Start.Column; c <= l_RangeHeader.End.Column; c++)
            {
              Cells.ICellVirtual l_tmp = e.Grid.GetCell(r, c);
              if (l_tmp != null && l_tmp is ICellSortableHeader && c != e.Position.Column)
              {
                ((ICellSortableHeader)l_tmp).SetSortMode(new Position(r, c), GridSortMode.None);
              }
            }
          }
        }
      }
    }
    #endregion
    #endregion
  }
}