#region MIT License
//
// Filename: CellVirtual.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

using Fr.Fc.FcCore.Logging;

using Fr.Medit.MedDataGrid.BehaviorModel;

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  /// <summary>
  /// Represents a CellVirtual in a grid.
  /// </summary>
  [ComVisible(false)]
  public abstract class CellVirtual : ICellVirtual
  {
    #region Class variables
    private GridVirtual ownerGrid;
    private VisualModels.IVisualModel visualModel;
    private DataModels.IDataModel cellModel = null;
    private List<IBehaviorModel> behaviorModels = new List<IBehaviorModel>();
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="CellVirtual"/> class.
    /// </summary>
    protected CellVirtual()
      : this(null, VisualModels.Common.Default)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CellVirtual"/> class.
    /// Create a cell with an editor using the type specified. (using Utility.CreateCellModel).
    /// </summary>
    /// <param name="p_Type">Type of the cell</param>
    protected CellVirtual(Type p_Type)
      : this(DataModels.DataModelFactory.CreateDataModel(p_Type))
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CellVirtual"/> class.
    /// </summary>
    /// <param name="p_Editor">The editor.</param>
    protected CellVirtual(DataModels.IDataModel p_Editor)
      : this(p_Editor, VisualModels.Common.Default)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CellVirtual"/> class.
    /// </summary>
    /// <param name="p_Editor">Formatters used for string conversion, if null is used a shared default formatter.</param>
    /// <param name="p_VisualModel">Visual properties of the current cell, if null is used a shared default properties.</param>
    protected CellVirtual(DataModels.IDataModel p_Editor, VisualModels.IVisualModel p_VisualModel)
    {
      DataModel = p_Editor;

      if (p_VisualModel != null)
      {
        VisualModel = p_VisualModel;
      }
      else
      {
        VisualModel = VisualModels.Common.Default;
      }

      BehaviorModels.Add(BehaviorModel.CommonBehaviorModel.Default);
    }
    #endregion

    #region Format
    /// <summary>
    /// Gets or sets the font.
    /// </summary>
    /// <remarks>
    /// If null, the default font is used
    /// </remarks>
    /// <value>The font.</value>
    public Font Font
    {
      get
      {
        return this.visualModel.Font;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.Font = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets the color of the cell background.
    /// </summary>
    /// <value>The color of the background.</value>
    public Color BackColor
    {
      get
      {
        return this.visualModel.BackColor;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.BackColor = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets the color of the cell foreground.
    /// </summary>
    /// <value>The color of the foreground.</value>
    public Color ForeColor
    {
      get
      {
        return this.visualModel.ForeColor;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.ForeColor = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets the normal border of a cell
    /// </summary>
    /// <value>The border.</value>
    public RectangleBorder Border
    {
      get
      {
        return this.visualModel.Border;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.Border = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to apply word wrapping.
    /// This property is only a wrapper around StringFormat
    /// </summary>
    /// <value><c>true</c> if word wrap is enabled; otherwise, <c>false</c>.</value>
    public bool WordWrap
    {
      get
      {
        return this.visualModel.WordWrap;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.WordWrap = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets the text alignment.
    /// This property is only a wrapper around StringFormat
    /// </summary>
    /// <value>The text alignment.</value>
    public ContentAlignment TextAlignment
    {
      get
      {
        return this.visualModel.TextAlignment;
      }
      set
      {
        VisualModels.IVisualModel l_Model = (VisualModels.IVisualModel)this.visualModel.Clone(false);
        l_Model.TextAlignment = value;
        l_Model.MakeReadOnly();
        VisualModel = l_Model;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this is an expanded cell.
    /// An expanded cell has lines of expanded nodes as in a treeview.
    /// </summary>
    /// <value><c>true</c> if cell is an expanded cell; otherwise, <c>false</c>.</value>
    public bool ExpandedCell
    {
      get { return visualModel.ExpandedCell; }
      set { visualModel.ExpandedCell = value; }
    }
    #endregion

    #region LinkToGrid
    /// <summary>
    /// Gets the Grid object
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get { return this.ownerGrid; }
    }

    /// <summary>
    /// Link the cell to the specified grid.
    /// </summary>
    /// <param name="grid">The grid.</param>
    public virtual void BindToGrid(GridVirtual grid)
    {
      this.ownerGrid = grid;
      OnAddToGrid(EventArgs.Empty);
    }

    /// <summary>
    /// Remove the link of the cell from the previous grid.
    /// </summary>
    public virtual void UnBindToGrid()
    {
      if (this.ownerGrid != null)
      { // remove the cell from the grid previously selected
        OnRemoveToGrid(EventArgs.Empty);
      }

      this.ownerGrid = null;
    }

    /// <summary>
    /// Fired when the cell is added to a grid
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnAddToGrid(EventArgs e)
    {
    }

    /// <summary>
    /// Fired before a cell is removed from a grid
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnRemoveToGrid(EventArgs e)
    {
    }
    #endregion

    #region GetValue, SetValue (ABSTRACT)
    /// <summary>
    /// Get the value of the cell at the specified position
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>The value of the cell at the specified position</returns>
    public abstract object GetValue(Position position);

    /// <summary>
    /// Set the value of the cell at the specified position. This method must call OnValueChanged() event.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="value">The value.</param>
    public abstract void SetValue(Position position, object value);
    #endregion

    #region GetDisplayString
    /// <summary>
    /// Gets the display text.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    /// <remarks>
    /// This is the string representation of the Cell.GetValue method (default Value.ToString())
    /// </remarks>
    public virtual string GetDisplayText(Position position)
    {
      try
      {
        object cellValue = GetValue(position);
        if (this.cellModel == null)
        {
          // return cellValue == null ? string.Empty : cellValue.ToString();
          if (cellValue == null)
          {
            return string.Empty;
          }
          else if (cellValue is double)
          {
            return ((double)cellValue).ToString(CultureInfo.InvariantCulture);
          }
          else
          {
            return cellValue.ToString();
          }
        }
        else
        {
          return this.cellModel.ValueToDisplayString(cellValue);
        }
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Unexpected exception caught: " + ex.ToString());
        return "Error:" + ex.Message;
      }
    }
    #endregion

    #region VisualModel
    /// <summary>
    /// Gets or sets the visual properties of this cell and other cell. You can share the
    /// VisualProperties between many cells to optimize memory use.
    /// </summary>
    /// <value>The visual model.</value>
    /// <remarks>
    /// Visual properties of this cell and other cell. You can share the VisualProperties between many cell to optimize memory size.
    /// Warning: Changing this property can affect many cells.
    /// </remarks>
    [Browsable(false)]
    public virtual VisualModels.IVisualModel VisualModel
    {
      get
      {
        return this.visualModel;
      }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }

        visualModel = value;

        ////Invalidate();
      }
    }
    #endregion

    #region CanReceiveFocus
    /// <summary>
    /// Gets a value indicating whether the current cell can receive the focus.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance can receive focus; otherwise, <c>false</c>.
    /// </value>
    /// <returns></returns>
    public virtual bool CanReceiveFocus
    {
      get
      {
        for (int i = 0; i < behaviorModels.Count; i++)
        {
          if (behaviorModels[i].CanReceiveFocus == false)
          {
            return false;
          }
        }

        return true;
      }
    }
    #endregion

    #region GetRequiredSize
    /// <summary>
    /// If the cell is not linked to a grid the result is not accurate (Font can be null). Call InternalGetRequiredSize with RowSpan and ColSpan = 1.
    /// </summary>
    /// <param name="position">Position of the current cell</param>
    /// <param name="g">The GDI+ graphics surface</param>
    /// <returns></returns>
    public virtual Size CalculateRequiredSize(Position position, Graphics g)
    {
      return InternalCalculateRequiredSize(position, g, 1, 1);
    }

    /// <summary>
    /// If the cell is not linked to a grid the result is not accurate (Font can be null)
    /// </summary>
    /// <param name="position">Position of the current cell</param>
    /// <param name="g">The graphics.</param>
    /// <param name="p_RowSpan">The row span.</param>
    /// <param name="p_ColSpan">The column span.</param>
    /// <returns></returns>
    protected virtual Size InternalCalculateRequiredSize(Position position, Graphics g, int p_RowSpan, int p_ColSpan)
    {
      SizeF l_ReqSize = VisualModel.GetRequiredSize(g, this, position);

      // Approximate the width and Height value if ColSpan or RowSpan are grater than 1
      l_ReqSize.Width = l_ReqSize.Width / (float)p_ColSpan;
      l_ReqSize.Height = l_ReqSize.Height / (float)p_RowSpan;

      return l_ReqSize.ToSize();
    }
    #endregion

    #region Editing
    /// <summary>
    /// Gets or sets editor of this cell and other cells. If null no edit is supported.
    /// You can share the same model between many cells to optimize memory size. Warning Changing this property can affect many cells
    /// </summary>
    /// <value>The data model.</value>
    public DataModels.IDataModel DataModel
    {
      get { return this.cellModel; }
      set { this.cellModel = value; }
    }

    /// <summary>
    /// Start the edit operation with the current editor specified in the Model property.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_NewStartEditValue">The value that the editor receive</param>
    public virtual void StartEdit(Position position, object p_NewStartEditValue)
    {
      if (this.ownerGrid != null &&
        this.cellModel != null &&
        IsEditing(position) == false &&  //se la cella non è già in stato edit
        this.cellModel.EnableEdit == true &&
        this.ownerGrid.SetFocusCell(position)) //per finire eventuali altri edit e posizionare il focus su questa cella
      {
        PositionCancelEventArgs l_EventArgs = new PositionCancelEventArgs(position, this);
        OnEditStarting(l_EventArgs);
        if (l_EventArgs.Cancel == false)
        {
          this.cellModel.InternalStartEdit(this, position, p_NewStartEditValue);
        }
      }
    }

    /// <summary>
    /// Start the edit operation with the current editor specified in the Model property.
    /// </summary>
    /// <param name="position">The position.</param>
    public void StartEdit(Position position)
    {
      StartEdit(position, null);
    }

    /// <summary>
    /// Terminate the edit operation
    /// </summary>
    /// <param name="isCancelled">If true undo all the changes</param>
    /// <returns>
    /// Returns true if the edit operation is successfully terminated, otherwise false
    /// </returns>
    public bool EndEdit(bool isCancelled)
    {
      if (this.cellModel == null || !this.cellModel.IsEditing)
      {
        return true;
      }

      Position l_Position = this.cellModel.EditPosition;
      bool l_Success = this.cellModel.InternalEndEdit(isCancelled);
      if (l_Success)
      {
        PositionCancelEventArgs eventArgs = new PositionCancelEventArgs(l_Position, this);
        eventArgs.Cancel = isCancelled;
        OnEditEnded(eventArgs);
      }
      return l_Success;
    }

    /// <summary>
    /// True if this cell is currently in edit state; otherwise <c>false</c>.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    ///   <c>true</c> if the specified position is editing; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsEditing(Position position)
    {
      return this.cellModel != null
        && this.cellModel.IsEditing == true
        && this.cellModel.EditCell == this
        && this.cellModel.EditPosition == position;
    }

    /// <summary>
    /// Gets or sets a value indicating whether editting is enabled.
    /// </summary>
    /// <value><c>true</c> if editting is enabled; otherwise, <c>false</c>.</value>
    public virtual bool EnableEdit
    {
      get
      {
        return this.cellModel != null && this.cellModel.EnableEdit;
      }
      set
      {
        if (this.cellModel != null)
        {
          this.cellModel.EnableEdit = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the editable mode.
    /// </summary>
    /// <value>The editable mode.</value>
    public virtual EditableModes EditableMode
    {
      get
      {
        return this.cellModel == null ? 0 : this.cellModel.EditableMode;
      }
      set
      {
        if (this.cellModel != null)
        {
          this.cellModel.EditableMode = value;
        }
      }
    }
    #endregion

    #region Events
    /// <summary>
    /// Raised when a context menu is shown
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionContextMenuEventArgs"/> instance containing the event data.</param>
    public virtual void OnContextMenuPopUp(PositionContextMenuEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnContextMenuPopUp(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseDown"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseDown(PositionMouseEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnMouseDown(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseUp"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseUp(PositionMouseEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnMouseUp(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseMove"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseMove(PositionMouseEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnMouseMove(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseEnter"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseEnter(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnMouseEnter(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:MouseLeave"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnMouseLeave(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnMouseLeave(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:KeyUp"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyUp(PositionKeyEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnKeyUp(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:KeyDown"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyDown(PositionKeyEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnKeyDown(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:KeyPress"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionKeyPressEventArgs"/> instance containing the event data.</param>
    public virtual void OnKeyPress(PositionKeyPressEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnKeyPress(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:DoubleClick"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnDoubleClick(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnDoubleClick(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:Click"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnClick(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnClick(e);
      }
    }

    /// <summary>
    /// Fired before the cell leave the focus, you can put the e.Cancel = true to cancel the leave operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusLeaving(PositionCancelEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnFocusLeaving(e);
      }
    }

    /// <summary>
    /// Fired when the cell has left the focus.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusLeft(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnFocusLeft(e);
      }
    }

    /// <summary>
    /// Fired when the focus is entering in the specified cell. You can put the e.Cancel = true to cancel the focus operation.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusEntering(PositionCancelEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnFocusEntering(e);
      }
    }

    /// <summary>
    /// Fired when the focus enter in the specified cell.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnFocusEntered(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnFocusEntered(e);
      }
    }

    /// <summary>
    /// Fired when the SetValue method is called.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public virtual void OnValueChanged(PositionEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnValueChanged(e);
      }
    }

    /// <summary>
    /// Fired when the StartEdit is called. You can set the Cancel = true to stop editing.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditStarting(PositionCancelEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnEditStarting(e);
      }
    }

    /// <summary>
    /// Fired when the EndEdit is called. You can read the Cancel property to determine if the edit is completed. If you change the cancel property there is no effect.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnEditEnded(PositionCancelEventArgs e)
    {
      for (int i = 0; i < behaviorModels.Count; i++)
      {
        behaviorModels[i].OnEditEnded(e);
      }
    }
    #endregion

    #region BehaviorModel
    /// <summary>
    /// Gets BehaviorModels. This represents the actions that a cell can perform.
    /// </summary>
    /// <value>The behaviors.</value>
    public IList<IBehaviorModel> BehaviorModels
    {
      get { return (IList<IBehaviorModel>)this.behaviorModels; }
    }
    #endregion

    #region Invalidate
    /// <summary>
    /// Invalidate this cell. For this type of class I must invalidate the whole grid, because I don't known the current cell position.
    /// </summary>
    public virtual void Invalidate()
    {
      if (this.ownerGrid != null)
      {
        this.ownerGrid.InvalidateCells();
      }
    }

    /// <summary>
    /// Invalidate the cell at the specified position.
    /// </summary>
    /// <param name="position">The position.</param>
    public virtual void Invalidate(Position position)
    {
      if (this.ownerGrid != null)
      {
        this.ownerGrid.InvalidateCell(position);
      }
    }
    #endregion
  }
}