#region MIT License
//
// Filename: LinkedControlsList.cs
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

using System.Collections;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A dictionary with keys of type Control and values of type Position
  /// </summary>
  public class LinkedControlsList : DictionaryBase
  {
    private bool doUseCellBorder = true;

    /// <summary>
    /// Initializes a new empty instance of the ControlToPositionAssociation class
    /// </summary>
    public LinkedControlsList()
    {
      // empty
    }

    /// <summary>
    /// Gets or sets the Position associated with the given Control.
    /// </summary>
    /// <param name="key">The Control whose value to get or set.</param>
    /// <returns>The Position associated with the given Control.</returns>
    public virtual Position this[Control key]
    {
      get { return (Position)this.Dictionary[key]; }
      set { this.Dictionary[key] = value; }
    }

    /// <summary>
    /// Adds an element with the specified key and value to this ControlToPositionAssociation.
    /// </summary>
    /// <param name="key">
    /// The Control key of the element to add.
    /// </param>
    /// <param name="value">
    /// The Position value of the element to add.
    /// </param>
    public virtual void Add(Control key, Position value)
    {
      this.Dictionary.Add(key, value);
    }

    /// <summary>
    /// Determines whether this ControlToPositionAssociation contains a specific key.
    /// </summary>
    /// <param name="key">
    /// The Control key to locate in this ControlToPositionAssociation.
    /// </param>
    /// <returns>
    /// true if this ControlToPositionAssociation contains an element with the specified key;
    /// otherwise, false.
    /// </returns>
    public virtual bool Contains(Control key)
    {
      return this.Dictionary.Contains(key);
    }

    /// <summary>
    /// Determines whether this ControlToPositionAssociation contains a specific key.
    /// </summary>
    /// <param name="key">
    /// The Control key to locate in this ControlToPositionAssociation.
    /// </param>
    /// <returns>
    /// true if this ControlToPositionAssociation contains an element with the specified key;
    /// otherwise, false.
    /// </returns>
    public virtual bool ContainsKey(Control key)
    {
      return this.Dictionary.Contains(key);
    }

    /// <summary>
    /// Determines whether this ControlToPositionAssociation contains a specific value.
    /// </summary>
    /// <param name="value">
    /// The Position value to locate in this ControlToPositionAssociation.
    /// </param>
    /// <returns>
    /// true if this ControlToPositionAssociation contains an element with the specified value;
    /// otherwise, false.
    /// </returns>
    public virtual bool ContainsValue(Position value)
    {
      foreach (Position item in this.Dictionary.Values)
      {
        if (item == value)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Removes the element with the specified key from this ControlToPositionAssociation.
    /// </summary>
    /// <param name="key">
    /// The Control key of the element to remove.
    /// </param>
    public virtual void Remove(Control key)
    {
      this.Dictionary.Remove(key);
    }

    /// <summary>
    /// Gets a collection containing the keys in this ControlToPositionAssociation.
    /// </summary>
    /// <value>Keys</value>
    /// <returns>An <see cref="T:System.Collections.ICollection"></see> object containing the keys of the <see cref="T:System.Collections.IDictionary"></see> object.</returns>
    public virtual ICollection Keys
    {
      get { return this.Dictionary.Keys; }
    }

    /// <summary>
    /// Gets a collection containing the values in this ControlToPositionAssociation.
    /// </summary>
    /// <value>Values</value>
    /// <returns>An <see cref="T:System.Collections.ICollection"></see> object containing the values in the <see cref="T:System.Collections.IDictionary"></see> object.</returns>
    public virtual System.Collections.ICollection Values
    {
      get { return this.Dictionary.Values; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to insert the editor control inside the border of the cell, false to put the editor control over the entire cell. If you use true remember to set EnableCellDrawOnEdit == true.
    /// </summary>
    /// <value><c>true</c> if set to insert the editor control inside the cell border; otherwise, <c>false</c>.</value>
    public bool UseCellBorder
    {
      get { return this.doUseCellBorder; }
      set { this.doUseCellBorder = value; }
    }
  }
}