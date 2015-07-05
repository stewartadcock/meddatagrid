#region MIT License
//
// Filename: TextBoxTypedNumeric.cs
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

using Fr.Medit.MedDataGrid.ConversionModel.Validator;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// Numeric typed TextBox.
  /// </summary>
  [ComVisible(false)]
  public class TextBoxTypedNumeric : TextBoxTyped
  {
    private NumericCharStyles numericCharStyle;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBoxTypedNumeric"/> class.
    /// </summary>
    public TextBoxTypedNumeric()
    {
      Validator = new ValidatorTypeConverter(typeof(double));
      Value = 0.0;
      TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      NumericCharStyle = NumericCharStyles.DecimalSeparator | NumericCharStyles.NegativeSymbol | NumericCharStyles.NegativeSymbol;
    }

    /// <summary>
    /// Gets or sets the numeric style of characters allowed.
    /// </summary>
    /// <value>The numeric char style.</value>
    public NumericCharStyles NumericCharStyle
    {
      get
      {
        return numericCharStyle;
      }
      set
      {
        numericCharStyle = value;
        RecalculateCharacters();
      }
    }

    /// <summary>
    /// Recalculate the valid characters
    /// </summary>
    protected virtual void RecalculateCharacters()
    {
      ValidCharacters = CreateNumericValidChars(Validator.CultureInfo, numericCharStyle);
    }

    /// <summary>
    /// Returns an array of valid numeric char
    /// </summary>
    /// <param name="p_Culture">If null the current culture is used</param>
    /// <param name="p_NumericCharStyle">The numeric char style.</param>
    /// <returns></returns>
    public static char[] CreateNumericValidChars(System.Globalization.CultureInfo p_Culture, NumericCharStyles p_NumericCharStyle)
    {
      if (p_Culture == null)
      {
        p_Culture = System.Globalization.CultureInfo.CurrentCulture;
      }

      string l_AllowedChars = "0123456789";

      if ((p_NumericCharStyle & NumericCharStyles.CurrencySymbol) == NumericCharStyles.CurrencySymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.CurrencySymbol;

        if ((p_NumericCharStyle & NumericCharStyles.DecimalSeparator) == NumericCharStyles.DecimalSeparator)
        {
          l_AllowedChars += p_Culture.NumberFormat.CurrencyDecimalSeparator;
        }
        if ((p_NumericCharStyle & NumericCharStyles.GroupSeparator) == NumericCharStyles.GroupSeparator)
        {
          l_AllowedChars += p_Culture.NumberFormat.CurrencyGroupSeparator;
        }
      }

      if ((p_NumericCharStyle & NumericCharStyles.PercentSymbol) == NumericCharStyles.PercentSymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.PercentSymbol;

        if ((p_NumericCharStyle & NumericCharStyles.DecimalSeparator) == NumericCharStyles.DecimalSeparator)
        {
          l_AllowedChars += p_Culture.NumberFormat.PercentDecimalSeparator;
        }
        if ((p_NumericCharStyle & NumericCharStyles.GroupSeparator) == NumericCharStyles.GroupSeparator)
        {
          l_AllowedChars += p_Culture.NumberFormat.PercentGroupSeparator;
        }
      }

      if ((p_NumericCharStyle & NumericCharStyles.DecimalSeparator) == NumericCharStyles.DecimalSeparator)
      {
        l_AllowedChars += p_Culture.NumberFormat.NumberDecimalSeparator;
      }
      if ((p_NumericCharStyle & NumericCharStyles.GroupSeparator) == NumericCharStyles.GroupSeparator)
      {
        l_AllowedChars += p_Culture.NumberFormat.NumberGroupSeparator;
      }

      if ((p_NumericCharStyle & NumericCharStyles.InfinitySymbol) == NumericCharStyles.InfinitySymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.NegativeInfinitySymbol;
        l_AllowedChars += p_Culture.NumberFormat.PositiveInfinitySymbol;
      }

      if ((p_NumericCharStyle & NumericCharStyles.NaNSymbol) == NumericCharStyles.NaNSymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.NaNSymbol;
      }

      if ((p_NumericCharStyle & NumericCharStyles.NegativeSymbol) == NumericCharStyles.NegativeSymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.NegativeSign;
      }

      if ((p_NumericCharStyle & NumericCharStyles.PerMilleSymbol) == NumericCharStyles.PerMilleSymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.PerMilleSymbol;
      }

      if ((p_NumericCharStyle & NumericCharStyles.PositiveSymbol) == NumericCharStyles.PositiveSymbol)
      {
        l_AllowedChars += p_Culture.NumberFormat.PositiveSign;
      }

      if (l_AllowedChars.Length > 0)
      {
        return l_AllowedChars.ToCharArray();
      }
      else
      {
        return null;
      }
    }
  }

  [Flags]
  public enum NumericCharStyles
  {
    None = 0,
    CurrencySymbol = 1,
    PercentSymbol = 2,
    PerMilleSymbol = 4,
    DecimalSeparator = 8,
    GroupSeparator = 16,
    NegativeSymbol = 32,
    PositiveSymbol = 64,
    NaNSymbol = 128,
    InfinitySymbol = 256
  }
}