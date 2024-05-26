namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;

	internal static class CollectionExtensions
	{
		/// <summary>
		///     Adds an item to the collection if it's not already in the collection.
		/// </summary>
		/// <param name="collection">The collection</param>
		/// <param name="item">Item to check and add</param>
		/// <typeparam name="T">Type of the items in the collection</typeparam>
		/// <returns>Returns True if added, returns False if not.</returns>
		public static bool AddIfNotContains<T>(this ICollection<T> collection, T item)
		{
			Guard.ThrowIfNull(collection);

			if(collection.Contains(item))
			{
				return false;
			}

			collection.Add(item);
			return true;
		}
	}
}
