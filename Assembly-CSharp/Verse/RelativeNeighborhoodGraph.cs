using System;
using System.Collections.Generic;
using DelaunatorSharp;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000367 RID: 871
	public class RelativeNeighborhoodGraph
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x0006D6E8 File Offset: 0x0006B8E8
		public RelativeNeighborhoodGraph(Delaunator delaunator)
		{
			foreach (Vector2 key in delaunator.Points)
			{
				this.connections[key] = new List<Vector2>();
			}
			foreach (Vector2 vector in delaunator.Points)
			{
				foreach (Vector2 vector2 in delaunator.Points)
				{
					Edge edge;
					if (!this.connections[vector].Contains(vector2) && RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector, vector2, out edge))
					{
						float weight = RelativeNeighborhoodGraph.GetWeight(edge);
						bool flag = false;
						foreach (Vector2 vector3 in delaunator.Points)
						{
							Edge edge2;
							Edge edge3;
							if (RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector, vector3, out edge2) && RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector3, vector2, out edge3))
							{
								float weight2 = RelativeNeighborhoodGraph.GetWeight(edge2);
								float weight3 = RelativeNeighborhoodGraph.GetWeight(edge3);
								if (Mathf.Max(weight2, weight3) < weight)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							this.connections[vector].Add(vector2);
							this.connections[vector2].Add(vector);
						}
					}
				}
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0006D844 File Offset: 0x0006BA44
		private static float GetWeight(Edge edge)
		{
			return (edge.Q - edge.P).magnitude;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0006D86C File Offset: 0x0006BA6C
		private static bool TryGetEdge(Delaunator delaunator, Vector2 a, Vector2 b, out Edge edge)
		{
			foreach (IEdge edge2 in delaunator.GetEdges())
			{
				if ((edge2.P == a || edge2.Q == a) && (edge2.P == b || edge2.Q == b))
				{
					edge = (Edge)edge2;
					return true;
				}
			}
			edge = default(Edge);
			return false;
		}

		// Token: 0x0400147A RID: 5242
		public readonly Dictionary<Vector2, List<Vector2>> connections = new Dictionary<Vector2, List<Vector2>>();
	}
}
