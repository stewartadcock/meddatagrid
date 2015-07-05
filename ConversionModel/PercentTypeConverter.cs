#region MIT License
//
// Filename: PercentTypeConverter.cs
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
  /// A TypeConverter that support string conversion from and to string with the percent symbol.
  /// Support Conversion for Float, Double and Decimal
  /// </summary>
  [ComVisible(false)]
  public class PercentTypeConverter : TypeConverter
  {
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="PercentTypeConverter"/> class.
    /// </summary>
    /// <param name="p_BaseType">The p_BaseType.</param>
    public PercentTypeConverter(Type p_BaseType)
    {
      BaseType = p_BaseType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PercentTypeConverter"/> class.
    /// </summary>
    /// <param name="p_BaseType">The p_BaseType.</param>
    /// <param name="p_Format">The p_Format.</param>
    public PercentTypeConverter(Type p_BaseType,
      string p_Format)
      : this(p_BaseType)
    {
      Format = p_Format;
    }
    #endregion

    #region Member Variables
    private System.ComponentModel.TypeConverter baseTypeConverter;
    private Type baseType;
    private string formatString = "P";
    private bool doConsiderAllStringAsPercent = true;
    private NumberStyles numberStyles = NumberStyles.Number;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the base type converter.
    /// </summary>
    /// <value>The base type converter.</value>
    public System.ComponentModel.TypeConverter BaseTypeConverter
    {
      get { return this.baseTypeConverter; }
      set { this.baseTypeConverter = value; }
    }

    /// <summary>
    /// Gets or sets the base type
    /// </summary>
    /// <value>The base type.</value>
    public Type BaseType
    {
      get
      {
        return this.baseType;
      }
      set
      {
        if (value != typeof(double) &&
          value != typeof(float) &&
          value != typeof(decimal))
        {
          throw new MEDDataGridException("Type not supported");
        }

        baseTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(value);

        baseType = value;
      }
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    /// <value>The format.</value>
    public string Format
    {
      get { return this.formatString; }
      set { this.formatString = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether when the user insert a string with no percent symbol the value is divided by 100, otherwise not.
    /// </summary>
    /// <value>
    /// <c>true</c> if consider all string as percent; otherwise, <c>false</c>.
    /// </value>
    public bool ConsiderAllStringAsPercent
    {
      get { return this.doConsiderAllStringAsPercent; }
      set { this.doConsiderAllStringAsPercent = value; }
    }

    /// <summary>
    /// Gets or sets the number styles.
    /// </summary>
    /// <value>The number styles.</value>
    public System.Globalization.NumberStyles NumberStyles
    {
      get { return this.numberStyles; }
      set { this.numberStyles = value; }
    }
    #endregion

    #region Implementation TypeConverter
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
      return (destinationType == typeof(string)) || baseTypeConverter.CanConvertTo(context, destinationType);
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
    /// <exception cref="T:System.NotSupportedException">Thrown if the conversion can not be performed.</exception>
    /// <exception cref="T:System.ArgumentException">Thrown if the tye is not supported.</exception>
    public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if (value != null && value.GetType() == typeof(string))
      {
        if (BaseType == typeof(double))
        {
          return StringToDouble((string)value, NumberStyles, GetCulture(culture).NumberFormat, ConsiderAllStringAsPercent);
        }
        else if (BaseType == typeof(decimal))
        {
          return StringToDecimal((string)value, NumberStyles, GetCulture(culture).NumberFormat, ConsiderAllStringAsPercent);
        }
        else if (BaseType == typeof(float))
        {
          return StringToFloat((string)value, NumberStyles, GetCulture(culture).NumberFormat, ConsiderAllStringAsPercent);
        }
        else
        {
          throw new ArgumentException("Unsupported type");
        }
      }
      else
      {
        return this.baseTypeConverter.ConvertFrom(context, culture, value);
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
        if (BaseType == typeof(double))
        {
          return DoubleToString((double)value, Format, GetCulture(culture).NumberFormat);
        }
        else if (BaseType == typeof(decimal))
        {
          return DecimalToString((decimal)value, Format, GetCulture(culture).NumberFormat);
        }
        else if (BaseType == typeof(float))
        {
          return FloatToString((float)value, Format, GetCulture(culture).NumberFormat);
        }
        else
        {
          return this.baseTypeConverter.ConvertTo(context, culture, value, destinationType);
        }
      }
      else
      {
        return this.baseTypeConverter.ConvertTo(context, culture, value, destinationType);
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
      return this.baseTypeConverter.GetCreateInstanceSupported(context);
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
      return this.baseTypeConverter.GetProperties(context, value, attributes);
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
      return this.baseTypeConverter.GetPropertiesSupported(context);
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
      return this.baseTypeConverter.GetStandardValues(context);
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
      return this.baseTypeConverter.GetStandardValuesExclusive(context);
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
      return this.baseTypeConverter.GetStandardValuesSupported(context);
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
        catch (NotSupportedException)
        {
          return false;
        }
      }
      else
      {
        return this.baseTypeConverter.IsValid(context, value);
      }
    }
    #endregion

    #region Member Utility Functino
    private static CultureInfo GetCulture(CultureInfo requestedCulture)
    {
      return requestedCulture ?? CultureInfo.CurrentCulture;
    }
    #endregion

    #region Conversion String Methods
    /// <summary>
    /// Determines whether the specified string is a valid percent value.
    /// </summary>
    /// <param name="p_strVal">The string value.</param>
    /// <param name="provider">The provider.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is a valid percent value; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsPercentString(string p_strVal, System.IFormatProvider provider)
    {
      if (p_strVal == null)
      {
        return false;
      }

      NumberFormatInfo l_Info;
      if (provider == null)
      {
        l_Info = CultureInfo.CurrentCulture.NumberFormat;
      }
      else
      {
        l_Info = (NumberFormatInfo)provider.GetFormat(typeof(NumberFormatInfo));
      }

      return p_strVal.IndexOf(l_Info.PercentSymbol) != -1;
    }

    private static double StringToDouble(string p_strVal,
                      NumberStyles style,
                      IFormatProvider provider,
                      bool p_ConsiderAllStringAsPercent)
    {
      bool l_IsPercentString = IsPercentString(p_strVal, provider);
      if (l_IsPercentString)
      {
        return double.Parse(p_strVal.Replace("%", string.Empty), style, provider) / 100.0;
      }
      else
      {
        if (p_ConsiderAllStringAsPercent)
        {
          return double.Parse(p_strVal, style, provider) / 100.0;
        }
        else
        {
          return double.Parse(p_strVal, style, provider);
        }
      }
    }

    private static float StringToFloat(string p_strVal,
      NumberStyles style,
      IFormatProvider provider,
      bool p_ConsiderAllStringAsPercent)
    {
      bool l_IsPercentString = IsPercentString(p_strVal, provider);
      if (l_IsPercentString)
      {
        return float.Parse(p_strVal.Replace("%", string.Empty), style, provider) / 100;
      }
      else
      {
        if (p_ConsiderAllStringAsPercent)
        {
          return float.Parse(p_strVal, style, provider) / 100;
        }
        else
        {
          return float.Parse(p_strVal, style, provider);
        }
      }
    }

    private static decimal StringToDecimal(string p_strVal,
      NumberStyles style,
      IFormatProvider provider,
      bool p_ConsiderAllStringAsPercent)
    {
      bool l_IsPercentString = IsPercentString(p_strVal, provider);
      if (l_IsPercentString)
      {
        return decimal.Parse(p_strVal.Replace("%", string.Empty), style, provider) / 100.0M;
      }
      else
      {
        if (p_ConsiderAllStringAsPercent)
        {
          return decimal.Parse(p_strVal, style, provider) / 100.0M;
        }
        else
        {
          return decimal.Parse(p_strVal, style, provider);
        }
      }
    }

    private static string DoubleToString(double p_Val, System.String format, System.IFormatProvider provider)
    {
      return p_Val.ToString(format, provider);
    }

    private static string FloatToString(float p_Val, System.String format, System.IFormatProvider provider)
    {
      return p_Val.ToString(format, provider);
    }

    private static string DecimalToString(decimal p_Val, System.String format, System.IFormatProvider provider)
    {
      return p_Val.ToString(format, provider);
    }
    #endregion
  }
}