using OhMyToggles.Interfaces;


namespace OhMyToggles.Transitions
{
	public interface IPointerEventTransition
	{
		void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState);
	}
}
