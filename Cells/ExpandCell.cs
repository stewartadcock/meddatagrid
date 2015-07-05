#region MIT License
//
// Filename: ExpandCell.cs
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

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  [ComVisible(false)]
  public abstract class ExpandCell : CellVirtual, IExpandCell
  {
    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCell"/> class.
    /// </summary>
    protected ExpandCell()
    {
      DataModel = new DataModels.DataModelBase(typeof(bool));
      VisualModel = VisualModels.ExpandCell.Default;
      BehaviorModels.Add(BehaviorModel.ExpandCellBehaviorModel.Default);
    }
    #endregion

    /// <summary>
    /// Gets the state value at the specified position
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns></returns>
    public virtual bool GetStateValue(Position position)
    {
      return (bool)GetValue(position);
    }

    /// <summary>
    /// Sets the state value.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_State">if set to <c>true</c> cell is expanded.</param>
    public virtual void SetStateValue(Position position, bool p_State)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, p_State);
      }
      else
      {
        this.SetValue(position, p_State);
      }
    }

    /// <summary>
    /// Gets a vertical range of cells contained in the parent cells.
    /// These are treeview-like collapsible cells.
    /// </summary>
    /// <returns></returns>
    public virtual IList<ICellVirtual> GetChildCells()
    {
      return null;
    }
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// Description résumée de ExpandCell.
  /// </summary>
  public class ExpandCell : Cell, IExpandCell
  {
    private List<ICellVirtual> childCells;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCell"/> class.
    /// </summary>
    /// <param name="p_expand">if set to <c>true</c> cell is expanded.</param>
    public ExpandCell(bool p_expand)
    {
      childCells = new List<ICellVirtual>();
      ExpandedCell = false;

      DataModel = new DataModels.DataModelBase(typeof(bool));

      VisualModel = VisualModels.ExpandCell.Default;
      BehaviorModels.Add(BehaviorModel.ExpandCellBehaviorModel.Default);
      Value = p_expand;
      IsLastExpandedCell = false;
    }

    /// <summary>
    /// Gets the state value at the specified position
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual bool GetStateValue(Position position)
    {
      return (bool)GetValue(position);
    }

    /// <summary>
    /// Sets the state value.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_State">if set to <c>true</c> cell is expanded.</param>
    public virtual void SetStateValue(Position position, bool p_State)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, p_State);
      }
      else
      {
        this.SetValue(position, p_State);
      }
    }

    /// <summary>
    /// Gets a vertical range of cells contained in the parent cells.
    /// These are treeview-like collapsible cells.
    /// </summary>
    /// <returns></returns>
    public virtual IList<ICellVirtual> GetChildCells()
    {
      return (IList<ICellVirtual>)childCells;
    }
  }
}