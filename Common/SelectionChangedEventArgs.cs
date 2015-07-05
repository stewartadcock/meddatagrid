#region MIT License
//
// Filename: SelectionChangedEventArgs.cs
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
  /// EventArgs class for selection changed event.
  /// </summary>
  [ComVisible(false)]
  public class SelectionChangedEventArgs : EventArgs
  {
    private Range range;
    private SelectionChangedEventType eventType;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionChangedEventArgs"/> class.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="range">The range.</param>
    public SelectionChangedEventArgs(SelectionChangedEventType eventType, Range range)
    {
      this.eventType = eventType;
      this.range = range;
    }

    /// <summary>
    /// Gets the range.
    /// </summary>
    /// <value>The range.</value>
    public Range Range
    {
      get { return this.range; }
    }

    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    /// <value>The type of the event.</value>
    public SelectionChangedEventType EventType
    {
      get { return this.eventType; }
    }
  }

  [ComVisible(false)]
  public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}