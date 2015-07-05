#region MIT License
//
// Filename: CheckBox.cs
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
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// CheckBox visual model
  /// </summary>
  [ComVisible(false)]
  public class CheckBox : Common
  {
    #region Constants
    /// <summary>
    /// Represents a default CheckBox with the CheckBox image aligned to the
    /// Middle Centre of the cell. You must use this VisualModel with a Cell of type ICellCheckBox.
    /// </summary>
    public new static readonly CheckBox Default = new CheckBox(true);

    /// <summary>
    /// Represents a CheckBox with the CheckBox image align to the Middle Right of the cell
    /// </summary>
    public static readonly CheckBox MiddleLeftAlign;
    #endregion

    #region Class members
    private ContentAlignment checkBoxAlignment = ContentAlignment.MiddleCenter;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes the <see cref="CheckBox"/> class.
    /// </summary>
    static CheckBox()
    {
      MiddleLeftAlign = new CheckBox(false);
      MiddleLeftAlign.CheckBoxAlignment = ContentAlignment.MiddleLeft;
      MiddleLeftAlign.AlignTextToImage = true;
      MiddleLeftAlign.TextAlignment = ContentAlignment.MiddleLeft;
      MiddleLeftAlign.MakeReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBox"/> class.
    /// </summary>
    /// <remarks>
    /// Use default settings and construct read and write VisualProperties
    /// </remarks>
    public CheckBox()
      : this(false)
    {
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBox"/> class.
    /// </summary>
    /// <remarks>
    /// Use default settings.
    /// </remarks>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public CheckBox(bool isReadOnly)
    {
      this.isReadOnly = isReadOnly;
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBox"/> class.
    /// </summary>
    /// <remarks>
    /// Copy Constructor.  This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </remarks>
    /// <param name="source">The source.</param>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public CheckBox(CheckBox source, bool isReadOnly)
      : base(source, isReadOnly)
    {
      this.checkBoxAlignment = source.checkBoxAlignment;
      ExpandedCell = false;
    }
    #endregion

    /// <summary>
    /// Gets or sets the check box alignment.
    /// </summary>
    /// <value>The check box alignment.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public ContentAlignment CheckBoxAlignment
    {
      get
      {
        return this.checkBoxAlignment;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }

        this.checkBoxAlignment = value;

        OnChange();
      }
    }

    #region DrawCell
    /// <summary>
    /// Returns the image for the current check state
    /// </summary>
    /// <param name="isChecked">if set to <c>true</c> is checked.</param>
    /// <param name="isSelected">if set to <c>true</c> is selected.</param>
    /// <param name="isCheckEnabled">if set to <c>true</c> check is enabled.</param>
    /// <returns></returns>
    public static System.Drawing.Image GetImageForState(bool isChecked, bool isSelected, bool isCheckEnabled)
    {
      if (isCheckEnabled)
      {
        if (isChecked)
        {
          if (isSelected)
          {
            return IconUtility.CheckBoxCheckedSelected;
          }
          else
          {
            return IconUtility.CheckBoxChecked;
          }
        }
        else
        {
          if (isSelected)
          {
            return IconUtility.CheckBoxUncheckedSelected;
          }
          else
          {
            return IconUtility.CheckBoxUnchecked;
          }
        }
      }
      else
      {
        if (isChecked)
        {
          return IconUtility.CheckBoxCheckedDisabled;
        }
        else
        {
          return IconUtility.CheckBoxUncheckedDisabled;
        }
      }
    }

    /// <summary>
    /// Draw the image and the displaystring of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(ICellVirtual p_Cell, Position p_CellPosition, System.Windows.Forms.PaintEventArgs e, System.Drawing.Rectangle p_ClientRectangle, DrawCellStatus p_Status)
    {
      //base.DrawCell_ImageAndText (p_Cell, p_CellPosition, e, p_ClientRectangle, p_ForeColor, p_CellBorder);
      if (p_ClientRectangle.Width == 0 || p_ClientRectangle.Height == 0)
      {
        return;
      }

      RectangleBorder l_Border = Border;
      Color l_ForeColor = ForeColor;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_Border = FocusBorder;
        l_ForeColor = FocusForeColor;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_Border = SelectionBorder;
        l_ForeColor = SelectionForeColor;
      }

      ICellCheckBox l_CheckBox = (ICellCheckBox)p_Cell;
      CheckBoxStatus l_Status = l_CheckBox.GetCheckBoxStatus(p_CellPosition);

      Image l_CheckImage = GetImageForState(l_Status.Checked, p_Cell.Grid.MouseCellPosition == p_CellPosition, l_Status.CheckEnable);

      Font l_CurrentFont = GetCellFont();

      bool l_lastExpandedCell = p_Cell is ICell && ((ICell)p_Cell).IsLastExpandedCell;

      // Image and Text
      VisualModelBase.PaintImageAndText(e.Graphics,
        p_ClientRectangle,
        l_CheckImage,
        checkBoxAlignment,
        ImageStretch,
        l_Status.Caption,
        StringFormat,
        AlignTextToImage,
        l_Border,
        l_ForeColor,
        l_CurrentFont,
        false,
        l_lastExpandedCell);
    }
    #endregion

    #region Clone
    /// <summary>
    /// Clone this object. This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </summary>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    public override object Clone(bool isReadOnly)
    {
      return new CheckBox(this, isReadOnly);
    }
    #endregion
  }
}