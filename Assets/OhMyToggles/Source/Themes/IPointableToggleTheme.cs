namespace OhMyToggles.Themes
{
	public interface IPointableToggleTheme<T>
	{
		T IdleWhenNotSelected { get; }
		T HoveredWhenNotSelected { get; }
		T PressedWhenNotSelected { get; }
		T IdleWhenSelected { get; }
		T HoveredWhenSelected { get; }
		T PressedWhenSelected { get; }
	}
}
