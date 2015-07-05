#region MIT License
//
// Filename: CheckBoxCell.cs
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
  /// A Cell with a CheckBox. This Cell value is of type bool.
  /// Abstract, you must override GetValue and SetValue.
  /// </summary>
  [ComVisible(false)]
  public abstract class CheckBoxCell : CellVirtual, ICellCheckBox
  {
    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxCell"/> class.
    /// </summary>
    protected CheckBoxCell()
    {
      DataModel = new DataModels.DataModelBase(typeof(bool));
      VisualModel = VisualModels.CheckBox.Default;
      BehaviorModels.Add(BehaviorModel.CheckBoxBehaviorModel.Default);
    }
    #endregion

    #region ICellCheckBox Members
    /// <summary>
    /// Checked status (equal to the Value property but returns a bool). Call the GetValue
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual bool GetCheckedValue(Position position)
    {
      return (bool)GetValue(position);
    }

    /// <summary>
    /// Set checked value, call the Model.SetCellValue. Can be called only if EnableEdit is true
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="isChecked">if set to <c>true</c> cell is checked.</param>
    public virtual void SetCheckedValue(Position position, bool isChecked)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, isChecked);
      }
    }

    /// <summary>
    /// Get the status of the checkbox at the current position
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual CheckBoxStatus GetCheckBoxStatus(Position position)
    {
      return new CheckBoxStatus(DataModel.EnableEdit, GetCheckedValue(position), null);
    }
    #endregion
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A Cell with a CheckBox. This Cell value is of type bool.
  /// </summary>
  [ComVisible(false)]
  public class CheckBoxCell : Cell, ICellCheckBox, IComparable
  {
    private string caption;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxCell"/> class.
    /// </summary>
    /// <remarks>
    /// Construct a CellCheckBox class with no caption, unchecked, and align the checkbox in MiddleCenter position
    /// </remarks>
    public CheckBoxCell()
      : this(null, false)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxCell"/> class.
    /// </summary>
    /// <param name="p_InitialValue">if set to <c>true</c> initial value is true.</param>
    /// <remarks>
    /// Construct a CellCheckBox class with no caption, and align the checkbox in MiddleCenter position
    /// </remarks>
    public CheckBoxCell(bool p_InitialValue)
      : this(null, p_InitialValue)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxCell"/> class.
    /// </summary>
    /// <param name="p_Caption">The caption.</param>
    /// <param name="p_InitialValue">if set to <c>true</c> initial value is true.</param>
    /// <remarks>
    /// Construct a CellCheckBox class with caption and align checkbox in the MiddleLeft
    /// </remarks>
    public CheckBoxCell(string p_Caption, bool p_InitialValue)
    {
      caption = p_Caption;

      DataModel = new DataModels.DataModelBase(typeof(bool));

      if (p_Caption == null || p_Caption.Length <= 0)
      {
        VisualModel = VisualModels.CheckBox.Default;
      }
      else
      {
        VisualModel = VisualModels.CheckBox.MiddleLeftAlign;
      }

      BehaviorModels.Add(BehaviorModel.CheckBoxBehaviorModel.Default);
      Value = p_InitialValue;

      IsLastExpandedCell = false;
      EnableEdit = true;
      EditableMode = EditableModes.None;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether the checked status is
    /// true (equal to the Value property but returns a bool)
    /// </summary>
    /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
    public bool Checked
    {
      get { return GetCheckedValue(Range.Start); }
      set { SetCheckedValue(Range.Start, value); }
    }

    /// <summary>
    /// Gets or sets the caption of the cell
    /// </summary>
    /// <value>The caption.</value>
    public string Caption
    {
      get { return this.caption; }
      set { this.caption = value; }
    }
    #endregion

    #region ICellCheckBox Members
    /// <summary>
    /// Checked status (equal to the Value property but returns a bool). Call the GetValue
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual bool GetCheckedValue(Position position)
    {
      return (bool)GetValue(position);
    }

    /// <summary>
    /// Set checked value, call the Model.SetCellValue. Can be called only if EnableEdit is true
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="isChecked">if set to <c>true</c> cell is checked.</param>
    public virtual void SetCheckedValue(Position position, bool isChecked)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, isChecked);
      }
    }

    /// <summary>
    /// Get the status of the checkbox at the current position
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual CheckBoxStatus GetCheckBoxStatus(Position position)
    {
      return new CheckBoxStatus(DataModel.EnableEdit, GetCheckedValue(position), caption);
    }

    /// <summary>
    /// compare 2 CheckBoxCell according to checked or not
    /// </summary>
    /// <param name="obj">The other CheckBoxCell</param>
    /// <returns>0 if both checked or unchecked, -1 if this unchecked and other(obj) checked else +1</returns>
    public int CompareTo(object obj)
    {
      bool otherChecked = ((CheckBoxCell)obj).Checked;
      if (!Checked && otherChecked)
      {
        return -1;
      }
      else if (Checked && !otherChecked)
      {
        return 1;
      }
      else
      {
        return 0;
      }
    }
    #endregion
  }
}