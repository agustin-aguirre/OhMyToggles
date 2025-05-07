using OhMyToggles.Interfaces;
using UnityEngine;
using UnityEngine.Events;


namespace OhMyToggles.Utils
{
	public class ToggleObserver : MonoBehaviour
	{
		public IOption ObservedToggle { get; private set; }

		public UnityEvent<bool> OnSelectedValueChanged;
		public UnityEvent OnSelected;
		public UnityEvent OnDiselected;


		private void Awake()
		{
			ObservedToggle = GetComponent<IOption>();
			if (ObservedToggle == null)
			{
				Debug.LogWarning($"No toggle found to observe in gameobject {name}. Disabling component.");
				enabled = false;
				return;
			}


			ObservedToggle.OnSwitched += (toggle, newValue) =>
			{
				OnSelectedValueChanged?.Invoke(newValue);
				(newValue ? OnSelected : OnDiselected)?.Invoke();
			};
		}
	}
}
