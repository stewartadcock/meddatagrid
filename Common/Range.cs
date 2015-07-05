#region MIT License
//
// Filename: Range.cs
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

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Represents range of cells. Once created can not be modified.
  /// This Range has always Start in the Top-Left, and End in the Bottom-Right (see Normalize method).
  /// </summary>
  [ComVisible(false)]
  public struct Range
  {
    #region Class variables
    /// <summary>
    /// Represents an empty range
    /// </summary>
    public static readonly Range Empty = new Range(Position.Empty, Position.Empty, false);

    /// <summary>
    /// The start of the range.
    /// </summary>
    private Position start;

    /// <summary>
    /// The end of the range.
    /// </summary>
    private Position end;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Range"/> class.
    /// </summary>
    /// <param name="p_Start">The start.</param>
    /// <param name="p_End">The end.</param>
    public Range(Position p_Start, Position p_End)
      : this(p_Start, p_End, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Range"/> class.
    /// </summary>
    /// <param name="p_StartRow">The start row.</param>
    /// <param name="p_StartCol">The start column.</param>
    /// <param name="p_EndRow">The end row.</param>
    /// <param name="p_EndCol">The end column.</param>
    public Range(int p_StartRow, int p_StartCol, int p_EndRow, int p_EndCol)
    {
      start = new Position(p_StartRow, p_StartCol);
      end = new Position(p_EndRow, p_EndCol);

      Normalize();
    }

    private Range(Position p_Start, Position p_End, bool p_bCheck)
    {
      start = p_Start;
      end = p_End;

      if (p_bCheck)
      {
        Normalize();
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Range"/> class.
    /// Construct a Range of a single cell
    /// </summary>
    /// <param name="p_SinglePosition">The single position.</param>
    public Range(Position p_SinglePosition)
      : this(p_SinglePosition, p_SinglePosition, false)
    {
    }
    #endregion

    /// <summary>
    /// Gets or sets the start of the range.
    /// </summary>
    /// <value>The start.</value>
    public Position Start
    {
      get
      {
        return this.start;
      }
      set
      {
        this.start = value;
        Normalize();
      }
    }

    /// <summary>
    /// Gets or sets the end or the range.
    /// </summary>
    /// <value>The end.</value>
    public Position End
    {
      get
      {
        return this.end;
      }
      set
      {
        this.end = value;
        Normalize();
      }
    }

    /// <summary>
    /// Move the current range to the specified position, leaving the current ColumnsCount and RowsCount
    /// </summary>
    /// <param name="p_StartPosition">The start position.</param>
    public void MoveTo(Position p_StartPosition)
    {
      int l_ColCount = ColumnsCount;
      int l_RowCount = RowsCount;
      this.start = p_StartPosition;
      RowsCount = l_RowCount;
      ColumnsCount = l_ColCount;
    }

    /// <summary>
    /// Gets or sets the columns count (End.Column-Start.Column)
    /// </summary>
    /// <value>The columns count.</value>
    public int ColumnsCount
    {
      get
      {
        return (end.Column - start.Column) + 1;
      }
      set
      {
        if (value <= 0)
        {
          throw new MEDDataGridException("Invalid columns count");
        }
        end = new Position(end.Row, start.Column + value - 1);
      }
    }

    /// <summary>
    /// Gets or sets the rows count (End.Row-Start.Row)
    /// </summary>
    /// <value>The rows count.</value>
    public int RowsCount
    {
      get
      {
        return (end.Row - start.Row) + 1;
      }
      set
      {
        if (value <= 0)
        {
          throw new MEDDataGridException("Invalid columns count");
        }
        end = new Position(start.Row + value - 1, end.Column);
      }
    }

    /// <summary>
    /// Check and fix the range to always have Start smaller than End
    /// </summary>
    private void Normalize()
    {
      int l_MinRow, l_MinCol, l_MaxRow, l_MaxCol;

      if (start.Row < end.Row)
      {
        l_MinRow = start.Row;
      }
      else
      {
        l_MinRow = end.Row;
      }

      if (start.Column < end.Column)
      {
        l_MinCol = start.Column;
      }
      else
      {
        l_MinCol = end.Column;
      }

      if (start.Row > end.Row)
      {
        l_MaxRow = start.Row;
      }
      else
      {
        l_MaxRow = end.Row;
      }

      if (start.Column > end.Column)
      {
        l_MaxCol = start.Column;
      }
      else
      {
        l_MaxCol = end.Column;
      }

      start = new Position(l_MinRow, l_MinCol);
      end = new Position(l_MaxRow, l_MaxCol);
    }

    /// <summary>
    /// Returns true if the specified row is present in the current range.
    /// </summary>
    /// <param name="p_Row">The row.</param>
    /// <returns></returns>
    public bool ContainsRow(int p_Row)
    {
      return p_Row >= start.Row && p_Row <= end.Row;
    }

    /// <summary>
    /// Returns true if the specified column is present in the current range.
    /// </summary>
    /// <param name="p_Col">The column.</param>
    /// <returns>
    /// <c>true</c> if the range contains the specified column; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsColumn(int p_Col)
    {
      return p_Col >= start.Column && p_Col <= end.Column;
    }

    /// <summary>
    /// Returns true if the specified cell position is present in the current range.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    /// <c>true</c> if the range contains the specified position; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(Position position)
    {
      int l_Row = position.Row;
      int l_Col = position.Column;

      return l_Row >= start.Row && l_Row <= end.Row &&
              l_Col >= start.Column && l_Col <= end.Column;
    }

    /// <summary>
    /// Returns true if the specified range is present in the current range.
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <returns></returns>
    public bool Contains(Range p_Range)
    {
      return Contains(p_Range.Start) && Contains(p_Range.End);
    }

    /// <summary>
    /// Determines whether the current range is empty
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
      return Start.IsEmpty() || End.IsEmpty();
    }

    /// <summary>
    /// == operator
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Range left, Range right)
    {
      if ((object)left == (object)right)
      {
        return true;
      }
      if ((object)left == null || (object)right == null)
      {
        return false;
      }
      return left.Equals(right);
    }

    /// <summary>
    /// != operator
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Range left, Range right)
    {
      return !left.Equals(right);
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode()
    {
      return start.Row;
    }

    /// <summary>
    /// Determines whether the ranges are equal.
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <returns></returns>
    public bool Equals(Range p_Range)
    {
      return Start.Equals(p_Range.Start) && End.Equals(p_Range.End);
    }

    /// <summary>
    /// Determines whether the ranges are equal.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      return Equals((Range)obj);
    }

    /// <summary>
    /// Gets the cells positions.
    /// </summary>
    /// <returns></returns>
    public IList<Position> GetCellsPositions()
    {
      List<Position> l_List = new List<Position>();
      for (int r = Start.Row; r <= End.Row; r++)
      {
        for (int c = Start.Column; c <= End.Column; c++)
        {
          l_List.Add(new Position(r, c));
        }
      }

      return (IList<Position>)l_List;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return Start.ToString() + " to " + End.ToString();
    }

    /// <summary>
    /// Returns a range with the smaller Start and the bigger End, the Union of the two ranges.
    /// If one of the range is empty then returns other range.
    /// </summary>
    /// <param name="p_Range1">The range1.</param>
    /// <param name="p_Range2">The range2.</param>
    /// <returns></returns>
    public static Range Union(Range p_Range1, Range p_Range2)
    {
      if (p_Range1.IsEmpty())
      {
        return p_Range2;
      }
      else if (p_Range2.IsEmpty())
      {
        return p_Range1;
      }

      return new Range(Position.MergeMinor(p_Range1.Start, p_Range2.Start),
                Position.MergeMajor(p_Range1.End, p_Range2.End), false);
    }

    /// <summary>
    /// Returns the intersection between the two ranges.
    /// If one of the range is empty then the return is empty.
    /// </summary>
    /// <param name="p_Range1">The range1.</param>
    /// <param name="p_Range2">The range2.</param>
    /// <returns></returns>
    public static Range Intersect(Range p_Range1, Range p_Range2)
    {
      if (p_Range1.IsEmpty() || p_Range2.IsEmpty())
      {
        return Range.Empty;
      }

      return new Range(Position.MergeMinor(p_Range1.Start, p_Range2.Start),
          Position.MergeMinor(p_Range1.End, p_Range2.End), false);
    }
  }
}