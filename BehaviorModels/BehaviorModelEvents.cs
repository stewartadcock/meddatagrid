#region MIT License
//
// Filename: BehaviorModelEvents.cs
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

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// BehaviorModel Events
  /// </summary>
  public class BehaviorModelEvents : IBehaviorModel
  {
    #region IBehaviorModel Members
    /// <summary>
    /// Event representing the menu popup event
    /// </summary>
    public event PositionContextMenuEventHandler ContextMenuPopUp;

    /// <summary>
    /// Fires the context menu popup
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnContextMenuPopUp(PositionContextMenuEventArgs e)
    {
      if (ContextMenuPopUp != null)
      {
        ContextMenuPopUp(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse down event
    /// </summary>
    public event PositionMouseEventHandler MouseDown;

    /// <summary>
    /// Fires the mouse down event
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnMouseDown(PositionMouseEventArgs e)
    {
      if (MouseDown != null)
      {
        MouseDown(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse up event
    /// </summary>
    public event PositionMouseEventHandler MouseUp;

    /// <summary>
    /// Fires the mouse up event
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnMouseUp(PositionMouseEventArgs e)
    {
      if (MouseUp != null)
      {
        MouseUp(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse move event
    /// </summary>
    public event PositionMouseEventHandler MouseMove;

    /// <summary>
    /// Fires the mouse move event in a specified cell
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnMouseMove(PositionMouseEventArgs e)
    {
      if (MouseMove != null)
      {
        MouseMove(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse enter event
    /// </summary>
    public event PositionEventHandler MouseEnter;

    /// <summary>
    /// Fires the mouse enter event in a specified cell
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnMouseEnter(PositionEventArgs e)
    {
      if (MouseEnter != null)
      {
        MouseEnter(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse leave event
    /// </summary>
    public event PositionEventHandler MouseLeave;

    /// <summary>
    /// Fires the mouse leave event in a specified cell
    /// </summary>
    /// <param name="e">The specified cell</param>
    public void OnMouseLeave(PositionEventArgs e)
    {
      if (MouseLeave != null)
      {
        MouseLeave(this, e);
      }
    }

    /// <summary>
    /// Event representing the key up event
    /// </summary>
    public event PositionKeyEventHandler KeyUp;

    /// <summary>
    /// Fires the key up event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public void OnKeyUp(PositionKeyEventArgs e)
    {
      if (KeyUp != null)
      {
        KeyUp(this, e);
      }
    }

    /// <summary>
    /// Event representing the key down event
    /// </summary>
    public event PositionKeyEventHandler KeyDown;

    /// <summary>
    /// Fires the key down event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public void OnKeyDown(PositionKeyEventArgs e)
    {
      if (KeyDown != null)
      {
        KeyDown(this, e);
      }
    }

    /// <summary>
    /// Event representing the key press event
    /// </summary>
    public event PositionKeyPressEventHandler KeyPress;

    /// <summary>
    /// Fires the key press event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyPressEventArgs"/> instance containing the event data.</param>
    public void OnKeyPress(PositionKeyPressEventArgs e)
    {
      if (KeyPress != null)
      {
        KeyPress(this, e);
      }
    }

    /// <summary>
    /// Event representing the double click event
    /// </summary>
    public event PositionEventHandler DoubleClick;

    /// <summary>
    /// Fires the double click event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void OnDoubleClick(PositionEventArgs e)
    {
      if (DoubleClick != null)
      {
        DoubleClick(this, e);
      }
    }

    /// <summary>
    /// Event representing the mouse simple click event
    /// </summary>
    public event PositionEventHandler Click;

    /// <summary>
    /// Fires the mouse click event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void OnClick(PositionEventArgs e)
    {
      if (Click != null)
      {
        Click(this, e);
      }
    }

    /// <summary>
    /// Event representing the focus leaving event
    /// </summary>
    public event PositionCancelEventHandler FocusLeaving;

    /// <summary>
    /// Fires the focus leaving event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public void OnFocusLeaving(PositionCancelEventArgs e)
    {
      if (FocusLeaving != null)
      {
        FocusLeaving(this, e);
      }
    }

    /// <summary>
    /// Event representing the focus left event
    /// </summary>
    public event PositionEventHandler FocusLeft;

    /// <summary>
    /// Fires the focus left event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void OnFocusLeft(PositionEventArgs e)
    {
      if (FocusLeft != null)
      {
        FocusLeft(this, e);
      }
    }

    /// <summary>
    /// Event representing the focus entering event
    /// </summary>
    public event PositionCancelEventHandler FocusEntering;

    /// <summary>
    /// Fires the focus entering event
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public void OnFocusEntering(PositionCancelEventArgs e)
    {
      if (FocusEntering != null)
      {
        FocusEntering(this, e);
      }
    }

    /// <summary>
    /// Event representing the focus entered event
    /// </summary>
    public event PositionEventHandler FocusEntered;

    /// <summary>
    /// Fires the focus entered event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void OnFocusEntered(PositionEventArgs e)
    {
      if (FocusEntered != null)
      {
        FocusEntered(this, e);
      }
    }

    /// <summary>
    /// Event representing the value changed event
    /// </summary>
    public event PositionEventHandler ValueChanged;

    /// <summary>
    /// Fires the value changed event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public void OnValueChanged(PositionEventArgs e)
    {
      if (ValueChanged != null)
      {
        ValueChanged(this, e);
      }
    }

    /// <summary>
    /// Event representing the edit starting event
    /// </summary>
    public event PositionCancelEventHandler EditStarting;

    /// <summary>
    /// Fires the edit starting in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditStarting(PositionCancelEventArgs e)
    {
      if (EditStarting != null)
      {
        EditStarting(this, e);
      }
    }

    /// <summary>
    /// Event representing the edit ended event
    /// </summary>
    public event PositionCancelEventHandler EditEnded;

    /// <summary>
    /// Fires the edit ended event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditEnded(PositionCancelEventArgs e)
    {
      if (EditEnded != null)
      {
        EditEnded(this, e);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the cell can receive the focus.
    /// </summary>
    /// <value>
    /// Always <c>true</c>.
    /// </value>
    public bool CanReceiveFocus
    {
      get { return true; }
    }
    #endregion
  }
}