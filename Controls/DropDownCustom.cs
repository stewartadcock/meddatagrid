#region MIT License
//
// Filename: DropDownCustom.cs
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
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// Custom Drop-Down control, intended for use in MedDataGrids.
  /// </summary>
  public class DropDownCustom : Form, IDisposable
  {
    private Point startLocation = new Point(0, 0);
    private Panel panelContainer;

    private DropDownBehaviours dropDownFlags = DropDownBehaviours.CloseOnEnter | DropDownBehaviours.CloseOnEscape;
    private bool isDeactivated = false;
    private Control parentControl = null;
    private Control innerControl = null;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="DropDownCustom"/> class.
    /// </summary>
    public DropDownCustom()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DropDownCustom"/> class.
    /// </summary>
    /// <param name="p_ParentControl">The parent control.</param>
    /// <param name="p_InnerControl">The inner control.</param>
    public DropDownCustom(Control p_ParentControl, Control p_InnerControl)
      : this()
    {
      innerControl = p_InnerControl;
      parentControl = p_ParentControl;
    }

    private void InnerControl_Resize(object sender, EventArgs e)
    {
      if (innerControl != null)
      {
        Size = innerControl.Size;
      }
    }

    #region IDisposable methods
    /// <summary>
    /// Disposes of the resources (other than memory) used by the
    /// <see cref="T:System.Windows.Forms.Form"></see>.
    /// </summary>
    /// <param name="disposing">true to release both managed and
    /// unmanaged resources; false to release only unmanaged resources.</param>
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

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropDownCustom));
      this.panelContainer = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      //
      // panelContainer
      //
      this.panelContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelContainer.Location = new System.Drawing.Point(0, 0);
      this.panelContainer.Name = "panelContainer";
      this.panelContainer.Size = new System.Drawing.Size(84, 48);
      this.panelContainer.TabIndex = 0;
      //
      // DropDownCustom
      //
      ////this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(84, 48);
      this.ControlBox = false;
      this.Controls.Add(this.panelContainer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DropDownCustom";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "ctlDropDownCustom";
      this.Deactivate += new EventHandler(this.ctlDropDownCustom_Deactivate);
      this.Activated += new EventHandler(this.ctlDropDownCustom_Activated);
      this.ResumeLayout(false);
    }
    #endregion

    /// <summary>
    /// Gets or sets the parent control.
    /// </summary>
    /// <value>The parent control.</value>
    public Control ParentControl
    {
      get { return parentControl; }
      set { parentControl = value; }
    }

    /// <summary>
    /// Gets or sets the inner control.
    /// </summary>
    /// <value>The inner control.</value>
    public Control InnerControl
    {
      get { return innerControl; }
      set { innerControl = value; }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"></see> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      if (innerControl != null && parentControl != null)
      {
        innerControl.Width = Math.Max(parentControl.Width, innerControl.Width);
        panelContainer.Controls.Add(innerControl);
        innerControl.Location = new Point(0, 0);
        Size = innerControl.Size;
      }
    }

    private void CalcLocation()
    {
      Rectangle parentRectangle = new Rectangle(0, 0, 0, 0);
      if (innerControl != null && parentControl != null)
      {
        parentRectangle = parentControl.RectangleToScreen(parentControl.ClientRectangle);
      }

      // Determine which screen we're on and how big it is.
      Screen displayedOnScreen = Screen.FromPoint(new Point(parentRectangle.X, parentRectangle.Bottom));
      int minScreenXPos = displayedOnScreen.Bounds.X;
      int maxScreenXPos = displayedOnScreen.Bounds.X + displayedOnScreen.Bounds.Width;
      int maxScreenYPos = displayedOnScreen.Bounds.Y + displayedOnScreen.Bounds.Height;

      int dropdownWidth = Width;
      int dropdownHeight = Height;

      // Will we bump into the right edge of the window when we first display the control?
      if ((parentRectangle.X + dropdownWidth) <= maxScreenXPos)
      {
        if (parentRectangle.X < minScreenXPos)
        {
          startLocation.X = minScreenXPos;
        }
        else
        {
          startLocation.X = parentRectangle.X;
        }
      }
      else
      {
        // Make sure we aren't overhanging the left side of the screen.
        if (Screen.FromPoint(new Point(parentRectangle.X + parentRectangle.Width, 0)) == displayedOnScreen)
        {
          startLocation.X = parentRectangle.Right - dropdownWidth;
        }
        else
        {
          startLocation.X = maxScreenXPos - dropdownWidth;
        }
      }

      // And now check the bottom of the screen.
      if ((parentRectangle.Bottom + dropdownHeight) <= maxScreenYPos)
      {
        startLocation.Y = parentRectangle.Bottom;
      }
      else
      {
        startLocation.Y = parentRectangle.Y - dropdownHeight;
      }

      this.Location = startLocation;
    }

    /// <summary>
    /// Handles the Activated event of the ctlDropDownCustom control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ctlDropDownCustom_Activated(object sender, System.EventArgs e)
    {
      if (innerControl != null && parentControl != null)
      {
        CalcLocation();
      }
    }

    /// <summary>
    /// Processes a command key.
    /// </summary>
    /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"></see>, passed by reference, that represents the Win32 message to process.</param>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
    /// <returns>
    /// <c>true</c> if the keystroke was processed and consumed by the control; otherwise, <c>false</c> to allow further processing.
    /// </returns>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    protected override bool ProcessCmdKey(
      ref Message msg,
      Keys keyData
      )
    {
      if ((dropDownFlags & DropDownBehaviours.CloseOnEscape) == DropDownBehaviours.CloseOnEscape && keyData == Keys.Escape)
      {
        DialogResult = DialogResult.Cancel;
        Hide();
      }

      if ((dropDownFlags & DropDownBehaviours.CloseOnEnter) == DropDownBehaviours.CloseOnEnter && keyData == Keys.Enter)
      {
        DialogResult = DialogResult.OK;
        Hide();
      }

      return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// Gets or sets the drop down flags.
    /// </summary>
    /// <value>The drop down flags.</value>
    public DropDownBehaviours DropDownBehaviours
    {
      get { return this.dropDownFlags; }
      set { this.dropDownFlags = value; }
    }

    private void ctlDropDownCustom_Deactivate(object sender, System.EventArgs e)
    {
      this.isDeactivated = true;
      Hide();
    }

    /// <summary>
    /// Show the drop down.
    /// </summary>
    public void ShowDropDown()
    {
      innerControl.Resize += new EventHandler(InnerControl_Resize);

      CalcLocation();

      Show();

      Size = innerControl.Size;

      // wait until form deactivated
      while (isDeactivated == false)
      {
        Application.DoEvents();
      }

      innerControl.Resize -= new EventHandler(InnerControl_Resize);
    }
  }

  /// <summary>
  /// Behaviours for custom Drop Down control.
  /// </summary>
  [Flags]
  public enum DropDownBehaviours
  {
    /// <summary>
    /// Null flag
    /// </summary>
    None = 0,
    /// <summary>
    /// Close the DropDown when the user press the escape key, return DialogResult.Cancel
    /// </summary>
    CloseOnEscape = 1,
    /// <summary>
    /// Close the DropDown when the user press the enter key, return DialogResult.OK
    /// </summary>
    CloseOnEnter = 2
  }
}