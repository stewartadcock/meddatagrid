#region MIT License
//
// Filename: Common.cs
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
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Fr.Medit.MedDataGrid.Cells;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// Class to manage the visual aspect of a cell. This class can be shared between multiple cells.
  /// SAA TODO: Merge class into VisualModelBase to remove an unneeded layer of indirection.
  /// </summary>
  [ComVisible(false), Serializable]
  public class Common : VisualModelBase
  {
    /// <summary>
    /// Represents a default Model
    /// </summary>
    public static readonly Common Default = new Common(true);

    /// <summary>
    /// Represents a model with a link style font and forecolor.
    /// </summary>
    public static readonly Common LinkStyle;

    #region Class variables
    private Image cellImage;

    private Color selectionBackColor;
    private Color focusForeColor;
    private Color focusBackColor;
    private Color selectionForeColor;
    [NonSerialized]
    private RectangleBorder focusBorder;
    [NonSerialized]
    private RectangleBorder selectionBorder;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes the <see cref="Common"/> class.
    /// </summary>
    static Common()
    {
      LinkStyle = new Common(false);
      LinkStyle.Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline);
      LinkStyle.ForeColor = Color.Blue;
      LinkStyle.isReadOnly = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Common"/> class.
    /// Use default setting and construct a read and write VisualProperties
    /// </summary>
    public Common()
      : this(false)
    {
      ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Common"/> class.
    /// Use default setting
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public Common(bool isReadOnly)
      : base(isReadOnly)
    {
      this.selectionBackColor = Color.Yellow;
      this.selectionForeColor = Color.Black;
      this.focusBackColor = Color.LightBlue;
      this.focusForeColor = ForeColor;

      this.cellImage = null;
      this.imageAlignment = ContentAlignment.MiddleLeft;
      this.doImageStretch = false;
      this.doAlignTextToImage = true;

      // Border
      this.focusBorder = Border;
      this.selectionBorder = Border;

      this.ExpandedCell = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Common"/> class.
    /// </summary>
    /// <remarks>
    /// Copy Constructor.  This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </remarks>
    /// <param name="source">The source.</param>
    /// <param name="isReadOnly">if set to <c>true</c> is read only.</param>
    public Common(Common source, bool isReadOnly)
      : base(source, isReadOnly)
    {
      // Duplicate the reference fields
      Image l_tmpImage = null;
      if (source.cellImage != null)
      {
        l_tmpImage = ImageClone(source.cellImage);
      }

      this.selectionBackColor = source.selectionBackColor;
      this.selectionForeColor = source.selectionForeColor;
      this.focusBackColor = source.focusBackColor;
      this.focusForeColor = source.focusForeColor;
      this.cellImage = l_tmpImage;
      this.imageAlignment = source.imageAlignment;
      this.doImageStretch = source.ImageStretch;
      this.doAlignTextToImage = source.doAlignTextToImage;
      this.focusBorder = source.focusBorder;
      this.selectionBorder = source.selectionBorder;

      ExpandedCell = false;
    }

    private static Image ImageClone(Image p_Image)
    {
      System.Runtime.Serialization.Formatters.Binary.BinaryFormatter l_BinForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
      using (MemoryStream l_Stream = new MemoryStream())
      {
        l_BinForm.Serialize(l_Stream, p_Image);
        l_Stream.Seek(0, SeekOrigin.Begin);
        return (Image)l_BinForm.Deserialize(l_Stream);
      }
    }
    #endregion

    #region Format
    #region BackColor/ForeColor
    /// <summary>
    /// Gets or sets selection forecolor (when Select is true)
    /// </summary>
    /// <value>The color of the selection foreground.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color SelectionForeColor
    {
      get
      {
        return this.selectionForeColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.selectionForeColor = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets selection backcolor (when Select is true)
    /// </summary>
    /// <value>The color of the selection background.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color SelectionBackColor
    {
      get
      {
        return this.selectionBackColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.selectionBackColor = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets focus ForeColor (when Focus is true)
    /// </summary>
    /// <value>The color of the focus foreground.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color FocusForeColor
    {
      get
      {
        return this.focusForeColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.focusForeColor = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets focus BackColor (when Focus is true)
    /// </summary>
    /// <value>The color of the focus background.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color FocusBackColor
    {
      get
      {
        return this.focusBackColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.focusBackColor = value;
        OnChange();
      }
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets Image of the cell
    /// </summary>
    /// <value>The image.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Image Image
    {
      get
      {
        return this.cellImage;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.cellImage = value;
        OnChange();
      }
    }

    private bool doImageStretch = false;
    /// <summary>
    /// Gets or sets a value indicating whether to stretch image to fit cell display area.
    /// </summary>
    /// <value><c>true</c> if image stretch is enabled; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// True to stretch the image otherwise false
    /// </remarks>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public bool ImageStretch
    {
      get
      {
        return this.doImageStretch;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.doImageStretch = value;
        OnChange();
      }
    }

    private bool doAlignTextToImage = true;
    /// <summary>
    /// Gets or sets a value indicating whether to align the text with the image.
    /// </summary>
    /// <value><c>true</c> if align text to image; otherwise, <c>false</c>.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public bool AlignTextToImage
    {
      get
      {
        return this.doAlignTextToImage;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.doAlignTextToImage = value;
        OnChange();
      }
    }

    private ContentAlignment imageAlignment = ContentAlignment.MiddleLeft;
    /// <summary>
    /// Gets or sets Image Alignment
    /// </summary>
    /// <value>The image alignment.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public ContentAlignment ImageAlignment
    {
      get
      {
        return this.imageAlignment;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.imageAlignment = value;
        OnChange();
      }
    }
    #endregion

    #region Border
    /// <summary>
    /// Gets or sets the border of a cell when it has the focus.
    /// </summary>
    /// <value>The focus border.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public RectangleBorder FocusBorder
    {
      get
      {
        return this.focusBorder;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.focusBorder = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the border of the cell when it is selected.
    /// </summary>
    /// <value>The selection border.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public RectangleBorder SelectionBorder
    {
      get
      {
        return this.selectionBorder;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.selectionBorder = value;
        OnChange();
      }
    }
    #endregion
    #endregion

    #region Clone
    /// <summary>
    /// Clone this object. This method duplicates all the reference field (Image, Font, StringFormat) creating a new instance.
    /// </summary>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    public override object Clone(bool isReadOnly)
    {
      return new Common(this, isReadOnly);
    }
    #endregion

    #region GetRequiredSize
    /// <summary>
    /// Returns the minimum required size of the current cell, calculating using the
    /// current DisplayString, Image and Borders information.
    /// </summary>
    /// <param name="p_Graphics">GDI+ drawing surface</param>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <returns></returns>
    public override SizeF GetRequiredSize(Graphics p_Graphics,
      Cells.ICellVirtual p_Cell,
      Position p_CellPosition)
    {
      return VisualModelBase.CalculateRequiredSize(
        p_Graphics,
        p_Cell.GetDisplayText(p_CellPosition),
        StringFormat,
        GetCellFont(),
        this.cellImage,
        this.imageAlignment,
        this.doAlignTextToImage,
        this.doImageStretch,
        Border);
    }
    #endregion

    #region DrawCell
    /// <summary>
    /// Draw the background of the specified cell. Background
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_Background(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status)
    {
      if (p_ClientRectangle.Width == 0 || p_ClientRectangle.Height == 0)
      {
        return;
      }

      Color l_BackColor;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_BackColor = FocusBackColor;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_BackColor = SelectionBackColor;
      }
      else
      {
        l_BackColor = BackColor;
      }

      SolidBrush br = new SolidBrush(l_BackColor);
      e.Graphics.FillRectangle(br, p_ClientRectangle);
    }

    /// <summary>
    /// Draw the borders of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_Border(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status)
    {
      RectangleBorder l_Border = Border;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_Border = FocusBorder;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_Border = SelectionBorder;
      }

      ControlPaint.DrawBorder(e.Graphics, p_ClientRectangle,
        l_Border.Left.Color,
        l_Border.Left.Width,
        ButtonBorderStyle.Solid,
        l_Border.Top.Color,
        l_Border.Top.Width,
        ButtonBorderStyle.Solid,
        l_Border.Right.Color,
        l_Border.Right.Width,
        ButtonBorderStyle.Solid,
        l_Border.Bottom.Color,
        l_Border.Bottom.Width,
        ButtonBorderStyle.Solid);
    }

    /// <summary>
    /// Draw the image and the displaystring of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">Cell status</param>
    protected override void DrawCell_ImageAndText(ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status)
    {
      bool l_lastExpandedCell;

      ICell cell = p_Cell as ICell;
      if (cell == null)
      { // this must be a Virtual Cell.
        l_lastExpandedCell = false;
      }
      else
      { // this is a Real Cell
        if (cell.Grid.Rows[cell.Row].Height == 0)
        { // It is safe to quit now if this cell isn't visible.
          return;
        }
        l_lastExpandedCell = cell.IsLastExpandedCell;
      }

      RectangleBorder l_Border = Border;
      Color l_ForeColor = ForeColor;
      if (p_Status == DrawCellStatus.Focus)
      {
        l_Border = FocusBorder;
        l_ForeColor = FocusForeColor;
      }
      else if (p_Status == DrawCellStatus.Selected)
      {
        l_Border = SelectionBorder;
        l_ForeColor = SelectionForeColor;
      }

      Font l_CurrentFont = GetCellFont();

      bool l_Expand = ExpandedCell;

      // Image and Text
      VisualModelBase.PaintImageAndText(e.Graphics,
        p_ClientRectangle,
        Image,
        ImageAlignment,
        ImageStretch,
        p_Cell.GetDisplayText(p_CellPosition),
        StringFormat,
        AlignTextToImage,
        l_Border,
        l_ForeColor,
        l_CurrentFont,
        l_Expand,
        l_lastExpandedCell);
    }
    #endregion
  }
}