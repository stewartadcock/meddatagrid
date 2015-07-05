#region MIT License
//
// Filename: BehaviorModelGroup.cs
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// A behaviour model with a collection of child models (SubModels).
  /// </summary>
  /// <remarks>
  /// This can be used to nest a list of behaviour models.
  /// </remarks>
  [ComVisible(false)]
  public class BehaviorModelGroup : IBehaviorModel
  {
    private List<IBehaviorModel> subModels = new List<IBehaviorModel>();

    #region Public methods
    /// <summary>
    /// Fires the popup event when the popup must be displayed upon a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionContextMenuEventArgs"/> instance containing the event data.</param>
    public virtual void OnContextMenuPopUp(PositionContextMenuEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnContextMenuPopUp(e);
      }
    }

    /// <summary>
    /// Fires the mouse down event when the mouse button is down in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseDown(PositionMouseEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnMouseDown(e);
      }
    }

    /// <summary>
    /// Fires the mouse up event when the mouse button is up in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseUp(PositionMouseEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnMouseUp(e);
      }
    }

    /// <summary>
    /// Fires the mouse move event when the mouse moves in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseMove(PositionMouseEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnMouseMove(e);
      }
    }

    /// <summary>
    /// Fires the mouse enter event when the mouse enters in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseEnter(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnMouseEnter(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseLeave"/> event.
    /// </summary>
    /// <remarks>
    /// Raised when the mouse leaves a specified cell position
    /// </remarks>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseLeave(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnMouseLeave(e);
      }
    }

    /// <summary>
    /// Fires the key up event when a given key is up in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyUp(PositionKeyEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnKeyUp(e);
      }
    }

    /// <summary>
    /// Fires the key down event when a given key is down in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyDown(PositionKeyEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnKeyDown(e);
      }
    }

    /// <summary>
    /// Fires the key press event when a given key is pressed in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyPressEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyPress(PositionKeyPressEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnKeyPress(e);
      }
    }

    /// <summary>
    /// Firse the double click event produced on a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnDoubleClick(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnDoubleClick(e);
      }
    }

    /// <summary>
    /// Fires the click event on a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnClick(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnClick(e);
      }
    }

    /// <summary>
    /// Fires the focus leaving event on a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusLeaving(PositionCancelEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnFocusLeaving(e);
      }
    }

    /// <summary>
    /// Fires the focus left event on a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusLeft(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnFocusLeft(e);
      }
    }

    /// <summary>
    /// Fires the focus entering in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusEntering(PositionCancelEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnFocusEntering(e);
      }
    }

    /// <summary>
    /// Fires the focus entered event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusEntered(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnFocusEntered(e);
      }
    }

    /// <summary>
    /// Fires the value changed event of a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnValueChanged(PositionEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnValueChanged(e);
      }
    }

    /// <summary>
    /// Fires the edit starting event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditStarting(PositionCancelEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnEditStarting(e);
      }
    }

    /// <summary>
    /// Fires the edit ended event in a specified cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditEnded(PositionCancelEventArgs e)
    {
      for (int i = 0; i < subModels.Count; i++)
      {
        subModels[i].OnEditEnded(e);
      }
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the collection of behavior models of the current cell
    /// </summary>
    /// <value>The sub models.</value>
    public IList<IBehaviorModel> SubModels
    {
      get { return (IList<IBehaviorModel>)this.subModels; }
    }

    /// <summary>
    /// Gets a value indicating whether the current cell can receive the focus.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance can receive focus; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// If one or more behaviours return <c>false</c> the return value is <c>false</c>.
    /// This method simply calls <see cref="IBehaviorModel.CanReceiveFocus"/> for each behaviour.
    /// </remarks>
    public virtual bool CanReceiveFocus
    {
      get
      {
        for (int i = 0; i < subModels.Count; i++)
        {
          if (subModels[i].CanReceiveFocus == false)
          {
            return false;
          }
        }

        return true;
      }
    }
    #endregion
  }
}