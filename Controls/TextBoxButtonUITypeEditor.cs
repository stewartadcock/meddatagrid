#region MIT License
//
// Filename: TextBoxButtonUITypeEditor.cs
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
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Fr.Fc.FcCore.Logging;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// A TextBoxTypedButton that uses the UITypeEditor associated with the type.
  /// </summary>
  [ComVisible(false)]
  public class TextBoxButtonUITypeEditor : TextBoxTypedButton, IServiceProvider, System.Windows.Forms.Design.IWindowsFormsEditorService
  {
    private System.ComponentModel.IContainer components = null;
    private UITypeEditor uiTypeEditor;
    private DropDownCustom dropDown = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBoxButtonUITypeEditor"/> class.
    /// </summary>
    public TextBoxButtonUITypeEditor()
    {
      // This call is required by the Windows Form Designer.
      InitializeComponent();
    }

    #region Dispose
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
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
    #endregion

    #region Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
    }
    #endregion

    /// <summary>
    /// Reload the properties from the validator
    /// </summary>
    public override void OnLoadingValidator()
    {
      object tmp = System.ComponentModel.TypeDescriptor.GetEditor(Validator.ValueType, typeof(UITypeEditor));
      if (tmp is UITypeEditor)
      {
        UITypeEditor = (UITypeEditor)tmp;
      }

      base.OnLoadingValidator();
    }

    /// <summary>
    /// Show the dialog
    /// </summary>
    public override void ShowDialog()
    {
      try
      {
        OnDialogOpen(EventArgs.Empty);
        if (uiTypeEditor != null)
        {
          UITypeEditorEditStyle l_Style = uiTypeEditor.GetEditStyle();
          if (l_Style == UITypeEditorEditStyle.DropDown ||
            l_Style == UITypeEditorEditStyle.Modal)
          {
            object l_EditObject;
            try
            {
              l_EditObject = Value;
            }
            catch
            {
              if (Validator != null)
              {
                l_EditObject = Validator.DefaultValue;
              }
              else
              {
                l_EditObject = null;
              }
            }

            object tmp = uiTypeEditor.EditValue(this, l_EditObject);
            Value = tmp;
          }
        }

        OnDialogClosed(EventArgs.Empty);
      }
      catch (Exception ex)
      {
        LoggerManager.Log(LogLevels.Error, "Unexpected exception: " + ex.ToString());
        MessageBox.Show(ex.Message, Application.ProductName + " build " + Application.ProductVersion,
         MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Gets or sets the UI type editor.
    /// </summary>
    /// <value>The UI type editor.</value>
    public UITypeEditor UITypeEditor
    {
      get { return this.uiTypeEditor; }
      set { this.uiTypeEditor = value; }
    }

    #region IServiceProvider
    /// <summary>
    /// Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>
    /// A service object of type serviceType. -or- null if there is no service object of type serviceType.
    /// </returns>
    Object IServiceProvider.GetService(System.Type serviceType)
    {
      if (serviceType == typeof(IWindowsFormsEditorService))
      {
        return this;
      }

      return null;
    }
    #endregion

    #region System.Windows.Forms.Design.IWindowsFormsEditorService
    /// <summary>
    /// Closes any previously opened drop down control area.
    /// </summary>
    public virtual void CloseDropDown()
    {
      if (this.dropDown != null)
      {
        this.dropDown.Hide();
      }
    }

    /// <summary>
    /// Displays the specified control in a drop down area below a value field of the property grid that provides this service.
    /// </summary>
    /// <param name="control">The drop down list <see cref="T:System.Windows.Forms.Control"></see> to open.</param>
    public virtual void DropDownControl(System.Windows.Forms.Control control)
    {
      this.dropDown = new DropDownCustom(this, control);
      this.dropDown.DropDownBehaviours = DropDownBehaviours.CloseOnEscape;
      this.dropDown.ShowDropDown();
      this.dropDown = null;
    }

    /// <summary>
    /// Show the specified <see cref="T:System.Windows.Forms.Form"></see>.
    /// </summary>
    /// <param name="dialog">The <see cref="T:System.Windows.Forms.Form"></see> to display.</param>
    /// <returns>
    /// A <see cref="T:System.Windows.Forms.DialogResult"></see> indicating the result code returned by the <see cref="T:System.Windows.Forms.Form"></see>.
    /// </returns>
    public virtual System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form dialog)
    {
      return dialog.ShowDialog(this);
    }
    #endregion
  }
}