namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region interface IOneToManyCollectionSimpleItem<TContainer, TItem>

	/// <summary>Item adapter for simple relation.</summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public interface IOneToManyCollectionSimpleItem<TContainer, TItem>
	{
		/// <summary>Gets or sets the containing object.</summary>
		/// <value>The containing object.</value>
		TContainer Container { get; set; }
	}

	#endregion
}
