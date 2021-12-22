namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;

	internal sealed class LifecycleRegister : ILifecycleRegister
	{
		private readonly HashSet<Action> actions = new HashSet<Action>();

		public void Register(Action callback)
		{
			this.actions.Add(callback);
		}

		public void Notify()
		{
			foreach(Action? action in this.actions)
			{
				action.Invoke();
			}
		}
	}
}
