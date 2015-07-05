#region MIT License
//
// Filename: LinkCell.cs
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

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  /// <summary>
  /// A cell that contains a HTML style link. Use the click event to execute the link.
  /// </summary>
  [ComVisible(false)]
  public abstract class LinkCell : CellVirtual, ICellCursor
  {
    public event PositionEventHandler Click;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="LinkCell"/> class.
    /// </summary>
    protected LinkCell()
    {
      VisualModel = VisualModels.Common.LinkStyle;
      BehaviorModels.Add(BehaviorModel.CursorBehaviorModel.Default);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkCell"/> class.
    /// </summary>
    /// <param name="p_ExecuteLink">Event to execute when the user Click on this cell</param>
    protected LinkCell(PositionEventHandler p_ExecuteLink)
      : this()
    {
      if (p_ExecuteLink != null)
      {
        Click += p_ExecuteLink;
      }
    }
    #endregion

    /// <summary>
    /// Raises the <see cref="E:Click"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);

      if (Click != null)
      {
        Click(this, e);
      }
    }

    /// <summary>
    /// Get the cursor of the specified cell.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public System.Windows.Forms.Cursor GetCursor(Position position)
    {
      return System.Windows.Forms.Cursors.Hand;
    }
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A cell that contains a HTML style link. Use the click event to execute the link.
  /// </summary>
  [ComVisible(false)]
  public class LinkCell : Cell
  {
    public event PositionEventHandler Click;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="LinkCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public LinkCell(object value)
      : base(value)
    {
      VisualModel = VisualModels.Common.LinkStyle;
      BehaviorModels.Add(BehaviorModel.CursorBehaviorModel.Default);

      Cursor = System.Windows.Forms.Cursors.Hand;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="executeLink">Event to execute when the user Click on this cell</param>
    public LinkCell(object value, PositionEventHandler executeLink)
      : this(value)
    {
      if (executeLink != null)
      {
        Click += executeLink;
      }
    }
    #endregion

    /// <summary>
    /// Raises the <see cref="E:Click"/> event.
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionEventArgs"/> instance containing the event data.</param>
    public override void OnClick(PositionEventArgs e)
    {
      base.OnClick(e);

      if (Click != null)
      {
        Click(this, e);
      }
    }
  }
}