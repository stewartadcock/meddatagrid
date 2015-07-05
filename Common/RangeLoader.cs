#region MIT License
//
// Filename: RangeLoader.cs
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

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Interface that represent a range of the grid. (RangeFullGridNoFixedRows, RangeFullGridNoFixedCols, RangeFixedRows, RangeFixedCols, Range)
  /// </summary>
  [ComVisible(false)]
  public interface IRangeLoader
  {
    /// <summary>
    /// Rectangle that contains the range.
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    Range GetRange(GridVirtual grid);
  }

  /// <summary>
  /// Range custom
  /// </summary>
  [ComVisible(false)]
  public class RangeLoader : IRangeLoader
  {
    private Range gridRange;

    /// <summary>
    /// Initializes a new instance of the <see cref="RangeLoader"/> class.
    /// </summary>
    /// <param name="gridRange">The grid range.</param>
    public RangeLoader(Range gridRange)
    {
      this.gridRange = gridRange;
    }

    /// <summary>
    /// Gets or sets the range.
    /// </summary>
    /// <value>The cell range.</value>
    public Range CellRange
    {
      get { return this.gridRange; }
      set { this.gridRange = value; }
    }

    /// <summary>
    /// Returns the Range from the specified grid.
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public virtual Range GetRange(GridVirtual grid)
    {
      return this.gridRange;
    }
  }

  /// <summary>
  /// Represents a range that contains all the grid with no fixed cols
  /// </summary>
  [ComVisible(false)]
  public class RangeFullGridNoFixedCols : IRangeLoader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeFullGridNoFixedCols"/> class.
    /// </summary>
    public RangeFullGridNoFixedCols()
    {
    }

    /// <summary>
    /// Returns the Range struct from the specific instance
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public Range GetRange(GridVirtual grid)
    {
      if (grid.ColumnsCount >= grid.FixedColumns)
      {
        return new Range(0, grid.FixedColumns, grid.RowsCount - 1, grid.ColumnsCount - 1);
      }
      else
      {
        return Range.Empty;
      }
    }
  }

  /// <summary>
  /// Represents a range that contains only fixed rows
  /// </summary>
  [ComVisible(false)]
  public class RangeFixedRows : IRangeLoader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeFixedRows"/> class.
    /// </summary>
    public RangeFixedRows()
    {
    }

    /// <summary>
    /// Returns the Range struct from the specific instance
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public Range GetRange(GridVirtual grid)
    {
      if (grid.RowsCount >= grid.FixedRows)
      {
        return new Range(0, 0, grid.FixedRows, grid.ColumnsCount - 1);
      }
      else
      {
        return Range.Empty;
      }
    }
  }

  /// <summary>
  /// Represents a range that contains only fixed cols
  /// </summary>
  [ComVisible(false)]
  public class RangeFixedCols : IRangeLoader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeFixedCols"/> class.
    /// </summary>
    public RangeFixedCols()
    {
    }

    /// <summary>
    /// Returns the Range struct from the specific instance
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public Range GetRange(GridVirtual grid)
    {
      if (grid.ColumnsCount >= grid.FixedColumns)
      {
        return new Range(0, 0, grid.RowsCount - 1, grid.FixedColumns);
      }
      else
      {
        return Range.Empty;
      }
    }
  }

  /// <summary>
  /// Represents a range that contains all the grid
  /// </summary>
  [ComVisible(false)]
  public class RangeFullGrid : IRangeLoader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeFullGrid"/> class.
    /// </summary>
    public RangeFullGrid()
    {
    }

    /// <summary>
    /// Returns the Range struct from the specific instance
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public Range GetRange(GridVirtual grid)
    {
      return grid.CompleteRange;
    }
  }

  /// <summary>
  /// Represents a range that contains all the grid with no fixed rows
  /// </summary>
  [ComVisible(false)]
  public class RangeFullGridNoFixedRows : IRangeLoader
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeFullGridNoFixedRows"/> class.
    /// </summary>
    public RangeFullGridNoFixedRows()
    {
    }

    /// <summary>
    /// Returns the Range struct from the specific instance
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <returns></returns>
    public Range GetRange(GridVirtual grid)
    {
      if (grid.RowsCount >= grid.FixedRows)
      {
        return new Range(grid.FixedRows, 0, grid.RowsCount - 1, grid.ColumnsCount - 1);
      }
      else
      {
        return Range.Empty;
      }
    }
  }
}