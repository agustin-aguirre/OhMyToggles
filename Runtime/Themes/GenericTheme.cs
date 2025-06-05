using UnityEngine;


namespace OhMyToggles.Themes
{
	public class GenericTheme<T> : ScriptableObject, IPointableToggleTheme<T>
	{
		[field: SerializeField] public T IdleWhenNotSelected { get; private set; }
		[field: SerializeField] public T HoveredWhenNotSelected { get; private set; }
		[field: SerializeField] public T PressedWhenNotSelected { get; private set; }
		[field: SerializeField] public T IdleWhenSelected { get; private set; }
		[field: SerializeField] public T HoveredWhenSelected { get; private set; }
		[field: SerializeField] public T PressedWhenSelected { get; private set; }
	}
}
