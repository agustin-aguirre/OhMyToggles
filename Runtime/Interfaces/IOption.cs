namespace OhMyToggles.Interfaces
{
	public interface IOption : IToggle, IObservableToggable<IOption>
	{
		OptionEventRaiseCondition EventRaiseCondition { get; set; }
		OptionsGroup Group { get; set; }
		void ForceIsOnValue(bool newIsOnValue);
	}

	public enum OptionEventRaiseCondition
	{
		NEVER = 0,
		ONLY_WHEN_TURNED_ON = 1,
		ONLY_WHEN_TURNED_OFF = 2,
		ALWAYS = 3
	}
}
