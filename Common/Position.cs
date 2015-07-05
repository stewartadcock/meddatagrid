#region MIT License
//
// Filename: Position.cs
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
  /// Represents a cell position (Row, Col). Once created can not be modified
  /// </summary>
  [ComVisible(false)]
  public struct Position
  {
    #region Constants
    /// <summary>
    /// Empty position
    /// </summary>
    private static readonly Position empty = new Position(EmptyIndexValue, EmptyIndexValue);

    /// <summary>
    /// An empty index constant
    /// </summary>
    private const int EmptyIndexValue = -1;
    #endregion

    #region Class variables
    private readonly int row;
    private readonly int column;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="col">The column.</param>
    public Position(int row, int col)
    {
      this.row = row;
      this.column = col;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the empty position.
    /// </summary>
    /// <value>The empty.</value>
    public static Position Empty
    {
      get { return Position.empty; }
    }

    /// <summary>
    /// Gets the empty index.
    /// </summary>
    /// <remarks>
    /// This is a constant.
    /// </remarks>
    /// <value>The empty index.</value>
    public static int EmptyIndex
    {
      get { return Position.EmptyIndexValue; }
    }

    /// <summary>
    /// Gets the row
    /// </summary>
    /// <value>The row.</value>
    public int Row
    {
      get { return this.row; }
    }

    /// <summary>
    /// Gets the column
    /// </summary>
    /// <value>The column.</value>
    public int Column
    {
      get { return this.column; }
    }
    #endregion

    /// <summary>
    /// Returns true if the current struct is empty
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
      return this.Equals(empty);
    }

    /// <summary>
    /// GetHashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      return Row;
    }

    /// <summary>
    /// Whether the position.s are equal.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public bool Equals(Position position)
    {
      return column == position.column && row == position.row;
    }

    /// <summary>
    /// Whether the position.s are equal.
    /// </summary>
    /// <param name="obj">The position.</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      return Equals((Position)obj);
    }

    /// <summary>
    /// == operator
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Position left, Position right)
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
    public static bool operator !=(Position left, Position right)
    {
      return !left.Equals(right);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return Row.ToString() + ";" + Column.ToString();
    }

    /// <summary>
    /// Returns a position with the smaller Row and the smaller column
    /// </summary>
    /// <param name="position1">The position1.</param>
    /// <param name="position2">The position2.</param>
    /// <returns></returns>
    public static Position MergeMinor(Position position1, Position position2)
    {
      int l_Row, l_Col;
      if (position1.Row < position2.Row)
      {
        l_Row = position1.Row;
      }
      else
      {
        l_Row = position2.Row;
      }
      if (position1.Column < position2.Column)
      {
        l_Col = position1.Column;
      }
      else
      {
        l_Col = position2.Column;
      }
      return new Position(l_Row, l_Col);
    }

    /// <summary>
    /// Returns a position with the bigger Row and the bigger column
    /// </summary>
    /// <param name="position1">The position1.</param>
    /// <param name="position2">The position2.</param>
    /// <returns></returns>
    public static Position MergeMajor(Position position1, Position position2)
    {
      int l_Row, l_Col;
      if (position1.Row > position2.Row)
      {
        l_Row = position1.Row;
      }
      else
      {
        l_Row = position2.Row;
      }
      if (position1.Column > position2.Column)
      {
        l_Col = position1.Column;
      }
      else
      {
        l_Col = position2.Column;
      }
      return new Position(l_Row, l_Col);
    }
  }
}