#region MIT License
//
// Filename: ConvertingObjectEventArgs.cs
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

namespace Fr.Medit.MedDataGrid.ConversionModel
{
  /// <summary>
  /// ConvertingObjectEventArgs
  /// </summary>
  [ComVisible(false)]
  public class ConvertingObjectEventArgs : EventArgs
  {
    private object objectValue;
    private Type destinationType;
    private ConvertingStatus convertingStatus = ConvertingStatus.Converting;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertingObjectEventArgs"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="p_DestinationType">The destination tye.</param>
    public ConvertingObjectEventArgs(object value, Type p_DestinationType)
    {
      objectValue = value;
      destinationType = p_DestinationType;
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public object Value
    {
      get { return this.objectValue; }
      set { this.objectValue = value; }
    }

    /// <summary>
    /// Gets the type of the destination.
    /// </summary>
    /// <value>The type of the destination.</value>
    public Type DestinationType
    {
      get { return this.destinationType; }
    }

    /// <summary>
    /// Gets or sets the converting status.
    /// </summary>
    /// <value>The converting status.</value>
    public ConvertingStatus ConvertingStatus
    {
      get { return convertingStatus; }
      set { convertingStatus = value; }
    }
  }

  [ComVisible(false)]
  public delegate void ConvertingObjectEventHandler(object sender, ConvertingObjectEventArgs e);

  /// <summary>
  /// ConvertingStatus
  /// </summary>
  [ComVisible(false)]
  public enum ConvertingStatus
  {
    Converting = 0,
    Error = 1,
    Completed = 2
  }
}