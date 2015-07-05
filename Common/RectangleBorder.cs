#region MIT License
//
// Filename: RectangleBorder.cs
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
  /// A class that represents the borders of a cell. Contains 4 borders: Right, Left, Top, Bottom.
  /// </summary>
  /// <remarks>
  /// If you have 2 adjacent cells and want to create a 1 pixel width border, you must set
  /// width 1 for one cell and width 0 for the other. Usually a cell has only Right and Bottom border.
  /// </remarks>
  [ComVisible(false)]
  public class RectangleBorder
  {
    private static readonly RectangleBorder defaultRectangleBorder = new RectangleBorder(new Border(Color.LightGray, 1), new Border(Color.LightGray, 1));
    private static readonly RectangleBorder defaultNoRectangleBorder = new RectangleBorder(new Border(Color.White, 0));

    /// <summary>
    /// Gets the default border.
    /// </summary>
    /// <value>The default.</value>
    public static RectangleBorder Default
    {
      get
      {
        return defaultRectangleBorder;
      }
    }

    /// <summary>
    /// Gets the default no border.
    /// </summary>
    /// <value>The no border.</value>
    public static RectangleBorder NoBorder
    {
      get
      {
        return defaultNoRectangleBorder;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleBorder"/> class.
    /// Construct a RectangleBorder with the same border on all the side
    /// </summary>
    /// <param name="p_Border">The border.</param>
    public RectangleBorder(Border p_Border)
    {
      this.top = p_Border;
      this.bottom = p_Border;
      this.left = p_Border;
      this.right = p_Border;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleBorder"/> class.
    /// Construct a RectangleBorder with the specified Right and Bottom border and a 0 Left and Top border
    /// </summary>
    /// <param name="p_Right">The right border.</param>
    /// <param name="p_Bottom">The bottom border.</param>
    public RectangleBorder(Border p_Right, Border p_Bottom)
    {
      this.right = p_Right;
      this.bottom = p_Bottom;
      this.top = new Border(Color.White, 0);
      this.left = new Border(Color.White, 0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleBorder"/> class.
    /// Construct a RectangleBorder with the specified borders
    /// </summary>
    /// <param name="p_Top">The top border.</param>
    /// <param name="p_Bottom">The bottom border.</param>
    /// <param name="p_Left">The left border.</param>
    /// <param name="p_Right">The right border.</param>
    public RectangleBorder(Border p_Top, Border p_Bottom, Border p_Left, Border p_Right)
    {
      this.top = p_Top;
      this.bottom = p_Bottom;
      this.left = p_Left;
      this.right = p_Right;
    }

    private Border top;
    /// <summary>
    /// Gets or sets the top border.
    /// </summary>
    /// <value>The top border.</value>
    public Border Top
    {
      get { return this.top; }
      set { this.top = value; }
    }

    private Border bottom;
    /// <summary>
    /// Gets or sets the bottom border.
    /// </summary>
    /// <value>The bottom border.</value>
    public Border Bottom
    {
      get { return this.bottom; }
      set { this.bottom = value; }
    }

    private Border left;
    /// <summary>
    /// Gets or sets the left border.
    /// </summary>
    /// <value>The left border.</value>
    public Border Left
    {
      get { return this.left; }
      set { this.left = value; }
    }

    private Border right;
    /// <summary>
    /// Gets or sets the right border.
    /// </summary>
    /// <value>The right border.</value>
    public Border Right
    {
      get { return this.right; }
      set { this.right = value; }
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString()
    {
      return "Top:" + this.top.ToString() + " Bottom:" + this.bottom.ToString() + " Left:" + this.left.ToString() + " Right:" + this.right.ToString();
    }

    /// <summary>
    /// Change the color of the current struct instance and return a copy of the modified struct.
    /// </summary>
    /// <param name="p_Color">Color of the border.</param>
    /// <returns></returns>
    public RectangleBorder SetColor(Color p_Color)
    {
      this.top = new Border(p_Color, Top.Width);
      this.bottom = new Border(p_Color, Bottom.Width);
      this.left = new Border(p_Color, Left.Width);
      this.right = new Border(p_Color, Right.Width);

      return this;
    }

    /// <summary>
    /// Change the width of the current struct instance and return a copy of the modified struct.
    /// </summary>
    /// <param name="width">Width of the border.</param>
    /// <returns></returns>
    public RectangleBorder SetWidth(int width)
    {
      this.top = new Border(Top.Color, width);
      this.bottom = new Border(Bottom.Color, width);
      this.left = new Border(Left.Color, width);
      this.right = new Border(Right.Color, width);

      return this;
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != GetType())
      {
        return false;
      }
      RectangleBorder l_Other = (RectangleBorder)obj;
      return l_Other.Left == this.left && l_Other.Bottom == this.bottom &&
        l_Other.Top == this.top && l_Other.Right == this.right;
    }

    /// <summary>
    /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
    /// </summary>
    /// <returns>
    /// A hash code for the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override int GetHashCode()
    {
      return this.left.GetHashCode();
    }

    /// <summary>
    /// Operator ==.
    /// </summary>
    /// <param name="a">border a</param>
    /// <param name="b">border b</param>
    /// <returns></returns>
    public static bool operator ==(RectangleBorder a, RectangleBorder b)
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
    /// Operator !=.
    /// </summary>
    /// <param name="a">border a</param>
    /// <param name="b">border b</param>
    /// <returns></returns>
    public static bool operator !=(RectangleBorder a, RectangleBorder b)
    {
      return a.Equals(b) == false;
    }

    /// <summary>
    /// Remove the width of all the borders (top, bottom, left, right) from a specified rectangle
    /// </summary>
    /// <param name="p_Input">The input.</param>
    /// <returns></returns>
    public Rectangle RemoveBorderFromRectangle(Rectangle p_Input)
    {
      p_Input.Y += this.top.Width;
      p_Input.X += this.left.Width;
      p_Input.Width -= this.left.Width + this.right.Width;
      p_Input.Height -= this.top.Width + this.bottom.Width;

      return p_Input;
    }

    #region utility
    /// <summary>
    /// Format the border
    /// </summary>
    /// <param name="p_Style">The style.</param>
    /// <param name="p_width">The width.</param>
    /// <param name="p_ShadowColor">Color of the shadow.</param>
    /// <param name="p_LightColor">Color of the light.</param>
    /// <returns></returns>
    public static RectangleBorder FormatBorder(CommonBorderStyle p_Style, int p_width, Color p_ShadowColor, Color p_LightColor)
    {
      RectangleBorder l_Border = new RectangleBorder(new Border(Color.White));
      if (p_Style == CommonBorderStyle.Inset)
      {
        l_Border.Top = new Border(p_ShadowColor, p_width);
        l_Border.Left = new Border(p_ShadowColor, p_width);
        l_Border.Bottom = new Border(p_LightColor, p_width);
        l_Border.Right = new Border(p_LightColor, p_width);
      }
      else if (p_Style == CommonBorderStyle.Raised)
      {
        l_Border.Top = new Border(p_LightColor, p_width);
        l_Border.Left = new Border(p_LightColor, p_width);
        l_Border.Bottom = new Border(p_ShadowColor, p_width);
        l_Border.Right = new Border(p_ShadowColor, p_width);
      }
      else
      {
        l_Border.Top = new Border(p_ShadowColor, p_width);
        l_Border.Left = new Border(p_ShadowColor, p_width);
        l_Border.Bottom = new Border(p_ShadowColor, p_width);
        l_Border.Right = new Border(p_ShadowColor, p_width);
      }
      return l_Border;
    }
    #endregion
  }
}