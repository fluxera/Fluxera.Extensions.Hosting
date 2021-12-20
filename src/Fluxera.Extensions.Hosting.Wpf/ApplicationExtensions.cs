namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Windows;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class ApplicationExtensions
	{
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
