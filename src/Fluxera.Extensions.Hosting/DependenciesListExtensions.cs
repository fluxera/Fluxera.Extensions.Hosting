namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Utilities.Extensions;

	internal static class DependenciesListExtensions
	{
		public static void MoveItem<T>(this IList<T> source, Predicate<T> selector, int targetIndex)
		{
			if(!targetIndex.IsBetween(0, source.Count - 1))
			{
				throw new IndexOutOfRangeException($"targetIndex should be between 0 and {source.Count - 1}.");
			}

			int currentIndex = source.FindIndex(selector);
			if(currentIndex == targetIndex)
			{
				return;
			}

			T item = source[currentIndex];
			source.RemoveAt(currentIndex);
			source.Insert(targetIndex, item);
		}

		public static int FindIndex<T>(this IList<T> source, Predicate<T> selector)
		{
			for(int i = 0; i < source.Count; ++i)
			{
				if(selector(source[i]))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		///     Sort a list by a topological sorting, which consider their dependencies.
		/// </summary>
		/// <typeparam name="T">The type of the members of values.</typeparam>
		/// <param name="source">A list of objects to sort</param>
		/// <param name="getDependencies">Function to resolve the dependencies</param>
		/// <returns></returns>
		public static IList<T> SortByDependencies<T>(this IEnumerable<T> source,
			Func<T, IEnumerable<T>> getDependencies)
		{
			/*
			 * See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
			 *      http://en.wikipedia.org/wiki/Topological_sorting
			 */

			IList<T> sorted = new List<T>();
			IDictionary<T, bool> visited = new Dictionary<T, bool>();

			foreach(T item in source)
			{
				SortByDependenciesVisit(item, getDependencies, sorted, visited);
			}

			return sorted;
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="T">The type of the members of values.</typeparam>
		/// <param name="item">Item to resolve</param>
		/// <param name="getDependencies">Function to resolve the dependencies</param>
		/// <param name="sorted">List with the sorted items</param>
		/// <param name="visited">Dictionary with the visited items</param>
		private static void SortByDependenciesVisit<T>(T item, Func<T, IEnumerable<T>> getDependencies, IList<T> sorted,
			IDictionary<T, bool> visited)
		{
			bool alreadyVisited = visited.TryGetValue(item, out bool inProcess);

			if(alreadyVisited)
			{
				if(inProcess)
				{
					throw new ArgumentException($"Cyclic dependency found for item: {item}.");
				}
			}
			else
			{
				visited[item] = true;

				IEnumerable<T> dependencies = getDependencies(item);
				if(dependencies != null)
				{
					foreach(T dependency in dependencies)
					{
						SortByDependenciesVisit(dependency, getDependencies, sorted,
							visited);
					}
				}

				visited[item] = false;
				sorted.Add(item);
			}
		}
	}
}
