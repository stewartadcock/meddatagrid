#region MIT License
//
// Filename: DataModelBase.cs
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

using Fr.Fc.FcCore.Logging;

using Fr.Medit.MedDataGrid.ConversionModel.Validator;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// Represents the base class of a DataModel. This DataModel support conversion but doesn't provide any user interface editor.
  /// </summary>
  [ComVisible(false)]
  public class DataModelBase : ValidatorTypeConverter, IDataModel
  {
    #region Class variables
    private Cells.ICellVirtual editCell;
    private Position editPosition = Position.Empty;

    private bool doEnableEdit = false;
    private EditableModes editableMode = EditableModes.Default;
    private bool doEnableCellDrawOnEdit = true;

    private event CellValidatingEventHandler validatingHandler;
    private event CellValidatedEventHandler validatedHandler;

    /// <summary>
    /// Error representation
    /// </summary>
    private string errorString = "#ERROR!";
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="DataModelBase"/> class.
    /// </summary>
    /// <param name="type">The type of this model</param>
    /// <remarks>
    /// Construct a Model. Based on the Type specified the Constructor populate
    /// StringEditor property
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown is null Type argument passed.</exception>
    public DataModelBase(Type type)
      : base(type)
    {
      // Do nothing.
    }
    #endregion

    #region Edit coordinates
    /// <summary>
    /// Gets cell if editing, if null no cell is in editing state
    /// </summary>
    /// <value>The edit cell.</value>
    public Cells.ICellVirtual EditCell
    {
      get { return this.editCell; }
    }

    /// <summary>
    /// Gets cell if editing, if Empty no cell is in editing state
    /// </summary>
    /// <value>The edit position.</value>
    public Position EditPosition
    {
      get { return this.editPosition; }
    }

    /// <summary>
    /// Set the current editing cell, for an editor only one cell can be in editing state
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <param name="position">The position.</param>
    protected void SetEditCell(Cells.ICellVirtual cell, Position position)
    {
      this.editCell = cell;
      this.editPosition = position;
    }
    #endregion

    #region ErrorString
    /// <summary>
    /// Returns true if the string passed is equal to the error string representation
    /// </summary>
    /// <param name="p_str">Error string</param>
    /// <returns></returns>
    public bool IsErrorString(string p_str)
    {
      return p_str == this.errorString;
    }

    /// <summary>
    /// Gets or sets string used when error occurred
    /// </summary>
    /// <value>The error string.</value>
    public string ErrorString
    {
      get { return this.errorString; }
      set { this.errorString = value; }
    }
    #endregion

    #region Editable settings
    /// <summary>
    /// Gets or sets a value indicating whether editting is enabled for this cell.
    /// If disabled, no edit is allowed, including non-UI editing.
    /// </summary>
    /// <value><c>true</c> if editting is enabled; otherwise, <c>false</c>.</value>
    public bool EnableEdit
    {
      get { return this.doEnableEdit; }
      set { this.doEnableEdit = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editable mode is enabled.
    /// </summary>
    /// <value>The editable mode.</value>
    public EditableModes EditableMode
    {
      get { return this.editableMode; }
      set { this.editableMode = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the draw of the cell, when in editing mode, is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if set to enable cell draw on edit; otherwise, <c>false</c>.
    /// </value>
    public virtual bool EnableCellDrawOnEdit
    {
      get { return this.doEnableCellDrawOnEdit; }
      set { this.doEnableCellDrawOnEdit = value; }
    }
    #endregion

    #region StartEdit/EndEdit/IsEditing/ApplyEdit/GetEditedValue
    /// <summary>
    /// Gets a value indicating whether the current editor is in editing state
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is editing state; otherwise, <c>false</c>.
    /// </value>
    public bool IsEditing
    {
      get { return editCell != null; }
    }

    /// <summary>
    /// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit.
    /// </summary>
    /// <param name="p_Cell">Cell to start edit</param>
    /// <param name="position">Editing position(Row/Col)</param>
    /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
    public virtual void InternalStartEdit(Cells.ICellVirtual p_Cell, Position position, object p_StartEditValue)
    {
      //throw new MEDDataGridException("No edit supported for this editor");
    }

    /// <summary>
    /// Apply edited value
    /// </summary>
    /// <returns></returns>
    public virtual bool InternalApplyEdit()
    {
      return true;
    }

    /// <summary>
    /// Cancel the edit action
    /// </summary>
    /// <param name="p_Cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
    /// <returns>
    /// Returns true if the cell terminate the editing mode
    /// </returns>
    public virtual bool InternalEndEdit(bool p_Cancel)
    {
      return true;
    }

    /// <summary>
    /// Returns the new value edited with the custom control
    /// </summary>
    /// <returns></returns>
    public virtual object GetEditedValue()
    {
      throw new MEDDataGridException("No valid cell editor found");
    }
    #endregion

    #region ClearCell/SetCellValue
    /// <summary>
    /// Clear the value of the cell using the default value
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="position">Cell position</param>
    public virtual void ClearCell(Cells.ICellVirtual p_Cell, Position position)
    {
      SetCellValue(p_Cell, position, DefaultValue);
    }

    /// <summary>
    /// Change the value of the cell applying the rule of the current editor.
    /// Is recommend to use this method to simulate a edit operation and to validate
    /// the cell value using the current model.
    /// </summary>
    /// <param name="p_Cell">Cell to change value</param>
    /// <param name="position">Current Cell Position</param>
    /// <param name="p_NewValue">New value</param>
    /// <returns>
    /// returns true if the value passed is valid and has been applied to the cell
    /// </returns>
    public virtual bool SetCellValue(Cells.ICellVirtual p_Cell, Position position, object p_NewValue)
    {
      if (EnableEdit == false)
      {
        return false;
      }

      CellValidatingEventArgs l_cancelEvent = new CellValidatingEventArgs(p_Cell, p_NewValue);
      OnValidating(l_cancelEvent);

      // check whether cancel == true
      if (l_cancelEvent.Cancel == false)
      {
        object l_PrevValue = p_Cell.GetValue(position);
        try
        {
          p_Cell.SetValue(position, ObjectToValue(l_cancelEvent.NewValue));
          OnValidated(new CellValidatedEventArgs(p_Cell));
        }
        catch (Exception ex)
        {
          LoggerManager.Log(LogLevels.Warning, "Exception caught: " + ex.ToString());
          p_Cell.SetValue(position, l_PrevValue);
          l_cancelEvent.Cancel = true;
        }
      }

      return l_cancelEvent.Cancel == false;
    }
    #endregion

    #region Validation
    /// <summary>
    /// Functions used when the validating operation is finished
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.CellValidatedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnValidated(CellValidatedEventArgs e)
    {
      if (this.validatedHandler != null)
      {
        this.validatedHandler(this, e);
      }
    }

    /// <summary>
    /// Validating the value of the cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.CellValidatingEventArgs"/> instance containing the event data.</param>
    protected virtual void OnValidating(CellValidatingEventArgs e)
    {
      if (this.validatingHandler != null)
      {
        this.validatingHandler(this, e);
      }
    }

    /// <summary>
    /// Cell validating event
    /// </summary>
    public event CellValidatingEventHandler Validating
    {
      add { this.validatingHandler += value; }
      remove { this.validatingHandler -= value; }
    }

    /// <summary>
    /// Cell validated event
    /// </summary>
    public event CellValidatedEventHandler Validated
    {
      add { this.validatedHandler += value; }
      remove { this.validatedHandler -= value; }
    }
    #endregion
  }
}