#region MIT License
//
// Filename: DataModelFactory.cs
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
using System.Collections;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// Factory class for data models.
  /// </summary>
  public static class DataModelFactory
  {
    /// <summary>
    /// Construct a DataModel for the specified type.
    /// </summary>
    /// <param name="p_Type">Cell Type</param>
    /// <returns>
    /// If the Type support an <c>UITypeEditor</c> returns an <see cref="EditorUITypeEditor"/> else if the type has a StandardValues list return an <c>EditorComboBox</c>
    /// else if the type support string conversion returns an <see cref="EditorTextBox"/> otherwise returns <c>null</c>.
    /// </returns>
    public static DataModels.IDataModel CreateDataModel(Type p_Type)
    {
      System.ComponentModel.TypeConverter l_TypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(p_Type);
      ICollection l_StandardValues = null;
      bool l_StandardValuesExclusive = false;
      if (l_TypeConverter != null)
      {
        l_StandardValues = l_TypeConverter.GetStandardValues();
        l_StandardValuesExclusive = l_TypeConverter.GetStandardValuesExclusive();
      }
      object l_objUITypeEditor = System.ComponentModel.TypeDescriptor.GetEditor(p_Type, typeof(System.Drawing.Design.UITypeEditor));
      if (l_objUITypeEditor != null) // UITypeEditor founded
      {
        return new DataModels.EditorUITypeEditor(p_Type, (System.Drawing.Design.UITypeEditor)l_objUITypeEditor);
      }
      else
      {
        if (l_StandardValues != null) // combo box
        {
          return new DataModels.EditorComboBox(p_Type, l_StandardValues, l_StandardValuesExclusive);
        }
        else if (l_TypeConverter != null && l_TypeConverter.CanConvertFrom(typeof(string)))//txtbox
        {
          return new DataModels.EditorTextBox(p_Type);
        }
        else // no editor found
        {
          return null;
        }
      }
    }

    /// <summary>
    /// Creates the data model.
    /// Construct a CellEditor for the specified type
    /// </summary>
    /// <param name="p_Type">Cell Type</param>
    /// <param name="p_DefaultValue">Default value of the editor</param>
    /// <param name="p_bAllowNull">Allow null</param>
    /// <param name="p_StandardValues">List of available values or null if there is no available values list</param>
    /// <param name="p_bStandardValueExclusive">Indicates whether the p_StandardValue are the unique values supported</param>
    /// <param name="p_TypeConverter">Type converter used for conversion for the specified type</param>
    /// <param name="p_UITypeEditor">UITypeEditor if null must be populated the TypeConverter</param>
    /// <returns></returns>
    public static DataModels.IDataModel CreateDataModel(Type p_Type,
      object p_DefaultValue,
      bool p_bAllowNull,
      System.Collections.ICollection p_StandardValues,
      bool p_bStandardValueExclusive,
      System.ComponentModel.TypeConverter p_TypeConverter,
      System.Drawing.Design.UITypeEditor p_UITypeEditor)
    {
      DataModels.DataModelBase l_Editor;
      if (p_UITypeEditor == null)
      {
        if (p_StandardValues != null)
        {
          DataModels.EditorComboBox l_EditCombo = new DataModels.EditorComboBox(p_Type);
          l_Editor = l_EditCombo;
        }
        else if (p_TypeConverter != null && p_TypeConverter.CanConvertFrom(typeof(string)))//txtbox
        {
          DataModels.EditorTextBox l_EditTextBox = new DataModels.EditorTextBox(p_Type);
          l_Editor = l_EditTextBox;
        }
        else // if no editor no edit support
        {
          l_Editor = null;
        }
      }
      else // UITypeEditor supported
      {
        DataModels.EditorUITypeEditor l_UITypeEditor = new DataModels.EditorUITypeEditor(p_Type, p_UITypeEditor);
        l_Editor = l_UITypeEditor;
      }

      if (l_Editor != null)
      {
        l_Editor.DefaultValue = p_DefaultValue;
        l_Editor.AllowNull = p_bAllowNull;
        ////l_Editor.CellType = p_Type;
        l_Editor.StandardValues = p_StandardValues;
        l_Editor.StandardValuesExclusive = p_bStandardValueExclusive;
        l_Editor.TypeConverter = p_TypeConverter;
      }

      return l_Editor;
    }
  }
}