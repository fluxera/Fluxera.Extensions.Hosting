namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;

	public class LifecycleRegister : ILifecycleRegister
	{
		private readonly HashSet<Action> callbacks = new HashSet<Action>();

		public void Register(Action callback)
		{
			this.callbacks.Add(callback);
		}

		public void Notify()
		{
			foreach(Action? callback in this.callbacks)
			{
				callback.Invoke();
			}
		}
	}
}
