#region MIT License
//
// Filename: ICellVirtual.cs
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

using Fr.Medit.MedDataGrid.BehaviorModel;

namespace Fr.Medit.MedDataGrid.Cells
{
  /// <summary>
  /// Interface to represents a cell virtual (without position or value information).
  /// </summary>
  /// <remarks>
  /// SAA TODO: This should be a generic interface.
  /// </remarks>
  [ComVisible(false)]
  public interface ICellVirtual
  {
    #region LinkToGrid
    /// <summary>
    /// Gets the owner Grid
    /// </summary>
    /// <value>The grid.</value>
    GridVirtual Grid
    {
      get;
    }

    /// <summary>
    /// Link the cell to the specified grid.
    /// </summary>
    /// <param name="p_grid">The grid.</param>
    void BindToGrid(GridVirtual p_grid);

    /// <summary>
    /// Remove the link of the cell from the previous grid.
    /// </summary>
    void UnBindToGrid();
    #endregion

    #region GetValue, SetValue, GetDisplayString
    /// <summary>
    /// Get the value of the cell at the specified position
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    object GetValue(Position position);

    /// <summary>
    /// Set the value of the cell at the specified position. This method must call OnValueChanged() event.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="value">The value.</param>
    void SetValue(Position position, object value);

    /// <summary>
    /// Gets the display text.
    /// </summary>
    /// <remarks>
    /// This is the string representation of the Cell.GetValue method (default Value.ToString())
    /// </remarks>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    string GetDisplayText(Position position);
    #endregion

    #region VisualProperties
    /// <summary>
    /// Gets or sets the visual properties of this cell and other cell. You can share the
    /// VisualProperties between many cells to optimize memory use.
    /// </summary>
    /// <remarks>
    /// Warning: Changing this property can affect many cells.
    /// </remarks>
    /// <value>The visual model.</value>
    [Browsable(false)]
    VisualModels.IVisualModel VisualModel
    {
      get;
      set;
    }
    #endregion

    #region CanReceiveFocus
    /// <summary>
    /// Gets a value indicating whether the current cell can receive the focus.
    /// This method simply calls BehaviorModel.CanReceiveFocus.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance can receive focus; otherwise, <c>false</c>.
    /// </value>
    bool CanReceiveFocus
    {
      get;
    }
    #endregion

    #region CalculateRequiredSize
    /// <summary>
    /// If the cell is not linked to a grid the result is not accurate (Font can be null). Call InternalGetRequiredSize with RowSpan and ColSpan = 1.
    /// </summary>
    /// <param name="position">Position of the current cell</param>
    /// <param name="g">The GDI+ graphics surface</param>
    /// <returns></returns>
    Size CalculateRequiredSize(Position position, Graphics g);
    #endregion

    #region Editing
    /// <summary>
    /// Gets or sets the data model.
    /// </summary>
    /// <value>The data model.</value>
    /// <remarks>
    /// Editor of this cell and others cells. If null no edit is supported.
    /// You can share the same model between many cells to optimize memory size. Warning Changing this property can affect many cells
    /// </remarks>
    DataModels.IDataModel DataModel
    {
      get;
      set;
    }

    /// <summary>
    /// Start the edit operation with the current editor specified in the Model property.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_NewStartEditValue">The value that the editor receive. Null to use the Value of the Cell.</param>
    void StartEdit(Position position, object p_NewStartEditValue);

    /// <summary>
    /// Terminate the edit operation
    /// </summary>
    /// <param name="p_bCancel">If true undo all the changes</param>
    /// <returns>
    /// Returns true if the edit operation is successfully terminated, otherwise false
    /// </returns>
    bool EndEdit(bool p_bCancel);

    /// <summary>
    /// True if this cell is currently in edit state; otherwise <c>false</c>.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    ///   <c>true</c> if the specified position is editing; otherwise, <c>false</c>.
    /// </returns>
    bool IsEditing(Position position);
    #endregion

    #region Events
    /// <summary>
    /// Raised when a context menu is shown
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionContextMenuEventArgs"/> instance containing the event data.</param>
    void OnContextMenuPopUp(PositionContextMenuEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:MouseDown"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    void OnMouseDown(PositionMouseEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:MouseUp"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    void OnMouseUp(PositionMouseEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:MouseMove"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    void OnMouseMove(PositionMouseEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:MouseEnter"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnMouseEnter(PositionEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:MouseLeave"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnMouseLeave(PositionEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:KeyUp"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    void OnKeyUp(PositionKeyEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:KeyDown"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    void OnKeyDown(PositionKeyEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:KeyPress"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyPressEventArgs"/> instance containing the event data.</param>
    void OnKeyPress(PositionKeyPressEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:DoubleClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnDoubleClick(PositionEventArgs e);

    /// <summary>
    /// Raises the <see cref="E:Click"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnClick(PositionEventArgs e);

    /// <summary>
    /// Fired before the cell leave the focus, you can put the e.Cancel = true to cancel the leave operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnFocusLeaving(PositionCancelEventArgs e);

    /// <summary>
    /// Fired when the cell has left the focus.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnFocusLeft(PositionEventArgs e);

    /// <summary>
    /// Fired when the focus is entering in the specified cell. You can put the e.Cancel = true to cancel the focus operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnFocusEntering(PositionCancelEventArgs e);

    /// <summary>
    /// Fired when the focus enter in the specified cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnFocusEntered(PositionEventArgs e);

    /// <summary>
    /// Fired when the SetValue method is called.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnValueChanged(PositionEventArgs e);

    /// <summary>
    /// Fired when the StartEdit is called. You can set the Cancel = true to stop editing.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnEditStarting(PositionCancelEventArgs e);

    /// <summary>
    /// Fired when the EndEdit is called. You can read the Cancel property to determine if the edit is completed. If you change the cancel property there is no effect.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnEditEnded(PositionCancelEventArgs e);
    #endregion

    #region BehaviorModel
    /// <summary>
    /// Gets BehaviorModels defining the actions that a cell can exhibit.
    /// </summary>
    /// <value>The behaviors.</value>
    IList<IBehaviorModel> BehaviorModels
    {
      get;
    }
    #endregion

    #region Invalidate
    /// <summary>
    /// Invalidate the specified position
    /// </summary>
    /// <param name="position">The position.</param>
    void Invalidate(Position position);
    #endregion
  }
}