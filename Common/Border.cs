#region MIT License
//
// Filename: Border.cs
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

using System.Drawing;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A class that represents a single border line.
  /// </summary>
  /// <remarks>
  /// If you have two adjacent cells and want to create a 1 pixel width border, you must set width 1 for
  /// one cell and width 0 for the other. Usually a cell has only Right and Bottom border.
  /// </remarks>
  [ComVisible(false)]
  public struct Border
  {
    private int width;
    private Color color;

    /// <summary>
    /// Initializes a new instance of the <see cref="Border"/> class.
    /// This allows the color to be specified, but the width takes the default value of 1 pixel.
    /// </summary>
    /// <param name="p_Color">Color of the border.</param>
    public Border(Color p_Color)
      : this(p_Color, 1)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Border"/> class.
    /// </summary>
    /// <param name="p_Color">Color of the border.</param>
    /// <param name="p_Width">Width of the border.</param>
    public Border(Color p_Color, int p_Width)
    {
      width = p_Width;
      color = p_Color;
    }

    /// <summary>
    /// Gets or sets the line width
    /// </summary>
    /// <value>The width.</value>
    public int Width
    {
      get { return this.width; }
      set { this.width = value; }
    }

    /// <summary>
    /// Gets or sets the color
    /// </summary>
    /// <value>The color.</value>
    public Color Color
    {
      get { return this.color; }
      set { this.color = value; }
    }

    /// <summary>
    /// Returns the human readable representation of this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> containing a fully qualified type name.
    /// </returns>
    public override string ToString()
    {
      return this.color.ToString() + ", Width= " + this.width.ToString();
    }

    /// <summary>
    /// Compare to current border with another border.
    /// </summary>
    /// <param name="obj">Another object to compare to.</param>
    /// <returns>
    /// <c>true</c> if obj and this instance are the same type and represent the same value; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }
      if (obj.GetType() != GetType())
      {
        return false;
      }
      Border l_Other = (Border)obj;
      return l_Other.Width == this.width && l_Other.Color == this.color;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode()
    {
      return this.color.GetHashCode();
    }

    /// <summary>
    /// operator for testing the equality of two Borders
    /// </summary>
    /// <param name="a">border a</param>
    /// <param name="b">border b</param>
    /// <returns></returns>
    public static bool operator ==(Border a, Border b)
    {
      if ((object)a == (object)b)
      {
        return true;
      }
      if ((object)a == null || (object)b == null)
      {
        return false;
      }
      return a.Equals(b);
    }

    /// <summary>
    /// operator for testing the inequality of two borders
    /// </summary>
    /// <param name="a">border a</param>
    /// <param name="b">border b</param>
    /// <returns></returns>
    public static bool operator !=(Border a, Border b)
    {
      return a.Equals(b) == false;
    }
  }
}