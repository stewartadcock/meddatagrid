#region MIT License
//
// Filename: ExpandCellBehaviorModel.cs
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
  /// The ExpandCell class, for controlling the expansion state of collapsible cells.
  /// </summary>
  [ComVisible(false)]
  public class ExpandCellBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default Constructor for an expand cell
    /// </summary>
    public static readonly ExpandCellBehaviorModel Default = new ExpandCellBehaviorModel();

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCellBehaviorModel"/> class.
    /// </summary>
    public ExpandCellBehaviorModel()
    {
      // Do nothing.
    }
    #endregion

    #region IBehaviorModel Members
    ////    public override void OnMouseEnter(PositionEventArgs e)
    ////    {
    ////      base.OnMouseEnter(e);
    ////
    ////      e.Cell.Invalidate(e.Position);//Invalidate the cell to refresh the box
    ////    }
    ////
    ////    public override void OnMouseLeave(PositionEventArgs e)
    ////    {
    ////      base.OnMouseLeave(e);
    ////
    ////      e.Cell.Invalidate(e.Position);//Invalidate the cell to refresh the box
    ////    }

    /// <summary>
    /// Handles the focus entering event in the current cell. This type of cell can't receive the focus
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public override void OnFocusEntering(PositionCancelEventArgs e)
    {
      e.Cancel = true;
    }

    /// <summary>
    /// Handles the mouse click event. The cell state changes (true or false)
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);
      ChangeState(e);
    }
    #endregion

    /// <summary>
    /// Toggle the state value of the image cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void ChangeState(PositionEventArgs e)
    {
      e.Grid.Redraw = false;
      IExpandCell l_Cell = (IExpandCell)e.Cell;
      bool l_newState = !l_Cell.GetStateValue(e.Position);

      l_Cell.SetStateValue(e.Position, l_newState);

      foreach (ICell item in l_Cell.GetChildCells())
      {
        if (l_newState == true)
        {
          e.Grid.Rows[item.Row].Height = 0;
        }
        else
        {
          e.Grid.AutoSizeRowRange(item.Row, 10, 0, e.Grid.ColumnsCount - 1);
        }
      }

      e.Grid.Redraw = true;

      //  e.Grid.InvalidateRange(new Range(e.Position.Row, 0, e.Grid.RowsCount-1, e.Grid.ColumnsCount-1));
    }
  }
}