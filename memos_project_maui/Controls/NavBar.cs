using CommunityToolkit.Maui.Markup;
using memos_project_maui.Utilities;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace memos_project_maui.Controls;

public class NavBar : Grid 
{
    #region NavTitle
    public static readonly BindableProperty NavTitleProperty = BindableProperty.Create(
        nameof(NavTitleProperty),
        typeof(string),
        typeof(NavBar),
        defaultValue: "",
        propertyChanged: (bindable, oldval, newval) =>
        {
            NavBar view = (NavBar)bindable;
            string newTitle = (string)newval;

            if (view == null || string.IsNullOrEmpty(newTitle))
                return;

            view._titleLabel.Text = newTitle;
        });

    public string NavTitle
    {
        set => SetValue(NavTitleProperty, value);
        get => (string)GetValue(NavTitleProperty);
    }
    #endregion

    #region BackButton
    public static readonly BindableProperty ShowBackButtonProperty = BindableProperty.Create(
        nameof(ShowBackButtonProperty),
        typeof(bool),
        typeof(NavBar),
        defaultValue: false,
        propertyChanged: (bindable, oldval, newval) =>
        {
            NavBar view = (NavBar)bindable;
            bool show = (bool)newval;

            if (view == null || !show)
            {
                view.Children.Remove(view._backButton);
                return;
            }

            view.Children.Remove(view._menuButton);
            view.Children.Add(view._backButton.Column(0).Start());
        });

    public bool ShowBackButton
    {
        set => SetValue(ShowBackButtonProperty, value);
        get => (bool)GetValue(ShowBackButtonProperty);
    }
    #endregion

    #region MenuButton
    public static readonly BindableProperty ShowMenuButtonProperty = BindableProperty.Create(
        nameof(ShowMenuButtonProperty),
        typeof(bool),
        typeof(NavBar),
        defaultValue: false,
        propertyChanged: (bindable, oldval, newval) =>
        {
            NavBar view = (NavBar)bindable;
            bool show = (bool)newval;

            if (view == null || !show)
            {
                view.Children.Remove(view._menuButton);
                return;
            }

            view.Children.Remove(view._backButton);
            view.Children.Add(view._menuButton.Column(0).Start());
        });

    public bool ShowMenuButton
    {
        set => SetValue(ShowMenuButtonProperty, value);
        get => (bool)GetValue(ShowMenuButtonProperty);
    }
    #endregion

    private const int _navBarHeight = 45;
    private const int _actionSize = 24;
    private const int _titleSize = 18;

    private Image _backButton = UIUtils.IconButton(
        UIUtils.MaterialIcon(MaterialFont.Arrow_back_ios_new, _actionSize, true),
        new Action(() =>
        {
            Shell.Current.GoToAsync("..");
        }));

    private Image _menuButton = UIUtils.IconButton(
        UIUtils.MaterialIcon(MaterialFont.Menu, _actionSize, true),
        new Action(() =>
        {
            // TODO: implement when flyout is supported
        }));

    private Label _titleLabel = new()
    {
        FontSize = _titleSize,
        FontAttributes = FontAttributes.Bold,
        VerticalOptions = LayoutOptions.Center,
        HorizontalTextAlignment = TextAlignment.Center
    };

    public NavBar()
	{
        Padding = new Thickness(8,0,8,0);
        HeightRequest = _navBarHeight;
        BackgroundColor = Colors.Transparent;
        ColumnDefinitions = Columns.Define(
            new GridLength(0.20, GridUnitType.Star),
            new GridLength(0.60, GridUnitType.Star),
            new GridLength(0.20, GridUnitType.Star));

        _titleLabel.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);
        _titleLabel.Text = NavTitle;
        Children.Add(_titleLabel.Column(1));
	}
}
