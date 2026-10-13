using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB3 RID: 3251
	public class DirectedAcyclicGraph
	{
		// Token: 0x06005348 RID: 21320 RVA: 0x001AD490 File Offset: 0x001AB690
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x001AD4D1 File Offset: 0x001AB6D1
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x001AD4E8 File Offset: 0x001AB6E8
		public List<int> TopologicalSort()
		{
			bool[] array = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
			}
			List<int> result = new List<int>();
			for (int j = 0; j < this.numVertices; j++)
			{
				if (!array[j])
				{
					this.TopologicalSortInner(j, array, result);
				}
			}
			return result;
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x001AD53C File Offset: 0x001AB73C
		private void TopologicalSortInner(int v, bool[] visited, List<int> result)
		{
			visited[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num])
				{
					this.TopologicalSortInner(num, visited, result);
				}
			}
			result.Add(v);
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x001AD5A8 File Offset: 0x001AB7A8
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001AD5B8 File Offset: 0x001AB7B8
		public int FindCycle()
		{
			bool[] array = new bool[this.numVertices];
			bool[] array2 = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
				array2[i] = false;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				if (this.IsCyclicInner(j, array, array2))
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001AD618 File Offset: 0x001AB818
		private bool IsCyclicInner(int v, bool[] visited, bool[] history)
		{
			visited[v] = true;
			history[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num] && this.IsCyclicInner(num, visited, history))
				{
					return true;
				}
				if (history[num])
				{
					return true;
				}
			}
			history[v] = false;
			return false;
		}

		// Token: 0x04003C22 RID: 15394
		private int numVertices;

		// Token: 0x04003C23 RID: 15395
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
