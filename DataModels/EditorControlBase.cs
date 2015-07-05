#region MIT License
//
// Filename: EditorControlBase.cs
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

using Fr.Medit.MedDataGrid.Controls;

namespace Fr.Medit.MedDataGrid.DataModels
{
  /// <summary>
  /// The base class for all the editor that have a control
  /// </summary>
  [ComVisible(false)]
  public abstract class EditorControlBase : DataModelBase
  {
    private Guid guid = Guid.NewGuid(); // create a new key for each instance of the class
    public EditExceptionEventHandler EditException;

    #region Constructor
    /// <summary>
    /// Construct a Model. Based on the Type specified the Constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
    /// </summary>
    /// <param name="type">The type of this model</param>
    /// <exception cref="ArgumentNullException">Thrown is null Type argument passed.</exception>
    protected EditorControlBase(Type type)
      : base(type)
    {
    }
    #endregion

    #region Editor Control
    /// <summary>
    /// Returns the control attached to the specified grid, using ScrollablePanel.
    /// </summary>
    /// <remarks>
    /// This method can not be used to retrieve editor attached to the fixed panel. Use
    /// GetEditorControl(GridSubPanel p_GridPanel) if you want to attach the editor to a fixed panel.
    /// </remarks>
    /// <param name="grid">The grid where the control is attached.</param>
    /// <returns></returns>
    public virtual System.Windows.Forms.Control GetEditorControl(GridVirtual grid)
    {
      return GetEditorControl(grid.ScrollablePanel);
    }

    /// <summary>
    /// Returns the control attached to the specified grid panel.
    /// </summary>
    /// <param name="gridPanel">The grid panel.</param>
    /// <returns></returns>
    public virtual System.Windows.Forms.Control GetEditorControl(GridSubPanel gridPanel)
    {
      if (IsAttached(gridPanel))
      {
        return gridPanel.ControlsRepository[GetEditorControlGuid()];
      }
      else
      {
        throw new MEDDataGridException("Editor not attached to the grid, call AttachEditorControl first");
      }
    }

    //ATTENZIONE: Ho dovuto fare un sistema di chiavi per i controlli dell'edit perchÃ¨ altrimenti non riuscivo a fare uno share corretto degli editor.
    // questo perchÃ¨ se ad esempio aggiungo un controllo di edito a una griglia e questa griglia viene distrutta, viene chiamato il
    // dispose su tutti gli oggetti contenuti, questo di fatto distruggeva l'editor che invece era magari condiviso da più griglie

    /// <summary>
    /// Returns true if the control is atteched to the grid. This method use IsAttached(GridSubPanel p_GridPanel) with ScrollablePanel property.
    /// </summary>
    /// <param name="p_Grid">The grid to check whether the control is attached</param>
    /// <returns></returns>
    public virtual bool IsAttached(GridVirtual p_Grid)
    {
      return IsAttached(p_Grid.ScrollablePanel);
    }

    /// <summary>
    /// Returns true if the control is atteched to the grid panel.
    /// </summary>
    /// <param name="p_GridPanel">The grid to check whether the control is attached</param>
    /// <returns></returns>
    public virtual bool IsAttached(GridSubPanel p_GridPanel)
    {
      return p_GridPanel.ControlsRepository.ContainsKey(GetEditorControlGuid());
    }

    /// <summary>
    /// Add the current editor to the grid ScrollablePanel. If you want to attach the editor to
    /// another panel call AttachEditorControl(GridSubPanel p_GridPanel)
    /// </summary>
    /// <param name="p_Grid">The grid.</param>
    public virtual void AttachEditorControl(GridVirtual p_Grid)
    {
      AttachEditorControl(p_Grid.ScrollablePanel);
    }

    /// <summary>
    /// Add the current editor to the grid panel.
    /// </summary>
    /// <param name="p_GridPanel">The grid panel.</param>
    public virtual void AttachEditorControl(GridSubPanel p_GridPanel)
    {
      if (IsAttached(p_GridPanel) == false)
      {
        p_GridPanel.SuspendLayout();

        try
        {
          Control l_EditorControl = CreateEditorControl();
          l_EditorControl.Visible = false;
          p_GridPanel.ControlsRepository.Add(GetEditorControlGuid(), l_EditorControl);
          l_EditorControl.CreateControl();

          l_EditorControl.Validated += new EventHandler(InnerControl_Validated);
        }
        finally
        {
          p_GridPanel.ResumeLayout(true);
        }
      }
    }

    /// <summary>
    /// Remove the current editor from the grid control
    /// </summary>
    /// <param name="p_Grid">The grid.</param>
    public virtual void DetachEditorControl(GridVirtual p_Grid)
    {
      DetachEditorControl(p_Grid.ScrollablePanel);
    }

    /// <summary>
    /// Remove the current editor from the grid panel.
    /// </summary>
    /// <param name="p_GridPanel">The grid panel.</param>
    public virtual void DetachEditorControl(GridSubPanel p_GridPanel)
    {
      if (IsAttached(p_GridPanel))
      {
        Control l_EditorControl = GetEditorControl(p_GridPanel);

        l_EditorControl.Validated -= new EventHandler(InnerControl_Validated);

        // A .Net application can't close when a active control is removed from the
        // control collection so change the focus first.
        if (l_EditorControl.ContainsFocus)
        {
          p_GridPanel.Grid.SetFocusOnCells();
        }

        l_EditorControl.Hide();
        p_GridPanel.ControlsRepository.Remove(GetEditorControlGuid());
      }
    }
    #endregion

