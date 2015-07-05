#region MIT License
//
// Filename: RowInfo.cs
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Row Information
  /// </summary>
  [ComVisible(false)]
  public class RowInfo
  {
    #region Constants
    /// <summary>
    /// Default Cell height
    /// </summary>
    public const int DefaultCellHeight = 20;
    #endregion

    #region Class variables
    private int height = DefaultCellHeight;
    private int top;
    private GridVirtual ownerGrid;
    private AutoSizeModes autoSizeMode = AutoSizeModes.EnableAutoSize | AutoSizeModes.EnableStretch;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RowInfo"/> class.
    /// </summary>
    /// <param name="grid">The grid.</param>
    private RowInfo(GridVirtual grid)
    {
      this.ownerGrid = grid;
    }

    /// <summary>
    /// Gets or sets teight of the current row
    /// </summary>
    /// <value>The height.</value>
    public int Height
    {
      get
      {
        return this.height;
      }
      set
      {
        if (this.height != value)
        {
          this.height = value;
          if (this.ownerGrid != null)
          {
            this.ownerGrid.Rows.InvokeRowHeightChanged(new RowInfoEventArgs(this));
          }
        }
      }
    }

    /// <summary>
    /// Gets the top absolute position of the current row
    /// </summary>
    /// <value>The top.</value>
    public int Top
    {
      get { return this.top; }
    }

    /// <summary>
    /// Gets the Bottom of the row (Top+Height)
    /// </summary>
    /// <value>The bottom.</value>
    public int Bottom
    {
      get { return this.top + this.height; }
    }

    /// <summary>
    /// Gets the Index of the current row
    /// </summary>
    /// <value>The index.</value>
    public int Index
    {
      get { return this.ownerGrid.Rows.IndexOf(this); }
    }

    /// <summary>
    /// Gets the owner Grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.ownerGrid; }
    }

    /// <summary>
    /// Returns all the cells at current row position
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MEDDataGridException">Thrown if invalid grid object.</exception>
    public Cells.ICellVirtual[] GetCells()
    {
      if (this.ownerGrid == null)
      {
        throw new MEDDataGridException("Invalid Grid object");
      }

      int l_CurrentRow = Index;

      Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[ownerGrid.Columns.Count];
      for (int c = 0; c < ownerGrid.Columns.Count; c++)
      {
        l_Cells[c] = ownerGrid.GetCell(l_CurrentRow, c);
      }

      return l_Cells;
    }

    /// <summary>
    /// Set the specified cells at the current row position
    /// </summary>
    /// <param name="p_Cells">The cells.</param>
    /// <exception cref="MEDDataGridException">Thrown if invalid grid object.</exception>
    public void SetCells(params Cells.ICellVirtual[] p_Cells)
    {
      if (ownerGrid == null)
      {
        throw new MEDDataGridException("Invalid Grid object");
      }

      if (p_Cells != null)
      {
        int l_CurrentRow = Index;

        for (int c = 0; c < p_Cells.Length; c++)
        {
          ownerGrid.SetCell(l_CurrentRow, c, p_Cells[c]);
        }
      }
    }

    /// <summary>
    /// Move the Focus to the first cell not fixed of the current row
    /// </summary>
    /// <returns></returns>
    public bool Focus()
    {
      if (Grid.ColumnsCount > Grid.FixedColumns && Grid.RowsCount > Index)
      {
        return Grid.SetFocusCell(new Position(Index, Grid.FixedColumns));
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current row is selected. If only a column of the row is selected this property returns true.
    /// </summary>
    /// <value><c>true</c> if select; otherwise, <c>false</c>.</value>
    public bool Select
    {
      get
      {
        return Grid.Selection.ContainsRow(Index);
      }
      set
      {
        if (Grid.ColumnsCount > 0 && Grid.RowsCount > Index)
        {
          if (value)
          {
            Grid.Selection.AddRange(new Range(Index, 0, Index, Grid.ColumnsCount - 1));
          }
          else
          {
            Grid.Selection.RemoveRange(new Range(Index, 0, Index, Grid.ColumnsCount - 1));
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets Flags for autosize and stretch
    /// </summary>
    /// <value>The auto size mode.</value>
    public AutoSizeModes AutoSizeMode
    {
      get { return this.autoSizeMode; }
      set { this.autoSizeMode = value; }
    }

    #region RowInfoCollection
    /// <summary>
    /// Collection of RowInfo
    /// </summary>
    [ComVisible(false)]
    public class RowInfoCollection : ICollection<RowInfo>
    {
      private GridVirtual ownerGrid;

      private List<RowInfo> rowInfoList;

      private RowInfoTopComparer comparer = new RowInfoTopComparer();
      private bool doAllowMoveRowsAfterRemoving = true;

      /// <summary>
      /// Initializes a new instance of the <see cref="RowInfoCollection"/> class.
      /// </summary>
      /// <param name="grid">The grid.</param>
      public RowInfoCollection(GridVirtual grid)
      {
        this.ownerGrid = grid;
        this.rowInfoList = new List<RowInfo>();
      }

      /// <summary>
      /// Gets the owner Grid
      /// </summary>
      /// <value>The grid.</value>
      public GridVirtual Grid
      {
        get { return ownerGrid; }
      }

      #region Comparer
      [ComVisible(false)]
      public class RowInfoTopComparer : IComparer<RowInfo>
      {
        public System.Int32 Compare(RowInfo x, RowInfo y)
        {
          return x.Top.CompareTo(y.Top);
        }
      }
      #endregion

      /// <summary>
      /// Calculate the Row that have the Top value smaller or equal than the point p_Y, or -1 if not found found. ExactMatch = false
      /// </summary>
      /// <param name="p_Y">Absolute point to search</param>
      /// <returns></returns>
      public int RowAtPoint(int p_Y)
      {
        return RowAtPoint(p_Y, false);
      }

      /// <summary>
      /// Calculate the Row that have the Top value smaller or equal than the point p_Y, or -1 if not found found.
      /// </summary>
      /// <param name="p_Y">Y Coordinate to search for a row</param>
      /// <param name="p_ExactMatch">True to returns only exact position. For example if you use a point outside the range and this value is true no row is returned otherwise the nearest row is returned.</param>
      /// <returns></returns>
      public int RowAtPoint(int p_Y, bool p_ExactMatch)
      {
        // Restituisce la righa con il Left uguale a quello passato o la righa con il Left minore a quallo passato.
        // o -1 se tutte le righe hanno il Left maggiore
        int l_IndexFound;

        RowInfo l_Find = new RowInfo(null);
        l_Find.top = p_Y;
        int l_ObjFound = rowInfoList.BinarySearch(l_Find, comparer);
        if (l_ObjFound >= 0) // trovato il valore uguale
        {
          l_IndexFound = l_ObjFound;
        }
        else
        {
          l_ObjFound = ~l_ObjFound; // bitwise operator to return the nearest index
          if (l_ObjFound <= 0)
          {
            l_IndexFound = -1; // nessuna righa compatibile
          }
          else if (l_ObjFound <= rowInfoList.Count)
          {
            l_IndexFound = l_ObjFound - 1; // trovata una righa compatibile
          }
          else
          {
            l_IndexFound = -1; // non dovrebbe mai capitare
          }
        }

        // se è stato richiesto un exactMatch verifico che il punto sia compreso tra il minimo e il massimo
        if (p_ExactMatch && l_IndexFound >= 0 && (p_Y > Bottom || p_Y < Top))
        {
          l_IndexFound = -1;
        }

        return l_IndexFound;
      }

      /// <summary>
      /// Returns true if the specified range is valid
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <returns>
      /// <c>true</c> if the specified range is valid; otherwise, <c>false</c>.
      /// </returns>
      public bool IsValidRange(int p_StartIndex, int p_Count)
      {
        return p_StartIndex < Count && p_StartIndex >= 0 &&
          p_Count > 0 && (p_StartIndex + p_Count) <= Count;
      }

      /// <summary>
      /// Returns true if the range passed is valid for insert method
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <returns>
      /// <c>true</c> if specified range is valid for insert; otherwise, <c>false</c>.
      /// </returns>
      public bool IsValidRangeForInsert(int p_StartIndex, int p_Count)
      {
        return p_StartIndex <= Count && p_StartIndex >= 0 && p_Count > 0;
      }

      #region Insert/Remove Methods
      /// <summary>
      /// Insert a row at the specified position using the specified cells
      /// </summary>
      /// <param name="p_Index">The row index.</param>
      /// <param name="p_Cells">The new row values</param>
      public void Insert(int p_Index, params Cells.ICellVirtual[] p_Cells)
      {
        Insert(p_Index);

        this[p_Index].SetCells(p_Cells);
      }

      /// <summary>
      /// Insert a row at the specified position
      /// </summary>
      /// <param name="p_Index">The row index.</param>
      public void Insert(int p_Index)
      {
        InsertRange(p_Index, 1);
      }

      /// <summary>
      /// Remove a row at the speicifed position
      /// </summary>
      /// <param name="p_Index">The row index.</param>
      public void Remove(int p_Index)
      {
        RemoveRange(p_Index, 1);
      }

      /// <summary>
      /// Insert the specified number of rows at the specified position
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <exception cref="MEDDataGridException">Thrown if invalid index passed</exception>
      public void InsertRange(int p_StartIndex, int p_Count)
      {
        if (IsValidRangeForInsert(p_StartIndex, p_Count) == false)
        {
          throw new MEDDataGridException("Invalid index");
        }

        for (int r = 0; r < p_Count; r++)
        {
          rowInfoList.Insert(p_StartIndex + r, new RowInfo(ownerGrid));
        }

        if (AutoCalculateTop)
        {
          CalculateTop(p_StartIndex);
        }

        OnRowsAdded(new IndexRangeEventArgs(p_StartIndex, p_Count));
      }

      /// <summary>
      /// Remove the RowInfo at the specified positions
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <exception cref="MEDDataGridException">Thrown if invalid index passed</exception>
      public void RemoveRange(int p_StartIndex, int p_Count)
      {
        if (IsValidRange(p_StartIndex, p_Count) == false)
        {
          throw new MEDDataGridException("Invalid index");
        }

        rowInfoList.RemoveRange(p_StartIndex, p_Count);

        if (AutoCalculateTop)
        {
          CalculateTop(p_StartIndex, p_StartIndex + p_Count);
        }

        if (AllowMoveRowsAfterRemoving)
        {
          OnRowsRemoved(new IndexRangeEventArgs(p_StartIndex, p_Count));
        }
      }
      #endregion

      /// <summary>
      /// Move a row from one position to another position
      /// </summary>
      /// <param name="p_CurrentRowPosition">The current row position.</param>
      /// <param name="p_NewRowPosition">The new row position.</param>
      public void Move(int p_CurrentRowPosition, int p_NewRowPosition)
      {
        if (p_CurrentRowPosition == p_NewRowPosition)
        {
          return;
        }

        int l_RowMin, l_RowMax;
        if (p_CurrentRowPosition < p_NewRowPosition)
        {
          l_RowMin = p_CurrentRowPosition;
          l_RowMax = p_NewRowPosition;
        }
        else
        {
          l_RowMin = p_NewRowPosition;
          l_RowMax = p_CurrentRowPosition;
        }

        Swap(l_RowMin, l_RowMax);
      }

      /// <summary>
      /// Change the position of row 1 with row 2.
      /// </summary>
      /// <param name="p_RowIndex1">The 1st row index.</param>
      /// <param name="p_RowIndex2">The 2nd row index.</param>
      public void Swap(int p_RowIndex1, int p_RowIndex2)
      {
        if (p_RowIndex1 == p_RowIndex2)
        {
          return;
        }

        RowInfo l_Row1 = this[p_RowIndex1];
        Cells.ICellVirtual[] l_Cells1 = l_Row1.GetCells();
        RowInfo l_Row2 = this[p_RowIndex2];
        Cells.ICellVirtual[] l_Cells2 = l_Row2.GetCells();

        rowInfoList[p_RowIndex1] = l_Row2;
        rowInfoList[p_RowIndex2] = l_Row1;

        l_Row1.SetCells(new Cells.ICellVirtual[l_Cells1.Length]);
        l_Row2.SetCells(new Cells.ICellVirtual[l_Cells1.Length]);
        l_Row1.SetCells(l_Cells1);
        l_Row2.SetCells(l_Cells2);

        if (AutoCalculateTop)
        {
          CalculateTop(0);
        }
      }

      /// <summary>
      /// Fired when the number of rows change
      /// </summary>
      public event IndexRangeEventHandler RowsAdded;

      /// <summary>
      /// Fired when the number of rows change
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
      protected virtual void OnRowsAdded(IndexRangeEventArgs e)
      {
        if (RowsAdded != null)
        {
          RowsAdded(this, e);
        }
      }

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      public event IndexRangeEventHandler RowsRemoved;

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
      protected virtual void OnRowsRemoved(IndexRangeEventArgs e)
      {
        if (RowsRemoved != null)
        {
          RowsRemoved(this, e);
        }
      }

      /// <summary>
      /// Gets a <see cref="Fr.Medit.MedDataGrid.RowInfo"/> instance at the specified position.
      /// </summary>
      /// <param name="p">Position.</param>
      /// <value>RowInfo</value>
      /// <returns>The RowInfo instance at the specified position.</returns>
      public RowInfo this[int p]
      {
        get { return (RowInfo)rowInfoList[p]; }
      }

      /// <summary>
      /// Recalculate all the top positions from the specified index.
      /// </summary>
      /// <param name="p_StartIndex">Start index</param>
      /// <param name="p_EndIndex">End index</param>
      public void CalculateTop(int p_StartIndex, int p_EndIndex)
      {
        if (Count > 0)
        {
          int l_CurrentTop = 0;
          if (p_StartIndex != 0)
          {
            l_CurrentTop = this[p_StartIndex - 1].Top + this[p_StartIndex - 1].Height;
          }

          for (int r = p_StartIndex; r < Count; r++)
          {
            if (r < p_EndIndex)
            {
              this[r].ownerGrid = null;
            }
            this[r].top = l_CurrentTop;
            l_CurrentTop += this[r].height;
          }
        }
      }

      /// <summary>
      /// Recalculate all the top positions from the specified index.
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      public void CalculateTop(int p_StartIndex)
      {
        if (Count > 0)
        {
          int l_CurrentTop = 0;
          if (p_StartIndex != 0)
          {
            l_CurrentTop = this[p_StartIndex - 1].Top + this[p_StartIndex - 1].Height;
          }

          for (int r = p_StartIndex; r < Count; r++)
          {
            this[r].top = l_CurrentTop;
            l_CurrentTop += this[r].height;
          }
        }
      }

      /// <summary>
      /// Gets the bottom-most row.
      /// </summary>
      /// <remarks>
      /// Returns the maximum bottom value of the rows. Calculated with Rows[lastRow].Bottom or 0 if no rows are presents.
      /// </remarks>
      /// <value>The bottom.</value>
      public int Bottom
      {
        get
        {
          if (Count <= 0)
          {
            return 0;
          }
          else
          {
            return this[Count - 1].Bottom;
          }
        }
      }

      /// <summary>
      /// Gets the top-most row.
      /// </summary>
      /// <remarks>
      /// Returns the minimum top value of the rows. Calculated with Rows[0].Top or 0 if no rows are presents.
      /// </remarks>
      /// <value>The top.</value>
      public int Top
      {
        get
        {
          if (Count <= 0)
          {
            return 0;
          }
          else
          {
            return this[0].Top;
          }
        }
      }

      /// <summary>
      /// Fired when the user change the Height property of one of the Row
      /// </summary>
      public event RowInfoEventHandler RowHeightChanged;

      /// <summary>
      /// Execute the RowHeightChanged event
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.RowInfoEventArgs"/> instance containing the event data.</param>
      public void InvokeRowHeightChanged(RowInfoEventArgs e)
      {
        if (AutoCalculateTop)
        {
          CalculateTop(e.Row.Index);
        }

        if (RowHeightChanged != null)
        {
          RowHeightChanged(this, e);
        }
      }

      private bool doAutoCalculateTop = true;

      /// <summary>
      /// Gets or sets a value indicating whether auto recalculate top position when height value change. Default = true. Can be used when you need to change many times Height value for example for an AutoSize operation to increase performance.
      /// </summary>
      /// <value><c>true</c> if set to auto calculate top position; otherwise, <c>false</c>.</value>
      public bool AutoCalculateTop
      {
        get
        {
          return this.doAutoCalculateTop;
        }
        set
        {
          this.doAutoCalculateTop = value;
          if (this.doAutoCalculateTop)
          {
            CalculateTop(0);
          }
        }
      }

      /// <summary>
      /// returns index the of the first occurrence of the specified RowInfo.
      /// </summary>
      /// <param name="info">The info.</param>
      /// <returns></returns>
      public int IndexOf(RowInfo info)
      {
        return rowInfoList.IndexOf(info);
      }

      /// <summary>
      /// Gets or sets a value indicating whether to allow move rows after removing.
      /// </summary>
      /// <value>
      ///   <c>true</c> if set to allow move rows after removing; otherwise, <c>false</c>.
      /// </value>
      public bool AllowMoveRowsAfterRemoving
      {
        get { return this.doAllowMoveRowsAfterRemoving; }
        set { this.doAllowMoveRowsAfterRemoving = value; }
      }

      #region ICollection<RowInfo>
      /// <summary>
      /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
      /// </summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in array at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
      /// <exception cref="T:System.ArgumentException">array is multidimensional. -or- index is equal to or greater than the length of array. -or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
      /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> can not be cast automatically to the type of the destination array. </exception>
      public virtual void CopyTo(RowInfo[] array, System.Int32 index)
      {
        rowInfoList.CopyTo(array, index);
      }

      /// <summary>
      /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
      /// </summary>
      /// <value>count</value>
      /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
      public int Count
      {
        get { return this.rowInfoList.Count; }
      }

      /// <summary>
      /// Adds the specified row.
      /// </summary>
      /// <param name="row">The row.</param>
      public virtual void Add(RowInfo row)
      {
        rowInfoList.Add(row);
      }

      /// <summary>
      /// Determines whether the collection contains the specified row.
      /// </summary>
      /// <param name="row">The row.</param>
      /// <returns>
      /// <c>true</c> if containsthe specified row; otherwise, <c>false</c>.
      /// </returns>
      public virtual bool Contains(RowInfo row)
      {
        return rowInfoList.Contains(row);
      }

      /// <summary>
      /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
      /// </summary>
      /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
      public virtual void Clear()
      {
        rowInfoList.Clear();
      }

      /// <summary>
      /// Removes the specified row.
      /// </summary>
      /// <param name="row">The row.</param>
      /// <returns></returns>
      public virtual bool Remove(RowInfo row)
      {
        return rowInfoList.Remove(row);
      }

      /// <summary>
      /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
      /// </summary>
      /// <value>Whether read only</value>
      /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, <c>false</c>.</returns>
      public virtual bool IsReadOnly
      {
        get { return false; }
      }

      /// <summary>
      /// Returns an enumerator that iterates through a collection.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
      /// </returns>
      public IEnumerator<RowInfo> GetEnumerator()
      {
        return rowInfoList.GetEnumerator();
      }

      /// <summary>
      /// Returns an enumerator that iterates through a collection.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
      /// </returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
        return this.GetEnumerator();
      }
      #endregion
    }
    #endregion
  }
}