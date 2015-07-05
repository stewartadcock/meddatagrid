#region MIT License
//
// Filename: GridContextMenu.cs
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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// A class derived from ContextMenu but that is syncronized with the grid using the ContextMenuStyle property
  /// </summary>
  [System.ComponentModel.ToolboxItem(false)]
  public class GridContextMenu : ContextMenu
  {
    private GridVirtual grid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GridContextMenu"/> class.
    /// </summary>
    /// <param name="p_Grid">The grid to which the menu is attached</param>
    public GridContextMenu(GridVirtual p_Grid)
    {
      this.grid = p_Grid;
    }

    /// <summary>
    /// Gets the attached grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.grid; }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.ContextMenu.Popup"></see> event
    /// </summary>
    /// <remarks>
    /// Raised when the contextmenu is shown
    /// </remarks>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnPopup(EventArgs e)
    {
      this.MenuItems.Clear();

      base.OnPopup(e);

      List<MenuItem> l_Menus = grid.GetGridContextMenus();
      for (int i = 0; i < l_Menus.Count; i++)
      {
        MenuItems.Add(l_Menus[i]);
      }
    }
  }
}