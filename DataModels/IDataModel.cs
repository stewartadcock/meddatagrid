#region MIT License
//
// Filename: IDataModel.cs
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

using Fr.Medit.MedDataGrid.ConversionModel.Validator;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// Class used for editing operation, string conversion and value formatting
  /// </summary>
  public interface IDataModel : IValidator
  {
    #region Editing
    /// <summary>
    /// Gets cell if editing, if null no cell is in editing state
    /// </summary>
    /// <value>The edit cell.</value>
    Cells.ICellVirtual EditCell
    {
      get;
    }

    /// <summary>
    /// Gets cell if editing, if Empty no cell is in editing state
    /// </summary>
    /// <value>The edit position.</value>
    Position EditPosition
    {
      get;
    }

    /// <summary>
    /// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit. For internal use only, use Cell.StartEdit.
    /// </summary>
    /// <param name="p_Cell">Cell to start edit</param>
    /// <param name="position">Editing position(Row/Col)</param>
    /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
    void InternalStartEdit(Cells.ICellVirtual p_Cell, Position position, object p_StartEditValue);

    /// <summary>
    /// Terminate the edit action. For internal use only, use Cell.EndEdit.
    /// </summary>
    /// <param name="p_Cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
    /// <returns>Returns true if the cell terminate the editing mode</returns>
    bool InternalEndEdit(bool p_Cancel);

    /// <summary>
    /// Gets a value indicating whether this instance is in editing state.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is editing state; otherwise, <c>false</c>.
    /// </value>
    bool IsEditing
    {
      get;
    }

    /// <summary>
    /// Gets or sets a value indicating whether editting is enabled for this cell.
    /// If disabled, no edit is allowed, including non-UI editing.
    /// </summary>
    /// <value><c>true</c> if editting is enabled; otherwise, <c>false</c>.</value>
    bool EnableEdit
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the editable mode.
    /// </summary>
    /// <value>The editable mode.</value>
    EditableModes EditableMode
    {
      get;
      set;
    }

    /// <summary>
    /// Gets a value indicating whether the draw of the cell, when in editing mode, is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if set to enable cell draw on edit; otherwise, <c>false</c>.
    /// </value>
    bool EnableCellDrawOnEdit
    {
      get;
    }
    #endregion

    #region Modify Functions
    /// <summary>
    /// Clear the value of the cell using the default value
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="position">Cell position</param>
    void ClearCell(Cells.ICellVirtual p_Cell, Position position);

    /// <summary>
    /// Change the value of the cell applying the rule of the current editor. Is recommend to use this
    /// method to simulate an edit operation and to validate the cell value using the current model.
    /// </summary>
    /// <param name="p_Cell">Cell to change value</param>
    /// <param name="position">Current Cell Position</param>
    /// <param name="p_NewValue">The new value.</param>
    /// <returns>
    /// returns true if the value passed is valid and has been applied to the cell
    /// </returns>
    bool SetCellValue(Cells.ICellVirtual p_Cell, Position position, object p_NewValue);
    #endregion

    #region Validating
    /// <summary>
    /// Fired to check whether the value specified by the user is allowed
    /// this event is fired after the ValidatingValue (use ValidatingValue to check whether the value is compatible with the cell)
    /// </summary>
    event CellValidatingEventHandler Validating;
    /// <summary>
    /// Fired after the value specified by the user inserited in the cell
    /// </summary>
    event CellValidatedEventHandler Validated;
    #endregion

    #region Conversion
    /// <summary>
    /// Check whether the given string is error
    /// </summary>
    /// <param name="p_str">The string.</param>
    /// <returns>
    /// <c>true</c> if the specified string is the error string; otherwise, <c>false</c>.
    /// </returns>
    bool IsErrorString(string p_str);
    #endregion
  }
}