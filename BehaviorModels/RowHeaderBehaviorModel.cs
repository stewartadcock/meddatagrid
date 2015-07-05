#region MIT License
//
// Filename: RowHeaderBehaviorModel.cs
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

namespace Fr.Medit.MedDataGrid.BehaviorModel
{
  /// <summary>
  /// BehaviorModel for RowHeader cells.
  /// </summary>
  [ComVisible(false)]
  public class RowHeaderBehaviorModel : BehaviorModelGroup
  {
    /// <summary>
    /// Default Row Header
    /// </summary>
    public static readonly RowHeaderBehaviorModel Default = new RowHeaderBehaviorModel();

    private ResizeBehaviorModel resize;

    /// <summary>
    /// Initializes a new instance of the <see cref="RowHeaderBehaviorModel"/> class.
    /// </summary>
    public RowHeaderBehaviorModel()
      : this(ResizeBehaviorModel.ResizeHeight, ButtonBehaviorModel.Default, UnselectableBehaviorModel.Default)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RowHeaderBehaviorModel"/> class.
    /// </summary>
    /// <param name="p_BehaviorResize">The behavior resize.</param>
    /// <param name="p_BehaviorButton">The behavior button.</param>
    /// <param name="p_BehaviorUnselectable">The behavior unselectable.</param>
    public RowHeaderBehaviorModel(ResizeBehaviorModel p_BehaviorResize, ButtonBehaviorModel p_BehaviorButton, UnselectableBehaviorModel p_BehaviorUnselectable)
    {
      resize = p_BehaviorResize;
      SubModels.Add(resize);
      SubModels.Add(p_BehaviorButton);
      SubModels.Add(p_BehaviorUnselectable);
    }

    public override void OnFocusEntering(PositionCancelEventArgs e)
    {
      // check whether the user is in a resize region
      if (resize != null &&
        resize.IsHeightResizing == false &&
        resize.IsWidthResizing == false &&
        e.Grid.Selection.SelectionMode != GridSelectionMode.Column)
      {
        e.Grid.Rows[e.Position.Row].Focus();
        e.Grid.Rows[e.Position.Row].Select = false;
      }

      base.OnFocusEntering(e);
    }
  }
}