#region MIT License
//
// Filename: ValueMapping.cs
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

namespace Fr.Medit.MedDataGrid.ConversionModel.Validator
{
  /// <summary>
  /// The ValueMapping class can be used to easily map a value to a string value or a display string for conversion
  /// </summary>
  [ComVisible(false)]
  public class ValueMapping
  {
    private System.Collections.IList valueList;
    private System.Collections.IList objectList;
    private System.Collections.IList displayStringList;
    private bool doThrowErrorIfNotFound = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueMapping"/> class.
    /// </summary>
    public ValueMapping()
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueMapping"/> class.
    /// </summary>
    /// <param name="p_Validator">The validator.</param>
    /// <param name="valueList">A list of valid value. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
    /// <param name="p_ObjectList">A list of object that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
    /// <param name="p_DisplayStringList">A list of displayString. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
    public ValueMapping(ValidatorBase p_Validator, System.Collections.IList valueList, System.Collections.IList p_ObjectList, System.Collections.IList p_DisplayStringList)
    {
      ValueList = valueList;
      DisplayStringList = p_DisplayStringList;
      ObjectList = p_ObjectList;
      if (p_Validator != null)
      {
        BindValidator(p_Validator);
      }
    }

    /// <summary>
    /// Bind the specified validator
    /// </summary>
    /// <param name="p_Validator">The validator.</param>
    public void BindValidator(ValidatorBase p_Validator)
    {
      p_Validator.ConvertingValueToDisplayString += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
      p_Validator.ConvertingObjectToValue += new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
      p_Validator.ConvertingValueToObject += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
    }

    /// <summary>
    /// Unbind the specified validator
    /// </summary>
    /// <param name="p_Validator">The validator.</param>
    public void UnBindValidator(ValidatorBase p_Validator)
    {
      p_Validator.ConvertingValueToDisplayString -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
      p_Validator.ConvertingObjectToValue -= new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
      p_Validator.ConvertingValueToObject -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
    }

    /// <summary>
    /// Gets or sets the list of valid values. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList
    /// </summary>
    /// <value>The value list.</value>
    public System.Collections.IList ValueList
    {
      get { return this.valueList; }
      set { this.valueList = value; }
    }

    /// <summary>
    /// Gets or sets the list of objects that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList
    /// </summary>
    /// <value>The object list.</value>
    public System.Collections.IList ObjectList
    {
      get { return this.objectList; }
      set { this.objectList = value; }
    }

    /// <summary>
    /// Gets or sets the list of displayStrings. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList
    /// </summary>
    /// <value>The display string list.</value>
    public System.Collections.IList DisplayStringList
    {
      get { return this.displayStringList; }
      set { this.displayStringList = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an error when the value if not found in one of the dictionary
    /// </summary>
    /// <value>
    /// <c>true</c> if should throw error if not found; otherwise, <c>false</c>.
    /// </value>
    public bool ThrowErrorIfNotFound
    {
      get { return this.doThrowErrorIfNotFound; }
      set { this.doThrowErrorIfNotFound = value; }
    }

    private void p_Validator_ConvertingValueToDisplayString(object sender, ConvertingObjectEventArgs e)
    {
      if (displayStringList != null)
      {
        if (valueList == null)
        {
          throw new MEDDataGridException("ValueList can not be null");
        }

        int l_Index = valueList.IndexOf(e.Value);
        if (l_Index >= 0)
        {
          e.Value = displayStringList[l_Index];
          e.ConvertingStatus = ConvertingStatus.Completed;
        }
        else if (doThrowErrorIfNotFound)
        {
          e.ConvertingStatus = ConvertingStatus.Error;
        }
      }
    }

    private void p_Validator_ConvertingObjectToValue(object sender, ConvertingObjectEventArgs e)
    {
      if (objectList != null)
      {
        if (valueList == null)
        {
          throw new MEDDataGridException("ValueList can not be null");
        }

        int l_Index = objectList.IndexOf(e.Value);
        if (l_Index >= 0)
        {
          e.Value = valueList[l_Index];
          e.ConvertingStatus = ConvertingStatus.Completed;
        }
        else if (doThrowErrorIfNotFound)
        {
          e.ConvertingStatus = ConvertingStatus.Error;
        }
      }
    }

    private void p_Validator_ConvertingValueToObject(object sender, ConvertingObjectEventArgs e)
    {
      if (objectList != null)
      {
        if (valueList == null)
        {
          throw new MEDDataGridException("ValueList can not be null");
        }

        int l_Index = valueList.IndexOf(e.Value);
        if (l_Index >= 0)
        {
          e.Value = objectList[l_Index];
          e.ConvertingStatus = ConvertingStatus.Completed;
        }
        else if (doThrowErrorIfNotFound)
        {
          e.ConvertingStatus = ConvertingStatus.Error;
        }
      }
    }
  }
}