using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6C RID: 3436
	public class Triangulator
	{
		// Token: 0x06005775 RID: 22389 RVA: 0x001C3038 File Offset: 0x001C1238
		public Triangulator(List<Vector3> points)
		{
			this.m_points = points;
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001C3054 File Offset: 0x001C1254
		public List<int> Triangulate()
		{
			this.indices.Clear();
			int count = this.m_points.Count;
			if (count < 3)
			{
				return this.indices;
			}
			int[] array = new int[count];
			if (this.Area() > 0f)
			{
				for (int i = 0; i < count; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					array[j] = count - 1 - j;
				}
			}
			int k = count;
			int num = 2 * k;
			int num2 = k - 1;
			while (k > 2)
			{
				if (num-- <= 0)
				{
					return this.indices;
				}
				int num3 = num2;
				if (k <= num3)
				{
					num3 = 0;
				}
				num2 = num3 + 1;
				if (k <= num2)
				{
					num2 = 0;
				}
				int num4 = num2 + 1;
				if (k <= num4)
				{
					num4 = 0;
				}
				if (this.Snip(num3, num2, num4, k, array))
				{
					int item = array[num3];
					int item2 = array[num2];
					int item3 = array[num4];
					this.indices.Add(item);
					this.indices.Add(item2);
					this.indices.Add(item3);
					int num5 = num2;
					for (int l = num2 + 1; l < k; l++)
					{
						array[num5] = array[l];
						num5++;
					}
					k--;
					num = 2 * k;
				}
			}
			this.indices.Reverse();
			return this.indices;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001C31A0 File Offset: 0x001C13A0
		private float Area()
		{
			int count = this.m_points.Count;
			float num = 0f;
			int index = count - 1;
			int i = 0;
			while (i < count)
			{
				Vector3 vector = this.m_points[index];
				Vector3 vector2 = this.m_points[i];
				num += vector.x * vector2.z - vector2.x * vector.z;
				index = i++;
			}
			return num * 0.5f;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001C3218 File Offset: 0x001C1418
		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector3 vector = this.m_points[V[u]];
			Vector3 vector2 = this.m_points[V[v]];
			Vector3 vector3 = this.m_points[V[w]];
			if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.z - vector.z) - (vector2.z - vector.z) * (vector3.x - vector.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector3 p = this.m_points[V[i]];
					if (Triangulator.InsideTriangle(vector, vector2, vector3, p))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001C32D0 File Offset: 0x001C14D0
		private static bool InsideTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
		{
			float num = C.x - B.x;
			float num2 = C.z - B.z;
			float num3 = A.x - C.x;
			float num4 = A.z - C.z;
			float num5 = B.x - A.x;
			float num6 = B.z - A.z;
			float num7 = P.x - A.x;
			float num8 = P.z - A.z;
			float num9 = P.x - B.x;
			float num10 = P.z - B.z;
			float num11 = P.x - C.x;
			float num12 = P.z - C.z;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}

		// Token: 0x04003FC3 RID: 16323
		private readonly List<Vector3> m_points;

		// Token: 0x04003FC4 RID: 16324
		private readonly List<int> indices = new List<int>();
	}
}
