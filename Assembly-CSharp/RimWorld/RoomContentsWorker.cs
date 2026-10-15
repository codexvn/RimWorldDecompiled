using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class RoomContentsWorker
{
	[CompilerGenerated]
	private sealed class _003CFillEdgeParms_003Ed__13 : IEnumerable<LayoutFillEdgesParms>, IEnumerable, IEnumerator<LayoutFillEdgesParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutFillEdgesParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutFillEdgesParms>.Enumerator _003C_003E7__wrap2;

		LayoutFillEdgesParms IEnumerator<LayoutFillEdgesParms>.Current
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
		public _003CFillEdgeParms_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutFillEdgesParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.fillEdges.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.fillEdges.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutFillEdgesParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutFillEdgesParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutFillEdgesParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutFillEdgesParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutFillEdgesParms> IEnumerable<LayoutFillEdgesParms>.GetEnumerator()
		{
			_003CFillEdgeParms_003Ed__13 _003CFillEdgeParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFillEdgeParms_003Ed__ = this;
			}
			else
			{
				_003CFillEdgeParms_003Ed__ = new _003CFillEdgeParms_003Ed__13(0);
			}
			_003CFillEdgeParms_003Ed__.room = _003C_003E3__room;
			return _003CFillEdgeParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutFillEdgesParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillInteriorParms_003Ed__14 : IEnumerable<LayoutFillInteriorParms>, IEnumerable, IEnumerator<LayoutFillInteriorParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutFillInteriorParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutFillInteriorParms>.Enumerator _003C_003E7__wrap2;

		LayoutFillInteriorParms IEnumerator<LayoutFillInteriorParms>.Current
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
		public _003CFillInteriorParms_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutFillInteriorParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.fillInterior.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.fillInterior.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutFillInteriorParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutFillInteriorParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutFillInteriorParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutFillInteriorParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutFillInteriorParms> IEnumerable<LayoutFillInteriorParms>.GetEnumerator()
		{
			_003CFillInteriorParms_003Ed__14 _003CFillInteriorParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFillInteriorParms_003Ed__ = this;
			}
			else
			{
				_003CFillInteriorParms_003Ed__ = new _003CFillInteriorParms_003Ed__14(0);
			}
			_003CFillInteriorParms_003Ed__.room = _003C_003E3__room;
			return _003CFillInteriorParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutFillInteriorParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPartParms_003Ed__17 : IEnumerable<LayoutPartParms>, IEnumerable, IEnumerator<LayoutPartParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutPartParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutPartParms>.Enumerator _003C_003E7__wrap2;

		LayoutPartParms IEnumerator<LayoutPartParms>.Current
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
		public _003CPartParms_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutPartParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.parts.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.parts.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutPartParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutPartParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutPartParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutPartParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutPartParms> IEnumerable<LayoutPartParms>.GetEnumerator()
		{
			_003CPartParms_003Ed__17 _003CPartParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPartParms_003Ed__ = this;
			}
			else
			{
				_003CPartParms_003Ed__ = new _003CPartParms_003Ed__17(0);
			}
			_003CPartParms_003Ed__.room = _003C_003E3__room;
			return _003CPartParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutPartParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPrefabParms_003Ed__16 : IEnumerable<LayoutPrefabParms>, IEnumerable, IEnumerator<LayoutPrefabParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutPrefabParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutPrefabParms>.Enumerator _003C_003E7__wrap2;

		LayoutPrefabParms IEnumerator<LayoutPrefabParms>.Current
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
		public _003CPrefabParms_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutPrefabParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.prefabs.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.prefabs.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutPrefabParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutPrefabParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutPrefabParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutPrefabParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutPrefabParms> IEnumerable<LayoutPrefabParms>.GetEnumerator()
		{
			_003CPrefabParms_003Ed__16 _003CPrefabParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPrefabParms_003Ed__ = this;
			}
			else
			{
				_003CPrefabParms_003Ed__ = new _003CPrefabParms_003Ed__16(0);
			}
			_003CPrefabParms_003Ed__.room = _003C_003E3__room;
			return _003CPrefabParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutPrefabParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CScatterParms_003Ed__11 : IEnumerable<LayoutScatterParms>, IEnumerable, IEnumerator<LayoutScatterParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutScatterParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutScatterParms>.Enumerator _003C_003E7__wrap2;

		LayoutScatterParms IEnumerator<LayoutScatterParms>.Current
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
		public _003CScatterParms_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutScatterParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.scatter.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					foreach (LayoutRoomDef def in room.defs)
					{
						if (!def.spawnJunk)
						{
							return false;
						}
					}
					_003C_003E7__wrap2 = room.sketch.layoutDef.junkScaterrers.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutScatterParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutScatterParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutScatterParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutScatterParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutScatterParms> IEnumerable<LayoutScatterParms>.GetEnumerator()
		{
			_003CScatterParms_003Ed__11 _003CScatterParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CScatterParms_003Ed__ = this;
			}
			else
			{
				_003CScatterParms_003Ed__ = new _003CScatterParms_003Ed__11(0);
			}
			_003CScatterParms_003Ed__.room = _003C_003E3__room;
			return _003CScatterParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutScatterParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CScatterTerrainParms_003Ed__12 : IEnumerable<LayoutScatterTerrainParms>, IEnumerable, IEnumerator<LayoutScatterTerrainParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutScatterTerrainParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutScatterTerrainParms>.Enumerator _003C_003E7__wrap2;

		LayoutScatterTerrainParms IEnumerator<LayoutScatterTerrainParms>.Current
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
		public _003CScatterTerrainParms_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
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
				break;
			case -5:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutScatterTerrainParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0097;
				case 2:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.scatterTerrain.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0097;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.scatterTerrain.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutScatterTerrainParms current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutScatterTerrainParms>.Enumerator);
					goto IL_00b6;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutScatterTerrainParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutScatterTerrainParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutScatterTerrainParms> IEnumerable<LayoutScatterTerrainParms>.GetEnumerator()
		{
			_003CScatterTerrainParms_003Ed__12 _003CScatterTerrainParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CScatterTerrainParms_003Ed__ = this;
			}
			else
			{
				_003CScatterTerrainParms_003Ed__ = new _003CScatterTerrainParms_003Ed__12(0);
			}
			_003CScatterTerrainParms_003Ed__.room = _003C_003E3__room;
			return _003CScatterTerrainParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutScatterTerrainParms>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWallAttachmentParms_003Ed__15 : IEnumerable<LayoutWallAttatchmentParms>, IEnumerable, IEnumerator<LayoutWallAttatchmentParms>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LayoutWallAttatchmentParms _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private List<LayoutRoomDef>.Enumerator _003C_003E7__wrap1;

		private List<LayoutWallAttatchmentParms>.Enumerator _003C_003E7__wrap2;

		LayoutWallAttatchmentParms IEnumerator<LayoutWallAttatchmentParms>.Current
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
		public _003CWallAttachmentParms_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 2:
				try
				{
					if (num == -4 || num == 2)
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
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
			_003C_003E7__wrap2 = default(List<LayoutWallAttatchmentParms>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (room.sketch.layoutDef is StructureLayoutDef { wallLampDef: not null } structureLayoutDef)
					{
						_003C_003E2__current = new LayoutWallAttatchmentParms
						{
							def = structureLayoutDef.wallLampDef,
							spawnChancePerPosition = structureLayoutDef.wallLampChancePerPosition
						};
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0084;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f6;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_00f6:
					if (_003C_003E7__wrap2.MoveNext())
					{
						LayoutWallAttatchmentParms current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<LayoutWallAttatchmentParms>.Enumerator);
					goto IL_0115;
					IL_0084:
					_003C_003E7__wrap1 = room.defs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0115;
					IL_0115:
					if (_003C_003E7__wrap1.MoveNext())
					{
						LayoutRoomDef current2 = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current2.wallAttachments.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f6;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<LayoutRoomDef>.Enumerator);
					_003C_003E7__wrap2 = room.sketch.layoutDef.wallAttachments.GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					LayoutWallAttatchmentParms current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<LayoutWallAttatchmentParms>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LayoutWallAttatchmentParms> IEnumerable<LayoutWallAttatchmentParms>.GetEnumerator()
		{
			_003CWallAttachmentParms_003Ed__15 _003CWallAttachmentParms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CWallAttachmentParms_003Ed__ = this;
			}
			else
			{
				_003CWallAttachmentParms_003Ed__ = new _003CWallAttachmentParms_003Ed__15(0);
			}
			_003CWallAttachmentParms_003Ed__.room = _003C_003E3__room;
			return _003CWallAttachmentParms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LayoutWallAttatchmentParms>)this).GetEnumerator();
		}
	}

	private const int DefaultThreatPoints = 300;

	private static readonly List<IntVec3> tmpOccupiedEdges = new List<IntVec3>();

	public LayoutRoomDef RoomDef { get; private set; }

	protected virtual string ThreatSignal { get; set; }

	public void Initialize(LayoutRoomDef roomDef)
	{
		RoomDef = roomDef;
	}

	private bool GenerateRoof(LayoutRoom room)
	{
		if (!RoomDef.noRoof)
		{
			return !room.sketch.layoutDef.noRoof;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CScatterParms_003Ed__11))]
	private IEnumerable<LayoutScatterParms> ScatterParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CScatterParms_003Ed__11(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CScatterTerrainParms_003Ed__12))]
	private IEnumerable<LayoutScatterTerrainParms> ScatterTerrainParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CScatterTerrainParms_003Ed__12(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CFillEdgeParms_003Ed__13))]
	private IEnumerable<LayoutFillEdgesParms> FillEdgeParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFillEdgeParms_003Ed__13(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CFillInteriorParms_003Ed__14))]
	private IEnumerable<LayoutFillInteriorParms> FillInteriorParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFillInteriorParms_003Ed__14(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CWallAttachmentParms_003Ed__15))]
	private IEnumerable<LayoutWallAttatchmentParms> WallAttachmentParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWallAttachmentParms_003Ed__15(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CPrefabParms_003Ed__16))]
	private IEnumerable<LayoutPrefabParms> PrefabParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPrefabParms_003Ed__16(-2)
		{
			_003C_003E3__room = room
		};
	}

	[IteratorStateMachine(typeof(_003CPartParms_003Ed__17))]
	private IEnumerable<LayoutPartParms> PartParms(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPartParms_003Ed__17(-2)
		{
			_003C_003E3__room = room
		};
	}

	public virtual void PreFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
	}

	public virtual void PostFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
		TrySpawnParts(map, room, faction, threatPoints, post: true);
	}

	public virtual void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
		if (RoomDef != null)
		{
			TrySetRoof(map, room);
			RemoveWalls(map, room);
			TryScatterTerrain(map, room);
			TrySpawnParts(map, room, faction, threatPoints, post: false);
			TryPlacePrefabs(map, room, faction);
			TryFillContents(map, room, faction);
			TryScatterContents(map, room, faction);
			TryPlaceWallAttachments(map, room, faction);
		}
	}

	private void TrySetRoof(Map map, LayoutRoom room)
	{
		if (!GenerateRoof(room))
		{
			return;
		}
		foreach (CellRect rect in room.rects)
		{
			foreach (IntVec3 item in rect)
			{
				SpawnRoof(map, item);
			}
		}
	}

	protected void SpawnRoof(Map map, IntVec3 cell)
	{
		RoofDef roof = cell.GetRoof(map);
		if (roof == null || !roof.isNatural)
		{
			map.roofGrid.SetRoof(cell, RoomDef.roofDef ?? RoofDefOf.RoofConstructed);
		}
	}

	private void RemoveWalls(Map map, LayoutRoom room)
	{
		if (!CanRemoveWalls(room))
		{
			return;
		}
		foreach (CellRect rect in room.rects)
		{
			foreach (IntVec3 edgeCell in rect.EdgeCells)
			{
				if (!CanRemoveWall(edgeCell, map, room))
				{
					continue;
				}
				bool flag = true;
				for (int i = 0; i < 4; i++)
				{
					if ((edgeCell + GenAdj.CardinalDirections[i]).GetDoor(map) != null)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				foreach (LayoutRoom room2 in room.sketch.structureLayout.Rooms)
				{
					if (!CanRemoveWalls(room2) && room2.Contains(edgeCell))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					Building building = map.edificeGrid[edgeCell];
					if (building != null && building.def.IsWall)
					{
						building.Destroy();
						map.roofGrid.SetRoof(edgeCell, null);
					}
				}
			}
		}
	}

	private void TryScatterTerrain(Map map, LayoutRoom room)
	{
		float num = (float)room.Area / 100f;
		foreach (LayoutScatterTerrainParms item in ScatterTerrainParms(room))
		{
			int a = ((!(item.groupCount != IntRange.Invalid)) ? Mathf.RoundToInt(item.groupsPerHundredCells.RandomInRange * num) : item.groupCount.RandomInRange);
			a = Mathf.Max(a, item.minGroups);
			for (int i = 0; i < a; i++)
			{
				SpawnIrregularTerrainLump(item, map, room);
			}
		}
	}

	private void TrySpawnParts(Map map, LayoutRoom room, Faction faction, float? threatPoints, bool post)
	{
		float num = ((!threatPoints.HasValue) ? 300f : (RoomDef.threatPointsScaleCurve?.Evaluate(threatPoints.Value) ?? threatPoints.Value));
		foreach (LayoutPartParms item in PartParms(room))
		{
			int num2 = 1;
			if (item.def.Worker.FillOnPost == post)
			{
				if (item.countRange != IntRange.Invalid)
				{
					num2 = item.countRange.RandomInRange;
				}
				else if (!Rand.Chance(item.chance))
				{
					continue;
				}
				float threatPoints2 = num;
				if (item.threatPointsRange != IntRange.Invalid)
				{
					threatPoints2 = item.threatPointsRange.RandomInRange;
				}
				for (int i = 0; i < num2; i++)
				{
					item.def.Worker.FillRoom(map, room, faction, threatPoints2);
				}
			}
		}
	}

	private void TryPlacePrefabs(Map map, LayoutRoom room, Faction faction)
	{
		LayoutSketch layoutSketch = room.sketch.layoutSketch;
		float num = (float)room.Area / 100f;
		float num2 = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
		Tuple<ThingDef, ThingDef> wallTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.wall, layoutSketch.wallStuff);
		Tuple<ThingDef, ThingDef> doorTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.door, layoutSketch.doorStuff);
		Dictionary<LayoutPrefabParms, int> dictionary = new Dictionary<LayoutPrefabParms, int>();
		List<LayoutPrefabParms> list = new List<LayoutPrefabParms>();
		foreach (LayoutPrefabParms item in PrefabParms(room))
		{
			list.Add(item);
			int a = ((!(item.countRange != IntRange.Invalid)) ? ((!item.def.edgeOnly) ? Mathf.RoundToInt(item.countPerHundredCells.RandomInRange * num) : Mathf.RoundToInt(item.countPerTenEdgeCells.RandomInRange * num2)) : item.countRange.RandomInRange);
			a = Mathf.Min(Mathf.Max(a, item.minMaxRange.TrueMin), item.minMaxRange.TrueMax);
			dictionary[item] = a;
		}
		int num3 = 0;
		while (dictionary.Any())
		{
			LayoutPrefabParms parms = list[num3];
			num3 = ++num3 % list.Count;
			if (dictionary.ContainsKey(parms))
			{
				if (--dictionary[parms] <= 0)
				{
					dictionary.Remove(parms);
				}
				if (parms.def.edgeOnly)
				{
					RoomGenUtility.FillPrefabsAroundEdges(parms, 1, room, map, Validator, null, 1, avoidDoors: true, faction, OverrideSpawnData);
				}
				else
				{
					RoomGenUtility.FillPrefabs(parms, 1, room, map, Validator, 1, null, alignWithRect: false, snapToGrid: false, faction, OverrideSpawnData);
				}
			}
			bool Validator(IntVec3 cell, Rot4 rot)
			{
				return IsValidPrefabRect(parms, cell, rot, room, map);
			}
		}
		Tuple<ThingDef, ThingDef> OverrideSpawnData(PrefabThingData data)
		{
			if (data.def.IsWall)
			{
				return wallTuple;
			}
			if (data.def.IsDoor)
			{
				return doorTuple;
			}
			return null;
		}
	}

	private void TryPlaceWallAttachments(Map map, LayoutRoom room, Faction faction)
	{
		foreach (LayoutWallAttatchmentParms item in WallAttachmentParms(room))
		{
			LayoutWallAttatchmentParms parms = item;
			if (parms.countRange != IntRange.Invalid)
			{
				RoomGenUtility.SpawnWallAttatchments(parms.def, map, room, parms.countRange, Validator, parms.stuff, faction);
			}
			else
			{
				RoomGenUtility.SpawnWallAttatchments(parms.def, map, room, parms.spawnChancePerPosition, parms.thingsPer10EdgeCells.RandomInRange, Validator, parms.stuff, faction);
			}
			bool Validator(IntVec3 cell, Rot4 rot)
			{
				return IsValidWallAttachmentCell(parms, cell, rot, room, map);
			}
		}
	}

	private void TryFillContents(Map map, LayoutRoom room, Faction faction)
	{
		foreach (LayoutFillEdgesParms item in FillEdgeParms(room))
		{
			LayoutFillEdgesParms parms = item;
			if (parms.countRange != IntRange.Invalid)
			{
				RoomGenUtility.FillAroundEdges(parms.def, parms.countRange.RandomInRange, parms.groupCountRange, room, map, Validator, null, stuff: parms.stuff, padding: parms.padding, contractedBy: parms.contractedBy, avoidDoors: true, rotationDirectionOffset: parms.rotOffset, spawnAction: null, faction: faction);
				continue;
			}
			RoomGenUtility.FillAroundEdges(parms.def, parms.groupsPerTenEdgeCells, parms.groupCountRange, room, map, Validator, null, stuff: parms.stuff, padding: parms.padding, contractedBy: parms.contractedBy, avoidDoors: true, rotationDirectionOffset: parms.rotOffset, spawnAction: null, faction: faction);
			bool Validator(IntVec3 c, Rot4 rot, CellRect rect)
			{
				return IsValidFillEdgeCell(parms, c, rot, room, map);
			}
		}
		foreach (LayoutFillInteriorParms item2 in FillInteriorParms(room))
		{
			LayoutFillInteriorParms parms2 = item2;
			if (parms2.countRange != IntRange.Invalid)
			{
				RoomGenUtility.FillWithPadding(parms2.def, parms2.countRange.RandomInRange, room, map, parms2.fixedRot, Validator, null, stuff: parms2.stuff, contractedBy: parms2.contractedBy, alignWithRect: parms2.alignWithRect, snapToGrid: parms2.snapToGrid, spawnAction: null, faction: faction);
				continue;
			}
			RoomGenUtility.FillWithPadding(parms2.def, parms2.thingsPerHundredCells, room, map, parms2.fixedRot, Validator, null, stuff: parms2.stuff, contractedBy: parms2.contractedBy, alignWithRect: parms2.alignWithRect, snapToGrid: parms2.snapToGrid, spawnAction: null, faction: faction);
			bool Validator(IntVec3 c, Rot4 rot, CellRect rect)
			{
				return IsValidFillInteriorCell(parms2, c, rot, room, map);
			}
		}
	}

	private void TryScatterContents(Map map, LayoutRoom room, Faction faction)
	{
		float num = (float)room.Area / 100f;
		foreach (LayoutScatterParms item in ScatterParms(room))
		{
			int a = ((!(item.groupCount != IntRange.Invalid)) ? Mathf.RoundToInt(item.groupsPerHundredCells.RandomInRange * num) : item.groupCount.RandomInRange);
			a = Mathf.Max(a, item.minGroups);
			for (int i = 0; i < a; i++)
			{
				SpawnIrregularLump(item, map, room, faction);
			}
		}
	}

	private void SpawnIrregularLump(LayoutScatterParms parms, Map map, LayoutRoom room, Faction faction, List<IntVec3> area = null, List<Thing> spawned = null)
	{
		if (room.TryGetRandomCellInRoom(map, out var cell, 2, 0, (IntVec3 c) => IsValidScatterCell(parms, c, room, map)))
		{
			GenSpawn.SpawnIrregularLump(parms.def, cell, map, parms.itemsPerGroup, parms.groupDistRange, WipeMode.Vanish, (IntVec3 c) => IsValidScatterCell(parms, c, room, map), area, spawned, parms.stuff, faction);
		}
	}

	private void SpawnIrregularTerrainLump(LayoutScatterTerrainParms parms, Map map, LayoutRoom room, List<IntVec3> area = null)
	{
		if (!room.TryGetRandomCellInRoom(map, out var cell, 2, 0, Validator))
		{
			return;
		}
		int randomInRange = parms.groupDistRange.RandomInRange;
		int randomInRange2 = parms.itemsPerGroup.RandomInRange;
		int num = 0;
		List<IntVec3> list = GridShapeMaker.IrregularLump(cell, map, randomInRange, Validator);
		for (int i = 0; i < list.Count; i++)
		{
			IntVec3 intVec = list[i];
			if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
			{
				num++;
				map.terrainGrid.SetTerrain(intVec, parms.def);
				area?.Add(intVec);
			}
		}
		bool Validator(IntVec3 c)
		{
			return room.Contains(c);
		}
	}

	protected virtual bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
	{
		TerrainAffordanceDef terrainAffordanceNeed = thing.GetTerrainAffordanceNeed(stuff);
		if (room.Contains(cell, 1) && !cell.IsBuildingInteractionCell(map) && (terrainAffordanceNeed == null || cell.GetAffordances(map).Contains(terrainAffordanceNeed)) && (thing.passability != Traversability.Impassable || cell.GetFirstPawn(map) == null) && cell.GetFirstBuilding(map) == null && (thing.IsFilth || !RoomGenUtility.IsDoorAdjacentTo(cell, map)))
		{
			if (cell.GetTerrain(map).exposesToVacuum)
			{
				return cell.GetRoof(map) != null;
			}
			return true;
		}
		return false;
	}

	protected virtual bool IsValidScatterCell(LayoutScatterParms parms, IntVec3 cell, LayoutRoom room, Map map)
	{
		if (parms.def.passability == Traversability.Impassable && !RoomGenUtility.IsClearAndNotAdjacentToDoor(parms.def, cell, map, Rot4.North))
		{
			return false;
		}
		foreach (Thing thing in cell.GetThingList(map))
		{
			if (parms.def.IsFilth && thing.def.IsFilth)
			{
				return false;
			}
			if (!parms.def.IsFilth && !thing.def.IsFilth)
			{
				return false;
			}
		}
		return IsValidCellBase(parms.def, parms.stuff, cell, room, map);
	}

	protected virtual bool IsValidFillEdgeCell(LayoutFillEdgesParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
	{
		CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
		foreach (IntVec3 item in cellRect)
		{
			if (!IsValidCellBase(parms.def, parms.stuff, item, room, map))
			{
				return false;
			}
		}
		if (parms.def.Fillage != FillCategory.Full)
		{
			return true;
		}
		foreach (IntVec3 edgeCell in cellRect.ExpandedBy(1).EdgeCells)
		{
			if (room.Contains(edgeCell, 2) && !edgeCell.Standable(map))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual bool IsValidPrefabRect(LayoutPrefabParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
	{
		CellRect cellRect = cell.RectAbout(parms.def.size, rot);
		tmpOccupiedEdges.Clear();
		foreach (var (prefabThingData, center, rot2) in PrefabUtility.GetThings(parms.def, cell, rot))
		{
			foreach (IntVec3 cell2 in center.RectAbout(prefabThingData.def.Size, rot2).Cells)
			{
				if (!IsValidCellBase(prefabThingData.def, prefabThingData.stuff, cell2, room, map))
				{
					return false;
				}
				if (!cellRect.EdgeCells.Contains(cell2) || cell2.Standable(map))
				{
					continue;
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = cell2 + GenAdj.AdjacentCellsAround[i];
					if (!cellRect.Contains(intVec) && !intVec.Standable(map) && !cellRect.ExpandedBy(1).IsCorner(intVec) && !tmpOccupiedEdges.Contains(intVec))
					{
						tmpOccupiedEdges.Add(intVec);
					}
				}
			}
		}
		return true;
	}

	protected virtual bool IsValidFillInteriorCell(LayoutFillInteriorParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
	{
		CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
		foreach (IntVec3 item in cellRect)
		{
			if (!IsValidCellBase(parms.def, parms.stuff, item, room, map))
			{
				return false;
			}
		}
		if (parms.def.Fillage != FillCategory.Full)
		{
			return true;
		}
		foreach (IntVec3 edgeCell in cellRect.ExpandedBy(1).EdgeCells)
		{
			if (room.Contains(edgeCell, 2) && !edgeCell.Standable(map))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
	{
		if ((cell + rot.FacingCell).GetEdifice(map)?.def != room.sketch.layoutSketch.wall)
		{
			return false;
		}
		return true;
	}

	protected virtual bool CanRemoveWall(IntVec3 cell, Map map, LayoutRoom room)
	{
		return true;
	}

	private static bool CanRemoveWalls(LayoutRoom room)
	{
		foreach (LayoutRoomDef def in room.defs)
		{
			if (!def.canRemoveBorderWalls)
			{
				return false;
			}
		}
		return true;
	}
}
