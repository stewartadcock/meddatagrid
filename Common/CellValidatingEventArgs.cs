#region MIT License
//
// Filename: CellValidatingEventHandler.cs
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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  [ComVisible(false)]
  public delegate void CellValidatingEventHandler(object sender, CellValidatingEventArgs e);

  [ComVisible(false)]
  public delegate void CellValidatedEventHandler(object sender, CellValidatedEventArgs e);

  /// <summary>
  /// EventArgs class for cell validating events.
  /// </summary>
  [ComVisible(false)]
  public class CellValidatingEventArgs : CancelEventArgs
  {
    private Cells.ICellVirtual cell;
    private object newValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="CellValidatingEventArgs"/> class.
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <param name="newValue">The new value.</param>
    public CellValidatingEventArgs(Cells.ICellVirtual cell, object newValue)
      : base(false)
    {
      this.cell = cell;
      this.newValue = newValue;
    }

    /// <summary>
    /// Gets or sets the new value.
    /// </summary>
    /// <value>The new value.</value>
    public object NewValue
    {
      get { return this.newValue; }
      set { this.newValue = value; }
    }

    /// <summary>
    /// Gets the cell.
    /// </summary>
    /// <value>The cell.</value>
    public Cells.ICellVirtual Cell
    {
      get { return this.cell; }
    }
  }

  /// <summary>
  /// EventArgs class for cell validated events.
  /// </summary>
  [ComVisible(false)]
  public class CellValidatedEventArgs : EventArgs
  {
    private Cells.ICellVirtual cell;

    /// <summary>
    /// Initializes a new instance of the <see cref="CellValidatedEventArgs"/> class.
    /// </summary>
    /// <param name="cell">The cell.</param>
    public CellValidatedEventArgs(Cells.ICellVirtual cell)
    {
      this.cell = cell;
    }

    /// <summary>
    /// Gets or sets the cell.
    /// </summary>
    /// <value>The cell.</value>
    public Cells.ICellVirtual Cell
    {
      get { return this.cell; }
      set { this.cell = value; }
    }
  }
}