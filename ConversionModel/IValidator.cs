#region MIT License
//
// Filename: IValidator.cs
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.ConversionModel.Validator
{
  /// <summary>
  /// An interface to support string conversion and validation.
  /// Object = an object not yet converted for the current validator,
  /// Value = an object already converted and valid for the current validator,
  /// String = a string that can be used for conversion to and from Value,
  /// DisplayString = a string representation of the Value
  /// </summary>
  /// <remarks>
  /// SAA TODO: This should be a generic interface, rather than relying on object parameters throughout.
  /// </remarks>
  [ComVisible(false)]
  public interface IValidator
  {
    #region Null
    /// <summary>
    /// Gets or sets a value indicating whether to allow null object value or NullString string Value
    /// </summary>
    /// <value><c>true</c> if allow null; otherwise, <c>false</c>.</value>
    bool AllowNull
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the null string representation. A string is null when is null or when is equals to this string. Default is empty string.
    /// Used by ValueToString and StringToValue
    /// </summary>
    /// <value>The null string.</value>
    string NullString
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the null string representation. A string is null when is null or when is equals to this string. Default is empty string.
    /// Used by ValueToDisplayString
    /// </summary>
    /// <value>The null display string.</value>
    string NullDisplayString
    {
      get;
      set;
    }

    /// <summary>
    /// Returns true if the string is null or if is equals to the NullString
    /// </summary>
    /// <param name="p_str">The string.</param>
    /// <returns>
    /// <c>true</c> if the specified string is the null string; otherwise, <c>false</c>.
    /// </returns>
    bool IsNullString(string p_str);
    #endregion

    #region Conversion
    /// <summary>
    /// Convert an object according to the current ValueType of the validator
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <returns></returns>
    object ObjectToValue(object p_Object);

    /// <summary>
    /// Convert a value valid for the current validator ValueType to an object with the Type specified. Throw an exception on error.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_ReturnObjectType">Type of the return object.</param>
    /// <returns></returns>
    object ValueToObject(object value, Type p_ReturnObjectType);

    /// <summary>
    /// Convert a value valid for the current validator ValueType to a string that can be used for other conversions, for example StringToValue method.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    string ValueToString(object value);

    /// <summary>
    /// Converts a string to an object according to the type of the string editor
    /// </summary>
    /// <param name="p_str">The string.</param>
    /// <returns></returns>
    /// <exception cref="InvalidConversionException">Thrown when unable to convert object</exception>
    object StringToValue(string p_str);

    /// <summary>
    /// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
    /// </summary>
    /// <returns>
    /// <c>true</c> if is string conversion supported; otherwise, <c>false</c>.
    /// </returns>
    bool IsStringConversionSupported();

    /// <summary>
    /// Gets or sets a value indicating whether the string conversion is allowed.
    /// </summary>
    /// <value>
    /// <c>true</c> if allow string conversion; otherwise, <c>false</c>.
    /// </value>
    bool AllowStringConversion
    {
      get;
      set;
    }
    #endregion

    #region DisplayString
    /// <summary>
    /// Converts a value valid for this validator valuetype to a string representation. The string can not be used for conversion.
    /// If the validator support string conversion this method simply call ValueToString otherwise call Value.ToString()
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    string ValueToDisplayString(object value);
    #endregion

    #region Validating
    /// <summary>
    /// Returns true if the value is valid for this type of editor without any conversion.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// <c>true</c> if the value is valid for this type of editor without any conversion; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidValue(object value);

    /// <summary>
    /// Returns true if the object is valid for this type of validator, using conversion functions.
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidObject(object p_Object);

    /// <summary>
    /// Returns true if the object is valid for this type of validator, using conversion functions. Returns as parameter the value converted.
    /// </summary>
    /// <param name="p_Object">The object.</param>
    /// <param name="valueConverted">The converted value.</param>
    /// <returns>
    /// <c>true</c> if the specified object is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidObject(object p_Object, out object valueConverted);

    /// <summary>
    /// Returns true if the string is valid for this type of editor, using string conversion function.
    /// </summary>
    /// <param name="p_strValue">The string value.</param>
    /// <returns>
    /// <c>true</c> if the specified string value is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidString(string p_strValue);

    /// <summary>
    /// Returns true if the string is valid for this type of editor, using
    /// string conversion function. Also returns the object converted.
    /// </summary>
    /// <param name="p_strValue">The string value.</param>
    /// <param name="valueConverted">The value converted.</param>
    /// <returns>
    /// <c>true</c> if the specified string value is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidString(string p_strValue, out object valueConverted);
    #endregion

    #region Maximum/Minimum
    /// <summary>
    /// Gets or sets the Minimum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
    /// </summary>
    /// <value>The minimum value.</value>
    object MinimumValue
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the Maximum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
    /// </summary>
    /// <value>The maximum value.</value>
    object MaximumValue
    {
      get;
      set;
    }
    #endregion

    #region Type
    /// <summary>
    /// Gets the Type allowed for the current editor. Can not be null.
    /// </summary>
    /// <value>The type of the value.</value>
    Type ValueType
    {
      get;
    }
    #endregion

    #region Default Value
    /// <summary>
    /// Gets or sets the Default value for this editor, usually is the default value for the specified type.
    /// </summary>
    /// <value>The default value.</value>
    object DefaultValue
    {
      get;
      set;
    }
    #endregion

    #region Standard Value Exclusive
    /// <summary>
    /// Gets or sets a list of values that this editor can support. If StandardValuesExclusive is true then the editor can only support one of these values.
    /// </summary>
    /// <value>The standard values.</value>
    System.Collections.ICollection StandardValues
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editor can only support the list specified in StandardValues.
    /// </summary>
    /// <value>
    /// <c>true</c> if standard values are exclusive; otherwise, <c>false</c>.
    /// </value>
    bool StandardValuesExclusive
    {
      get;
      set;
    }

    /// <summary>
    /// Returns true if the value specified is presents in the list StandardValues.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// <c>true</c> if specified value is in standard values; otherwise, <c>false</c>.
    /// </returns>
    bool IsInStandardValues(object value);

    /// <summary>
    /// Returns the standard values at the specified index.
    /// If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
    /// </summary>
    /// <param name="p_Index">The index</param>
    /// <returns></returns>
    object StandardValueAtIndex(int p_Index);

    /// <summary>
    /// Returns the index of the specified standard value. -1 if not found. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
    /// </summary>
    /// <param name="p_StandardValue">The standard value.</param>
    /// <returns></returns>
    int StandardValuesIndexOf(object p_StandardValue);
    #endregion

    #region Culture
    /// <summary>
    /// Gets or sets the Culture for conversion.
    /// </summary>
    /// <remarks>
    /// If null, the default user culture is used. Default is null.
    /// </remarks>
    /// <value>The culture info.</value>
    System.Globalization.CultureInfo CultureInfo
    {
      get;
      set;
    }
    #endregion
  }
}