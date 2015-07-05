#region MIT License
//
// Filename: toolTipTextBehaviorModel.cs
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
  /// Allow to customize the tooltiptext of a cell.
  /// </summary>
  /// <remarks>
  /// This class reads the tooltiptext from the ICellToolTipText.GetToolTipText. This behavior can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class ToolTipTextBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default <see cref="ToolTipTextBehaviorModel"/>.
    /// </summary>
    public static readonly ToolTipTextBehaviorModel Default = new ToolTipTextBehaviorModel();

    #region IBehaviorModel Members
    /// <summary>
    /// Fires the mouse enter event when the mouse enters in a specified cell
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnMouseEnter(PositionEventArgs e)
    {
      base.OnMouseEnter(e);
      ApplyToolTipText(e);
    }

    /// <summary>
    /// Raised when the mouse leaves a specified cell position
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnMouseLeave(PositionEventArgs e)
    {
      base.OnMouseLeave(e);
      ResetToolTipText(e);
    }
    #endregion

    /// <summary>
    /// Applies the tool tip text.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    protected virtual void ApplyToolTipText(PositionEventArgs e)
    {
      if (e.Cell is ICellToolTipText)
      {
        ICellToolTipText l_CellToolTip = (ICellToolTipText)e.Cell;
        string l_ToolTipText = l_CellToolTip.GetToolTipText(e.Position);
        if (l_ToolTipText != null && l_ToolTipText.Length > 0)
        {
          e.Grid.GridToolTipText = l_ToolTipText;
        }
      }
    }

    /// <summary>
    /// Resets the tool tip text.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    protected virtual void ResetToolTipText(PositionEventArgs e)
    {
      e.Grid.GridToolTipText = null;
    }
  }
}