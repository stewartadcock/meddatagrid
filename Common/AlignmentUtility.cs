#region MIT License
//
// Filename: AlignmentUtility.cs
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
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Alignment Utility
  /// </summary>
  public static class AlignmentUtility
  {

    /// <summary>
    /// Determines whether the specified alignment is bottom.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is bottom; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBottom(ContentAlignment a)
    {
      return a == ContentAlignment.BottomCenter ||
        a == ContentAlignment.BottomLeft ||
        a == ContentAlignment.BottomRight;
    }

    /// <summary>
    /// Determines whether the specified string format is bottom aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is bottom; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBottom(StringFormat a)
    {
      return a.LineAlignment == StringAlignment.Far;
    }

    /// <summary>
    /// Determines whether the specified alignment is top.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is top; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTop(ContentAlignment a)
    {
      return a == ContentAlignment.TopCenter ||
        a == ContentAlignment.TopLeft ||
        a == ContentAlignment.TopRight;
    }

    /// <summary>
    /// Determines whether the specified string format is top aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified a is top; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTop(StringFormat a)
    {
      return a.LineAlignment == StringAlignment.Near;
    }

    /// <summary>
    /// Determines whether the specified alignment is middle.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is middle; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMiddle(ContentAlignment a)
    {
      return a == ContentAlignment.MiddleCenter ||
        a == ContentAlignment.MiddleLeft ||
        a == ContentAlignment.MiddleRight;
    }

    /// <summary>
    /// Determines whether the specified string format is middle aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is middle; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMiddle(StringFormat a)
    {
      return a.LineAlignment == StringAlignment.Center;
    }

    /// <summary>
    /// Determines whether the specified alignment is center.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is center; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsCenter(ContentAlignment a)
    {
      return a == ContentAlignment.BottomCenter ||
        a == ContentAlignment.MiddleCenter ||
        a == ContentAlignment.TopCenter;
    }

    /// <summary>
    /// Determines whether the specified string format is center aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is center; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsCenter(StringFormat a)
    {
      return a.Alignment == StringAlignment.Center;
    }

    /// <summary>
    /// Determines whether the specified alignment is left.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is left; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLeft(ContentAlignment a)
    {
      return a == ContentAlignment.BottomLeft ||
        a == ContentAlignment.MiddleLeft ||
        a == ContentAlignment.TopLeft;
    }

    /// <summary>
    /// Determines whether the specified string format is left aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is left; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLeft(StringFormat a)
    {
      return a.Alignment == StringAlignment.Near;
    }

    /// <summary>
    /// Determines whether the specified alignment is right.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is right; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRight(ContentAlignment a)
    {
      return a == ContentAlignment.BottomRight ||
        a == ContentAlignment.MiddleRight ||
        a == ContentAlignment.TopRight;
    }

    /// <summary>
    /// Determines whether the specified string format is right aligned.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns>
    ///   <c>true</c> if the specified alignment is right; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRight(StringFormat a)
    {
      return a.Alignment == StringAlignment.Far;
    }

    /// <summary>
    /// Converts content alignment to corresponding horizontal alignment.
    /// </summary>
    /// <param name="a">The alignment mode</param>
    /// <returns></returns>
    public static HorizontalAlignment ContentToHorizontalAlignment(ContentAlignment a)
    {
      if (IsLeft(a))
      {
        return HorizontalAlignment.Left;
      }
      else if (IsRight(a))
      {
        return HorizontalAlignment.Right;
      }
      else
      {
        return HorizontalAlignment.Center;
      }
    }
  }
}