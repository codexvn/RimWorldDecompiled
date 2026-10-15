using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_Altar : RitualObligationTargetFilter
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualObligationTargetWorker_Altar _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CGetTargetInfos_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualObligationTargetWorker_Altar ritualObligationTargetWorker_Altar = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = GetTargetInfosWorker(ritualObligationTargetWorker_Altar.parent.ideo).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetTargetInfos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTargetInfos_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargetInfosWorker_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Ideo ideo;

		public Ideo _003C_003E3__ideo;

		private int _003Ci_003E5__2;

		string IEnumerator<string>.Current
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
		public _003CGetTargetInfosWorker_003Ed__7(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0068;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0078;
			IL_0068:
			_003Ci_003E5__2++;
			goto IL_0078;
			IL_0078:
			if (_003Ci_003E5__2 < ideo.PreceptsListForReading.Count)
			{
				if (ideo.PreceptsListForReading[_003Ci_003E5__2] is Precept_Building precept_Building && precept_Building.ThingDef.isAltar)
				{
					_003C_003E2__current = precept_Building.LabelCap;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0068;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetTargetInfosWorker_003Ed__7 _003CGetTargetInfosWorker_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargetInfosWorker_003Ed__ = this;
			}
			else
			{
				_003CGetTargetInfosWorker_003Ed__ = new _003CGetTargetInfosWorker_003Ed__7(0);
			}
			_003CGetTargetInfosWorker_003Ed__.ideo = _003C_003E3__ideo;
			return _003CGetTargetInfosWorker_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargets_003Ed__2 : IEnumerable<TargetInfo>, IEnumerable, IEnumerator<TargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualObligationTargetWorker_Altar _003C_003E4__this;

		private RitualObligation obligation;

		public RitualObligation _003C_003E3__obligation;

		private Map map;

		public Map _003C_003E3__map;

		private IEnumerator<TargetInfo> _003C_003E7__wrap1;

		TargetInfo IEnumerator<TargetInfo>.Current
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
		public _003CGetTargets_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualObligationTargetWorker_Altar ritualObligationTargetWorker_Altar = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!ModLister.CheckIdeology("Altar target"))
					{
						return false;
					}
					Ideo ideo = ritualObligationTargetWorker_Altar.parent.ideo;
					_003C_003E7__wrap1 = GetTargetsWorker(obligation, map, ideo).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					TargetInfo current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<TargetInfo> IEnumerable<TargetInfo>.GetEnumerator()
		{
			_003CGetTargets_003Ed__2 _003CGetTargets_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargets_003Ed__ = this;
			}
			else
			{
				_003CGetTargets_003Ed__ = new _003CGetTargets_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTargets_003Ed__.obligation = _003C_003E3__obligation;
			_003CGetTargets_003Ed__.map = _003C_003E3__map;
			return _003CGetTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargetsWorker_003Ed__3 : IEnumerable<TargetInfo>, IEnumerable, IEnumerator<TargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Ideo ideo;

		public Ideo _003C_003E3__ideo;

		private Map map;

		public Map _003C_003E3__map;

		private int _003Ci_003E5__2;

		private IEnumerator<Thing> _003C_003E7__wrap2;

		TargetInfo IEnumerator<TargetInfo>.Current
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
		public _003CGetTargetsWorker_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00a8;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00d4;
				IL_00d4:
				if (_003Ci_003E5__2 < ideo.PreceptsListForReading.Count)
				{
					if (ideo.PreceptsListForReading[_003Ci_003E5__2] is Precept_Building precept_Building && precept_Building.ThingDef.isAltar)
					{
						_003C_003E7__wrap2 = precept_Building.presenceDemand.AllBuildings(map).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a8;
					}
					goto IL_00c2;
				}
				return false;
				IL_00a8:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Thing current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				goto IL_00c2;
				IL_00c2:
				_003Ci_003E5__2++;
				goto IL_00d4;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<TargetInfo> IEnumerable<TargetInfo>.GetEnumerator()
		{
			_003CGetTargetsWorker_003Ed__3 _003CGetTargetsWorker_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargetsWorker_003Ed__ = this;
			}
			else
			{
				_003CGetTargetsWorker_003Ed__ = new _003CGetTargetsWorker_003Ed__3(0);
			}
			_003CGetTargetsWorker_003Ed__.map = _003C_003E3__map;
			_003CGetTargetsWorker_003Ed__.ideo = _003C_003E3__ideo;
			return _003CGetTargetsWorker_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	public RitualObligationTargetWorker_Altar()
	{
	}

	public RitualObligationTargetWorker_Altar(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__2))]
	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__obligation = obligation,
			_003C_003E3__map = map
		};
	}

	[IteratorStateMachine(typeof(_003CGetTargetsWorker_003Ed__3))]
	public static IEnumerable<TargetInfo> GetTargetsWorker(RitualObligation obligation, Map map, Ideo ideo)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetsWorker_003Ed__3(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__ideo = ideo
		};
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		return CanUseTargetWorker(target, obligation, parent.ideo);
	}

	public static bool CanUseTargetWorker(TargetInfo target, RitualObligation obligation, Ideo ideo)
	{
		if (!(target.Thing is Building { Faction: not null } building) || !building.Faction.IsPlayer)
		{
			return false;
		}
		if (!GetTargetsWorker(obligation, building.Map, ideo).Contains(building))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__6))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfosWorker_003Ed__7))]
	public static IEnumerable<string> GetTargetInfosWorker(Ideo ideo)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfosWorker_003Ed__7(-2)
		{
			_003C_003E3__ideo = ideo
		};
	}

	public override List<string> MissingTargetBuilding(Ideo ideo)
	{
		if (!GetTargetInfos(null).Any())
		{
			return new List<string> { "Altar".Translate() };
		}
		return null;
	}
}
