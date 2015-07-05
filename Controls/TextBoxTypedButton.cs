#region MIT License
//
// Filename: TextBoxTypedButton.cs
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.ConversionModel.Validator;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// Control to simulate a ComboBox, because the one provided with the Framework doesn't support vertical sizing different from the size of the font.
  /// </summary>
  [ComVisible(false)]
  public class TextBoxTypedButton : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.Button btDown;
    private TextBoxTyped txtBox;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBoxTypedButton"/> class.
    /// </summary>
    public TextBoxTypedButton()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      btDown.BackColor = Color.FromKnownColor(KnownColor.Control);
      txtBox.LoadingValidator += new EventHandler(txtBox_LoadingValidator);
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

    #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.btDown = new System.Windows.Forms.Button();
      this.txtBox = new TextBoxTyped();
      this.SuspendLayout();
      //
      // btDown
      //
      this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.btDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btDown.Location = new System.Drawing.Point(136, 0);
      this.btDown.Name = "btDown";
      this.btDown.Size = new System.Drawing.Size(27, 23);
      this.btDown.TabIndex = 1;
      this.btDown.Text = "...";
      this.btDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.btDown.Click += new EventHandler(this.btDown_Click);
      //
      // txtBox
      //
      this.txtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBox.AutoSize = false;
      this.txtBox.BackColor = System.Drawing.Color.White;
      this.txtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtBox.EnableAutoValidation = false;
      this.txtBox.EnableEnterKeyValidate = false;
      this.txtBox.EnableEscapeKeyUndo = true;
      this.txtBox.EnableLastValidValue = true;
      this.txtBox.ErrorProvider = null;
      this.txtBox.ErrorProviderMessage = "Invalid value";
      this.txtBox.ForceFormatText = true;
      this.txtBox.HideSelection = false;
      this.txtBox.Location = new System.Drawing.Point(0, 0);
      this.txtBox.Name = "txtBox";
      this.txtBox.Size = new System.Drawing.Size(136, 20);
      this.txtBox.TabIndex = 0;
      this.txtBox.Text = string.Empty;
      this.txtBox.WordWrap = false;
      //
      // TextBoxTypedButton
      //
      this.Controls.Add(this.txtBox);
      this.Controls.Add(this.btDown);
      this.Name = "TextBoxTypedButton";
      this.Size = new System.Drawing.Size(160, 20);
      this.ResumeLayout(false);
    }
    #endregion

    /// <summary>
    /// Gets or sets the validator.
    /// </summary>
    /// <value>The validator.</value>
    /// <remarks>
    /// Required
    /// </remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IValidator Validator
    {
      get { return txtBox.Validator; }
      set { txtBox.Validator = value; }
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
    }

    /// <summary>
    /// Loading Validator
    /// </summary>
    public event EventHandler LoadingValidator;

    /// <summary>
    /// Gets or sets a value indicating whether to set the textbox readonly otherwise false.
    /// </summary>
    /// <value><c>true</c> if read only text box; otherwise, <c>false</c>.</value>
    public bool ReadOnlyTextBox
    {
      get { return txtBox.ReadOnly; }
      set { txtBox.ReadOnly = value; }
    }

    /// <summary>
    /// Show the dialog
    /// </summary>
    public virtual void ShowDialog()
    {
      OnDialogOpen(EventArgs.Empty);

      OnDialogClosed(EventArgs.Empty);
    }

    /// <summary>
    /// Gets or sets the current value of the editor.
    /// </summary>
    /// <value>The value.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object Value
    {
      get
      {
        return txtBox.Value;
      }
      set
      {
        txtBox.Value = value;
      }
    }

    private void btDown_Click(object sender, System.EventArgs e)
    {
      ShowDialog();
    }

    /// <summary>
    /// Select all the text of the textbox
    /// </summary>
    public void SelectAllTextBox()
    {
      txtBox.SelectAll();
    }

    /// <summary>
    /// Gets the button in the right of the editor
    /// </summary>
    /// <value>The button.</value>
    public Button Button
    {
      get { return btDown; }
    }

    /// <summary>
    /// Gets the text box.
    /// </summary>
    /// <value>The text box.</value>
    public TextBoxTyped TextBox
    {
      get { return txtBox; }
    }

    /// <summary>
    /// Fired when showing the drop down
    /// </summary>
    public event EventHandler DialogOpen;

    /// <summary>
    /// Fired when showing the drop down
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnDialogOpen(EventArgs e)
    {
      if (DialogOpen != null)
      {
        DialogOpen(this, e);
      }
    }

    /// <summary>
    /// Fired when closing the dropdown
    /// </summary>
    public event EventHandler DialogClosed;

    /// <summary>
    /// Fired when closing the dropdown
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnDialogClosed(EventArgs e)
    {
      if (DialogClosed != null)
      {
        DialogClosed(this, e);
      }
    }

    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether after the Validating event
    /// the Text is refreshed with the new value, forcing the correct formatting.
    /// </summary>
    /// <value><c>true</c> if set to force format text; otherwise, <c>false</c>.</value>
    public bool ForceFormatText
    {
      get { return txtBox.ForceFormatText; }
      set { txtBox.ForceFormatText = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the Escape key to undo any changes. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if enable escape key undo; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEscapeKeyUndo
    {
      get { return txtBox.EnableEscapeKeyUndo; }
      set { txtBox.EnableEscapeKeyUndo = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the Enter key to validate any changes. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if enable enter key validate; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEnterKeyValidate
    {
      get { return txtBox.EnableEnterKeyValidate; }
      set { txtBox.EnableEnterKeyValidate = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the validation of the textbox text when the Validating
    /// event is fired, to force always the control to be valid. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if enable auto validation; otherwise, <c>false</c>.
    /// </value>
    public bool EnableAutoValidation
    {
      get { return txtBox.EnableAutoValidation; }
      set { txtBox.EnableAutoValidation = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to allow the Value property to always return
    /// a valid value when the textbox.text is not valid, false to throw an error when textbox.text is not valid.
    /// </summary>
    /// <value>
    /// <c>true</c> if enable last valid value; otherwise, <c>false</c>.
    /// </value>
    public bool EnableLastValidValue
    {
      get { return txtBox.EnableLastValidValue; }
      set { txtBox.EnableLastValidValue = value; }
    }
    #endregion

    private void txtBox_LoadingValidator(object sender, EventArgs e)
    {
      OnLoadingValidator();
    }
  }
}