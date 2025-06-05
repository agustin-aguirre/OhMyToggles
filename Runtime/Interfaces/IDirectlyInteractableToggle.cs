using UnityEngine.EventSystems;


namespace OhMyToggles.Interfaces
{
	public interface IPointerInteractableOption : IOption,
		IPointerEnterHandler,
		IPointerExitHandler,
		IPointerDownHandler,
		IPointerClickHandler
	{
		bool PlayerIsPressingIt { get; }
		bool UpdateVisualsWhenIsOn { get; set; }
		PointerRelativeState State { get; }
		UserPointerEventRaiseCondition PointerEventConfig { get; set; }
		void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState);
	}


	public enum PointerRelativeState
	{
		IdleWhenNotSelected = 0,
		HoveredWhenNotSelected = 1,
		PressedWhenNotSelected = 2,
		IdleWhenSelected = 3,
		HoveredWhenSelected = 4,
		PressedWhenSelected = 5
	}

	public enum UserPointerEventRaiseCondition
	{
		NEVER = 0,
		ONLY_WHEN_SELECTED_BY_USER = 1,
		ONLY_WHEN_SELECTED_ON_CODE = 2,
		ALWAYS = 3
	}
}
