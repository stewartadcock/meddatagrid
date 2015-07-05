#region MIT License
//
// Filename: CellSizeDialog.cs
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
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls
{
  /// <summary>
  /// Dialog for resizing cells.
  /// </summary>
  internal sealed class CellSizeDialog : Form
  {
    /// <summary>
    /// CellSizeMode
    /// </summary>
    internal enum CellSizeMode
    {
      /// <summary>
      /// Row sizing
      /// </summary>
      Row,
      /// <summary>
      /// Column sizing
      /// </summary>
      Column
    }

    #region Class variables
    private Label lblWidthHeight;
    private Label lblColRow;
    private ComboBox cbColRow;
    private Button btClose;
    private NumericUpDown txtWidthHeight;

    private GridVirtual ownerGrid = null;
    private int columnId = 0;
    private int rowId = 0;
    private CellSizeMode cellSizeMode = CellSizeMode.Column;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="CellSizeDialog"/> class.
    /// </summary>
    public CellSizeDialog()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
    }
    #endregion

    #region Dispose
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
        if (ownerGrid != null)
        {
          ownerGrid.Dispose();
        }
        if (lblWidthHeight != null)
        {
          lblWidthHeight.Dispose();
        }
        if (lblColRow != null)
        {
          lblColRow.Dispose();
        }
        if (cbColRow != null)
        {
          cbColRow.Dispose();
        }
        if (btClose != null)
        {
          btClose.Dispose();
        }
        if (txtWidthHeight != null)
        {
          txtWidthHeight.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the row index.
    /// </summary>
    /// <value>The row index.</value>
    public int Row
    {
      get
      {
        return rowId;
      }
      set
      {
        rowId = value;
        RefreshSetting();
      }
    }

    /// <summary>
    /// Gets or sets the column index.
    /// </summary>
    /// <value>The column index.</value>
    public int Column
    {
      get
      {
        return columnId;
      }
      set
      {
        columnId = value;
        RefreshSetting();
      }
    }

    /// <summary>
    /// Gets or sets the parent grid.
    /// </summary>
    /// <value>The grid.</value>
    public GridVirtual Grid
    {
      get
      {
        return ownerGrid;
      }
      set
      {
        ownerGrid = value;
        RefreshSetting();
      }
    }
    #endregion

    #region public methods
    /// <summary>
    /// Loads the setting.
    /// </summary>
    /// <param name="grid">The parent grid.</param>
    /// <param name="column">The column.</param>
    /// <param name="row">The row.</param>
    /// <param name="mode">The cell size mode.</param>
    public void LoadSetting(GridVirtual grid, int column, int row, CellSizeMode mode)
    {
      this.ownerGrid = grid;
      this.columnId = column;
      this.rowId = row;
      this.cellSizeMode = mode;

      RefreshSetting();
    }

    /// <summary>
    /// Refreshes the setting.
    /// </summary>
    public void RefreshSetting()
    {
      if (ownerGrid != null)
      {
        if (cellSizeMode == CellSizeMode.Column)
        {
          lblColRow.Text = "Column:";
          lblWidthHeight.Text = "Width:";
          cbColRow.Items.Clear();
          for (int c = 0; c < ownerGrid.ColumnsCount; c++)
          {
            cbColRow.Items.Add("Col " + (c + 1).ToString());
          }
          cbColRow.SelectedIndex = columnId;
        }
        else if (cellSizeMode == CellSizeMode.Row)
        {
          lblColRow.Text = "Row:";
          lblWidthHeight.Text = "Height:";
          cbColRow.Items.Clear();
          for (int r = 0; r < ownerGrid.RowsCount; r++)
          {
            cbColRow.Items.Add("Row " + (r + 1).ToString());
          }
          cbColRow.SelectedIndex = rowId;
        }
      }
    }
    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.txtWidthHeight = new NumericUpDown();
      this.lblWidthHeight = new System.Windows.Forms.Label();
      this.lblColRow = new System.Windows.Forms.Label();
      this.cbColRow = new System.Windows.Forms.ComboBox();
      this.btClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.txtWidthHeight)).BeginInit();
      this.SuspendLayout();
      //
      // txtWidthHeight
      //
      this.txtWidthHeight.Increment = new System.Decimal(new int[] {
                                       2,
                                       0,
                                       0,
                                       0});
      this.txtWidthHeight.Location = new System.Drawing.Point(80, 40);
      this.txtWidthHeight.Maximum = new System.Decimal(new int[] {
                                       10000,
                                       0,
                                       0,
                                       0});
      this.txtWidthHeight.Name = "txtWidthHeight";
      this.txtWidthHeight.Size = new System.Drawing.Size(104, 20);
      this.txtWidthHeight.TabIndex = 0;
      this.txtWidthHeight.ValueChanged += new EventHandler(this.txtWidthHeight_ValueChanged);
      //
      // lblWidthHeight
      //
      this.lblWidthHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWidthHeight.Location = new System.Drawing.Point(8, 40);
      this.lblWidthHeight.Name = "lblWidthHeight";
      this.lblWidthHeight.Size = new System.Drawing.Size(72, 16);
      this.lblWidthHeight.TabIndex = 1;
      this.lblWidthHeight.Text = "label1";
      //
      // lblColRow
      //
      this.lblColRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColRow.Location = new System.Drawing.Point(8, 16);
      this.lblColRow.Name = "lblColRow";
      this.lblColRow.Size = new System.Drawing.Size(64, 16);
      this.lblColRow.TabIndex = 2;
      this.lblColRow.Text = "label1";
      //
      // cbColRow
      //
      this.cbColRow.Location = new System.Drawing.Point(80, 8);
      this.cbColRow.Name = "cbColRow";
      this.cbColRow.Size = new System.Drawing.Size(104, 21);
      this.cbColRow.TabIndex = 3;
      this.cbColRow.Text = "comboBox1";
      this.cbColRow.SelectedIndexChanged += new EventHandler(this.cbColRow_SelectedIndexChanged);
      //
      // btClose
      //
      this.btClose.BackColor = System.Drawing.SystemColors.Control;
      this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btClose.Location = new System.Drawing.Point(112, 72);
      this.btClose.Name = "btClose";
      this.btClose.TabIndex = 4;
      this.btClose.Text = "Close";
      this.btClose.Click += new EventHandler(this.btClose_Click);
      //
      // CellSizeDialog
      //
      this.AcceptButton = this.btClose;
      ////this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.SystemColors.Control;
      this.CancelButton = this.btClose;
      this.ClientSize = new System.Drawing.Size(194, 104);
      this.Controls.Add(this.btClose);
      this.Controls.Add(this.cbColRow);
      this.Controls.Add(this.lblColRow);
      this.Controls.Add(this.lblWidthHeight);
      this.Controls.Add(this.txtWidthHeight);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "CellSizeDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Grid properties";
      ((System.ComponentModel.ISupportInitialize)(this.txtWidthHeight)).EndInit();
      this.ResumeLayout(false);
    }
    #endregion

    #region Windows events
    private void cbColRow_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      if (ownerGrid != null && cbColRow.SelectedIndex >= 0)
      {
        if (cellSizeMode == CellSizeMode.Column)
        {
          columnId = cbColRow.SelectedIndex;
          txtWidthHeight.Value = ownerGrid.Columns[columnId].Width;
        }
        else if (cellSizeMode == CellSizeMode.Row)
        {
          rowId = cbColRow.SelectedIndex;
          txtWidthHeight.Value = ownerGrid.Rows[rowId].Height;
        }
      }
    }

    private void txtWidthHeight_ValueChanged(object sender, System.EventArgs e)
    {
      if (ownerGrid != null)
      {
        if (cellSizeMode == CellSizeMode.Column)
        {
          ownerGrid.Columns[columnId].Width = (int)txtWidthHeight.Value;
        }
        else if (cellSizeMode == CellSizeMode.Row)
        {
          ownerGrid.Rows[rowId].Height = (int)txtWidthHeight.Value;
        }
      }
    }

    private void btClose_Click(object sender, System.EventArgs e)
    {
      Close();
    }
    #endregion
  }
}