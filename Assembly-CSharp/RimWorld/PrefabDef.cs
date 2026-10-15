using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PrefabDef : Def
{
	[CompilerGenerated]
	private sealed class _003CGetPrefabs_003Ed__7 : IEnumerable<(SubPrefabData data, IntVec3 cell)>, IEnumerable, IEnumerator<(SubPrefabData data, IntVec3 cell)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (SubPrefabData data, IntVec3 cell) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PrefabDef _003C_003E4__this;

		private List<SubPrefabData>.Enumerator _003C_003E7__wrap1;

		private SubPrefabData _003Cdata_003E5__3;

		private List<IntVec3>.Enumerator _003C_003E7__wrap3;

		(SubPrefabData data, IntVec3 cell) IEnumerator<(SubPrefabData, IntVec3)>.Current
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
		public _003CGetPrefabs_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<SubPrefabData>.Enumerator);
			_003Cdata_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<IntVec3>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PrefabDef prefabDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = prefabDef.prefabs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_011a;
				case 1:
					_003C_003E1__state = -4;
					goto IL_00c3;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_0113;
					}
					IL_011a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cdata_003E5__3 = _003C_003E7__wrap1.Current;
						if (!_003Cdata_003E5__3.positions.NullOrEmpty())
						{
							_003C_003E7__wrap3 = _003Cdata_003E5__3.positions.GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_00c3;
						}
						_003C_003E2__current = (data: _003Cdata_003E5__3, cell: _003Cdata_003E5__3.position);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<SubPrefabData>.Enumerator);
					return false;
					IL_00c3:
					if (_003C_003E7__wrap3.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = (data: _003Cdata_003E5__3, cell: current);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<IntVec3>.Enumerator);
					goto IL_0113;
					IL_0113:
					_003Cdata_003E5__3 = null;
					goto IL_011a;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(SubPrefabData data, IntVec3 cell)> IEnumerable<(SubPrefabData, IntVec3)>.GetEnumerator()
		{
			_003CGetPrefabs_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPrefabs_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(SubPrefabData, IntVec3)>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTerrain_003Ed__8 : IEnumerable<(PrefabTerrainData data, IntVec3 cell)>, IEnumerable, IEnumerator<(PrefabTerrainData data, IntVec3 cell)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PrefabTerrainData data, IntVec3 cell) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PrefabDef _003C_003E4__this;

		private List<PrefabTerrainData>.Enumerator _003C_003E7__wrap1;

		private PrefabTerrainData _003Cdata_003E5__3;

		private List<CellRect>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<IntVec3> _003C_003E7__wrap4;

		(PrefabTerrainData data, IntVec3 cell) IEnumerator<(PrefabTerrainData, IntVec3)>.Current
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
		public _003CGetTerrain_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || num == 1)
			{
				try
				{
					if ((uint)(num - -5) <= 1u || num == 1)
					{
						try
						{
							if (num == -5 || num == 1)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<PrefabTerrainData>.Enumerator);
			_003Cdata_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<CellRect>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PrefabDef prefabDef = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -5;
					goto IL_00e5;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = prefabDef.terrain.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_0128;
				IL_0128:
				do
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cdata_003E5__3 = _003C_003E7__wrap1.Current;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PrefabTerrainData>.Enumerator);
					return false;
				}
				while (_003Cdata_003E5__3.rects.NullOrEmpty());
				_003C_003E7__wrap3 = _003Cdata_003E5__3.rects.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_00ff;
				IL_00e5:
				if (_003C_003E7__wrap4.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap4.Current;
					_003C_003E2__current = (data: _003Cdata_003E5__3, cell: current);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap4 = null;
				goto IL_00ff;
				IL_00ff:
				if (_003C_003E7__wrap3.MoveNext())
				{
					_003C_003E7__wrap4 = _003C_003E7__wrap3.Current.Cells.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_00e5;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap3 = default(List<CellRect>.Enumerator);
				_003Cdata_003E5__3 = null;
				goto IL_0128;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PrefabTerrainData data, IntVec3 cell)> IEnumerable<(PrefabTerrainData, IntVec3)>.GetEnumerator()
		{
			_003CGetTerrain_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTerrain_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PrefabTerrainData, IntVec3)>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetThings_003Ed__6 : IEnumerable<(PrefabThingData data, IntVec3 cell)>, IEnumerable, IEnumerator<(PrefabThingData data, IntVec3 cell)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PrefabThingData data, IntVec3 cell) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PrefabDef _003C_003E4__this;

		private List<PrefabThingData>.Enumerator _003C_003E7__wrap1;

		private PrefabThingData _003Cdata_003E5__3;

		private List<CellRect>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<IntVec3> _003C_003E7__wrap4;

		private List<IntVec3>.Enumerator _003C_003E7__wrap5;

		(PrefabThingData data, IntVec3 cell) IEnumerator<(PrefabThingData, IntVec3)>.Current
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
		public _003CGetThings_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -6) <= 3u || (uint)(num - 1) <= 2u)
			{
				try
				{
					switch (num)
					{
					case -5:
					case -4:
					case 1:
						try
						{
							if (num == -5 || num == 1)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally2();
						}
						break;
					case -6:
					case 2:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
						break;
					case -3:
					case -2:
					case -1:
					case 0:
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<PrefabThingData>.Enumerator);
			_003Cdata_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<CellRect>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = default(List<IntVec3>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PrefabDef prefabDef = _003C_003E4__this;
				bool flag;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = prefabDef.things.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01f7;
				case 1:
					_003C_003E1__state = -5;
					flag = true;
					goto IL_00f9;
				case 2:
					_003C_003E1__state = -6;
					flag = true;
					goto IL_019f;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_01f0;
					}
					IL_01f7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cdata_003E5__3 = _003C_003E7__wrap1.Current;
						flag = false;
						if (!_003Cdata_003E5__3.rects.NullOrEmpty())
						{
							_003C_003E7__wrap3 = _003Cdata_003E5__3.rects.GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_0113;
						}
						goto IL_0135;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PrefabThingData>.Enumerator);
					return false;
					IL_01f0:
					_003Cdata_003E5__3 = null;
					goto IL_01f7;
					IL_0135:
					if (!_003Cdata_003E5__3.positions.NullOrEmpty())
					{
						_003C_003E7__wrap5 = _003Cdata_003E5__3.positions.GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_019f;
					}
					goto IL_01be;
					IL_0113:
					if (_003C_003E7__wrap3.MoveNext())
					{
						_003C_003E7__wrap4 = _003C_003E7__wrap3.Current.Cells.GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_00f9;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<CellRect>.Enumerator);
					goto IL_0135;
					IL_01be:
					if (!flag)
					{
						_003C_003E2__current = (data: _003Cdata_003E5__3, cell: _003Cdata_003E5__3.position);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01f0;
					IL_019f:
					if (_003C_003E7__wrap5.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap5.Current;
						_003C_003E2__current = (data: _003Cdata_003E5__3, cell: current);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap5 = default(List<IntVec3>.Enumerator);
					goto IL_01be;
					IL_00f9:
					if (_003C_003E7__wrap4.MoveNext())
					{
						IntVec3 current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = (data: _003Cdata_003E5__3, cell: current2);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					goto IL_0113;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PrefabThingData data, IntVec3 cell)> IEnumerable<(PrefabThingData, IntVec3)>.GetEnumerator()
		{
			_003CGetThings_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetThings_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PrefabThingData, IntVec3)>)this).GetEnumerator();
		}
	}

	internal List<PrefabThingData> things = new List<PrefabThingData>();

	internal List<SubPrefabData> prefabs = new List<SubPrefabData>();

	internal List<PrefabTerrainData> terrain = new List<PrefabTerrainData>();

	public IntVec2 size;

	public RotEnum rotations = RotEnum.All;

	public bool edgeOnly;

	[IteratorStateMachine(typeof(_003CGetThings_003Ed__6))]
	public IEnumerable<(PrefabThingData data, IntVec3 cell)> GetThings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetThings_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetPrefabs_003Ed__7))]
	public IEnumerable<(SubPrefabData data, IntVec3 cell)> GetPrefabs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPrefabs_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetTerrain_003Ed__8))]
	public IEnumerable<(PrefabTerrainData data, IntVec3 cell)> GetTerrain()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTerrain_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
