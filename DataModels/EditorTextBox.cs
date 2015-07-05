#region MIT License
//
// Filename: EditorTextBox.cs
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
  /// A DataModel that use a TextBoxTyped for editing support.
  /// </summary>
  [ComVisible(false)]
  public class EditorTextBox : EditorControlBase
  {
    #region Class variables
    private bool isMultiLine = false;
    private int maximumLength = 0;
    private char[] validCharacters;
    private char[] invalidCharacters;
    #endregion

    #region Constructor
    /// <summary>
    /// Construct a Model. Based on the Type specified the Constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
    /// </summary>
    /// <param name="p_Type">The type of this model</param>
    public EditorTextBox(Type p_Type)
      : base(p_Type)
    {
    }
    #endregion

    #region Edit Control
    public override Control CreateEditorControl()
    {
      TextBoxTyped l_Control = new TextBoxTyped();
      l_Control.BorderStyle = BorderStyle.None;
      l_Control.AutoSize = false;
      return l_Control;
    }
    public virtual TextBoxTyped GetEditorTextBox(GridVirtual p_Grid)
    {
      return (TextBoxTyped)GetEditorControl(p_Grid);
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

      TextBoxTyped l_TxtBox = GetEditorTextBox(p_Cell.Grid);

      l_TxtBox.Validator = this;
      l_TxtBox.EnableEscapeKeyUndo = false;
      l_TxtBox.EnableEnterKeyValidate = false;
      l_TxtBox.EnableLastValidValue = false;
      l_TxtBox.EnableAutoValidation = false;

      l_TxtBox.Multiline = isMultiLine;
      if (isMultiLine == false)
      {
        l_TxtBox.MaxLength = maximumLength;
      }

      l_TxtBox.WordWrap = p_Cell.VisualModel.WordWrap;
      l_TxtBox.TextAlign = AlignmentUtility.ContentToHorizontalAlignment(p_Cell.VisualModel.TextAlignment);
      l_TxtBox.Font = p_Cell.VisualModel.Font;
      l_TxtBox.ValidCharacters = validCharacters;
      l_TxtBox.InvalidCharacters = invalidCharacters;

      if (p_StartEditValue is string && IsStringConversionSupported())
      {
        l_TxtBox.Text = TextBoxTyped.ValidateCharactersString((string)p_StartEditValue, validCharacters, invalidCharacters);
        l_TxtBox.SelectionStart = l_TxtBox.Text.Length;
      }
      else
      {
        l_TxtBox.Value = p_Cell.GetValue(position);
        l_TxtBox.SelectAll();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this is a multiline text box editor.
    /// </summary>
    /// <value><c>true</c> if multiline; otherwise, <c>false</c>.</value>
    public bool Multiline
    {
      get { return this.isMultiLine; }
      set { this.isMultiLine = value; }
    }

    /// <summary>
    /// Gets or sets the maximum number of characters allowed in the text box editor.
    /// </summary>
    /// <value>The length of the max.</value>
    public int MaxLength
    {
      get { return this.maximumLength; }
      set { this.maximumLength = value; }
    }

    /// <summary>
    /// Gets or sets a list of characters allowed for the textbox. Used in the OnKeyPress event. If null no check is made.
    /// If not null any others charecters is not allowed. First the function check whether ValidCharacters is not null then check for InvalidCharacters.
    /// </summary>
    /// <value>The valid characters.</value>
    public char[] ValidCharacters
    {
      get { return this.validCharacters; }
      set { this.validCharacters = value; }
    }

    /// <summary>
    /// Gets or sets a list of characters not allowed for the textbox. Used in the OnKeyPress event. If null no check is made.
    /// If not null any characters in the list is not allowed. First the function check whether ValidCharacters is not null then check for InvalidCharacters.
    /// </summary>
    /// <value>The invalid characters.</value>
    public char[] InvalidCharacters
    {
      get { return this.invalidCharacters; }
      set { this.invalidCharacters = value; }
    }

    /// <summary>
    /// Returns the value inserted with the current editor control
    /// </summary>
    /// <returns></returns>
    public override object GetEditedValue()
    {
      return GetEditorTextBox(EditCell.Grid).Value;
    }
  }
}