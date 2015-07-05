#region MIT License
//
// Filename: HeaderCell.cs
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
  /// A cell that represent a header of a table, with 3D effect.
  /// </summary>
  /// <remarks>
  /// This cell override <c>IsSelectable</c> to <c>false</c>.
  /// Default use <c>VisualModels.HeaderCell.Default</c>.
  /// </remarks>
  [ComVisible(false)]
  public abstract class HeaderCell : CellVirtual, IHeaderCell
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCell"/> class.
    /// </summary>
    protected HeaderCell()
      : this(VisualModels.Header.Default, BehaviorModel.HeaderBehaviorModel.Default)
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCell"/> class.
    /// </summary>
    /// <param name="p_VisualModel">The visual model.</param>
    /// <param name="p_HeaderBehavior">The header behavior.</param>
    protected HeaderCell(VisualModels.IVisualModel p_VisualModel, BehaviorModel.IBehaviorModel p_HeaderBehavior)
    {
      VisualModel = p_VisualModel;
      if (p_HeaderBehavior != null)
      {
        BehaviorModels.Add(p_HeaderBehavior);
      }
    }
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A cell that represent a header of a table, with 3D effect. This cell override IsSelectable to false. Default use VisualModels.VisualModelHeader.Style1
  /// </summary>
  [ComVisible(false)]
  public class HeaderCell : Cell
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCell"/> class.
    /// </summary>
    public HeaderCell()
      : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public HeaderCell(object value)
      : this(value, VisualModels.Header.Default, BehaviorModel.HeaderBehaviorModel.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCell"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_VisualModel">The visual model.</param>
    /// <param name="p_HeaderBehavior">The header behavior.</param>
    public HeaderCell(object value, VisualModels.IVisualModel p_VisualModel, BehaviorModel.IBehaviorModel p_HeaderBehavior)
      : base(value)
    {
      VisualModel = p_VisualModel;
      if (p_HeaderBehavior != null)
      {
        BehaviorModels.Add(p_HeaderBehavior);
      }
    }
  }
}