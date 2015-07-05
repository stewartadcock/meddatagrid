#region MIT License
//
// Filename: BitmapCell.cs
//
// Copyright © 2011-2013 Felix Concordia SARL. All rights reserved.
// Felix Concordia SARL, 400 avenue Roumanille, Bat 7 - BP 309, 06906 Sophia-Antipolis Cedex, FRANCE.
// 
// Copyright © 2005-2011 MEDIT S.A. All rights reserved.
// MEDIT S.A., 2 rue du Belvedere, 91120 Palaiseau, FRANCE.
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

using System.Drawing;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.Cells.Virtual
{
  /// <summary>
  /// A Cell with a Bitmap.
  /// </summary>
  /// <remarks>
  /// This Cell is of type BitmapCell.
  /// Abstract, you must override GetValue and SetValue.
  /// </remarks>
  [ComVisible(false)]
  public abstract class BitmapCell : CellVirtual, ICellBitmapCell
  {
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapCell"/> class.
    /// </summary>
    protected BitmapCell()
    {
      DataModel = new DataModels.DataModelBase(typeof(Image));
      VisualModel = VisualModels.BitmapCell.Default;
      BehaviorModels.Add(BehaviorModel.BitmapCellBehaviorModel.Default);
    }
    #endregion

    /// <summary>
    /// return image value
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual Bitmap GetBitmap(Position position)
    {
      return (Bitmap)GetValue(position);
    }

    /// <summary>
    /// Set image value, call the Model.SetCellValue. Can be called only if EnableEdit is true
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="p_Bitmap">The bitmap.</param>
    public virtual void SetBitmap(Position position, Bitmap p_Bitmap)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, p_Bitmap);
      }
    }
  }
}

namespace Fr.Medit.MedDataGrid.Cells.Real
{
  /// <summary>
  /// A Cell with a BitmapCell. This Cell is of type Bitmap.
  /// </summary>
  [ComVisible(false)]
  public class BitmapCell : Cell, ICellBitmapCell
  {
    private string captionText;

    #region Constructor
    /// <summary>
    /// Construct a BitmapCell class with no caption, and align
    /// the image in MiddleCenter position
    /// </summary>
    /// <param name="p_InitialValue">The initial value.</param>
    public BitmapCell(Bitmap p_InitialValue)
      : this(null, p_InitialValue)
    {
    }

    /// <summary>
    /// Construct a BitmapCell class with no caption, and align
    /// the image in MiddleCenter position
    /// </summary>
    public BitmapCell()
      : this(null, null)
    {
    }

    /// <summary>
    /// Construct a BitmapCell class with caption and align checkbox in the MiddleLeft
    /// </summary>
    /// <param name="p_Caption">The caption.</param>
    /// <param name="p_InitialValue">The initial value.</param>
    public BitmapCell(string p_Caption, Bitmap p_InitialValue)
    {
      captionText = p_Caption;

      DataModel = new DataModels.DataModelBase(typeof(Image));

      if (p_Caption == null || p_Caption.Length <= 0)
      {
        VisualModel = VisualModels.BitmapCell.Default;
      }
      else
      {
        VisualModel = VisualModels.BitmapCell.MiddleLeftAlign;
      }

      BehaviorModels.Add(BehaviorModel.BitmapCellBehaviorModel.Default);
      Value = p_InitialValue;

      IsLastExpandedCell = false;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the checked status (equal to the Value property but returns a Bitmap)
    /// </summary>
    /// <value>The image.</value>
    public Bitmap Image
    {
      get { return GetBitmap(Range.Start); }
      set { SetBitmap(Range.Start, value); }
    }

    /// <summary>
    /// Gets or sets the caption of the cell
    /// </summary>
    /// <value>The caption.</value>
    public string Caption
    {
      get { return this.captionText; }
      set { this.captionText = value; }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Checked status (equal to the Value property but returns a Bitmap). Call the GetValue
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public virtual Bitmap GetBitmap(Position position)
    {
      return (Bitmap)GetValue(position);
    }

    /// <summary>
    /// Set checked value, call the Model.SetCellValue. Can be called only if EnableEdit is true
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="bitmap">The bitmap.</param>
    public virtual void SetBitmap(Position position, Bitmap bitmap)
    {
      if (DataModel != null && DataModel.EnableEdit)
      {
        DataModel.SetCellValue(this, position, bitmap);
      }
    }
    #endregion
  }
}