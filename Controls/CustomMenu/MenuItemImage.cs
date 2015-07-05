using System;
using System.Runtime.InteropServices;

namespace Fr.Medit.MedDataGrid.Controls.CustomMenu
{
  /// <summary>
  /// A menu with Image support.
  /// </summary>
  public class MenuItemImage : System.Windows.Forms.MenuItem
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="mergeType">One of the <see cref="T:System.Windows.Forms.MenuMerge"></see> values.</param>
    /// <param name="mergeOrder">The relative position that this menu item will take in a merged menu.</param>
    /// <param name="shortcut">One of the <see cref="T:System.Windows.Forms.Shortcut"></see> values.</param>
    /// <param name="text">The caption for the menu item.</param>
    /// <param name="onClick">The <see cref="T:System.EventHandler"></see> that handles the <see cref="E:System.Windows.Forms.MenuItem.Click"></see> event for this menu item.</param>
    /// <param name="onPopup">The <see cref="T:System.EventHandler"></see> that handles the <see cref="E:System.Windows.Forms.MenuItem.Popup"></see> event for this menu item.</param>
    /// <param name="onSelect">The <see cref="T:System.EventHandler"></see> that handles the <see cref="E:System.Windows.Forms.MenuItem.Select"></see> event for this menu item.</param>
    /// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem"></see> objects that contains the submenu items for this menu item.</param>
    public MenuItemImage ( System.Windows.Forms.MenuMerge mergeType , System.Int32 mergeOrder , System.Windows.Forms.Shortcut shortcut , System.String text , System.EventHandler onClick , System.EventHandler onPopup , System.EventHandler onSelect , System.Windows.Forms.MenuItem[] items )
      :base( mergeType , mergeOrder , shortcut , text , onClick , onPopup , onSelect , items )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    public MenuItemImage ( System.String text , System.Windows.Forms.MenuItem[] items )
      :base(text, items)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="shortcut">The shortcut.</param>
    public MenuItemImage ( System.String text , System.EventHandler onClick , System.Windows.Forms.Shortcut shortcut )
      :base(text, onClick, shortcut )
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="onClick">The on click.</param>
    public MenuItemImage ( System.String text , System.EventHandler onClick )
      :base(text, onClick)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    public MenuItemImage ( System.String text )
      :base(text)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    public MenuItemImage (  )
      :base()
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="p_Image">The image.</param>
    public MenuItemImage ( System.String text , System.EventHandler onClick , System.Drawing.Image p_Image)
      :base(text, onClick)
    {
      SetImage(p_Image);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="p_Image">The image.</param>
    public MenuItemImage ( System.String text , System.Drawing.Image p_Image)
      :base(text)
    {
      SetImage(p_Image);
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="onClick">The on click.</param>
    /// <param name="p_ImageList">The image list.</param>
    /// <param name="p_ImageIndex">Index of the image.</param>
    public MenuItemImage ( System.String text , System.EventHandler onClick , System.Windows.Forms.ImageList p_ImageList, int p_ImageIndex)
      :base(text, onClick)
    {
      SetImage(p_ImageList, p_ImageIndex);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemImage"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="p_ImageList">The image list.</param>
    /// <param name="p_ImageIndex">Index of the image.</param>
    public MenuItemImage ( System.String text , System.Windows.Forms.ImageList p_ImageList, int p_ImageIndex)
      :base(text)
    {
      SetImage(p_ImageList, p_ImageIndex);
    }

    private MenuImage m_ImageLib = null;

    /// <summary>
    /// Set the image associated with this menu, this method can be called only one time.
    /// </summary>
    /// <param name="p_Image">The image.</param>
    public void SetImage(System.Drawing.Image p_Image)
    {
      System.Windows.Forms.ImageList l_ImageList = new System.Windows.Forms.ImageList();
      l_ImageList.Images.Add(p_Image);

      SetImage(l_ImageList, 0);
    }

    /// <summary>
    /// Set the image associated with this menu, this method can be called only one time.
    /// </summary>
    /// <param name="p_ImageList">The image list.</param>
    /// <param name="p_ImageIndex">Index of the image.</param>
    public void SetImage(System.Windows.Forms.ImageList p_ImageList, int p_ImageIndex)
    {
      if (m_ImageLib != null)
      {
        throw new MEDDataGridException("SetImage already called");
      }

      m_ImageLib = new MenuImage();
      m_ImageLib.ImageList = p_ImageList;

      m_ImageLib.SetMenuImage(this, p_ImageIndex);
    }
  }
}
