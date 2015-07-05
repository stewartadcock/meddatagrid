#region MIT License
//
// Filename: CheckBoxBehaviorModel.cs
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
  /// BehaviorModel for CheckBox cells.
  /// </summary>
  /// <remarks>
  /// This behaviour can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class CheckBoxBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default behaviour checkbox
    /// </summary>
    public static readonly CheckBoxBehaviorModel Default = new CheckBoxBehaviorModel();

    private bool doAutoChangeValueOfSelectedCells = false;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxBehaviorModel"/> class.
    /// </summary>
    public CheckBoxBehaviorModel()
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxBehaviorModel"/> class.
    /// </summary>
    /// <param name="doAutoChangeValueOfSelectedCells">if set to <c>true</c> auto change value of selected cells.</param>
    public CheckBoxBehaviorModel(bool doAutoChangeValueOfSelectedCells)
    {
      this.doAutoChangeValueOfSelectedCells = doAutoChangeValueOfSelectedCells;
    }
    #endregion

    #region IBehaviorModel Members
    /// <summary>
    /// Handles the mouse enter event in the current cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnMouseEnter(PositionEventArgs e)
    {
      base.OnMouseEnter(e);

      e.Cell.Invalidate(e.Position); // Invalidate the cell to refresh the checkbox
    }

    /// <summary>
    /// Handles the mouse leave event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnMouseLeave(PositionEventArgs e)
    {
      base.OnMouseLeave(e);

      e.Cell.Invalidate(e.Position); // Invalidate the cell to refresh the checkbox
    }

    /// <summary>
    /// Handles the key press event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyPressEventArgs"/> instance containing the event data.</param>
    public override void OnKeyPress(PositionKeyPressEventArgs e)
    {
      base.OnKeyPress(e);

      if (e.KeyPressEventArgs.KeyChar == ' ')
      {
        UIChangeChecked(e);
      }
    }

    /// <summary>
    /// Handles the click event on the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);

      UIChangeChecked(e);
    }
    #endregion

    /// <summary>
    /// Gets a value indicating whether if this cells when checked or uncheck must change also the value of the selected cells of type CellCheckBox
    /// </summary>
    /// <value>
    ///   <c>true</c> if set to auto change value of selected cells; otherwise, <c>false</c>.
    /// </value>
    public bool AutoChangeValueOfSelectedCells
    {
      get { return this.doAutoChangeValueOfSelectedCells; }
    }

    /// <summary>
    /// Toggle the value of the current cell and if AutoChangeValueOfSelectedCells is true of all the selected cells
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    private void UIChangeChecked(PositionEventArgs e)
    {
      ICellCheckBox l_Check = (ICellCheckBox)e.Cell;
      if (l_Check.GetCheckBoxStatus(e.Position).CheckEnable)
      {
        bool l_NewVal = !l_Check.GetCheckedValue(e.Position);
        l_Check.SetCheckedValue(e.Position, l_NewVal);

        // change the status of all selected controls
        if (AutoChangeValueOfSelectedCells == true)
        {
          foreach (Position pos in e.Cell.Grid.Selection.GetCellsPositions())
          {
            Cells.ICellVirtual c = e.Grid.GetCell(pos);
            if (c != null && c is ICellCheckBox && (ICellCheckBox)c != l_Check)
            {
              ICellCheckBox cCh = (ICellCheckBox)c;
              cCh.SetCheckedValue(pos, l_NewVal);
            }
          }
        }
      }
    }
  }
}