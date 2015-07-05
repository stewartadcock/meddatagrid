#region MIT License
//
// Filename: ColumnHeader.cs
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

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  /// <summary>
  /// Class that represent a column header with sort and resize feature.
  /// </summary>
  [ComVisible(false)]
  public abstract class ColumnHeader : HeaderCell, IHeaderCell, ICellSortableHeader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeader"/> class.
    /// </summary>
    protected ColumnHeader()
      : base(VisualModels.Header.ColumnHeader, BehaviorModel.ColumnHeaderBehaviorModel.SortableHeader)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeader"/> class.
    /// </summary>
    /// <param name="visualModel">The visual model.</param>
    /// <param name="behaviorModel">The behavior model.</param>
    protected ColumnHeader(VisualModels.Header visualModel, BehaviorModel.ColumnHeaderBehaviorModel behaviorModel)
      : base(visualModel, behaviorModel)
    {
      // Do nothing.
    }

    #region ICellSortableHeader Members
    /// <summary>
    /// Gets or sets a value indicating whether to enable sorting.
    /// </summary>
    /// <value><c>true</c> if sorting is enabled; otherwise, <c>false</c>.</value>
    public abstract bool EnableSort
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets comparer used to sort the cells. If null the default comparer is used.
    /// </summary>
    /// <value>The comparer.</value>
    /// <remarks>
    /// this is the comparer used to sort the cells. If null the default comparer is used.
    /// </remarks>
    public abstract System.Collections.IComparer Comparer
    {
      get;
      set;
    }

    /// <summary>
    /// Returns the current sort status
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public abstract SortStatus GetSortStatus(Position position);

    /// <summary>
    /// Set the current sort mode
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="mode">The mode.</param>
    public abstract void SetSortMode(Position position, GridSortMode mode);
    #endregion
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// Compares two objects and returns a value indicating whether one is less than,
  /// equal to, or greater than the other.
  /// </summary>
  public class ComparerLeftPadding : System.Collections.IComparer
  {
    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less than,
    /// equal to, or greater than the other.
    /// compare strings by completting the beginning of the size of the first with spaces " "
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>
    /// Value Condition Less than zero x is less than y. Zero x equals y.
    /// Greater than zero x is greater than y.</returns>
    public int Compare(object x, object y)
    {
      string a = ((Cell)x).Value.ToString();
      string b = ((Cell)y).Value.ToString();
      int lengthDiff = a.Length - b.Length;
      if (lengthDiff > 0) // a.Length > b.Length
      {
        b = b.PadLeft(a.Length, ' ');
      }
      else if (lengthDiff < 0) // a.Length < b.Length
      {
        a = a.PadLeft(b.Length, ' ');
      }
      return System.Collections.Comparer.Default.Compare(a, b);
    }
  }

  /// <summary>
  /// Compares two cells and returns a value indicating whether one is less than,
  /// equal to, or greater than the other.
  /// </summary>
  /// <remarks>
  /// This compares strings that are formatted with a number concatenated with one char or nothing,
  /// with comparison of the numeric part first then of the char.
  /// </remarks>
  /// <example>
  /// <c>ComparerIntAndAChar.Compare</c> will create an order like this:
  /// "11", "11A", "12", "100", "110B"
  /// </example>
  public class ComparerIntAndAChar : System.Collections.IComparer
  {
    /// <summary>
    /// Compares two cells and returns a value indicating whether one is less than,
    /// equal to, or greater than the other.
    /// </summary>
    /// <remarks>
    /// This compares strings that are formatted with a number concatenated with one char or nothing,
    /// with comparison of the numeric part first then of the char.
    /// </remarks>
    /// <example>
    /// <c>ComparerIntAndAChar.Compare</c> will create an order like this:
    /// "11", "11A", "12", "100", "110B"
    /// </example>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>
    /// Value Condition Less than zero x is less than y. Zero x equals y.
    /// Greater than zero x is greater than y.
    /// </returns>
    public int Compare(object x, object y)
    {
      string a = ((Cell)x).Value.ToString();
      string b = ((Cell)y).Value.ToString();
      int tmpInt;
      if (int.TryParse(a, out tmpInt))
      {
        a = a.PadRight(a.Length + 1, ' ');
      }
      if (int.TryParse(b, out tmpInt))
      {
        b = b.PadRight(b.Length + 1, ' ');
      }
      int lengthDiff = a.Length - b.Length;
      if (lengthDiff > 0) // a.Length > b.Length
      {
        b = b.PadLeft(a.Length, ' ');
      }
      else if (lengthDiff < 0) // a.Length < b.Length
      {
        a = a.PadLeft(b.Length, ' ');
      }
      return System.Collections.Comparer.Default.Compare(a, b);
    }
  }

  /// <summary>
  /// Class that represent a column header with sort and resize feature.
  /// </summary>
  [ComVisible(false)]
  public class ColumnHeader : HeaderCell, IHeaderCell, ICellSortableHeader
  {
    private GridSortMode sortMode = GridSortMode.None;
    private bool enableSort = true;
    private System.Collections.IComparer comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeader"/> class.
    /// </summary>
    public ColumnHeader()
      : base(null, VisualModels.Header.ColumnHeader, BehaviorModel.ColumnHeaderBehaviorModel.SortableHeader)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeader"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ColumnHeader(object value)
      : base(value, VisualModels.Header.ColumnHeader, BehaviorModel.ColumnHeaderBehaviorModel.SortableHeader)
    {
      // Do nothing.
    }

    #region ICellSortableHeader Members
    /// <summary>
    /// Gets or sets a value indicating whether to enable sorting.
    /// </summary>
    /// <value><c>true</c> if sorting is enabled; otherwise, <c>false</c>.</value>
    public bool EnableSort
    {
      get { return this.enableSort; }
      set { this.enableSort = value; }
    }

    /// <summary>
    /// Gets or sets comparer used to sort the cells. If null the default comparer is used.
    /// </summary>
    /// <value>The comparer.</value>
    /// <remarks>
    /// this is the comparer used to sort the cells. If null the default comparer is used.
    /// </remarks>
    public System.Collections.IComparer Comparer
    {
      get { return this.comparer; }
      set { this.comparer = value; }
    }

    /// <summary>
    /// Returns the current sort status
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual SortStatus GetSortStatus(Position position)
    {
      return new SortStatus(this.sortMode, this.enableSort, this.comparer);
    }

    /// <summary>
    /// Set the current sort mode
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="mode">The mode.</param>
    public virtual void SetSortMode(Position position, GridSortMode mode)
    {
      this.sortMode = mode;
    }
    #endregion
  }
}