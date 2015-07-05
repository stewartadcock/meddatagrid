#region MIT License
//
// Filename: PositionCancelEventArgs.cs
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

using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  [ComVisible(false)]
  public class PositionCancelEventArgs : PositionEventArgs
  {
    private bool doCancel = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionCancelEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="cell">The cell.</param>
    public PositionCancelEventArgs(Position position, Cells.ICellVirtual cell)
      : base(position, cell)
    {
      // Do nothing.
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PositionCancelEventArgs"/> is
    /// cancelled.
    /// </summary>
    /// <value><c>true</c> if cancelled; otherwise, <c>false</c>.</value>
    public bool Cancel
    {
      get { return this.doCancel; }
      set { this.doCancel = value; }
    }
  }

  [ComVisible(false)]
  public delegate void PositionCancelEventHandler(object sender, PositionCancelEventArgs e);
}