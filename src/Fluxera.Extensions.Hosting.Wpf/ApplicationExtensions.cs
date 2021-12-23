namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Windows;
	using JetBrains.Annotations;

	/// <summary>
	///     Extensions methods on the <see cref="Application" /> type.
	/// </summary>
	[PublicAPI]
	public static class ApplicationExtensions
	{
		/// <summary>
		///     Adds a <see cref="ResourceDictionary" /> for the given <see cref="Uri" /> resource.
		/// </summary>
		/// <param name="application"></param>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static Application AddResourceDictionary(this Application application, Uri resource)
		{
			ResourceDictionary resourceDictionary = new ResourceDictionary
			{
				Source = resource
			};
			application.Resources.MergedDictionaries.Add(resourceDictionary);

			return application;
		}
	}
}
