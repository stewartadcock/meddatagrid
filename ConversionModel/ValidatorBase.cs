#region MIT License
//
// Filename: ValidatorBase.cs
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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.ConversionModel.Validator
{
  /// <summary>
  /// A string editor for basic validator features, with no conversion.
  /// </summary>
  [ComVisible(false)]
  public class ValidatorBase : IValidator
  {
    private object defaultValue;
    private System.Collections.ICollection standardValues;
    private bool isStandardValuesExclusive;
    private object minimumValue = null;
    private object maximumValue = null;
    private CultureInfo cultureInfo = null;

    private bool doAllowNull;
    private string nullString;
    private string nullDisplayString;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatorBase"/> class.
    /// </summary>
    /// <param name="type">Can not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown is null Type argument passed.</exception>
    public ValidatorBase(Type type)
    {
      if (type == null)
      {
        throw new ArgumentNullException("type", "Null Type argument passed.");
      }

      valueType = type;
      if (type.IsValueType)
      {
        defaultValue = Activator.CreateInstance(type);
        doAllowNull = false;
      }
      else
      {
        defaultValue = null;
        doAllowNull = true;
      }

      standardValues = null;
      isStandardValuesExclusive = false;

      maximumValue = null;
      minimumValue = null;
      nullString = string.Empty;
      nullDisplayString = string.Empty;
    }
    #endregion

    #region Null
    /// <summary>
    /// Gets or sets a value indicating whether to allow null object value or NullString string Value
    /// </summary>
    /// <value><c>true</c> if allow null; otherwise, <c>false</c>.</value>
    public bool AllowNull
    {
      get { return this.doAllowNull; }
      set { this.doAllowNull = value; }
    }

    /// <summary>
    /// Gets or sets the null string representation. A string is null when is null or when is equals to this string. Default is empty string.
    /// Used by ValueToString and StringToValue
    /// </summary>
    /// <value>The null string.</value>
    public string NullString
    {
      get { return this.nullString; }
      set { this.nullString = value; }
    }

    /// <summary>
    /// Gets or sets the null string representation. A string is null when is null or when is equals to this string. Default is empty string.
    /// Used by ValueToDisplayString
    /// </summary>
    /// <value>The null display string.</value>
    public string NullDisplayString
    {
      get { return this.nullDisplayString; }
      set { this.nullDisplayString = value; }
    }

    /// <summary>
    /// Returns true if the string is null or if is equals to the NullString
    /// </summary>
    /// <param name="p_str">The string.</param>
    /// <returns>
    /// <c>true</c> if the specified string is the null string; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsNullString(string p_str)
    {
      return p_str == null || p_str == nullString;
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Convert an object according to the current ValueType of the validator
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <returns></returns>
    /// <exception cref="InvalidConversionException">Thrown when unable to convert object to ValueType</exception>
    public object ObjectToValue(object p_Object)
    {
      ConvertingObjectEventArgs l_Converting = new ConvertingObjectEventArgs(p_Object, valueType);
      OnConvertingObjectToValue(l_Converting);
      if (l_Converting.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new InvalidConversionException("Can not convert object to " + ValueType.Name);
      }

      if (IsValidValue(l_Converting.Value))
      {
        return l_Converting.Value;
      }
      else
      {
        throw new InvalidConversionException("Can not convert object to " + ValueType.Name);
      }
    }

    /// <summary>
    /// Convert a value according to the current ValueType to an object with the Type specified. Throw an exception on error.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_ReturnObjectType">Type of the return object.</param>
    /// <returns></returns>
    public object ValueToObject(object value, Type p_ReturnObjectType)
    {
      ConvertingObjectEventArgs l_Converting = new ConvertingObjectEventArgs(value, p_ReturnObjectType);
      OnConvertingValueToObject(l_Converting);
      if (l_Converting.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new MEDDataGridException("Can not convert value to " + p_ReturnObjectType.Name);
      }

      if (l_Converting.Value == null)
      {
        return null;
      }
      else if (l_Converting.DestinationType.IsAssignableFrom(l_Converting.Value.GetType()))
      {
        return l_Converting.Value;
      }
      else
      {
        throw new MEDDataGridException("Can not convert value to " + p_ReturnObjectType.Name);
      }
    }

    /// <summary>
    /// Converts a value object to a string representation
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public string ValueToString(object value)
    {
      object tmp = ValueToObject(value, typeof(string));
      if (tmp == null)
      {
        return null;
      }

      return (string)tmp;
    }

    /// <summary>
    /// Converts a string to an object according to the type of the string editor
    /// </summary>
    /// <param name="p_str">The string.</param>
    /// <returns>Value</returns>
    /// <exception cref="InvalidConversionException">Thrown when unable to convert object</exception>
    public object StringToValue(string p_str)
    {
      return ObjectToValue(p_str);
    }

    private bool doAllowStringConversion = true;

    /// <summary>
    /// Gets or sets a value indicating whether the string conversion is allowed.
    /// </summary>
    /// <value><c>true</c> if allow string conversion; otherwise, <c>false</c>.</value>
    public bool AllowStringConversion
    {
      get { return this.doAllowStringConversion; }
      set { this.doAllowStringConversion = value; }
    }

    /// <summary>
    /// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
    /// </summary>
    /// <returns>
    /// <c>true</c> if string conversion is supported; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsStringConversionSupported()
    {
      return AllowStringConversion && typeof(string) == this.valueType;
    }
    #endregion

    #region DisplayString
    /// <summary>
    /// Converts a value valid for this validator valuetype to a string representation. The string can not be used for conversion.
    /// If the validator support string conversion this method simply call ValueToString otherwise call Value.ToString()
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public virtual string ValueToDisplayString(object value)
    {
      ConvertingObjectEventArgs convertingArgs = new ConvertingObjectEventArgs(value, typeof(string));
      OnConvertingValueToDisplayString(convertingArgs);
      if (convertingArgs.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new MEDDataGridException("Can not convert value to display string");
      }

      if (convertingArgs.Value == null)
      {
        return NullDisplayString;
      }
      else if (convertingArgs.Value is string)
      {
        return (string)convertingArgs.Value;
      }
      else
      {
        throw new MEDDataGridException("Can not convert value to display string");
      }
    }
    #endregion

    #region Events
    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    protected ConvertingObjectEventHandler convertingObjectToValueHandler;
    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    protected ConvertingObjectEventHandler convertingValueToObjectHandler;
    /// <summary>
    /// Fired when converting a value to a display string. Called from method ValueToDisplayString
    /// </summary>
    protected ConvertingObjectEventHandler convertingValueToDisplayStringHandler;

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    public event ConvertingObjectEventHandler ConvertingObjectToValue
    {
      add { this.convertingObjectToValueHandler += value; }
      remove { this.convertingObjectToValueHandler -= value; }
    }

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    public event ConvertingObjectEventHandler ConvertingValueToObject
    {
      add { this.convertingValueToObjectHandler += value; }
      remove { this.convertingValueToObjectHandler -= value; }
    }

    /// <summary>
    /// Fired when converting a value to a display string. Called from method ValueToDisplayString
    /// </summary>
    public event ConvertingObjectEventHandler ConvertingValueToDisplayString
    {
      add { this.convertingValueToDisplayStringHandler += value; }
      remove { this.convertingValueToDisplayStringHandler -= value; }
    }

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    /// <param name="e">The <see cref="ConversionModel.ConvertingObjectEventArgs"/> instance containing the event data.</param>
    /// <exception cref="InvalidConversionException">Thrown when unable to convert object</exception>
    protected virtual void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
    {
      if (convertingObjectToValueHandler != null)
      {
        convertingObjectToValueHandler(this, e);
      }
      if (e.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new InvalidConversionException("Invalid conversion");
      }
      else if (e.ConvertingStatus == ConvertingStatus.Completed)
      {
        return;
      }

      if (e.Value == null)
      {
      }
      else if (e.Value is string) //è importante fare prima il caso stringa per gestire correttamente il null
      {
        string tmp = (string)e.Value;
        if (IsNullString(tmp))
        {
          e.Value = null;
        }
        else if (e.DestinationType != typeof(string) && IsStringConversionSupported() == false)
        {
          throw new InvalidConversionException("String conversion not supported for this type of Validator.");
        }
      }
      else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
      {
      }
    }

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ConversionModel.ConvertingObjectEventArgs"/> instance containing the event data.</param>
    protected virtual void OnConvertingValueToObject(ConvertingObjectEventArgs e)
    {
      if (convertingValueToObjectHandler != null)
      {
        convertingValueToObjectHandler(this, e);
      }
      if (e.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new MEDDataGridException("Invalid conversion");
      }
      else if (e.ConvertingStatus == ConvertingStatus.Completed)
      {
        return;
      }

      if (e.Value == null)
      {
      }
      else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
      {
      }
      else if (e.DestinationType == typeof(string))
      {
        if (IsStringConversionSupported() == false)
        {
          throw new MEDDataGridException("String conversion not supported for this type of Validator.");
        }
        e.Value = e.Value.ToString();
      }
    }

    /// <summary>
    /// Fired when converting a value to a display string. Called from method ValueToDisplayString
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ConversionModel.ConvertingObjectEventArgs"/> instance containing the event data.</param>
    protected virtual void OnConvertingValueToDisplayString(ConvertingObjectEventArgs e)
    {
      if (convertingValueToDisplayStringHandler != null)
      {
        convertingValueToDisplayStringHandler(this, e);
      }
      if (e.ConvertingStatus == ConvertingStatus.Error)
      {
        throw new MEDDataGridException("Invalid conversion");
      }
      else if (e.ConvertingStatus == ConvertingStatus.Completed)
      {
        return;
      }

      if (e.Value == null)
      {
        e.Value = NullDisplayString;
      }
      else if (IsStringConversionSupported())
      {
        e.Value = ValueToString(e.Value);
      }
      else
      {
        e.Value = e.Value.ToString();
      }
    }
    #endregion

    #region Validating
    /// <summary>
    /// Returns true if the value is valid for this type of editor without any conversion.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// <c>true</c> if the value is valid for this type of editor without any conversion; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidValue(object value)
    {
      try
      {
        if (IsInStandardValues(value))
        {
          return true;
        }

        if (isStandardValuesExclusive)
        {
          return false;
        }

        if (value == null)
        {
          return AllowNull;
        }

        if (maximumValue != null)
        {
          IComparable l_Max = (IComparable)maximumValue;
          if (l_Max.CompareTo(value) < 0)
          {
            return false;
          }
        }

        if (minimumValue != null)
        {
          IComparable l_Min = (IComparable)minimumValue;
          if (l_Min.CompareTo(value) > 0)
          {
            return false;
          }
        }

        return valueType.IsAssignableFrom(value.GetType());
      }
      catch (ArgumentException)
      {
        return false;
      }
    }

    /// <summary>
    /// Returns true if the object is valid for this type of validator, using conversion functions.
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidObject(object p_Object)
    {
      object dummy;
      return IsValidObject(p_Object, out dummy);
    }

    /// <summary>
    /// Returns true if the object is valid for this type of validator, using conversion functions. Returns as parameter the value converted.
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <param name="valueConverted">The converted value.</param>
    /// <returns>
    /// <c>true</c> if the specified object is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidObject(object p_Object, out object valueConverted)
    {
      valueConverted = null;
      try
      {
        valueConverted = ObjectToValue(p_Object);
        return IsValidValue(valueConverted);
      }
      catch (InvalidConversionException)
      {
        return false;
      }
    }

    /// <summary>
    /// Returns true if the string is valid for this type of editor, using
    /// string conversion function.
    /// </summary>
    /// <param name="p_strValue">The string value.</param>
    /// <returns>
    /// <c>true</c> if the specified string value is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidString(string p_strValue)
    {
      object dummy;
      return IsValidString(p_strValue, out dummy);
    }

    /// <summary>
    /// Returns true if the string is valid for this type of editor, using
    /// string conversion function. Returns as out parameter the object converted.
    /// </summary>
    /// <param name="p_strValue">The string value.</param>
    /// <param name="valueConverted">The value converted.</param>
    /// <returns>
    /// <c>true</c> if the specified string value is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValidString(string p_strValue, out object valueConverted)
    {
      return IsValidObject(p_strValue, out valueConverted);
    }
    #endregion

    #region Maximum/Minimum
    /// <summary>
    /// Gets or sets the minimum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
    /// </summary>
    /// <value>The minimum value.</value>
    public object MinimumValue
    {
      get { return this.minimumValue; }
      set { this.minimumValue = value; }
    }

    /// <summary>
    /// Gets or sets the maximum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
    /// </summary>
    /// <value>The maximum value.</value>
    public object MaximumValue
    {
      get { return this.maximumValue; }
      set { this.maximumValue = value; }
    }
    #endregion

    #region Type
    private Type valueType;
    /// <summary>
    /// Gets Type allowed for the current editor. Can not be null.
    /// </summary>
    /// <value>The type of the value.</value>
    public Type ValueType
    {
      get { return this.valueType; }
    }
    #endregion

    #region Default Value
    /// <summary>
    /// Gets or sets default value for this editor, usually is the default value for the specified type.
    /// </summary>
    /// <value>The default value.</value>
    public object DefaultValue
    {
      get { return this.defaultValue; }
      set { this.defaultValue = value; }
    }
    #endregion

    #region Standard Value Exclusive
    /// <summary>
    /// Gets or sets a list of values that this editor can support. If StandardValuesExclusive is true then the editor can only support one of these values.
    /// </summary>
    /// <value>The standard values.</value>
    public System.Collections.ICollection StandardValues
    {
      get { return this.standardValues; }
      set { this.standardValues = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editor can only support
    /// the list specified in StandardValues
    /// </summary>
    /// <value>
    /// <c>true</c> if standard values are exclusive; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// If StandardValuesExclusive is true then the editor can only support
    /// the list specified in StandardValues.
    /// </remarks>
    public bool StandardValuesExclusive
    {
      get { return this.isStandardValuesExclusive; }
      set { this.isStandardValuesExclusive = value; }
    }

    /// <summary>
    /// Returns true if the value specified is presents in the list StandardValues.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// <c>true</c> if specified value is in standard values; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsInStandardValues(object value)
    {
      if (this.standardValues == null)
      {
        return false;
      }

      foreach (object l_ListVal in this.standardValues)
      {
        if ((l_ListVal == null && value == null) ||
          (l_ListVal != null && l_ListVal.Equals(value)))
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Returns the standard values at the specified index. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
    /// </summary>
    /// <param name="p_Index">The index</param>
    /// <returns></returns>
    public virtual object StandardValueAtIndex(int p_Index)
    {
      if (this.standardValues == null)
      {
        throw new MEDDataGridException("StandardValues is null");
      }

      System.Collections.IList l_List = this.standardValues as System.Collections.IList;
      if (l_List != null)
      {
        return l_List[p_Index];
      }
      else
      {
        int l_CurrentIndex = 0;
        foreach (object o in this.standardValues)
        {
          if (l_CurrentIndex == p_Index)
          {
            return o;
          }

          l_CurrentIndex++;
        }

        throw new MEDDataGridException("Invalid Index");
      }
    }

    /// <summary>
    /// Returns the index of the specified standard value. -1 if not found. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
    /// </summary>
    /// <param name="standardValue">The standard value.</param>
    /// <returns></returns>
    public virtual int StandardValuesIndexOf(object standardValue)
    {
      if (this.standardValues == null)
      {
        throw new MEDDataGridException("StandardValues is null");
      }

      System.Collections.IList l_List = this.standardValues as System.Collections.IList;
      if (l_List != null)
      {
        return l_List.IndexOf(standardValue);
      }
      else
      {
        int l_CurrentIndex = 0;
        foreach (object o in standardValues)
        {
          if (o == null && standardValue == null)
          {
            return l_CurrentIndex;
          }
          else if (o != null)
          {
            if (o.Equals(standardValue))
            {
              return l_CurrentIndex;
            }
          }

          l_CurrentIndex++;
        }

        return -1;
      }
    }
    #endregion

    #region Culture
    /// <summary>
    /// Gets or sets culture for conversion. If null the default user culture is used. Default is null.
    /// </summary>
    /// <value>The culture info.</value>
    /// <remarks>
    /// If null, the default user culture is used. Default is null.
    /// </remarks>
    public System.Globalization.CultureInfo CultureInfo
    {
      get { return this.cultureInfo; }
      set { this.cultureInfo = value; }
    }
    #endregion
  }
}