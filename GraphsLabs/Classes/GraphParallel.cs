using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsLabs
{
	internal partial class Graph
	{
		private IEnumerable<int> ChildrenParallel()
		{
			List<int> children = new List<int>();
			Parallel.For(0, AdjMatrix.Dimension, child =>
			{
				switch (moving)
				{
					case Moving.Direct:
						if (AdjMatrix[vertex, child] == 1)
							children.Add(child);
						break;
					case Moving.Reverse:
						if (AdjMatrix[child, vertex] == 1)
							children.Add(child);
						break;
				}
			});
			return children;
		}

		private bool[] BreadthFirstSearchParallel()
		{
			bool[] visited = new bool[AdjMatrix.Dimension];     // изначально список посещённых узлов пуст
			Queue<int> queue = new Queue<int>();
			queue.Enqueue(vertex);                         // начиная с узла-источника
			visited[vertex] = true;
			while (queue.Count != 0)
			{
				vertex = queue.Dequeue();                  // извлечь первый элемент в очереди
				Parallel.ForEach(ChildrenParallel(), child => // все преемники текущего узла, ...
				{
					if (visited[child] == false)                // ... которые ещё не были посещены ...
					{
						queue.Enqueue(child);                   // ... добавить в конец очереди...
						visited[child] = true;                  // ... и пометить как посещённые
					}
				});
			}
			return visited;
		}

		private List<int> TransitiveClosureParallel()
		{
			List<int> directTransitiveClosure = new List<int>();
			int index = 0;
			Parallel.ForEach(BreadthFirstSearchParallel(), item =>
			{
				if (item) directTransitiveClosure.Add(index);
				index++;
			});
			return directTransitiveClosure;
		}

		public string GetTransitiveClosureParallel(int vertex, Moving moving)
		{
			this.vertex = vertex;
			this.moving = moving;
			string trClosure = string.Empty;
			foreach (int item in TransitiveClosureParallel())
				trClosure += $" x{item + 1},";
			string sign = string.Empty;
			switch (moving)
			{
				case Moving.Direct:
					sign = "⁺";
					break;
				case Moving.Reverse:
					sign = "⁻";
					break;
			}
			return $"T{sign}(x{vertex + 1}) = {{{trClosure.Substring(0, trClosure.Length - 1)} }}";
		}
	}
}
