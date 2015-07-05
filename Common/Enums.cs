#region MIT License
//
// Filename: Enums.cs
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

// SAA TODO: Split this into multiple files.

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// Selection Mode
  /// </summary>
  [ComVisible(false)]
  public enum GridSelectionMode
  {
    /// <summary>
    /// Cell selection
    /// </summary>
    Cell,
    /// <summary>
    /// Row selection
    /// </summary>
    Row,
    /// <summary>
    /// Column selection
    /// </summary>
    Column
  }

  /// <summary>
  /// Sort Mode
  /// </summary>
  [ComVisible(false)]
  public enum GridSortMode
  {
    /// <summary>
    /// None
    /// </summary>
    None = 0,
    /// <summary>
    /// Ascending sort
    /// </summary>
    Ascending = 1,
    /// <summary>
    /// Descending sort
    /// </summary>
    Descending = 2
  }

  /// <summary>
  /// ContextMenu
  /// </summary>
  [Flags]
  public enum ContextMenuStyles
  {
    None = 0,
    ColumnResize = 1,
    RowResize = 2,
    AutoSize = 4,
    ClearSelection = 8,
    CopyPasteSelection = 16,
    CellContextMenu = 32,
    ////SelectAll = 35,
    ////RowSelection = 36
  }

  /// <summary>
  /// Editable Cell mode
  /// </summary>
  [Flags]
  public enum EditableModes
  {
    /// <summary>
    /// No edit support
    /// </summary>
    None = 0,
    /// <summary>
    /// Edit the cell with F2 key ( 1 )
    /// </summary>
    F2Key = 1,
    /// <summary>
    /// Edit the cell with a double click (2)
    /// </summary>
    DoubleClick = 2,
    /// <summary>
    /// Edit a cell with a single click (4)
    /// </summary>
    SingleClick = 4,
    /// <summary>
    /// Edit the cell pressing any keys (8)
    /// </summary>
    AnyKey = 8,
    /// <summary>
    /// Edit the cell when it receive the focus (16)
    /// </summary>
    Focus = 16,
    /// <summary>
    /// DoubleClick + F2Key
    /// </summary>
    Default = DoubleClick | F2Key | AnyKey
  }

  /// <summary>
  /// Border Style
  /// </summary>
  [ComVisible(false)]
  public enum CommonBorderStyle
  {
    /// <summary>
    /// Normal
    /// </summary>
    Normal,
    /// <summary>
    /// Raised
    /// </summary>
    Raised,
    /// <summary>
    /// Inset
    /// </summary>
    Inset
  }

  /// <summary>
  /// Type of resize of the cells
  /// </summary>
  [Flags]
  public enum CellResizeModes
  {
    /// <summary>
    /// None
    /// </summary>
    None = 0,
    /// <summary>
    /// Height
    /// </summary>
    Height = 1,
    /// <summary>
    /// Width
    /// </summary>
    Width = 2,
    /// <summary>
    /// Height and width
    /// </summary>
    Both = 3
  }

  /// <summary>
  /// Special keys that the grid can handle. You can change this enum to block or allow some special keys function.
  /// </summary>
  [Flags]
  public enum GridSpecialKeys
  {
    /// <summary>
    /// No keys
    /// </summary>
    None = 0,
    /// <summary>
    /// Ctrl+C for Copy selection operation
    /// </summary>
    Ctrl_C = 1,
    /// <summary>
    /// Ctrl+V for paste selection operation
    /// </summary>
    Ctrl_V = 2,
    /// <summary>
    /// Ctrl+X for cut selection operation
    /// </summary>
    Ctrl_X = 4,
    /// <summary>
    /// Delete key, for Clear selection operation
    /// </summary>
    Delete = 8,
    /// <summary>
    /// Arrows keys, for moving focus cell operation
    /// </summary>
    Arrows = 16,
    /// <summary>
    /// Tab and Shift+Tab keys, for moving focus cell operation
    /// </summary>
    Tab = 32,
    /// <summary>
    /// PageDown and PageUp keys, for page operation
    /// </summary>
    PageDownUp = 64,
    /// <summary>
    /// Enter key, for apply editing operation
    /// </summary>
    Enter = 128,
    /// <summary>
    /// Escape key, for cancel editing operation
    /// </summary>
    Escape = 256,
    /// <summary>
    /// Default: Arrows|Ctrl_C|Ctrl_V|Ctrl_X|Delete|Tab|PageDownUp
    /// </summary>
    Default = Arrows | Ctrl_C | Ctrl_V | Ctrl_X | Delete | Tab | PageDownUp | Enter | Escape
  }

  /// <summary>
  /// Position type of the cell. Look at the .vsd diagram for details.
  /// </summary>
  [ComVisible(false)]
  public enum CellPositionType
  {
    /// <summary>
    /// Empty Cell
    /// </summary>
    Empty = 0,
    /// <summary>
    /// Fixed Top+Left Cell
    /// </summary>
    FixedTopLeft = 1,
    /// <summary>
    /// Fixed Top Cell
    /// </summary>
    FixedTop = 2,
    /// <summary>
    /// Fixed Left cell
    /// </summary>
    FixedLeft = 3,
    /// <summary>
    /// Scrollable Cell
    /// </summary>
    Scrollable = 4
  }

  /// <summary>
  /// The type of change to selection that caused the SelectionChangedEvent to be raised.
  /// </summary>
  [ComVisible(false)]
  public enum SelectionChangedEventType
  {
    /// <summary>
    /// Selection was cleared.
    /// </summary>
    Clear,
    /// <summary>
    /// Selection had items added.
    /// </summary>
    Add,
    /// <summary>
    /// Selection had items removed.
    /// </summary>
    Remove
  }

  /// <summary>
  /// DrawCellStatus
  /// </summary>
  [ComVisible(false)]
  public enum DrawCellStatus
  {
    Focus,
    Selected,
    Normal
  }

  /// <summary>
  /// FocusStyle
  /// </summary>
  [Flags]
  public enum FocusStyles
  {
    /// <summary>
    /// None specified
    /// </summary>
    None = 0,
    /// <summary>
    /// Remove the focus cell when the grid lost the focus
    /// </summary>
    RemoveFocusCellOnLeave = 1,
    /// <summary>
    /// Remove the selection when the grid lost the focus
    /// </summary>
    RemoveSelectionOnLeave = 2
  }

  /// <summary>
  /// AutoSizeMode
  /// </summary>
  [Flags]
  public enum AutoSizeModes
  {
    /// <summary>
    /// None specified
    /// </summary>
    None = 0,
    /// <summary>
    /// Enable the AutoSize
    /// </summary>
    EnableAutoSize = 1,
    /// <summary>
    /// Enable Stretch operation
    /// </summary>
    EnableStretch = 2
  }
}