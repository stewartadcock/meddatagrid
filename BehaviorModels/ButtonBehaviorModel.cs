#region MIT License
//
// Filename: ButtonBehaviorModel.cs
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
  /// BehaviorModel for buttons.
  /// </summary>
  /// <remarks>
  /// This behaviour can be shared between multiple cells.
  /// </remarks>
  public class ButtonBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// The default behaviour of a button's behaviour
    /// </summary>
    public static readonly ButtonBehaviorModel Default = new ButtonBehaviorModel();

    /// <summary>
    /// Handles the mouse down event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public override void OnMouseDown(PositionMouseEventArgs e)
    {
      base.OnMouseDown(e);

      e.Grid.InvalidateCell(e.Position);
    }

    /// <summary>
    /// Handles the mouse up event in the current cell
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.PositionMouseEventArgs"/> instance containing the event data.</param>
    public override void OnMouseUp(PositionMouseEventArgs e)
    {
      base.OnMouseUp(e);

      e.Grid.InvalidateCell(e.Position);
    }
  }
}