#region MIT License
//
// Filename: EditorComboBox.cs
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
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Controls;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// Editor for a ComboBox (using ComboBoxEx control)
  /// </summary>
  [ComVisible(false)]
  public class EditorComboBox : EditorControlBase
  {
    #region Constructor
    /// <summary>
    /// Construct a Model. Based on the Type specified the Constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
    /// </summary>
    /// <param name="p_Type">The type of this model</param>
    public EditorComboBox(Type p_Type)
      : base(p_Type)
    {
    }

    /// <summary>
    /// Construct a Model. Based on the Type specified the Constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
    /// </summary>
    /// <param name="p_Type">The type of this model</param>
    /// <param name="p_StandardValues">The standard values.</param>
    /// <param name="p_StandardValueExclusive">True to not allow custom value, only the values specified in the standardvalues collection are allowed.</param>
    public EditorComboBox(Type p_Type, ICollection p_StandardValues, bool p_StandardValueExclusive)
      : base(p_Type)
    {
      StandardValues = p_StandardValues;
      StandardValuesExclusive = p_StandardValueExclusive;
    }
    #endregion

    #region Edit Control
    public override Control CreateEditorControl()
    {
      ComboBoxTyped l_ComboBox = new ComboBoxTyped();
      l_ComboBox.TextBox.BorderStyle = BorderStyle.None;
      return l_ComboBox;
    }

    public virtual ComboBoxTyped GetEditorComboBox(GridVirtual p_Grid)
    {
      return (ComboBoxTyped)GetEditorControl(p_Grid);
    }
    #endregion

    /// <summary>
    /// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit.
    /// </summary>
    /// <param name="p_Cell">Cell to start edit</param>
    /// <param name="position">Editing position(Row/Col)</param>
    /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
    public override void InternalStartEdit(Cells.ICellVirtual p_Cell, Position position, object p_StartEditValue)
    {
      base.InternalStartEdit(p_Cell, position, p_StartEditValue);

      if (EnableEdit == false)
      {
        return;
      }

      ComboBoxTyped l_Combo = GetEditorComboBox(p_Cell.Grid);
      l_Combo.Validator = this;
      l_Combo.EnableEscapeKeyUndo = false;
      l_Combo.EnableEnterKeyValidate = false;
      l_Combo.EnableLastValidValue = false;
      l_Combo.EnableAutoValidation = false;

      if (p_StartEditValue is string && IsStringConversionSupported())
      {
        l_Combo.TextBox.Text = TextBoxTyped.ValidateCharactersString((string)p_StartEditValue, l_Combo.TextBox.ValidCharacters, l_Combo.TextBox.InvalidCharacters);
        if (l_Combo.TextBox.Text != null)
        {
          l_Combo.TextBox.SelectionStart = l_Combo.TextBox.Text.Length;
        }
        else
        {
          l_Combo.TextBox.SelectionStart = 0;
        }
      }
      else
      {
        l_Combo.Value = p_Cell.GetValue(position);
        l_Combo.SelectAllTextBox();
      }
    }

    /// <summary>
    /// Returns the value inserted with the current editor control
    /// </summary>
    /// <returns></returns>
    public override object GetEditedValue()
    {
      return GetEditorComboBox(EditCell.Grid).Value;
    }
  }
}