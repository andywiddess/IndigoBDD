using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Class that performs topological sort.
	/// </summary>
	/// <typeparam name="T">Type of objects to be sorted.</typeparam>
	public class TopologicalSort<T>
	{
		#region interface IVisitedHandler

		/// <summary>
		/// Determines if object has been visited or not. Simple implementation can
		/// be based on set, for large amount of object it's better to implement
		/// a flag in visited object, for performance reasons.
		/// </summary>
		public interface IVisitedHandler
		{
			/// <summary>
			/// Checks if all objects are in non-visited set. It can return <c>null</c> if not known.
			/// </summary>
			/// <value><c>true</c> is no objects visted, <c>false</c> otherwise. <c>null</c> if not known.</value>
			bool? NoneVisited { get; }

			/// <summary>
			/// Checks if all objects are visited. It can return <c>null</c> if not known.
			/// </summary>
			/// <value><c>true</c> is all objects visted, <c>false</c> otherwise. <c>null</c> if not known.</value>
			bool? AllVisited { get; }

			/// <summary>
			/// Determines whether the specified node has been visited.
			/// </summary>
			/// <param name="node">The subject.</param>
			/// <returns><c>true</c> if the specified subject is visited; otherwise, <c>false</c>.</returns>
			bool IsVisited(T node);


			/// <summary>
			/// Visits the specified node.
			/// </summary>
			/// <param name="node">The node.</param>
			/// <param name="value">set or reset visited flag.</param>
			void Visit(T node, bool value);
		}

		#endregion

		#region interface IDependsOnHandler

		/// <summary>
		/// Provides information about dependant objects.
		/// </summary>
		public interface IDependsOnHandler
		{
			/// <summary>
			/// Lists objects required by given one.
			/// </summary>
			/// <param name="subject">The object.</param>
			/// <returns>Returns list of objects required by given one.</returns>
			IEnumerable<T> ListRequiredBy(T subject);
		}

		private class DependsOnDelegateHandler:IDependsOnHandler
		{
			private readonly Converter<T, IEnumerable<T>> m_Delegate;

			public DependsOnDelegateHandler(Converter<T, IEnumerable<T>> handler)
			{
				if (handler == null)
					throw new ArgumentNullException("handler", "handler is null.");
				m_Delegate = handler;
			}

			public IEnumerable<T> ListRequiredBy(T subject)
			{
				return m_Delegate(subject);
			}
		}

		#endregion

		#region class DefaultVisitedHandler

		/// <summary>
		/// Default implementation of <see cref="IVisitedHandler"/>. Implemented using Set.
		/// </summary>
		internal class DefaultVisitedHandler:IVisitedHandler
		{
			readonly HashSet<T> m_visitedSet = new HashSet<T>();

			#region IVisitedHandler Members

			public bool? NoneVisited
			{
				get { return m_visitedSet.Count == 0; }
			}

			public bool? AllVisited
			{
				get { return null; }
			}

			public bool IsVisited(T subject)
			{
				return m_visitedSet.Contains(subject);
			}

			public void Visit(T subject, bool value)
			{
				if (value)
				{
					m_visitedSet.Add(subject);
				}
				else
				{
					m_visitedSet.Remove(subject);
				}
			}

			#endregion
		}

		#endregion

		#region sort

		/// <summary>
		/// Sorts given nodes. Uses default <see cref="IVisitedHandler"/> handler and
		/// a Converter&lt;T, IEnumerable%lt;T&gt;&gt; as a dependency handler.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <param name="listDependencies">The list dependencies.</param>
		/// <returns>Sorted nodes.</returns>
		public static IEnumerable<T> Sort(
				IEnumerable<T> nodes,
				Converter<T, IEnumerable<T>> listDependencies)
		{
			return Sort(nodes, new DependsOnDelegateHandler(listDependencies));
		}

		/// <summary>
		/// Sorts given nodes. Uses default <see cref="IVisitedHandler"/> handler.
		/// </summary>
		/// <param name="nodes">Nodes collection.</param>
		/// <param name="dependencyHandler">The <see cref="IDependsOnHandler"/> handler.</param>
		/// <returns>Sorted nodes.</returns>
		public static IEnumerable<T> Sort(
				IEnumerable<T> nodes,
				IDependsOnHandler dependencyHandler)
		{
			return Sort(nodes, new DefaultVisitedHandler(), dependencyHandler);
		}

		/// <summary>
		/// Sorts given nodes.
		/// </summary>
		/// <param name="nodes">Nodes collection.</param>
		/// <param name="visitedHandler">The <see cref="IVisitedHandler"/> handler.</param>
		/// <param name="dependencyHandler">The <see cref="IDependsOnHandler"/> handler.</param>
		/// <returns>Sorted nodes.</returns>
		public static IEnumerable<T> Sort(
				IEnumerable<T> nodes,
				IVisitedHandler visitedHandler,
				IDependsOnHandler dependencyHandler)
		{
			bool? noneVisited = visitedHandler.NoneVisited;
			if (noneVisited.HasValue && noneVisited.Value)
			{
				// handler is sure - no nodes has been visited
				// so we can skip initialization
			}
			else
			{
				foreach (T node in nodes)
				{
					visitedHandler.Visit(node, false);
				}
			}

			// visit every node
			foreach (T node in nodes)
			{
				foreach (T result in Visit(node, visitedHandler, dependencyHandler))
				{
					yield return result;
				}
			}
		}

		/// <summary>
		/// Visits the specified node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="visitedHandler">The <see cref="IVisitedHandler"/> handler.</param>
		/// <param name="dependancyHandler">The <see cref="IDependsOnHandler"/> handler.</param>
		/// <returns>Sorted subset of nodes.</returns>
		protected static IEnumerable<T> Visit(T node, IVisitedHandler visitedHandler, IDependsOnHandler dependancyHandler)
		{
			// If we're visiting some place we've already been, then we need go no further
			if (visitedHandler.IsVisited(node))
				yield break;

			// We're about to explore this entire dependency, so mark it as dead right away...
			visitedHandler.Visit(node, true);

			// ...and then recursively explore all this dependency's children, looking for the bottom. 
			foreach (T child in dependancyHandler.ListRequiredBy(node))
			{
				foreach (T result in Visit(child, visitedHandler, dependancyHandler))
				{
					yield return result;
				}
			}

			// As we return back up the stack, we know that all of this dependency's children are taken care of.  
			// Therefore, this is a ?bottom?.  Therefore, put it on the list. 
			yield return node;
		}

		#endregion
	}
}
