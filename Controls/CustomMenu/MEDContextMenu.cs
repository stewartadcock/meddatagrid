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
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Fr.Medit.MedDataGrid.Controls.CustomMenu
{
  /// <summary>
  /// Menu with customised appearance
  /// </summary>
  public class MEDContextMenu : System.ComponentModel.Component, IContextMenu
  {
    #region private members
    /// <summary>
    /// Variable nécessaire au concepteur.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private int MenuItemHeight = 22;
    private int BitmapWidth = 20;
    //private int BitmapHeight = 22;
    private int VerticalTextOffset = 0;
    private int HorizontalTextOffset = 6;
    private int SeparatorHeight = 6;
    private int RightOffset = 15;
    private Dictionary<MenuItem, string> menuItemIconCollection;
    private Font ItemFont;
    private RectangleF BitmapBounds;
    private RectangleF MenuItemBounds;
    private RectangleF ItemBounds;
    private RectangleF ItemTextBounds;

    private Color __BitmapBackColorStart = Color.White;
    private Color __BitmapBackColorEnd = Color.Gray;
    private Color __MenuItemBackColorStart = Color.Snow;
    private Color __MenuItemBackColorEnd = Color.Gainsboro;
    private Color __MenuItemForeColor = Color.Navy;
    private Color __MenuItemForeColorDisabled = Color.Gray;
    private Color __MenuItemBackColorSelected = Color.FromArgb(182, 189, 210);
    private Color __MenuItemBackColorSelectedStart = Color.FromArgb(247, 200, 134);
    private Color __MenuItemBackColorSelectedEnd = Color.FromArgb(247, 166, 51);
    private bool __SpecialBackColorSelected = false;
    private Color __MenuItemBorderSelected = Color.Indigo;
    private bool __MenuItemDithered = true;

    private string RadioCheckIcon;
    private string RadioUnCheckIcon;
    private string CheckIcon;
    private string UnCheckIcon;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="MEDContextMenu"/> class.
    /// </summary>
    /// <param name="container">The container.</param>
    public MEDContextMenu(System.ComponentModel.IContainer container)
    {
      container.Add(this);
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MEDContextMenu"/> class.
    /// </summary>
    public MEDContextMenu()
    {
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
        if (ItemFont != null)
        {
          ItemFont.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Code généré par le Concepteur de composants
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
    }
    #endregion

    #region "Appearance Properties"
    // ***********************************************************************
    // public properties defining the appearance of the menu

    // ***********************************************************************
    // PROPERTY: BitmapBackColor
    // NOTES   : Background color of the bitmap (if Color.Empty, use the  
    //           menu item background color)
    public Color BitmapBackColorStart
    {
      get
      {
        return __BitmapBackColorStart;
      }
      set
      {
        __BitmapBackColorStart = value;
      }
    }

    public Color BitmapBackColorEnd
    {
      get
      {
        return __BitmapBackColorEnd;
      }
      set
      {
        __BitmapBackColorEnd = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemBackColorStart
    // NOTES   : Start color of a menu item dithered background. if ( not 
    //           dithered, this is the menu item background color
    public Color MenuItemBackColorStart
    {
      get
      {
        return __MenuItemBackColorStart;
      }
      set
      {
        __MenuItemBackColorStart = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemBackColorEnd
    // NOTES   : } color of a menu item dithered background. ! used if 
    //           the background is not dithered
    public Color MenuItemBackColorEnd
    {
      get
      {
        return __MenuItemBackColorEnd;
      }
      set
      {
        __MenuItemBackColorEnd = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemForeColor
    // NOTES   : Color of the menu item text
    public Color MenuItemForeColor
    {
      get
      {
        return __MenuItemForeColor;
      }
      set
      {
        __MenuItemForeColor = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemForeColorDisabled
    // NOTES   : Color of the menu item text when the item is disabled
    public Color MenuItemForeColorDisabled
    {
      get
      {
        return __MenuItemForeColorDisabled;
      }
      set
      {
        __MenuItemForeColorDisabled = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemBackColorSelected
    // NOTES   : Background color when a menu item is selected
    [
    Category("MenuItemBackColorSelected"),
    Description("Gets or sets the background color")
    ]
    public Color MenuItemBackColorSelected
    {
      get
      {
        return __MenuItemBackColorSelected;
      }
      set
      {
        __MenuItemBackColorSelected = value;
      }
    }

    public Color MenuItemBackColorSelectedStart
    {
      get { return __MenuItemBackColorSelectedStart; }
      set { __MenuItemBackColorSelectedStart = value; }
    }

    public Color MenuItemBackColorSelectedEnd
    {
      get { return __MenuItemBackColorSelectedEnd; }
      set { __MenuItemBackColorSelectedEnd = value; }
    }

    public bool SpecialBackColorSelected
    {
      get { return __SpecialBackColorSelected; }
      set { __SpecialBackColorSelected = value; }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemBorderSelected
    // NOTES   : Border color when a menu item is selected
    public Color MenuItemBorderSelected
    {
      get
      {
        return __MenuItemBorderSelected;
      }
      set
      {
        __MenuItemBorderSelected = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: Font 
    // NOTES   : Font object to use for menu items
    public Font Font
    {
      get
      {
        return ItemFont;
      }
      set
      {
        ItemFont = value;
      }
    }

    // ***********************************************************************
    // PROPERTY: MenuItemDithered
    // NOTES   : Gets and sets whether the background of the menu item must be
    //           painted with a gradient brush
    public bool MenuItemDithered
    {
      get
      {
        return __MenuItemDithered;
      }
      set
      {
        __MenuItemDithered = value;
      }
    }

    #endregion

    #region Images for Check and Radio states

    /// <summary>
    /// Gets or sets the bullet image.
    /// </summary>
    /// <value>The bullet image.</value>
    public string BulletImage
    {
      get { return RadioCheckIcon; }
      set { RadioCheckIcon = value; }
    }

    /// <summary>
    /// Gets or sets the bullet empty image.
    /// </summary>
    /// <value>The bullet empty image.</value>
    public string BulletEmptyImage
    {
      get { return RadioUnCheckIcon; }
      set { RadioUnCheckIcon = value; }
    }

    /// <summary>
    /// Gets or sets the check image.
    /// </summary>
    /// <value>The check image.</value>
    public string CheckImage
    {
      get { return CheckIcon; }
      set { CheckIcon = value; }
    }

    /// <summary>
    /// Gets or sets the un check image.
    /// </summary>
    /// <value>The uncheck image.</value>
    public string UnCheckImage
    {
      get { return UnCheckIcon; }
      set { UnCheckIcon = value; }
    }

    #endregion

    // ***********************************************************************
    // METHOD: Init
    // INPUT : menu object to subclass
    // NOTES : Entry point method, subclasses the menu and iteratively marks 
    //         child items as ownerdraw. Popup menus are not modified.
    public void Init(System.Windows.Forms.Menu menu)
    {
      // return if the menu is null
      if (menu == null)
      {
        return;
      }

      // Initialize the font object used to render the menu items
      if ((ItemFont == null))
      {
        ItemFont = new Font("Microsoft Sans Serif", 8.25F);
      }

      // Initialize the hashtable used to hold bitmap/item bindings
      if (menuItemIconCollection == null)
      {
        menuItemIconCollection = new Dictionary<MenuItem, string>();
      }

      // Iterate on all top-level menus and handle their items
      foreach (MenuItem popup in menu.MenuItems)
      {
        //  MakeItemOwnerDraw(popup);
        //  HandleChildMenuItems(popup);
        HandleChildMenuItems(popup);
      }

    }

    // ***********************************************************************
    // METHOD: AddIcon
    // INPUT : menu item, icon file
    // NOTES : Binds the given menu item with the specified icon name.
    //         The icon can be any file supported by GDI+ (18x18)
    public void AddIcon(MenuItem item, string iconName)
    {

      // Add the image to the collection. The menu item object is used 
      // as the key of the hash table. The collection is not null by design
      menuItemIconCollection.Add(item, iconName);
    }

    public void ChangeIcon(MenuItem item, string newiconName)
    {
      // change the image to the collection.
      menuItemIconCollection[item] = newiconName;
    }

    // ***********************************************************************
    // HELPER: HandleChildMenuItems
    // INPUT : popup menu 
    // NOTES : Mark the current item as owner draw and recursively processes
    //         all children
    public virtual void HandleChildMenuItems(Menu popupMenu)
    {
      MakeItemOwnerDraw((MenuItem)popupMenu);
      // Mark as ownerdraw the current item and iterate on its children
      foreach (MenuItem item in popupMenu.MenuItems)
      {
        MakeItemOwnerDraw(item);
        HandleChildMenuItems(item);
      }
    }

    // ***********************************************************************
    // HELPER: MakeItemOwnerDraw
    // INPUT : menu item
    // NOTES : Turns the ownerdraw mode on for the specified item
    public virtual void MakeItemOwnerDraw(MenuItem item)
    {
      item.OwnerDraw = true;

      item.DrawItem += new System.Windows.Forms.DrawItemEventHandler(MyDrawItem);
      item.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(MyMeasureItem);
    }

    //***********************************************************************
    // METHOD: Close
    // INPUT : void
    // NOTES : Frees any resource held by the object
    public void Close()
    {
      // Free the font
      if (!(ItemFont == null))
      {
        ItemFont.Dispose();
      }

      // Clear the icon collection
      menuItemIconCollection.Clear();
    }

    // ***********************************************************************
    // HELPER: MyMeasureItem
    // INPUT : menu item, ad hoc structure for measurement
    // NOTES : event handler for the MeasureItem event typical of 
    //         ownerdraw objects
    public void MyMeasureItem(object sender, MeasureItemEventArgs e)
    {

      // Grab a reference to the menu item being measured
      MenuItem item = (MenuItem)sender;

      // if ( it is a separator, handle differently
      if (item.Text == "-")
      {
        e.ItemHeight = SeparatorHeight;
        return;
      }

      // Measure the item text with the current font. The text to 
      // measure includes keyboard shortcuts
      SizeF stringSize;
      stringSize = e.Graphics.MeasureString(GetEffectiveText(item), ItemFont);

      // set { the height and width of the item
      e.ItemHeight = MenuItemHeight;
      e.ItemWidth = BitmapWidth + HorizontalTextOffset + (int)stringSize.Width + RightOffset;
    }


    // ***********************************************************************
    // HELPER: MyDrawItem
    // INPUT : menu item, ad hoc structure for custom drawing
    // NOTES : event handler for the DrawItem event typical of 
    //         ownerdraw objects
    public void MyDrawItem(object sender, DrawItemEventArgs e)
    {

      // Grab a reference to the item being drawn
      MenuItem item = (MenuItem)sender;

      // Saves helper objects for easier reference
      Graphics g = e.Graphics;
      RectangleF bounds = MakeRectangleF(e.Bounds);
      DrawItemState itemState = e.State;

      // Define bounding rectangles to use later
      CreateLayout(bounds);

      // Draw the menu item background and text
      DrawBackground(g, itemState);

      // Draw the bitmap leftmost area
      DrawBitmap(g, item, itemState);

      // Draw the text
      DrawText(g, item, itemState);
    }

    // ***********************************************************************
    // HELPER: MakeRectangleF
    // INPUT : rectangle expressed with integer coordinates
    // OUT   : A RectangleF object
    // NOTES : Converts coordinates of the rectangle to float values
    public RectangleF MakeRectangleF(Rectangle rect)
    {
      RectangleF rectF = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

      return rectF;
    }

    // ***********************************************************************
    // HELPER: CreateLayout
    // INPUT : Base item rectangle (expressed with float values)
    // NOTES : Create additional rectangles to delimit helpful areas like
    //         the text area, the bitmap area, and a few more
    public void CreateLayout(RectangleF bounds)
    {

      // Define the overall menu item area
      MenuItemBounds = bounds;

      // Define the Bitmap area
      BitmapBounds = MenuItemBounds;
      BitmapBounds.Width = BitmapWidth + 2;

      // Define the Client area (everything right of the bitmap)
      ItemBounds = bounds;
      ItemBounds.X = BitmapWidth;

      // Define the Text area (including text offset)
      ItemTextBounds = new RectangleF(0, 0, 0, 0);
      ItemTextBounds.X = (float)BitmapWidth + HorizontalTextOffset;
      ItemTextBounds.Y = (float)bounds.Y + VerticalTextOffset;
      ItemTextBounds.Width = (float)bounds.Width;
      ItemTextBounds.Height = (float)bounds.Height;
    }

    // ***********************************************************************
    // HELPER: DrawBitmap
    // INPUT : Graphics, menu item, state of the item
    // NOTES : Renders the bitmap for the current item taking into account the 
    //         current state of the item
    public virtual void DrawBitmap(Graphics g, MenuItem item, DrawItemState itemState)
    {
      // Grab the current state of the menu item 
      //bool isSelected = (itemState & DrawItemState.Selected) != 0;
      bool isDisabled = (itemState & DrawItemState.Disabled) != 0;
      bool isChecked = (itemState & DrawItemState.Checked) != 0;

      Bitmap bmp = null;

      // Determine the bitmap to use if checked, radio-checked, normal 
      if (isChecked == true)
      {
        if (item.RadioCheck)
        {
          bmp = (Bitmap)GetEmbeddedImage(this.RadioCheckIcon);
        }
        else
        {
          bmp = (Bitmap)GetEmbeddedImage(this.CheckIcon);
        }
      }
      else
      {
        if (item.RadioCheck)
        {
          bmp = (Bitmap)GetEmbeddedImage(this.RadioUnCheckIcon);
        }
        else
        {
          if (menuItemIconCollection.ContainsKey(item))
          {
            bmp = (Bitmap)GetEmbeddedImage(menuItemIconCollection[item]);
          }
        }
      }

      // if no valid bitmap is found, exit.
      if (bmp == null)
      {
        return;
      }

      // Make the bitmap transparent
      bmp.MakeTransparent();

      // Render the bitmap (the bitmap is grayed out if the 
      // item is disabled)
      if (isDisabled == true)
      {
        ImageAttributes imageAttr = new ImageAttributes();
        imageAttr.SetGamma(0.2F);
        Rectangle tmpRect = new Rectangle((int)BitmapBounds.X + 2, (int)BitmapBounds.Y + 2, (int)BitmapBounds.Width - 2, (int)BitmapBounds.Right - 2);
        g.DrawImage(bmp, tmpRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
        imageAttr.ClearGamma();
      }
      else
      {
        g.DrawImage(bmp, BitmapBounds.X + 2, BitmapBounds.Y + 2);
      }

      // Free the resource.
      bmp.Dispose();
    }

    // ***********************************************************************
    // HELPER: DrawBackground
    // INPUT : Graphics, state of the item
    // NOTES : Fills the  background of the item including the bitmap area
    public void DrawBackground(Graphics g, DrawItemState itemState)
    {

      // some helper variables
      Brush backBrush = null;
      Brush bitmapBrush = null;
      Pen borderPen = null;
      bool selected, disabled, paintBitmapArea;
      Rectangle rectToPaint;

      // Determine the state of the item
      selected = ((itemState & DrawItemState.Selected) > 0);
      disabled = ((itemState & DrawItemState.Disabled) > 0);

      // Determine whether the bitmap vertical strip must be created
      paintBitmapArea = !(BitmapBackColorStart.Equals(Color.Empty) && BitmapBackColorEnd.Equals(Color.Empty));
      if (paintBitmapArea)
      {
        rectToPaint = Rectangle.Round(ItemBounds);
      }
      else
      {
        rectToPaint = Rectangle.Round(MenuItemBounds);
      }

      // Determine the brushes to use based on the state
      if (selected && !disabled)
      {
        if (SpecialBackColorSelected)
        {
          backBrush = new LinearGradientBrush(rectToPaint,
            MenuItemBackColorSelectedStart,
            MenuItemBackColorSelectedEnd,
            LinearGradientMode.Horizontal);
        }
        else
        {
          backBrush = new SolidBrush(MenuItemBackColorSelected);
        }
        borderPen = new Pen(MenuItemBorderSelected);
      }
      else
      {
        if (MenuItemDithered)
        {
          backBrush = new LinearGradientBrush(rectToPaint,
            MenuItemBackColorStart, MenuItemBackColorEnd, LinearGradientMode.Horizontal);
          borderPen = null;
        }
        else
        {
          backBrush = new SolidBrush(MenuItemBackColorStart);
        }
      }

      // Fill the area
      // NOTE:
      //    When you fill an area larger than the linear gradient, the
      //    end color is used to fill it. This ensures that we also have 
      //    the bitmap area painted with the end color of the gradient.
      //    This is for free

      if ((selected && !disabled))
      {
        rectToPaint = Rectangle.Round(MenuItemBounds);
        g.FillRectangle(backBrush, rectToPaint);

        bitmapBrush = new SolidBrush(MenuItemBackColorSelectedStart);  // dessine pour la zone du bitmap une couleur identique
        g.FillRectangle(bitmapBrush, BitmapBounds);

        // Draw border
        rectToPaint.Width -= 1;
        rectToPaint.Height -= 1;
        g.DrawRectangle(borderPen, rectToPaint);
      }
      else
      {
        g.FillRectangle(backBrush, rectToPaint);
        if (paintBitmapArea)
        {
          bitmapBrush = new LinearGradientBrush(BitmapBounds,
            __BitmapBackColorStart, __BitmapBackColorEnd, LinearGradientMode.Horizontal);
          g.FillRectangle(bitmapBrush, BitmapBounds);
        }
      }

      // Cleanup objects
      if (!(bitmapBrush == null))
      {
        bitmapBrush.Dispose();
      }
      backBrush.Dispose();
      if (!(borderPen == null))
      {
        borderPen.Dispose();
      }
    }

    // ***********************************************************************
    // HELPER: DrawText
    // INPUT : Graphics, menu item 
    // NOTES : Paints the text of the menu item 
    private void DrawText(Graphics g, MenuItem item, DrawItemState itemState)
    {
      // the foreground brush
      Brush foreBrush;

      // Handle the separator as a special case; then return
      if (item.Text == "-")
      {
        DrawSeparator(g);
        return;
      }

      // Determine the foreground brush to use based on the state
      // NOTE: you could use gradients too
      if (!item.Enabled)
      {
        foreBrush = new SolidBrush(MenuItemForeColorDisabled);
      }
      else
      {
        foreBrush = new SolidBrush(MenuItemForeColor);
      }

      // if ( default item, use bold
      Font tmpFont;
      bool defaultItem;
      defaultItem = ((itemState & DrawItemState.Default) > 0);
      if ((defaultItem))
      {
        tmpFont = new Font(ItemFont, FontStyle.Bold);
      }
      else
      {
        tmpFont = ItemFont;
      }

      // get { text and keyboard shortcut mySingleton to paint
      string textToPaint = GetEffectiveText(item);

      // Text and shortcut are null-separated. Split the string in two parts
      string[] parts = textToPaint.Split('\0');

      // Format the string(s) to render
      StringFormat strFormat = new StringFormat();
      strFormat.HotkeyPrefix = HotkeyPrefix.Show;
      strFormat.LineAlignment = StringAlignment.Center;

      // Paint text
      if (parts.Length == 1)
      {
        // Paint when no shortcut mySingleton is found
        g.DrawString(textToPaint, tmpFont, foreBrush, ItemTextBounds, strFormat);
      }
      else
      {
        // Paint text when shortcut mySingleton is found
        g.DrawString(parts[0], tmpFont, foreBrush, ItemTextBounds, strFormat);

        // Paint right-aligned shortcut mySingleton
        RectangleF rect = new RectangleF(ItemBounds.X, ItemBounds.Y, ItemBounds.Width, ItemBounds.Height);
        rect.Width -= BitmapWidth + HorizontalTextOffset + RightOffset;
        strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
        g.DrawString(parts[1], tmpFont, foreBrush, rect, strFormat);
      }

      // Cleanup resources
      foreBrush.Dispose();
    }

    // ***********************************************************************
    // HELPER: GetEffectiveText
    // INPUT : menu item 
    // OUT   : text + expanded shortcut mySingleton
    // NOTES : Adds shortcut mySingleton to the menu item text. Shortcut mySingleton
    //         doesn//t contain + separators. The function adds them all
    //         and separates the two parts with a null [Chr(0)]
    private string GetEffectiveText(MenuItem item)
    {
      string finalText = item.Text;
      string tmp = null;

      // Separates each component in the shortcut string with a +
      // A typical shortcut is CtrlO. We insert a + after each upper 
      // case character 
      if (item.ShowShortcut && item.Shortcut != Shortcut.None)
      {
        string buf = item.Shortcut.ToString();
        tmp = buf;
        for (int index = 0; index <= buf.Length - 1; index++)
        {
          if (char.IsUpper(buf[index]))
          {
            if (index > 0)
            {
              tmp = tmp.Insert(index, "+");
            }
          }
        }
      } //
      //finalText += '\0' + tmp;
      finalText = string.Format("{0}{1}{2}", item.Text, '\0', tmp);

      return finalText;
    }

    // ***********************************************************************
    // HELPER: DrawSeparator
    // INPUT : Graphics
    // NOTES : Paints a separator line 1 pixel thin
    private void DrawSeparator(Graphics g)
    {
      Pen sepPen = new Pen(MenuItemForeColorDisabled, 1);
      g.DrawLine(sepPen, ItemTextBounds.X, ItemTextBounds.Y, ItemTextBounds.X + ItemTextBounds.Right, ItemTextBounds.Y);
      sepPen.Dispose();
    }

    private static Image GetEmbeddedImage(string p_Image)
    {
      if (p_Image == null)
      {
        return null;
      }

      System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      Stream resourceStream = executingAssembly.GetManifestResourceStream("Fr.Medit.MedDataGrid.Controls.CustomMenu.Icons." + p_Image);
      Image image = Image.FromStream(resourceStream);
      return image;
    }
  }
}
