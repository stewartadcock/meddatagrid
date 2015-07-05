#region MIT License
//
// Filename: EditorNumericUpDown.cs
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
  /// NumericUpDownEditor editor control.
  /// This is a model of type Decimal.
  /// </summary>
  [ComVisible(false)]
  public class EditorNumericUpDown : EditorControlBase
  {
    private decimal maximum = 100;
    private decimal minimum = 0;
    private decimal increment = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorNumericUpDown"/> class.
    /// </summary>
    public EditorNumericUpDown()
      : base(typeof(decimal))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorNumericUpDown"/> class.
    /// </summary>
    /// <param name="p_CellType">Type of the cell.</param>
    /// <param name="p_Maximum">The maximum value.</param>
    /// <param name="p_Minimum">The minimum value.</param>
    /// <param name="p_Increment">The increment.</param>
    /// <exception cref="MEDDataGridException">Thrown if Invalid type of the cell - expected decimal, long or int</exception>
    public EditorNumericUpDown(Type p_CellType, decimal p_Maximum, decimal p_Minimum, decimal p_Increment)
      : base(p_CellType)
    {
      if (p_CellType == null || p_CellType == typeof(int) ||
        p_CellType == typeof(long) || p_CellType == typeof(decimal))
      {
        maximum = p_Maximum;
        minimum = p_Minimum;
        increment = p_Increment;
      }
      else
      {
        throw new MEDDataGridException("Invalid CellType expected long, int or decimal");
      }
    }

    #region Edit Control
    public override Control CreateEditorControl()
    {
      System.Windows.Forms.NumericUpDown l_Control = new System.Windows.Forms.NumericUpDown();
      l_Control.BorderStyle = System.Windows.Forms.BorderStyle.None;

      return l_Control;
    }
    public virtual System.Windows.Forms.NumericUpDown GetEditorNumericUpDown(GridVirtual p_Grid)
    {
      return (System.Windows.Forms.NumericUpDown)GetEditorControl(p_Grid);
    }
    #endregion

    /// <summary>
    /// Start editing the cell passed
    /// </summary>
    /// <param name="p_Cell">Cell to start edit</param>
    /// <param name="position">Editing position(Row/Col)</param>
    /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
    /// <exception cref="MEDDataGridException">Thrown if Invalid type of the cell - expected decimal, long or int</exception>
    public override void InternalStartEdit(Cells.ICellVirtual p_Cell, Position position, object p_StartEditValue)
    {
      base.InternalStartEdit(p_Cell, position, p_StartEditValue);

      if (EnableEdit == false)
      {
        return;
      }

      System.Windows.Forms.NumericUpDown l_Control = GetEditorNumericUpDown(p_Cell.Grid);

      l_Control.Maximum = maximum;
      l_Control.Minimum = minimum;
      l_Control.Increment = increment;

      if (p_StartEditValue != null)
      {
        SetValueToControl(p_StartEditValue);
      }
      else
      {
        SetValueToControl(p_Cell.GetValue(position));
      }
    }

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    /// <value>The maximum.</value>
    public decimal Maximum
    {
      get { return this.maximum; }
      set { this.maximum = value; }
    }

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    /// <value>The minimum.</value>
    public decimal Minimum
    {
      get { return this.minimum; }
      set { this.minimum = value; }
    }

    /// <summary>
    /// Gets or sets the increment.
    /// </summary>
    /// <value>The increment.</value>
    public decimal Increment
    {
      get { return this.increment; }
      set { this.increment = value; }
    }

    /// <summary>
    /// Returns the value inserted with the current editor control
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MEDDataGridException">Thrown if Invalid type of the cell - expected decimal, long or int</exception>
    public override object GetEditedValue()
    {
      return GetValueFromControl();
    }

    /// <summary>
    /// Gets the value from control.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MEDDataGridException">Thrown if Invalid type of the cell - expected decimal, long or int</exception>
    private object GetValueFromControl()
    {
      if (ValueType == null)
      {
        return GetEditorNumericUpDown(EditCell.Grid).Value;
      }
      if (ValueType == typeof(decimal))
      {
        return GetEditorNumericUpDown(EditCell.Grid).Value;
      }
      if (ValueType == typeof(int))
      {
        return (int)GetEditorNumericUpDown(EditCell.Grid).Value;
      }
      if (ValueType == typeof(long))
      {
        return (long)GetEditorNumericUpDown(EditCell.Grid).Value;
      }

      throw new MEDDataGridException("Invalid type of the cell expected decimal, long or int");
    }

    /// <summary>
    /// Sets the value to control.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="MEDDataGridException">Thrown if Invalid type of the cell - expected decimal, long or int</exception>
    private void SetValueToControl(object value)
    {
      if (value is decimal)
      {
        GetEditorNumericUpDown(EditCell.Grid).Value = (decimal)value;
      }
      else if (value is long)
      {
        GetEditorNumericUpDown(EditCell.Grid).Value = (decimal)((long)value);
      }
      else if (value is int)
      {
        GetEditorNumericUpDown(EditCell.Grid).Value = (decimal)((int)value);
      }
      else if (value == null)
      {
        GetEditorNumericUpDown(EditCell.Grid).Value = GetEditorNumericUpDown(EditCell.Grid).Minimum;
      }
      else
      {
        throw new MEDDataGridException("Invalid value, expected Decimal, Int or Long");
      }
    }
  }
}