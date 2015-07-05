#region MIT License
//
// Filename: Cell.cs
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// Represents a Cell in a grid, with Cell.Value support and row/col span.
  /// Also supports ToolTipText, ContextMenu and Cursor.
  /// </summary>
  [ComVisible(false)]
  public class Cell : Virtual.CellVirtual, ICell, ICellToolTipText, ICellCursor, ICellContextMenu
  {
    private object cellValue = null;
    private ICell mergedCell;
    private bool isLastExpandedCell;
    private Cursor cursor;
    private Range range = Range.Empty; // the position is Range.Start
    private string toolTipText;

    /// <summary>
    /// To optimize creation of the cell collections are only instatiated when needed.
    /// </summary>
    private List<MenuItem> contextMenuItems = null;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    public Cell()
      : this(null)
    {
      // Do nothing
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    /// <param name="value">The value of the cell</param>
    public Cell(object value)
    {
      Value = value;
      VisualModel = VisualModels.Common.Default;
      BehaviorModels.Add(BehaviorModel.ToolTipTextBehaviorModel.Default);
      BehaviorModels.Add(BehaviorModel.CursorBehaviorModel.Default);
      BehaviorModels.Add(BehaviorModel.ContextMenuBehaviorModel.Default);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// Create a cell with an editor using the type specified. (using Utility.CreateCellModel).
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_Type">Type of the cell</param>
    public Cell(object value, Type p_Type)
      : this(value)
    {
      DataModel = DataModels.DataModelFactory.CreateDataModel(p_Type);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    /// <param name="value">The value of the cell</param>
    /// <param name="p_Editor">The editor of this cell</param>
    public Cell(object value, DataModels.IDataModel p_Editor)
      : this(value)
    {
      DataModel = p_Editor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    /// <param name="value">Initial value of the cell.</param>
    /// <param name="p_Editor">Formatters used for string conversion, if null is used a shared default formatter.</param>
    /// <param name="p_VisualModel">Visual properties of the current cell, if null is used a shared default properties.</param>
    public Cell(object value, DataModels.IDataModel p_Editor, VisualModels.IVisualModel p_VisualModel)
      : this(value)
    {
      DataModel = p_Editor;

      if (p_VisualModel != null)
      {
        VisualModel = p_VisualModel;
      }
    }
    #endregion

    #region Cell Data (Value, DisplayText, Tag)
    /// <summary>
    /// Get the value of the current cell
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    /// The value of the cell at the specified position
    /// </returns>
    public override object GetValue(Position position)
    {
      return this.cellValue;
    }

    /// <summary>
    /// Set the value of the cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="value">The value.</param>
    /// <remarks>
    /// It is strongly recommended that any actions initiated by the user interface do not
    /// modify this property. Instead, call the CellEditor.ChangeCellValue to preserve
    /// data consistency.
    /// </remarks>
    public override void SetValue(Position position, object value)
    {
      // Do nothing if the values match, with special-case code for common string case.
      if ((this.cellValue == value)
         || (value is string && this.cellValue is string && ((string)value).CompareTo((string)this.cellValue) == 0))
      {
        return;
      }

      this.cellValue = value;

      OnValueChanged(new PositionEventArgs(position, this));
    }

    /// <summary>
    /// Gets the display text.
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns></returns>
    /// <remarks>
    /// This is the string representation of the Cell.GetValue method (default Value.ToString())
    /// </remarks>
    public override string GetDisplayText(Position position)
    {
      return this.DisplayText;
    }

    /// <summary>
    /// Gets the string representation of the Cell.Value property (default Value.ToString())
    /// </summary>
    /// <value>The display text.</value>
    public virtual string DisplayText
    {
      get
      {
        return base.GetDisplayText(range.Start);
      }
    }

    /// <summary>
    /// Gets or sets the cell value.
    /// </summary>
    /// <value>The value.</value>
    /// <remarks>
    /// It is recommended that no actions initiated by the user interface modify this property.
    /// Instead call the CellEditor.ChangeCellValue to preserve data consistency.
    /// </remarks>
    public virtual object Value
    {
      get { return this.cellValue; }
      set { SetValue(this.range.Start, value); }
    }

    /// <summary>
    /// Gets or sets whether this is a merged cell.
    /// </summary>
    /// <value>The merged cell.</value>
    public ICell MergedCell
    {
      get { return this.mergedCell; }
      set { this.mergedCell = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is last expanded cell.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is last expanded cell; otherwise, <c>false</c>.
    /// </value>
    public bool IsLastExpandedCell
    {
      get { return this.isLastExpandedCell; }
      set { this.isLastExpandedCell = value; }
    }
    #endregion

    #region ToString()
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString()
    {
      return DisplayText;
    }
    #endregion

    #region LinkToGrid
    /// <summary>
    /// Link the cell to the specified grid.
    /// For Cell derived classes you must call BindToGrid(Grid p_grid, Position position).
    /// </summary>
    /// <param name="p_grid">The grid.</param>
    public override void BindToGrid(GridVirtual p_grid)
    {
      throw new MEDDataGridException("Invalid method for this class, use BindToGrid(Grid p_grid, Position position)");
    }

    /// <summary>
    /// Link the cell to the specified grid.
    /// </summary>
    /// <remarks>
    /// To insert a cell in a grid use Grid.InsertCell, this method is for internal use only
    /// </remarks>
    /// <param name="grid">The grid.</param>
    /// <param name="position">The position.</param>
    public virtual void BindToGrid(Grid grid, Position position)
    {
      range.MoveTo(position);
      base.BindToGrid(grid);
      RefreshSpanSearch();
    }

    /// <summary>
    /// Remove the link of the cell from the previous grid.
    /// </summary>
    /// <remarks>
    /// To remove a cell from a grid use the grid.RemoveCell, this method is for internal use only
    /// </remarks>
    public override void UnBindToGrid()
    {
      range.MoveTo(Position.Empty);
      base.UnBindToGrid();
    }
    #endregion

    #region Range and Position
    /// <summary>
    /// Gets the current range.
    /// </summary>
    /// <remarks>
    /// Returns the current Row and Column position. If this cell is not attached to the grid returns Position.Empty. And the range occupied by the current cell.
    /// Returns the Range of the cells occupied by the current cell. If RowSpan and ColSpan = 1 then this method returns a single cell.
    /// </remarks>
    /// <value>The range.</value>
    public Range Range
    {
      get { return this.range; }
    }

    /// <summary>
    /// Gets the index of the current row
    /// </summary>
    /// <value>The row.</value>
    public int Row
    {
      get { return this.range.Start.Row; }
    }

    /// <summary>
    /// Gets the index of the current column
    /// </summary>
    /// <value>The column.</value>
    public int Column
    {
      get { return this.range.Start.Column; }
    }
    #endregion

    #region Row/Col Span
    /// <summary>
    /// Refreshes the span search.
    /// </summary>
    private void RefreshSpanSearch()
    {
      if (this.Grid is Grid)
      {
        Grid l_grid = (Grid)this.Grid;
        l_grid.SetMaxSpanSearch(Math.Max(ColumnSpan, RowSpan) - 1, false);
      }
    }

    /// <summary>
    /// Gets or sets the column span for merge operation, calculated using the current range.
    /// </summary>
    /// <value>The column span.</value>
    public int ColumnSpan
    {
      get
      {
        return range.ColumnsCount;
      }
      set
      {
        range.ColumnsCount = value;

        RefreshSpanSearch();
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets RowSpan for merge operation, calculated using the current range.
    /// </summary>
    /// <value>The row span.</value>
    public int RowSpan
    {
      get
      {
        return range.RowsCount;
      }
      set
      {
        range.RowsCount = value;
        RefreshSpanSearch();
        Invalidate();
      }
    }

    /// <summary>
    /// Returns true if the position specified is inside the current cell range (use Range.Contains)
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    ///   <c>true</c> if the cell contains the specified position; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool ContainsPosition(Position position)
    {
      return range.Contains(position);
    }
    #endregion

    #region CalculateRequiredSize
    /// <summary>
    /// If the cell is not linked to a grid the result is not accurate (Font can be null).
    /// This method uses RowSpan and ColSpan of the current Cell (InternalGetRequiredSize).
    /// </summary>
    /// <param name="position">Position of the current cell</param>
    /// <param name="graphics">The GDI+ graphics context.</param>
    /// <returns></returns>
    public override Size CalculateRequiredSize(Position position, Graphics graphics)
    {
      // override base CalculateRequiredSize to use correct RowSpan and ColumnSpan
      return InternalCalculateRequiredSize(position, graphics, RowSpan, ColumnSpan);
    }
    #endregion

    #region Selection
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ICell"/> is selected.
    /// </summary>
    /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
    public bool Select
    {
      get
      {
        if (Grid != null)
        {
          return Grid.Selection.Contains(range.Start);
        }
        else
        {
          return false;
        }
      }
      set
      {
        if (Select != value && Grid != null)
        {
          if (value)
          {
            Grid.Selection.Add(range.Start);
          }
          else
          {
            Grid.Selection.Remove(range.Start);
          }
        }
      }
    }
    #endregion

    #region Focus
    /// <summary>
    /// Gets a value indicating whether the has the focus
    /// </summary>
    /// <value><c>true</c> if focused; otherwise, <c>false</c>.</value>
    public bool Focused
    {
      get
      {
        if (Grid != null)
        {
          return Grid.FocusCellPosition == range.Start;
        }
        else
        {
          return false;
        }
      }
    }

    /// <summary>
    /// Give the focus at the cell
    /// </summary>
    /// <returns>Returns if the cell can receive the focus</returns>
    public bool Focus()
    {
      if (Grid != null)
      {
        return Grid.SetFocusCell(range.Start);
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Remove the focus from the cell
    /// </summary>
    /// <returns>Returns true if the cell can leave the focus otherwise false</returns>
    public bool LeaveFocus()
    {
      if (Grid != null && Focused)
      {
        return Grid.SetFocusCell(Position.Empty);
      }
      else
      {
        return true;
      }
    }
    #endregion

    #region Editing
    /// <summary>
    /// True if this cell is currently in edit state; otherwise <c>false</c>.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is editing; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEditing()
    {
      return IsEditing(range.Start);
    }

    /// <summary>
    /// Start the edit operation with the current editor specified in the Model property. Using the current cell position.
    /// </summary>
    /// <param name="position">Not used with this type of class.</param>
    /// <param name="p_NewStartEditValue">The value that the editor receive</param>
    public override void StartEdit(Position position, object p_NewStartEditValue)
    {
      base.StartEdit(range.Start, p_NewStartEditValue);
    }
    #endregion

    #region Invalidate
    /// <summary>
    /// Invalidate this cell
    /// </summary>
    public override void Invalidate()
    {
      if (Grid != null)
      {
        Grid.InvalidateRange(Range);
      }
    }
    #endregion

    #region ToolTipText
    /// <summary>
    /// Gets or sets the ToolTip Text for the current cell
    /// </summary>
    /// <value>The tool tip text.</value>
    public virtual string ToolTipText
    {
      get { return this.toolTipText; }
      set { this.toolTipText = value; }
    }

    /// <summary>
    /// Returns the tooltip text for the current cell position. Returns the ToolTipText property.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual string GetToolTipText(Position position)
    {
      return this.toolTipText;
    }
    #endregion

    #region ICellContextMenu
    /// <summary>
    /// Gets or sets Context Menu items.
    /// </summary>
    /// <value>The context menu items.</value>
    public List<MenuItem> ContextMenuItems
    {
      get
      {
        if (this.contextMenuItems == null)
        {
          this.contextMenuItems = new List<MenuItem>();
        }
        return this.contextMenuItems;
      }
      set
      {
        this.contextMenuItems = value;
      }
    }

    /// <summary>
    /// Get the context menu items of the specified cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public List<MenuItem> GetContextMenu(Position position)
    {
      return this.contextMenuItems;
    }
    #endregion

    #region ICellCursor Members
    /// <summary>
    /// Gets or sets Cursor of the cell
    /// </summary>
    /// <value>The cursor.</value>
    public Cursor Cursor
    {
      get { return cursor; }
      set { cursor = value; }
    }

    /// <summary>
    /// Get the Cursor property.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual Cursor GetCursor(Position position)
    {
      return cursor;
    }
    #endregion
  }
}