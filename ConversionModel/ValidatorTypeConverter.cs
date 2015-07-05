#region MIT License
//
// Filename: ValidatorTypeConverter.cs
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
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.ConversionModel.Validator
{
  /// <summary>
  /// A string editor that use a TypeConverter for conversion.
  /// </summary>
  [ComVisible(false)]
  public class ValidatorTypeConverter : ValidatorBase
  {
    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatorTypeConverter"/> class.
    /// If the Type doesn't implements a TypeConverter no conversion is performed.
    /// </summary>
    /// <param name="type">Can not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown is null Type argument passed.</exception>
    public ValidatorTypeConverter(Type type)
      : this(type, TypeDescriptor.GetConverter(type))
    {
      // Do nothing.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatorTypeConverter"/> class.
    /// </summary>
    /// <param name="type">Can not be null.</param>
    /// <param name="typeConverter">Can be null to don't allow any conversion.</param>
    public ValidatorTypeConverter(Type type, TypeConverter typeConverter)
      : base(type)
    {
      this.typeConverter = typeConverter;

      // Populate properties using TypeConverter
      if (typeConverter != null)
      {
        StandardValues = typeConverter.GetStandardValues();
        if (StandardValues != null && StandardValues.Count > 0)
        {
          StandardValuesExclusive = typeConverter.GetStandardValuesExclusive();
        }
        else
        {
          StandardValuesExclusive = false;
        }
      }
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
    /// </summary>
    /// <returns>
    /// <c>true</c> if string conversion is supported; otherwise, <c>false</c>.
    /// </returns>
    public override bool IsStringConversionSupported()
    {
      if (typeof(string).IsAssignableFrom(ValueType))
      {
        return AllowStringConversion;
      }

      if (typeConverter != null)
      {
        return this.AllowStringConversion && typeConverter.CanConvertFrom(typeof(string)) && typeConverter.CanConvertTo(typeof(string));
      }
      else
      {
        return this.AllowStringConversion;
      }
    }

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    /// <param name="e">The <see cref="ConversionModel.ConvertingObjectEventArgs"/> instance containing the event data.</param>
    /// <exception cref="InvalidConversionException">Thrown when unable to convert object</exception>
    protected override void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
    {
      if (convertingObjectToValueHandler != null)
      {
        convertingObjectToValueHandler(this, e);
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
      else if (e.Value is string) //è importante fare prima il caso stringa per gestire correttamente il null
      {
        string tmp = (string)e.Value;
        if (IsNullString(tmp))
        {
          e.Value = null;
        }
        else if (e.DestinationType.IsAssignableFrom(e.Value.GetType())) //se la stringa non è nulla e il tipo di destinazione è sempre una string allora non faccio nessuna conversione
        {
        }
        else if (IsStringConversionSupported())
        {
          e.Value = typeConverter.ConvertFromString(null, CultureInfo, tmp);
        }
        else
        {
          throw new MEDDataGridException("String conversion not supported for this type of Validator.");
        }
      }
      else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
      {
      }
      else if (typeConverter != null)
      {
        e.Value = typeConverter.ConvertFrom(null, CultureInfo, e.Value);
      }
    }

    /// <summary>
    /// Fired when converting an object to the value specified. Called from method ObjectToValue and IsValidObject
    /// </summary>
    /// <param name="e">The <see cref="Fr.Medit.MedDataGrid.ConversionModel.ConvertingObjectEventArgs"/> instance containing the event data.</param>
    protected override void OnConvertingValueToObject(ConvertingObjectEventArgs e)
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
      else if (e.DestinationType == typeof(string) && IsStringConversionSupported() == false)
      {
        // SAA TODO: Deficiency in conversion here.  Add Image->string conversion when image
        // is ligand depiction.
        // Here we just choose not to copy anything because we don't have a conversion.
        e.Value = string.Empty;
      }
      else if (typeConverter != null)
      {
        e.Value = typeConverter.ConvertTo(null, CultureInfo, e.Value, e.DestinationType);
      }
    }
    #endregion

    #region Type
    private System.ComponentModel.TypeConverter typeConverter;
    /// <summary>
    /// Gets or sets the TypeConverter used for this type editor, can not be null.
    /// </summary>
    /// <value>The type converter.</value>
    public TypeConverter TypeConverter
    {
      get { return this.typeConverter; }
      set { this.typeConverter = value; }
    }
    #endregion
  }
}