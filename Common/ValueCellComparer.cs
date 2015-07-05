#region MIT License
//
// Filename: ValueCellComparer.cs
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

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A comparer for the Cell class that uses the value of the cell.
  /// This is NOT for CellVirtual.
  /// </summary>
  public class ValueCellComparer : IComparer
  {
    public virtual System.Int32 Compare(System.Object x, System.Object y)
    {
      // Cell object
      if (x == null && y == null)
      {
        return 0;
      }
      if (x == null)
      {
        return -1;
      }
      if (y == null)
      {
        return 1;
      }

      if (x is IComparable)
      {
        return ((IComparable)x).CompareTo(y);
      }
      if (y is IComparable)
      {
        return -((IComparable)y).CompareTo(x);
      }

      // Cell.Value object
      object vx = ((Cells.Real.Cell)x).Value;
      object vy = ((Cells.Real.Cell)y).Value;

      if (vx == null && vy == null)
      {
        return 0;
      }
      if (vx == null)
      {
        return -1;
      }
      if (vy == null)
      {
        return 1;
      }

      if (vx is IComparable)
      {
        return ((IComparable)vx).CompareTo(vy);
      }
      if (vy is IComparable)
      {
        return -((IComparable)vy).CompareTo(vx);
      }

      throw new ArgumentException("Invalid cell object, no IComparable interface found");
    }
  }
}