using OhMyToggles.Interfaces;
using OhMyToggles.Themes;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace OhMyToggles.Transitions
{
	[Serializable]
	public class SpriteSwapTransition : IPointerEventTransition
	{
		public Sprite IdleWhenNotSelected;
		public Sprite HoveredWhenNotSelected;
		public Sprite PressedWhenNotSelected;
		public Sprite IdleWhenSelected;
		public Sprite HoveredWhenSelected;
		public Sprite PressedWhenSelected;

		public readonly IOption Toggle;
		public readonly Image RenderTarget;


		public SpriteSwapTransition(IOption toggle, Image renderTarget)
		{
			Toggle = toggle;
			RenderTarget = renderTarget;
		}


		public void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState)
		{
			switch (pointerRelativeState)
			{
				case PointerRelativeState.IdleWhenNotSelected:
					setSprite(IdleWhenNotSelected);
					break;
				case PointerRelativeState.HoveredWhenNotSelected:
					setSprite(HoveredWhenNotSelected);
					break;
				case PointerRelativeState.PressedWhenNotSelected:
					setSprite(PressedWhenNotSelected);
					break;
				case PointerRelativeState.IdleWhenSelected:
					setSprite(IdleWhenSelected);
					break;
			}

			switch (pointerRelativeState)
			{
				case PointerRelativeState.HoveredWhenSelected:
					setSprite(HoveredWhenSelected);
					break;
				case PointerRelativeState.PressedWhenSelected:
					setSprite(PressedWhenSelected);
					break;
			}
		}


		protected virtual void setSprite(Sprite sprite)
		{
			RenderTarget.sprite = sprite;
		}



		public void UseTheme(ImageTheme theme)
		{
			IdleWhenNotSelected = theme.IdleWhenNotSelected;
			HoveredWhenNotSelected = theme.HoveredWhenNotSelected;
			PressedWhenNotSelected = theme.PressedWhenNotSelected;
			IdleWhenSelected = theme.IdleWhenSelected;
			HoveredWhenSelected = theme.HoveredWhenSelected;
			PressedWhenSelected = theme.PressedWhenSelected;
		}
	}
}
