#region MIT License
//
// Filename: ComboBoxTyped.cs
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
  /// Control to simulate a ComboBox.
  /// </summary>
  /// <remarks>
  /// Required because the one provided with the .Net Framework doesn't support vertical sizing differently from the size of the font.
  /// </remarks>
  [ComVisible(false)]
  public class ComboBoxTyped : UserControl
  {
    private System.Windows.Forms.Button btDown;
    private TextBoxTyped txtBox;
    private int selectedItem = -1;
    private bool doEditTxtBoxByCode = false;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBoxTyped"/> class.
    /// </summary>
    public ComboBoxTyped()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      btDown.BackColor = Color.FromKnownColor(KnownColor.Control);
      txtBox.LoadingValidator += new EventHandler(txtBox_LoadingValidator);
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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

    #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ComboBoxTyped));
      this.btDown = new System.Windows.Forms.Button();
      this.txtBox = new TextBoxTyped();
      this.SuspendLayout();
      //
      // btDown
      //
      this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
      this.btDown.Location = new System.Drawing.Point(142, 0);
      this.btDown.Name = "btDown";
      this.btDown.Size = new System.Drawing.Size(18, 20);
      this.btDown.TabIndex = 1;
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
      this.txtBox.Size = new System.Drawing.Size(142, 20);
      this.txtBox.TabIndex = 0;
      this.txtBox.Text = string.Empty;
      this.txtBox.WordWrap = false;
      this.txtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
      this.txtBox.TextChanged += new EventHandler(this.txtBox_TextChanged);
      //
      // ComboBoxTyped
      //
      this.Controls.Add(this.txtBox);
      this.Controls.Add(this.btDown);
      this.Name = "ComboBoxTyped";
      this.Size = new System.Drawing.Size(160, 20);
      this.ResumeLayout(false);
    }
    #endregion

    /// <summary>
    /// Gets or sets the validator.
    /// </summary>
    /// <value>The validator.</value>
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
    /// Occurs when loading validator.
    /// </summary>
    public event EventHandler LoadingValidator;

    /// <summary>
    /// Gets or sets a value indicating whether the textbox readonly.
    /// </summary>
    /// <value><c>true</c> if the textbox is readonly; otherwise, <c>false</c>.</value>
    public bool ReadOnlyTextBox
    {
      get { return this.txtBox.ReadOnly; }
      set { this.txtBox.ReadOnly = value; }
    }

    /// <summary>
    /// Gets or sets the selected Index of the Items array. -1 if no value is selected or if the value is not in the Items list.
    /// </summary>
    /// <value>The index of the selected.</value>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
      get
      {
        return this.selectedItem;
      }
      set
      {
        this.selectedItem = value;
        OnSelectedIndexChanged();
      }
    }

    /// <summary>
    /// Populate and show the listbox
    /// </summary>
    public virtual void ShowListBox()
    {
      OnDropDownOpen(EventArgs.Empty);

      using (ListBox l_ListBox = new ListBox())
      {
        l_ListBox.BorderStyle = BorderStyle.None;
        l_ListBox.HorizontalScrollbar = true;
        l_ListBox.IntegralHeight = false;
        if (Validator.StandardValues != null)
        {
          foreach (object o in Validator.StandardValues)
          {
            l_ListBox.Items.Add(Validator.ValueToDisplayString(o));
          }
        }

        // Calculate the height of the ListBox : l_ListBox.Height
        int defaultHeight = l_ListBox.ItemHeight * 3;
        int maximumHeight = (int)(Screen.PrimaryScreen.Bounds.Height / 2.1);
        l_ListBox.Height = Math.Max(defaultHeight, Math.Min(maximumHeight, l_ListBox.PreferredHeight)) + 2;

        using (DropDownCustom l_DropDown = new DropDownCustom(this, l_ListBox))
        {
          if (selectedItem >= 0 && selectedItem < l_ListBox.Items.Count)
          {
            l_ListBox.SelectedIndex = selectedItem;
          }
          l_ListBox.SelectedIndexChanged += new EventHandler(ListBox_SelectedChange);
          l_ListBox.Click += new EventHandler(ListBox_Click);
          l_DropDown.ShowDropDown();
          l_ListBox.Click -= new EventHandler(ListBox_Click);
          l_ListBox.SelectedIndexChanged -= new EventHandler(ListBox_SelectedChange);

          txtBox.Focus();

          OnDropDownClosed(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Fired when the SelectedIndex property change
    /// </summary>
    protected virtual void OnSelectedIndexChanged()
    {
      if (Validator.StandardValues != null &&
        selectedItem != -1 &&
        selectedItem < Validator.StandardValues.Count)
      {
        try
        {
          doEditTxtBoxByCode = true; // per disabilitare l'evento txtBoxChange

          txtBox.Value = Validator.StandardValueAtIndex(selectedItem);

          SelectAllTextBox();
        }
        finally
        {
          doEditTxtBoxByCode = false;
        }
      }
    }

    /// <summary>
    /// Returns the string valud at the specified index using the editor.
    /// If index is not valid return Validator.NullDisplayString.
    /// </summary>
    /// <param name="p_Index">The index</param>
    /// <returns></returns>
    protected virtual string GetStringValueAtIndex(int p_Index)
    {
      if (Validator.StandardValues != null &&
        p_Index >= 0 &&
        p_Index < Validator.StandardValues.Count)
      {
        if (Validator.IsStringConversionSupported())
        {
          return Validator.ValueToString(Validator.StandardValueAtIndex(p_Index));
        }
        else
        {
          return Validator.ValueToDisplayString(Validator.StandardValueAtIndex(p_Index));
        }
      }
      else
      {
        return Validator.NullDisplayString;
      }
    }

    /// <summary>
    /// Handles the SelectedChange event of the ListBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ListBox_SelectedChange(object sender, EventArgs e)
    {
      SelectedIndex = ((ListBox)sender).SelectedIndex;
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

        // provo a cercare il valore nell'elenco di valori attualmente nella lista in modo da poterlo selezionare
        if (Validator.StandardValues != null)
        {
          selectedItem = Validator.StandardValuesIndexOf(value);
        }
        else
        {
          selectedItem = -1;
        }
      }
    }

    /// <summary>
    /// Handles the Click event of the btDown control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btDown_Click(object sender, System.EventArgs e)
    {
      ShowListBox();
    }

    /// <summary>
    /// Select all the text of the textbox
    /// </summary>
    public void SelectAllTextBox()
    {
      txtBox.SelectAll();
    }

    /// <summary>
    /// Handles the TextChanged event of the txtBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void txtBox_TextChanged(object sender, System.EventArgs e)
    {
      if (doEditTxtBoxByCode == false)
      {
        selectedItem = -1;
      }
    }

    /// <summary>
    /// Handles the KeyDown event of the txtBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
    private void txtBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
      {
        int l_SelectedIndex = selectedItem;
        if (e.KeyCode == Keys.Down)
        {
          l_SelectedIndex++;
        }
        else if (e.KeyCode == Keys.Up)
        {
          l_SelectedIndex--;
        }

        // controllo che sia valido
        if (l_SelectedIndex >= 0 && Validator.StandardValues != null && l_SelectedIndex < Validator.StandardValues.Count)
        {
          SelectedIndex = l_SelectedIndex;
        }
      }
    }

    /// <summary>
    /// Gets the button in the right of the editor
    /// </summary>
    /// <value>The button.</value>
    public Button Button
    {
      get { return this.btDown; }
    }

    /// <summary>
    /// Gets the text box.
    /// </summary>
    /// <value>The text box.</value>
    public TextBoxTyped TextBox
    {
      get { return this.txtBox; }
    }

    /// <summary>
    /// Handles the Click event of the ListBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ListBox_Click(object sender, EventArgs e)
    {
      ListBox_SelectedChange(sender, EventArgs.Empty);
      txtBox.Focus();
    }

    /// <summary>
    /// Fired when showing the drop down
    /// </summary>
    public event EventHandler DropDownOpen;

    /// <summary>
    /// Fired when showing the drop down
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnDropDownOpen(EventArgs e)
    {
      if (DropDownOpen != null)
      {
        DropDownOpen(this, e);
      }
    }

    /// <summary>
    /// Fired when closing the dropdown
    /// </summary>
    public event EventHandler DropDownClosed;

    /// <summary>
    /// Fired when closing the dropdown
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnDropDownClosed(EventArgs e)
    {
      if (DropDownClosed != null)
      {
        DropDownClosed(this, e);
      }
    }

    /// <summary>
    /// Handles the LoadingValidator event of the txtBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void txtBox_LoadingValidator(object sender, EventArgs e)
    {
      OnLoadingValidator();
    }

    #region Properties
    /// <summary>
    /// Gets or sets a value indicating whether after the Validating event the Text is refreshed with the new value, forcing the correct formatting.
    /// </summary>
    /// <value><c>true</c> if set to force format text; otherwise, <c>false</c>.</value>
    public bool ForceFormatText
    {
      get { return this.txtBox.ForceFormatText; }
      set { this.txtBox.ForceFormatText = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the Escape key to undo any changes. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if escape key undo is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEscapeKeyUndo
    {
      get { return this.txtBox.EnableEscapeKeyUndo; }
      set { this.txtBox.EnableEscapeKeyUndo = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the Enter key to validate any changes. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if enter key validation is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableEnterKeyValidate
    {
      get { return this.txtBox.EnableEnterKeyValidate; }
      set { this.txtBox.EnableEnterKeyValidate = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the validation of the
    /// textbox text when the Validating event is fired, to force always the
    /// control to be valid. Default is true.
    /// </summary>
    /// <value>
    /// <c>true</c> if auto validation is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableAutoValidation
    {
      get { return txtBox.EnableAutoValidation; }
      set { txtBox.EnableAutoValidation = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to allow the Value property
    /// to always return a valid value when the textbox.text is not valid,
    /// false to throw an error when textbox.text is not valid.
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
  }
}