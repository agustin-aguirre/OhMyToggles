namespace OhMyToggles.Interfaces
{
	public interface IObservableToggable<out TToggable> : IToggable
		where TToggable : IToggable
	{
		event System.Action<TToggable, bool> OnSwitched;
	}
}
