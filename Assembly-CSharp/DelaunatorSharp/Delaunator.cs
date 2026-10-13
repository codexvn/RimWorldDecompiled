using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ECC RID: 16076
	public class Delaunator
	{
		// Token: 0x1700374D RID: 14157
		// (get) Token: 0x06016106 RID: 90374 RVA: 0x0069A965 File Offset: 0x00698B65
		// (set) Token: 0x06016107 RID: 90375 RVA: 0x0069A96D File Offset: 0x00698B6D
		public int[] Triangles { get; private set; }

		// Token: 0x1700374E RID: 14158
		// (get) Token: 0x06016108 RID: 90376 RVA: 0x0069A976 File Offset: 0x00698B76
		// (set) Token: 0x06016109 RID: 90377 RVA: 0x0069A97E File Offset: 0x00698B7E
		public int[] Halfedges { get; private set; }

		// Token: 0x1700374F RID: 14159
		// (get) Token: 0x0601610A RID: 90378 RVA: 0x0069A987 File Offset: 0x00698B87
		// (set) Token: 0x0601610B RID: 90379 RVA: 0x0069A98F File Offset: 0x00698B8F
		public Vector2[] Points { get; private set; }

		// Token: 0x17003750 RID: 14160
		// (get) Token: 0x0601610C RID: 90380 RVA: 0x0069A998 File Offset: 0x00698B98
		// (set) Token: 0x0601610D RID: 90381 RVA: 0x0069A9A0 File Offset: 0x00698BA0
		public int[] Hull { get; private set; }

		// Token: 0x0601610E RID: 90382 RVA: 0x0069A9AC File Offset: 0x00698BAC
		public Delaunator(Vector2[] points)
		{
			if (points.Length < 3)
			{
				throw new ArgumentOutOfRangeException("Need at least 3 points");
			}
			this.Points = points;
			this.coords = new double[this.Points.Length * 2];
			for (int i = 0; i < this.Points.Length; i++)
			{
				Vector2 vector = this.Points[i];
				this.coords[2 * i] = (double)vector.x;
				this.coords[2 * i + 1] = (double)vector.y;
			}
			int num = points.Length;
			int num2 = 2 * num - 5;
			this.Triangles = new int[num2 * 3];
			this.Halfedges = new int[num2 * 3];
			this.hashSize = (int)Math.Ceiling(Math.Sqrt((double)num));
			this.hullPrev = new int[num];
			this.hullNext = new int[num];
			this.hullTri = new int[num];
			this.hullHash = new int[this.hashSize];
			int[] array = new int[num];
			double num3 = double.PositiveInfinity;
			double num4 = double.PositiveInfinity;
			double num5 = double.NegativeInfinity;
			double num6 = double.NegativeInfinity;
			for (int j = 0; j < num; j++)
			{
				double num7 = this.coords[2 * j];
				double num8 = this.coords[2 * j + 1];
				if (num7 < num3)
				{
					num3 = num7;
				}
				if (num8 < num4)
				{
					num4 = num8;
				}
				if (num7 > num5)
				{
					num5 = num7;
				}
				if (num8 > num6)
				{
					num6 = num8;
				}
				array[j] = j;
			}
			double ax = (num3 + num5) / 2.0;
			double ay = (num4 + num6) / 2.0;
			double num9 = double.PositiveInfinity;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			for (int k = 0; k < num; k++)
			{
				double num13 = Delaunator.Dist(ax, ay, this.coords[2 * k], this.coords[2 * k + 1]);
				if (num13 < num9)
				{
					num10 = k;
					num9 = num13;
				}
			}
			double num14 = this.coords[2 * num10];
			double num15 = this.coords[2 * num10 + 1];
			num9 = double.PositiveInfinity;
			for (int l = 0; l < num; l++)
			{
				if (l != num10)
				{
					double num16 = Delaunator.Dist(num14, num15, this.coords[2 * l], this.coords[2 * l + 1]);
					if (num16 < num9 && num16 > 0.0)
					{
						num11 = l;
						num9 = num16;
					}
				}
			}
			double num17 = this.coords[2 * num11];
			double num18 = this.coords[2 * num11 + 1];
			double num19 = double.PositiveInfinity;
			for (int m = 0; m < num; m++)
			{
				if (m != num10 && m != num11)
				{
					double num20 = Delaunator.Circumradius(num14, num15, num17, num18, this.coords[2 * m], this.coords[2 * m + 1]);
					if (num20 < num19)
					{
						num12 = m;
						num19 = num20;
					}
				}
			}
			double num21 = this.coords[2 * num12];
			double num22 = this.coords[2 * num12 + 1];
			if (num19 == double.PositiveInfinity)
			{
				throw new Exception("No Delaunay triangulation exists for this input.");
			}
			if (Delaunator.Orient(num14, num15, num17, num18, num21, num22))
			{
				int num23 = num11;
				double num24 = num17;
				double num25 = num18;
				num11 = num12;
				num17 = num21;
				num18 = num22;
				num12 = num23;
				num21 = num24;
				num22 = num25;
			}
			Vector2 vector2 = Delaunator.Circumcenter(num14, num15, num17, num18, num21, num22);
			this.cx = (double)vector2.x;
			this.cy = (double)vector2.y;
			double[] array2 = new double[num];
			for (int n = 0; n < num; n++)
			{
				array2[n] = Delaunator.Dist(this.coords[2 * n], this.coords[2 * n + 1], (double)vector2.x, (double)vector2.y);
			}
			Delaunator.Quicksort(array, array2, 0, num - 1);
			this.hullStart = num10;
			this.hullSize = 3;
			this.hullNext[num10] = (this.hullPrev[num12] = num11);
			this.hullNext[num11] = (this.hullPrev[num10] = num12);
			this.hullNext[num12] = (this.hullPrev[num11] = num10);
			this.hullTri[num10] = 0;
			this.hullTri[num11] = 1;
			this.hullTri[num12] = 2;
			this.hullHash[this.HashKey(num14, num15)] = num10;
			this.hullHash[this.HashKey(num17, num18)] = num11;
			this.hullHash[this.HashKey(num21, num22)] = num12;
			this.trianglesLen = 0;
			this.AddTriangle(num10, num11, num12, -1, -1, -1);
			double num26 = 0.0;
			double num27 = 0.0;
			for (int num28 = 0; num28 < array.Length; num28++)
			{
				int num29 = array[num28];
				double num30 = this.coords[2 * num29];
				double num31 = this.coords[2 * num29 + 1];
				if (num28 <= 0 || Math.Abs(num30 - num26) > this.EPSILON || Math.Abs(num31 - num27) > this.EPSILON)
				{
					num26 = num30;
					num27 = num31;
					if (num29 != num10 && num29 != num11 && num29 != num12)
					{
						int num32 = 0;
						for (int num33 = 0; num33 < this.hashSize; num33++)
						{
							int num34 = this.HashKey(num30, num31);
							num32 = this.hullHash[(num34 + num33) % this.hashSize];
							if (num32 != -1 && num32 != this.hullNext[num32])
							{
								break;
							}
						}
						num32 = this.hullPrev[num32];
						int num35 = num32;
						int num36 = this.hullNext[num35];
						while (!Delaunator.Orient(num30, num31, this.coords[2 * num35], this.coords[2 * num35 + 1], this.coords[2 * num36], this.coords[2 * num36 + 1]))
						{
							num35 = num36;
							if (num35 == num32)
							{
								num35 = int.MaxValue;
								break;
							}
							num36 = this.hullNext[num35];
						}
						if (num35 != 2147483647)
						{
							int num37 = this.AddTriangle(num35, num29, this.hullNext[num35], -1, -1, this.hullTri[num35]);
							this.hullTri[num29] = this.Legalize(num37 + 2);
							this.hullTri[num35] = num37;
							this.hullSize++;
							int num38 = this.hullNext[num35];
							num36 = this.hullNext[num38];
							while (Delaunator.Orient(num30, num31, this.coords[2 * num38], this.coords[2 * num38 + 1], this.coords[2 * num36], this.coords[2 * num36 + 1]))
							{
								num37 = this.AddTriangle(num38, num29, num36, this.hullTri[num29], -1, this.hullTri[num38]);
								this.hullTri[num29] = this.Legalize(num37 + 2);
								this.hullNext[num38] = num38;
								this.hullSize--;
								num38 = num36;
								num36 = this.hullNext[num38];
							}
							if (num35 == num32)
							{
								num36 = this.hullPrev[num35];
								while (Delaunator.Orient(num30, num31, this.coords[2 * num36], this.coords[2 * num36 + 1], this.coords[2 * num35], this.coords[2 * num35 + 1]))
								{
									num37 = this.AddTriangle(num36, num29, num35, -1, this.hullTri[num35], this.hullTri[num36]);
									this.Legalize(num37 + 2);
									this.hullTri[num36] = num37;
									this.hullNext[num35] = num35;
									this.hullSize--;
									num35 = num36;
									num36 = this.hullPrev[num35];
								}
							}
							this.hullStart = (this.hullPrev[num29] = num35);
							this.hullNext[num35] = (this.hullPrev[num38] = num29);
							this.hullNext[num29] = num38;
							this.hullHash[this.HashKey(num30, num31)] = num29;
							this.hullHash[this.HashKey(this.coords[2 * num35], this.coords[2 * num35 + 1])] = num35;
						}
					}
				}
			}
			this.Hull = new int[this.hullSize];
			int num39 = this.hullStart;
			for (int num40 = 0; num40 < this.hullSize; num40++)
			{
				this.Hull[num40] = num39;
				num39 = this.hullNext[num39];
			}
			this.hullPrev = (this.hullNext = (this.hullTri = null));
			this.Triangles = this.Triangles.Take(this.trianglesLen).ToArray<int>();
			this.Halfedges = this.Halfedges.Take(this.trianglesLen).ToArray<int>();
		}

		// Token: 0x0601610F RID: 90383 RVA: 0x0069B2A8 File Offset: 0x006994A8
		private int Legalize(int a)
		{
			int num = 0;
			int num4;
			for (;;)
			{
				int num2 = this.Halfedges[a];
				int num3 = a - a % 3;
				num4 = num3 + (a + 2) % 3;
				if (num2 == -1)
				{
					if (num == 0)
					{
						break;
					}
					a = this.EDGE_STACK[--num];
				}
				else
				{
					int num5 = num2 - num2 % 3;
					int num6 = num3 + (a + 1) % 3;
					int num7 = num5 + (num2 + 2) % 3;
					int num8 = this.Triangles[num4];
					int num9 = this.Triangles[a];
					int num10 = this.Triangles[num6];
					int num11 = this.Triangles[num7];
					if (Delaunator.InCircle(this.coords[2 * num8], this.coords[2 * num8 + 1], this.coords[2 * num9], this.coords[2 * num9 + 1], this.coords[2 * num10], this.coords[2 * num10 + 1], this.coords[2 * num11], this.coords[2 * num11 + 1]))
					{
						this.Triangles[a] = num11;
						this.Triangles[num2] = num8;
						int num12 = this.Halfedges[num7];
						if (num12 == -1)
						{
							int num13 = this.hullStart;
							while (this.hullTri[num13] != num7)
							{
								num13 = this.hullPrev[num13];
								if (num13 == this.hullStart)
								{
									goto IL_13B;
								}
							}
							this.hullTri[num13] = a;
						}
						IL_13B:
						this.Link(a, num12);
						this.Link(num2, this.Halfedges[num4]);
						this.Link(num4, num7);
						int num14 = num5 + (num2 + 1) % 3;
						if (num < this.EDGE_STACK.Length)
						{
							this.EDGE_STACK[num++] = num14;
						}
					}
					else
					{
						if (num == 0)
						{
							break;
						}
						a = this.EDGE_STACK[--num];
					}
				}
			}
			return num4;
		}

		// Token: 0x06016110 RID: 90384 RVA: 0x0069B454 File Offset: 0x00699654
		private static bool InCircle(double ax, double ay, double bx, double by, double cx, double cy, double px, double py)
		{
			double num = ax - px;
			double num2 = ay - py;
			double num3 = bx - px;
			double num4 = by - py;
			double num5 = cx - px;
			double num6 = cy - py;
			double num7 = num * num + num2 * num2;
			double num8 = num3 * num3 + num4 * num4;
			double num9 = num5 * num5 + num6 * num6;
			return num * (num4 * num9 - num8 * num6) - num2 * (num3 * num9 - num8 * num5) + num7 * (num3 * num6 - num4 * num5) < 0.0;
		}

		// Token: 0x06016111 RID: 90385 RVA: 0x0069B4CC File Offset: 0x006996CC
		private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
		{
			int num = this.trianglesLen;
			this.Triangles[num] = i0;
			this.Triangles[num + 1] = i1;
			this.Triangles[num + 2] = i2;
			this.Link(num, a);
			this.Link(num + 1, b);
			this.Link(num + 2, c);
			this.trianglesLen += 3;
			return num;
		}

		// Token: 0x06016112 RID: 90386 RVA: 0x0069B52D File Offset: 0x0069972D
		private void Link(int a, int b)
		{
			this.Halfedges[a] = b;
			if (b != -1)
			{
				this.Halfedges[b] = a;
			}
		}

		// Token: 0x06016113 RID: 90387 RVA: 0x0069B545 File Offset: 0x00699745
		private int HashKey(double x, double y)
		{
			return (int)(Math.Floor(Delaunator.PseudoAngle(x - this.cx, y - this.cy) * (double)this.hashSize) % (double)this.hashSize);
		}

		// Token: 0x06016114 RID: 90388 RVA: 0x0069B574 File Offset: 0x00699774
		private static double PseudoAngle(double dx, double dy)
		{
			double num = dx / (Math.Abs(dx) + Math.Abs(dy));
			return ((dy > 0.0) ? (3.0 - num) : (1.0 + num)) / 4.0;
		}

		// Token: 0x06016115 RID: 90389 RVA: 0x0069B5C0 File Offset: 0x006997C0
		private static void Quicksort(int[] ids, double[] dists, int left, int right)
		{
			if (right - left <= 20)
			{
				for (int i = left + 1; i <= right; i++)
				{
					int num = ids[i];
					double num2 = dists[num];
					int num3 = i - 1;
					while (num3 >= left && dists[ids[num3]] > num2)
					{
						ids[num3 + 1] = ids[num3--];
					}
					ids[num3 + 1] = num;
				}
				return;
			}
			int i2 = left + right >> 1;
			int num4 = left + 1;
			int num5 = right;
			Delaunator.Swap(ids, i2, num4);
			if (dists[ids[left]] > dists[ids[right]])
			{
				Delaunator.Swap(ids, left, right);
			}
			if (dists[ids[num4]] > dists[ids[right]])
			{
				Delaunator.Swap(ids, num4, right);
			}
			if (dists[ids[left]] > dists[ids[num4]])
			{
				Delaunator.Swap(ids, left, num4);
			}
			int num6 = ids[num4];
			double num7 = dists[num6];
			for (;;)
			{
				num4++;
				if (dists[ids[num4]] >= num7)
				{
					do
					{
						num5--;
					}
					while (dists[ids[num5]] > num7);
					if (num5 < num4)
					{
						break;
					}
					Delaunator.Swap(ids, num4, num5);
				}
			}
			ids[left + 1] = ids[num5];
			ids[num5] = num6;
			if (right - num4 + 1 >= num5 - left)
			{
				Delaunator.Quicksort(ids, dists, num4, right);
				Delaunator.Quicksort(ids, dists, left, num5 - 1);
				return;
			}
			Delaunator.Quicksort(ids, dists, left, num5 - 1);
			Delaunator.Quicksort(ids, dists, num4, right);
		}

		// Token: 0x06016116 RID: 90390 RVA: 0x0069B6F0 File Offset: 0x006998F0
		private static void Swap(int[] arr, int i, int j)
		{
			int num = arr[i];
			arr[i] = arr[j];
			arr[j] = num;
		}

		// Token: 0x06016117 RID: 90391 RVA: 0x0069B70B File Offset: 0x0069990B
		private static bool Orient(double px, double py, double qx, double qy, double rx, double ry)
		{
			return (qy - py) * (rx - qx) - (qx - px) * (ry - qy) < 0.0;
		}

		// Token: 0x06016118 RID: 90392 RVA: 0x0069B72C File Offset: 0x0069992C
		private static double Circumradius(double ax, double ay, double bx, double by, double cx, double cy)
		{
			double num = bx - ax;
			double num2 = by - ay;
			double num3 = cx - ax;
			double num4 = cy - ay;
			double num5 = num * num + num2 * num2;
			double num6 = num3 * num3 + num4 * num4;
			double num7 = 0.5 / (num * num4 - num2 * num3);
			double num8 = (num4 * num5 - num2 * num6) * num7;
			double num9 = (num * num6 - num3 * num5) * num7;
			return num8 * num8 + num9 * num9;
		}

		// Token: 0x06016119 RID: 90393 RVA: 0x0069B794 File Offset: 0x00699994
		private static Vector2 Circumcenter(double ax, double ay, double bx, double by, double cx, double cy)
		{
			double num = bx - ax;
			double num2 = by - ay;
			double num3 = cx - ax;
			double num4 = cy - ay;
			double num5 = num * num + num2 * num2;
			double num6 = num3 * num3 + num4 * num4;
			double num7 = 0.5 / (num * num4 - num2 * num3);
			float num8 = (float)(ax + (num4 * num5 - num2 * num6) * num7);
			double num9 = ay + (num * num6 - num3 * num5) * num7;
			return new Vector2(num8, (float)num9);
		}

		// Token: 0x0601611A RID: 90394 RVA: 0x0069B800 File Offset: 0x00699A00
		private static double Dist(double ax, double ay, double bx, double by)
		{
			double num = ax - bx;
			double num2 = ay - by;
			return num * num + num2 * num2;
		}

		// Token: 0x0601611B RID: 90395 RVA: 0x0069B81A File Offset: 0x00699A1A
		public IEnumerable<ITriangle> GetTriangles()
		{
			int num;
			for (int t = 0; t < this.Triangles.Length / 3; t = num + 1)
			{
				yield return new Triangle(t, this.GetTrianglePoints(t));
				num = t;
			}
			yield break;
		}

		// Token: 0x0601611C RID: 90396 RVA: 0x0069B82A File Offset: 0x00699A2A
		public IEnumerable<IEdge> GetEdges()
		{
			int num;
			for (int e = 0; e < this.Triangles.Length; e = num + 1)
			{
				if (e > this.Halfedges[e])
				{
					Vector2 p = this.Points[this.Triangles[e]];
					Vector2 q = this.Points[this.Triangles[Delaunator.NextHalfedge(e)]];
					yield return new Edge(e, p, q);
				}
				num = e;
			}
			yield break;
		}

		// Token: 0x0601611D RID: 90397 RVA: 0x0069B83A File Offset: 0x00699A3A
		public IEnumerable<IEdge> GetVoronoiEdges(Func<int, Vector2> triangleVerticeSelector = null)
		{
			if (triangleVerticeSelector == null)
			{
				triangleVerticeSelector = ((int x) => this.GetCentroid(x));
			}
			int num;
			for (int e = 0; e < this.Triangles.Length; e = num + 1)
			{
				if (e < this.Halfedges[e])
				{
					Vector2 p = triangleVerticeSelector(Delaunator.TriangleOfEdge(e));
					Vector2 q = triangleVerticeSelector(Delaunator.TriangleOfEdge(this.Halfedges[e]));
					yield return new Edge(e, p, q);
				}
				num = e;
			}
			yield break;
		}

		// Token: 0x0601611E RID: 90398 RVA: 0x0069B851 File Offset: 0x00699A51
		public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCircumCenter()
		{
			return this.GetVoronoiEdges(new Func<int, Vector2>(this.GetTriangleCircumcenter));
		}

		// Token: 0x0601611F RID: 90399 RVA: 0x0069B865 File Offset: 0x00699A65
		public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCentroids()
		{
			return this.GetVoronoiEdges(new Func<int, Vector2>(this.GetCentroid));
		}

		// Token: 0x06016120 RID: 90400 RVA: 0x0069B879 File Offset: 0x00699A79
		public IEnumerable<IVoronoiCell> GetVoronoiCells(Func<int, Vector2> triangleVerticeSelector = null)
		{
			if (triangleVerticeSelector == null)
			{
				triangleVerticeSelector = ((int x) => this.GetCentroid(x));
			}
			HashSet<int> seen = new HashSet<int>();
			List<Vector2> vertices = new List<Vector2>(10);
			int num2;
			for (int e = 0; e < this.Triangles.Length; e = num2 + 1)
			{
				int num = this.Triangles[Delaunator.NextHalfedge(e)];
				if (seen.Add(num))
				{
					foreach (int e2 in this.EdgesAroundPoint(e))
					{
						vertices.Add(triangleVerticeSelector(Delaunator.TriangleOfEdge(e2)));
					}
					yield return new VoronoiCell(num, vertices.ToArray());
					vertices.Clear();
				}
				num2 = e;
			}
			yield break;
		}

		// Token: 0x06016121 RID: 90401 RVA: 0x0069B890 File Offset: 0x00699A90
		public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCircumcenters()
		{
			return this.GetVoronoiCells(new Func<int, Vector2>(this.GetTriangleCircumcenter));
		}

		// Token: 0x06016122 RID: 90402 RVA: 0x0069B8A4 File Offset: 0x00699AA4
		public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCentroids()
		{
			return this.GetVoronoiCells(new Func<int, Vector2>(this.GetCentroid));
		}

		// Token: 0x06016123 RID: 90403 RVA: 0x0069B8B8 File Offset: 0x00699AB8
		public IEnumerable<IEdge> GetHullEdges()
		{
			return Delaunator.CreateHull(this.GetHullPoints());
		}

		// Token: 0x06016124 RID: 90404 RVA: 0x0069B8C5 File Offset: 0x00699AC5
		public Vector2[] GetHullPoints()
		{
			return Array.ConvertAll<int, Vector2>(this.Hull, (int x) => this.Points[x]);
		}

		// Token: 0x06016125 RID: 90405 RVA: 0x0069B8E0 File Offset: 0x00699AE0
		public Vector2[] GetTrianglePoints(int t)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (int num in this.PointsOfTriangle(t))
			{
				list.Add(this.Points[num]);
			}
			return list.ToArray();
		}

		// Token: 0x06016126 RID: 90406 RVA: 0x0069B948 File Offset: 0x00699B48
		public Vector2[] GetRellaxedPoints()
		{
			List<Vector2> list = new List<Vector2>();
			foreach (IVoronoiCell voronoiCell in this.GetVoronoiCellsBasedOnCircumcenters())
			{
				list.Add(Delaunator.GetCentroid(voronoiCell.Points));
			}
			return list.ToArray();
		}

		// Token: 0x06016127 RID: 90407 RVA: 0x0069B9AC File Offset: 0x00699BAC
		public IEnumerable<IEdge> GetEdgesOfTriangle(int t)
		{
			return Delaunator.CreateHull(from p in Delaunator.EdgesOfTriangle(t)
			select this.Points[p]);
		}

		// Token: 0x06016128 RID: 90408 RVA: 0x0069B9CA File Offset: 0x00699BCA
		public static IEnumerable<IEdge> CreateHull(IEnumerable<Vector2> points)
		{
			return points.Zip(points.Skip(1).Append(points.FirstOrDefault<Vector2>()), (Vector2 a, Vector2 b) => new Edge(0, a, b)).OfType<IEdge>();
		}

		// Token: 0x06016129 RID: 90409 RVA: 0x0069BA08 File Offset: 0x00699C08
		public Vector2 GetTriangleCircumcenter(int t)
		{
			Vector2[] trianglePoints = this.GetTrianglePoints(t);
			return Delaunator.GetCircumcenter(trianglePoints[0], trianglePoints[1], trianglePoints[2]);
		}

		// Token: 0x0601612A RID: 90410 RVA: 0x0069BA37 File Offset: 0x00699C37
		public Vector2 GetCentroid(int t)
		{
			return Delaunator.GetCentroid(this.GetTrianglePoints(t));
		}

		// Token: 0x0601612B RID: 90411 RVA: 0x0069BA45 File Offset: 0x00699C45
		public static Vector2 GetCircumcenter(Vector2 a, Vector2 b, Vector2 c)
		{
			return Delaunator.Circumcenter((double)a.x, (double)a.y, (double)b.x, (double)b.y, (double)c.x, (double)c.y);
		}

		// Token: 0x0601612C RID: 90412 RVA: 0x0069BA78 File Offset: 0x00699C78
		public static Vector2 GetCentroid(Vector2[] points)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			int i = 0;
			int num4 = points.Length - 1;
			while (i < points.Length)
			{
				float num5 = points[i].x * points[num4].y - points[num4].x * points[i].y;
				num += (double)num5;
				num2 += (double)((points[i].x + points[num4].x) * num5);
				num3 += (double)((points[i].y + points[num4].y) * num5);
				num4 = i++;
			}
			if (Math.Abs(num) < 1.0000000116860974E-07)
			{
				return default(Vector2);
			}
			num *= 3.0;
			return new Vector2((float)(num2 / num), (float)(num3 / num));
		}

		// Token: 0x0601612D RID: 90413 RVA: 0x0069BB78 File Offset: 0x00699D78
		public void ForEachTriangle(Action<ITriangle> callback)
		{
			foreach (ITriangle obj in this.GetTriangles())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0601612E RID: 90414 RVA: 0x0069BBC8 File Offset: 0x00699DC8
		public void ForEachTriangleEdge(Action<IEdge> callback)
		{
			foreach (IEdge obj in this.GetEdges())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0601612F RID: 90415 RVA: 0x0069BC18 File Offset: 0x00699E18
		public void ForEachVoronoiEdge(Action<IEdge> callback)
		{
			foreach (IEdge obj in this.GetVoronoiEdges(null))
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x06016130 RID: 90416 RVA: 0x0069BC6C File Offset: 0x00699E6C
		public void ForEachVoronoiCellBasedOnCentroids(Action<IVoronoiCell> callback)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCellsBasedOnCentroids())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x06016131 RID: 90417 RVA: 0x0069BCBC File Offset: 0x00699EBC
		public void ForEachVoronoiCellBasedOnCircumcenters(Action<IVoronoiCell> callback)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCellsBasedOnCircumcenters())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x06016132 RID: 90418 RVA: 0x0069BD0C File Offset: 0x00699F0C
		public void ForEachVoronoiCell(Action<IVoronoiCell> callback, Func<int, Vector2> triangleVertexSelector = null)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCells(triangleVertexSelector))
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x06016133 RID: 90419 RVA: 0x0069BD60 File Offset: 0x00699F60
		public IEnumerable<int> EdgesAroundPoint(int start)
		{
			int incoming = start;
			do
			{
				yield return incoming;
				int num = Delaunator.NextHalfedge(incoming);
				incoming = this.Halfedges[num];
			}
			while (incoming != -1 && incoming != start);
			yield break;
		}

		// Token: 0x06016134 RID: 90420 RVA: 0x0069BD77 File Offset: 0x00699F77
		public IEnumerable<int> PointsOfTriangle(int t)
		{
			foreach (int num in Delaunator.EdgesOfTriangle(t))
			{
				yield return this.Triangles[num];
			}
			int[] array = null;
			yield break;
		}

		// Token: 0x06016135 RID: 90421 RVA: 0x0069BD90 File Offset: 0x00699F90
		public IEnumerable<int> TrianglesAdjacentToTriangle(int t)
		{
			List<int> list = new List<int>();
			foreach (int num in Delaunator.EdgesOfTriangle(t))
			{
				int num2 = this.Halfedges[num];
				if (num2 >= 0)
				{
					list.Add(Delaunator.TriangleOfEdge(num2));
				}
			}
			return list;
		}

		// Token: 0x06016136 RID: 90422 RVA: 0x0069BDD9 File Offset: 0x00699FD9
		public static int NextHalfedge(int e)
		{
			if (e % 3 != 2)
			{
				return e + 1;
			}
			return e - 2;
		}

		// Token: 0x06016137 RID: 90423 RVA: 0x0069BDE8 File Offset: 0x00699FE8
		public static int PreviousHalfedge(int e)
		{
			if (e % 3 != 0)
			{
				return e - 1;
			}
			return e + 2;
		}

		// Token: 0x06016138 RID: 90424 RVA: 0x0069BDF6 File Offset: 0x00699FF6
		public static int[] EdgesOfTriangle(int t)
		{
			return new int[]
			{
				3 * t,
				3 * t + 1,
				3 * t + 2
			};
		}

		// Token: 0x06016139 RID: 90425 RVA: 0x0069BE14 File Offset: 0x0069A014
		public static int TriangleOfEdge(int e)
		{
			return e / 3;
		}

		// Token: 0x0400F50B RID: 62731
		private readonly double EPSILON = Math.Pow(2.0, -52.0);

		// Token: 0x0400F50C RID: 62732
		private readonly int[] EDGE_STACK = new int[512];

		// Token: 0x0400F511 RID: 62737
		private readonly int hashSize;

		// Token: 0x0400F512 RID: 62738
		private readonly int[] hullPrev;

		// Token: 0x0400F513 RID: 62739
		private readonly int[] hullNext;

		// Token: 0x0400F514 RID: 62740
		private readonly int[] hullTri;

		// Token: 0x0400F515 RID: 62741
		private readonly int[] hullHash;

		// Token: 0x0400F516 RID: 62742
		private double cx;

		// Token: 0x0400F517 RID: 62743
		private double cy;

		// Token: 0x0400F518 RID: 62744
		private int trianglesLen;

		// Token: 0x0400F519 RID: 62745
		private readonly double[] coords;

		// Token: 0x0400F51A RID: 62746
		private readonly int hullStart;

		// Token: 0x0400F51B RID: 62747
		private readonly int hullSize;
	}
}
