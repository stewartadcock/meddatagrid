#region MIT License
//
// Filename: GridVirtual.cs
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

using Fr.Fc.FcCore.Logging;

using Fr.Medit.MedDataGrid.Cells;
using Fr.Medit.MedDataGrid.Controls;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A Grid control that supports large virtual data.
  /// </summary>
  /// <remarks>
  /// The GetCell and SetCell methods must be overridden in a derived class, or
  /// the GettingCell and SettingCell handlers must be defined.
  /// </remarks>
  [System.ComponentModel.ToolboxItem(true)]
  public class GridVirtual : CustomScrollControl
  {
    #region Class variables
    // Panels
    private readonly GridSubPanel leftPanel;
    private readonly GridSubPanel topPanel;
    private readonly GridSubPanel topLeftPanel;
    private readonly GridSubPanel scrollablePanel;

    /// <summary>
    /// This is a hidden panel to manage the focus of the cell. The editor come inserted
    /// in the panel according to the position of the cells and therefore for being able
    /// to remove the focus from the cell it must move focus on a parallel control that
    /// is not parent cell editor.
    /// </summary>
    private readonly GridSubPanel hiddenFocusPanel;

    // Rows and columns.
    private readonly RowInfo.RowInfoCollection rows;
    private readonly ColumnInfo.ColumnInfoCollection columns;

    private readonly LinkedControlsList linkedControls = new LinkedControlsList();

    private readonly Selection selection;

    private int fixedRows = 0;
    private int fixedColumns = 0;

    private bool doAutoStretchColumnsToFitWidth = false;
    private bool doAutoStretchRowsToFitHeight = false;

    // Selection
    private Range mouseSelectionRange = Range.Empty;
    private Range oldMouseSelectionRange = Range.Empty;

    /// <summary>
    /// Whether sorting is enabled for this grid.
    /// </summary>
    private bool doEnableSort = true;

    /// <summary>
    /// Whether sorting is currently being performed.
    /// </summary>
    private bool isSorting = false;

    private bool doDefaultClipboardOperations = true;

    private ContextMenuStyles contextMenuStyle = ContextMenuStyles.None;

    private FocusStyles focusStyle = FocusStyles.None;

    private int autoSizeMinHeight = 10;
    private int autoSizeMinWidth = 10;

    /// <summary>
    /// Represents the cell that receive the mouse down event
    /// </summary>
    private Position mouseDownPosition = Position.Empty;

    /// <summary>
    /// indica l'ultima cella su cui il mouse √® stato spostato
    /// serve per la gestione dell'evento Cell.MouseLeave e MouseEnter
    /// </summary>
    private Position mouseCellPosition = Position.Empty;

    private Position focusPosition = Position.Empty;

    private GridSpecialKeys gridSpecialKeys = GridSpecialKeys.Default;

    private Position contextMenuCellPosition = Position.Empty;
    #endregion

    #region Private Events
    private event PositionCancelEventHandler cellGotFocus;
    private event PositionCancelEventHandler cellLostFocus;

    /// <summary>
    /// Fired when calling SortRangeRows method
    /// </summary>
    private event SortRangeRowsEventHandler sortingRangeRows;

    /// <summary>
    /// Fired when finishing SortRangeRows method
    /// </summary>
    private event SortRangeRowsEventHandler sortedRangeRows;

    /// <summary>
    /// Fired when user action changes the selection.
    /// </summary>
    private event UserSelectionEventHandler userSelection;

    /// <summary>
    /// MouseDown event
    /// </summary>
    private event MouseEventHandler mouseDown;

    /// <summary>
    /// Click event
    /// </summary>
    private event EventHandler click;
    #endregion

    #region Constructor
    /// <summary>
    /// Grid Constructor
    /// </summary>
    public GridVirtual()
    {
      // default double buffer
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.ResizeRedraw, true);
      SetStyle(ControlStyles.DoubleBuffer, true);

      topPanel = new GridSubPanel(this, true);
      topPanel.TabStop = false;
      leftPanel = new GridSubPanel(this, true);
      leftPanel.TabStop = false;
      topLeftPanel = new GridSubPanel(this, true);
      topLeftPanel.TabStop = false;
      scrollablePanel = new GridSubPanel(this, true);
      scrollablePanel.TabStop = false;
      hiddenFocusPanel = new GridSubPanel(this, false);
      hiddenFocusPanel.TabStop = true; //questo √® l'unico pannello a poter ricevere il tab

      rows = new RowInfo.RowInfoCollection(this);
      rows.RowHeightChanged += new RowInfoEventHandler(rows_RowHeightChanged);
      rows.RowsAdded += new IndexRangeEventHandler(rows_RowsAdded);
      columns = new ColumnInfo.ColumnInfoCollection(this);
      columns.ColumnWidthChanged += new ColumnInfoEventHandler(columns_ColumnWidthChanged);
      columns.ColumnsAdded += new IndexRangeEventHandler(columns_ColumnsAdded);

      rows.RowsRemoved += new IndexRangeEventHandler(rows_RowsRemoved);
      columns.ColumnsRemoved += new IndexRangeEventHandler(columns_ColumnsRemoved);

      SuspendLayoutGrid();

      Controls.Add(hiddenFocusPanel);
      hiddenFocusPanel.Location = new Point(0, 0);
      hiddenFocusPanel.Size = new Size(2, 2);

      topLeftPanel.Location = new Point(0, 0);

      Controls.Add(scrollablePanel);
      Controls.Add(topLeftPanel);
      Controls.Add(topPanel);
      Controls.Add(leftPanel);

      // hide this panel
      hiddenFocusPanel.SendToBack();
      hiddenFocusPanel.TabIndex = 0;

      Size = new System.Drawing.Size(200, 200);

      selection = new Selection(this);

      ContextMenuStyle = 0;

      ResumeLayoutGrid();
    }
    #endregion

    #region Dispose
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (selection != null)
        {
          selection.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Events
    /// <summary>
    /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
    /// </summary>
    public new event MouseEventHandler MouseDown
    {
      add { this.mouseDown += value; }
      remove { this.mouseDown -= value; }
    }

    /// <summary>
    /// Occurs when the control is clicked.
    /// </summary>
    public new event EventHandler Click
    {
      add { this.click += value; }
      remove { this.click -= value; }
    }

    /// <summary>
    /// Occurs when an user action changes the selection.
    /// </summary>
    public event UserSelectionEventHandler UserSelection
    {
      add { this.userSelection += value; }
      remove { this.userSelection -= value; }
    }

    /// <summary>
    /// Occurs when sorting row ranges.
    /// </summary>
    public event SortRangeRowsEventHandler SortingRangeRows
    {
      add { this.sortingRangeRows += value; }
      remove { this.sortingRangeRows -= value; }
    }

    /// <summary>
    /// Occurs after sorting row ranges.
    /// </summary>
    public event SortRangeRowsEventHandler SortedRangeRows
    {
      add { this.sortedRangeRows += value; }
      remove { this.sortedRangeRows -= value; }
    }

    /// <summary>
    /// Occurs when clipboard copy requested.
    /// </summary>
    public event EventHandler ClipboardCopy;

    /// <summary>
    /// Occurs when clipboard cut requested.
    /// </summary>
    public event EventHandler ClipboardCut;

    /// <summary>
    /// Occurs when clipboard paste requested.
    /// </summary>
    public event EventHandler ClipboardPaste;
    #endregion

    #region Menu
    /// <summary>
    /// Create the standard contextmenu based on the current selection, current focuscell and current grid settings
    /// </summary>
    /// <returns></returns>
    public List<MenuItem> GetGridContextMenus()
    {
      List<MenuItem> l_BuiltInMenu = new List<MenuItem>();

      // if there are already items in the context menu, add a menu separator
      if (ContextMenu.MenuItems.Count > 0)
      {
        l_BuiltInMenu.Add(new MenuItem("-"));
      }

      // Add selection context menu
      if (selection.Count > 0)
      {
        List<MenuItem> l_SelectionMenus = selection.GetContextMenus();
        foreach (MenuItem m in l_SelectionMenus)
        {
          l_BuiltInMenu.Add(m);
        }
      }

      // Add focus cell context menu
      // first look at whether there is a cell that has received the mousedown (in this way also manage the cells Selectable == false)
      Position l_ContextMenuCell = mouseDownPosition;
      if (l_ContextMenuCell.IsEmpty())
      {
        // otherwise use the cell with the focus
        l_ContextMenuCell = focusPosition;
      }
      if (l_ContextMenuCell.IsEmpty() == false &&
          (ContextMenuStyle & ContextMenuStyles.CellContextMenu) == ContextMenuStyles.CellContextMenu)
      {
        ICellVirtual l_tmp = GetCell(l_ContextMenuCell.Row, l_ContextMenuCell.Column);
        l_tmp.OnContextMenuPopUp(new PositionContextMenuEventArgs(l_ContextMenuCell, l_tmp, l_BuiltInMenu));
      }

      bool isAllowChangeCellHeight = (ContextMenuStyle & ContextMenuStyles.RowResize) == ContextMenuStyles.RowResize;
      bool isAllowChangeCellWidth = (ContextMenuStyle & ContextMenuStyles.ColumnResize) == ContextMenuStyles.ColumnResize;
      bool isAllowAutoSize = (ContextMenuStyle & ContextMenuStyles.AutoSize) == ContextMenuStyles.AutoSize;

      // context menu for setting height and width
      if ((isAllowChangeCellHeight == true || isAllowChangeCellWidth == true)
          && (RowsCount > 0 || ColumnsCount > 0))
      {
        if (l_BuiltInMenu.Count > 0)
        {
          l_BuiltInMenu.Add(new MenuItem("-"));
        }

        if (isAllowChangeCellHeight)
        {
          l_BuiltInMenu.Add(new MenuItem("Column Width...", new EventHandler(Menu_ColumnWidth)));
        }
        if (isAllowChangeCellHeight)
        {
          l_BuiltInMenu.Add(new MenuItem("Row Height...", new EventHandler(Menu_RowHeight)));
        }

        if (l_BuiltInMenu.Count > 0)
        {
          l_BuiltInMenu.Add(new MenuItem("-"));
        }
        if (isAllowChangeCellHeight && focusPosition.IsEmpty() == false && isAllowAutoSize)
        {
          l_BuiltInMenu.Add(new MenuItem("AutoSize Column Width...", new EventHandler(Menu_AutoSizeColumnWidth)));
        }
        if (isAllowChangeCellHeight && focusPosition.IsEmpty() == false && isAllowAutoSize)
        {
          l_BuiltInMenu.Add(new MenuItem("AutoSize Row Height...", new EventHandler(Menu_AutoSizeRowHeight)));
        }
        if (isAllowAutoSize)
        {
          l_BuiltInMenu.Add(new MenuItem("AutoSize All...", new EventHandler(Menu_AutoSizeAll)));
        }
      }

      return l_BuiltInMenu;
    }

    private void Menu_AutoSizeColumnWidth(object sender, EventArgs e)
    {
      if (focusPosition.IsEmpty() == false)
      {
        AutoSizeColumn(focusPosition.Column, autoSizeMinWidth);
      }
    }

    private void Menu_AutoSizeRowHeight(object sender, EventArgs e)
    {
      if (focusPosition.IsEmpty() == false)
      {
        AutoSizeRow(focusPosition.Row, autoSizeMinHeight);
      }
    }

    private void Menu_AutoSizeAll(object sender, EventArgs e)
    {
      AutoSizeAll(autoSizeMinHeight, autoSizeMinWidth);
    }

    private void Menu_ColumnWidth(object sender, EventArgs e)
    {
      if (focusPosition.IsEmpty() == false)
      {
        ShowColumnWidthSettings(focusPosition.Column);
      }
      else
      {
        ShowColumnWidthSettings(0);
      }
    }

    private void Menu_RowHeight(object sender, EventArgs e)
    {
      if (focusPosition.IsEmpty() == false)
      {
        ShowRowHeightSettings(focusPosition.Row);
      }
      else
      {
        ShowRowHeightSettings(0);
      }
    }

    /// <summary>
    /// Gets or sets the context menu style.
    /// </summary>
    /// <value>The context menu style.</value>
    /// <remarks>
    /// default = ContextMenuStyle.AllowAutoSize | ContextMenuStyle.AllowColumnResize | ContextMenuStyle.AllowRowResize ).
    /// </remarks>
    public ContextMenuStyles ContextMenuStyle
    {
      get
      {
        return this.contextMenuStyle;
      }
      set
      {
        this.contextMenuStyle = value;
        if (this.contextMenuStyle != 0 && !(base.ContextMenu is GridContextMenu))
        {
          ContextMenu = new GridContextMenu(this);
        }
      }
    }

    /// <summary>
    /// Gets or sets the shortcut menu associated with the control.
    /// </summary>
    /// <value>Context Menu</value>
    /// <returns>A <see cref="T:System.Windows.Forms.ContextMenu"></see> that represents the shortcut menu associated with the control.</returns>
    public override ContextMenu ContextMenu
    {
      get
      {
        return base.ContextMenu;
      }
      set
      {
        base.ContextMenu = value;
        if (!(base.ContextMenu is GridContextMenu) && ContextMenuStyle != 0)
        {
          ContextMenuStyle = 0;
        }
      }
    }

    /// <summary>
    /// Gets the cell position under the mouse cursor (row, col) when mouse right button pressed.
    /// </summary>
    /// <value>The context menu cell position.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Position ContextMenuCellPosition
    {
      get { return this.contextMenuCellPosition; }
    }
    #endregion

    #region AutoSize
    /// <summary>
    /// Gets or sets the minimun height when autosize row
    /// </summary>
    /// <value>The height of the auto size minimum.</value>
    public int AutoSizeMinHeight
    {
      get { return this.autoSizeMinHeight; }
      set { this.autoSizeMinHeight = value; }
    }

    /// <summary>
    /// Gets or sets the minimun when autosize column
    /// </summary>
    /// <value>The width of the auto size minimum.</value>
    public int AutoSizeMinWidth
    {
      get { return this.autoSizeMinWidth; }
      set { this.autoSizeMinWidth = value; }
    }

    /// <summary>
    /// Auto size the specified column with the minimum required width of each cell
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="minimumWidth">Minimum width.</param>
    public virtual void AutoSizeColumn(int column, int minimumWidth)
    {
      AutoSizeColumnRange(column, minimumWidth, 0, RowsCount - 1);
    }

    /// <summary>
    /// Auto size the specified column with the minimum required width of each cell
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="minimumWidth">The minimum width.</param>
    /// <param name="startRow">The start row.</param>
    /// <param name="endRow">The end row.</param>
    public virtual void AutoSizeColumnRange(int column, int minimumWidth, int startRow, int endRow)
    {
      if ((Columns[column].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableAutoSize) == MedDataGrid.AutoSizeModes.EnableAutoSize)
      {
        int l_minWidth = minimumWidth;
        using (Graphics l_graphics = CreateGraphics())
        {
          for (int r = startRow; r <= endRow; r++)
          {
            if (GetCell(r, column) != null)
            {
              Size l_size = GetCell(r, column).CalculateRequiredSize(new Position(r, column), l_graphics);
              if (l_size.Width > l_minWidth)
              {
                l_minWidth = l_size.Width;
              }
            }
          }
        }
        Columns[column].Width = l_minWidth;
      }
    }

    /// <summary>
    /// Auto size the specified row with the minimum required height of each cell
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="minHeight">The minimum height.</param>
    public virtual void AutoSizeRow(int row, int minHeight)
    {
      AutoSizeRowRange(row, minHeight, 0, ColumnsCount - 1);
    }

    /// <summary>
    /// Auto size the specified row with the minimum required height of each cell.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="minHeight">The minimum height.</param>
    /// <param name="startCol">The start column.</param>
    /// <param name="endCol">The end column.</param>
    public virtual void AutoSizeRowRange(int row, int minHeight, int startCol, int endCol)
    {
      if ((Rows[row].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableAutoSize) == MedDataGrid.AutoSizeModes.EnableAutoSize)
      {
        int l_minHeight = minHeight;
        using (Graphics l_graphics = CreateGraphics())
        {
          for (int c = startCol; c <= endCol; c++)
          {
            if (GetCell(row, c) != null)
            {
              Size l_size = GetCell(row, c).CalculateRequiredSize(new Position(row, c), l_graphics);
              if (l_size.Height > l_minHeight)
              {
                l_minHeight = l_size.Height;
              }
            }
          }
        }
        Rows[row].Height = l_minHeight;
      }
    }

    /// <summary>
    /// Auto size all the columns and all the rows with the required width and height
    /// </summary>
    /// <param name="minHeight">Minimum Height.</param>
    /// <param name="minWidth">Minimum Width.</param>
    public virtual void AutoSizeAll(int minHeight, int minWidth)
    {
      if (RowsCount > 0 && ColumnsCount > 0)
      {
        AutoSizeRange(minHeight, minWidth, CompleteRange);
      }
    }

    /// <summary>
    /// Auto size all the columns and all the rows with the required width and height
    /// </summary>
    /// <param name="p_MinHeight">Minimum Height</param>
    /// <param name="p_MinWidth">Minimum Width</param>
    /// <param name="p_RangeToAutoSize">Range to autosize.</param>
    public virtual void AutoSizeRange(int p_MinHeight, int p_MinWidth, Range p_RangeToAutoSize)
    {
      if (p_RangeToAutoSize.IsEmpty() == false)
      {
        bool l_bOldRedraw = Redraw;
        bool l_bOldAutoCalculateTop = Rows.AutoCalculateTop;
        bool l_bOldAutoCalculateLeft = Columns.AutoCalculateLeft;
        try
        {
          Redraw = false;
          Rows.AutoCalculateTop = false;
          Columns.AutoCalculateLeft = false;

          for (int c = p_RangeToAutoSize.End.Column; c >= p_RangeToAutoSize.Start.Column; c--)
          {
            AutoSizeColumnRange(c, p_MinWidth, p_RangeToAutoSize.Start.Row, p_RangeToAutoSize.End.Row);
          }
          for (int r = p_RangeToAutoSize.End.Row; r >= p_RangeToAutoSize.Start.Row; r--)
          {
            AutoSizeRowRange(r, p_MinHeight, p_RangeToAutoSize.Start.Column, p_RangeToAutoSize.End.Column);
          }
        }
        finally
        {
          Rows.AutoCalculateTop = l_bOldAutoCalculateTop;
          Columns.AutoCalculateLeft = l_bOldAutoCalculateLeft;
          Redraw = l_bOldRedraw;
        }

        // questo codice deve essere fatto dopo AutoCalculateTop e AutoCalculateLeft
        if (AutoStretchColumnsToFitWidth)
        {
          StretchColumnsToFitWidth();
        }
        if (AutoStretchRowsToFitHeight)
        {
          StretchRowsToFitHeight();
        }
      }
    }

    /// <summary>
    /// Auto size all the columns and all the rows with the required width and height
    /// </summary>
    public virtual void AutoSizeAll()
    {
      AutoSizeAll(AutoSizeMinHeight, AutoSizeMinWidth);
    }

    /// <summary>
    /// Auto size the columns and the rows currently visible
    /// </summary>
    /// <param name="useAllColumns">If true this method AutoSize all the columns using the data in the current rows visible, otherwise autosize only visible columns</param>
    public virtual void AutoSizeView(bool useAllColumns)
    {
      AutoSizeView(useAllColumns, AutoSizeMinHeight, AutoSizeMinWidth);
    }

    /// <summary>
    /// Auto size the columns and the rows currently visible.
    /// </summary>
    /// <param name="p_UseAllColumns">If true this method AutoSize all the columns using the data in the current rows visible, otherwise autosize only visible columns</param>
    /// <param name="p_AutoSizeMinHeight">Minimum height of the auto size.</param>
    /// <param name="p_AutoSizeMinWidth">Minimum width of the auto size.</param>
    public virtual void AutoSizeView(bool p_UseAllColumns, int p_AutoSizeMinHeight, int p_AutoSizeMinWidth)
    {
      Range l_Range = RangeAtAbsRect(RectangleRelativeToAbsolute(ClientRectangle));
      if (l_Range.IsEmpty() == false)
      {
        if (p_UseAllColumns)
        {
          AutoSizeRange(p_AutoSizeMinHeight, p_AutoSizeMinWidth, new Range(l_Range.Start.Row, 0, l_Range.End.Row, ColumnsCount - 1));
        }
        else
        {
          AutoSizeRange(p_AutoSizeMinHeight, p_AutoSizeMinWidth, l_Range);
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to auto stretch the columns width to always fit the available space, also when the contents of the cell is smaller.
    /// False to leave the original width of the columns
    /// </summary>
    /// <value>
    /// <c>true</c> if auto stretch columns to fit width; otherwise, <c>false</c>.
    /// </value>
    public bool AutoStretchColumnsToFitWidth
    {
      get { return this.doAutoStretchColumnsToFitWidth; }
      set { this.doAutoStretchColumnsToFitWidth = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to auto stretch the rows height to always fit the available space, also when the contents of the cell is smaller.
    /// False to leave the original height of the rows
    /// </summary>
    /// <value>
    /// <c>true</c> if auto stretch rows to fit height; otherwise, <c>false</c>.
    /// </value>
    public bool AutoStretchRowsToFitHeight
    {
      get { return this.doAutoStretchRowsToFitHeight; }
      set { this.doAutoStretchRowsToFitHeight = value; }
    }

    /// <summary>
    /// stretch the columns width to always fit the available space, also when the contents of the cell is smaller.
    /// </summary>
    public virtual void StretchColumnsToFitWidth()
    {
      // calcolo la grandezza attuale
      if (ColumnsCount > 0)
      {
        int l_CurrentPos = Columns.Right + 4; // pi˘ 4 per non arrivare proprio a filo
        if (DisplayRectangle.Width > l_CurrentPos)
        {
          int l_Count = 0;
          for (int i = 0; i < ColumnsCount; i++)
          {
            if ((Columns[i].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableStretch) == Fr.Medit.MedDataGrid.AutoSizeModes.EnableStretch)
            {
              l_Count++;
            }
          }

          if (l_Count > 0)
          {
            int l_DeltaPerCol = (DisplayRectangle.Width - l_CurrentPos) / l_Count;
            for (int i = 0; i < ColumnsCount; i++)
            {
              if ((Columns[i].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableStretch) == Fr.Medit.MedDataGrid.AutoSizeModes.EnableStretch)
              {
                Columns[i].Width += l_DeltaPerCol;
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// stretch the rows height to always fit the available space, also when the contents of the cell is smaller.
    /// </summary>
    public virtual void StretchRowsToFitHeight()
    {
      // calcolo la grandezza attuale
      if (RowsCount > 0)
      {
        int l_CurrentPos = Rows.Bottom + 4; // pi˘ 4 per non arrivare proprio a filo
        if (DisplayRectangle.Height > l_CurrentPos)
        {
          int l_Count = 0;
          for (int i = 0; i < RowsCount; i++)
          {
            if ((Rows[i].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableStretch) == Fr.Medit.MedDataGrid.AutoSizeModes.EnableStretch)
            {
              l_Count++;
            }
          }

          if (l_Count > 0)
          {
            int l_DeltaPerRow = (DisplayRectangle.Height - l_CurrentPos) / l_Count;
            for (int i = 0; i < RowsCount; i++)
            {
              if ((Rows[i].AutoSizeMode & MedDataGrid.AutoSizeModes.EnableStretch) == Fr.Medit.MedDataGrid.AutoSizeModes.EnableStretch)
              {
                Rows[i].Height += l_DeltaPerRow;
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Raises the System.Windows.Forms.Control.Resize event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      SuspendLayout();
      CalculatePanelsLocation();
      RecalcCustomScrollBars();
      PerformAutoStretch();
      ResumeLayout(true);
    }

    private void PerformAutoStretch()
    {
      if (AutoStretchColumnsToFitWidth == true || AutoStretchRowsToFitHeight == true)
      {
        AutoSizeAll();
        if (AutoStretchColumnsToFitWidth == true)
        {
          StretchColumnsToFitWidth();
        }
        if (AutoStretchRowsToFitHeight == true)
        {
          StretchRowsToFitHeight();
        }
      }
    }
    #endregion

    #region ColumnWidth/RowHeight form setting
    /// <summary>
    /// Display the form for customize column's width
    /// </summary>
    /// <param name="column">The column.</param>
    public virtual void ShowColumnWidthSettings(int column)
    {
      if (ColumnsCount > 0 && column >= 0 && column < ColumnsCount)
      {
        CellSizeDialog l_CellSizeDialog = new CellSizeDialog();
        l_CellSizeDialog.LoadSetting(this, column, -1, CellSizeDialog.CellSizeMode.Column);
        l_CellSizeDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Display the form for customize row's height
    /// </summary>
    /// <param name="row">The row.</param>
    public virtual void ShowRowHeightSettings(int row)
    {
      if (RowsCount > 0 && row >= 0 && row < RowsCount)
      {
        CellSizeDialog l_CellSizeDialog = new CellSizeDialog();
        l_CellSizeDialog.LoadSetting(this, -1, row, CellSizeDialog.CellSizeMode.Row);
        l_CellSizeDialog.ShowDialog();
      }
    }
    #endregion

    #region Redim, AddRow/Col, RemoveRow/Col
    /// <summary>
    /// Set the number of columns and rows.
    /// </summary>
    /// <param name="rowCount">The row count.</param>
    /// <param name="columnCount">The column count.</param>
    public void Redimension(int rowCount, int columnCount)
    {
      bool l_bOldRedraw = Redraw;
      try
      {
        Redraw = false;
        RemoveLinkedControlsOutsideRange(rowCount, columnCount);
        RowsCount = rowCount;
        ColumnsCount = columnCount;
      }
      finally
      {
        Redraw = l_bOldRedraw;
      }
    }

    private void RemoveLinkedControlsOutsideRange(int rowCount, int columnCount)
    {
      List<Control> toRemove = new List<Control>();

      foreach (DictionaryEntry linkedControl in this.linkedControls)
      {
        Position position = (Position)linkedControl.Value;
        if (position.Row >= rowCount || position.Column >= columnCount)
        {
          toRemove.Add((Control)linkedControl.Key);
        }
      }
      foreach (Control linkedControl in toRemove)
      {
        this.linkedControls.Remove(linkedControl);
      }
    }

    private void rows_RowsRemoved(object sender, IndexRangeEventArgs e)
    {
      Range l_RemovedRange = new Range(e.StartIndex, 0, e.StartIndex + e.Count - 1, ColumnsCount - 1);

      if (l_RemovedRange.Contains(FocusCellPosition))
      {
        SetFocusCell(Position.Empty);
      }
      if (l_RemovedRange.Contains(mouseCellPosition))
      {
        mouseCellPosition = Position.Empty;
      }
      if (l_RemovedRange.Contains(mouseDownPosition))
      {
        mouseDownPosition = Position.Empty;
      }
    }

    private void columns_ColumnsRemoved(object sender, IndexRangeEventArgs e)
    {
      Range l_RemovedRange = new Range(0, e.StartIndex, RowsCount - 1, e.StartIndex + e.Count - 1);

      if (l_RemovedRange.Contains(FocusCellPosition))
      {
        SetFocusCell(Position.Empty);
      }
      if (l_RemovedRange.Contains(mouseCellPosition))
      {
        mouseCellPosition = Position.Empty;
      }
      if (l_RemovedRange.Contains(mouseDownPosition))
      {
        mouseDownPosition = Position.Empty;
      }
    }
    #endregion

    #region Cell to Rectangle
    /// <summary>
    /// Get the rectangle of the cell respect to the client area visible, the grid DisplayRectangle.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public Rectangle PositionToDisplayRect(Position position)
    {
      return RangeToDisplayRect(new Range(position));
    }

    /// <summary>
    /// Get the Rectangle of the cell respect all the scrollable area.
    /// </summary>
    /// <remarks>
    /// This method can not use Row/Col Span.
    /// </remarks>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual Rectangle PositionToAbsoluteRect(Position position)
    {
      return RangeToAbsoluteRect(new Range(position));
    }

    /// <summary>
    /// Returns the absolute rectangle relative to the total scrollable area of the specified Range. Returns a 0 rectangle if the Range is not valid
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <returns></returns>
    public virtual Rectangle RangeToAbsoluteRect(Range p_Range)
    {
      if (p_Range.IsEmpty())
      {
        return new Rectangle(0, 0, 0, 0);
      }

      int l_Left = Columns[p_Range.Start.Column].Left;
      int l_Top = Rows[p_Range.Start.Row].Top;

      return new Rectangle(l_Left, // x
                          l_Top,  // y
                          Columns[p_Range.End.Column].Right - l_Left,   // width
                          Rows[p_Range.End.Row].Bottom - l_Top);  // height
    }

    /// <summary>
    /// Returns the relative rectangle to the current scrollable area of the specified Range. Returns a 0 rectangle if the Range is not valid
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <returns></returns>
    public Rectangle RangeToDisplayRect(Range p_Range)
    {
      if (p_Range.IsEmpty())
      {
        return new Rectangle(0, 0, 0, 0);
      }

      Rectangle l_Absolute = RangeToAbsoluteRect(p_Range);
      Rectangle l_Display = RectangleAbsoluteToRelative(l_Absolute);

      CellPositionType l_Type = GetPositionType(p_Range.Start);
      if (l_Type == CellPositionType.FixedTopLeft)
      {
        return new Rectangle(l_Absolute.X, l_Absolute.Y, l_Absolute.Width, l_Absolute.Height);
      }
      else if (l_Type == CellPositionType.FixedTop)
      {
        return new Rectangle(l_Display.X, l_Absolute.Y, l_Absolute.Width, l_Absolute.Height);
      }
      else if (l_Type == CellPositionType.FixedLeft)
      {
        return new Rectangle(l_Absolute.X, l_Display.Y, l_Absolute.Width, l_Absolute.Height);
      }
      else if (l_Type == CellPositionType.Scrollable)
      {
        return l_Display;
      }
      else
      {
        return new Rectangle(0, 0, 0, 0);
      }
    }

    /// <summary>
    /// Indicates whether the specified cell is visible, partially or fully.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    /// <c>true</c> if cell is visible at the specified position; otherwise, <c>false</c>.
    /// </returns>
    public bool IsCellVisible(Position position)
    {
      Rectangle l_cellRect = PositionToDisplayRect(position);
      Rectangle l_ClientRectangle = DisplayRectangle;

      if (Rows[position.Row].Height == 0) // || Columns[position.Column].Width == 0)
      {
        return false;
      }

      if ((FixedColumns > 0 && position.Column != Columns[0].Index && l_cellRect.Left < Columns[0].Left && l_cellRect.Right <= Columns[0].Left)
        || (FixedColumns == 0 && l_cellRect.Left < l_ClientRectangle.Left && l_cellRect.Right < l_ClientRectangle.Left))
      {
        return false;
      }

      if ((FixedRows > 0 && position.Row != Rows[0].Index && l_cellRect.Location.Y <= Rows[0].Top && l_cellRect.Bottom <= Rows[0].Bottom)
        || (FixedRows == 0 && l_cellRect.Location.Y < l_ClientRectangle.Top && l_cellRect.Bottom < l_ClientRectangle.Top))
      {
        return false;
      }

      if (l_cellRect.Right > l_ClientRectangle.Right && l_cellRect.Left > l_ClientRectangle.Right)
      {
        return false;
      }

      if (l_cellRect.Bottom > l_ClientRectangle.Bottom && l_cellRect.Top > l_ClientRectangle.Bottom)
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Return the scroll position that must be set to fully show the specified cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_NewScrollPosition">The new scroll position.</param>
    /// <returns>
    /// Return false if the cell is already fully visible, return true if the cell is not currently visible.
    /// </returns>
    private bool GetScrollPositionToShowCell(Position position, out Point p_NewScrollPosition)
    {
      Rectangle l_cellRect = PositionToDisplayRect(position);
      Point l_newCustomScrollPosition = new Point(CustomScrollPosition.X, CustomScrollPosition.Y);
      bool l_ApplyScroll = false;
      Rectangle l_ClientRectangle = DisplayRectangle;

      if ((FixedColumns > 0 && position.Column != Columns[0].Index && l_cellRect.Left < Columns[0].Left && l_cellRect.Right <= Columns[0].Left)
        || (FixedColumns == 0 && l_cellRect.Left < l_ClientRectangle.Left && l_cellRect.Right < l_ClientRectangle.Left))
      {
        l_newCustomScrollPosition.X -= l_cellRect.Location.X - Columns[Math.Min(FixedColumns, position.Column)].Left;
        l_ApplyScroll = true;
      }

      if ((FixedRows > 0 && position.Row != Rows[0].Index && (l_cellRect.Location.Y <= Rows[0].Top || l_cellRect.Bottom <= Rows[0].Bottom))
        || (FixedRows == 0 && l_cellRect.Location.Y < l_ClientRectangle.Top && l_cellRect.Bottom < l_ClientRectangle.Top))
      {
        l_newCustomScrollPosition.Y -= l_cellRect.Location.Y - Rows[Math.Min(FixedRows, position.Row)].Top;
        l_ApplyScroll = true;
      }

      if (l_cellRect.Right > l_ClientRectangle.Right && l_cellRect.Left > l_ClientRectangle.Right)
      {
        l_newCustomScrollPosition.X -= l_cellRect.Right - l_ClientRectangle.Right;
        l_ApplyScroll = true;
      }

      if (l_cellRect.Bottom > l_ClientRectangle.Bottom) // && l_cellRect.Top > l_ClientRectangle.Bottom)
      {
        l_newCustomScrollPosition.Y -= l_cellRect.Bottom - l_ClientRectangle.Bottom;
        l_ApplyScroll = true;
      }

      p_NewScrollPosition = l_newCustomScrollPosition;

      return l_ApplyScroll;
    }

    /// <summary>
    /// Scroll the view to show the cell passed
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    /// Returns true if the Cell passed was already visible, otherwise false
    /// </returns>
    public bool ShowCell(Position position)
    {
      Point l_newCustomScrollPosition;
      if (GetScrollPositionToShowCell(position, out l_newCustomScrollPosition))
      {
        CustomScrollPosition = l_newCustomScrollPosition;

        if (FixedRows > 0 || FixedColumns > 0)
        {
          InvalidateCells();
        }

        return false;
      }
      return true;
    }

    /// <summary>
    /// Force a cell to redraw. If Redraw is set to false this function has no effects
    /// </summary>
    /// <param name="position">The position.</param>
    public virtual void InvalidateCell(Position position)
    {
      if (Redraw && position.IsEmpty() == false)
      {
        Rectangle l_GridRectangle = PositionToDisplayRect(position);
        GridSubPanel l_Panel = PanelAtPosition(position);

        l_Panel.Invalidate(l_Panel.RectangleGridToPanel(l_GridRectangle), false);
      }
    }

    /// <summary>
    /// Force a range of cells to redraw. If Redraw is set to false this function has no effects
    /// </summary>
    /// <param name="p_Range">The range.</param>
    public void InvalidateRange(Range p_Range)
    {
      p_Range = Range.Intersect(p_Range, CompleteRange); // to ensure the range is valid
      if (Redraw && p_Range.IsEmpty() == false)// && !this.DrawColLine)
      {
        Rectangle l_GridRectangle = RangeToDisplayRect(p_Range);
        Invalidate(l_GridRectangle, true);
      }
    }
    #endregion

    #region Focus
    /// <summary>
    /// Fired before a cell receive the focus (FocusCell is populated after
    /// this event, use e.Cell to read the cell that will receive the focus)
    /// </summary>
    public event PositionCancelEventHandler CellGotFocus
    {
      add { this.cellGotFocus += value; }
      remove { this.cellGotFocus -= value; }
    }

    /// <summary>
    /// Fired before a cell lost the focus
    /// </summary>
    public event PositionCancelEventHandler CellLostFocus
    {
      add { this.cellLostFocus += value; }
      remove { this.cellLostFocus -= value; }
    }

    /// <summary>
    /// Fired when a cell receive the focus
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    protected virtual void OnCellGotFocus(PositionCancelEventArgs e)
    {
      if (e.Cancel)
      {
        return;
      }

      // Event Got Focus
      if (cellGotFocus != null)
      {
        cellGotFocus(this, e);
      }

      if (e.Cancel)
      {
        return;
      }

      ////e.Cancel = !SetFocusOnCells();
      ////if (e.Cancel)
      ////{
      ////  return;
      ////}
      SetFocusOnCells();

      this.focusPosition = e.Position;
      e.Cell.OnFocusEntered(e);
    }

    /// <summary>
    /// Fired when a cell lost the focus
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionCancelEventArgs"/> instance containing the event data.</param>
    public virtual void OnCellLostFocus(PositionCancelEventArgs e)
    {
      if (e.Cancel)
      {
        return;
      }

      bool l_FocusContainer = true;
      if (ContainsFocus)
      {
        l_FocusContainer = SetFocusOnCells();
      }

      e.Cancel = l_FocusContainer && e.Cell.EndEdit(false) == false;

      if (e.Cancel)
      {
        return;
      }

      //event Lost Focus
      if (cellLostFocus != null)
      {
        cellLostFocus(this, e);
      }

      if (e.Cancel)
      {
        return;
      }

      this.focusPosition = Position.Empty;
      e.Cell.OnFocusLeft(e);
    }

    /// <summary>
    /// Gets the active cell
    /// </summary>
    /// <value>The focus cell position.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Position FocusCellPosition
    {
      get { return this.focusPosition; }
    }

    /// <summary>
    /// Gets the row that has the focus. If no row is selected return null.
    /// </summary>
    /// <value>The focus row.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RowInfo FocusRow
    {
      get
      {
        if (FocusCellPosition.IsEmpty() || FocusCellPosition.Row >= RowsCount)
        {
          return null;
        }
        return Rows[FocusCellPosition.Row];
      }
    }

    /// <summary>
    /// Gets the column that has the focus. If no column is selected return <c>null</c>.
    /// </summary>
    /// <value>The focus column.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ColumnInfo FocusColumn
    {
      get
      {
        if (FocusCellPosition.IsEmpty() || FocusCellPosition.Column >= ColumnsCount)
        {
          return null;
        }
        return Columns[FocusCellPosition.Column];
      }
    }

    /// <summary>
    /// Change the focus of the grid. The calls order is: (the user select CellX) Grid.CellGotFocus(CellX), CellX.Enter, (the user select CellY), Grid.CellLostFocus(CellX), CellX.Leave, Grid.CellGotFocus(CellY), CellY.Enter. If Control key is not pressed deselect others cells
    /// </summary>
    /// <param name="p_CellToSetFocus">Must be a valid cell linked to the grid or null of you want to remove the focus</param>
    /// <returns>
    /// Return true if the grid can select the cell specified, otherwise false
    /// </returns>
    public bool SetFocusCell(Position p_CellToSetFocus)
    {
      // teste si le bouton control est appuye
      bool l_bControlPress = (Control.ModifierKeys & Keys.Control) == Keys.Control;
      // deselectionne la selection de la selection precedente
      return SetFocusCell(p_CellToSetFocus, l_bControlPress == false || Selection.EnableMultiSelection == false);
    }

    /// <summary>
    /// Change the focus of the grid.
    /// </summary>
    /// <param name="p_CellToSetFocus">Must be a valid cell linked to the grid or null of you want to remove the focus</param>
    /// <param name="p_DeselectOtherCells">True to deselect others selected cells</param>
    /// <returns>
    /// Return true if the grid can select the cell specified, otherwise false
    /// </returns>
    /// <remarks>
    /// The call order is:
    /// (the user select CellX)
    /// CellX.FocusEntering
    /// Grid.CellGotFocus(CellX),
    /// CellX.FocusEntered,
    /// (the user select CellY),
    /// CellY.FocusEntering
    /// CellX.FocusLeaving
    /// Grid.CellLostFocus(CellX),
    /// CellX.FocusLeft,
    /// Grid.CellGotFocus(CellY),
    /// CellY.FocusEntered
    /// </remarks>
    public virtual bool SetFocusCell(Position p_CellToSetFocus, bool p_DeselectOtherCells)
    {
      if (p_CellToSetFocus == FocusCellPosition
        || (p_CellToSetFocus.Column != FocusCellPosition.Column && p_CellToSetFocus.Row == FocusCellPosition.Row && Selection.SelectionMode == GridSelectionMode.Row))
      {
        // put anyway the focus on the cells
        if (p_CellToSetFocus.IsEmpty() == false)
        {
          SetFocusOnCells();
        }

        if (FocusCellPosition.IsEmpty() == false && GetCell(FocusCellPosition) != null)
        {
          PositionCancelEventArgs l_OldEventArg = new PositionCancelEventArgs(FocusCellPosition, GetCell(FocusCellPosition));

          l_OldEventArg.Cell.OnFocusLeaving(l_OldEventArg);
          if (l_OldEventArg.Cancel)
          {
            return false;
          }

          OnCellLostFocus(l_OldEventArg);
          if (l_OldEventArg.Cancel)
          {
            return false;
          }
        }

        // deselect old selected cells
        if (p_DeselectOtherCells)
        {
          Selection.Clear();
        }
      }
      else // if (p_CellToSetFocus != FocusCellPosition || (p_CellToSetFocus.Column==FocusCellPosition.Column || p_CellToSetFocus.Row!=FocusCellPosition.Row || Selection.SelectionMode==GridSelectionMode.Row) ) //
      {
        // New Focus Cell Entering
        ICellVirtual l_CellToFocus = GetCell(p_CellToSetFocus);
        PositionCancelEventArgs l_NewEventArg = new PositionCancelEventArgs(p_CellToSetFocus, l_CellToFocus);
        if (p_CellToSetFocus.IsEmpty() == false && l_CellToFocus != null)
        {
          l_CellToFocus.OnFocusEntering(l_NewEventArg);
          if (l_NewEventArg.Cancel)
          {
            return false;
          }
        }

        if (l_CellToFocus != null && l_CellToFocus.CanReceiveFocus == false)
        {
          return false;
        }

        // Old Focus Cell Leaving and Left
        if (FocusCellPosition.IsEmpty() == false && GetCell(FocusCellPosition) != null)
        {
          PositionCancelEventArgs l_OldEventArg = new PositionCancelEventArgs(FocusCellPosition, GetCell(FocusCellPosition));

          l_OldEventArg.Cell.OnFocusLeaving(l_OldEventArg);
          if (l_OldEventArg.Cancel)
          {
            return false;
          }

          OnCellLostFocus(l_OldEventArg);
          if (l_OldEventArg.Cancel)
          {
            return false;
          }
        }

        // Deselect Old Cells
        if (p_DeselectOtherCells)
        {
          Selection.Clear();
        }

        // New Focus Cell Entered
        if (p_CellToSetFocus.IsEmpty() == false &&
            l_CellToFocus != null)
        {
          OnCellGotFocus(l_NewEventArg);

          return !l_NewEventArg.Cancel;
        }
      }

      return true;
    }

    /// <summary>
    /// Get the real position for the specified position. For example when position
    /// is a merged cell this method returns the starting position of the merged cells.
    /// Usually this method returns the same cell specified as parameter.
    /// This method is used for processing arrow keys, to find a valid cell when the
    /// focus is in a merged cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual Position GetStartingPosition(Position position)
    {
      return position;
    }

    /// <summary>
    /// Raises the System.Windows.Forms.Control.Leave event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnLeave(EventArgs e)
    {
      base.OnLeave(e);

      if ((this.focusStyle & FocusStyles.RemoveFocusCellOnLeave) == FocusStyles.RemoveFocusCellOnLeave)
      {
        SetFocusCell(Position.Empty, false);
      }

      if ((this.focusStyle & FocusStyles.RemoveSelectionOnLeave) == FocusStyles.RemoveSelectionOnLeave)
      {
        Selection.Clear();
      }
    }

    /// <summary>
    /// Gets or sets the behavior of the focus and selection. Default is FocusStyle.None.
    /// </summary>
    /// <value>The focus style.</value>
    public FocusStyles FocusStyle
    {
      get { return this.focusStyle; }
      set { this.focusStyle = value; }
    }

    /// <summary>
    /// Gets the cell position currently under the mouse cursor (row, col). If you MouseDown on a cell this cell is the MouseCellPosition until an MouseUp is fired
    /// </summary>
    /// <value>The mouse cell position.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Position MouseCellPosition
    {
      get { return this.mouseCellPosition; }
    }

    /// <summary>
    /// Fired when the cell under the mouse change. For internal use only.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    protected void ChangeMouseCell(Position p_Cell)
    {
      if (this.mouseCellPosition != p_Cell)
      {
        if (this.mouseCellPosition.IsEmpty() == false &&
            this.mouseCellPosition != mouseDownPosition)
        {
          ICellVirtual l_OldCell = GetCell(this.mouseCellPosition.Row, mouseCellPosition.Column);
          if (l_OldCell != null)
          {
            l_OldCell.OnMouseLeave(new PositionEventArgs(this.mouseCellPosition, l_OldCell));
          }
        }

        this.mouseCellPosition = p_Cell;
        if (mouseCellPosition.IsEmpty() == false)
        {
          ICellVirtual l_NewCell = GetCell(this.mouseCellPosition.Row, this.mouseCellPosition.Column);
          if (l_NewCell != null)
          {
            l_NewCell.OnMouseEnter(new PositionEventArgs(this.mouseCellPosition, l_NewCell));
          }
        }
      }
    }

    /// <summary>
    /// Change the cell currently under the mouse
    /// </summary>
    /// <param name="p_MouseDownCell">The mouse down cell.</param>
    /// <param name="p_MouseCell">The mouse cell.</param>
    protected void ChangeMouseDownCell(Position p_MouseDownCell, Position p_MouseCell)
    {
      mouseDownPosition = p_MouseDownCell;
      ChangeMouseCell(p_MouseCell);
    }

    /// <summary>
    /// Fired when the selection with the mouse is finished
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.RangeEventArgs"/> instance containing the event data.</param>
    protected virtual void OnMouseSelectionFinish(RangeEventArgs e)
    {
      oldMouseSelectionRange = Range.Empty;
    }

    /// <summary>
    /// Gets the mouse selection range.
    /// </summary>
    /// <value>The mouse selection range.</value>
    /// <remarks>
    /// Returns the cells that are selected with the mouse.
    /// Returns Range.Empty if no cells are selected.
    /// </remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual Range MouseSelectionRange
    {
      get { return this.mouseSelectionRange; }
    }

    /// <summary>
    /// Fired when the mouse selection must be cancelled.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.RangeEventArgs"/> instance containing the event data.</param>
    protected virtual void OnUndoMouseSelection(RangeEventArgs e)
    {
      Selection.RemoveRange(e.Range);
    }

    /// <summary>
    /// Fired when the mouse selection is succesfully finished
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.RangeEventArgs"/> instance containing the event data.</param>
    protected virtual void OnApplyMouseSelection(RangeEventArgs e)
    {
      Selection.AddRange(e.Range);
    }

    /// <summary>
    /// Fired when the mouse selection change
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnMouseSelectionChange(EventArgs e)
    {
      Range l_MouseRange = MouseSelectionRange;

      OnUndoMouseSelection(new RangeEventArgs(oldMouseSelectionRange));
      OnApplyMouseSelection(new RangeEventArgs(l_MouseRange));

      oldMouseSelectionRange = l_MouseRange;
    }

    /// <summary>
    /// Fired when the mouse selection finish
    /// </summary>
    protected void MouseSelectionFinish()
    {
      if (mouseSelectionRange != Range.Empty)
      {
        OnMouseSelectionFinish(new RangeEventArgs(oldMouseSelectionRange));
      }

      mouseSelectionRange = Range.Empty;
    }

    /// <summary>
    /// Fired when the corner of the mouse selection change. For internal use only.
    /// </summary>
    /// <param name="p_Corner">The corner.</param>
    protected virtual void ChangeMouseSelectionCorner(Position p_Corner)
    {
      bool isChanged = false;
      if (mouseSelectionRange.Start != focusPosition
        || mouseSelectionRange.End != p_Corner)
      {
        isChanged = true;
      }

      mouseSelectionRange = new Range(focusPosition, p_Corner);

      if (isChanged)
      {
        OnMouseSelectionChange(EventArgs.Empty);
        OnUserSelection(new UserSelectionEventArgs(selection));
      }
    }

    /// <summary>
    /// Gets the selected cells
    /// </summary>
    /// <value>The selection.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Selection Selection
    {
      get { return this.selection; }
    }
    #endregion

    #region Mouse Properties
    /// <summary>
    /// Gets the mouse down position.
    /// </summary>
    /// <value>The mouse down position.</value>
    /// <remarks>
    /// Represents the cell that have received the MouseDown event. You can use this cell for contextmenu logic. Can be null.
    /// </remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Position MouseDownPosition
    {
      get { return this.mouseDownPosition; }
    }
    #endregion

    #region Special Keys
    /// <summary>
    /// Gets or sets the special keys that the grid can handle.
    /// </summary>
    /// <value>The special keys.</value>
    /// <remarks>
    /// This enum can be changed to block or allow some special keys function. For example to disable Ctrl+C Copy operation remove from this enum the GridSpecialKeys.Ctrl_C.
    /// </remarks>
    public GridSpecialKeys SpecialKeys
    {
      get { return this.gridSpecialKeys; }
      set { this.gridSpecialKeys = value; }
    }

    /// <summary>
    /// Process Delete, Ctrl+C, Ctrl+V, Up, Down, Left, Right, Tab keys
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
    protected virtual void ProcessSpecialGridKey(KeyEventArgs e)
    {
      bool l_enableArrows = (this.gridSpecialKeys & GridSpecialKeys.Arrows) == GridSpecialKeys.Arrows;
      bool l_enableCtrl_C = (this.gridSpecialKeys & GridSpecialKeys.Ctrl_C) == GridSpecialKeys.Ctrl_C;
      bool l_enableCtrl_V = (this.gridSpecialKeys & GridSpecialKeys.Ctrl_V) == GridSpecialKeys.Ctrl_V;
      bool l_enableCtrl_X = (this.gridSpecialKeys & GridSpecialKeys.Ctrl_X) == GridSpecialKeys.Ctrl_X;
      bool l_enableDelete = (this.gridSpecialKeys & GridSpecialKeys.Delete) == GridSpecialKeys.Delete;
      bool l_enablePageDownUp = (this.gridSpecialKeys & GridSpecialKeys.PageDownUp) == GridSpecialKeys.PageDownUp;
      bool l_enableTab = (this.gridSpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab;

      //Selection Keys
      if (selection.Count > 0)
      {
        if (e.KeyCode == Keys.Delete && l_enableDelete)
        {
          selection.ClearValues();
        }
        else if (e.Control && e.KeyCode == Keys.C && l_enableCtrl_C)
        {
          if (this.ClipboardCopy != null)
          {
            this.ClipboardCopy(this, EventArgs.Empty);
          }
          if (doDefaultClipboardOperations)
          {
            selection.OnClipboardCopy();
          }
        }
        else if (e.Control && e.KeyCode == Keys.V && l_enableCtrl_V)
        {
          if (this.ClipboardPaste != null)
          {
            this.ClipboardPaste(this, EventArgs.Empty);
          }
          if (doDefaultClipboardOperations)
          {
            selection.OnClipboardPaste();
          }
        }
        else if (e.Control && e.KeyCode == Keys.X && l_enableCtrl_X)
        {
          if (this.ClipboardCut != null)
          {
            this.ClipboardCut(this, EventArgs.Empty);
          }
          if (doDefaultClipboardOperations)
          {
            selection.OnClipboardCut();
          }
        }
      }

      if (focusPosition.IsEmpty() == false)
      {
        ICellVirtual l_FocusCell = GetCell(focusPosition);
        if (l_FocusCell != null)
        {
          l_FocusCell.OnKeyDown(new PositionKeyEventArgs(focusPosition, l_FocusCell, e));

          // Process ArrowKey For navigate into cells, tab and PgDown/Up
          ICellVirtual tmp = null;
          Position l_NewPosition = Position.Empty;
          if (e.KeyCode == Keys.Down && l_enableArrows)
          {
            int tmpRow = focusPosition.Row;
            tmpRow++;
            while (tmp == null && tmpRow < RowsCount)
            {
              l_NewPosition = new Position(tmpRow, focusPosition.Column);
              // Verifies that the starting position does not coincide with that of focus, otherwise it means that we are moving in the same cell because we use a RowSpan / ColSpan
              if (GetStartingPosition(l_NewPosition) == focusPosition)
              {
                tmp = null;
              }
              else
              {
                tmp = GetCell(l_NewPosition);
                if (tmp != null && tmp.CanReceiveFocus == false)
                {
                  tmp = null;
                }
              }

              tmpRow++;
            }
          }
          else if (e.KeyCode == Keys.Up && l_enableArrows)
          {
            int tmpRow = focusPosition.Row;
            tmpRow--;
            while (tmp == null && tmpRow >= 0)
            {
              l_NewPosition = new Position(tmpRow, focusPosition.Column);
              //verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perch√® usa un RowSpan/ColSpan
              if (GetStartingPosition(l_NewPosition) == focusPosition)
              {
                tmp = null;
              }
              else
              {
                tmp = GetCell(l_NewPosition);
                if (tmp != null && tmp.CanReceiveFocus == false)
                {
                  tmp = null;
                }
              }

              tmpRow--;
            }
          }
          else if (e.KeyCode == Keys.Right && l_enableArrows)
          {
            int tmpCol = focusPosition.Column;
            tmpCol++;
            while (tmp == null && tmpCol < ColumnsCount)
            {
              l_NewPosition = new Position(focusPosition.Row, tmpCol);
              //verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perch√® usa un RowSpan/ColSpan
              if (GetStartingPosition(l_NewPosition) == focusPosition)
              {
                tmp = null;
              }
              else
              {
                tmp = GetCell(l_NewPosition);
                if (tmp != null && tmp.CanReceiveFocus == false)
                {
                  tmp = null;
                }
              }

              tmpCol++;
            }
          }
          else if (e.KeyCode == Keys.Left && l_enableArrows)
          {
            int tmpCol = focusPosition.Column;
            tmpCol--;
            while (tmp == null && tmpCol >= 0)
            {
              l_NewPosition = new Position(focusPosition.Row, tmpCol);
              //verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perch√® usa un RowSpan/ColSpan
              if (GetStartingPosition(l_NewPosition) == focusPosition)
              {
                tmp = null;
              }
              else
              {
                tmp = GetCell(l_NewPosition);
                if (tmp != null && tmp.CanReceiveFocus == false)
                {
                  tmp = null;
                }
              }

              tmpCol--;
            }
          }
          else if (e.KeyCode == Keys.Tab && l_enableTab)
          {
            int tmpRow = focusPosition.Row;
            int tmpCol = focusPosition.Column;

            if (e.Modifiers == Keys.Shift)
            {
              tmpCol--;
              while (tmp == null && tmpRow >= 0)
              {
                while (tmp == null && tmpCol >= 0)
                {
                  l_NewPosition = new Position(tmpRow, tmpCol);
                  //verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perch√® usa un RowSpan/ColSpan
                  if (GetStartingPosition(l_NewPosition) == focusPosition)
                  {
                    tmp = null;
                  }
                  else
                  {
                    tmp = GetCell(l_NewPosition);
                    if (tmp != null && tmp.CanReceiveFocus == false)
                    {
                      tmp = null;
                    }
                  }

                  tmpCol--;
                }

                tmpRow--;
                tmpCol = ColumnsCount - 1;
              }
            }
            else // avanti
            {
              tmpCol++;
              while (tmp == null && tmpRow < RowsCount)
              {
                while (tmp == null && tmpCol < ColumnsCount)
                {
                  l_NewPosition = new Position(tmpRow, tmpCol);
                  //verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perch√® usa un RowSpan/ColSpan
                  if (GetStartingPosition(l_NewPosition) == focusPosition)
                  {
                    tmp = null;
                  }
                  else
                  {
                    tmp = GetCell(l_NewPosition);
                    if (tmp != null && tmp.CanReceiveFocus == false)
                    {
                      tmp = null;
                    }
                  }

                  tmpCol++;
                }

                tmpRow++;
                tmpCol = 0;
              }
            }
          }
          else if ((e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown)
              && l_enablePageDownUp)
          {
            Point l_FocusPoint = PositionToDisplayRect(focusPosition).Location;
            l_FocusPoint.Offset(1, 1);

            if (e.KeyCode == Keys.PageDown)
            {
              CustomScrollPageDown();
            }
            else if (e.KeyCode == Keys.PageUp)
            {
              CustomScrollPageUp();
            }
          }

          if (tmp != null && l_NewPosition.IsEmpty() == false)
          {
            SetFocusCell(l_NewPosition);
            OnUserSelection(new UserSelectionEventArgs(selection));
          }
        }
      }
    }
    #endregion

    #region Scroll
    /// <summary>
    /// Gets or sets the position of the scrollbars
    /// </summary>
    /// <value>The grid scroll position.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Point GridScrollPosition
    {
      get { return CustomScrollPosition; }
      set { CustomScrollPosition = value; }
    }
    #endregion

    #region Paint Events
    /// <summary>
    /// Fired when draw Left Panel
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    public virtual void OnTopLeftPanelPaint(PaintEventArgs e)
    {
      PanelPaint(TopLeftPanel, e);
    }

    /// <summary>
    /// Fired when draw Left Panel
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    public virtual void OnLeftPanelPaint(PaintEventArgs e)
    {
      PanelPaint(LeftPanel, e);
    }

    /// <summary>
    /// Fired when draw Top Panel
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    public virtual void OnTopPanelPaint(PaintEventArgs e)
    {
      PanelPaint(TopPanel, e);
    }

    /// <summary>
    /// Fired when draw scrollable panel
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    public virtual void OnScrollablePanelPaint(PaintEventArgs e)
    {
      PanelPaint(ScrollablePanel, e);
    }

    /// <summary>
    /// Draw the specified region of cells in PaintEventArgs to the specified GridSubPanel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    protected virtual void PanelPaint(GridSubPanel panel, PaintEventArgs e)
    {
      if (!Redraw || this.DrawColLine)
      {
        return;
      }

      // Draw BackColor (Needed due to Opaque ControlStyles)
      e.Graphics.Clear(BackColor);

      // DrawCells
      Range l_Range = panel.RangeAtDisplayRect(panel.RectanglePanelToGrid(e.ClipRectangle));

      if (l_Range.Start.IsEmpty() == false)
      {
        PaintRange(panel, e, l_Range);
      }
    }

    /// <summary>
    /// Paint the specified GridSubPanel.
    /// </summary>
    /// <param name="panel">The panel.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    public virtual void PanelsPaint(GridSubPanel panel, PaintEventArgs e)
    {
      if (!Redraw || this.DrawColLine)
      {
        return;
      }

      // Draw BackColor (needed due to Opaque ControlStyles)
      e.Graphics.Clear(BackColor);

      // DrawCells
      if (e.ClipRectangle.Width > 0 && e.ClipRectangle.Height > 0)
      {
        if (panel == TopLeftPanel)
        {
          Range l_Range = TopLeftPanel.RangeAtDisplayRect(TopLeftPanel.RectanglePanelToGrid(e.ClipRectangle));
          if (l_Range.Start.IsEmpty() == false)
          {
            PaintRange(TopLeftPanel, e, l_Range);
          }
          TopLeftPanel.SuspendLayout();
        }
        if (panel == LeftPanel)
        {
          Range l_Range = LeftPanel.RangeAtDisplayRect(LeftPanel.RectanglePanelToGrid(e.ClipRectangle));
          if (l_Range.Start.IsEmpty() == false)
          {
            PaintRange(LeftPanel, e, l_Range);
          }
          LeftPanel.SuspendLayout();
        }
        if (panel == TopPanel)
        {
          Range l_Range = TopPanel.RangeAtDisplayRect(TopPanel.RectanglePanelToGrid(e.ClipRectangle));
          if (l_Range.Start.IsEmpty() == false)
          {
            PaintRange(TopPanel, e, l_Range);
          }
          TopPanel.SuspendLayout();
        }
        if (panel == ScrollablePanel)
        {
          Range l_Range = ScrollablePanel.RangeAtDisplayRect(ScrollablePanel.RectanglePanelToGrid(e.ClipRectangle));
          if (l_Range.Start.IsEmpty() == false)
          {
            PaintRange(ScrollablePanel, e, l_Range);
          }
          ResumeLayoutGrid();
        }
      }
    }

    /// <summary>
    /// Draw a range of cells in the specified panel
    /// </summary>
    /// <param name="p_Panel">The panel.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    /// <param name="p_Range">The range.</param>
    protected virtual void PaintRange(GridSubPanel p_Panel, PaintEventArgs e, Range p_Range)
    {
      Rectangle l_DrawRect;

      l_DrawRect = p_Panel.RectangleGridToPanel(PositionToDisplayRect(p_Range.Start));
      Rectangle l_AbsRect = PositionToAbsoluteRect(p_Range.Start);
      int l_DeltaX = l_AbsRect.Left - l_DrawRect.Left;
      int l_DeltaY = l_AbsRect.Top - l_DrawRect.Top;

      Position l_p;

      for (int r = p_Range.Start.Row; r <= p_Range.End.Row; ++r)
      {
        int l_Top = Rows[r].Top - l_DeltaY;
        int l_Height = Rows[r].Height;

        for (int c = p_Range.Start.Column; c <= p_Range.End.Column; ++c)
        {
          l_DrawRect.Location = new Point(Columns[c].Left - l_DeltaX, l_Top);
          l_DrawRect.Size = new Size(Columns[c].Width, l_Height);

          ICellVirtual l_Cell = GetCell(r, c);
          if (l_Cell != null)
          {
            l_p = new Position(r, c);
            if (IsCellVisible(l_p))
            {
              PaintCell(p_Panel, e, l_Cell, l_p, l_DrawRect);
            }
            else
            {
              while (c != p_Range.End.Column && !IsCellVisible(l_p))
              {
                c++;
                l_p = new Position(r, c);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Draw the specified Cell
    /// </summary>
    /// <param name="p_Panel">The panel.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="p_PanelDrawRectangle">The panel draw rectangle.</param>
    protected virtual void PaintCell(GridSubPanel p_Panel, PaintEventArgs e, ICellVirtual p_Cell, Position p_CellPosition, Rectangle p_PanelDrawRectangle)
    {
      p_Cell.VisualModel.DrawCell(p_Cell, p_CellPosition, e, p_PanelDrawRectangle);
    }
    #endregion

    #region Mouse Events
    /// <summary>
    /// MouseDown event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridMouseDown(MouseEventArgs e)
    {
      if (mouseDown != null)
      {
        mouseDown(this, e);
      }

      if (FocusCellPosition.IsEmpty() == false)
      {
        ICellVirtual l_FocusCell = GetCell(FocusCellPosition);
        if (l_FocusCell != null && l_FocusCell.IsEditing(FocusCellPosition))
        {
          if (l_FocusCell.EndEdit(false) == false)
          {
            return;
          }
        }
      }

      Position l_Position = PositionAtPoint(new Point(e.X, e.Y));
      if (l_Position.IsEmpty() == false)
      {
        ICellVirtual l_CellMouseDown = GetCell(l_Position);
        if (l_CellMouseDown != null)
        {
          ChangeMouseDownCell(l_Position, l_Position);
          // Cell.OnMouseDown
          PositionMouseEventArgs l_EventArgs = new PositionMouseEventArgs(l_Position, l_CellMouseDown, e);
          l_CellMouseDown.OnMouseDown(l_EventArgs);

          if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && e.Button == MouseButtons.Left) // && Selection.EnableMultiSelection)
          {
            // New Focus Cell Entering
            ICellVirtual l_CellToFocus = GetCell(mouseDownPosition);
            PositionCancelEventArgs l_NewEventArg = new PositionCancelEventArgs(mouseDownPosition, l_CellToFocus);
            l_CellToFocus.OnFocusEntering(l_NewEventArg);
            if (!l_NewEventArg.Cancel)
            {
              Selection.Clear(FocusCellPosition);
              if (!FocusCellPosition.IsEmpty())
              {
                int start = Math.Min(FocusCellPosition.Row, MouseDownPosition.Row);
                int end = Math.Max(FocusCellPosition.Row, MouseDownPosition.Row);
                Selection.AddRange(new Range(start, MouseDownPosition.Column, end, MouseDownPosition.Column));
                SetFocusCell(mouseDownPosition, false);
                OnUserSelection(new UserSelectionEventArgs(selection));
              }
            }
          }
          else if ((Control.ModifierKeys & Keys.Control) == Keys.Control && e.Button == MouseButtons.Left && Selection.EnableMultiSelection)
          {
            // New Focus Cell Entering
            ICellVirtual l_CellToFocus = GetCell(mouseDownPosition);
            PositionCancelEventArgs l_NewEventArg = new PositionCancelEventArgs(mouseDownPosition, l_CellToFocus);
            l_CellToFocus.OnFocusEntering(l_NewEventArg);
            if (!l_NewEventArg.Cancel)
            {
              if (Selection.Contains(mouseDownPosition) == false)
              {
                Selection.Add(mouseDownPosition);
                SetFocusCell(mouseDownPosition, false);
                OnUserSelection(new UserSelectionEventArgs(selection));
              }
              else
              {
                if (mouseDownPosition.Row == FocusCellPosition.Row)
                {
                  PositionCancelEventArgs l_OldEventArg = new PositionCancelEventArgs(FocusCellPosition, GetCell(FocusCellPosition));
                  l_OldEventArg.Cell.OnFocusLeaving(l_OldEventArg);
                  OnCellLostFocus(l_OldEventArg);
                }
                Selection.Remove(mouseDownPosition);
                OnUserSelection(new UserSelectionEventArgs(selection));
              }
            }
          }
          else if (e.Button == MouseButtons.Right)
          {
            contextMenuCellPosition = new Position(mouseDownPosition.Row, mouseDownPosition.Column);
            //// The following code would cause the right-click to additionally
            //// set the focus cell to the cell under the cursor and also
            //// add the focus cell/row to te current selection.
            ////ICellVirtual l_CellToFocus = GetCell(mouseDownPosition);
            ////if (mouseDownPosition.IsEmpty() == false && l_CellToFocus != null)
            ////{
            ////  PositionCancelEventArgs l_NewEventArg = new PositionCancelEventArgs(mouseDownPosition, l_CellToFocus);
            ////  l_CellToFocus.OnFocusEntering(l_NewEventArg);
            ////  if (!l_NewEventArg.Cancel)
            ////  {
            ////    SetFocusCell(mouseDownPosition, false);
            ////  }
            ////}
            ////else
            ////{
            ////  SetFocusCell(mouseDownPosition, false);  // focus permet d'annuler certains evenements
            ////}
          }
          else if (Selection.Contains(mouseDownPosition) == false || e.Button == MouseButtons.Left)
          {
            // New Focus Cell Entering
            ICellVirtual l_CellToFocus = GetCell(mouseDownPosition);
            if (mouseDownPosition.IsEmpty() == false && l_CellToFocus != null)
            {
              PositionCancelEventArgs l_NewEventArg = new PositionCancelEventArgs(mouseDownPosition, l_CellToFocus);
              l_CellToFocus.OnFocusEntering(l_NewEventArg);
              if (!l_NewEventArg.Cancel)
              {
                SetFocusCell(Position.Empty);
                Selection.Clear();
                Selection.Add(mouseDownPosition);
                SetFocusCell(mouseDownPosition, true);
                OnUserSelection(new UserSelectionEventArgs(selection));
              }
            }
            else
            {
              SetFocusCell(mouseDownPosition, false);  // focus permet d'annuler certains evenements
            }
          }
        }
      }
      else
      {
        ChangeMouseDownCell(Position.Empty, Position.Empty);
      }
    }

    private bool isRowHeaderSelectable = false;

    /// <summary>
    /// Gets or sets a value indicating whether the row header can be selected.
    /// </summary>
    /// <value><c>true</c> if row header can be selected; otherwise, <c>false</c>.</value>
    public bool SelectRowHeader
    {
      get { return this.isRowHeaderSelectable; }
      set { this.isRowHeaderSelectable = value; }
    }

    private bool isColumnHeaderSelectable = false;

    /// <summary>
    /// Gets or sets a value indicating whether the column header can be selected.
    /// </summary>
    /// <value><c>true</c> if column header can be selected; otherwise, <c>false</c>.</value>
    public bool SelectColumnHeader
    {
      get { return this.isColumnHeaderSelectable; }
      set { this.isColumnHeaderSelectable = value; }
    }

    /// <summary>
    /// MouseUp event
    /// </summary>
    public new event MouseEventHandler MouseUp;
    /// <summary>
    /// MouseUp event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridMouseUp(MouseEventArgs e)
    {
      if (MouseUp != null)
      {
        MouseUp(this, e);
      }

      if (doDrawLineWhileResizingColumn)
      {
        this.doDrawLineWhileResizingColumn = false;
      }
      if (this.doDrawLineWhileResizingRow)
      {
        this.doDrawLineWhileResizingRow = false;
      }

      MouseSelectionFinish();

      if (mouseDownPosition.IsEmpty() == false)
      {
        ICellVirtual l_MouseDownCell = GetCell(mouseDownPosition);
        if (l_MouseDownCell != null)
        {
          l_MouseDownCell.OnMouseUp(new PositionMouseEventArgs(mouseDownPosition, l_MouseDownCell, e));
        }
        ChangeMouseDownCell(Position.Empty, PositionAtPoint(new Point(e.X, e.Y)));
      }
    }

    /// <summary>
    /// MouseMove event
    /// </summary>
    public new event MouseEventHandler MouseMove;
    /// <summary>
    /// MouseMove event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridMouseMove(MouseEventArgs e)
    {
      if (MouseMove != null)
      {
        MouseMove(this, e);
      }

      Position l_PointPosition = PositionAtPoint(new Point(e.X, e.Y));
      ICellVirtual l_CellPosition = GetCell(l_PointPosition);
      // Call MouseMove on the cell that receive that MouseDown event if it is
      // an IHeaderCell that can resize itself
      IHeaderCell l_HeaderDownCell = null;
      if (!mouseDownPosition.IsEmpty())
      {
        l_HeaderDownCell = GetCell(mouseDownPosition) as IHeaderCell;
      }
      if (l_HeaderDownCell != null)
      {
        l_HeaderDownCell.OnMouseMove(new PositionMouseEventArgs(mouseDownPosition, l_HeaderDownCell, e));
      }
      else if (!l_PointPosition.IsEmpty() && l_CellPosition != null)
      {
        ChangeMouseCell(l_PointPosition);
        l_CellPosition.OnMouseMove(new PositionMouseEventArgs(l_PointPosition, l_CellPosition, e));
      }

      // Mouse Multiselection
      if (e.Button == MouseButtons.Left && Selection.EnableMultiSelection)
      {
        Position l_SelCornerPos = l_PointPosition; // PositionAtPoint(new Point(e.X,e.Y),false);

        if (l_SelCornerPos.IsEmpty() == false)
        {
          ICellVirtual l_SelCorner = l_CellPosition;

          if (l_SelCorner != null)
          {
            // Only if there is a FocusCell
            // and the user start the selection with a cell (m_MouseDownCell!=null)
            if (focusPosition.IsEmpty() == false)
            {
              ICellVirtual l_FocusCell = GetCell(focusPosition);

              if (l_FocusCell != null && l_FocusCell.IsEditing(focusPosition) == false)
              {
                if (mouseDownPosition.IsEmpty() == false && Selection.Contains(mouseDownPosition))
                {
                  switch (this.Selection.SelectionMode)
                  {
                    case GridSelectionMode.Row:

                      if (FixedRows < l_SelCornerPos.Row + 1 && FixedColumns < l_SelCornerPos.Column + 1)
                      {
                        ChangeMouseSelectionCorner(l_SelCornerPos);
                        ShowCell(l_SelCornerPos);
                      }
                      break;
                    case GridSelectionMode.Column:

                      if (FixedColumns < l_SelCornerPos.Column + 1)
                      {
                        ChangeMouseSelectionCorner(l_SelCornerPos);
                        ShowCell(l_SelCornerPos);
                      }
                      break;
                  }
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// MouseWheel event
    /// </summary>
    public new event MouseEventHandler MouseWheel;

    /// <summary>
    /// MouseWheel event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridMouseWheel(MouseEventArgs e)
    {
      if (MouseWheel != null)
      {
        MouseWheel(this, e);
      }

      try
      {
        if (e.Delta >= 120 || e.Delta <= -120)
        {
          Point t = CustomScrollPosition;
          int l_NewY = t.Y +
              (SystemInformation.MouseWheelScrollLines * 6) *
              Math.Sign(e.Delta);

          // check that the value is between max and min
          if (l_NewY > 0)
          {
            l_NewY = 0;
          }
          if (l_NewY < (-this.MaximumVScroll))
          {
            l_NewY = -this.MaximumVScroll;
          }

          CustomScrollPosition = new Point(t.X, l_NewY);
        }
      }
      catch (Exception ex)
      {
        // SAA TODO: Avoid improper catch-discard.
        LoggerManager.Log(LogLevels.Error, "Unexpected exception caught: " + ex.ToString());
      }
    }

    /// <summary>
    /// MouseLeave event
    /// </summary>
    public new event EventHandler MouseLeave;
    /// <summary>
    /// MouseLeave event attached to a Panel
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelMouseLeave(EventArgs e)
    {
      if (MouseLeave != null)
      {
        MouseLeave(this, e);
      }

      ChangeMouseCell(Position.Empty);

      MouseSelectionFinish();
    }

    /// <summary>
    /// MouseEnter event
    /// </summary>
    public new event EventHandler MouseEnter;
    /// <summary>
    /// MouseEnter event attached to a Panel
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelMouseEnter(EventArgs e)
    {
      if (MouseEnter != null)
      {
        MouseEnter(this, e);
      }
    }

    /// <summary>
    /// Mouse Hover
    /// </summary>
    public new event EventHandler MouseHover;
    /// <summary>
    /// Raises the <see cref="E:GridMouseHover"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnGridMouseHover(EventArgs e)
    {
      if (MouseHover != null)
      {
        MouseHover(this, e);
      }
    }

    /// <summary>
    /// Fired when an user scroll with the mouse wheel
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
    protected override void OnMouseWheel(MouseEventArgs e)
    {
      base.OnMouseWheel(e);
      OnGridMouseWheel(e);
    }
    #endregion

    #region Drag Events
    /// <summary>
    /// DragDrop event
    /// </summary>
    public new event DragEventHandler DragDrop;
    /// <summary>
    /// DragDrop event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelDragDrop(DragEventArgs e)
    {
      if (DragDrop != null)
      {
        DragDrop(this, e);
      }
    }
    /// <summary>
    /// DragEnter event
    /// </summary>
    public new event DragEventHandler DragEnter;
    /// <summary>
    /// DragEnter event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelDragEnter(DragEventArgs e)
    {
      if (DragEnter != null)
      {
        DragEnter(this, e);
      }
    }
    /// <summary>
    /// DragLeave event
    /// </summary>
    public new event EventHandler DragLeave;
    /// <summary>
    /// DragDrop event
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelDragLeave(EventArgs e)
    {
      if (DragLeave != null)
      {
        DragLeave(this, e);
      }
    }
    /// <summary>
    /// DragOver event
    /// </summary>
    public new event DragEventHandler DragOver;
    /// <summary>
    /// DragOver event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
    public virtual void OnPanelDragOver(DragEventArgs e)
    {
      if (DragOver != null)
      {
        DragOver(this, e);
      }
    }
    #endregion

    #region Click Events
    /// <summary>
    /// Click event
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnGridClick(EventArgs e)
    {
      if (this.click != null)
      {
        this.click(this, e);
      }

      if (mouseDownPosition.IsEmpty() == false &&
          mouseDownPosition == PositionAtPoint(this.PointToClient(Control.MousePosition)))
      {
        ICellVirtual l_MouseDownCell = GetCell(mouseDownPosition);
        if (l_MouseDownCell != null)
        {
          l_MouseDownCell.OnClick(new PositionEventArgs(mouseDownPosition, l_MouseDownCell));
        }
      }
    }

    /// <summary>
    /// DoubleClick event
    /// </summary>
    public new event EventHandler DoubleClick;
    /// <summary>
    /// Double-Click event
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public virtual void OnGridDoubleClick(EventArgs e)
    {
      if (DoubleClick != null)
      {
        DoubleClick(this, e);
      }

      if (mouseDownPosition.IsEmpty() == false)
      {
        ICellVirtual l_MouseDownCell = GetCell(mouseDownPosition);
        if (l_MouseDownCell != null)
        {
          l_MouseDownCell.OnDoubleClick(new PositionEventArgs(mouseDownPosition, l_MouseDownCell));
        }
      }
    }
    #endregion

    #region Keys
    /// <summary>
    /// KeyDown event
    /// </summary>
    public new event KeyEventHandler KeyDown;
    /// <summary>
    /// KeyDown event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridKeyDown(KeyEventArgs e)
    {
      if (KeyDown != null)
      {
        KeyDown(this, e);
      }

      ProcessSpecialGridKey(e);
    }

    /// <summary>
    /// KeyUp event
    /// </summary>
    public new event KeyEventHandler KeyUp;
    /// <summary>
    /// KeyUp event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridKeyUp(KeyEventArgs e)
    {
      if (KeyUp != null)
      {
        KeyUp(this, e);
      }

      if (focusPosition.IsEmpty() == false)
      {
        ICellVirtual l_FocusCell = GetCell(focusPosition);
        if (l_FocusCell != null)
        {
          l_FocusCell.OnKeyUp(new PositionKeyEventArgs(focusPosition, l_FocusCell, e));
        }
      }
    }

    /// <summary>
    /// KeyPress event
    /// </summary>
    public new event KeyPressEventHandler KeyPress;

    /// <summary>
    /// KeyPress event
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
    public virtual void OnGridKeyPress(KeyPressEventArgs e)
    {
      if (KeyPress != null)
      {
        KeyPress(this, e);
      }

      // Only if different from tab and head (nor a copy/paste command)
      if (focusPosition.IsEmpty() || e.KeyChar == '\t' || e.KeyChar == 13 ||
          e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24)
      {
        return;
      }

      ICellVirtual l_FocusCell = GetCell(focusPosition);
      if (l_FocusCell != null)
      {
        l_FocusCell.OnKeyPress(new PositionKeyPressEventArgs(focusPosition, l_FocusCell, e));
      }
    }
    #endregion

    #region Controls linked
    /// <summary>
    /// Gets the list of controls that are linked to a specific cell position. For example is used for editors controls. Key=Control, Value=Position. The controls are automatically removed from the list when they are removed from the Grid.Controls collection
    /// </summary>
    /// <value>The linked controls.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinkedControlsList LinkedControls
    {
      get { return this.linkedControls; }
    }

    /// <summary>
    /// OnHScrollPositionChanged
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ScrollPositionChangedEventArgs"/> instance containing the event data.</param>
    protected override void OnHScrollPositionChanged(ScrollPositionChangedEventArgs e)
    {
      base.OnHScrollPositionChanged(e);

      if (Redraw && !this.DrawColLine)
      {
        RefreshLinkedControlsBounds();
      }
    }

    /// <summary>
    /// OnVScrollPositionChanged
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ScrollPositionChangedEventArgs"/> instance containing the event data.</param>
    protected override void OnVScrollPositionChanged(ScrollPositionChangedEventArgs e)
    {
      base.OnVScrollPositionChanged(e);

      if (Redraw && !this.DrawColLine)
      {
        RefreshLinkedControlsBounds();
      }
    }

    /// <summary>
    /// Refresh the linked controls bounds
    /// </summary>
    public virtual void RefreshLinkedControlsBounds()
    {
      foreach (DictionaryEntry e in this.linkedControls)
      {
        Position l_CellPosition = (Position)e.Value;
        Control l_Control = (Control)e.Key;
        GridSubPanel l_Panel = PanelAtPosition(l_CellPosition);
        if (l_Panel == null)
        {
          throw new MEDDataGridException("Invalid position, panel not found");
        }

        ICellVirtual l_Cell = GetCell(l_CellPosition);
        if (l_Cell != null || this.linkedControls.UseCellBorder == false)
        {
          l_Control.Bounds = l_Cell.VisualModel.Border.RemoveBorderFromRectangle(l_Panel.RectangleGridToPanel(PositionToDisplayRect(l_CellPosition)));
        }
        else
        {
          l_Control.Bounds = l_Panel.RectangleGridToPanel(PositionToDisplayRect(l_CellPosition));
        }
      }
    }

    /// <summary>
    /// Fired when you remove a linked control from the grid.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"></see> that contains the event data.</param>
    protected override void OnControlRemoved(ControlEventArgs e)
    {
      base.OnControlRemoved(e);

      if (LinkedControls.ContainsKey(e.Control))
      {
        LinkedControls.Remove(e.Control);
      }
    }
    #endregion

    #region Layout
    /// <summary>
    /// Temporarily suspends the layout logic for the control and all the children panels controls.
    /// </summary>
    public virtual void SuspendLayoutGrid()
    {
      SuspendLayout();
      this.topLeftPanel.SuspendLayout();
      this.leftPanel.SuspendLayout();
      this.topPanel.SuspendLayout();
      this.scrollablePanel.SuspendLayout();
    }

    /// <summary>
    /// Resumes the grid layout logic.
    /// </summary>
    /// <remarks>
    /// Resumes normal layout logic to current control and children controls and forces
    /// an immediate layout of pending layout requests.
    /// </remarks>
    public virtual void ResumeLayoutGrid()
    {
      this.scrollablePanel.ResumeLayout();
      this.topPanel.ResumeLayout();
      this.leftPanel.ResumeLayout();
      this.topLeftPanel.ResumeLayout();
      ResumeLayout(true);
    }

    /// <summary>
    /// Refreshes the grid layout.
    /// </summary>
    /// <remarks>
    /// Recalculate the scrollbar position and value based on the current cells,
    /// scroll client area, linked controls and more. If redraw == false this
    /// method has no effect. This method is called when you set Redraw = true;
    /// </remarks>
    protected void RefreshGridLayout()
    {
      CustomScrollArea = new Size(Columns.Right, Rows.Bottom);

      CalculatePanelsLocation();

      InvalidateCells();

      RefreshLinkedControlsBounds();
    }

    private bool doRedraw = true;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GridVirtual"/> is redrawn
    /// when any change occurs.
    /// </summary>
    /// <value><c>true</c> if redraw; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// If false the cells are not redrawed. Set False to increase performance when adding many cells, after adding the cells remember to set this property to true.
    /// </remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Redraw
    {
      get
      {
        return this.doRedraw;
      }
      set
      {
        this.doRedraw = value;
        if (doRedraw)
        {
          RefreshGridLayout();
          ResumeLayoutGrid();
        }
        else
        {
          SuspendLayoutGrid();
        }
      }
    }

    /// <summary>
    /// Invalidate all the cells.
    /// </summary>
    public virtual void InvalidateCells()
    {
      InvalidateScrollableArea();
    }

    /// <summary>
    /// OnLayout Method
    /// </summary>
    /// <param name="e">The <see cref="System.Windows.Forms.LayoutEventArgs"/> instance containing the event data.</param>
    protected override void OnLayout(LayoutEventArgs e)
    {
      base.OnLayout(e);

      if (Redraw && !this.DrawColLine)
      {
        RefreshGridLayout();
      }
    }
    #endregion

    #region Sorting
    /// <summary>
    /// Gets or sets a value indicating whether sorting is enabled.
    /// </summary>
    /// <value><c>true</c> if sorting is enabled; otherwise, <c>false</c>.</value>
    public bool EnableSort
    {
      get { return this.doEnableSort; }
      set { this.doEnableSort = value; }
    }

    /// <summary>
    /// Sort a range of the grid
    /// </summary>
    /// <param name="p_RangeToSort">Range to sort</param>
    /// <param name="p_AbsoluteColKeys">Index of the column relative to the grid to use as sort keys, must be between start and end column of the range</param>
    /// <param name="p_bAsc">Ascending true, Descending false</param>
    /// <param name="p_CellComparer">CellComparer, if null the default comparer will be used</param>
    public void SortRangeRows(IRangeLoader p_RangeToSort,
        int p_AbsoluteColKeys,
        bool p_bAsc,
        IComparer p_CellComparer)
    {
      Range l_Range = p_RangeToSort.GetRange(this);
      SortRangeRows(l_Range, p_AbsoluteColKeys, p_bAsc, p_CellComparer);
    }

    /// <summary>
    /// Sort a range of the grid.
    /// </summary>
    /// <param name="p_Range">The range.</param>
    /// <param name="p_AbsoluteColKeys">Index of the column relative to the grid to use as sort keys, must be between start and end col</param>
    /// <param name="p_bAscending">Ascending true, Descending false</param>
    /// <param name="p_CellComparer">CellComparer, if null the default ValueCellComparer comparer will be used</param>
    public void SortRangeRows(Range p_Range,
        int p_AbsoluteColKeys,
        bool p_bAscending,
        IComparer p_CellComparer)
    {
      this.Cursor = Cursors.WaitCursor;
      bool l_oldRedraw = Redraw;
      Redraw = false;
      isSorting = true;

      try
      {
        SortRangeRowsEventArgs eventArgs = new SortRangeRowsEventArgs(p_Range, p_AbsoluteColKeys, p_bAscending, p_CellComparer);
        OnSortingRangeRows(eventArgs);
        OnSortedRangeRows(eventArgs);
      }
      finally
      {
        Redraw = l_oldRedraw;
        isSorting = false;
      }

      this.Cursor = Cursors.Arrow;
    }

    /// <summary>
    /// Fired when user action changes selection
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.UserSelectionEventArgs"/> instance containing the event data.</param>
    protected virtual void OnUserSelection(UserSelectionEventArgs e)
    {
      if (userSelection != null)
      {
        userSelection(this, e);
      }
    }

    /// <summary>
    /// Fired when calling SortRangeRows method
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.SortRangeRowsEventArgs"/> instance containing the event data.</param>
    protected virtual void OnSortingRangeRows(SortRangeRowsEventArgs e)
    {
      if (this.sortingRangeRows != null)
      {
        this.sortingRangeRows(this, e);
      }
    }

    /// <summary>
    /// Fired when calling SortRangeRows method
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.SortRangeRowsEventArgs"/> instance containing the event data.</param>
    protected virtual void OnSortedRangeRows(SortRangeRowsEventArgs e)
    {
      if (this.sortedRangeRows != null)
      {
        this.sortedRangeRows(this, e);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is currently sorting.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is sorting; otherwise, <c>false</c>.
    /// </value>
    public bool IsSorting
    {
      get { return this.isSorting; }
      set { this.isSorting = value; }
    }
    #endregion

    #region ProcessCmdKey
    /// <summary>
    /// Processes a command key.
    /// </summary>
    /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"></see>, passed by reference, that represents the window message to process.</param>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
    /// <returns>
    /// <c>true</c> if the character was processed by the control; otherwise, <c>false</c>.
    /// </returns>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    protected override bool ProcessCmdKey(
        ref Message msg,
        Keys keyData
        )
    {
      bool l_EnableEscape = (this.gridSpecialKeys & GridSpecialKeys.Escape) == GridSpecialKeys.Escape;
      bool l_EnableEnter = (this.gridSpecialKeys & GridSpecialKeys.Enter) == GridSpecialKeys.Enter;
      bool l_EnableTab = (this.gridSpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab;

      if (keyData == Keys.Escape && l_EnableEscape)
      {
        ICellVirtual l_FocusCell = GetCell(focusPosition);
        if (l_FocusCell != null && l_FocusCell.IsEditing(focusPosition))
        {
          if (l_FocusCell.EndEdit(true))
          {
            return true;
          }
        }
      }

      // in questo caso il tasto viene sempre considerato processato
      if (keyData == Keys.Enter && l_EnableEnter)
      {
        ICellVirtual l_FocusCell = GetCell(focusPosition);
        if (l_FocusCell != null && l_FocusCell.IsEditing(focusPosition))
        {
          l_FocusCell.EndEdit(false);

          return true;
        }
      }

      // in questo caso il tasto viene considerato processato
      // solo se la cella era in editing e l'editing non riesce
      if (keyData == Keys.Tab && l_EnableTab)
      {
        ICellVirtual l_FocusCell = GetCell(focusPosition);
        if (l_FocusCell != null && l_FocusCell.IsEditing(focusPosition))
        {
          // se l'editing non riesce considero il tasto processato
          // altrimenti no, in questo modo il tab ha effetto anche per lo spostamento
          if (l_FocusCell.EndEdit(false) == false)
          {
            return true;
          }

          // altrimenti scateno anche il muovimento della cella
          ProcessSpecialGridKey(new KeyEventArgs(keyData));
          return true; // considero il tasto processato altrimenti si sposta ancora il focus
        }
      }

      return base.ProcessCmdKey(ref msg, keyData);
    }
    #endregion

    #region GetCell/SetCell
    /// <summary>
    /// Return the Cell at the specified Row and Column position. Simply call GettingCell event.
    /// </summary>
    /// <param name="rowIndex">The row.</param>
    /// <param name="columnIndex">The column.</param>
    /// <returns></returns>
    public virtual ICellVirtual GetCell(int rowIndex, int columnIndex)
    {
      if (GettingCell != null)
      {
        PositionEventArgs e = new PositionEventArgs(new Position(rowIndex, columnIndex), null);
        GettingCell(this, e);
        return e.Cell;
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Set the specified cell in the specified position. Simply call SettingCell event.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <param name="columnIndex">Index of the column.</param>
    /// <param name="p_Cell">The cell.</param>
    public virtual void SetCell(int rowIndex, int columnIndex, ICellVirtual p_Cell)
    {
      if (SettingCell != null)
      {
        SettingCell(this, new PositionEventArgs(new Position(rowIndex, columnIndex), p_Cell));
      }
    }

    /// <summary>
    /// Fired when GetCell is called with GridVirtual class. Use the e.Cell property to set the cell class
    /// </summary>
    public event PositionEventHandler GettingCell;

    /// <summary>
    /// Fired when SetCell is called with GridVirtual class. Read the e.Cell property to get the cell class
    /// </summary>
    public event PositionEventHandler SettingCell;
    #endregion

    #region Panels
    /// <summary>
    /// Gets the non-scrollable left panel (For RowHeader)
    /// </summary>
    /// <value>The left panel.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public GridSubPanel LeftPanel
    {
      get { return this.leftPanel; }
    }

    /// <summary>
    /// Gets the non-scrollable top panel (For ColHeader)
    /// </summary>
    /// <value>The top panel.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public GridSubPanel TopPanel
    {
      get { return this.topPanel; }
    }

    /// <summary>
    /// Gets the non-scrollable top+left panel (For Row or Colunm Header)
    /// </summary>
    /// <value>The top left panel.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public GridSubPanel TopLeftPanel
    {
      get { return this.topLeftPanel; }
    }

    /// <summary>
    /// Gets the scrollable panel for normal scrollable cells
    /// </summary>
    /// <value>The scrollable panel.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public GridSubPanel ScrollablePanel
    {
      get { return this.scrollablePanel; }
    }

    /// <summary>
    /// Gets the hidden panel for internal use only. I use this panel to catch mouse and keyboard events.
    /// </summary>
    /// <value>The hidden focus panel.</value>
    protected GridSubPanel HiddenFocusPanel
    {
      get { return this.hiddenFocusPanel; }
    }

    /// <summary>
    /// Recalculate panel position
    /// </summary>
    private void CalculatePanelsLocation()
    {
      int l_Height = 0;
      if (Rows.Count >= FixedRows && FixedRows > 0)
      {
        l_Height = Rows[FixedRows - 1].Bottom;
      }

      int l_Width = 0;
      if (Columns.Count >= FixedColumns && FixedColumns > 0)
      {
        l_Width = Columns[FixedColumns - 1].Right;
      }

      Rectangle l_DisplayRectangle = DisplayRectangle;
      topLeftPanel.Size = new Size(l_Width, l_Height);
      leftPanel.Location = new Point(0, l_Height);
      leftPanel.Size = new Size(l_Width, l_DisplayRectangle.Height - l_Height);
      topPanel.Location = new Point(l_Width, 0);
      topPanel.Size = new Size(l_DisplayRectangle.Width - l_Width, l_Height);
      scrollablePanel.Location = new Point(l_Width, l_Height);
      scrollablePanel.Size = new Size(l_DisplayRectangle.Width - l_Width, l_DisplayRectangle.Height - l_Height);
    }

    /// <summary>
    /// Get the panels that contains the specified cells position. Returns null if the position is not valid
    /// </summary>
    /// <param name="cellPosition">The cell position.</param>
    /// <returns></returns>
    public GridSubPanel PanelAtPosition(Position cellPosition)
    {
      if (cellPosition.IsEmpty() == false)
      {
        CellPositionType l_Type = GetPositionType(cellPosition);
        if (l_Type == CellPositionType.FixedTopLeft)
        {
          return this.topLeftPanel;
        }
        else if (l_Type == CellPositionType.FixedLeft)
        {
          return this.leftPanel;
        }
        else if (l_Type == CellPositionType.FixedTop)
        {
          return this.topPanel;
        }
        else if (l_Type == CellPositionType.Scrollable)
        {
          return this.scrollablePanel;
        }
        else
        {
          return null;
        }
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Set the focus on the control that contains the cells.
    /// </summary>
    /// <returns></returns>
    public bool SetFocusOnCells()
    {
      return HiddenFocusPanel.Focus();
    }

    /// <summary>
    /// Gets a value indicating whether the cells have the focus.
    /// </summary>
    /// <value><c>true</c> if cells contain focus; otherwise, <c>false</c>.</value>
    /// <returns></returns>
    public bool CellsContainsFocus
    {
      get { return HiddenFocusPanel.ContainsFocus; }
    }

    /// <summary>
    /// Invalidate the scrollable area
    /// </summary>
    protected override void InvalidateScrollableArea()
    {
      this.scrollablePanel.Refresh();
      this.topLeftPanel.Refresh();
      this.leftPanel.Refresh();
      this.topPanel.Refresh();
    }
    #endregion

    #region Rows, Columns
    /// <summary>
    /// Gets or sets the number of columns
    /// </summary>
    /// <value>The columns count.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int ColumnsCount
    {
      get
      {
        return this.columns.Count;
      }
      set
      {
        if (this.columns.Count < value)
        {
          this.columns.InsertRange(this.columns.Count, value - ColumnsCount);
        }
        else if (this.columns.Count > value)
        {
          this.columns.RemoveRange(value, this.columns.Count - value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the number of rows
    /// </summary>
    /// <value>The rows count.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int RowsCount
    {
      get
      {
        return this.rows.Count;
      }
      set
      {
        if (this.rows.Count < value)
        {
          this.rows.InsertRange(this.rows.Count, value - RowsCount);
        }
        else if (this.rows.Count > value)
        {
          this.rows.RemoveRange(value, this.rows.Count - value);
        }
      }
    }

    /// <summary>
    /// Gets or sets how many rows are not scrollable
    /// </summary>
    /// <value>The fixed rows.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int FixedRows
    {
      get { return this.fixedRows; }
      set { this.fixedRows = value; }
    }

    /// <summary>
    /// Gets or sets how many cols are not scrollable
    /// </summary>
    /// <value>The fixed columns.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int FixedColumns
    {
      get { return this.fixedColumns; }
      set { this.fixedColumns = value; }
    }

    /// <summary>
    /// Gets Rows information.
    /// </summary>
    /// <value>The rows.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RowInfo.RowInfoCollection Rows
    {
      get { return this.rows; }
    }

    /// <summary>
    /// Gets Columns information.
    /// </summary>
    /// <value>The columns.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ColumnInfo.ColumnInfoCollection Columns
    {
      get { return this.columns; }
    }

    /// <summary>
    /// Handles the RowHeightChanged event of the rows control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.RowInfoEventArgs"/> instance containing the event data.</param>
    private void rows_RowHeightChanged(object sender, RowInfoEventArgs e)
    {
      if (Redraw && !this.DrawColLine)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Handles the ColumnWidthChanged event of the columns control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ColumnInfoEventArgs"/> instance containing the event data.</param>
    private void columns_ColumnWidthChanged(object sender, ColumnInfoEventArgs e)
    {
      if (Redraw && !this.DrawColLine)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Handles the RowsAdded event of the rows control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
    private void rows_RowsAdded(object sender, IndexRangeEventArgs e)
    {
      if (Redraw && !this.DrawColLine)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Handles the ColumnsAdded event of the columns control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.IndexRangeEventArgs"/> instance containing the event data.</param>
    private void columns_ColumnsAdded(object sender, IndexRangeEventArgs e)
    {
      if (Redraw && !this.DrawColLine)
      {
        RefreshGridLayout();
      }
    }

    /// <summary>
    /// Returns the type of a cell position
    /// </summary>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <returns></returns>
    public CellPositionType GetPositionType(Position p_CellPosition)
    {
      if (p_CellPosition.IsEmpty())
      {
        return CellPositionType.Empty;
      }
      else if (p_CellPosition.Row < FixedRows && p_CellPosition.Column < FixedColumns)
      {
        return CellPositionType.FixedTopLeft;
      }
      else if (p_CellPosition.Row < FixedRows)
      {
        return CellPositionType.FixedTop;
      }
      else if (p_CellPosition.Column < FixedColumns)
      {
        return CellPositionType.FixedLeft;
      }
      else
      {
        return CellPositionType.Scrollable;
      }
    }

    /// <summary>
    /// Gets a Range that represents the complete cells of the grid
    /// </summary>
    /// <value>The complete range.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Range CompleteRange
    {
      get
      {
        if (RowsCount > 0 && ColumnsCount > 0)
        {
          return new Range(0, 0, RowsCount - 1, ColumnsCount - 1);
        }
        else
        {
          return Range.Empty;
        }
      }
    }
    #endregion

    #region Position Search (PositionAtPoint)
    /// <summary>
    /// Returns the cell at the specified grid view relative point (the point must be relative to the grid display region), SearchInFixedCells = true. Return Empty if no valid cells are found
    /// </summary>
    /// <param name="p_RelativeViewPoint">Point</param>
    /// <returns></returns>
    public virtual Position PositionAtPoint(Point p_RelativeViewPoint)
    {
      return PositionAtPoint(p_RelativeViewPoint, true);
    }

    /// <summary>
    /// Returns the cell at the specified grid view relative point (the point must be relative to the grid display region)
    /// </summary>
    /// <param name="p_RelativeViewPoint">Point</param>
    /// <param name="p_bSearchInFixedCells">True if you want to consider fixed cells in the search</param>
    /// <returns></returns>
    public virtual Position PositionAtPoint(Point p_RelativeViewPoint, bool p_bSearchInFixedCells)
    {
      Position l_Found = Position.Empty;
      if (p_bSearchInFixedCells)
      {
        l_Found = TopLeftPanel.PositionAtPoint(p_RelativeViewPoint);
        if (l_Found.IsEmpty())
        {
          l_Found = LeftPanel.PositionAtPoint(p_RelativeViewPoint);
          if (l_Found.IsEmpty())
          {
            l_Found = TopPanel.PositionAtPoint(p_RelativeViewPoint);
          }
        }
      }

      if (l_Found.IsEmpty())
      {
        l_Found = ScrollablePanel.PositionAtPoint(p_RelativeViewPoint);
      }

      return l_Found;
    }
    #endregion

    #region RangeSearch (RangeAtRectangle)
    /// <summary>
    /// Returns a range of cells inside an absolute rectangle
    /// </summary>
    /// <param name="p_AbsoluteRect">The absolute rectangle.</param>
    /// <returns></returns>
    public Range RangeAtAbsRect(Rectangle p_AbsoluteRect)
    {
      int l_Start_R, l_Start_C, l_End_R, l_End_C;

      l_Start_R = Rows.RowAtPoint(p_AbsoluteRect.Y);
      l_Start_C = Columns.ColumnAtPoint(p_AbsoluteRect.X);

      l_End_R = Rows.RowAtPoint(p_AbsoluteRect.Bottom);
      l_End_C = Columns.ColumnAtPoint(p_AbsoluteRect.Right);

      if (l_Start_R == Position.EmptyIndex || l_Start_C == Position.EmptyIndex
          || l_End_C == Position.EmptyIndex || l_End_R == Position.EmptyIndex)
      {
        return Range.Empty;
      }

      return new Range(l_Start_R, l_Start_C, l_End_R, l_End_C);
    }
    #endregion

    #region ToolTip and Cursor
    /// <summary>
    /// Gets or sets a value indicating whether the grid tool tip is active.
    /// </summary>
    /// <value><c>true</c> if grid tool tip is active; otherwise, <c>false</c>.</value>
    public bool GridToolTipActive
    {
      get
      {
        return ScrollablePanel.ToolTipActive;
      }
      set
      {
        ScrollablePanel.ToolTipActive = value;
        LeftPanel.ToolTipActive = value;
        TopPanel.ToolTipActive = value;
        TopLeftPanel.ToolTipActive = value;
      }
    }

    /// <summary>
    /// Gets or sets cursor for the container of the cells. This property is used when you set a cursor to a specified cell.
    /// </summary>
    /// <value>The grid cursor.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Cursor GridCursor
    {
      get
      {
        return ScrollablePanel.Cursor;
      }
      set
      {
        ScrollablePanel.Cursor = value;
        LeftPanel.Cursor = value;
        TopPanel.Cursor = value;
        TopLeftPanel.Cursor = value;
      }
    }

    /// <summary>
    /// Gets or sets ToolTip text of the container of the cells. This property is used when you set a tooltip to a specified cell.
    /// </summary>
    /// <value>The grid tool tip text.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string GridToolTipText
    {
      get
      {
        return ScrollablePanel.ToolTipText;
      }
      set
      {
        ScrollablePanel.ToolTipText = value;
        LeftPanel.ToolTipText = value;
        TopPanel.ToolTipText = value;
        TopLeftPanel.ToolTipText = value;
      }
    }
    #endregion

    #region Abstract Methods
    /// <summary>
    /// Return the Cell at the specified Row and Column position. This method is called for sort
    /// operations and for Move operations. If position is Empty return <c>null</c>. This method calls GetCell(int rowIndex, int columnIndex)
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public ICellVirtual GetCell(Position position)
    {
      if (position.IsEmpty())
      {
        return null;
      }

      return GetCell(position.Row, position.Column);
    }

    /// <summary>
    /// Set the specified cell in the specified position. This method calls SetCell(int rowIndex, int columnIndex, ICellVirtual p_Cell)
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="cell">The cell.</param>
    public void SetCell(Position position, ICellVirtual cell)
    {
      SetCell(position.Row, position.Column, cell);
    }
    #endregion

    #region Resize Column / Rows
    private bool doDrawLineWhileResizingColumn = false;
    public bool DrawColLine
    {
      get { return doDrawLineWhileResizingColumn; }
      set { doDrawLineWhileResizingColumn = value; }
    }

    private bool doDrawLineWhileResizingRow = false;
    public bool DrawRowLine
    {
      get { return doDrawLineWhileResizingColumn; }
      set { doDrawLineWhileResizingColumn = value; }
    }
    #endregion

    /// <summary>
    /// Gets or sets a value indicating whether to do default clipboard operations.
    /// </summary>
    /// <remarks>
    /// This should be set to <c>false</c> if you are using custom Copy, Cut and Paste
    /// event code.
    /// </remarks>
    /// <value>
    /// <c>true</c> if do default clipboard operations; otherwise, <c>false</c>.
    /// </value>
    public bool DoDefaultClipboardOperations
    {
      get { return this.doDefaultClipboardOperations; }
      set { this.doDefaultClipboardOperations = value; }
    }
  }
}