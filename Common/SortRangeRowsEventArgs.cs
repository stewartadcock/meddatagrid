#region MIT License
//
// Filename: SortRangeRowsEventArgs.cs
//
// Copyright © 2011-2013 Felix Concordia SARL. All rights reserved.
// Felix Concordia SARL, 400 avenue Roumanille, Bat 7 - BP 309, 06906 Sophia-Antipolis Cedex, FRANCE.
// 
// Copyright © 2005-2011 MEDIT S.A. All rights reserved.
// MEDIT S.A., 2 rue du Belvedere, 91120 Palaiseau, FRANCE.
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// EventArgs class for SortRangeRows events.
  /// </summary>
  [ComVisible(false)]
  public class SortRangeRowsEventArgs : EventArgs
  {
    private Range range;
    private int absoluteColumnIds;
    private bool isAscending;
    private IComparer cellComparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortRangeRowsEventArgs"/> class.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <param name="absoluteColumnIds">The absolute column keys.</param>
    /// <param name="isAscending">if set to <c>true</c> sort order is ascending.</param>
    /// <param name="cellComparer">The cell comparer.</param>
    public SortRangeRowsEventArgs(Range range,
      int absoluteColumnIds,
      bool isAscending,
      IComparer cellComparer)
    {
      this.range = range;
      this.absoluteColumnIds = absoluteColumnIds;
      this.isAscending = isAscending;
      this.cellComparer = cellComparer;
    }

    /// <summary>
    /// Gets the range.
    /// </summary>
    /// <value>The range.</value>
    public Range Range
    {
      get { return this.range; }
    }

    /// <summary>
    /// Gets the absolute column keys.
    /// </summary>
    /// <value>The absolute column keys.</value>
    public int AbsoluteColumnIndexes
    {
      get { return this.absoluteColumnIds; }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="SortRangeRowsEventArgs"/> is for
    /// a sort in ascending order.
    /// </summary>
    /// <value><c>true</c> if ascending order; otherwise, <c>false</c>.</value>
    public bool Ascending
    {
      get { return this.isAscending; }
    }

    /// <summary>
    /// Gets the cell comparer.
    /// </summary>
    /// <value>The cell comparer.</value>
    public IComparer CellComparer
    {
      get { return this.cellComparer; }
    }
  }

  [ComVisible(false)]
  public delegate void SortRangeRowsEventHandler(object sender, SortRangeRowsEventArgs e);
}