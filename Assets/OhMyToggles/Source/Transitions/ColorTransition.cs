using OhMyToggles.Interfaces;
using OhMyToggles.Themes;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace OhMyToggles.Transitions
{
	[Serializable]
	public class ColorTransition : IPointerEventTransition
	{
		public Color IdleWhenNotSelected;
		public Color HoveredWhenNotSelected;
		public Color PressedWhenNotSelected;
		public Color IdleWhenSelected;
		public Color HoveredWhenSelected;
		public Color PressedWhenSelected;

		public readonly Image RenderTarget;
		public readonly IOption Toggle;


		public ColorTransition(IOption toggle, Image renderTarget)
		{
			Toggle = toggle;
			RenderTarget = renderTarget;
		}

		public void UseTheme(ColorTheme theme)
		{
			IdleWhenNotSelected = theme.IdleWhenNotSelected;
			HoveredWhenNotSelected = theme.HoveredWhenNotSelected;
			PressedWhenNotSelected = theme.PressedWhenNotSelected;
			IdleWhenSelected = theme.IdleWhenSelected;
			HoveredWhenSelected = theme.HoveredWhenSelected;
			PressedWhenSelected = theme.PressedWhenSelected;
		}


		public void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState)
		{
			switch (pointerRelativeState)
			{
				case PointerRelativeState.IdleWhenNotSelected:
					setColor(IdleWhenNotSelected);
					break;
				case PointerRelativeState.HoveredWhenNotSelected:
					setColor(HoveredWhenNotSelected);
					break;
				case PointerRelativeState.PressedWhenNotSelected:
					setColor(PressedWhenNotSelected);
					break;
				case PointerRelativeState.IdleWhenSelected:
					setColor(IdleWhenSelected);
					break;
			}

			switch (pointerRelativeState)
			{
				case PointerRelativeState.HoveredWhenSelected:
					setColor(HoveredWhenSelected);
					break;
				case PointerRelativeState.PressedWhenSelected:
					setColor(PressedWhenSelected);
					break;
			}
		}

		protected virtual void setColor(Color color)
		{
			if (RenderTarget != null)
				RenderTarget.color = color;
		}
	}
}
