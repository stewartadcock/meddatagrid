#region MIT License
//
// Filename: ContextMenuBehaviorModel.cs
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
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// Support customised context-sensitive menus on a cell.
  /// </summary>
  /// <remarks>
  /// This class read the contextmenu from the ICellContextMenu.GetContextMenu.
  /// This behavior can be shared between multiple cells.
  /// </remarks>
  [ComVisible(false)]
  public class ContextMenuBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default ContextMenu
    /// </summary>
    public static readonly ContextMenuBehaviorModel Default = new ContextMenuBehaviorModel();

    #region IBehaviorModel Members
    /// <summary>
    /// Handles the context menu popup
    /// </summary>
    /// <param name="e">The cell position</param>
    public override void OnContextMenuPopUp(PositionContextMenuEventArgs e)
    {
      base.OnContextMenuPopUp(e);
      if (e.Cell is ICellContextMenu)
      {
        ICellContextMenu l_ContextMenu = (ICellContextMenu)e.Cell;
        List<MenuItem> l_Menus = l_ContextMenu.GetContextMenu(e.Position);
        if (l_Menus != null && l_Menus.Count > 0)
        {
          if (e.ContextMenu.Count > 0)
          {
            System.Windows.Forms.MenuItem l_menuBreak = new System.Windows.Forms.MenuItem("-");
            e.ContextMenu.Add(l_menuBreak);
          }

          foreach (System.Windows.Forms.MenuItem m in l_Menus)
          {
            e.ContextMenu.Add(m);
          }
        }
      }
    }
    #endregion
  }
}