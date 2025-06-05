using OhMyToggles.Interfaces;
using OhMyToggles.Themes;
using OhMyToggles.Transitions;
using UnityEngine;
using UnityEngine.UI;


namespace OhMyToggles
{
	public class SpriteSwapToggle : PointerInteractableOption
	{
		public Image Target;

		public Sprite IdleWhenNotSelected;
		public Sprite HoveredWhenNotSelected;
		public Sprite PressedWhenNotSelected;
		public Sprite IdleWhenSelected;
		public Sprite HoveredWhenSelected;
		public Sprite PressedWhenSelected;

		[SerializeField] ImageTheme theme;

		SpriteSwapTransition transition;



		private void Awake()
		{
			transition = new(this, Target);
			transition.UseTheme(theme);
		}


		public override void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState) => transition.ForceUpdateVisualsToPointerRelativeState(pointerRelativeState);
	}
}
