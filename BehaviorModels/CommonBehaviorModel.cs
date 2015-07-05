#region MIT License
//
// Filename: CommonBehaviorModel.cs
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
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// Common behavior of the cell.
  /// </summary>
  /// <remarks>
  /// This behavior can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class CommonBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// The default behavior of a cell.
    /// </summary>
    public static readonly CommonBehaviorModel Default = new CommonBehaviorModel();

    /// <summary>
    /// Handles the key down event on the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnKeyDown(PositionKeyEventArgs e)
    {
      base.OnKeyDown(e);

      if (e.KeyEventArgs.KeyCode == Keys.F2 && e.Cell.DataModel != null && ((e.Cell.DataModel.EditableMode & EditableModes.F2Key) == EditableModes.F2Key))
      {
        e.Cell.StartEdit(e.Position, null);
      }
    }

    /// <summary>
    /// Handles the key press event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnKeyPress(PositionKeyPressEventArgs e)
    {
      base.OnKeyPress(e);

      if (e.Cell.DataModel != null && (e.Cell.DataModel.EditableMode & EditableModes.AnyKey) == EditableModes.AnyKey && e.Cell.IsEditing(e.Position) == false)
      {
        e.Cell.StartEdit(e.Position, e.KeyPressEventArgs.KeyChar.ToString());
      }
    }

    /// <summary>
    /// Handles the double click event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnDoubleClick(PositionEventArgs e)
    {
      base.OnDoubleClick(e);

      if (e.Cell.DataModel != null && (e.Cell.DataModel.EditableMode & EditableModes.DoubleClick) == EditableModes.DoubleClick)
      {
        e.Cell.StartEdit(e.Position, null);
      }
    }

    /// <summary>
    /// Handles the mouse click event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);

      if (e.Cell.DataModel != null && (e.Cell.DataModel.EditableMode & EditableModes.SingleClick) == EditableModes.SingleClick)
      {
        e.Cell.StartEdit(e.Position, null);
      }
    }

    /// <summary>
    /// Handles the focus entering event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnFocusEntering(PositionCancelEventArgs e)
    {
      Cells.Real.Cell cell = e.Cell as Cells.Real.Cell;
      if (cell != null && cell.ExpandedCell == true)
      {
        e.Cancel = true;
      }
    }

    /// <summary>
    /// Handles the focus entered event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnFocusEntered(PositionEventArgs e)
    {
      base.OnFocusEntered(e);

      e.Grid.Selection.Add(e.Position);

      // selectionne la cellule
      e.Grid.ShowCell(e.Position);

      // Gestion Edit
      if (e.Cell.DataModel != null && (e.Cell.DataModel.EditableMode & EditableModes.Focus) == EditableModes.Focus)
      {
        e.Cell.StartEdit(e.Position, null);
      }
    }

    /// <summary>
    /// Handles the focus left event in the specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnFocusLeft(PositionEventArgs e)
    {
      base.OnFocusLeft(e);

      if (e.Grid != null)
      {
        e.Grid.InvalidateCell(e.Position);
      }
    }

    /// <summary>
    /// Fires the value changed event of a specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    /// <remarks>
    /// Raised when the SetValue method is called.
    /// </remarks>
    public override void OnValueChanged(PositionEventArgs e)
    {
      base.OnValueChanged(e);

      if (e.Grid != null)
      {
        e.Grid.InvalidateCell(e.Position);
      }
    }
  }
}