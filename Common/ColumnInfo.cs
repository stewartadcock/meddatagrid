#region MIT License
//
// Filename: ColumnInfo.cs
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
  /// Column Information
  /// </summary>
  [ComVisible(false)]
  public class ColumnInfo
  {
    #region Constants
    /// <summary>
    /// Default cell width
    /// </summary>
    public const int DefaultCellWidth = 50;
    #endregion

    #region Class variables
    private int width = DefaultCellWidth;
    private int left;
    private GridVirtual ownerGrid;
    private AutoSizeModes autoSizeMode = AutoSizeModes.EnableAutoSize | AutoSizeModes.EnableStretch;
    private bool isResizable;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnInfo"/> class.
    /// </summary>
    /// <param name="grid">The grid.</param>
    private ColumnInfo(GridVirtual grid)
    {
      this.ownerGrid = grid;
      this.isResizable = true;
    }

    /// <summary>
    /// Returns all the cells at current column position
    /// </summary>
    /// <returns></returns>
    public Cells.ICellVirtual[] GetCells()
    {
      if (ownerGrid == null)
      {
        throw new MEDDataGridException("Invalid Grid object");
      }

      int l_CurrentCol = Index;

      Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[ownerGrid.Rows.Count];
      for (int r = 0; r < ownerGrid.Rows.Count; r++)
      {
        l_Cells[r] = ownerGrid.GetCell(r, l_CurrentCol);
      }

      return l_Cells;
    }

    #region Properties
    /// <summary>
    /// Set the specified cells at the current row position
    /// </summary>
    /// <param name="cells">The cells.</param>
    public void SetCells(params Cells.ICellVirtual[] cells)
    {
      if (this.ownerGrid == null)
      {
        throw new MEDDataGridException("Invalid Grid object");
      }

      if (cells != null)
      {
        int l_CurrentCol = Index;

        for (int r = 0; r < cells.Length; r++)
        {
          ownerGrid.SetCell(r, l_CurrentCol, cells[r]);
        }
      }
    }

    /// <summary>
    /// Move the Focus to the first cell not fixed of the current row
    /// </summary>
    /// <returns></returns>
    public bool Focus()
    {
      if (Index >= Grid.ColumnsCount || Grid.FixedRows >= Grid.RowsCount)
      {
        return false;
      }

      return Grid.SetFocusCell(new Position(Grid.FixedRows, Index));
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current row is selected.
    /// If only a column of the row is selected this property returns true.
    /// </summary>
    /// <value><c>true</c> if select; otherwise, <c>false</c>.</value>
    public bool Select
    {
      get
      {
        return Grid.Selection.ContainsColumn(Index);
      }
      set
      {
        if (Grid.ColumnsCount > Index && Grid.RowsCount > 0)
        {
          if (value)
          {
            Grid.Selection.AddRange(new Range(0, Index, Grid.RowsCount - 1, Index));
          }
          else
          {
            Grid.Selection.RemoveRange(new Range(0, Index, Grid.RowsCount - 1, Index));
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the autosize and stretch mode.
    /// </summary>
    /// <value>The auto size mode.</value>
    public AutoSizeModes AutoSizeMode
    {
      get { return this.autoSizeMode; }
      set { this.autoSizeMode = value; }
    }

    /// <summary>
    /// Gets or sets the width of the current Column
    /// </summary>
    /// <value>The width.</value>
    public int Width
    {
      get
      {
        return width;
      }
      set
      {
        if (value < 0)
        {
          value = 1;
        }
        if (width != value)
        {
          width = value;
          if (ownerGrid != null)
          {
            ownerGrid.Columns.InvokeColumnWidthChanged(new ColumnInfoEventArgs(this));
          }
        }
      }
    }

    /// <summary>
    /// Gets the left absolute position of the current Column
    /// </summary>
    /// <value>The left.</value>
    public int Left
    {
      get { return left; }
    }

    /// <summary>
    /// Gets the right position of the column (Left+Width)
    /// </summary>
    /// <value>The right.</value>
    public int Right
    {
      get { return Left + Width; }
    }

    /// <summary>
    /// Gets index of the current column.
    /// </summary>
    /// <value>The index.</value>
    public int Index
    {
      get { return this.ownerGrid.Columns.IndexOf(this); }
    }

    /// <summary>
    /// Gets the attached grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.ownerGrid; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this column is resizable.
    /// </summary>
    /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
    public bool Resizable
    {
      get { return this.isResizable; }
      set { this.isResizable = value; }
    }
    #endregion

    #region ColumnInfoCollection
    /// <summary>
    /// Collection of ColumnInfo
    /// </summary>
    [ComVisible(false)]
    public class ColumnInfoCollection : ICollection<ColumnInfo>
    {
      #region Class variables
      private bool doAutoCalculateLeft = true;
      private GridVirtual ownerGrid;
      private List<ColumnInfo> columnInfoList;
      private ColumnInfoLeftComparer comparer = new ColumnInfoLeftComparer();
      #endregion

      /// <summary>
      /// Initializes a new instance of the <see cref="ColumnInfoCollection"/> class.
      /// </summary>
      /// <param name="grid">The grid.</param>
      public ColumnInfoCollection(GridVirtual grid)
      {
        this.ownerGrid = grid;
        this.columnInfoList = new List<ColumnInfo>();
      }

      /// <summary>
      /// Gets the attached Grid
      /// </summary>
      /// <value>The grid.</value>
      public GridVirtual Grid
      {
        get { return this.ownerGrid; }
      }

      #region Comparer
      /// <summary>
      /// ColumnInfoLeftComparer
      /// </summary>
      [ComVisible(false)]
      public class ColumnInfoLeftComparer : IComparer<ColumnInfo>
      {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        /// </returns>
        public System.Int32 Compare(ColumnInfo x, ColumnInfo y)
        {
          return x.Left.CompareTo(y.Left);
        }
      }
      #endregion

      /// <summary>
      /// Calculate the Column that have the Left value smaller or equal than the point p_X, or -1 if not found found. ExactMatch = false
      /// </summary>
      /// <param name="p_X">Absolute point to search</param>
      /// <returns></returns>
      public int ColumnAtPoint(int p_X)
      {
        return ColumnAtPoint(p_X, false);
      }

      /// <summary>
      /// Calculate the Column that have the Left value smaller or equal than the point p_X, or -1 if not found found.
      /// </summary>
      /// <param name="p_X">X Coordinate to search for a column</param>
      /// <param name="p_ExactMatch">True to returns only exact position. For example if you use a point outside the range and this value is true no column is returned otherwise the nearest column is returned.</param>
      /// <returns></returns>
      public int ColumnAtPoint(int p_X, bool p_ExactMatch)
      {
        // Restituisce la righa con il Left uguale a quello passato o la righa con il Left minore a quallo passato.
        // o -1 se tutte le righe hanno il Left maggiore
        int l_IndexFound;

        ColumnInfo l_Find = new ColumnInfo(null);
        l_Find.left = p_X;
        int l_ObjFound = columnInfoList.BinarySearch(l_Find, comparer);
        if (l_ObjFound >= 0)
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
          else if (l_ObjFound <= columnInfoList.Count)
          {
            l_IndexFound = l_ObjFound - 1; // trovata una righa compatibile
          }
          else
          {
            l_IndexFound = -1; // non dovrebbe mai capitare
          }
        }

        // se è stato richiesto un exactMatch verifico che il punto sia compreso tra il minimo e il massimo
        if (p_ExactMatch && l_IndexFound >= 0 && (p_X > Right ||
            p_X < Left))
        {
          l_IndexFound = -1;
        }

        return l_IndexFound;
      }

      /// <summary>
      /// Returns true if the range passed is valid
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <returns>
      /// <c>true</c> if this is a valid range; otherwise, <c>false</c>.
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
      /// <c>true</c> if specified start index is valid range for insert; otherwise, <c>false</c>.
      /// </returns>
      public bool IsValidRangeForInsert(int p_StartIndex, int p_Count)
      {
        return p_StartIndex <= Count && p_StartIndex >= 0 &&
          p_Count > 0;
      }

      #region Insert/Remove Methods
      /// <summary>
      /// Insert a column at the specified position using the specified cells
      /// </summary>
      /// <param name="p_Index">Index of the p_.</param>
      /// <param name="p_Cells">The new column values</param>
      public void Insert(int p_Index, params Cells.ICellVirtual[] p_Cells)
      {
        Insert(p_Index);

        this[p_Index].SetCells(p_Cells);
      }

      /// <summary>
      /// Insert a column at the specified position
      /// </summary>
      /// <param name="p_Index">Index</param>
      public void Insert(int p_Index)
      {
        InsertRange(p_Index, 1);
      }

      /// <summary>
      /// Remove a column at the speicifed position
      /// </summary>
      /// <param name="p_Index">Index</param>
      public void Remove(int p_Index)
      {
        RemoveRange(p_Index, 1);
      }

      /// <summary>
      /// Insert the specified number of Columns at the specified position
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      /// <exception cref="MEDDataGridException">Thrown if the index is invalid.</exception>
      public void InsertRange(int p_StartIndex, int p_Count)
      {
        if (IsValidRangeForInsert(p_StartIndex, p_Count) == false)
        {
          throw new MEDDataGridException("Invalid index");
        }

        for (int c = 0; c < p_Count; c++)
        {
          columnInfoList.Insert(p_StartIndex + c, new ColumnInfo(ownerGrid));
        }

        if (AutoCalculateLeft)
        {
          CalculateLeft(p_StartIndex);
        }

        OnColumnsAdded(new IndexRangeEventArgs(p_StartIndex, p_Count));
      }

      /// <summary>
      /// Remove the ColumnInfo at the specified positions
      /// </summary>
      /// <param name="p_StartIndex">Start index.</param>
      /// <param name="p_Count">The count.</param>
      public void RemoveRange(int p_StartIndex, int p_Count)
      {
        if (IsValidRange(p_StartIndex, p_Count) == false)
        {
          throw new MEDDataGridException("Invalid index");
        }

        for (int c = p_StartIndex; c < p_StartIndex + p_Count; c++)
        {
          this[c].ownerGrid = null;
        }

        columnInfoList.RemoveRange(p_StartIndex, p_Count);

        if (AutoCalculateLeft)
        {
          CalculateLeft(p_StartIndex);
        }

        OnColumnsRemoved(new IndexRangeEventArgs(p_StartIndex, p_Count));
      }
      #endregion

      /// <summary>
      /// Move a column from one position to another position
      /// </summary>
      /// <param name="p_CurrentColumnPosition">The current column position.</param>
      /// <param name="p_NewColumnPosition">The new column position.</param>
      public void Move(int p_CurrentColumnPosition, int p_NewColumnPosition)
      {
        if (p_CurrentColumnPosition == p_NewColumnPosition)
        {
          return;
        }

        int l_ColumnMin, l_ColumnMax;
        if (p_CurrentColumnPosition < p_NewColumnPosition)
        {
          l_ColumnMin = p_CurrentColumnPosition;
          l_ColumnMax = p_NewColumnPosition;
        }
        else
        {
          l_ColumnMin = p_NewColumnPosition;
          l_ColumnMax = p_CurrentColumnPosition;
        }

        for (int r = l_ColumnMin; r < l_ColumnMax; r++)
        {
          Swap(r, r + 1);
        }
      }

      /// <summary>
      /// Change the position of column 1 with column 2.
      /// </summary>
      /// <param name="p_ColumnIndex1">The column index1.</param>
      /// <param name="p_ColumnIndex2">The column index2.</param>
      public void Swap(int p_ColumnIndex1, int p_ColumnIndex2)
      {
        if (p_ColumnIndex1 == p_ColumnIndex2)
        {
          return;
        }

        ColumnInfo l_Column1 = this[p_ColumnIndex1];
        Cells.ICellVirtual[] l_Cells1 = l_Column1.GetCells();
        ColumnInfo l_Column2 = this[p_ColumnIndex2];
        Cells.ICellVirtual[] l_Cells2 = l_Column2.GetCells();

        columnInfoList[p_ColumnIndex1] = l_Column2;
        columnInfoList[p_ColumnIndex2] = l_Column1;

        l_Column1.SetCells(new Cells.ICellVirtual[l_Cells1.Length]);
        l_Column2.SetCells(new Cells.ICellVirtual[l_Cells1.Length]);
        l_Column1.SetCells(l_Cells1);
        l_Column2.SetCells(l_Cells2);

        if (AutoCalculateLeft)
        {
          CalculateLeft(0);
        }
      }

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      public event IndexRangeEventHandler ColumnsAdded;

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
      protected virtual void OnColumnsAdded(IndexRangeEventArgs e)
      {
        if (ColumnsAdded != null)
        {
          ColumnsAdded(this, e);
        }
      }

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      public event IndexRangeEventHandler ColumnsRemoved;

      /// <summary>
      /// Fired when the number of columns change
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
      protected virtual void OnColumnsRemoved(IndexRangeEventArgs e)
      {
        if (ColumnsRemoved != null)
        {
          ColumnsRemoved(this, e);
        }
      }

      /// <summary>
      /// Gets the <see cref="Fr.Medit.MedDataGrid.ColumnInfo"/> instance for the specified position.
      /// </summary>
      /// <param name="index">The index.</param>
      /// <value>
      /// The <see cref="Fr.Medit.MedDataGrid.ColumnInfo"/> instance for the specified position.
      /// </value>
      /// <returns>
      /// The <see cref="Fr.Medit.MedDataGrid.ColumnInfo"/> instance for the specified position.
      /// </returns>
      public ColumnInfo this[int index]
      {
        get { return (ColumnInfo)this.columnInfoList[index]; }
      }

      /// <summary>
      /// Recalculate all the Left positions from the specified index
      /// </summary>
      /// <param name="startIndex">Start index.</param>
      public void CalculateLeft(int startIndex)
      {
        if (Count > 0)
        {
          int l_CurrentLeft = 0;
          if (startIndex != 0)
          {
            l_CurrentLeft = this[startIndex - 1].Left + this[startIndex - 1].Width;
          }

          for (int c = startIndex; c < Count; c++)
          {
            this[c].left = l_CurrentLeft;
            l_CurrentLeft += this[c].width;
          }
        }
      }

      /// <summary>
      /// Gets the right-most column.
      /// </summary>
      /// <remarks>
      /// Returns the maximum right value of the columns. Calculated with Columns[lastCol].Right or 0 if no columns are presents.
      /// </remarks>
      /// <value>The right value.</value>
      public int Right
      {
        get
        {
          if (Count <= 0)
          {
            return 0;
          }
          else
          {
            return this[Count - 1].Right;
          }
        }
      }

      /// <summary>
      /// Gets the right-most column.
      /// </summary>
      /// <value>The left.</value>
      /// <remarks>
      /// Returns the minimum left value of the columns. Calculated with Columns[0].Left or 0 if no columns are presents.
      /// </remarks>
      public int Left
      {
        get
        {
          if (Count <= 0)
          {
            return 0;
          }
          else
          {
            return this[0].Left;
          }
        }
      }

      /// <summary>
      /// Fired when the user change the Width property of one of the Column
      /// </summary>
      public event ColumnInfoEventHandler ColumnWidthChanged;

      /// <summary>
      /// Execute the RowHeightChanged event
      /// </summary>
      /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ColumnInfoEventArgs"/> instance containing the event data.</param>
      public void InvokeColumnWidthChanged(ColumnInfoEventArgs e)
      {
        if (AutoCalculateLeft)
        {
          CalculateLeft(e.Column.Index);
        }

        if (ColumnWidthChanged != null)
        {
          ColumnWidthChanged(this, e);
        }
      }

      /// <summary>
      /// Gets or sets a value indicating whether auto recalculate left position when width value change. Default = true. Can be used when you need to change many Width value for example for an AutoSize operation to increase performance.
      /// </summary>
      /// <value><c>true</c> if set to auto calculate left position; otherwise, <c>false</c>.</value>
      public bool AutoCalculateLeft
      {
        get
        {
          return doAutoCalculateLeft;
        }
        set
        {
          doAutoCalculateLeft = value;
          if (doAutoCalculateLeft)
          {
            CalculateLeft(0);
          }
        }
      }

      public int IndexOf(ColumnInfo info)
      {
        return columnInfoList.IndexOf(info);
      }

      #region ICollection<ColumnInfo>
      /// <summary>
      /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
      /// </summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in array at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
      /// <exception cref="T:System.ArgumentException">array is multidimensional. -or- index is equal to or greater than the length of array. -or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
      /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> can not be cast automatically to the type of the destination array. </exception>
      public virtual void CopyTo(ColumnInfo[] array, System.Int32 index)
      {
        columnInfoList.CopyTo(array, index);
      }

      /// <summary>
      /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
      /// </summary>
      /// <value>Count</value>
      /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
      public int Count
      {
        get { return this.columnInfoList.Count; }
      }

      /// <summary>
      /// Adds the specified column.
      /// </summary>
      /// <param name="column">The column.</param>
      public virtual void Add(ColumnInfo column)
      {
        columnInfoList.Add(column);
      }

      /// <summary>
      /// Determines whether the collection contains the specified column.
      /// </summary>
      /// <param name="column">The column.</param>
      /// <returns>
      /// <c>true</c> if contains the specified column; otherwise, <c>false</c>.
      /// </returns>
      public virtual bool Contains(ColumnInfo column)
      {
        return columnInfoList.Contains(column);
      }

      /// <summary>
      /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
      /// </summary>
      /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
      public virtual void Clear()
      {
        columnInfoList.Clear();
      }

      /// <summary>
      /// Removes the specified column.
      /// </summary>
      /// <param name="column">The column.</param>
      /// <returns></returns>
      public virtual bool Remove(ColumnInfo column)
      {
        return this.columnInfoList.Remove(column);
      }

      /// <summary>
      /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
      /// </summary>
      /// <value>Always false</value>
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
      public IEnumerator<ColumnInfo> GetEnumerator()
      {
        return this.columnInfoList.GetEnumerator();
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