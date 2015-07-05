using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Fr.Medit.MedDataGrid.Controls.CustomMenu
{
  /// <summary>
  /// A custom extender class that adds a <c>MenuImage</c>
  /// attribute to <c>MenuItem</c> objects, and custom drawns the menu
  /// with an icon stored in a referenced <c>ImageList</c> control.
  /// </summary>
  /// <remarks>
  /// This extension was written to provide an simple way to link
  /// icons in an Imagelist with a menu, and owner draw the menu. Other menu
  /// icon samples sub-class a MenuItem which interferes with the Visual Studio
  /// IDE for designing menus. Other examples required a lot of custom tooling
  /// and hand-coding. By using an extender, no custom coding is required.
  /// </remarks>
  [ProvideProperty("MenuImage", typeof(Component))]
  [DefaultProperty("ImageList")]
  internal sealed class MenuImage : Component, IExtenderProvider
  {
    #region Class members
    /// <summary>
    /// Hashtable is used to relate added <c>MenuItem</c> components
    /// with each custom status messsage attribute value.
    /// </summary>
    private Dictionary<Component, string> _hashTable;

    /// <summary>
    /// Holds a reference to the user selected <c>StatusBar</c>
    /// instance where custom statusmessage attribute values
    /// are displayed.
    /// </summary>
    private ImageList _imageList;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuImage"/> class.
    /// </summary>
    /// <param name="container">Reference to container hosting this instance.</param>
    /// <remarks>
    /// Constructor for instance that supports Class Composition designer.
    /// </remarks>
    public MenuImage(System.ComponentModel.IContainer container)
    {
      container.Add(this);
      this._hashTable = new Dictionary<Component, string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuImage"/> class.
    /// </summary>
    public MenuImage()
    {
      this._hashTable = new Dictionary<Component, string>();
    }
    #endregion

    #region Public Members
    /// <summary>
    /// Used to set a MenuImage property value for
    /// a specific <c>MenuItem</c> component instance.
    /// </summary>
    /// <param name="component">the <c>MenuItem</c> object to store</param>
    /// <param name="indexValue">the image index value to associate with the menu item</param>
    public void SetMenuImage(Component component, int indexValue)
    {

      // store the menuitem and related index in the local hashtable
      if (_hashTable.ContainsKey(component) != true)
      {
        _hashTable.Add(component, indexValue.ToString());
        MenuItem menuItem = (MenuItem)component;

        // set the menu to owner drawn
        menuItem.OwnerDraw = true;

        // hook up the menu owner drawn events
        menuItem.MeasureItem += new MeasureItemEventHandler(OnMeasureItem);
        menuItem.DrawItem += new DrawItemEventHandler(OnDrawItem);
      }
      else
      {
        _hashTable[component] = indexValue.ToString();
      }
    }

    /// <summary>
    /// Used to retrieve the MenuImage extender property value
    /// for a given <c>MenuItem</c> component instance.
    /// </summary>
    /// <param name="component">the menu item instance associated with the value</param>
    /// <returns>Returns the MenuImage index property value for the specified <c>MenuItem</c> component instance.</returns>
    public string GetMenuImage(Component component)
    {
      if (_hashTable.ContainsKey(component))
      {
        return _hashTable[component];
      }

      return null;
    }


    /// <summary>
    /// Used to determine if the given component is supported by
    /// the extender.
    /// </summary>
    /// <param name="component">component to evaluate for compatability</param>
    /// <returns>Returns True/False if the component supports the extender.</returns>
    public bool CanExtend(object component)
    {
      // only support MenuItem objects that are not
      // top-level menus (default rendering for top-level
      // menus is fine - does not need extension
      if (component is MenuItem)
      {
        MenuItem menuItem = (MenuItem)component;
        return !(menuItem.Parent is MainMenu);
      }

      return false;
    }

    /// <summary>
    /// Gets or sets the <c>ImageList</c> control that holds menu images.
    /// </summary>
    /// <value>an <c>ImageList</c> instance that holds menu icons.</value>
    public ImageList ImageList
    {
      get { return _imageList; }
      set { _imageList = value; }
    }

    #endregion

    #region Private Members/Helpers

    /// <summary>
    /// Performs a set of checks related to a menu image such as 
    /// a ImageList has been assigned, the image index is a valid
    /// number and is within the ImageList images collection boundaries, etc.
    /// </summary>
    /// <param name="sender">the client object to retrieve the menuindex for</param>
    private int GetMenuImageIndex(Object sender)
    {
      string menuImageValue = this.GetMenuImage(sender as Component);

      // first check that the ImageList reference has been assigned
      // then verify the specified MenuImage index is valid for the
      // imagelist. Then convert and return the index as an integer
      if (_imageList != null)
        if (menuImageValue != null)
          if (menuImageValue.Length >= 0)
          {
            int imageIndex = Convert.ToInt32(menuImageValue);
            if (imageIndex >= 0 && imageIndex < _imageList.Images.Count)
              return imageIndex;
          }

      return -1;
    }

    /// <summary>
    /// Event triggered to measure the size of a owner drawn <c>MenuItem</c>.
    /// </summary>
    /// <param name="sender">the menu item client object</param>
    /// <param name="e">the event arguments</param>
    private void OnMeasureItem(Object sender, MeasureItemEventArgs e)
    {
      // retrieve the image list index from hash table
      MenuItem menuItem = (MenuItem)sender;
      MenuHelper menuHelper = new MenuHelper(menuItem, e.Graphics, _imageList);

      // calculate the menu height
      e.ItemHeight = menuHelper.CalcHeight();
      e.ItemWidth = menuHelper.CalcWidth();
    }

    /// <summary>
    /// Event triggered to owner draw the provide <c>MenuItem</c>.
    /// </summary>
    /// <param name="sender">the menu item client object</param>
    /// <param name="e">the event arguments</param>
    private void OnDrawItem(Object sender, DrawItemEventArgs e)
    {
      // derive the MenuItem object, and create the MenuHelper
      MenuItem menuItem = (MenuItem)sender;
      MenuHelper menuHelper = new MenuHelper(menuItem, e.Graphics, _imageList);

      // draw the menu background
      bool menuSelected = (e.State & DrawItemState.Selected) > 0;
      menuHelper.DrawBackground(e.Bounds, menuSelected);

      if (menuHelper.IsSeperator() == true)
        menuHelper.DrawSeperator(e.Bounds);
      else
      {
        int imageIndex = this.GetMenuImageIndex(sender);
        menuHelper.DrawMenu(e.Bounds, menuSelected, imageIndex);
      }
    }

    #endregion

    #region MenuHelper Class
    /// <summary>
    /// MenuHelper class
    /// </summary>
    /// <remarks>
    /// </remarks>
    private sealed class MenuHelper
    {
      #region Class members

      // some pre-defined buffer values for putting space between
      // icon, menutext, seperator text, and submenu arrow indicators
      private const int SeperatorHeight = 8;
      private const int SideBorderWidth = 1;
      private const int BorderSize = SideBorderWidth * 2;
      private const int TextLeftPaddingWidth = 15;
      private const int ShortcutPaddingWidth = 20;
      private const int SubmenuArrowWidth = 15;
      private readonly int IconWidth = SystemInformation.SmallIconSize.Width;
      private readonly int IconHeight = SystemInformation.SmallIconSize.Height;
      private const int IconPaddingWidth = 10;

      // holds the local instances of the MenuItem and Graphics
      // objects passed in through the Constructor
      private MenuItem _menuItem = null;
      private Graphics _graphics = null;
      private ImageList _imageList = null;

      private static Image s_menuImageSubItem = null;
      #endregion

      #region Constructors

      /// <summary>
      /// MenuHelper Constructor to assist in owner drawn menus.
      /// </summary>
      /// <param name="menuItem">a <c>MenuItem</c> object to custom draw</param>
      /// <param name="graphics">a <c>Graphics</c> object provided by the <c>MeasureItem</c> and <c>DrawItem</c> events</param>
      /// <param name="imageList"></param>
      public MenuHelper(MenuItem menuItem, Graphics graphics, ImageList imageList)
      {
        _menuItem = menuItem;
        _graphics = graphics;
        _imageList = imageList;

      }

      static MenuHelper()
      {
        // SAA TODO: This image is missing:
        System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(@"MEDSUMO.Fr.Medit.MedDataGrid.Controls.CustomMenu.SubItem16.ico");
        s_menuImageSubItem = Image.FromStream(stream);
      }

      #endregion

      #region Public Members
      /// <summary>
      /// Based on the menu item text, and the <c>SystemInformation.SmallIconSize,</c>
      /// performs a calculation to determine the correct <c>MenuItem</c> height.
      /// </summary>
      /// <returns>Returns an <c>int</c> value that contains the calculated height of the menu item.</returns>
      public int CalcHeight()
      {
        // if the menu is a seperator, then return a fixed height
        // otherwise calculate the menu size based on the system font
        // and smalliconsize calculations (with some added buffer values)
        if (_menuItem.Text == "-")
          return SeperatorHeight;
        else
        {
          // depending on which is longer, set the menu height to either
          // the icon, or the system menu font
          if (SystemInformation.MenuFont.Height > SystemInformation.SmallIconSize.Height)
            return SystemInformation.MenuFont.Height + BorderSize;
          else
            return SystemInformation.SmallIconSize.Height + BorderSize;
        }
      }

      /// <summary>
      /// Based on the menu item text, and the <c>SystemInformation.SmallIconSize,</c>
      /// performs a calculation to determine the correct <c>MenuItem</c> width.
      /// </summary>
      /// <returns>Returns an <c>int</c> value that contains the calculated width of the menu item.</returns>
      public int CalcWidth()
      {
        // prepare string formatting used for rendering menu caption
        StringFormat sf = new StringFormat();
        sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

        // set the menu width by measuring the string, icon and buffer spaces
        int menuWidth = (int)_graphics.MeasureString(_menuItem.Text, SystemInformation.MenuFont, 1000, sf).Width;
        int shortcutWidth = (int)_graphics.MeasureString(this.ShortcutText, SystemInformation.MenuFont, 1000, sf).Width;

        // if a top-level menu, no image support
        if (this.IsTopLevel() == true)
        {
          return menuWidth;
        }
        else
        {
          return IconWidth + IconPaddingWidth + menuWidth + ShortcutPaddingWidth + shortcutWidth;
        }
      }

      /// <summary>
      /// A method to evaluate if the <c>MenuItem</c> has a shortcut selected, and the shortcut
      /// has been selected for show.
      /// </summary>
      /// <returns>Returns True/False whether the menu has a shortcut to be displayed.</returns>
      public bool HasShortcut()
      {
        return (_menuItem.ShowShortcut == true && _menuItem.Shortcut != Shortcut.None);
      }

      /// <summary>
      /// Evaluates whether the <c>MenuItem</c> is a seperator by evaluating the text.
      /// </summary>
      /// <returns>Returns True/False whether the menu is a seperator.</returns>
      public bool IsSeperator()
      {
        return (_menuItem.Text == "-");
      }

      /// <summary>
      /// Evaluates whether the <c>MenuItem</c> is a top-level menu that is sited directly
      /// on a <c>MainMenu</c> control.
      /// </summary>
      /// <returns>Returns True/False if the menu item is a top-level menu.</returns>
      public bool IsTopLevel()
      {
        return (_menuItem.Parent is MainMenu);
      }

      /// <summary>
      /// Formats the <c>MenuItem</c> and returns the shortcut selection as a displayable text string.
      /// </summary>
      public string ShortcutText
      {
        get
        {
          if (_menuItem.ShowShortcut == true && _menuItem.Shortcut != Shortcut.None)
          {
            Keys keys = (Keys)_menuItem.Shortcut;
            return Convert.ToChar(Keys.Tab) + System.ComponentModel.TypeDescriptor.GetConverter(keys.GetType()).ConvertToString(keys);
          }
          return null;
        }
      }

      /// <summary>
      /// Draws a normal menu item including any related icons, checkboxes,
      /// menu text, shortcuts text, and parent/submenu arrows.
      /// </summary>
      /// <param name="bounds">a <c>Rectangle</c> that holds the drawing canvas boundaries</param>
      /// <param name="selected">True/False if the menu item is currently selected</param>
      /// <param name="indexValue">the image index of the menu icon to draw, defaults to -1</param>
      public void DrawMenu(Rectangle bounds, bool selected, int indexValue)
      {
        // draw the menu text
        DrawMenuText(bounds, selected);

        // since icons make the menu height longer,
        // paint a custom arrow if the menu is a parent
        // to augment the one painted by the control
        if (_menuItem.IsParent == true)
        {
          this.DrawArrow(s_menuImageSubItem, bounds);
        }

        // if the menu item is checked, ignore any menuimage index
        // and draw the checkbox, otherwise draw the custom image
        if (_menuItem.Checked)
        {
          DrawCheckBox(bounds);
        }
        else
        {
          // see if the menu item has an icon associated and draw image
          if (indexValue > -1)
          {
            Image menuImage = null;
            menuImage = _imageList.Images[indexValue];
            DrawImage(menuImage, bounds);
          }
        }
      }

      /// <summary>
      /// Draws the <c>MenuItem</c> background.
      /// </summary>
      /// <param name="bounds">a <c>Rectangle</c> that holds the painting canvas boundaries</param>
      /// <param name="selected">True/False if the menu item is currently selected</param>
      public void DrawBackground(Rectangle bounds, bool selected)
      {
        // if selected then paint the menu as highlighted,
        // otherwise use the default menu brush
        if (selected == true)
        {
          _graphics.FillRectangle(SystemBrushes.Highlight, bounds);
        }
        else
        {
          _graphics.FillRectangle(SystemBrushes.Menu, bounds);
        }
      }

      /// <summary>
      /// Draws a menu seperator.
      /// </summary>
      /// <param name="bounds">a <c>Rectangle</c> that holds the drawing canvas boundaries</param>
      public void DrawSeperator(Rectangle bounds)
      {
        // create the seperator line pen
        Pen pen = new Pen(SystemColors.ControlDark);

        // calculate seperator boundaries
        int xLeft = bounds.Left + IconWidth + IconPaddingWidth;
        int xRight = xLeft + bounds.Width;
        int yCenter = bounds.Top + (bounds.Height / 2);

        // draw a seperator line and return
        _graphics.DrawLine(pen, xLeft, yCenter, xRight, yCenter);
      }
      #endregion

      #region Private Members

      /// <summary>
      /// Draws the text for an ownerdrawn <c>MenuItem</c>.
      /// </summary>
      /// <param name="bounds">a <c>Rectangle</c> that holds the drawing area boundaries</param>
      /// <param name="selected">True/False whether the menu item is currently selected</param>
      private void DrawMenuText(Rectangle bounds, bool selected)
      {
        // use system fonts and colors to select the menu brush so the menu
        // will appear correctly for any desktop theme
        Font menuFont = SystemInformation.MenuFont;
        SolidBrush menuBrush = null;
        if (_menuItem.Enabled == false)
          menuBrush = new SolidBrush(SystemColors.GrayText);
        else
        {
          if (selected == true)
            menuBrush = new SolidBrush(SystemColors.HighlightText);
          else
            menuBrush = new SolidBrush(SystemColors.MenuText);
        }

        // draw the menu text
        StringFormat sfMenu = new StringFormat();
        sfMenu.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
        _graphics.DrawString(_menuItem.Text, menuFont, menuBrush, bounds.Left + IconWidth + IconPaddingWidth, bounds.Top + ((bounds.Height - menuFont.Height) / 2), sfMenu);

        // if the menu has a shortcut, then also 
        // draw the shortcut right aligned
        if (this.IsTopLevel() != true || this.HasShortcut() == false)
        {
          StringFormat sfShortcut = new StringFormat();
          sfShortcut.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
          sfShortcut.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
          _graphics.DrawString(this.ShortcutText, menuFont, menuBrush, (bounds.Width) - TextLeftPaddingWidth, bounds.Top + ((bounds.Height - menuFont.Height) / 2), sfShortcut);
        }
      }

      /// <summary>
      /// Draws a checked item next to a <c>MenuItem</c>.
      /// </summary>
      /// <param name="bounds">a <c>Rectangle</c> that identifies the drawing area boundaries</param>
      private void DrawCheckBox(Rectangle bounds)
      {
        // use the very handy ControlPaint object to paint
        // a checkbox. ButtonState is a bitwise flat that can be built
        // to accomodate style and state appearance
        ButtonState btnState = ButtonState.Flat;

        if (_menuItem.Checked == true)
          btnState = btnState | ButtonState.Checked;

        if (_menuItem.Enabled == false)
          btnState = btnState | ButtonState.Inactive;

        // draw the checkbox
        ControlPaint.DrawCheckBox(_graphics, bounds.Left + SideBorderWidth, bounds.Top + ((bounds.Height - IconHeight) / 2), IconWidth, IconHeight, btnState);
      }

      /// <summary>
      /// Draws a provided image onto the <c>MenuItem</c>.
      /// </summary>
      /// <param name="menuImage">an <c>Image</c> to paint on the menu</param>
      /// <param name="bounds">a <c>Rectangle</c> that holds the drawing space boundaries</param>
      private void DrawImage(Image menuImage, Rectangle bounds)
      {
        // if the menu item is enabled, then draw the image normally
        // otherwise draw it as disabled
        if (_menuItem.Enabled == true)
          _graphics.DrawImage(menuImage, bounds.Left + SideBorderWidth, bounds.Top + ((bounds.Height - IconHeight) / 2), IconWidth, IconHeight);
        else
          ControlPaint.DrawImageDisabled(_graphics, menuImage, bounds.Left + SideBorderWidth, bounds.Top + ((bounds.Height - IconHeight) / 2), SystemColors.Menu);
      }

      /// <summary>
      /// Draws a custom arrow on the right-side edge of the menu to indicate
      /// the menu has submenu items. Used to supplement a base control arrow
      /// that is painted incorrectly (seems to be a bug), and make the arrow
      /// appear correctly for longer menu items.
      /// </summary>
      /// <param name="menuImage"></param>
      /// <param name="bounds"></param>
      private void DrawArrow(Image menuImage, Rectangle bounds)
      {
        _graphics.DrawImage(menuImage, bounds.Left + bounds.Width - SubmenuArrowWidth, bounds.Top + ((bounds.Height - IconHeight) / 2), IconWidth, IconHeight);
      }

      #endregion
    }
    #endregion
  }
}
