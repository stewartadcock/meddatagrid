#region MIT License
//
// Filename: ICellBitmapCell.cs
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

namespace Fr.Medit.MedDataGrid.Cells
{
  /// <summary>
  /// Interface for informations about a bitmap cell.
  /// </summary>
  [ComVisible(false)]
  public interface ICellBitmapCell
  {
    /// <summary>
    /// return image (equal to the Value property but returns a Bitmap). Call the GetValue
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    Bitmap GetBitmap(Position position);

    /// <summary>
    /// Set Image, call the Model.SetCellValue.
    /// Can be called only if EnableEdit is true
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="bitmap">The checked bitmap.</param>
    void SetBitmap(Position position, Bitmap bitmap);
  }
}