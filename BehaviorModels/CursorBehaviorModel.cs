#region MIT License
//
// Filename: CursorBehaviorModel.cs
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

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// Allow to customize the cursor of a cell.
  /// </summary>
  /// <remarks>
  /// The cell must also implement ICellCursor. This behavior can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class CursorBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default Constructor for the grid cursor
    /// </summary>
    public static readonly CursorBehaviorModel Default = new CursorBehaviorModel();

    #region IBehaviorModel Members
    /// <summary>
    /// Handles the mouse enter event
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnMouseEnter(PositionEventArgs e)
    {
      base.OnMouseEnter(e);

      ApplyCursor(e);
    }

    /// <summary>
    /// Handles the mouse leave event
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnMouseLeave(PositionEventArgs e)
    {
      base.OnMouseLeave(e);

      ResetCursor(e);
    }
    #endregion

    /// <summary>
    /// Change the cursor with the cursor of the cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public virtual void ApplyCursor(PositionEventArgs e)
    {
      if (e.Cell is ICellCursor)
      {
        ICellCursor l_CellCursor = (ICellCursor)e.Cell;

        System.Windows.Forms.Cursor l_Cursor = l_CellCursor.GetCursor(e.Position);
        if (l_Cursor != null)
        {
          e.Grid.GridCursor = l_Cursor;
        }
        else
        {
          e.Grid.GridCursor = System.Windows.Forms.Cursors.Default;
        }
      }
    }

    /// <summary>
    /// Reset the original cursor
    /// </summary>
    /// <param name="e">The cell position</param>
    public virtual void ResetCursor(PositionEventArgs e)
    {
      if (e.Cell is ICellCursor)
      {
        e.Grid.GridCursor = System.Windows.Forms.Cursors.Default;
      }
    }
  }
}