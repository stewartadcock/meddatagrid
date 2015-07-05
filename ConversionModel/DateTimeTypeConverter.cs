#region MIT License
//
// Filename: DateTimeTypeConverter.cs
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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.ConversionModel.Converter
{
  /// <summary>
  /// TypeConverter for DateTime.
  /// </summary>
  [ComVisible(false)]
  public class DateTimeTypeConverter : TypeConverter
  {
    #region Constructors
    public DateTimeTypeConverter()
    {
    }

    public DateTimeTypeConverter(string p_ToStringFormat)
    {
      format = p_ToStringFormat;
    }

    public DateTimeTypeConverter(string p_ToStringFormat, string[] p_ParseFormats)
    {
      parseFormats = p_ParseFormats;
      format = p_ToStringFormat;
    }

    public DateTimeTypeConverter(string p_ToStringFormat, string[] p_ParseFormats, System.Globalization.DateTimeStyles p_DateTimeStyles)
    {
      parseFormats = p_ParseFormats;
      format = p_ToStringFormat;
      dateTimeStyles = p_DateTimeStyles;
    }
    #endregion

    #region Member Variables
    private System.ComponentModel.TypeConverter baseTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(DateTime));

    /// <summary>
    /// Gets or sets the base type converter.
    /// </summary>
    /// <value>The base type converter.</value>
    public System.ComponentModel.TypeConverter BaseTypeConverter
    {
      get { return this.baseTypeConverter; }
      set { this.baseTypeConverter = value; }
    }

    private System.Globalization.DateTimeStyles dateTimeStyles = System.Globalization.DateTimeStyles.AllowInnerWhite | System.Globalization.DateTimeStyles.AllowLeadingWhite | System.Globalization.DateTimeStyles.AllowTrailingWhite | System.Globalization.DateTimeStyles.AllowWhiteSpaces;
    /// <summary>
    /// Gets or sets the DateTimeStyle for Parse operations. DefaultValue: AllowInnerWhite|AllowLeadingWhite|AllowTrailingWhite|AllowWhiteSpaces
    /// </summary>
    /// <value>The date time styles.</value>
    public System.Globalization.DateTimeStyles DateTimeStyles
    {
      get { return this.dateTimeStyles; }
      set { this.dateTimeStyles = value; }
    }

    private string format = "G";
    /// <summary>
    /// Gets or sets the Format of the Date. Example: G, g, d, D. Default value : G
    /// </summary>
    /// <value>The format.</value>
    public string Format
    {
      get { return this.format; }
      set { this.format = value; }
    }

    private string[] parseFormats = null;
    /// <summary>
    /// Gets or sets the Formats to check when parse the string. If null call with no format the parse method. Default value: null
    /// </summary>
    /// <value>The parse formats.</value>
    public string[] ParseFormats
    {
      get { return this.parseFormats; }
      set { this.parseFormats = value; }
    }
    #endregion

    #region TypeConverter Implementation
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
      Type sourceType)
    {
      return (sourceType == typeof(string)) || baseTypeConverter.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context,
      Type destinationType)
    {
      return destinationType == typeof(string) || baseTypeConverter.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter"></see> is associated with, using the specified context, given a set of property values for the object.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary"></see> of new property values.</param>
    /// <returns>
    /// An <see cref="T:System.Object"></see> representing the given <see cref="T:System.Collections.IDictionary"></see>, or <c>null</c> if the object can not be created.
    /// This method always returns <c>null</c>.
    /// </returns>
    public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
    {
      return baseTypeConverter.CreateInstance(context, propertyValues);
    }

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
    /// <returns>
    /// An <see cref="T:System.Object"></see> that represents the converted value.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">The conversion can not be performed. </exception>
    public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if (value != null && value.GetType() == typeof(string))
      {
        if (parseFormats != null)
        {
          return DateTime.ParseExact((string)value, parseFormats, GetCulture(culture).DateTimeFormat, dateTimeStyles);
        }
        else
        {
          return DateTime.Parse((string)value, GetCulture(culture).DateTimeFormat, dateTimeStyles);
        }
      }
      else
      {
        return baseTypeConverter.ConvertFrom(context, culture, value);
      }
    }

    /// <summary>
    /// Converts the given value object to the specified type, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
    /// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
    /// <returns>
    /// An <see cref="T:System.Object"></see> that represents the converted value.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">The conversion can not be performed. </exception>
    /// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
    public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == typeof(string) && value != null)
      {
        return ((DateTime)value).ToString(format, GetCulture(culture).DateTimeFormat);
      }
      else
      {
        return baseTypeConverter.ConvertTo(context, culture, value, destinationType);
      }
    }

    /// <summary>
    /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <returns>
    /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value; otherwise, <c>false</c>.
    /// </returns>
    public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
    {
      return baseTypeConverter.GetCreateInstanceSupported(context);
    }

    /// <summary>
    /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="value">An <see cref="T:System.Object"></see> that specifies the type of array for which to get properties.</param>
    /// <param name="attributes">An array of type <see cref="T:System.Attribute"></see> that is used as a filter.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> with the properties that are exposed for this data type, or null if there are no properties.
    /// </returns>
    public override System.ComponentModel.PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return baseTypeConverter.GetProperties(context, value, attributes);
    }

    /// <summary>
    /// Returns whether this object supports properties, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <returns>
    /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"></see> should be called to find the properties of this object; otherwise, <c>false</c>.
    /// </returns>
    public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context)
    {
      return baseTypeConverter.GetPropertiesSupported(context);
    }

    /// <summary>
    /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
    /// </returns>
    public override StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
    {
      return baseTypeConverter.GetStandardValues(context);
    }

    /// <summary>
    /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of possible values, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <returns>
    /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exhaustive list of possible values; false if other values are possible.
    /// </returns>
    public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
    {
      return baseTypeConverter.GetStandardValuesExclusive(context);
    }

    /// <summary>
    /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <returns>
    /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, <c>false</c>.
    /// </returns>
    public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
    {
      return baseTypeConverter.GetStandardValuesSupported(context);
    }

    /// <summary>
    /// Returns whether the given value object is valid for this type and for the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
    /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
    /// <returns>
    /// <c>true</c> if the specified value is valid for this object; otherwise, <c>false</c>.
    /// </returns>
    public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
    {
      if (value != null && value.GetType() == typeof(string))
      {
        // provo a convertirlo
        try
        {
          object val = ConvertFrom(context, CultureInfo.CurrentCulture, value);
          return true;
        }
        catch (FormatException)
        {
          return false;
        }
        catch (ArgumentException)
        {
          return false;
        }
      }
      else
      {
        return baseTypeConverter.IsValid(context, value);
      }
    }
    #endregion

    #region Member Utility Function
    /// <summary>
    /// Returns the specified culture, or the current culture if specified culture is null.
    /// </summary>
    /// <param name="requestedCulture">The requested culture.</param>
    /// <returns></returns>
    private static CultureInfo GetCulture(CultureInfo requestedCulture)
    {
      return requestedCulture ?? CultureInfo.CurrentCulture;
    }
    #endregion
  }
}