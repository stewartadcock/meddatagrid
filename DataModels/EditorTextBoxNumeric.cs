#region MIT License
//
// Filename: EditorTextBoxNumeric.cs
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
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Controls;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// A DataModel that use a TextBoxTypedNumeric for editing support.
  /// </summary>
  /// <remarks>
  /// You can customize the NumericCharStyle property to enable char validation.
  /// This class automatically set the ValidCharacters and InvalidCharacters using TextBoxTypedNumeric.CreateNumericValidChars method.
  /// </remarks>
  [ComVisible(false)]
  public class EditorTextBoxNumeric : EditorTextBox
  {
    private NumericCharStyles numericCharStyle = NumericCharStyles.DecimalSeparator | NumericCharStyles.NegativeSymbol;

    #region Constructor
    /// <summary>
    /// Construct a Model. Based on the Type specified the Constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
    /// </summary>
    /// <param name="p_Type">The type of this model</param>
    public EditorTextBoxNumeric(Type p_Type)
      : base(p_Type)
    {
    }
    #endregion

    #region Edit Control
    public override Control CreateEditorControl()
    {
      TextBoxTypedNumeric l_Control = new TextBoxTypedNumeric();
      l_Control.BorderStyle = BorderStyle.None;
      l_Control.AutoSize = false;
      return l_Control;
    }
    public virtual TextBoxTypedNumeric GetEditorTextBoxNumeric(GridVirtual p_Grid)
    {
      return (TextBoxTypedNumeric)GetEditorControl(p_Grid);
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
      ValidCharacters = TextBoxTypedNumeric.CreateNumericValidChars(CultureInfo, numericCharStyle);
      InvalidCharacters = null;
      base.InternalStartEdit(p_Cell, position, p_StartEditValue);
    }

    /// <summary>
    /// Gets or sets the numeric char style.
    /// </summary>
    /// <value>The numeric char style.</value>
    public NumericCharStyles NumericCharStyle
    {
      get { return this.numericCharStyle; }
      set { this.numericCharStyle = value; }
    }
  }
}