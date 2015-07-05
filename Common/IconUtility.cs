#region MIT License
//
// Filename: IconUtility.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Icon Utility.
  /// </summary>
  [ComVisible(false)]
  public class IconUtility
  {
    private static Image sortDown;
    private static Image sortUp;
    private static Image checkBoxChecked;
    private static Image checkBoxCheckedDisable;
    private static Image paste;
    private static Image properties;
    private static Image plus;
    private static Image minus;
    private static Image checkBoxCheckedSelected;
    private static Image checkBoxUnchecked;
    private static Image checkBoxUncheckedDisabled;
    private static Image checkBoxUncheckedSelected;
    private static Image clear;
    private static Image copy;
    private static Image cut;
    private static Image deleteColumn;
    private static Image deleteRow;
    private static Image insertColumn;
    private static Image insertRow;

    /// <summary>
    /// Gets the embedded image.
    /// </summary>
    /// <param name="imageLabel">The image label.</param>
    /// <returns></returns>
    private static Image GetEmbeddedImage(string imageLabel)
    {
      System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      Stream resourceStream = executingAssembly.GetManifestResourceStream("Fr.Medit.MedDataGrid.Common.Icons." + imageLabel);
      Image image = Image.FromStream(resourceStream);
      return image;
    }

    /// <summary>
    /// Initializes the <see cref="IconUtility"/> class.
    /// </summary>
    static IconUtility()
    {
      sortDown = GetEmbeddedImage("SortDown.ico");
      sortUp = GetEmbeddedImage("SortUp.ico");
      checkBoxChecked = GetEmbeddedImage("CheckBoxChecked.ico");
      checkBoxCheckedDisable = GetEmbeddedImage("CheckBoxCheckedDisabled.ico");
      checkBoxCheckedSelected = GetEmbeddedImage("CheckBoxCheckedSelected.ico");
      checkBoxUnchecked = GetEmbeddedImage("CheckBoxUnChecked.ico");
      checkBoxUncheckedDisabled = GetEmbeddedImage("CheckBoxUnCheckedDisabled.ico");
      checkBoxUncheckedSelected = GetEmbeddedImage("CheckBoxUnCheckedSelected.ico");
      clear = GetEmbeddedImage("clear.ico");
      copy = GetEmbeddedImage("copy.ico");
      cut = GetEmbeddedImage("cut.ico");
      deleteColumn = GetEmbeddedImage("DeleteColumn.ico");
      deleteRow = GetEmbeddedImage("DeleteRow.ico");
      insertColumn = GetEmbeddedImage("InsertColumn.ico");
      insertRow = GetEmbeddedImage("InsertRow.ico");
      paste = GetEmbeddedImage("paste.ico");
      properties = GetEmbeddedImage("properties.ico");
      plus = GetEmbeddedImage("Plus.bmp");
      minus = GetEmbeddedImage("Minus.bmp");
    }

    /// <summary>
    /// Gets the sort down icon.
    /// </summary>
    /// <value>The sort down icon.</value>
    public static Image SortDown
    {
      get { return sortDown; }
    }

    /// <summary>
    /// Gets the sort up icon.
    /// </summary>
    /// <value>The sort up icon.</value>
    public static Image SortUp
    {
      get { return sortUp; }
    }

    /// <summary>
    /// Gets the check box checked icon.
    /// </summary>
    /// <value>The check box checked icon.</value>
    public static Image CheckBoxChecked
    {
      get { return checkBoxChecked; }
    }

    /// <summary>
    /// Gets the check box checked disable icon.
    /// </summary>
    /// <value>The check box checked disable icon.</value>
    public static Image CheckBoxCheckedDisabled
    {
      get { return checkBoxCheckedDisable; }
    }

    /// <summary>
    /// Gets the check box checked selected icon.
    /// </summary>
    /// <value>The check box checked selected icon.</value>
    public static Image CheckBoxCheckedSelected
    {
      get { return checkBoxCheckedSelected; }
    }

    /// <summary>
    /// Gets the check box unchecked icon.
    /// </summary>
    /// <value>The check box unchecked icon.</value>
    public static Image CheckBoxUnchecked
    {
      get { return checkBoxUnchecked; }
    }

    /// <summary>
    /// Gets the check box unchecked disabled icon.
    /// </summary>
    /// <value>The check box unchecked disabled icon.</value>
    public static Image CheckBoxUncheckedDisabled
    {
      get { return checkBoxUncheckedDisabled; }
    }

    /// <summary>
    /// Gets the check box unchecked selected icon.
    /// </summary>
    /// <value>The check box unchecked selected icon.</value>
    public static Image CheckBoxUncheckedSelected
    {
      get { return checkBoxUncheckedSelected; }
    }

    /// <summary>
    /// Gets the clear icon.
    /// </summary>
    /// <value>The clear icon.</value>
    public static Image Clear
    {
      get { return clear; }
    }

    /// <summary>
    /// Gets the copy icon.
    /// </summary>
    /// <value>The copy icon.</value>
    public static Image Copy
    {
      get { return copy; }
    }

    /// <summary>
    /// Gets the cut icon.
    /// </summary>
    /// <value>The cut icon.</value>
    public static Image Cut
    {
      get { return cut; }
    }

    /// <summary>
    /// Gets the delete column icon.
    /// </summary>
    /// <value>The delete column  icon.</value>
    public static Image DeleteColumn
    {
      get { return deleteColumn; }
    }

    /// <summary>
    /// Gets the delete row icon.
    /// </summary>
    /// <value>The delete row icon.</value>
    public static Image DeleteRow
    {
      get { return deleteRow; }
    }

    /// <summary>
    /// Gets the insert column icon.
    /// </summary>
    /// <value>The insert column icon.</value>
    public static Image InsertColumn
    {
      get { return insertColumn; }
    }

    /// <summary>
    /// Gets the insert row icon.
    /// </summary>
    /// <value>The insert row icon.</value>
    public static Image InsertRow
    {
      get { return insertRow; }
    }

    /// <summary>
    /// Gets the paste icon.
    /// </summary>
    /// <value>The paste icon.</value>
    public static Image Paste
    {
      get { return paste; }
    }

    /// <summary>
    /// Gets the properties icon.
    /// </summary>
    /// <value>The properties icon.</value>
    public static Image Properties
    {
      get { return properties; }
    }

    /// <summary>
    /// Gets the plus icon.
    /// </summary>
    /// <value>The plus icon.</value>
    public static Image Plus
    {
      get { return plus; }
    }

    /// <summary>
    /// Gets the minus icon.
    /// </summary>
    /// <value>The minus icon.</value>
    public static Image Minus
    {
      get { return minus; }
    }
  }
}