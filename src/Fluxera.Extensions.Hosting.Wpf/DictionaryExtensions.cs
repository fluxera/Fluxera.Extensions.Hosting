namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;

	internal static class DictionaryExtensions
	{
		/// <summary>
		///     Helper method to retrieve a context from the host builder properties.
		/// </summary>
		public static bool TryRetrieveContext<TContext>(this IDictionary<object, object> properties,
			string contextKey, out TContext context)
			where TContext : class, new()
		{
			if(properties.TryGetValue(contextKey, out object? value))
			{
				context = (TContext)value;
				return true;
			}

			context = new TContext();
			properties[contextKey] = context;
			return false;
		}
	}
}
