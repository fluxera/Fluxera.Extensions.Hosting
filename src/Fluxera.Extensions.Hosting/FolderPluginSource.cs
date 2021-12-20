namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Utilities.Extensions;

	internal sealed class FolderPluginSource : IPluginSource
	{
		private readonly string folder;
		private readonly Lazy<IEnumerable<Assembly>> moduleAssemblies;
		private readonly SearchOption searchOption;

		public FolderPluginSource(string folder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			Guard.Against.NullOrWhiteSpace(folder, nameof(folder));

			this.folder = folder;
			this.searchOption = searchOption;

			this.moduleAssemblies = new Lazy<IEnumerable<Assembly>>(this.LoadAssemblies, true);
		}

		public IEnumerable<Assembly> GetAssemblies()
		{
			return this.moduleAssemblies.Value;
		}

		public IEnumerable<Type> GetModules()
		{
			IList<Type> modules = new List<Type>();

			foreach(Assembly assembly in this.GetAssemblies())
			{
				try
				{
					foreach(Type type in assembly.GetTypes())
					{
						if(ModuleHelper.IsModule(type))
						{
							modules.AddIfNotContains(type);
						}
					}
				}
				catch(Exception ex)
				{
					throw new InvalidOperationException(
						$"Could not get module types from assembly: {assembly.FullName}", ex);
				}
			}

			return modules;
		}

		private IEnumerable<Assembly> LoadAssemblies()
		{
			return GetAllAssembliesInFolder(this.folder, this.searchOption);
		}

		private static IEnumerable<Assembly> GetAllAssembliesInFolder(string folderPath, SearchOption searchOption)
		{
			return Directory
				.EnumerateFiles(folderPath, "*.dll", searchOption)
				.Select(Assembly.LoadFile);
		}
	}
}
