using OhMyToggles.Interfaces;
using OhMyToggles.Transitions;
using OhMyToggles.Themes;
using UnityEngine;
using UnityEngine.UI;


namespace OhMyToggles
{
	public class ColorTransitionToggle : PointerInteractableOption
	{
		public Image TargetGraphic;

		public ColorTheme Theme;

		[HideInInspector, SerializeField] ColorTransition transition;


		protected override void Awake()
		{
			base.Awake();
			ApplyTheme();
		}


#if UNITY_EDITOR
		[ContextMenu("Apply Theme")]
#endif
		public void ApplyTheme()
		{
			transition = new ColorTransition(this, TargetGraphic);

			if (Theme != null)
			{
				transition.UseTheme(Theme);
			}

			ForceUpdateVisualsToPointerRelativeState(State);
		}


		public override void ForceUpdateVisualsToPointerRelativeState(PointerRelativeState pointerRelativeState) => transition.ForceUpdateVisualsToPointerRelativeState(pointerRelativeState);



#if UNITY_EDITOR
		[ContextMenu("Turn On")]
		public void TurnOn()
		{
			ForceIsOnValue(false);
			Select(true);
		}
		
		[ContextMenu("Turn Off")]
		public void TurnOff()
		{
			ForceIsOnValue(true);
			Select(false);
		}
#endif
	}
}
