#region MIT License
//
// Filename: ButtonCell.cs
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
  /// A cell that contains a button
  /// </summary>
  [ComVisible(false)]
  public abstract class ButtonCell : CellVirtual
  {
    public event PositionEventHandler Click;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonCell"/> class.
    /// </summary>
    protected ButtonCell()
    {
      BehaviorModels.Add(BehaviorModel.ButtonBehaviorModel.Default);
      VisualModel = VisualModels.Header.Default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonCell"/> class.
    /// </summary>
    /// <param name="p_Click">The click.</param>
    protected ButtonCell(PositionEventHandler p_Click)
      : this()
    {
      if (p_Click != null)
      {
        Click += p_Click;
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

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A cell that represent a button
  /// </summary>
  [ComVisible(false)]
  public class ButtonCell : Cell
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ButtonCell(object value)
      : this(value, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_Click">The click.</param>
    public ButtonCell(object value, PositionEventHandler p_Click)
      : base(value)
    {
      BehaviorModels.Add(BehaviorModel.ButtonBehaviorModel.Default);

      VisualModel = VisualModels.Header.Default;
      if (p_Click != null)
      {
        Click += p_Click;
      }
    }

    public event PositionEventHandler Click;

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