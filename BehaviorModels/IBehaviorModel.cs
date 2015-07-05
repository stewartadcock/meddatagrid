#region MIT License
//
// Filename: IBehaviorModel.cs
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

using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// Represents a behaviour of a cell.
  /// </summary>
  [ComVisible(false)]
  public interface IBehaviorModel
  {
    /// <summary>
    /// Raised when a context menu is shown.
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
    /// Raised before the cell leave the focus, you can put the e.Cancel = true to cancel the leave operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnFocusLeaving(PositionCancelEventArgs e);

    /// <summary>
    /// Raised when the cell has left the focus.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnFocusLeft(PositionEventArgs e);

    /// <summary>
    /// Raised when the focus is entering in the specified cell. You can put the e.Cancel = true to cancel the focus operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnFocusEntering(PositionCancelEventArgs e);

    /// <summary>
    /// Raised when the focus enter in the specified cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnFocusEntered(PositionEventArgs e);

    /// <summary>
    /// Raised when the SetValue method is called.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    void OnValueChanged(PositionEventArgs e);

    /// <summary>
    /// Raised when the StartEdit is called. You can set the Cancel = true to stop editing.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnEditStarting(PositionCancelEventArgs e);

    /// <summary>
    /// Raised when the EndEdit is called. You can read the Cancel property to determine if the edit is completed. If you change the cancel property there is no effect.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    void OnEditEnded(PositionCancelEventArgs e);

    /// <summary>
    /// Gets a value indicating whether the current cell can receive the focus.
    /// </summary>
    /// <remarks>
    /// If any behavior return false the return value is false.
    /// </remarks>
    /// <value>
    ///   <c>true</c> if this instance can receive focus; otherwise, <c>false</c>.
    /// </value>
    bool CanReceiveFocus
    {
      get;
    }
  }
}