    /// <summary>
    /// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit.
    /// </summary>
    /// <param name="p_Cell">Cell to start edit</param>
    /// <param name="position">Editing position(Row/Col)</param>
    /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
    public override void InternalStartEdit(Cells.ICellVirtual p_Cell, Position position, object p_StartEditValue)
    {
      base.InternalStartEdit(p_Cell, position, p_StartEditValue);

      if (p_Cell == null)
      {
        throw new ArgumentNullException("p_Cell");
      }
      if (p_Cell.Grid == null)
      {
        throw new MEDDataGridException("Cell is not bounded to a grid");
      }
      if (p_Cell.Grid.FocusCellPosition != position)
      {
        p_Cell.Grid.SetFocusCell(position);
      }

      if (IsEditing == false && EnableEdit)
      {
        // Verify that the cell is still associated.
        if (EditCell != null)
        {
          throw new MEDDataGridException("There is already a Cell in edit state");
        }

        GridSubPanel l_Panel = p_Cell.Grid.PanelAtPosition(position);
        if (l_Panel == null)
        {
          throw new MEDDataGridException("Invalid Cell Position, panel not found");
        }
        AttachEditorControl(l_Panel);

        Control l_EditorControl = GetEditorControl(l_Panel);

        p_Cell.Grid.LinkedControls[l_EditorControl] = position;

        // update the position
        p_Cell.Grid.RefreshLinkedControlsBounds();

        l_EditorControl.Show();
        l_EditorControl.BringToFront();
        l_EditorControl.Focus();

        SetEditCell(p_Cell, position); // Start the edit

        ////p_Cell.Grid.InvalidateCell(position);
      }
    }

    /// <summary>
    /// Returns the GUID of the control that the current editor use
    /// </summary>
    /// <returns></returns>
    public virtual Guid GetEditorControlGuid()
    {
      return this.guid;
    }

    /// <summary>
    /// Create a new control used in this editor
    /// </summary>
    /// <returns></returns>
    public abstract Control CreateEditorControl();

    /// <summary>
    /// Apply edited value
    /// </summary>
    /// <returns></returns>
    public override bool InternalApplyEdit()
    {
      if (IsEditing != true || EnableEdit != true)
      {
        return true;
      }

      bool isSuccess;

      try
      {
        isSuccess = SetCellValue(EditCell, EditPosition, GetEditedValue());
      }
      catch (Exception ex)
      {
        // SAA TODO: Avoid catch of System.Exception only.
        OnEditException(new EditExceptionEventArgs(ex));
        isSuccess = false;
      }

      return isSuccess;
    }

    /// <summary>
    /// Terminate the edit action
    /// </summary>
    /// <param name="p_Cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
    /// <returns>Returns true if the cell terminate the editing mode</returns>
    public override bool InternalEndEdit(bool p_Cancel)
    {
      if (!IsEditing)
      {
        return true;
      }

      bool isSuccess = true;
      if (p_Cancel == false)
      {
        isSuccess = InternalApplyEdit();
      }

      if (isSuccess)
      {
        if (EditCell == null)
        {
          throw new InvalidOperationException();
        }
        else
        {
          GridVirtual l_Grid = EditCell.Grid;
          GridSubPanel l_Panel = l_Grid.PanelAtPosition(EditPosition);
          // In fact, setting this property to null logically ends the edit and it is
          // important that it is done as soon as possible (especially before the call
          // to SetFocusOnGridSubPanel is because otherwise this would call again EndEdit (false)
          SetEditCell(null, Position.Empty);

          Control l_EditorControl = null;
          if (IsAttached(l_Panel))
          {
            l_EditorControl = GetEditorControl(l_Panel);
          }
          if (l_EditorControl != null)
          {
            // If the control has the focus, put the focus on the cell to force an eventual
            // validation of that. If the control has the focus it should be validated already.
            if (l_Grid != null && l_EditorControl.ContainsFocus)
            {
              l_Grid.SetFocusOnCells();
            }
            l_Grid.LinkedControls.Remove(l_EditorControl);
            l_EditorControl.Hide();
          }
          else
          {
            throw new InvalidOperationException();
          }
        }
      }
      else // if the ApplyEdit failed
      {
        if (EditCell != null)
        {
          GridVirtual l_Grid = EditCell.Grid;
          GridSubPanel l_Panel = l_Grid.PanelAtPosition(EditPosition);
          if (IsAttached(l_Panel))
          {
            Control l_EditorControl = null;
            l_EditorControl = GetEditorControl(l_Panel);
            if (l_EditorControl != null && l_EditorControl.ContainsFocus == false)
            {
              l_EditorControl.Focus();
            }
          }
        }
      }

      return isSuccess;
    }

    /// <summary>
    /// Handles the Validated event of the InnerControl control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void InnerControl_Validated(object sender, EventArgs e)
    {
      try
      {
        if (IsEditing)
        {
          EditCell.EndEdit(false);
        }
      }
      catch (Exception ex)
      {
        OnEditException(new EditExceptionEventArgs(ex));
      }
    }

    /// <summary>
    /// Raises the <see cref="E:EditException"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.EditExceptionEventArgs"/> instance containing the event data.</param>
    protected virtual void OnEditException(EditExceptionEventArgs e)
    {
      if (EditException != null)
      {
        EditException(this, e);
      }
    }

    /// <summary>
    /// Returns the value inserted with the current editor control
    /// </summary>
    /// <returns></returns>
    public override abstract object GetEditedValue();
  }
}