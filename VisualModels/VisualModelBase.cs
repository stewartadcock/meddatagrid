#region MIT License
//
// Filename: VisualModelBase.cs
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
using System.Windows.Forms;

using Fr.Fc.FcCore.Logging;

namespace Fr.Medit.MedDataGrid.VisualModels
{
  /// <summary>
  /// Class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
  /// SAA TODO: Merge VisualModels.Common into this.
  /// </summary>
  [Serializable]
  public abstract class VisualModelBase : IVisualModel
  {
    #region Constants
    private const int MaxStringWidth = 2048;
    #endregion

    #region Class variables
    [NonSerialized]
    private StringFormat stringFormat = StringFormat.GenericDefault;

    private bool isExpandedCell;

    private Font font;
    private Color backColor;
    private Color foreColor;

    [NonSerialized]
    private RectangleBorder border;
    #endregion

    #region Constructors
    /// <summary>
    /// Use default setting and construct a read and write VisualProperties
    /// </summary>
    protected VisualModelBase()
      : this(false)
    {
    }

    /// <summary>
    /// Use default setting
    /// </summary>
    /// <param name="isReadOnly">if set to <c>true</c> this cell is is read only.</param>
    protected VisualModelBase(bool isReadOnly)
    {
      this.backColor = Color.FromKnownColor(KnownColor.Window);
      this.foreColor = Color.FromKnownColor(KnownColor.WindowText);

      this.font = null; // new Font(FontFamily.GenericSansSerif,8.25f); if null the grid font is used

      this.stringFormat = (StringFormat)StringFormat.GenericDefault.Clone();
      TextAlignment = ContentAlignment.MiddleLeft;
      WordWrap = false;
      this.border = RectangleBorder.Default;
      this.isReadOnly = isReadOnly;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualModelBase"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="isReadOnly">if set to <c>true</c> model is read only.</param>
    /// <remarks>
    /// Copy Constructor.
    /// This method duplicates all reference fields (Image, Font, StringFormat) to create a new instance.
    /// </remarks>
    protected VisualModelBase(VisualModelBase source, bool isReadOnly)
    {
      // Duplicate the reference fieldsD
      Font l_tmpFont = null;
      if (source.font != null)
      {
        l_tmpFont = (Font)source.font.Clone();
      }
      StringFormat l_tmpStringFormat = null;
      if (source.stringFormat != null)
      {
        l_tmpStringFormat = (StringFormat)source.stringFormat.Clone();
      }

      this.isReadOnly = isReadOnly;
      this.backColor = source.backColor;
      this.foreColor = source.foreColor;
      this.font = l_tmpFont;
      this.stringFormat = l_tmpStringFormat;
      this.border = source.border;
    }
    #endregion

    #region ReadOnly
    /// <summary>
    /// ReadOnly variable.
    /// </summary>
    protected bool isReadOnly = false;

    /// <summary>
    /// Gets a value indicating whetherthis class is ReadOnly otherwise False.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public bool ReadOnly
    {
      get { return this.isReadOnly; }
    }

    /// <summary>
    /// Make the current instance readonly. Use this method to prevent unexpected changes.
    /// </summary>
    public void MakeReadOnly()
    {
      this.isReadOnly = true;
    }

    /// <summary>
    /// Make the current instance not readonly.
    /// </summary>
    public void MakeNonReadOnly()
    {
      this.isReadOnly = false;
    }
    #endregion

    #region Events
    /// <summary>
    /// Fired when you change a property of this class
    /// </summary>
    public event EventHandler Change;

    /// <summary>
    /// Fired when you change a property of this class
    /// </summary>
    protected virtual void OnChange()
    {
      if (Change != null)
      {
        Change(this, EventArgs.Empty);
      }
    }
    #endregion

    #region Format
    /// <summary>
    /// Gets or sets StringFormat of the cell
    /// </summary>
    /// <value>The string format.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    [System.ComponentModel.Browsable(false)]
    public StringFormat StringFormat
    {
      get
      {
        return this.stringFormat;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.stringFormat = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the font.
    /// </summary>
    /// <value>The font.</value>
    /// <remarks>
    /// If null, the grid font is used
    /// </remarks>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Font Font
    {
      get
      {
        return this.font;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.font = value;
        OnChange();
      }
    }

    #region BackColor/ForeColor
    /// <summary>
    /// Gets or sets the color of the cell background.
    /// </summary>
    /// <value>The color of the background.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color BackColor
    {
      get
      {
        return this.backColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.backColor = value;
        OnChange();
      }
    }

    /// <summary>
    /// Gets or sets the color of the cell foreground.
    /// </summary>
    /// <value>The color of the foreground.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public Color ForeColor
    {
      get
      {
        return this.foreColor;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.foreColor = value;
        OnChange();
      }
    }
    #endregion

    #region Border
    /// <summary>
    /// Gets or sets the normal border of a cell
    /// </summary>
    /// <value>The border.</value>
    /// <exception cref="ObjectIsReadOnlyException">Thrown if VisualProperties is readonly when trying to set property.</exception>
    public RectangleBorder Border
    {
      get
      {
        return this.border;
      }
      set
      {
        if (this.isReadOnly)
        {
          throw new ObjectIsReadOnlyException("VisualProperties is readonly.");
        }
        this.border = value;
        OnChange();
      }
    }
    #endregion

    #region Format Properties Wrapper
    /// <summary>
    /// Gets or sets a value indicating whether Word Wrap is enabled.  This property is only a wrapper around StringFormat
    /// </summary>
    /// <value><c>true</c> if word wrap is enabled; otherwise, <c>false</c>.</value>
    public bool WordWrap
    {
      get
      {
        return (StringFormat.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
      }
      set
      {
        if (value && WordWrap == false)
        {
          StringFormat.FormatFlags = StringFormat.FormatFlags ^ StringFormatFlags.NoWrap;
        }
        else if (value == false)
        {
          StringFormat.FormatFlags = StringFormat.FormatFlags | StringFormatFlags.NoWrap;
        }
      }
    }

    /// <summary>
    /// Gets or sets the text alignment.
    /// This property is only a wrapper around StringFormat
    /// </summary>
    /// <value>The text alignment.</value>
    public ContentAlignment TextAlignment
    {
      get
      {
        if (AlignmentUtility.IsBottom(StringFormat))
        {
          if (AlignmentUtility.IsLeft(StringFormat))
          {
            return ContentAlignment.BottomLeft;
          }
          else if (AlignmentUtility.IsRight(StringFormat))
          {
            return ContentAlignment.BottomRight;
          }
          else // if (Utility.IsCenter(StringFormat))
          {
            return ContentAlignment.BottomCenter;
          }
        }
        else if (AlignmentUtility.IsTop(StringFormat))
        {
          if (AlignmentUtility.IsLeft(StringFormat))
          {
            return ContentAlignment.TopLeft;
          }
          else if (AlignmentUtility.IsRight(StringFormat))
          {
            return ContentAlignment.TopRight;
          }
          else // if (Utility.IsCenter(StringFormat))
          {
            return ContentAlignment.TopCenter;
          }
        }
        else // if (Utility.IsMiddle(StringFormat))
        {
          if (AlignmentUtility.IsLeft(StringFormat))
          {
            return ContentAlignment.MiddleLeft;
          }
          else if (AlignmentUtility.IsRight(StringFormat))
          {
            return ContentAlignment.MiddleRight;
          }
          else // if (Utility.IsCenter(StringFormat))
          {
            return ContentAlignment.MiddleCenter;
          }
        }
      }

      set
      {
        if (AlignmentUtility.IsBottom(value))
        {
          StringFormat.LineAlignment = StringAlignment.Far;
        }
        else if (AlignmentUtility.IsTop(value))
        {
          StringFormat.LineAlignment = StringAlignment.Near;
        }
        else // if (AlignmentUtility.IsMiddle(value))
        {
          StringFormat.LineAlignment = StringAlignment.Center;
        }

        if (AlignmentUtility.IsLeft(value))
        {
          StringFormat.Alignment = StringAlignment.Near;
        }
        else if (AlignmentUtility.IsRight(value))
        {
          StringFormat.Alignment = StringAlignment.Far;
        }
        else // if (Utility.IsCenter(value))
        {
          StringFormat.Alignment = StringAlignment.Center;
        }
      }
    }

    /// <summary>
    /// Get the font of the cell, check whether the current font is null and in this case return
    /// the default grid font.
    /// </summary>
    /// <returns></returns>
    protected Font GetCellFont()
    {
      return Font ?? Control.DefaultFont;
    }
    #endregion
    #endregion

    #region Clone
    /// <summary>
    /// Clone this object. Also ReadOnly flag is copied.
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      return Clone(isReadOnly);
    }

    /// <summary>
    /// Clone this object.
    /// </summary>
    /// <remarks>
    /// This method duplicates all reference fields (Image, Font, StringFormat) to create a new instance.
    /// </remarks>
    /// <param name="isReadOnly">True if the new object must be read only; otherwise <c>false</c>.</param>
    /// <returns></returns>
    public abstract object Clone(bool isReadOnly);
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
    public virtual SizeF GetRequiredSize(Graphics p_Graphics,
      Cells.ICellVirtual p_Cell,
      Position p_CellPosition)
    {
      return CalculateRequiredSize(p_Graphics, p_Cell.GetDisplayText(p_CellPosition), stringFormat, GetCellFont(), null, ContentAlignment.MiddleCenter, false, false, Border);
    }

    /// <summary>
    /// Returns the minimum required size of the current cell, calculating using the
    /// current DisplayString, Image and Borders information.
    /// </summary>
    /// <param name="p_Graphics">GDI+ drawing surface</param>
    /// <param name="p_DisplayText">The display text.</param>
    /// <param name="p_StringFormat">The string format.</param>
    /// <param name="p_Font">The font.</param>
    /// <param name="p_Image">The image.</param>
    /// <param name="p_ImageAlignment">The image alignment.</param>
    /// <param name="p_AlignTextToImage">if set to <c>true</c> align text to image.</param>
    /// <param name="p_ImageStretch">if set to <c>true</c> image stretch.</param>
    /// <param name="p_Border">The border.</param>
    /// <returns></returns>
    protected static SizeF CalculateRequiredSize(Graphics p_Graphics,
      string p_DisplayText,
      StringFormat p_StringFormat,
      Font p_Font,
      Image p_Image,
      ContentAlignment p_ImageAlignment,
      bool p_AlignTextToImage,
      bool p_ImageStretch,
      RectangleBorder p_Border)
    {
      SizeF l_ReqSize;

      // Calculate Text Size
      if (p_DisplayText != null && p_DisplayText.Length > 0)
      {
        l_ReqSize = p_Graphics.MeasureString(p_DisplayText, p_Font, MaxStringWidth, p_StringFormat);
        l_ReqSize.Width += 2; // 2 extra space to always fit the text
        l_ReqSize.Height += 2; // 2 extra space to always fit the text
      }
      else
      {
        l_ReqSize = new SizeF(0, 0);
      }

      // Calculate Image Size
      if (p_Image != null)
      {
        // Check whether align Text To Image
        if (p_ImageStretch == false && p_AlignTextToImage &&
          p_DisplayText != null && p_DisplayText.Length > 0)
        {
          if (AlignmentUtility.IsBottom(p_ImageAlignment) && AlignmentUtility.IsBottom(p_StringFormat))
          {
            l_ReqSize.Height += p_Image.Height;
          }
          else if (AlignmentUtility.IsTop(p_ImageAlignment) && AlignmentUtility.IsTop(p_StringFormat))
          {
            l_ReqSize.Height += p_Image.Height;
          }
          else // Max between Image and Text
          {
            if (p_Image.Height > l_ReqSize.Height)
            {
              l_ReqSize.Height = p_Image.Height;
            }
          }

          if (AlignmentUtility.IsLeft(p_ImageAlignment) && AlignmentUtility.IsLeft(p_StringFormat))
          {
            l_ReqSize.Width += p_Image.Width;
          }
          else if (AlignmentUtility.IsRight(p_ImageAlignment) && AlignmentUtility.IsRight(p_StringFormat))
          {
            l_ReqSize.Width += p_Image.Width;
          }
          else // Max between Image and Text
          {
            if (p_Image.Width > l_ReqSize.Width)
            {
              l_ReqSize.Width = p_Image.Width;
            }
          }
        }
        else
        {
          // Max between Image and Text
          if (p_Image.Height > l_ReqSize.Height)
          {
            l_ReqSize.Height = p_Image.Height;
          }
          if (p_Image.Width > l_ReqSize.Width)
          {
            l_ReqSize.Width = p_Image.Width;
          }
        }
      }

      // Add Border Width
      l_ReqSize.Width += p_Border.Left.Width + p_Border.Right.Width;
      l_ReqSize.Height += p_Border.Top.Width + p_Border.Bottom.Width;

      return l_ReqSize;
    }
    #endregion

    #region Expanded cell
    /// <summary>
    /// Gets or sets a value indicating whether this is an expanded cell.
    /// An expanded cell has lines of expanded nodes as in a tree view.
    /// </summary>
    /// <value>
    /// <c>true</c> if cell is an expanded cell; otherwise, <c>false</c>.
    /// </value>
    public bool ExpandedCell
    {
      get { return this.isExpandedCell; }
      set { this.isExpandedCell = value; }
    }
    #endregion

    #region DrawCell
    /// <summary>
    /// Draw the cell specified
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    public virtual void DrawCell(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle)
    {
      GridVirtual l_Grid = p_Cell.Grid;

      if (p_Cell.DataModel == null || p_Cell.DataModel.EnableCellDrawOnEdit || p_Cell.IsEditing(p_CellPosition) == false)
      {
        Graphics g = e.Graphics;

        // Set the clip region with the cell rectangle (for a bug in the PaintBorders function)
        Region l_PreviousClip = g.Clip;
        g.Clip = new Region(p_ClientRectangle);

        DrawCellStatus l_Status;

        if (l_Grid.FocusCellPosition == p_CellPosition)
        {
          // focus
          l_Status = DrawCellStatus.Focus;
        }
        else if (p_Cell.Grid.Selection.Contains(p_CellPosition))
        {
          // selected
          l_Status = DrawCellStatus.Selected;
        }
        else
        {
          l_Status = DrawCellStatus.Normal;
        }

        DrawCell_Background(p_Cell, p_CellPosition, e, p_ClientRectangle, l_Status);
        DrawCell_Border(p_Cell, p_CellPosition, e, p_ClientRectangle, l_Status);
        DrawCell_ImageAndText(p_Cell, p_CellPosition, e, p_ClientRectangle, l_Status);

        // Reset the clip region
        g.Clip = l_PreviousClip;
      }
    }

    /// <summary>
    /// Draw the background of the specified cell. Background
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">The cell status.</param>
    protected abstract void DrawCell_Background(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status);

    /// <summary>
    /// Draw the borders of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">The cell status.</param>
    protected abstract void DrawCell_Border(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status);

    /// <summary>
    /// Draw the image and the DisplayString of the specified cell.
    /// </summary>
    /// <param name="p_Cell">The cell.</param>
    /// <param name="p_CellPosition">The cell position.</param>
    /// <param name="e">Paint arguments</param>
    /// <param name="p_ClientRectangle">Rectangle position where draw the current cell, relative to the current view,</param>
    /// <param name="p_Status">The cell status.</param>
    protected abstract void DrawCell_ImageAndText(Cells.ICellVirtual p_Cell,
      Position p_CellPosition,
      PaintEventArgs e,
      Rectangle p_ClientRectangle,
      DrawCellStatus p_Status);
    #endregion

    #region Cell drawing utility method
    /// <summary>
    /// Calculate Rectangle with no border.
    /// </summary>
    /// <param name="p_Align">The content alignment.</param>
    /// <param name="p_ClientLeft">The client left.</param>
    /// <param name="p_ClientTop">The client top.</param>
    /// <param name="p_ClientWidth">Width of the client.</param>
    /// <param name="p_ClientHeight">Height of the client.</param>
    /// <param name="p_ObjWidth">Width of the obj.</param>
    /// <param name="p_ObjHeight">Height of the obj.</param>
    /// <returns></returns>
    private static PointF CalculateObjAlignment(ContentAlignment p_Align, int p_ClientLeft, int p_ClientTop, int p_ClientWidth, int p_ClientHeight, float p_ObjWidth, float p_ObjHeight)
    {
      // default X left
      PointF l_pointf = new PointF((float)p_ClientLeft, (float)p_ClientTop);

      // Y
      if (p_Align == ContentAlignment.TopCenter ||
        p_Align == ContentAlignment.TopLeft ||
        p_Align == ContentAlignment.TopRight) // Y Top
      {
        l_pointf.Y = (float)p_ClientTop;
      }
      else if (p_Align == ContentAlignment.BottomCenter ||
        p_Align == ContentAlignment.BottomLeft ||
        p_Align == ContentAlignment.BottomRight) // Y bottom
      {
        l_pointf.Y = (float)p_ClientTop + ((float)p_ClientHeight) - p_ObjHeight;
      }
      else // default Y middle
      {
        l_pointf.Y = (float)p_ClientTop + ((float)p_ClientHeight) / 2.0F - p_ObjHeight / 2.0F;
      }

      if (p_Align == ContentAlignment.BottomCenter ||
        p_Align == ContentAlignment.MiddleCenter ||
        p_Align == ContentAlignment.TopCenter) // X Centre
      {
        l_pointf.X = (float)p_ClientLeft + ((float)p_ClientWidth) / 2.0F - p_ObjWidth / 2.0F;
      }
      else if (p_Align == ContentAlignment.BottomRight ||
        p_Align == ContentAlignment.MiddleRight ||
        p_Align == ContentAlignment.TopRight)// X Right
      {
        l_pointf.X = (float)p_ClientLeft + (float)p_ClientWidth - p_ObjWidth;
        // middle default already set
      }

      return l_pointf;
    }

    /// <summary>
    /// Paint the Text and the Image passed
    /// </summary>
    /// <param name="g">Graphics device where you can render your image and text</param>
    /// <param name="p_displayRectangle">Relative rectangle based on the display area</param>
    /// <param name="p_Image">Image to draw. Can be null.</param>
    /// <param name="p_ImageAlignment">Alignment of the image</param>
    /// <param name="p_ImageStretch">True to make the draw the image with the same size of the cell</param>
    /// <param name="p_Text">Text to draw (can be null)</param>
    /// <param name="p_StringFormat">String format (can be null)</param>
    /// <param name="p_AlignTextToImage">True to align the text with the image</param>
    /// <param name="p_Border">Cell Border</param>
    /// <param name="p_TextColor">Text Color</param>
    /// <param name="p_TextFont">Text Font(can be null)</param>
    /// <param name="p_ExpandedCell">if set to <c>true</c> cell is expanded.</param>
    /// <param name="p_LastExpandedCell">if set to <c>true</c> cell is last expanded cell.</param>
    protected static void PaintImageAndText(Graphics g,
      Rectangle p_displayRectangle,
      Image p_Image,
      ContentAlignment p_ImageAlignment,
      bool p_ImageStretch,
      string p_Text,
      StringFormat p_StringFormat,
      bool p_AlignTextToImage,
      RectangleBorder p_Border,
      Color p_TextColor,
      Font p_TextFont,
      bool p_ExpandedCell,
      bool p_LastExpandedCell)
    {
      // Calculate Rectangle with no border
      Rectangle l_CellRectNoBorder = p_Border.RemoveBorderFromRectangle(p_displayRectangle);

      // Draw image
      if (p_Image != null)
      {
        if (p_ImageStretch) // stretch image
        {
          g.DrawImage(p_Image, l_CellRectNoBorder);
        }
        else
        {
          PointF l_PointImage = CalculateObjAlignment(p_ImageAlignment,
            (int)l_CellRectNoBorder.Left, (int)l_CellRectNoBorder.Top,
            (int)l_CellRectNoBorder.Width, (int)l_CellRectNoBorder.Height,
            p_Image.Width, p_Image.Height);

          RectangleF l_RectDrawImage = l_CellRectNoBorder;
          l_RectDrawImage.Intersect(new RectangleF(l_PointImage, p_Image.PhysicalDimension));

          // Truncate the Rectangle for approximation problem
          g.DrawImage(p_Image, Rectangle.Truncate(l_RectDrawImage));
        }
      }

      // Draw Text
      if (p_Text != null && p_Text.Length > 0)
      {
        if (l_CellRectNoBorder.Width > 0 && l_CellRectNoBorder.Height > 0)
        {
          RectangleF l_RectDrawText = l_CellRectNoBorder;

          // Align Text To Image
          if (p_Image != null && p_ImageStretch == false && p_AlignTextToImage)
          {
            if (AlignmentUtility.IsBottom(p_ImageAlignment) && AlignmentUtility.IsBottom(p_StringFormat))
            {
              l_RectDrawText.Height -= p_Image.Height;
            }
            if (AlignmentUtility.IsTop(p_ImageAlignment) && AlignmentUtility.IsTop(p_StringFormat))
            {
              l_RectDrawText.Y += p_Image.Height;
              l_RectDrawText.Height -= p_Image.Height;
            }
            if (AlignmentUtility.IsLeft(p_ImageAlignment) && AlignmentUtility.IsLeft(p_StringFormat))
            {
              l_RectDrawText.X += p_Image.Width;
              l_RectDrawText.Width -= p_Image.Width;
            }
            if (AlignmentUtility.IsRight(p_ImageAlignment) && AlignmentUtility.IsRight(p_StringFormat))
            {
              l_RectDrawText.Width -= p_Image.Width;
            }
          }

          try
          {
            SolidBrush textBrush = new SolidBrush(p_TextColor);
            g.DrawString(p_Text,
              p_TextFont,
              textBrush,
              l_RectDrawText,
              p_StringFormat);
          }
          catch (Exception ex)
          { // Very long strings can cause a GDI+ exception here.
            LoggerManager.Log(LogLevels.Error, "Cell rendering failed unexpectedly because: " + ex.Message);
          }
        }
      }

      // Draw expanded lines
      if (p_ExpandedCell)
      {
        Pen forePenDark = new Pen(Color.DarkGray, SystemInformation.BorderSize.Height);
        // horizontal line
        Point lineLeft = new Point(l_CellRectNoBorder.Left + (int)((l_CellRectNoBorder.Right - l_CellRectNoBorder.Left) / 2f), l_CellRectNoBorder.Top + (int)((l_CellRectNoBorder.Bottom - l_CellRectNoBorder.Top) / 2f));
        Point lineRight = new Point(l_CellRectNoBorder.Right, (int)(l_CellRectNoBorder.Top + (l_CellRectNoBorder.Bottom - l_CellRectNoBorder.Top) / 2f));
        Point lineTop = new Point(l_CellRectNoBorder.Left + (int)((l_CellRectNoBorder.Right - l_CellRectNoBorder.Left) / 2f), l_CellRectNoBorder.Top);
        Point lineBottom;
        if (p_LastExpandedCell == false)
        {
          lineBottom = new Point(l_CellRectNoBorder.Left + (int)((l_CellRectNoBorder.Right - l_CellRectNoBorder.Left) / 2f), l_CellRectNoBorder.Bottom);
        }
        else
        {
          lineBottom = new Point(l_CellRectNoBorder.Left + (int)((l_CellRectNoBorder.Right - l_CellRectNoBorder.Left) / 2f), l_CellRectNoBorder.Top + (int)((l_CellRectNoBorder.Bottom - l_CellRectNoBorder.Top) / 2f));
        }
        g.DrawLine(forePenDark, lineTop, lineBottom);
        g.DrawLine(forePenDark, lineLeft, lineRight);
      }
    }
    #endregion
  }
}