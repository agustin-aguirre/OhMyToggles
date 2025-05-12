using OhMyToggles.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


namespace OhMyToggles
{
	public class PointerInteractableOption : SimpleOption, IPointerInteractableOption
	{
		[field: SerializeField] public bool UpdateVisualsWhenIsOn { get; set; } = false;
		[field: SerializeField] public UserPointerEventRaiseCondition PointerEventConfig { get; set; } = UserPointerEventRaiseCondition.ONLY_WHEN_SELECTED_BY_USER;
		[field: SerializeField] public PointerRelativeState State { get; protected set; }
		[field: SerializeField] public bool PlayerIsPressingIt { get; private set; } = false;



		protected virtual void Awake()
		{
			if (IsOn) State = PointerRelativeState.IdleWhenSelected;
		}


		public virtual void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState)
		{
			// Empty on purpose.0
			// Have Fun!
		}


		public override void Select(bool newIsOnValue)
		{
			if (newIsOnValue == IsOn) return;

			if (Group != null && !Group.OptionCanBeSetToSelectedValue(this, newIsOnValue)) return;

			IsOn = newIsOnValue;

			// first we turn off every other option that needs to be off (even if they raise events)
			Group?.AssertConsistency(this, newIsOnValue);

			State = IsOn ? PointerRelativeState.IdleWhenSelected : PointerRelativeState.IdleWhenNotSelected;
			
			// then we change our visual and raise our event if needed
			ForceUpdateVisualsToPointerRelativeState(State);

			handleEventTrigger();
		}


		public void OnPointerDown(PointerEventData eventData)
		{
			PlayerIsPressingIt = true;
			State = IsOn ? PointerRelativeState.PressedWhenSelected : PointerRelativeState.PressedWhenNotSelected;
			updateVisualsToPointerInteraction(State);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			PlayerIsPressingIt = false;
			State = IsOn ? PointerRelativeState.IdleWhenSelected : PointerRelativeState.IdleWhenNotSelected;
			updateVisualsToPointerInteraction(State);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			State = IsOn ? PointerRelativeState.HoveredWhenSelected : PointerRelativeState.HoveredWhenNotSelected;
			updateVisualsToPointerInteraction(State);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			State = IsOn ? PointerRelativeState.IdleWhenSelected : PointerRelativeState.IdleWhenNotSelected;
			updateVisualsToPointerInteraction(State);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			PlayerIsPressingIt = false;
			Toggle();
		}


		protected void updateVisualsToPointerInteraction(PointerRelativeState newState)
		{
			if (IsOn && !UpdateVisualsWhenIsOn) return;
			ForceUpdateVisualsToPointerRelativeState(newState);
		}

		protected override void handleEventTrigger()
		{
			bool selectedByUser = State == PointerRelativeState.PressedWhenNotSelected || State == PointerRelativeState.PressedWhenSelected;
			bool shouldNeverRaiseEvent = EventRaiseCondition == OptionEventRaiseCondition.NEVER || (PointerEventConfig == UserPointerEventRaiseCondition.NEVER && selectedByUser);

			if (shouldNeverRaiseEvent) return;

			if (PointerEventConfig != UserPointerEventRaiseCondition.ALWAYS)
			{
				if (PointerEventConfig == UserPointerEventRaiseCondition.ONLY_WHEN_SELECTED_BY_USER && !selectedByUser ||
					PointerEventConfig == UserPointerEventRaiseCondition.ONLY_WHEN_SELECTED_ON_CODE && selectedByUser) return;
			}

			if (EventRaiseCondition != OptionEventRaiseCondition.ALWAYS)
			{
				if (EventRaiseCondition == OptionEventRaiseCondition.ONLY_WHEN_TURNED_ON && !IsOn) return;
				if (EventRaiseCondition == OptionEventRaiseCondition.ONLY_WHEN_TURNED_OFF && IsOn) return;
			}
			
			forceBroadcastEvent(IsOn);
		}
	}
}
