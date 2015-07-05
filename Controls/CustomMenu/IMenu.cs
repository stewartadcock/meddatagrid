using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Fr.Medit.MedDataGrid.Controls.CustomMenu
{
  /// <summary>
  /// Description résumée de IMenu.
  /// </summary>
  public interface IMenu
  {
    #region Methods

    // ***********************************************************************
    // METHOD: Init
    // INPUT : menu object to subclass
    // NOTES : Entry point method, subclasses the menu and iteratively marks 
    //         child items as ownerdraw. Popup menus are not modified.
    void Init( Menu menu);

    // ***********************************************************************
    // HELPER: HandleChildMenuItems
    // INPUT : popup menu 
    // NOTES : Mark the current item as owner draw and recursively processes
    //         all children
    void HandleChildMenuItems(Menu popupMenu);

    // ***********************************************************************
    // HELPER: MakeItemOwnerDraw
    // INPUT : menu item
    // NOTES : Turns the ownerdraw mode on for the specified item
    void MakeItemOwnerDraw(MenuItem item);

    // ***********************************************************************
    // HELPER: MakeRectangleF
    // INPUT : rectangle expressed with integer coordinates
    // OUT   : A RectangleF object
    // NOTES : Converts coordinates of the rectangle to float values
    RectangleF MakeRectangleF( Rectangle rect);

    // ***********************************************************************
    // HELPER: CreateLayout
    // INPUT : Base item rectangle (expressed with float values)
    // NOTES : Create additional rectangles to delimit helpful areas like
    //         the text area, the bitmap area, and a few more
    void CreateLayout( RectangleF bounds);

    // ***********************************************************************
    // HELPER: DrawBitmap
    // INPUT : Graphics, menu item, state of the item
    // NOTES : Renders the bitmap for the current item taking into account the 
    //         current state of the item
    void DrawBitmap( Graphics g,  MenuItem item,  DrawItemState itemState);

    // ***********************************************************************
    // HELPER: DrawBackground
    // INPUT : Graphics, state of the item
    // NOTES : Fills the  background of the item including the bitmap area
    void DrawBackground( Graphics g,  DrawItemState itemState);

    #endregion

    #region Property

    string BulletEmptyImage
    {
      get;
      set;
    }

    #endregion

    #region Events
    // ***********************************************************************
    // HELPER: MyMeasureItem
    // INPUT : menu item, ad hoc structure for measurement
    // NOTES : event handler for the MeasureItem event typical of 
    //         ownerdraw objects
    void MyMeasureItem( object sender,  MeasureItemEventArgs e);


    // ***********************************************************************
    // HELPER: MyDrawItem
    // INPUT : menu item, ad hoc structure for custom drawing
    // NOTES : event handler for the DrawItem event typical of 
    //         ownerdraw objects
    void MyDrawItem( object sender,  DrawItemEventArgs e);
    #endregion

  }
}
