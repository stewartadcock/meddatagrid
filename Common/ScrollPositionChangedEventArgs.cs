#region MIT License
//
// Filename: ScrollPositionChangedEventArgs.cs
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Event args for ScrollPositionChanged events.
  /// </summary>
  [ComVisible(false)]
  public class ScrollPositionChangedEventArgs : EventArgs
  {
    private int newValue;
    private int oldValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScrollPositionChangedEventArgs"/> class.
    /// </summary>
    /// <param name="newValue">The new value.</param>
    /// <param name="oldValue">The old value.</param>
    public ScrollPositionChangedEventArgs(int newValue, int oldValue)
    {
      this.newValue = newValue;
      this.oldValue = oldValue;
    }

    /// <summary>
    /// Gets the new value.
    /// </summary>
    /// <value>The new value.</value>
    public int NewValue
    {
      get { return this.newValue; }
    }

    /// <summary>
    /// Gets the old value.
    /// </summary>
    /// <value>The old value.</value>
    public int OldValue
    {
      get { return this.oldValue; }
    }

    /// <summary>
    /// Gets the delta.
    /// </summary>
    /// <value>The delta.</value>
    public int Delta
    {
      get { return this.oldValue - this.newValue; }
    }
  }

  [ComVisible(false)]
  public delegate void ScrollPositionChangedEventHandler(object sender, ScrollPositionChangedEventArgs e);
}