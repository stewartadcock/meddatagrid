#region MIT License
//
// Filename: PositionEventArgs.cs
//
// Copyright © 2011-2013 Felix Concordia SARL. All rights reserved.
// Felix Concordia SARL, 400 avenue Roumanille, Bat 7 - BP 309, 06906 Sophia-Antipolis Cedex, FRANCE.
// 
// Copyright © 2005-2011 MEDIT S.A. All rights reserved.
// MEDIT S.A., 2 rue du Belvedere, 91120 Palaiseau, FRANCE.
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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid
{
  [ComVisible(false)]
  public class PositionEventArgs : EventArgs
  {
    private Position position;
    private Cells.ICellVirtual cell;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_Cell">The cell.</param>
    public PositionEventArgs(Position position, Cells.ICellVirtual p_Cell)
    {
      this.position = position;
      this.cell = p_Cell;
    }

    /// <summary>
    /// Gets the grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.cell.Grid; }
    }

    /// <summary>
    /// Gets or sets the cell.
    /// </summary>
    /// <value>The cell.</value>
    public Cells.ICellVirtual Cell
    {
      get { return this.cell; }
      set { this.cell = value; } // this set method is used for GettingCellEvent
    }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <value>The position.</value>
    public Position Position
    {
      get { return this.position; }
    }
  }

  [ComVisible(false)]
  public delegate void PositionEventHandler(object sender, PositionEventArgs e);

  [ComVisible(false)]
  public class PositionMouseEventArgs : PositionEventArgs
  {
    private System.Windows.Forms.MouseEventArgs mouseArgs;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionMouseEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_MouseArgs">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    public PositionMouseEventArgs(Position position, Cells.ICellVirtual p_Cell, MouseEventArgs p_MouseArgs)
      : base(position, p_Cell)
    {
      this.mouseArgs = p_MouseArgs;
    }

    /// <summary>
    /// Gets or sets the mouse event args.
    /// </summary>
    /// <value>The mouse event args.</value>
    public MouseEventArgs MouseEventArgs
    {
      get { return this.mouseArgs; }
      set { this.mouseArgs = value; }
    }
  }

  [ComVisible(false)]
  public delegate void PositionMouseEventHandler(object sender, PositionMouseEventArgs e);

  [ComVisible(false)]
  public class PositionContextMenuEventArgs : PositionEventArgs
  {
    private List<MenuItem> contextMenu;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionContextMenuEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_ContextMenu">The context menu itmes.</param>
    public PositionContextMenuEventArgs(Position position, Cells.ICellVirtual p_Cell, List<MenuItem> p_ContextMenu)
      : base(position, p_Cell)
    {
      this.contextMenu = p_ContextMenu;
    }

    /// <summary>
    /// Gets or sets the context menu.
    /// </summary>
    /// <value>The context menu.</value>
    public List<MenuItem> ContextMenu
    {
      get { return this.contextMenu; }
      set { this.contextMenu = value; }
    }
  }

  [ComVisible(false)]
  public delegate void PositionContextMenuEventHandler(object sender, PositionContextMenuEventArgs e);

  [ComVisible(false)]
  public class PositionKeyPressEventArgs : PositionEventArgs
  {
    private System.Windows.Forms.KeyPressEventArgs keyPressArgs;
    private bool isControlPressed;
    private bool isShiftPressed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionKeyPressEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_KeyPressArge">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
    public PositionKeyPressEventArgs(Position position, Cells.ICellVirtual p_Cell, System.Windows.Forms.KeyPressEventArgs p_KeyPressArge)
      : base(position, p_Cell)
    {
      this.keyPressArgs = p_KeyPressArge;
      this.isControlPressed = Control.ModifierKeys == Keys.Control;
      this.isShiftPressed = Control.ModifierKeys == Keys.Shift;
    }

    /// <summary>
    /// Gets or sets the key press event args.
    /// </summary>
    /// <value>The key press event args.</value>
    public System.Windows.Forms.KeyPressEventArgs KeyPressEventArgs
    {
      get { return this.keyPressArgs; }
      set { this.keyPressArgs = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is control pressed.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is control pressed; otherwise, <c>false</c>.
    /// </value>
    public bool IsControlPressed
    {
      get { return this.isControlPressed; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is shift pressed.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is shift pressed; otherwise, <c>false</c>.
    /// </value>
    public bool IsShiftPressed
    {
      get { return this.isShiftPressed; }
    }
  }

  [ComVisible(false)]
  public delegate void PositionKeyPressEventHandler(object sender, PositionKeyPressEventArgs e);
}