using OhMyToggles.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OhMyToggles
{
	public enum AutoChildRegisterMoment
	{
		DONT_AUTO_REGISTER = 0,
		ON_AWAKE = 1,
		ON_START = 2
	}
	public enum AutoChildRegisterMethod
	{
		REGISTER_ALL_CHILDREN = 0,
		ONLY_REGISTER_OPTIONS_THAT_ARE_DIRECT_CHILDREN = 1,
		ONLY_REGISTER_NON_ANIDATED_OPTIONS = 2
	}

	public class OptionsGroup : MonoBehaviour
	{
		public IEnumerable<IOption> Options { get; private set; } = new List<IOption>();

		[Header("Config:")]
		public bool AllowMultiOptionSelection = false;
		public bool AllowAllOptionsOff = false;
		public AutoChildRegisterMoment AutoRegisterMoment = AutoChildRegisterMoment.ON_START;
		public AutoChildRegisterMethod AutoRegisterMethod = AutoChildRegisterMethod.ONLY_REGISTER_NON_ANIDATED_OPTIONS;
		
		public IEnumerable<IOption> SelectedOptions => Options?.Where(o => o.IsOn);


		private void Awake() => tryAutoRegisterChildren(invokedOnAwake: true);

		private void Start()
		{
			tryAutoRegisterChildren(invokedOnStart: true);
			AssertConsistency();
		}


		public void AssertConsistency()
		{
			if (!Options.Any()) return;

			if (!SelectedOptions.Any())
			{
				if (!AllowAllOptionsOff)
				{
					var firstOpt = Options.First();
					firstOpt.Select(true);
				}
				return;
			}

			if (SelectedOptions.Count() > 1)
			{
				if (!AllowMultiOptionSelection)
				{
					foreach (var opt in SelectedOptions.Skip(1))
						opt.Select(false);
				}
				return;
			}
		}


		public void AssertConsistency(IOption updatedOption, bool newSelectedValue)
		{
			if (!Options.Any() || !OptionIsRegistered(updatedOption)) return;

			if (!newSelectedValue && AllowAllOptionsOff) return;

			if (newSelectedValue && !AllowMultiOptionSelection && SelectedOptions.Count() > 1)
			{
				var oldOption = SelectedOptions.First(option => updatedOption != option);
				oldOption.Select(false);
			}
		}


		public bool OptionCanBeSetToSelectedValue(IOption option, bool attemptedSelectedValue)
		{
			if (!OptionIsRegistered(option))
				return true;    // the option passed is not registered in this toggle group, so why not?

			if (!attemptedSelectedValue && SelectedOptions.Count() == 1 && !AllowAllOptionsOff)
				return false;

			return true;
		}


		public bool OptionIsRegistered(IOption option)
			=> Options.Contains(option) && option.Group == this;


		public void RegisterChildOptions()
		{
			IEnumerable<IOption> options = AutoRegisterMethod switch
			{
				AutoChildRegisterMethod.ONLY_REGISTER_OPTIONS_THAT_ARE_DIRECT_CHILDREN =>
					options = directChildren(transform)
						.Where(t => t.TryGetComponent(out IOption opt))
						.Select(t => t.GetComponent<IOption>()),

				AutoChildRegisterMethod.ONLY_REGISTER_NON_ANIDATED_OPTIONS =>
					options = getNonAnidatedOptions(),

				_ =>
					options = transform.GetComponentsInChildren<IOption>()
			};

			RegisterOptions(options);
		}


		public void RegisterChildOptions<TOption>() where TOption : MonoBehaviour, IOption
			=> RegisterOptions(transform.GetComponentsInChildren<TOption>());


		public void RegisterOption(IOption option)
		{
			if (!OptionIsRegistered(option))
			{
				(Options as List<IOption>).Add(option);
				option.Group = this;
			}
		}


		public void RegisterOptions(IEnumerable<IOption> options)
		{
			var childrenToAdd = options.Where(t => !OptionIsRegistered(t)).ToList();
			(Options as List<IOption>).AddRange(childrenToAdd);
			childrenToAdd.ForEach(o => o.Group = this);
		}


		public void UnregisterAllOptions()
		{
			var options = Options.ToList();
			options.RemoveAll(o => o == null);
			options.ForEach(o => o.Group = null);
			options.ToList().Clear();
		}



		void tryAutoRegisterChildren(bool invokedOnAwake = false, bool invokedOnStart = false)
		{
			bool shouldAutoRegister = AutoRegisterMoment != AutoChildRegisterMoment.DONT_AUTO_REGISTER;
			if (!shouldAutoRegister) return;

			bool registerNow = (invokedOnAwake && AutoRegisterMoment == AutoChildRegisterMoment.ON_AWAKE) ||
							   (invokedOnStart && AutoRegisterMoment == AutoChildRegisterMoment.ON_START);

			if (registerNow)
				RegisterChildOptions();
		}


		// BFS looking for IOptions
		IEnumerable<IOption> getNonAnidatedOptions()
		{
			List<IOption> foundOptions = new List<IOption>();
			var stack = new Stack<Transform>(directChildren(transform));
			
			while (stack.Count > 0)
			{
				var child = stack.Pop();

				if (child.TryGetComponent(out IOption option))
				{
					foundOptions.Add(option);
				}
				else if(!child.TryGetComponent(out OptionsGroup group))
				{
					foreach (var t in directChildren(child))
						stack.Push(t);
				}
			}

			return foundOptions;
		}


		List<Transform> directChildren(Transform targetTransform)
			=> Enumerable.Range(0, targetTransform.childCount).Select(i => targetTransform.GetChild(i)).ToList();
	}
}