#region MIT License
//
// Filename: ComboBoxCell.cs
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  /// <summary>
  /// A cell with a combobox for editor. Use a model with an ICollection for standard values.
  /// </summary>
  [ComVisible(false)]
  public abstract class ComboBoxCell : CellVirtual
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBoxCell"/> class.
    /// </summary>
    /// <param name="p_CellType">Type of the cell.</param>
    /// <param name="p_StandardValues">The standard values.</param>
    /// <param name="p_StandardValuesExclusive">if set to <c>true</c> standard values are exclusive.</param>
    protected ComboBoxCell(Type p_CellType, System.Collections.ICollection p_StandardValues, bool p_StandardValuesExclusive)
    {
      DataModel = new DataModels.EditorComboBox(p_CellType, p_StandardValues, p_StandardValuesExclusive);
    }
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A cell with a combobox for editor. Use a model with an ICollection for standard values.
  /// </summary>
  [ComVisible(false)]
  public class ComboBoxCell : Cell
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBoxCell"/> class.
    /// </summary>
    /// <param name="value">The value to display</param>
    /// <param name="p_CellType">The data type</param>
    /// <param name="p_StandardValues">The collection of items</param>
    /// <param name="p_StandardValuesExclusive">If true only the standard values are supported</param>
    public ComboBoxCell(object value, Type p_CellType, System.Collections.ICollection p_StandardValues, bool p_StandardValuesExclusive)
      : base(value)
    {
      DataModel = new DataModels.EditorComboBox(p_CellType, p_StandardValues, p_StandardValuesExclusive);
    }
  }
}