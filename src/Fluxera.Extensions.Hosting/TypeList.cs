namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	///     A shortcut for <see cref="TypeList{TBaseType}" /> to use object as base type.
	/// </summary>
	[PublicAPI]
	public class TypeList : TypeList<object>, ITypeList
	{
	}

	/// <summary>
	///     Extends <see cref="List{T}" /> to add restriction a specific base type.
	/// </summary>
	/// <typeparam name="TBaseType">Base Type of <see cref="Type" />s in this list</typeparam>
	[PublicAPI]
	public class TypeList<TBaseType> : ITypeList<TBaseType>
	{
		private readonly List<Type> typeList;

		/// <summary>
		///     Creates a new <see cref="TypeList{T}" /> object.
		/// </summary>
		public TypeList()
		{
			this.typeList = new List<Type>();
		}

		/// <summary>
		///     Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count => this.typeList.Count;

		/// <summary>
		///     Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly => false;

		/// <summary>
		///     Gets or sets the <see cref="Type" /> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public Type this[int index]
		{
			get => this.typeList[index];
			set
			{
				CheckType(value);
				this.typeList[index] = value;
			}
		}

		/// <inheritdoc />
		public void Add<T>() where T : TBaseType
		{
			this.typeList.Add(typeof(T));
		}

		public void TryAdd<T>() where T : TBaseType
		{
			if(this.Contains<T>())
			{
				return;
			}

			this.Add<T>();
		}

		/// <inheritdoc />
		public void Add(Type item)
		{
			CheckType(item);
			this.typeList.Add(item);
		}

		/// <inheritdoc />
		public void Insert(int index, Type item)
		{
			CheckType(item);
			this.typeList.Insert(index, item);
		}

		/// <inheritdoc />
		public int IndexOf(Type item)
		{
			return this.typeList.IndexOf(item);
		}

		/// <inheritdoc />
		public bool Contains<T>() where T : TBaseType
		{
			return this.Contains(typeof(T));
		}

		/// <inheritdoc />
		public bool Contains(Type item)
		{
			return this.typeList.Contains(item);
		}

		/// <inheritdoc />
		public void Remove<T>() where T : TBaseType
		{
			this.typeList.Remove(typeof(T));
		}

		/// <inheritdoc />
		public bool Remove(Type item)
		{
			return this.typeList.Remove(item);
		}

		/// <inheritdoc />
		public void RemoveAt(int index)
		{
			this.typeList.RemoveAt(index);
		}

		/// <inheritdoc />
		public void Clear()
		{
			this.typeList.Clear();
		}

		/// <inheritdoc />
		public void CopyTo(Type[] array, int arrayIndex)
		{
			this.typeList.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public IEnumerator<Type> GetEnumerator()
		{
			return this.typeList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.typeList.GetEnumerator();
		}

		private static void CheckType(Type item)
		{
			if(!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
			{
				throw new ArgumentException(
					$"Given type ({item.AssemblyQualifiedName}) should be instance of {typeof(TBaseType).AssemblyQualifiedName} ",
					nameof(item));
			}
		}
	}
}
