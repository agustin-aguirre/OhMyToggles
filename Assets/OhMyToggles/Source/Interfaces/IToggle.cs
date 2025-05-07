namespace OhMyToggles.Interfaces
{
	public interface IToggle : IToggable
	{
		bool IsOn { get; }
		void Select(bool newIsOnValue);
	}
}
