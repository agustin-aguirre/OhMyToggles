using OhMyToggles.Interfaces;
using UnityEngine;


namespace OhMyToggles
{
	public class SimpleOption : MonoBehaviour, IOption
	{
		[field: SerializeField] public virtual bool IsOn { get; protected set; } = false;
		[field: SerializeField] public OptionEventRaiseCondition EventRaiseCondition { get; set; } = OptionEventRaiseCondition.ONLY_WHEN_TURNED_ON;
		[field: SerializeField] public OptionsGroup Group { get; set; }

		public event System.Action<IOption, bool> OnSwitched = delegate { };


		public void ForceIsOnValue(bool newIsOnValue) => IsOn = newIsOnValue;

		public virtual void Toggle() => Select(!IsOn);

		public virtual void Select(bool newIsOnValue)
		{
			if (newIsOnValue == IsOn) return;

			if (Group != null && !Group.OptionCanBeSetToSelectedValue(this, newIsOnValue)) return;

			IsOn = newIsOnValue;

			Group?.AssertConsistency(this, newIsOnValue);

			handleEventTrigger();
		}


		protected virtual void handleEventTrigger()
		{
			if (EventRaiseCondition == OptionEventRaiseCondition.NEVER) return;
			if (EventRaiseCondition != OptionEventRaiseCondition.ALWAYS)
			{
				if (EventRaiseCondition == OptionEventRaiseCondition.ONLY_WHEN_TURNED_ON && !IsOn ||
					EventRaiseCondition == OptionEventRaiseCondition.ONLY_WHEN_TURNED_OFF && IsOn) return;
			}
			forceBroadcastEvent(IsOn);
		}


		protected void forceBroadcastEvent(bool newSelectedValue) => OnSwitched.Invoke(this, newSelectedValue);
	}
}
