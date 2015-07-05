#region MIT License
//
// Filename: ControlsRepository.cs
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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid
{
  /// <summary>
  /// A dictionary with keys of type Guid and values of type Control.
  /// </summary>
  [ComVisible(false)]
  public class ControlsRepository : DictionaryBase
  {
    private Control parentControl;

    /// <summary>
    /// Initializes a new empty instance of the ControlsRepository class
    /// </summary>
    /// <param name="parent">The parent control.</param>
    public ControlsRepository(Control parent)
    {
      this.parentControl = parent;
    }

    /// <summary>
    /// Gets or sets the Control associated with the given Guid
    /// </summary>
    /// <param name="key">
    /// The Guid whose value to get or set.
    /// </param>
    /// <returns>The Control associated with the given Guid</returns>
    public virtual Control this[Guid key]
    {
      get
      {
        return (Control)this.Dictionary[key];
      }
    }

    /// <summary>
    /// Adds an element with the specified key and value to this ControlsRepository.
    /// </summary>
    /// <param name="key">
    /// The Guid key of the element to add.
    /// </param>
    /// <param name="value">
    /// The Control value of the element to add.
    /// </param>
    public virtual void Add(Guid key, Control value)
    {
      this.Dictionary.Add(key, value);
      this.parentControl.Controls.Add(value);
    }

    /// <summary>
    /// Determines whether this ControlsRepository contains a specific key.
    /// </summary>
    /// <param name="key">
    /// The Guid key to locate in this ControlsRepository.
    /// </param>
    /// <returns>
    /// true if this ControlsRepository contains an element with the specified key;
    /// otherwise, false.
    /// </returns>
    public virtual bool Contains(Guid key)
    {
      return this.Dictionary.Contains(key);
    }

    /// <summary>
    /// Determines whether this ControlsRepository contains a specific key.
    /// </summary>
    /// <param name="key">
    /// The Guid key to locate in this ControlsRepository.
    /// </param>
    /// <returns>
    /// true if this ControlsRepository contains an element with the specified key;
    /// otherwise, false.
    /// </returns>
    public virtual bool ContainsKey(Guid key)
    {
      return this.Dictionary.Contains(key);
    }

    /// <summary>
    /// Determines whether this ControlsRepository contains a specific value.
    /// </summary>
    /// <param name="value">
    /// The Control value to locate in this ControlsRepository.
    /// </param>
    /// <returns>
    /// true if this ControlsRepository contains an element with the specified value;
    /// otherwise, false.
    /// </returns>
    public virtual bool ContainsValue(Control value)
    {
      foreach (Control item in this.Dictionary.Values)
      {
        if (item == value)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Removes the element with the specified key from this ControlsRepository.
    /// </summary>
    /// <param name="key">
    /// The Guid key of the element to remove.
    /// </param>
    public virtual void Remove(Guid key)
    {
      if (ContainsKey(key))
      {
        this.parentControl.Controls.Remove(this[key]);
        this.Dictionary.Remove(key);
      }
    }

    /// <summary>
    /// Gets a collection containing the keys in this ControlsRepository.
    /// </summary>
    /// <value>Keys</value>
    /// <returns>An <see cref="T:System.Collections.ICollection"></see> object containing the keys of the <see cref="T:System.Collections.IDictionary"></see> object.</returns>
    public virtual System.Collections.ICollection Keys
    {
      get { return this.Dictionary.Keys; }
    }

    /// <summary>
    /// Gets a collection containing the values in this ControlsRepository.
    /// </summary>
    /// <value>Values</value>
    /// <returns>An <see cref="T:System.Collections.ICollection"></see> object containing the values in the <see cref="T:System.Collections.IDictionary"></see> object.</returns>
    public virtual System.Collections.ICollection Values
    {
      get { return this.Dictionary.Values; }
    }
  }
}