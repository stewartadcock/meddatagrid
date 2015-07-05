#region MIT License
//
// Filename: SortStatus.cs
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

namespace Fr.Medit.MedDataGrid
{
  public class SortStatus
  {
    #region Class members
    private GridSortMode mode;
    private bool enableSort;
    private System.Collections.IComparer comparer;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="SortStatus"/> class.
    /// </summary>
    /// <param name="p_Mode">Status of current sort.</param>
    /// <param name="p_EnableSort">True to enable sort otherwise false</param>
    public SortStatus(GridSortMode p_Mode, bool p_EnableSort)
    {
      mode = p_Mode;
      enableSort = p_EnableSort;
      comparer = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortStatus"/> class.
    /// </summary>
    /// <param name="p_Mode">Status of current sort.</param>
    /// <param name="p_EnableSort">True to enable sort otherwise false</param>
    /// <param name="p_Comparer">Comparer used to sort the column. The comparer will take 2 Cell.
    /// If null the default ValueCellComparer is used.</param>
    public SortStatus(GridSortMode p_Mode, bool p_EnableSort, System.Collections.IComparer p_Comparer)
      : this(p_Mode, p_EnableSort)
    {
      comparer = p_Comparer;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the mode.
    /// </summary>
    /// <value>The mode.</value>
    public GridSortMode Mode
    {
      get { return mode; }
      set { mode = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether sorting is enabled.
    /// </summary>
    /// <value><c>true</c> if sorting is enabled; otherwise, <c>false</c>.</value>
    public bool EnableSort
    {
      get { return enableSort; }
      set { enableSort = value; }
    }

    /// <summary>
    /// Gets or sets the comparer.
    /// </summary>
    /// <value>The comparer.</value>
    public System.Collections.IComparer Comparer
    {
      get { return comparer; }
      set { comparer = value; }
    }
    #endregion
  }
}