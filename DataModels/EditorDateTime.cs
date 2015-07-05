#region MIT License
//
// Filename: EditorDateTime.cs
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

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// EditorDateTime
  /// </summary>
  [ComVisible(false)]
  public class EditorDateTime : EditorControlBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EditorDateTime"/> class.
    /// </summary>
    public EditorDateTime()
      : base(typeof(DateTime))
    {
    }

    #region Edit Control
    public override Control CreateEditorControl()
    {
      System.Windows.Forms.DateTimePicker l_dtPicker = new DateTimePicker();
      l_dtPicker.Format = DateTimePickerFormat.Short;
      return l_dtPicker;
    }
    public virtual System.Windows.Forms.DateTimePicker GetEditorDateTimePicker(GridVirtual p_Grid)
    {
      return (System.Windows.Forms.DateTimePicker)GetEditorControl(p_Grid);
    }
    #endregion

    /// <summary>
    /// Start editing the cell passed
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

      DateTimePicker l_DtPicker = GetEditorDateTimePicker(p_Cell.Grid);
      if (p_StartEditValue != null)
      {
        if (p_StartEditValue is DateTime)
        {
          l_DtPicker.Value = (DateTime)p_StartEditValue;
        }
        else if (p_StartEditValue == null)
        {
          l_DtPicker.Value = DateTime.Now;
        }
        else
        {
          throw new MEDDataGridException("Invalid StartEditValue, expected DateTime");
        }
      }
      else
      {
        object l_Val = p_Cell.GetValue(position);
        if (l_Val is DateTime)
        {
          l_DtPicker.Value = (DateTime)l_Val;
        }
        else if (l_Val == null)
        {
          l_DtPicker.Value = DateTime.Now;
        }
        else
        {
          throw new MEDDataGridException("Invalid cell value, expected DateTime");
        }
      }
    }

    /// <summary>
    /// Returns the value inserted with the current editor control
    /// </summary>
    /// <returns></returns>
    public override object GetEditedValue()
    {
      return GetEditorDateTimePicker(EditCell.Grid).Value;
    }
  }
}