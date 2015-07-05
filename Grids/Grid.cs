#region MIT License
//
// Filename: Grid.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;
using Fr.Medit.MedDataGrid.Controls;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// The main grid control with static data.
  /// </summary>
  [System.ComponentModel.ToolboxItem(true)]
  public class Grid : GridVirtual
  {
    private int maximumSpanSearch = 0;
    private ICell[][] rangeSort = null;
    private ICell[] keySort;
    private ICell[,] cells = null;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Grid"/> class.
    /// </summary>
    public Grid()
    {
      this.Name = "Grid";
      this.Size = new Size(100, 100);

      Rows.RowsAdded += new IndexRangeEventHandler(rows_RowsAdded);
      Rows.RowsRemoved += new IndexRangeEventHandler(rows_RowsRemoved);

      Columns.ColumnsAdded += new IndexRangeEventHandler(columns_ColumnsAdded);
      Columns.ColumnsRemoved += new IndexRangeEventHandler(columns_ColumnsRemoved);

      Selection.SelectionChanged += new SelectionChangedEventHandler(Selection_SelectionChange);
    }
    #endregion

    #region Cells
    /// <summary>
    /// Return the Cell at the specified Row and Column position.
    /// </summary>
    /// <param name="rowIndex">The row.</param>
    /// <param name="columnIndex">The column.</param>
    /// <returns></returns>
    public override Cells.ICellVirtual GetCell(int rowIndex, int columnIndex)
    {
      return this[rowIndex, columnIndex];
    }

    /// <summary>
    /// Set the specified cell in the specified position.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <param name="columnIndex">Index of the column.</param>
    /// <param name="p_Cell">The cell.</param>
    public override void SetCell(int rowIndex, int columnIndex, Cells.ICellVirtual p_Cell)
    {
      if (p_Cell is ICell)
      {
        InsertCell(rowIndex, columnIndex, (ICell)p_Cell);
      }
      else if (p_Cell == null)
      {
        InsertCell(rowIndex, columnIndex, null);
      }
      else
      {
        throw new MEDDataGridException("Expected ICell class");
      }
    }

    private int CellsRows
    {
      get
      {
        if (this.cells == null)
        {
          return 0;
        }
        else
        {
          return this.cells.GetLength(0);
        }
      }
    }

    private int CellsCols
    {
      get
      {
        if (this.cells == null)
        {
          return 0;
        }
        else
        {
          return this.cells.GetLength(1);
        }
      }
    }

    /// <summary>
    /// Returns or set a cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <remarks>
    /// If you get an ICell position occupied by a row/col span cell, and EnableRowColSpan is
    /// true, this method returns the cell with Row/Col span.
    /// </remarks>
    /// <returns>The cell at the specified row and column</returns>
    public ICell this[int row, int column]
    {
      get
      {
        if (EnableRowColSpan == false)
        {
          return this.cells[row, column];
        }
        else // enable Row Col Span search
        {
          ICell l_RetCell = this.cells[row, column];
          if (l_RetCell == null)
          {
            int l_StartRow = row;
            int l_StartCol;
            if (column == 0)
            {
              l_StartCol = 0;
              if (l_StartRow > 0)
              {
                l_StartRow--;
              }
            }
            else
            {
              l_StartCol = column - 1;
            }

            int l_EndCol = column;
            int l_EndRow;
            if (row == 0)
            {
              l_EndRow = 0;
              if (l_EndCol > 0)
              {
                l_EndCol--;
              }
            }
            else
            {
              l_EndRow = row - 1;
            }

            Position l_ReqPos = new Position(row, column);

            // ciclo fino a che non raggiungo la fine della griglia (con un massimo di MaxSpanSearch)
            for (int l_Search = 0; l_Search < maximumSpanSearch; l_Search++)
            {
              bool isAllFull = true;

              // ciclo sulla diagonale
              for (int r = l_StartRow, c = l_StartCol; r >= l_EndRow && c <= l_EndCol; r--, c++)
              {
                if (this.cells[r, c] != null)
                {
                  if (this.cells[r, c].ContainsPosition(l_ReqPos)) // se la cella richiesta fa parte di questa cella
                  {
                    return this.cells[r, c];
                  }
                  else
                  {
                    isAllFull &= true;
                  }
                }
                else
                {
                  isAllFull = false;
                }
              }

              if (isAllFull)
              {
                return null;
              }

              if (l_StartCol > 0)
              {
                l_StartCol--;
              }
              else
              {
                l_StartRow--;
              }

              if (l_EndRow > 0)
              {
                l_EndRow--;
              }
              else
              {
                l_EndCol--;
              }

              if (l_EndCol < 0 || l_StartRow < 0)
              {
                return null;
              }
            }

            return null;
          }
          else
          {
            return l_RetCell;
          }
        }
      }
      set
      {
        InsertCell(row, column, value);
      }
    }

    /// <summary>
    /// Remove the specified cell
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="col">The column.</param>
    public virtual void RemoveCell(int row, int col)
    {
      ICell cell = this.cells[row, col];

      if (cell != null)
      {
        if (cell == MouseCell)
        {
          ChangeMouseCell(null);
        }

        cell.Select = false;
        cell.LeaveFocus();

        cell.UnBindToGrid();

        this.cells[row, col] = null;
      }
    }

    /// <summary>
    /// Removes the specified rows.
    /// </summary>
    /// <param name="rowInfos">The row infos.</param>
    public virtual void RemoveRows(RowInfo[] rowInfos)
    {
      List<Range> rangeToRemove = new List<Range>(rowInfos.Length);

      foreach (RowInfo rowInfo in rowInfos)
      {
        rangeToRemove.Add(new Range(rowInfo.Index, 0, rowInfo.Index, this.ColumnsCount - 1));
      }
      RemoveRangeRows(rangeToRemove);
    }

    /// <summary>
    /// Removes the rows specified by the range.
    /// </summary>
    /// <remarks>
    /// SAA TODO: Tidy and shorten this ugly method.
    /// </remarks>
    /// <param name="p_gridRange">The grid range.</param>
    public virtual void RemoveRangeRows(IList<Range> p_gridRange)
    {
      Range range;
      int count;
      int start;
      int removedRowsCount = 0;
      Range next_range_to_remove;
      int next_start = 0;
      bool deleteRows = false;
      int r, c;
      IEnumerator myEnum = p_gridRange.GetEnumerator();

      List<Range> l_RangesToDelete = new List<Range>();
      int[] l_RangeKeys = new int[p_gridRange.Count];  // tableau pour trier les Range
      Range[] l_RangeItems = new Range[p_gridRange.Count];
      // avant tout il faut ordonner les range avec Start
      count = 0;
      while (myEnum.MoveNext())
      {
        l_RangeKeys[count] = ((Range)myEnum.Current).Start.Row;  // trie suivant les valeurs de Start
        l_RangeItems[count] = (Range)myEnum.Current;
        ++count;
      }
      // trie du tableau
      Array.Sort(l_RangeKeys, l_RangeItems);

      // on remplace les donne de p_gridRange par celle du tableau de Range trie
      p_gridRange.Clear();
      myEnum = l_RangeItems.GetEnumerator();

      while (myEnum.MoveNext())
      {
        l_RangesToDelete.Add((Range)myEnum.Current);
      }

      myEnum = l_RangesToDelete.GetEnumerator();
      IEnumerator myEnum_2 = l_RangesToDelete.GetEnumerator();
      myEnum_2.MoveNext();
      while (myEnum.MoveNext())
      {
        range = (Range)myEnum.Current;

        count = range.End.Row - range.Start.Row + 1;
        if (count < 0 || count == 0)
        {
          count = 1;
        }

        // move to unselected cells
        if (range.End.Row == RowsCount - 1)
        {
          start = range.Start.Row;
          deleteRows = true;
        }
        else
        {
          start = range.Start.Row + count;
          deleteRows = false;
        }

        //  myEnum_2 = myEnum;
        while ((Range)myEnum_2.Current != (Range)myEnum.Current)
        {
          myEnum_2.MoveNext();
        }

        if (myEnum_2.MoveNext())
        {
          next_range_to_remove = (Range)myEnum_2.Current;
          bool fbln = true;
          if (start >= next_range_to_remove.Start.Row)
          {
            int nb_range_ignore = 0;

            // si start du range courant est au dessus du start du range suivant
            while (start >= next_range_to_remove.Start.Row)
            {
              start = next_range_to_remove.End.Row + 1;
              count += next_range_to_remove.End.Row - next_range_to_remove.Start.Row + 1;

              ++nb_range_ignore;
              fbln = myEnum_2.MoveNext();
              if (next_range_to_remove.End.Row == next_range_to_remove.Start.Row && next_range_to_remove.End.Row > start && fbln)
              {
                --count;
              }

              if (!fbln)  // si false : il n'y a plus de range dans le tableau
              {
                next_start = RowsCount - 1;
                break;
              }
              next_range_to_remove = (Range)myEnum_2.Current;
            }
            while (nb_range_ignore > 0)
            {
              myEnum.MoveNext();
              --nb_range_ignore;
            }
          }
          else
          {
            next_range_to_remove = (Range)myEnum_2.Current;
            //  next_start = next_range_to_remove.Start.Row-1;
          }
          if (fbln)
          {
            next_start = next_range_to_remove.Start.Row - 1;
            if (next_start < range.End.Row)
            {
              next_start = range.Start.Row + count - 1;
            }
          }
        }
        else
        {
          next_start = RowsCount - 1;
        }

        myEnum_2.Reset();
        myEnum_2.MoveNext();

        removedRowsCount += count;

        if (deleteRows == false)
        {
          for (r = start; r <= next_start; r++)
          {
            for (c = 0; c < ColumnsCount; c++)
            {
              Cells.ICell tmp = this[r, c];
              RemoveCell(r, c);
              InsertCell(r - removedRowsCount, c, tmp);
            }
          }
        }
        else
        {
          for (r = start; r < next_start; r++)
          {
            for (c = 0; c < ColumnsCount; c++)
            {
              RemoveCell(r, c);
            }
          }
          break;
        }
      }
      Redimension(RowsCount - removedRowsCount, ColumnsCount);
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public virtual void ClearGrid()
    {
      for (int i = 0; i < this.RowsCount; ++i)
      {
        for (int j = 0; j < this.ColumnsCount; ++j)
        {
          RemoveCell(i, j);
        }
      }
      if (Columns != null && ColumnsCount > 0)
      {
        this.Columns.RemoveRange(0, ColumnsCount);
      }
      if (Rows != null && RowsCount > 0)
      {
        this.Rows.RemoveRange(0, RowsCount);
      }
    }

    /// <summary>
    /// Determines whether this grid instance is empty.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this grid instance is empty; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsEmpty()
    {
      return RowsCount == 0 && ColumnsCount == 0;
    }

    /// <summary>
    /// Insert the specified cell.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="col">The column.</param>
    /// <param name="p_cell">The p_cell.</param>
    /// <remarks>
    /// For good performance set Redraw property to false.
    /// </remarks>
    public virtual void InsertCell(int row, int col, ICell p_cell)
    {
      RemoveCell(row, col);
      this.cells[row, col] = p_cell;

      if (p_cell != null)
      {
        p_cell.BindToGrid(this, new Position(row, col));

        if (Redraw)
        {
          p_cell.Invalidate();
        }
      }
    }
    #endregion

    #region AddRow/Col, RemoveRow/Col
    private void rows_RowsAdded(object sender, IndexRangeEventArgs e)
    {
      // N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
      // m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

      RedimCellsMatrix(CellsRows + e.Count, CellsCols);

      // dopo aver ridimensionato la matrice sposto le celle in modo da fare spazio alla nuove righe
      for (int r = CellsRows - 1; r > (e.StartIndex + e.Count - 1); r--)
      {
        for (int c = 0; c < CellsCols; c++)
        {
          ICell tmp = cells[r - e.Count, c];
          RemoveCell(r - e.Count, c);
          InsertCell(r, c, tmp);
        }
      }

      if (Redraw)
      {
        RefreshGridLayout();
      }
    }

    private void rows_RowsRemoved(object sender, IndexRangeEventArgs e)
    {
      // N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
      // m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

      for (int r = e.StartIndex + e.Count; r < CellsRows; r++)
      {
        for (int c = 0; c < CellsCols; c++)
        {
          ICell tmp = cells[r, c];
          RemoveCell(r, c);
          InsertCell(r - e.Count, c, tmp);
        }
      }

      RedimCellsMatrix(CellsRows - e.Count, CellsCols);

      if (Redraw)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Handles the ColumnsAdded event of the columns control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
    private void columns_ColumnsAdded(object sender, IndexRangeEventArgs e)
    {
      // N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
      // m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

      RedimCellsMatrix(CellsRows, CellsCols + e.Count);

      // dopo aver ridimensionato la matrice sposto le celle in modo da fare spazio alla nuove righe
      for (int c = CellsCols - 1; c > (e.StartIndex + e.Count - 1); c--)
      {
        for (int r = 0; r < CellsRows; r++)
        {
          ICell tmp = cells[r, c - e.Count];
          RemoveCell(r, c - e.Count);
          InsertCell(r, c, tmp);
        }
      }

      if (Redraw)
      {
        RefreshGridLayout();
      }
    }

    private void columns_ColumnsRemoved(object sender, IndexRangeEventArgs e)
    {
      // N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
      // m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

      for (int c = e.StartIndex + e.Count; c < CellsCols; c++)
      {
        for (int r = 0; r < CellsRows; r++)
        {
          ICell tmp = cells[r, c];
          RemoveCell(r, c);
          InsertCell(r, c - e.Count, tmp);
        }
      }

      RedimCellsMatrix(CellsRows, CellsCols - e.Count);

      if (Redraw)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Resize the cell matrix
    /// </summary>
    /// <param name="rows">The rows.</param>
    /// <param name="cols">The columns.</param>
    private void RedimCellsMatrix(int rows, int cols)
    {
      if (this.cells == null)
      {
        this.cells = new ICell[rows, cols];
      }
      else if (rows != this.cells.GetLength(0) || cols != this.cells.GetLength(1))
      {
        ICell[,] l_tmp = this.cells;
        int l_minRows = Math.Min(l_tmp.GetLength(0), rows);
        int l_minCols = Math.Min(l_tmp.GetLength(1), cols);

        // delete the unused cells
        for (int i = l_minRows; i < l_tmp.GetLength(0); i++)
        {
          for (int j = 0; j < l_tmp.GetLength(1); j++)
          {
            RemoveCell(i, j);
          }
        }
        for (int i = 0; i < l_minRows; i++)
        {
          for (int j = l_minCols; j < l_tmp.GetLength(1); j++)
          {
            RemoveCell(i, j);
          }
        }

        this.cells = new ICell[rows, cols];

        // copy
        for (int i = 0; i < l_minRows; i++)
        {
          for (int j = 0; j < l_minCols; j++)
          {
            this.cells[i, j] = l_tmp[i, j];
          }
        }
      }
    }
    #endregion

    #region Row/Col Span
    /// <summary>
    /// Gets a value indicating whether Row/Column Span is enabled. This value is automatically calculated based on the current cells.
    /// </summary>
    /// <value><c>true</c> if row/column span enabled; otherwise, <c>false</c>.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool EnableRowColSpan
    {
      get { return this.maximumSpanSearch > 0; }
    }

    /// <summary>
    /// Gets the maximum rows or columns number to search when using Row/Col Span.
    /// This value is automatically calculated based on the current cells. Do not change this value manually.
    /// </summary>
    /// <value>The maximum span search.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int MaxSpanSearch
    {
      get { return this.maximumSpanSearch; }
    }

    /// <summary>
    /// Sets the MaxSpanSearch property.
    /// </summary>
    /// <param name="p_MaxSpanSearch">The max span search.</param>
    /// <param name="p_Reset">if set to <c>true</c> force reset.</param>
    public void SetMaxSpanSearch(int p_MaxSpanSearch, bool p_Reset)
    {
      if (p_MaxSpanSearch > this.maximumSpanSearch || p_Reset)
      {
        this.maximumSpanSearch = p_MaxSpanSearch;
      }
    }
    #endregion

    #region Cell Rectangle
    /// <summary>
    /// Get the Rectangle of the cell respect all the scrollable area. Using the Cell Row/Col Span.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public override Rectangle PositionToAbsoluteRect(Position position)
    {
      ICell l_Cell = this[position.Row, position.Column];
      if (l_Cell != null)
      {
        return base.RangeToAbsoluteRect(l_Cell.Range);
      }
      else
      {
        return base.PositionToAbsoluteRect(position);
      }
    }

    /// <summary>
    /// Returns the absolute rectangle relative to the total scrollable area of the specified Range. Returns a 0 rectangle if the Range is not valid
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <returns></returns>
    public override Rectangle RangeToAbsoluteRect(Range p_Range)
    {
      if (EnableRowColSpan)
      {
        // cerco il range anche tra le celle in rowspan o colspan
        Range l_RealRange = p_Range;
        for (int r = p_Range.Start.Row; r <= p_Range.End.Row; r++)
        {
          for (int c = p_Range.Start.Column; c <= p_Range.End.Column; c++)
          {
            ICell l_Cell = this[r, c];
            if (l_Cell != null && (l_Cell.RowSpan > 1 || l_Cell.ColumnSpan > 1))
            {
              l_RealRange = Range.Union(l_RealRange, l_Cell.Range);
            }
          }
        }

        return base.RangeToAbsoluteRect(l_RealRange);
      }
      else
      {
        return base.RangeToAbsoluteRect(p_Range);
      }
    }
    #endregion

    #region Cell Visible
    /// <summary>
    /// Returns true if the specified cell is visible; otherwise <c>false</c>.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <returns>
    /// <c>true</c> if cell is visible; otherwise, <c>false</c>.
    /// </returns>
    public bool IsCellVisible(ICell p_Cell)
    {
      if (p_Cell != null)
      {
        return base.IsCellVisible(p_Cell.Range.Start);
      }
      else
      {
        return true;
      }
    }

    /// <summary>
    /// Scroll the view to show the specified cell
    /// </summary>
    /// <param name="p_CellToShow">The cell to show.</param>
    /// <returns></returns>
    public bool ShowCell(ICell p_CellToShow)
    {
      if (p_CellToShow != null)
      {
        return base.ShowCell(p_CellToShow.Range.Start);
      }
      else
      {
        return true;
      }
    }
    #endregion

    #region InvalidateCell
    /// <summary>
    /// Force a redraw of the specified cell
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    public virtual void InvalidateCell(ICell p_Cell)
    {
      if (p_Cell != null)
      {
        this.InvalidateRange(p_Cell.Range);
      }
    }

    /// <summary>
    /// Force a cell to redraw. If Redraw is set to false this function has no effects.
    /// If ColSpan or RowSpan is greater than 0 this function invalidate the complete range with InvalidateRange
    /// </summary>
    /// <param name="position">The position.</param>
    public override void InvalidateCell(Position position)
    {
      ICell l_Cell = this[position.Row, position.Column];
      if (l_Cell == null)
      {
        base.InvalidateCell(position);
      }
      else
      {
        InvalidateRange(l_Cell.Range);
      }
    }
    #endregion

    #region PaintCell
    /// <summary>
    /// Draw the specified Cell
    /// </summary>
    /// <param name="p_Panel">The panel.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="p_PanelDrawRectangle">The panel draw rectangle.</param>
    protected override void PaintCell(GridSubPanel p_Panel, PaintEventArgs e, Cells.ICellVirtual p_Cell, Position p_CellPosition, Rectangle p_PanelDrawRectangle)
    {
      ICell l_Cell = (ICell)p_Cell;
      Range l_CellRange = l_Cell.Range;
      if (l_CellRange.RowsCount == 1 && l_CellRange.ColumnsCount == 1)
      {
        base.PaintCell(p_Panel, e, p_Cell, p_CellPosition, p_PanelDrawRectangle);
      }
      else // Row/Col Span > 1
      {
        Rectangle l_Rect = p_Panel.RectangleGridToPanel(PositionToDisplayRect(l_CellRange.Start));
        base.PaintCell(p_Panel, e, p_Cell, l_CellRange.Start, l_Rect);
      }
    }
    #endregion

    #region FocusCell
    /// <summary>
    /// Change the focus of the grid. The calls order is: (the user select CellX) Grid.CellGotFocus(CellX), CellX.Enter, (the user select CellY), Grid.CellLostFocus(CellX), CellX.Leave, Grid.CellGotFocus(CellY), CellY.Enter
    /// </summary>
    /// <param name="p_CellToSetFocus">Must be a valid cell linked to the grid or null of you want to remove the focus</param>
    /// <param name="p_DeselectOtherCells">True to deselect others selected cells</param>
    /// <returns>Return true if the grid can select the cell specified, otherwise false</returns>
    public override bool SetFocusCell(Position p_CellToSetFocus, bool p_DeselectOtherCells)
    {
      if (EnableRowColSpan == false || p_CellToSetFocus.IsEmpty())
      {
        return base.SetFocusCell(p_CellToSetFocus, p_DeselectOtherCells);
      }

      ICell l_Cell = this[p_CellToSetFocus.Row, p_CellToSetFocus.Column];
      if (l_Cell != null)
      {
        return base.SetFocusCell(l_Cell.Range.Start, p_DeselectOtherCells);
      }
      else
      {
        return base.SetFocusCell(p_CellToSetFocus, p_DeselectOtherCells);
      }
    }

    /// <summary>
    /// Gets the active cell. Null if no cell are active
    /// </summary>
    /// <value>The focus cell.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ICell FocusCell
    {
      get
      {
        if (this.FocusCellPosition.IsEmpty())
        {
          return null;
        }

        return this[this.FocusCellPosition.Row, this.FocusCellPosition.Column];
      }
    }

    /// <summary>
    /// Set the focus to the specified cell (the specified cell became the active cell, FocusCell property).
    /// </summary>
    /// <param name="p_CellToSetFocus">The cell to set focus on.</param>
    /// <returns></returns>
    public bool SetFocusCell(ICell p_CellToSetFocus)
    {
      if (p_CellToSetFocus == null)
      {
        return base.SetFocusCell(Position.Empty);
      }

      return base.SetFocusCell(p_CellToSetFocus.Range.Start);
    }

    /// <summary>
    /// Get the real position for the specified position. For example when position is a merged cell this method returns the starting position of the merged cells.
    /// Usually this method returns the same cell specified as parameter. This method is used for processing arrow keys, to find a valid cell when the focus is in a merged cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public override Position GetStartingPosition(Position position)
    {
      ICell l_tmp = (ICell)GetCell(position);
      if (l_tmp != null)
      {
        return l_tmp.Range.Start;
      }
      else
      {
        return position;
      }
    }
    #endregion

    #region MouseCell
    /// <summary>
    /// Gets the cell currently under the mouse cursor. Null if no cell are under the mouse cursor.
    /// </summary>
    /// <value>The mouse cell.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ICell MouseCell
    {
      get
      {
        if (this.MouseCellPosition.IsEmpty())
        {
          return null;
        }
        else
        {
          return this[this.MouseCellPosition.Row, this.MouseCellPosition.Column];
        }
      }
    }

    /// <summary>
    /// Fired when the cell under the mouse change. For internal use only.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    protected void ChangeMouseCell(ICell p_Cell)
    {
      if (p_Cell == null)
      {
        base.ChangeMouseCell(Position.Empty);
      }
      else
      {
        base.ChangeMouseCell(p_Cell.Range.Start);
      }
    }
    #endregion

    #region Sort
    private bool doUseCustomSort = false;

    /// <summary>
    /// Gets or sets a value indicating whether when calling SortRangeRows method use a custom sort or an automatic sort. Default = false (automatic)
    /// </summary>
    /// <value><c>true</c> if custom sort enabled; otherwise, <c>false</c>.</value>
    public bool CustomSort
    {
      get { return this.doUseCustomSort; }
      set { this.doUseCustomSort = value; }
    }

    private bool doKeepSelection = true;
    /// <summary>
    /// Gets or sets a value indicating whether to keep selection after sort.
    /// </summary>
    /// <value>
    /// <c>true</c> if set keep selection after sort; otherwise, <c>false</c>.
    /// </value>
    public bool KeepSelectionAfterSort
    {
      get { return this.doKeepSelection; }
      set { this.doKeepSelection = value; }
    }

    /// <summary>
    /// Gets or sets the sort range.
    /// </summary>
    /// <value>The sort range.</value>
    public ICell[][] RangeSort
    {
      get { return this.rangeSort; }
      set { this.rangeSort = value; }
    }

    /// <summary>
    /// Gets or sets the sort key.
    /// </summary>
    /// <value>The sort key.</value>
    public ICell[] KeySort
    {
      get { return this.keySort; }
      set { this.keySort = value; }
    }

    /// <summary>
    /// Fired when calling SortRangeRows method
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.SortRangeRowsEventArgs"/> instance containing the event data.</param>
    /// <remarks>
    /// This could be optimised by removing the duplicated sorting.
    /// </remarks>
    protected override void OnSortingRangeRows(SortRangeRowsEventArgs e)
    {
      base.OnSortingRangeRows(e);

      if (CustomSort == false && EnableSort == true)
      {
        if (e.AbsoluteColumnIndexes > e.Range.End.Column && e.AbsoluteColumnIndexes < e.Range.Start.Column)
        {
          throw new ArgumentOutOfRangeException("e", "Invalid e.AbsoluteColKeys");
        }

        Range range = e.Range;

        if (this.FixedRows > 1 && this.FixedRows > e.Range.Start.Row)
        {
          range = new Range(this.FixedRows - e.Range.Start.Row + 1, e.Range.Start.Column, e.Range.End.Row, e.Range.End.Column);
        }

        ICell[][] sortableRange = new ICell[range.End.Row - range.Start.Row + 1][];
        ICell[] cellsKeys = new ICell[range.End.Row - range.Start.Row + 1];
        ICell[] selectCellsKeys = new ICell[range.End.Row - range.Start.Row + 1];

        bool[][] sortableSelection = new bool[range.End.Row - range.Start.Row + 1][];
        List<Range> selectionRange = new List<Range>();

        int sortableRow = 0;

        for (int row = range.Start.Row; row <= range.End.Row; row++)
        {
          int sortableColumn = 0;

          sortableRange[sortableRow] = new ICell[range.End.Column - range.Start.Column + 1];
          cellsKeys[sortableRow] = this[row, e.AbsoluteColumnIndexes];
          selectCellsKeys[sortableRow] = this[row, e.AbsoluteColumnIndexes];
          sortableSelection[sortableRow] = new bool[range.End.Column - range.Start.Column + 1];
          for (int column = range.Start.Column; column <= range.End.Column; column++)
          {
            sortableRange[sortableRow][sortableColumn] = this[row, column];

            if (this.doKeepSelection == true && this[row, column] != null)
            {
              sortableSelection[sortableRow][sortableColumn] = this[row, column].Select;
            }

            sortableColumn++;
          }

          sortableRow++;
        }

        IComparer cellComparer = e.CellComparer;
        if (cellComparer == null)
        {
          cellComparer = new ValueCellComparer();
        }

        // SAA TODO: This is inefficient - we don't want 2 identical sorts!
        Array.Sort(cellsKeys, sortableRange, cellComparer);
        if (this.doKeepSelection == true)
        {
          Array.Sort(selectCellsKeys, sortableSelection, cellComparer);
        }

        // Apply sort
        sortableRow = 0;
        for (int i = 0; i <= range.End.Row - range.Start.Row; i++)
        {
          int row;
          if (e.Ascending)
          {
            row = range.Start.Row + i;
          }
          else
          {
            row = range.End.Row - i;
          }
          int sortableColumn = 0;
          for (int column = range.Start.Column; column <= range.End.Column; column++)
          {
            ICell sortedCell = sortableRange[sortableRow][sortableColumn];

            if (sortedCell != null && sortedCell.Grid != null && sortedCell.Range.Start.Row >= 0 && sortedCell.Range.Start.Column >= 0)
            {
              RemoveCell(sortedCell.Range.Start.Row, sortedCell.Range.Start.Column);
            }

            this[row, column] = sortedCell;
            sortableColumn++;
          }

          if (this.doKeepSelection == true && sortableSelection[sortableRow][1] == true)
          {
            Range ra = new Range(new Position(row, 0), new Position(row, range.End.Column));
            selectionRange.Add(ra);
          }
          sortableRow++;
        }

        if (this.doKeepSelection == true)
        {
          // and now we restore the selection in the grid
          foreach (Range selectedRange in selectionRange)
          {
            Selection.AddRange(selectedRange);
          }
        }
      }

      this.OnSortedRangeRows(e);
    }
    #endregion

    #region Selection
    private void Selection_SelectionChange(object sender, SelectionChangedEventArgs e)
    {
      if (EnableRowColSpan)
      {
        if (e.EventType == SelectionChangedEventType.Add)
        {
          for (int r = e.Range.Start.Row; r <= e.Range.End.Row; r++)
          {
            for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
            {
              ICell l_Cell = this[r, c];
              if (l_Cell != null)
              {
                Range l_Range = l_Cell.Range;

                if (l_Range != new Range(new Position(r, c)))
                {
                  Selection.AddRange(l_Range);
                }
              }
            }
          }
        }
        else if (e.EventType == SelectionChangedEventType.Remove)
        {
          for (int r = e.Range.Start.Row; r <= e.Range.End.Row; r++)
          {
            if (Selection.SelectionMode != GridSelectionMode.Row)
            {
              for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
              {
                ICell l_Cell = this[r, c];
                if (l_Cell != null)
                {
                  Range l_Range = l_Cell.Range;

                  if (l_Range != new Range(new Position(r, c)))
                  {
                    Selection.RemoveRange(l_Range);
                  }
                }
              }
            }
            else if (Selection.ContainsRow(e.Range.Start.Row))
            {
              Selection.RemoveRange(new Range(new Position(r, 0), new Position(r, ColumnsCount - 1)));
            }
          }
        }
      }
    }
    #endregion
  }
}