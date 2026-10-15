using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DelaunatorSharp;

public class Delaunator
{
	[CompilerGenerated]
	private sealed class _003CEdgesAroundPoint_003Ed__66 : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private int _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int start;

		public int _003C_003E3__start;

		public Delaunator _003C_003E4__this;

		private int _003Cincoming_003E5__2;

		int IEnumerator<int>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEdgesAroundPoint_003Ed__66(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator delaunator = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				int num2 = NextHalfedge(_003Cincoming_003E5__2);
				_003Cincoming_003E5__2 = delaunator.Halfedges[num2];
				if (_003Cincoming_003E5__2 == -1 || _003Cincoming_003E5__2 == start)
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003Cincoming_003E5__2 = start;
			}
			_003C_003E2__current = _003Cincoming_003E5__2;
			_003C_003E1__state = 1;
			return true;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			_003CEdgesAroundPoint_003Ed__66 _003CEdgesAroundPoint_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEdgesAroundPoint_003Ed__ = this;
			}
			else
			{
				_003CEdgesAroundPoint_003Ed__ = new _003CEdgesAroundPoint_003Ed__66(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CEdgesAroundPoint_003Ed__.start = _003C_003E3__start;
			return _003CEdgesAroundPoint_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEdges_003Ed__43 : IEnumerable<IEdge>, IEnumerable, IEnumerator<IEdge>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IEdge _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Delaunator _003C_003E4__this;

		private int _003Ce_003E5__2;

		IEdge IEnumerator<IEdge>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetEdges_003Ed__43(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator delaunator = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00a1;
			}
			_003C_003E1__state = -1;
			_003Ce_003E5__2 = 0;
			goto IL_00b3;
			IL_00a1:
			_003Ce_003E5__2++;
			goto IL_00b3;
			IL_00b3:
			if (_003Ce_003E5__2 < delaunator.Triangles.Length)
			{
				if (_003Ce_003E5__2 > delaunator.Halfedges[_003Ce_003E5__2])
				{
					Vector2 p = delaunator.Points[delaunator.Triangles[_003Ce_003E5__2]];
					Vector2 q = delaunator.Points[delaunator.Triangles[NextHalfedge(_003Ce_003E5__2)]];
					_003C_003E2__current = new Edge(_003Ce_003E5__2, p, q);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00a1;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IEdge> IEnumerable<IEdge>.GetEnumerator()
		{
			_003CGetEdges_003Ed__43 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetEdges_003Ed__43(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IEdge>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTriangles_003Ed__42 : IEnumerable<ITriangle>, IEnumerable, IEnumerator<ITriangle>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ITriangle _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Delaunator _003C_003E4__this;

		private int _003Ct_003E5__2;

		ITriangle IEnumerator<ITriangle>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetTriangles_003Ed__42(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator delaunator = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ct_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ct_003E5__2++;
				break;
			}
			if (_003Ct_003E5__2 < delaunator.Triangles.Length / 3)
			{
				_003C_003E2__current = new Triangle(_003Ct_003E5__2, delaunator.GetTrianglePoints(_003Ct_003E5__2));
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ITriangle> IEnumerable<ITriangle>.GetEnumerator()
		{
			_003CGetTriangles_003Ed__42 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTriangles_003Ed__42(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ITriangle>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetVoronoiCells_003Ed__47 : IEnumerable<IVoronoiCell>, IEnumerable, IEnumerator<IVoronoiCell>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IVoronoiCell _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Func<int, Vector2> triangleVerticeSelector;

		public Func<int, Vector2> _003C_003E3__triangleVerticeSelector;

		public Delaunator _003C_003E4__this;

		private HashSet<int> _003Cseen_003E5__2;

		private List<Vector2> _003Cvertices_003E5__3;

		private int _003Ce_003E5__4;

		IVoronoiCell IEnumerator<IVoronoiCell>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetVoronoiCells_003Ed__47(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cseen_003E5__2 = null;
			_003Cvertices_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cvertices_003E5__3.Clear();
				goto IL_0107;
			}
			_003C_003E1__state = -1;
			if (triangleVerticeSelector == null)
			{
				triangleVerticeSelector = (int x) => CS_0024_003C_003E8__locals4.GetCentroid(x);
			}
			_003Cseen_003E5__2 = new HashSet<int>();
			_003Cvertices_003E5__3 = new List<Vector2>(10);
			_003Ce_003E5__4 = 0;
			goto IL_0119;
			IL_0119:
			if (_003Ce_003E5__4 < CS_0024_003C_003E8__locals4.Triangles.Length)
			{
				int num2 = CS_0024_003C_003E8__locals4.Triangles[NextHalfedge(_003Ce_003E5__4)];
				if (_003Cseen_003E5__2.Add(num2))
				{
					foreach (int item in CS_0024_003C_003E8__locals4.EdgesAroundPoint(_003Ce_003E5__4))
					{
						_003Cvertices_003E5__3.Add(triangleVerticeSelector(TriangleOfEdge(item)));
					}
					_003C_003E2__current = new VoronoiCell(num2, _003Cvertices_003E5__3.ToArray());
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0107;
			}
			return false;
			IL_0107:
			_003Ce_003E5__4++;
			goto IL_0119;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IVoronoiCell> IEnumerable<IVoronoiCell>.GetEnumerator()
		{
			_003CGetVoronoiCells_003Ed__47 _003CGetVoronoiCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetVoronoiCells_003Ed__ = this;
			}
			else
			{
				_003CGetVoronoiCells_003Ed__ = new _003CGetVoronoiCells_003Ed__47(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetVoronoiCells_003Ed__.triangleVerticeSelector = _003C_003E3__triangleVerticeSelector;
			return _003CGetVoronoiCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IVoronoiCell>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetVoronoiEdges_003Ed__44 : IEnumerable<IEdge>, IEnumerable, IEnumerator<IEdge>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IEdge _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Func<int, Vector2> triangleVerticeSelector;

		public Func<int, Vector2> _003C_003E3__triangleVerticeSelector;

		public Delaunator _003C_003E4__this;

		private int _003Ce_003E5__2;

		IEdge IEnumerator<IEdge>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetVoronoiEdges_003Ed__44(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b9;
			}
			_003C_003E1__state = -1;
			if (triangleVerticeSelector == null)
			{
				triangleVerticeSelector = (int x) => CS_0024_003C_003E8__locals4.GetCentroid(x);
			}
			_003Ce_003E5__2 = 0;
			goto IL_00cb;
			IL_00cb:
			if (_003Ce_003E5__2 < CS_0024_003C_003E8__locals4.Triangles.Length)
			{
				if (_003Ce_003E5__2 < CS_0024_003C_003E8__locals4.Halfedges[_003Ce_003E5__2])
				{
					Vector2 p = triangleVerticeSelector(TriangleOfEdge(_003Ce_003E5__2));
					Vector2 q = triangleVerticeSelector(TriangleOfEdge(CS_0024_003C_003E8__locals4.Halfedges[_003Ce_003E5__2]));
					_003C_003E2__current = new Edge(_003Ce_003E5__2, p, q);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b9;
			}
			return false;
			IL_00b9:
			_003Ce_003E5__2++;
			goto IL_00cb;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IEdge> IEnumerable<IEdge>.GetEnumerator()
		{
			_003CGetVoronoiEdges_003Ed__44 _003CGetVoronoiEdges_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetVoronoiEdges_003Ed__ = this;
			}
			else
			{
				_003CGetVoronoiEdges_003Ed__ = new _003CGetVoronoiEdges_003Ed__44(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetVoronoiEdges_003Ed__.triangleVerticeSelector = _003C_003E3__triangleVerticeSelector;
			return _003CGetVoronoiEdges_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IEdge>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPointsOfTriangle_003Ed__67 : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private int _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int t;

		public int _003C_003E3__t;

		public Delaunator _003C_003E4__this;

		private int[] _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

		int IEnumerator<int>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CPointsOfTriangle_003Ed__67(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Delaunator delaunator = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = EdgesOfTriangle(t);
				_003C_003E7__wrap2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E7__wrap2++;
				break;
			}
			if (_003C_003E7__wrap2 < _003C_003E7__wrap1.Length)
			{
				int num2 = _003C_003E7__wrap1[_003C_003E7__wrap2];
				_003C_003E2__current = delaunator.Triangles[num2];
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E7__wrap1 = null;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			_003CPointsOfTriangle_003Ed__67 _003CPointsOfTriangle_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPointsOfTriangle_003Ed__ = this;
			}
			else
			{
				_003CPointsOfTriangle_003Ed__ = new _003CPointsOfTriangle_003Ed__67(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CPointsOfTriangle_003Ed__.t = _003C_003E3__t;
			return _003CPointsOfTriangle_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}
	}

	private readonly double EPSILON = Math.Pow(2.0, -52.0);

	private readonly int[] EDGE_STACK = new int[512];

	private readonly int hashSize;

	private readonly int[] hullPrev;

	private readonly int[] hullNext;

	private readonly int[] hullTri;

	private readonly int[] hullHash;

	private double cx;

	private double cy;

	private int trianglesLen;

	private readonly double[] coords;

	private readonly int hullStart;

	private readonly int hullSize;

	public int[] Triangles { get; private set; }

	public int[] Halfedges { get; private set; }

	public Vector2[] Points { get; private set; }

	public int[] Hull { get; private set; }

	public Delaunator(Vector2[] points)
	{
		if (points.Length < 3)
		{
			throw new ArgumentOutOfRangeException("Need at least 3 points");
		}
		Points = points;
		coords = new double[Points.Length * 2];
		for (int i = 0; i < Points.Length; i++)
		{
			Vector2 vector = Points[i];
			coords[2 * i] = vector.x;
			coords[2 * i + 1] = vector.y;
		}
		int num = points.Length;
		int num2 = 2 * num - 5;
		Triangles = new int[num2 * 3];
		Halfedges = new int[num2 * 3];
		hashSize = (int)Math.Ceiling(Math.Sqrt(num));
		hullPrev = new int[num];
		hullNext = new int[num];
		hullTri = new int[num];
		hullHash = new int[hashSize];
		int[] array = new int[num];
		double num3 = double.PositiveInfinity;
		double num4 = double.PositiveInfinity;
		double num5 = double.NegativeInfinity;
		double num6 = double.NegativeInfinity;
		for (int j = 0; j < num; j++)
		{
			double num7 = coords[2 * j];
			double num8 = coords[2 * j + 1];
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
			double num13 = Dist(ax, ay, coords[2 * k], coords[2 * k + 1]);
			if (num13 < num9)
			{
				num10 = k;
				num9 = num13;
			}
		}
		double num14 = coords[2 * num10];
		double num15 = coords[2 * num10 + 1];
		num9 = double.PositiveInfinity;
		for (int l = 0; l < num; l++)
		{
			if (l != num10)
			{
				double num16 = Dist(num14, num15, coords[2 * l], coords[2 * l + 1]);
				if (num16 < num9 && num16 > 0.0)
				{
					num11 = l;
					num9 = num16;
				}
			}
		}
		double num17 = coords[2 * num11];
		double num18 = coords[2 * num11 + 1];
		double num19 = double.PositiveInfinity;
		for (int m = 0; m < num; m++)
		{
			if (m != num10 && m != num11)
			{
				double num20 = Circumradius(num14, num15, num17, num18, coords[2 * m], coords[2 * m + 1]);
				if (num20 < num19)
				{
					num12 = m;
					num19 = num20;
				}
			}
		}
		double num21 = coords[2 * num12];
		double num22 = coords[2 * num12 + 1];
		if (num19 == double.PositiveInfinity)
		{
			throw new Exception("No Delaunay triangulation exists for this input.");
		}
		if (Orient(num14, num15, num17, num18, num21, num22))
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
		Vector2 vector2 = Circumcenter(num14, num15, num17, num18, num21, num22);
		cx = vector2.x;
		cy = vector2.y;
		double[] array2 = new double[num];
		for (int n = 0; n < num; n++)
		{
			array2[n] = Dist(coords[2 * n], coords[2 * n + 1], vector2.x, vector2.y);
		}
		Quicksort(array, array2, 0, num - 1);
		hullStart = num10;
		hullSize = 3;
		hullNext[num10] = (hullPrev[num12] = num11);
		hullNext[num11] = (hullPrev[num10] = num12);
		hullNext[num12] = (hullPrev[num11] = num10);
		hullTri[num10] = 0;
		hullTri[num11] = 1;
		hullTri[num12] = 2;
		hullHash[HashKey(num14, num15)] = num10;
		hullHash[HashKey(num17, num18)] = num11;
		hullHash[HashKey(num21, num22)] = num12;
		trianglesLen = 0;
		AddTriangle(num10, num11, num12, -1, -1, -1);
		double num26 = 0.0;
		double num27 = 0.0;
		for (int num28 = 0; num28 < array.Length; num28++)
		{
			int num29 = array[num28];
			double num30 = coords[2 * num29];
			double num31 = coords[2 * num29 + 1];
			if (num28 > 0 && Math.Abs(num30 - num26) <= EPSILON && Math.Abs(num31 - num27) <= EPSILON)
			{
				continue;
			}
			num26 = num30;
			num27 = num31;
			if (num29 == num10 || num29 == num11 || num29 == num12)
			{
				continue;
			}
			int num32 = 0;
			for (int num33 = 0; num33 < hashSize; num33++)
			{
				int num34 = HashKey(num30, num31);
				num32 = hullHash[(num34 + num33) % hashSize];
				if (num32 != -1 && num32 != hullNext[num32])
				{
					break;
				}
			}
			num32 = hullPrev[num32];
			int num35 = num32;
			int num36 = hullNext[num35];
			while (!Orient(num30, num31, coords[2 * num35], coords[2 * num35 + 1], coords[2 * num36], coords[2 * num36 + 1]))
			{
				num35 = num36;
				if (num35 == num32)
				{
					num35 = int.MaxValue;
					break;
				}
				num36 = hullNext[num35];
			}
			if (num35 == int.MaxValue)
			{
				continue;
			}
			int num37 = AddTriangle(num35, num29, hullNext[num35], -1, -1, hullTri[num35]);
			hullTri[num29] = Legalize(num37 + 2);
			hullTri[num35] = num37;
			hullSize++;
			int num38 = hullNext[num35];
			num36 = hullNext[num38];
			while (Orient(num30, num31, coords[2 * num38], coords[2 * num38 + 1], coords[2 * num36], coords[2 * num36 + 1]))
			{
				num37 = AddTriangle(num38, num29, num36, hullTri[num29], -1, hullTri[num38]);
				hullTri[num29] = Legalize(num37 + 2);
				hullNext[num38] = num38;
				hullSize--;
				num38 = num36;
				num36 = hullNext[num38];
			}
			if (num35 == num32)
			{
				num36 = hullPrev[num35];
				while (Orient(num30, num31, coords[2 * num36], coords[2 * num36 + 1], coords[2 * num35], coords[2 * num35 + 1]))
				{
					num37 = AddTriangle(num36, num29, num35, -1, hullTri[num35], hullTri[num36]);
					Legalize(num37 + 2);
					hullTri[num36] = num37;
					hullNext[num35] = num35;
					hullSize--;
					num35 = num36;
					num36 = hullPrev[num35];
				}
			}
			hullStart = (hullPrev[num29] = num35);
			hullNext[num35] = (hullPrev[num38] = num29);
			hullNext[num29] = num38;
			hullHash[HashKey(num30, num31)] = num29;
			hullHash[HashKey(coords[2 * num35], coords[2 * num35 + 1])] = num35;
		}
		Hull = new int[hullSize];
		int num39 = hullStart;
		for (int num40 = 0; num40 < hullSize; num40++)
		{
			Hull[num40] = num39;
			num39 = hullNext[num39];
		}
		hullPrev = (hullNext = (hullTri = null));
		Triangles = Triangles.Take(trianglesLen).ToArray();
		Halfedges = Halfedges.Take(trianglesLen).ToArray();
	}

	private int Legalize(int a)
	{
		int num = 0;
		int num4;
		while (true)
		{
			int num2 = Halfedges[a];
			int num3 = a - a % 3;
			num4 = num3 + (a + 2) % 3;
			if (num2 == -1)
			{
				if (num == 0)
				{
					break;
				}
				a = EDGE_STACK[--num];
				continue;
			}
			int num5 = num2 - num2 % 3;
			int num6 = num3 + (a + 1) % 3;
			int num7 = num5 + (num2 + 2) % 3;
			int num8 = Triangles[num4];
			int num9 = Triangles[a];
			int num10 = Triangles[num6];
			int num11 = Triangles[num7];
			if (InCircle(coords[2 * num8], coords[2 * num8 + 1], coords[2 * num9], coords[2 * num9 + 1], coords[2 * num10], coords[2 * num10 + 1], coords[2 * num11], coords[2 * num11 + 1]))
			{
				Triangles[a] = num11;
				Triangles[num2] = num8;
				int num12 = Halfedges[num7];
				if (num12 == -1)
				{
					int num13 = hullStart;
					do
					{
						if (hullTri[num13] == num7)
						{
							hullTri[num13] = a;
							break;
						}
						num13 = hullPrev[num13];
					}
					while (num13 != hullStart);
				}
				Link(a, num12);
				Link(num2, Halfedges[num4]);
				Link(num4, num7);
				int num14 = num5 + (num2 + 1) % 3;
				if (num < EDGE_STACK.Length)
				{
					EDGE_STACK[num++] = num14;
				}
			}
			else
			{
				if (num == 0)
				{
					break;
				}
				a = EDGE_STACK[--num];
			}
		}
		return num4;
	}

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

	private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
	{
		int num = trianglesLen;
		Triangles[num] = i0;
		Triangles[num + 1] = i1;
		Triangles[num + 2] = i2;
		Link(num, a);
		Link(num + 1, b);
		Link(num + 2, c);
		trianglesLen += 3;
		return num;
	}

	private void Link(int a, int b)
	{
		Halfedges[a] = b;
		if (b != -1)
		{
			Halfedges[b] = a;
		}
	}

	private int HashKey(double x, double y)
	{
		return (int)(Math.Floor(PseudoAngle(x - cx, y - cy) * (double)hashSize) % (double)hashSize);
	}

	private static double PseudoAngle(double dx, double dy)
	{
		double num = dx / (Math.Abs(dx) + Math.Abs(dy));
		return ((dy > 0.0) ? (3.0 - num) : (1.0 + num)) / 4.0;
	}

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
		Swap(ids, i2, num4);
		if (dists[ids[left]] > dists[ids[right]])
		{
			Swap(ids, left, right);
		}
		if (dists[ids[num4]] > dists[ids[right]])
		{
			Swap(ids, num4, right);
		}
		if (dists[ids[left]] > dists[ids[num4]])
		{
			Swap(ids, left, num4);
		}
		int num6 = ids[num4];
		double num7 = dists[num6];
		while (true)
		{
			num4++;
			if (!(dists[ids[num4]] < num7))
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
				Swap(ids, num4, num5);
			}
		}
		ids[left + 1] = ids[num5];
		ids[num5] = num6;
		if (right - num4 + 1 >= num5 - left)
		{
			Quicksort(ids, dists, num4, right);
			Quicksort(ids, dists, left, num5 - 1);
		}
		else
		{
			Quicksort(ids, dists, left, num5 - 1);
			Quicksort(ids, dists, num4, right);
		}
	}

	private static void Swap(int[] arr, int i, int j)
	{
		int num = arr[i];
		arr[i] = arr[j];
		arr[j] = num;
	}

	private static bool Orient(double px, double py, double qx, double qy, double rx, double ry)
	{
		return (qy - py) * (rx - qx) - (qx - px) * (ry - qy) < 0.0;
	}

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

	private static Vector2 Circumcenter(double ax, double ay, double bx, double by, double cx, double cy)
	{
		double num = bx - ax;
		double num2 = by - ay;
		double num3 = cx - ax;
		double num4 = cy - ay;
		double num5 = num * num + num2 * num2;
		double num6 = num3 * num3 + num4 * num4;
		double num7 = 0.5 / (num * num4 - num2 * num3);
		double num8 = ax + (num4 * num5 - num2 * num6) * num7;
		double num9 = ay + (num * num6 - num3 * num5) * num7;
		return new Vector2((float)num8, (float)num9);
	}

	private static double Dist(double ax, double ay, double bx, double by)
	{
		double num = ax - bx;
		double num2 = ay - by;
		return num * num + num2 * num2;
	}

	[IteratorStateMachine(typeof(_003CGetTriangles_003Ed__42))]
	public IEnumerable<ITriangle> GetTriangles()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTriangles_003Ed__42(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetEdges_003Ed__43))]
	public IEnumerable<IEdge> GetEdges()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEdges_003Ed__43(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetVoronoiEdges_003Ed__44))]
	public IEnumerable<IEdge> GetVoronoiEdges(Func<int, Vector2> triangleVerticeSelector = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetVoronoiEdges_003Ed__44(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__triangleVerticeSelector = triangleVerticeSelector
		};
	}

	public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCircumCenter()
	{
		return GetVoronoiEdges(GetTriangleCircumcenter);
	}

	public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCentroids()
	{
		return GetVoronoiEdges(GetCentroid);
	}

	[IteratorStateMachine(typeof(_003CGetVoronoiCells_003Ed__47))]
	public IEnumerable<IVoronoiCell> GetVoronoiCells(Func<int, Vector2> triangleVerticeSelector = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetVoronoiCells_003Ed__47(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__triangleVerticeSelector = triangleVerticeSelector
		};
	}

	public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCircumcenters()
	{
		return GetVoronoiCells(GetTriangleCircumcenter);
	}

	public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCentroids()
	{
		return GetVoronoiCells(GetCentroid);
	}

	public IEnumerable<IEdge> GetHullEdges()
	{
		return CreateHull(GetHullPoints());
	}

	public Vector2[] GetHullPoints()
	{
		return Array.ConvertAll(Hull, (int x) => Points[x]);
	}

	public Vector2[] GetTrianglePoints(int t)
	{
		List<Vector2> list = new List<Vector2>();
		foreach (int item in PointsOfTriangle(t))
		{
			list.Add(Points[item]);
		}
		return list.ToArray();
	}

	public Vector2[] GetRellaxedPoints()
	{
		List<Vector2> list = new List<Vector2>();
		foreach (IVoronoiCell voronoiCellsBasedOnCircumcenter in GetVoronoiCellsBasedOnCircumcenters())
		{
			list.Add(GetCentroid(voronoiCellsBasedOnCircumcenter.Points));
		}
		return list.ToArray();
	}

	public IEnumerable<IEdge> GetEdgesOfTriangle(int t)
	{
		return CreateHull(from p in EdgesOfTriangle(t)
			select Points[p]);
	}

	public static IEnumerable<IEdge> CreateHull(IEnumerable<Vector2> points)
	{
		return points.Zip(points.Skip(1).Append(points.FirstOrDefault()), (Vector2 a, Vector2 b) => new Edge(0, a, b)).OfType<IEdge>();
	}

	public Vector2 GetTriangleCircumcenter(int t)
	{
		Vector2[] trianglePoints = GetTrianglePoints(t);
		return GetCircumcenter(trianglePoints[0], trianglePoints[1], trianglePoints[2]);
	}

	public Vector2 GetCentroid(int t)
	{
		return GetCentroid(GetTrianglePoints(t));
	}

	public static Vector2 GetCircumcenter(Vector2 a, Vector2 b, Vector2 c)
	{
		return Circumcenter(a.x, a.y, b.x, b.y, c.x, c.y);
	}

	public static Vector2 GetCentroid(Vector2[] points)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		int num4 = 0;
		int num5 = points.Length - 1;
		while (num4 < points.Length)
		{
			float num6 = points[num4].x * points[num5].y - points[num5].x * points[num4].y;
			num += (double)num6;
			num2 += (double)((points[num4].x + points[num5].x) * num6);
			num3 += (double)((points[num4].y + points[num5].y) * num6);
			num5 = num4++;
		}
		if (Math.Abs(num) < 1.0000000116860974E-07)
		{
			return default(Vector2);
		}
		num *= 3.0;
		return new Vector2((float)(num2 / num), (float)(num3 / num));
	}

	public void ForEachTriangle(Action<ITriangle> callback)
	{
		foreach (ITriangle triangle in GetTriangles())
		{
			callback?.Invoke(triangle);
		}
	}

	public void ForEachTriangleEdge(Action<IEdge> callback)
	{
		foreach (IEdge edge in GetEdges())
		{
			callback?.Invoke(edge);
		}
	}

	public void ForEachVoronoiEdge(Action<IEdge> callback)
	{
		foreach (IEdge voronoiEdge in GetVoronoiEdges())
		{
			callback?.Invoke(voronoiEdge);
		}
	}

	public void ForEachVoronoiCellBasedOnCentroids(Action<IVoronoiCell> callback)
	{
		foreach (IVoronoiCell voronoiCellsBasedOnCentroid in GetVoronoiCellsBasedOnCentroids())
		{
			callback?.Invoke(voronoiCellsBasedOnCentroid);
		}
	}

	public void ForEachVoronoiCellBasedOnCircumcenters(Action<IVoronoiCell> callback)
	{
		foreach (IVoronoiCell voronoiCellsBasedOnCircumcenter in GetVoronoiCellsBasedOnCircumcenters())
		{
			callback?.Invoke(voronoiCellsBasedOnCircumcenter);
		}
	}

	public void ForEachVoronoiCell(Action<IVoronoiCell> callback, Func<int, Vector2> triangleVertexSelector = null)
	{
		foreach (IVoronoiCell voronoiCell in GetVoronoiCells(triangleVertexSelector))
		{
			callback?.Invoke(voronoiCell);
		}
	}

	[IteratorStateMachine(typeof(_003CEdgesAroundPoint_003Ed__66))]
	public IEnumerable<int> EdgesAroundPoint(int start)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEdgesAroundPoint_003Ed__66(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__start = start
		};
	}

	[IteratorStateMachine(typeof(_003CPointsOfTriangle_003Ed__67))]
	public IEnumerable<int> PointsOfTriangle(int t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPointsOfTriangle_003Ed__67(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__t = t
		};
	}

	public IEnumerable<int> TrianglesAdjacentToTriangle(int t)
	{
		List<int> list = new List<int>();
		int[] array = EdgesOfTriangle(t);
		foreach (int num in array)
		{
			int num2 = Halfedges[num];
			if (num2 >= 0)
			{
				list.Add(TriangleOfEdge(num2));
			}
		}
		return list;
	}

	public static int NextHalfedge(int e)
	{
		if (e % 3 != 2)
		{
			return e + 1;
		}
		return e - 2;
	}

	public static int PreviousHalfedge(int e)
	{
		if (e % 3 != 0)
		{
			return e - 1;
		}
		return e + 2;
	}

	public static int[] EdgesOfTriangle(int t)
	{
		return new int[3]
		{
			3 * t,
			3 * t + 1,
			3 * t + 2
		};
	}

	public static int TriangleOfEdge(int e)
	{
		return e / 3;
	}
}
