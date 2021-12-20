namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ModuleLifecycleOptions
	{
		public ModuleLifecycleOptions()
		{
			this.Contributors = new TypeList<IModuleLifecycleContributor>();
		}

		public ITypeList<IModuleLifecycleContributor> Contributors { get; }
	}
}