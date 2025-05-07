using OhMyToggles.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OhMyToggles
{
	public class OptionsGroup : MonoBehaviour
	{
		public IEnumerable<IOption> Options { get; private set; } = new List<IOption>();

		[Header("Config:")]
		public bool AllowMultiOptionSelection = false;
		public bool AllowAllOptionsOff = false;
		
		public bool RegisterAllOptionChildrenOnAwake = true;

		public IEnumerable<IOption> SelectedOptions => Options?.Where(o => o.IsOn);


		private void Awake()
		{
			if (RegisterAllOptionChildrenOnAwake)
				RegisterAllChildOptions();
		}

		private void Start() => AssertConsistency();


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


		public void RegisterAllChildOptions()
			=> RegisterOptions(transform.GetComponentsInChildren<IOption>());


		public void RegisterAllChildOptions<TToggle>() where TToggle : MonoBehaviour, IOption
			=> RegisterOptions(transform.GetComponentsInChildren<TToggle>());


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
	}
}