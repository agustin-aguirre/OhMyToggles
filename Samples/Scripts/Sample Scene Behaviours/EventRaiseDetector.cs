using OhMyToggles;
using OhMyToggles.Interfaces;
using UnityEngine;
using UnityEngine.UI;


public class EventRaiseDetector : MonoBehaviour
{
	[SerializeField] Image renderTarget;
	[SerializeField] Text text;
	[SerializeField] float fadeDuration = .5f;
	[SerializeField] float totalAnimTime = 1.5f;
	[SerializeField] Color colorToFade;

	IOption[] options;

	float fadeDelta = 0f;
	float totalAnimDelta = 0f;
	float timeBeforeFading => totalAnimTime - fadeDuration;
	float timeBeforeFadingDelta = 0f;


	private void Reset()
	{
		if (renderTarget == null)
			renderTarget = GetComponent<Image>();
	}


	private void Awake()
	{
		options = FindObjectsByType<PointerInteractableOption>(FindObjectsSortMode.None);
	}

	private void Start()
	{
		foreach (var option in options)
		{
			option.OnSwitched += handleOnSwitch;
		}
	}


	void handleOnSwitch(IOption option, bool newIsOnValue)
	{
		totalAnimDelta = totalAnimTime;
		timeBeforeFadingDelta = timeBeforeFading;
		fadeDelta = fadeDuration;
		renderTarget.color = new Color(colorToFade.r, colorToFade.g, colorToFade.b, 1f);
		text.text = ((MonoBehaviour)option).GetComponentInChildren<Text>().text;
	}


	private void Update()
	{
		if (totalAnimDelta <= 0) return;

		if (timeBeforeFadingDelta > 0f)
		{
			timeBeforeFadingDelta = Mathf.Max(0f, timeBeforeFadingDelta - Time.deltaTime);
		}
		else
		{
			fadeDelta = Mathf.Max(0f, fadeDelta - Time.deltaTime);
			var targetColor = renderTarget.color;
			targetColor.a = fadeDelta / fadeDuration;
			renderTarget.color = targetColor;
		}
		totalAnimDelta = Mathf.Max(0f, totalAnimDelta - Time.deltaTime);
	}
}
