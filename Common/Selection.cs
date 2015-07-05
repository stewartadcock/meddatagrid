#region MIT License
//
// Filename: Selection.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Fr.Fc.FcCore.Logging;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A class to represent the selected cells of a grid.
  /// </summary>
  [ComVisible(false)]
  public class Selection : ICollection, IDisposable
  {
    #region Class variables
    private List<Range> rangeList = new List<Range>();  // SAA TODO: List<Range>.Contains is often a bottle neck. Replace by HashSet?
    private List<Position> selectedPositionCache = null;
    private List<ICellVirtual> selectedCellCache = null;
    private GridSelectionMode selectionMode = GridSelectionMode.Cell;
    private bool doEnableMultiSelection = true;
    private bool doAutoClear = true;
    private List<MenuItem> contextMenuItems = null;

    /// <summary>
    /// The grid to which the selection applies.
    /// </summary>
    private GridVirtual grid;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Selection"/> class.
    /// </summary>
    /// <param name="grid">The grid to which the selection applies.</param>
    public Selection(GridVirtual grid)
    {
      this.grid = grid;

      imageCut = menuImageList.Images.Count;
      menuImageList.Images.Add(IconUtility.Cut);

      imageCopy = menuImageList.Images.Count;
      menuImageList.Images.Add(IconUtility.Copy);

      imagePaste = menuImageList.Images.Count;
      menuImageList.Images.Add(IconUtility.Paste);

      imageClear = menuImageList.Images.Count;
      menuImageList.Images.Add(IconUtility.Clear);

      imageFormatCells = menuImageList.Images.Count;
      menuImageList.Images.Add(IconUtility.Properties);
    }
    #endregion

    /// <summary>
    /// Returns the union of all the selected range as Position collection.
    /// </summary>
    /// <returns>The union of all the selected range as Position collection.</returns>
    public IList<Position> GetCellsPositions()
    {
      if (this.selectedPositionCache == null)
      {
        this.selectedPositionCache = new List<Position>();
        for (int i = 0; i < rangeList.Count; i++)
        {
          IList<Position> l_Positions = rangeList[i].GetCellsPositions();
          for (int j = 0; j < l_Positions.Count; j++)
          {
            if (this.selectedPositionCache.Contains(l_Positions[j]) == false)
            {
              this.selectedPositionCache.Add(l_Positions[j]);
            }
          }
        }
      }

      return (IList<Position>)this.selectedPositionCache;
    }

    /// <summary>
    /// Returns the union of all the selected range as Position collection.
    /// </summary>
    /// <returns></returns>
    public IList<ICellVirtual> GetCells()
    {
      if (this.selectedCellCache == null)
      {
        this.selectedCellCache = new List<ICellVirtual>();
        IList<Position> l_Positions = GetCellsPositions();
        for (int i = 0; i < l_Positions.Count; i++)
        {
          this.selectedCellCache.Add(this.grid.GetCell(l_Positions[i]));
        }
      }

      return (IList<ICellVirtual>)this.selectedCellCache;
    }

    /// <summary>
    /// Gets the range of cells at the specific position.
    /// </summary>
    /// <param name="index">Position.</param>
    /// <value>Range</value>
    /// <returns>
    /// The range of cells at the specific position.
    /// </returns>
    public Range this[int index]
    {
      get { return this.rangeList[index]; }
    }

    /// <summary>
    /// Indicates whether the specified cell is selected.
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <returns>
    /// <c>true</c> if the selection contains the specified cell; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(Position cell)
    {
      if (cell.IsEmpty())
      {
        return false;
      }

      for (int r = 0; r < rangeList.Count; r++)
      {
        if (this[r].Contains(cell))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Indicates whether the specified range of cells is selected.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <returns>
    /// <c>true</c> if the selection contains the specified range; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(Range range)
    {
      if (Count <= 0)
      {
        return false;
      }

      // prima cerco se è presente un range esattamente come quello richiesto
      if (rangeList.Contains(range))
      {
        return true;
      }

      // se non ho trovato uguale provo a cercare cella per cella
      IList<Position> l_SearchList = range.GetCellsPositions();
      for (int i = 0; i < l_SearchList.Count; i++)
      {
        bool isFound = false;
        for (int r = 0; r < Count; r++)
        {
          if (this[r].Contains(l_SearchList[i]))
          {
            isFound = true;
            break;
          }
        }
        if (isFound == false)
        {
          return false;
        }
      }

      return true;
    }

    /// <summary>
    /// Indicates whether the specified row is selected.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <returns>
    /// <c>true</c> if the selection contains the specified row; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsRow(int row)
    {
      for (int r = 0; r < rangeList.Count; r++)
      {
        if (this[r].ContainsRow(row))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Indicates whether the specified column is selected.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <returns>
    /// <c>true</c> if the selection contains the specified column; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsColumn(int column)
    {
      for (int r = 0; r < rangeList.Count; r++)
      {
        if (this[r].ContainsColumn(column))
        {
          return true;
        }
      }

      return false;
    }

    #region Add/Remove/Clear
    private void ClearCache()
    {
      selectedCellCache = null;
      selectedPositionCache = null;
    }

    /// <summary>
    /// Deselect all cells except that specified.
    /// </summary>
    /// <param name="p_CellLeaveThisCellSelected">The cell to leave selected.</param>
    public void Clear(Position p_CellLeaveThisCellSelected)
    {
      if (Count > 0)
      {
        rangeList.Clear();

        rangeList.Add(new Range(p_CellLeaveThisCellSelected));

        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Clear, Range.Empty));
      }
    }

    /// <summary>
    /// Deselect all the cells
    /// </summary>
    public void Clear()
    {
      if (Count > 0)
      {
        rangeList.Clear();

        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Clear, Range.Empty));
      }
    }

    /// <summary>
    /// Select the specified cell and add the cell to the collection.
    /// </summary>
    /// <param name="cell">The cell.</param>
    public void Add(Position cell)
    {
      AddRange(new Range(cell));
    }

    /// <summary>
    /// Select the specified Range of cells
    /// </summary>
    /// <param name="p_Range">The range.</param>
    public void AddRange(Range p_Range)
    {
      if (p_Range.IsEmpty() == true || grid.ColumnsCount == 0 || grid.RowsCount == 0)
      {
        return;
      }

      Range l_RangeToSelect = p_Range;

      // Apply SelectionMode
      if (selectionMode == GridSelectionMode.Row)
      {
        AddRangeRow(l_RangeToSelect);
      }
      else if (selectionMode == GridSelectionMode.Column)
      {
        AddRangeColumn(l_RangeToSelect);
      }
      else // if (selectionMode == GridSelectionMode.Cell)
      {
        l_RangeToSelect = AddRangeCell(l_RangeToSelect);
      }
    }

    private void AddRangeRow(Range l_RangeToSelect)
    {
      if (this.grid.SelectRowHeader && grid.FixedColumns > 0 && grid.Columns[0].Width > 0)
      {
        l_RangeToSelect = new Range(l_RangeToSelect.Start.Row, this.grid.FixedColumns,
          l_RangeToSelect.End.Row, this.grid.ColumnsCount - 1);
      }
      else
      {
        // Special case a lot faster
        l_RangeToSelect = new Range(l_RangeToSelect.Start.Row, 0, l_RangeToSelect.End.Row, grid.ColumnsCount - 1);
        rangeList.Add(l_RangeToSelect);
        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Add, l_RangeToSelect));
        return;
      }

      List<Range> l_rangesToAdd = new List<Range>();
      if (rangeList.Count > 0)
      {
        bool isAdded = false;
        foreach (Range l_range in rangeList)
        {
          // SL<SP && SP<EL && EL<EP
          if (l_range.Start.Row < l_RangeToSelect.Start.Row && l_RangeToSelect.Start.Row < l_range.End.Row && l_range.End.Row < l_RangeToSelect.End.Row)
          {
            l_rangesToAdd.Add(new Range(l_range.Start.Row, l_RangeToSelect.Start.Column, l_RangeToSelect.End.Row, grid.ColumnsCount - 1));
          }
          // SP<SL && SL<EP && EP<EL
          else if (l_RangeToSelect.Start.Row < l_range.Start.Row && l_range.Start.Row < l_RangeToSelect.End.Row && l_RangeToSelect.End.Row < l_range.End.Row)
          {
            l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, l_range.End.Row, grid.ColumnsCount - 1));
          }
          else if (l_range.Start.Row == l_RangeToSelect.Start.Row)
          {
            if (l_range.End.Row < l_RangeToSelect.End.Row)
            {
              l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, l_RangeToSelect.End.Row, grid.ColumnsCount - 1));
            }
            else if (l_range.End.Row != l_RangeToSelect.End.Row)
            {
              continue;
            }
            else if (!isAdded)
            {
              if (Contains(l_RangeToSelect) == false)
              {
                l_rangesToAdd.Add(l_RangeToSelect);
                isAdded = true;
              }
            }
          }
          else if (l_range.End.Row == l_RangeToSelect.End.Row)
          {
            if (l_RangeToSelect.Start.Row < l_range.Start.Row)
            {
              l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, l_RangeToSelect.End.Row, grid.ColumnsCount - 1));
            }
            else if (l_range.Start.Row != l_RangeToSelect.Start.Row)
            {
              continue;
            }
            else if (!isAdded)
            {
              if (Contains(l_RangeToSelect) == false)
              {
                l_rangesToAdd.Add(l_RangeToSelect);
                isAdded = true;
              }
            }
          }
          else if (l_RangeToSelect.Start.Row < l_range.Start.Row && l_range.End.Row < l_RangeToSelect.End.Row)
          {
            l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, l_RangeToSelect.End.Row, grid.ColumnsCount - 1));
          }
          else
          {
            //  l_rangesToAdd.Add(l_range);
            if (!isAdded)
            {
              if (Contains(l_RangeToSelect) == false)
              {
                l_rangesToAdd.Add(l_RangeToSelect);
                isAdded = true;
              }
            }
          }
        }
        foreach (Range range in l_rangesToAdd)
        {
          rangeList.Add(range);
        }
      }
      else
      {
        rangeList.Add(l_RangeToSelect);
      }

      OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Add, l_RangeToSelect));
    }

    private void AddRangeColumn(Range l_RangeToSelect)
    {
      if (this.grid.SelectColumnHeader && this.grid.FixedRows > 0 && this.grid.Rows[0].Height > 0)
      {
        l_RangeToSelect = new Range(0, l_RangeToSelect.Start.Column, this.grid.RowsCount - 1,
          l_RangeToSelect.End.Column);
      }
      else
      {
        l_RangeToSelect = new Range(this.grid.FixedRows, l_RangeToSelect.Start.Column,
          this.grid.RowsCount - 1, l_RangeToSelect.End.Column);
      }

      List<Range> l_rangesToAdd = new List<Range>();
      if (rangeList.Count > 0)
      {
        bool isAdded = false;
        foreach (Range l_range in rangeList)
        {
          // SL<SP && SP<EL && EL<EP
          if (l_range.Start.Column < l_RangeToSelect.Start.Column
            && l_RangeToSelect.Start.Column < l_range.End.Column
            && l_range.End.Column < l_RangeToSelect.End.Column)
          {
            l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_range.Start.Column, grid.RowsCount - 1, l_RangeToSelect.End.Column));
          }
          // SP<SL && SL<EP && EP<EL
          else if (l_RangeToSelect.Start.Column < l_range.Start.Column
            && l_range.Start.Column < l_RangeToSelect.End.Column
            && l_RangeToSelect.End.Column < l_range.End.Column)
          {
            l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, grid.RowsCount - 1, l_range.End.Column));
          }
          else if (l_range.Start.Column == l_RangeToSelect.Start.Column)
          {
            if (l_range.End.Column < l_RangeToSelect.End.Column)
            {
              l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_range.Start.Column, grid.RowsCount - 1, l_RangeToSelect.End.Column));
            }
            else
            {
              continue;
            }
          }
          else if (l_range.End.Column == l_RangeToSelect.End.Column)
          {
            if (l_RangeToSelect.Start.Column < l_range.Start.Column)
            {
              l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, grid.RowsCount - 1, l_RangeToSelect.End.Column));
            }
            else
            {
              continue;
            }
          }
          else if (l_RangeToSelect.Start.Column < l_range.Start.Column && l_range.End.Column < l_RangeToSelect.End.Column)
          {
            l_rangesToAdd.Add(new Range(l_RangeToSelect.Start.Row, l_RangeToSelect.Start.Column, grid.RowsCount - 1, l_RangeToSelect.End.Column));
          }
          else
          {
            //  l_rangesToAdd.Add(l_range);
            if (!isAdded)
            {
              if (Contains(l_RangeToSelect) == false)
              {
                l_rangesToAdd.Add(l_RangeToSelect);
                isAdded = true;
              }
            }
          }
        }
        foreach (Range range in l_rangesToAdd)
        {
          rangeList.Add(range);
        }
      }
      else
      {
        rangeList.Add(l_RangeToSelect);
      }
      OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Add, l_RangeToSelect));
    }

    private Range AddRangeCell(Range l_RangeToSelect)
    {
      bool isFound = false;
      if (l_RangeToSelect.Start == l_RangeToSelect.End && !Contains(l_RangeToSelect.Start))
      {
        rangeList.Add(l_RangeToSelect);
        isFound = true;
      }
      // here we check whether the l_RangeToSelect is a subset of a row
      else if (l_RangeToSelect.Start.Row == l_RangeToSelect.End.Row)
      {
        int row = l_RangeToSelect.Start.Row;
        for (int col = l_RangeToSelect.Start.Column; col <= l_RangeToSelect.End.Column; ++col)
        {
          Position position = new Position(row, col);
          if (!Contains(position))
          {
            rangeList.Add(new Range(position));
            isFound = true;
          }
        }
      }
      // here we check whether the l_RangeToSelect is a subset of a column
      else if (l_RangeToSelect.Start.Column == l_RangeToSelect.End.Column)
      {
        int col = l_RangeToSelect.Start.Column;
        for (int row = l_RangeToSelect.Start.Row; row <= l_RangeToSelect.End.Row; ++row)
        {
          Position position = new Position(row, col);
          if (!Contains(position))
          {
            rangeList.Add(new Range(position));
            isFound = true;
          }
        }
      }
      else // This is a rectangle.
      {
        for (int row = l_RangeToSelect.Start.Row; row <= l_RangeToSelect.End.Row; ++row)
        {
          for (int col = l_RangeToSelect.Start.Column; col <= l_RangeToSelect.End.Column; ++col)
          {
            Position position = new Position(row, col);
            if (!Contains(position))
            {
              rangeList.Add(new Range(position));
              isFound = true;
            }
          }
        }
      }
      if (isFound)
      {
        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Add, l_RangeToSelect));
      }
      return l_RangeToSelect;
    }

    /// <summary>
    /// Deselect and remove from the collection the specified range of cells
    /// </summary>
    /// <param name="range">The range.</param>
    /// <remarks>
    /// if GridSelectionMode is Cell this method work only if we have 1 cell in the range
    /// </remarks>
    public void RemoveRange(Range range)
    {
      if (range.IsEmpty() == true)
      {
        return;
      }

      if (selectionMode == GridSelectionMode.Row)
      {
        RemoveRangeRow(range);
      }
      else if (selectionMode == GridSelectionMode.Column)
      {
        RemoveRangeColumn(range);
      }
      else // GridSelectionMode.Cell
      {
        RemoveRangeCell(range);
      }
    }

    private void RemoveRangeRow(Range p_Range)
    {
      Range l_RangeToDeselect = p_Range;
      if (this.grid.ColumnsCount > 0)
      {
        l_RangeToDeselect = new Range(p_Range.Start.Row, 0, p_Range.End.Row, grid.ColumnsCount - 1);
      }

      // optimisation pour les selections en ligne
      // algo : regarder si le Range correspond a une ligne entiere
      // si oui on applique l'aglo de decoupage de m_RangeList

      int startColumnGrid = 0;  // the first column to select: it is 0 if the first column is not a header cell.

      if (this.grid.RowsCount > 1 && this.grid.ColumnsCount > 0
        && this.grid.GetCell(1, 0) is Cells.Real.HeaderCell)
      {
        startColumnGrid = 1;
      }
      else if (this.grid.RowsCount == 1 && this.grid.ColumnsCount > 0
        && this.grid.GetCell(0, 0) is Cells.Real.HeaderCell)
      {
        startColumnGrid = 1;
      }

      List<Range> l_rangesToAdd = new List<Range>();
      bool selectionChanged = false;
      foreach (Range l_range in rangeList)
      {
        if (l_range.Start.Row == l_range.End.Row && l_range.Start.Row == l_RangeToDeselect.Start.Row && l_range.Start.Row == l_RangeToDeselect.End.Row)
        {
          selectionChanged = true;
        }
        // SL<SP && SP<EL && EL<EP
        else if (l_range.Start.Row < l_RangeToDeselect.Start.Row && l_RangeToDeselect.Start.Row < l_range.End.Row && l_range.End.Row < l_RangeToDeselect.End.Row)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(l_range.Start.Row, startColumnGrid, l_RangeToDeselect.Start.Row - 1, grid.ColumnsCount - 1));
        }
        // SP<SL && SL<EP && EP<EL
        else if (l_RangeToDeselect.Start.Row < l_range.Start.Row && l_range.Start.Row < l_RangeToDeselect.End.Row && l_RangeToDeselect.End.Row < l_range.End.Row)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(l_RangeToDeselect.End.Row + 1, startColumnGrid, l_range.End.Row, grid.ColumnsCount - 1));
        }
        // SL<SP && EP<EL
        else if (l_range.Start.Row < l_RangeToDeselect.Start.Row && l_RangeToDeselect.End.Row < l_range.End.Row)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(l_range.Start.Row, startColumnGrid, l_RangeToDeselect.Start.Row - 1, grid.ColumnsCount - 1));
          l_rangesToAdd.Add(new Range(l_RangeToDeselect.End.Row + 1, startColumnGrid, l_range.End.Row, grid.ColumnsCount - 1));
        }
        else if (l_range.Start.Row == l_RangeToDeselect.Start.Row)
        {
          selectionChanged = true;
          if (l_RangeToDeselect.End.Row < l_range.End.Row)
          {
            l_rangesToAdd.Add(new Range(l_RangeToDeselect.End.Row + 1, startColumnGrid, l_range.End.Row, grid.ColumnsCount - 1));
          }
        }
        else if (l_range.End.Row == l_RangeToDeselect.End.Row)
        {
          selectionChanged = true;
          if (l_range.Start.Row < l_RangeToDeselect.Start.Row)
          {
            l_rangesToAdd.Add(new Range(l_range.Start.Row, startColumnGrid, l_RangeToDeselect.Start.Row - 1, grid.ColumnsCount - 1));
          }
        }
        else if (!(l_RangeToDeselect.Start.Row < l_range.Start.Row && l_range.End.Row < l_RangeToDeselect.End.Row))
        {
          l_rangesToAdd.Add(l_range);
        }
      }
      rangeList.Clear();
      foreach (Range range in l_rangesToAdd)
      {
        rangeList.Add(range);
      }

      if (selectionChanged)
      {
        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Remove, l_RangeToDeselect));
      }
    }

    private void RemoveRangeColumn(Range p_Range)
    {
      Range l_RangeToDeselect = p_Range;
      if (this.grid.RowsCount > 0)
      {
        l_RangeToDeselect = new Range(0, p_Range.Start.Column, grid.RowsCount - 1, p_Range.End.Column);
      }
      // optimisation pour les selections en ligne
      // algo : regarder si le Range correspond a une ligne entiere
      // si oui on applique l'aglo de decoupage de m_RangeList

      int startRowGrid = 0;  // the first row to select: it is 0 if the first row is not a column header

      if (this.grid.RowsCount > 0 && this.grid.ColumnsCount > 1
        && this.grid.GetCell(0, 1) is Cells.Real.HeaderCell)
      {
        startRowGrid = 1;
      }
      else if (this.grid.RowsCount > 0 && this.grid.ColumnsCount == 1
        && this.grid.GetCell(0, 0) is Cells.Real.HeaderCell)
      {
        startRowGrid = 1;
      }

      List<Range> l_rangesToAdd = new List<Range>();
      bool selectionChanged = false;
      foreach (Range l_range in rangeList)
      {
        if (l_range.Start.Column == l_range.End.Column
          && l_range.Start.Column == l_RangeToDeselect.Start.Column
          && l_range.Start.Column == l_RangeToDeselect.End.Column)
        {
          selectionChanged = true;
        }
        else if (l_range.Start.Column < l_RangeToDeselect.Start.Column
          && l_RangeToDeselect.Start.Column < l_range.End.Column
          && l_range.End.Column < l_RangeToDeselect.End.Column)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(startRowGrid, l_range.Start.Column, grid.RowsCount - 1, l_RangeToDeselect.Start.Column - 1));
        }
        else if (l_RangeToDeselect.Start.Column < l_range.Start.Column
          && l_range.Start.Column < l_RangeToDeselect.End.Column
          && l_RangeToDeselect.End.Column < l_range.End.Column)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(startRowGrid, l_RangeToDeselect.End.Column + 1, grid.RowsCount - 1, l_range.End.Column));
        }
        else if (l_range.Start.Column < l_RangeToDeselect.Start.Column
          && l_RangeToDeselect.End.Column < l_range.End.Column)
        {
          selectionChanged = true;
          l_rangesToAdd.Add(new Range(startRowGrid, l_range.Start.Column, grid.RowsCount - 1, l_RangeToDeselect.End.Column - 1));
          l_rangesToAdd.Add(new Range(startRowGrid, l_RangeToDeselect.End.Column + 1, grid.RowsCount - 1, l_range.End.Column));
        }
        else if (l_range.Start.Column == l_RangeToDeselect.Start.Column)
        {
          selectionChanged = true;
          if (l_RangeToDeselect.End.Column < l_range.End.Column)
          {
            l_rangesToAdd.Add(new Range(startRowGrid, l_RangeToDeselect.End.Column + 1, grid.RowsCount - 1, l_range.End.Column));
          }
        }
        else if (l_range.End.Column == l_RangeToDeselect.End.Column)
        {
          selectionChanged = true;
          if (l_range.Start.Column < l_RangeToDeselect.Start.Column)
          {
            l_rangesToAdd.Add(new Range(startRowGrid, l_range.Start.Column, grid.RowsCount - 1, l_RangeToDeselect.Start.Column - 1));
          }
        }
        else if (!(l_RangeToDeselect.Start.Column < l_range.Start.Column && l_range.End.Column < l_RangeToDeselect.End.Column))
        {
          l_rangesToAdd.Add(l_range);
        }
      }
      rangeList.Clear();
      foreach (Range range in l_rangesToAdd)
      {
        rangeList.Add(range);
      }

      if (selectionChanged)
      {
        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Remove, l_RangeToDeselect));
      }
    }

    private void RemoveRangeCell(Range l_RangeToDeselect)
    {
      bool selectionChanged = false;
      if (l_RangeToDeselect.Start == l_RangeToDeselect.End && Contains(l_RangeToDeselect.Start))
      {
        rangeList.Remove(l_RangeToDeselect);
        selectionChanged = true;
      }
      // here we check whether the l_RangeToDeselect is a subset of a row
      else if (l_RangeToDeselect.Start.Row == l_RangeToDeselect.End.Row)
      {
        int row = l_RangeToDeselect.Start.Row;
        for (int col = l_RangeToDeselect.Start.Column; col <= l_RangeToDeselect.End.Column; ++col)
        {
          Position position = new Position(row, col);
          if (Contains(position))
          {
            rangeList.Remove(new Range(position));
            selectionChanged = true;
          }
        }
      }
      // here we check whether the l_RangeToDeselect is a subset of a column
      else if (l_RangeToDeselect.Start.Column == l_RangeToDeselect.End.Column)
      {
        int col = l_RangeToDeselect.Start.Column;
        for (int row = l_RangeToDeselect.Start.Row; row <= l_RangeToDeselect.End.Row; ++row)
        {
          Position position = new Position(row, col);
          if (Contains(position))
          {
            rangeList.Remove(new Range(position));
            selectionChanged = true;
          }
        }
      }

      if (selectionChanged)
      {
        OnSelectionChange(new SelectionChangedEventArgs(SelectionChangedEventType.Remove, l_RangeToDeselect));
      }
    }

    /// <summary>
    /// Invert the selection when mode selection is row
    /// </summary>
    public void InvertSelectionRows()
    {
      this.grid.Redraw = false;

      List<Range> l_RangeCollection = new List<Range>();
      bool start_found = false;
      Position l_startPos = Position.Empty;
      for (int i = 1; i < grid.RowsCount; ++i)
      {
        if (start_found == false && this.ContainsRow(i) == false)
        {
          l_startPos = new Position(i, 0);
          start_found = true;
        }

        if (start_found == true && this.ContainsRow(i) == true)
        {
          Position l_endPos = new Position(i - 1, grid.ColumnsCount - 1);
          l_RangeCollection.Add(new Range(l_startPos, l_endPos));
          start_found = false;
        }
      }

      if (start_found == true)
      {
        Position l_endPos = new Position(this.grid.RowsCount - 1, this.grid.ColumnsCount - 1);
        l_RangeCollection.Add(new Range(l_startPos, l_endPos));
      }

      Clear();
      this.grid.SetFocusCell(Position.Empty);

      foreach (Range range in l_RangeCollection)
      {
        AddRange(range);
      }

      this.grid.Redraw = true;
    }

    /// <summary>
    /// Deselect and remove from the collection the specified cell
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    public void Remove(Position p_Cell)
    {
      RemoveRange(new Range(p_Cell));
    }
    #endregion

    /// <summary>
    /// Searches for the specified Cell and returns the zero-based index of the first occurrence that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <returns></returns>
    public int IndexOf(Cells.ICellVirtual p_Cell)
    {
      return GetCells().IndexOf(p_Cell);
    }

    /// <summary>
    /// Gets a value indicating whether the grid to which the selection belongs is undergoing a sort operation.
    /// </summary>
    /// <value>
    /// <c>true</c> if the grid sorting; otherwise, <c>false</c>.
    /// </value>
    public bool IsGridSorting
    {
      get
      {
        return grid.IsSorting;
      }
    }

    #region SelectionChange event
    /// <summary>
    /// Fired when a cell is added from the selection or removed from the selection
    /// </summary>
    public event SelectionChangedEventHandler SelectionChanged;

    /// <summary>
    /// Fired when a cell is added from the selection or removed from the selection
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.SelectionChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnSelectionChange(SelectionChangedEventArgs e)
    {
      ClearCache();

      if (e.EventType == SelectionChangedEventType.Add || e.EventType == SelectionChangedEventType.Remove)
      {
        this.grid.InvalidateRange(e.Range);
      }
      else
      {
        this.grid.InvalidateCells();
      }

      if (SelectionChanged != null)
      {
        SelectionChanged(this, e);
      }
    }
    #endregion

    #region SelectionMode
    /// <summary>
    /// Gets or sets selection type.
    /// </summary>
    /// <value>The selection mode.</value>
    public GridSelectionMode SelectionMode
    {
      get { return this.selectionMode; }
      set { this.selectionMode = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable multi selection with the Ctrl key or Shift Key or with mouse.
    /// </summary>
    /// <value>
    /// <c>true</c> if enable multi selection; otherwise, <c>false</c>.
    /// </value>
    public bool EnableMultiSelection
    {
      get { return this.doEnableMultiSelection; }
      set { this.doEnableMultiSelection = value; }
    }
    #endregion

    /// <summary>
    /// Returns the range of the current selection.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// If the user has selected non-contiguous cells this method returns a range
    /// that contains all the selected cells.
    /// </remarks>
    public Range GetRange()
    {
      if (Count < 1)
      {
        return Range.Empty;
      }

      int l_row1 = int.MaxValue;
      int l_col1 = int.MaxValue;
      int l_row2 = int.MinValue;
      int l_col2 = int.MinValue;
      foreach (Range r in this)
      {
        if (l_row1 > r.Start.Row)
        {
          l_row1 = r.Start.Row;
        }

        if (l_col1 > r.Start.Column)
        {
          l_col1 = r.Start.Column;
        }

        if (l_row2 < r.End.Row)
        {
          l_row2 = r.End.Row;
        }

        if (l_col2 < r.End.Column)
        {
          l_col2 = r.End.Column;
        }
      }

      return new Range(l_row1, l_col1, l_row2, l_col2);
    }

    #region Selected Rows/Columns
    /// <summary>
    /// Gets an array of the rows selected
    /// </summary>
    /// <value>The selected rows.</value>
    public RowInfo[] SelectedRows
    {
      get
      {
        List<RowInfo> l_List = new List<RowInfo>();
        Range l_Range = GetRange();
        if (l_Range.IsEmpty() == false)
        {
          for (int r = l_Range.Start.Row; r <= l_Range.End.Row; r++)
          {
            if (ContainsRow(r))
            {
              l_List.Add(this.grid.Rows[r]);
            }
          }
        }

        return l_List.ToArray();
      }
    }

    /// <summary>
    /// Gets an array of the columns selected
    /// </summary>
    /// <value>The selected columns.</value>
    public ColumnInfo[] SelectedColumns
    {
      get
      {
        List<ColumnInfo> l_List = new List<ColumnInfo>();
        Range l_Range = GetRange();
        if (l_Range.IsEmpty() == false)
        {
          for (int c = l_Range.Start.Column; c <= l_Range.End.Column; c++)
          {
            if (ContainsColumn(c))
            {
              l_List.Add(this.grid.Columns[c]);
            }
          }
        }

        return l_List.ToArray();
      }
    }

    /// <summary>
    /// Gets the range collection.
    /// </summary>
    /// <value>The range collection.</value>
    public IList<Range> RangeCollection
    {
      get { return (IList<Range>)this.rangeList; }
    }
    #endregion

    #region ContextMenu
    /// <summary>
    /// images for context menu
    /// </summary>
    private ImageList menuImageList = new ImageList();

    private int imageCut;
    private int imageCopy;
    private int imagePaste;
    private int imageClear;
    private int imageFormatCells;

    /// <summary>
    /// Returns the ContextMenu used when the user Right-Click on a selected cell.
    /// </summary>
    /// <returns></returns>
    public virtual List<MenuItem> GetContextMenus()
    {
      List<MenuItem> menuItems = new List<MenuItem>();

      ////bool l_EnableCopyPasteSelection = (grid.ContextMenuStyle & ContextMenuStyles.CopyPasteSelection) == ContextMenuStyles.CopyPasteSelection;

      ////bool l_EnableClearSelection = (grid.ContextMenuStyle & ContextMenuStyles.ClearSelection) == ContextMenuStyles.ClearSelection;

      ////if (contextMenuItems != null && contextMenuItems.Count > 0)
      ////{
      ////  foreach (MenuItem m in contextMenuItems)
      ////  {
      ////    //m.OwnerDraw = true;  // TEST
      ////    menuItems.Add(m);
      ////  }
      ////  if (l_EnableClearSelection || l_EnableCopyPasteSelection) //|| l_EnablePropertySelection)
      ////  {
      ////    menuItems.Add(new MenuItem("-"));
      ////  }
      ////}

      ////if (l_EnableCopyPasteSelection)
      ////{
      ////  // CUT
      ////  MenuItem l_mnCut = new MenuItemImage("Cut", new EventHandler(Selection_Cut), m_MenuImageList, m_iImageCut);
      ////  l_mnCut.Enabled = false;
      ////  l_Array.Add(l_mnCut);

      ////  // COPY
      ////  MenuItem l_mnCopy = new MenuItemImage("Copy", new EventHandler(Selection_Copy), menuImageList, imageCopy);
      ////  menuItems.Add(l_mnCopy);

      ////  // PASTE
      ////  MenuItem l_mnPaste = new MenuItemImage("Paste", new EventHandler(Selection_Paste), menuImageList, imagePaste);
      ////  l_mnPaste.Enabled = IsValidClipboardForPaste();
      ////  menuItems.Add(l_mnPaste);
      ////}

      ////if (l_EnableClearSelection)
      ////{
      ////  if (l_EnableCopyPasteSelection)// && l_EnablePropertySelection)
      ////  {
      ////    menuItems.Add(new MenuItem("-"));
      ////  }

      ////  MenuItem l_mnClear = new MenuItemImage("Clear", new EventHandler(Selection_ClearValues), menuImageList, imageClear);
      ////  menuItems.Add(l_mnClear);
      ////}

      return menuItems;
    }

    /// <summary>
    /// Gets or sets ContextMenu of the selected cells. Null if no contextmenu is active.
    /// </summary>
    /// <value>The context menu items.</value>
    public List<MenuItem> ContextMenuItems
    {
      get { return this.contextMenuItems; }
      set { this.contextMenuItems = value; }
    }
    #endregion

    #region Clipboard
    private bool doAutoCopyPaste = true;

    /// <summary>
    /// Gets or sets a value indicating whether to enable the default copy/paste operations
    /// </summary>
    /// <value><c>true</c> if auto copy paste; otherwise, <c>false</c>.</value>
    public bool AutoCopyPaste
    {
      get { return this.doAutoCopyPaste; }
      set { this.doAutoCopyPaste = value; }
    }

    /// <summary>
    /// Copy event
    /// </summary>
    public event EventHandler ClipboardCopy;
    /// <summary>
    /// Paste event
    /// </summary>
    public event EventHandler ClipboardPaste;
    /// <summary>
    /// Cut event
    /// </summary>
    public event EventHandler ClipboardCut;

    /// <summary>
    /// Cut the content of the selected cells. NOT YET IMPLEMENTED.
    /// </summary>
    public void OnClipboardCut()
    {
      try
      {
        if (ClipboardCut != null)
        {
          ClipboardCut(this, EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Clipboard cut error: " + ex.ToString());
        MessageBox.Show("Clipboard cut error" + " Details: " + ex.Message,
          Application.ProductName + " build " + Application.ProductVersion,
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Copy the content of the selected cells
    /// </summary>
    public void OnClipboardCopy()
    {
      try
      {
        if (ClipboardCopy != null)
        {
          ClipboardCopy(this, EventArgs.Empty);
        }

        if (doAutoCopyPaste)
        {
          if (Count > 0)
          {
            // Clipboard text format
            Range l_Range = GetRange();
            System.Text.StringBuilder l_TabBuffer = new System.Text.StringBuilder();
            for (int r = l_Range.Start.Row; r <= l_Range.End.Row; r++)
            {
              for (int c = l_Range.Start.Column; c <= l_Range.End.Column; c++)
              {
                // devo controllare che la cella sia selezionata perchè la find trova soltanto gli estremi
                if (this.grid.GetCell(r, c) != null && this.grid.Selection.Contains(new Position(r, c)))
                {
                  if (this.grid.GetCell(r, c).DataModel != null)
                  {
                    l_TabBuffer.Append(this.grid.GetCell(r, c).DataModel.ValueToString(grid.GetCell(r, c).GetValue(new Position(r, c))));
                  }
                  else
                  {
                    l_TabBuffer.Append(this.grid.GetCell(r, c).GetDisplayText(new Position(r, c)));
                  }
                }

                if (c < l_Range.End.Column)
                {
                  l_TabBuffer.Append("\t");
                }
              }
              if (r < l_Range.End.Row)
              {
                l_TabBuffer.Append("\x0D\x0A");
              }
            }
            DataObject l_dataObj = new DataObject();
            l_dataObj.SetData(DataFormats.Text, l_TabBuffer.ToString());

            Clipboard.SetDataObject(l_dataObj, true);
          }
        }
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Clipboard copy error: " + ex.ToString());
        MessageBox.Show("Clipboard copy error" + " Details: " + ex.Message,
          Application.ProductName + " build " + Application.ProductVersion,
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Paste the content of the selected cells
    /// </summary>
    public void OnClipboardPaste()
    {
      try
      {
        if (ClipboardPaste != null)
        {
          ClipboardPaste(this, EventArgs.Empty);
        }

        if (doAutoCopyPaste)
        {
          if (IsValidClipboardForPaste() && Count > 0)
          {
            IDataObject l_dtObj = Clipboard.GetDataObject();
            string l_buffer = (string)l_dtObj.GetData(DataFormats.Text, true);
            // tolgo uno dei due caratteri di a capo per usare lo split
            l_buffer = l_buffer.Replace("\x0D\x0A", "\x0A");
            string[] l_buffRows = l_buffer.Split('\x0A', '\x0D');

            Range l_Range = GetRange();
            for (int r = l_Range.Start.Row; r < Math.Min(l_Range.Start.Row + l_buffRows.Length, grid.RowsCount); r++)
            {
              if (l_buffRows[r - l_Range.Start.Row].Length > 0)
              {
                string[] l_buffCols = l_buffRows[r - l_Range.Start.Row].Split('\t');
                for (int c = l_Range.Start.Column; c < Math.Min(l_Range.Start.Column + l_buffCols.Length, grid.ColumnsCount); c++)
                {
                  Cells.ICellVirtual l_Cell = grid.GetCell(r, c);
                  if (l_Cell != null && l_Cell.DataModel != null)
                  {
                    l_Cell.DataModel.SetCellValue(l_Cell, new Position(r, c), l_buffCols[c - l_Range.Start.Column]);
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Clipboard paste error: " + ex.ToString());
        MessageBox.Show("Clipboard paste error" + " Details: " + ex.Message,
          Application.ProductName + " build " + Application.ProductVersion,
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Returns if the current content of the Clipboard is valid for Paste operations
    /// </summary>
    /// <returns></returns>
    private static bool IsValidClipboardForPaste()
    {
      IDataObject dtObj = Clipboard.GetDataObject();
      return dtObj != null && dtObj.GetDataPresent(DataFormats.Text, true);
    }

    private void Selection_Cut(object sender, EventArgs e)
    {
      OnClipboardCut();
    }

    private void Selection_Paste(object sender, EventArgs e)
    {
      OnClipboardPaste();
    }

    private void Selection_Copy(object sender, EventArgs e)
    {
      OnClipboardCopy();
    }
    #endregion

    #region ClearValues
    private void Selection_ClearValues(object sender, EventArgs e)
    {
      ClearValues();
    }

    /// <summary>
    /// Clear all the selected cells with a valid Model.
    /// </summary>
    public void ClearValues()
    {
      try
      {
        if (ClearCells != null)
        {
          ClearCells(this, EventArgs.Empty);
        }

        if (AutoClear)
        {
          foreach (Position c in GetCellsPositions())
          {
            if (this.grid.GetCell(c) != null)
            {
              if (this.grid.GetCell(c).DataModel != null)
              {
                this.grid.GetCell(c).DataModel.ClearCell(this.grid.GetCell(c), c);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Clipboard clear error: " + ex.ToString());
        MessageBox.Show("Clipboard clear error" + " Details: " + ex.Message,
          Application.ProductName + " build " + Application.ProductVersion,
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the default clear operation
    /// </summary>
    /// <value><c>true</c> if default clear operation is enabled; otherwise, <c>false</c>.</value>
    public bool AutoClear
    {
      get { return this.doAutoClear; }
      set { this.doAutoClear = value; }
    }

    /// <summary>
    /// Clear event
    /// </summary>
    public event EventHandler ClearCells;
    #endregion

    #region ICollection Members
    /// <summary>
    /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
    /// </summary>
    /// <value>IsSynchronized</value>
    /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, <c>false</c>.</returns>
    public bool IsSynchronized
    {
      get
      {
        return ((ICollection)rangeList).IsSynchronized;
      }
    }

    /// <summary>
    /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
    /// </summary>
    /// <value>count</value>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
    public int Count
    {
      get
      {
        return rangeList.Count;
      }
    }

    /// <summary>
    /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see>
    /// to an <see cref="T:System.Array"></see>, starting at a particular
    /// <see cref="T:System.Array"></see> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array"></see>
    /// that is the destination of the elements copied from
    /// <see cref="T:System.Collections.ICollection"></see>. The
    /// <see cref="T:System.Array"></see> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in array at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
    /// <exception cref="T:System.ArgumentException">array is multidimensional. -or- index
    /// is equal to or greater than the length of array. -or- The number of elements
    /// in the source <see cref="T:System.Collections.ICollection"></see> is greater
    /// than the available space from index to the end of the destination array. </exception>
    /// <exception cref="T:System.InvalidCastException">The type of the
    /// source <see cref="T:System.Collections.ICollection"></see> can not be cast
    /// automatically to the type of the destination array. </exception>
    public void CopyTo(Array array, int index)
    {
      ((ICollection)rangeList).CopyTo(array, index);
    }

    /// <summary>
    /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
    /// </summary>
    /// <value>SyncRoot object</value>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
    public object SyncRoot
    {
      get
      {
        return ((ICollection)rangeList).SyncRoot;
      }
    }
    #endregion

    #region IEnumerable Members
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator GetEnumerator()
    {
      return rangeList.GetEnumerator();
    }
    #endregion

    #region IDisposable methods
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        menuImageList.Dispose();
      }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}