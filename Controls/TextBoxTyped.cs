#region MIT License
//
// Filename: TextBoxTyped.cs
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
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.ConversionModel.Validator;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// A TextBox that allows to set the type of value to edit, then you can use the Value property to read and write the specific type.
  /// </summary>
  [ComVisible(false)]
  public class TextBoxTyped : System.Windows.Forms.TextBox
  {
    /// <summary>
    /// Loading Validator
    /// </summary>
    public event EventHandler LoadingValidator;

    /// <summary>
    /// Indica l'ultimo valore impostato valido. null se non è stato impostato nessun valore. Questo serve nel caso in cui ci sia un Validating che fallisce e viene richiesta la property Value. In questo caso si restituisce questo valore.
    /// </summary>
    private object lastValidValue = null;

    private string errorProviderMessage = "Invalid value";

    private ErrorProvider errorProvider;
    private IValidator validator;

    private char[] validCharacters;
    private char[] invalidCharacters;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBoxTyped"/> class.
    /// </summary>
    public TextBoxTyped()
    {
      // This call is required by the Windows Form Designer.
      InitializeComponent();

      Validator = new ValidatorTypeConverter(typeof(string));
    }

    #region Dispose
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
    }
    #endregion

    /// <summary>
    /// Gets or sets the value of the TextBox, returns an instance of the specified type.
    /// </summary>
    /// <value>The value.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object Value
    {
      get
      {
        //se il testo non è cambiato restituisco il valore precedente.
        if (isTextChanged == false)
        {
          return lastValidValue;
        }
        else if (validator.IsStringConversionSupported())
        {
          try
          {
            return validator.StringToValue(Text);
          }
          catch
          {
            if (EnableLastValidValue)
            {
              return lastValidValue;
            }
            else
            {
              throw;
            }
          }
        }
        else
        {
          throw new InvalidOperationException("TextBoxTyped not configured properly, string conversion is not supported but the text is changed");
        }
      }

      set
      {
        if (validator.IsValidValue(value) == false)
        {
          throw new InvalidOperationException("Invalid value for the current validator");
        }

        // markup the value so as to have in textbox the value in the correct format
        string tmp;
        if (validator.IsStringConversionSupported())
        {
          tmp = validator.ValueToString(value);
        }
        else
        {
          tmp = validator.ValueToDisplayString(value);
        }
        if (tmp != Text)
        {
          Text = tmp;
        }

        if (errorProvider != null)
        {
          errorProvider.SetError(this, null);
        }

        lastValidValue = value;
        isTextChanged = false;
      }
    }

    /// <summary>
    /// If true the text has changed. Returns false when set the Value property.
    /// </summary>
    private bool isTextChanged = false;

    /// <summary>
    /// Validate the content of the TextBox
    /// </summary>
    /// <returns>
    /// Returns True if the value is valid otherwise false
    /// </returns>
    public virtual bool ValidateTextBoxValue()
    {
      try
      {
        if (isTextChanged == false)
        {
          return validator.IsValidValue(Value);
        }

        object l_val;
        if (validator.IsValidString(Text, out l_val) == false)
        {
          if (errorProvider != null)
          {
            errorProvider.SetError(this, errorProviderMessage);
          }

          return false;
        }
        else
        {
          if (ForceFormatText)
          {
            // markup the value so as to have in textbox the value in the correct format
            Value = l_val;
          }

          if (errorProvider != null)
          {
            errorProvider.SetError(this, null);
          }

          return true;
        }
      }
      catch (Exception)
      {
        if (errorProvider != null)
        {
          errorProvider.SetError(this, errorProviderMessage);
        }

        return false;
      }
    }

    #region Properties
    /// <summary>
    /// Gets or sets the error provider used when a text is not valid.
    /// </summary>
    /// <value>The error provider.</value>
    public ErrorProvider ErrorProvider
    {
      get { return this.errorProvider; }
      set { this.errorProvider = value; }
    }

    /// <summary>
    /// Gets or sets the type converter used for conversion
    /// </summary>
    /// <value>The validator.</value>
    /// <exception cref="ArgumentNullException">Thrown if value is set to null</exception>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IValidator Validator
    {
      get
      {
        return this.validator;
      }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value", "Invalid Validator, can not be null");
        }
        this.validator = value;
        OnLoadingValidator();
      }
    }

    /// <summary>
    /// Reload the properties from the validator
    /// </summary>
    public virtual void OnLoadingValidator()
    {
      if (LoadingValidator != null)
      {
        LoadingValidator(this, EventArgs.Empty);
      }

      this.lastValidValue = validator.DefaultValue;
      ReadOnly = !validator.IsStringConversionSupported();
    }

    /// <summary>
    /// Gets or sets the message used with the ErrorProvider object
    /// </summary>
    /// <value>The error provider message.</value>
    public string ErrorProviderMessage
    {
      get { return this.errorProviderMessage; }
      set { this.errorProviderMessage = value; }
    }

    private bool doForceFormatText = true;
    /// <summary>
    /// Gets or sets a value indicating whether after the Validating event
    /// the Text is refreshed with the new value, forcing the correct formatting.
    /// </summary>
    /// <value>
    /// <c>true</c> if set to force format text; otherwise, <c>false</c>.
    /// </value>
    public bool ForceFormatText
    {
      get { return this.doForceFormatText; }
      set { this.doForceFormatText = value; }
    }

    private bool doEnableEscapeKeyUndo = true;
    /// <summary>
    /// Gets or sets a value indicating whether to enable the Escape key to
    /// undo any changes. Default is <c>true</c>.
    /// </summary>
    /// <value>
    /// <c>true</c> if escape key undo is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEscapeKeyUndo
    {
      get { return this.doEnableEscapeKeyUndo; }
      set { this.doEnableEscapeKeyUndo = value; }
    }

    private bool doEnableEnterKeyValidate = true;

    /// <summary>
    /// Gets or sets a value indicating whether to enable the Enter key to
    /// validate any changes. Default is <c>true</c>.
    /// </summary>
    /// <value>
    /// <c>true</c> if enter key validation is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEnterKeyValidate
    {
      get { return this.doEnableEnterKeyValidate; }
      set { this.doEnableEnterKeyValidate = value; }
    }

    private bool doEnableAutoValidation = true;
    /// <summary>
    /// Gets or sets a value indicating whether to enable the validation of the
    /// textbox text when the Validating event is fired, to force always the
    /// control to be valid. Default is <c>true</c>.
    /// </summary>
    /// <value>
    /// <c>true</c> if auto validation is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableAutoValidation
    {
      get { return this.doEnableAutoValidation; }
      set { this.doEnableAutoValidation = value; }
    }

    private bool doEnableLastValidValue = true;
    /// <summary>
    /// Gets or sets a value indicating whether to allow the Value property to
    /// always return a valid value when the textbox.Text is not valid
    /// (<c>true</c>), or to throw an error when textbox.Text is not valid (<c>false</c>).
    /// </summary>
    /// <value><c>true</c> if enable last valid value; otherwise, <c>false</c>.</value>
    public bool EnableLastValidValue
    {
      get { return this.doEnableLastValidValue; }
      set { this.doEnableLastValidValue = value; }
    }

    /// <summary>
    /// Gets or sets the valid characters.
    /// </summary>
    /// <remarks>
    /// This is a list of characters permitted for the textbox, used in the
    /// OnKeyPress event. If null no check is made. If not null only these
    /// charecters are allowed. First the method checks whether
    /// ValidCharacters is not null then checks for InvalidCharacters.
    /// </remarks>
    /// <value>The valid characters.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public char[] ValidCharacters
    {
      get { return this.validCharacters; }
      set { this.validCharacters = value; }
    }

    /// <summary>
    /// Gets or sets the invalid characters.
    /// </summary>
    /// <value>The invalid characters.</value>
    /// <remarks>
    /// This is a list of characters not permitted for the textbox, used in the
    /// OnKeyPress event. If null no check is made. If not null, these
    /// charecters are not allowed. First the method checks whether
    /// ValidCharacters is not null then checks for InvalidCharacters.
    /// </remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public char[] InvalidCharacters
    {
      get { return this.invalidCharacters; }
      set { this.invalidCharacters = value; }
    }
    #endregion

    #region Override Events
    /// <summary>
    /// Raises the System.Windows.Forms.Control.TextChanged event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnTextChanged(EventArgs e)
    {
      base.OnTextChanged(e);

      isTextChanged = true;
    }

    /// <summary>
    /// Raises the System.Windows.Forms.Control.Validating event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"></see> that contains the event data.</param>
    protected override void OnValidating(CancelEventArgs e)
    {
      base.OnValidating(e);

      if (EnableAutoValidation && ValidateTextBoxValue() == false)
      {
        e.Cancel = true;
      }
    }

    /// <summary>
    /// Raises the System.Windows.Forms.Control.KeyDown event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);

      if (e.KeyCode == Keys.Escape && EnableEscapeKeyUndo)
      {
        Value = lastValidValue;
      }
    }

    /// <summary>
    /// Raises the System.Windows.Forms.Control.KeyPress event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"></see> that contains the event data.</param>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      base.OnKeyPress(e);

      if (e.KeyChar == 13 && EnableEnterKeyValidate && Multiline == false)
      {
        ValidateTextBoxValue();
        e.Handled = true;
      }
      else if (char.IsControl(e.KeyChar) == false)
      { // is not a non printable character like backspace, ctrl+c, ...
        if (validCharacters != null && validCharacters.Length > 0)
        {
          for (int i = 0; i < validCharacters.Length; i++)
          {
            if (e.KeyChar == validCharacters[i])
            {
              return;
            }
          }

          e.Handled = true;
        }
        else if (invalidCharacters != null && invalidCharacters.Length > 0)
        {
          for (int i = 0; i < invalidCharacters.Length; i++)
          {
            if (e.KeyChar == invalidCharacters[i])
            {
              e.Handled = true;
              return;
            }
          }
        }
      }
    }
    #endregion

    /// <summary>
    /// Check in the specific string if all the characters are valid
    /// </summary>
    /// <param name="p_Input">The input.</param>
    /// <param name="p_ValidCharacters">The valid characters.</param>
    /// <param name="p_InvalidCharacters">The invalid characters.</param>
    /// <returns></returns>
    public static string ValidateCharactersString(string p_Input, char[] p_ValidCharacters, char[] p_InvalidCharacters)
    {
      string tmp;

      if (p_Input != null && p_ValidCharacters != null && p_ValidCharacters.Length > 0)
      {
        tmp = string.Empty;
        for (int i = 0; i < p_Input.Length; i++)
        {
          bool isFound = false;
          for (int j = 0; j < p_ValidCharacters.Length; j++)
          {
            if (p_ValidCharacters[j] == p_Input[i])
            {
              isFound = true;
              break;
            }
          }
          if (isFound)
          {
            tmp += p_Input[i];
          }
        }
      }
      else if (p_Input != null && p_InvalidCharacters != null && p_InvalidCharacters.Length > 0)
      {
        tmp = string.Empty;
        for (int i = 0; i < p_Input.Length; i++)
        {
          bool isFound = false;
          for (int j = 0; j < p_InvalidCharacters.Length; j++)
          {
            if (p_InvalidCharacters[j] == p_Input[i])
            {
              isFound = true;
              break;
            }
          }
          if (!isFound)
          {
            tmp += p_Input[i];
          }
        }
      }
      else
      {
        tmp = p_Input;
      }

      return tmp;
    }
  }
}