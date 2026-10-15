using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public static class CaravanArrivalActionUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0<T> where T : CaravanArrivalAction
	{
		public Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

		public Caravan caravan;

		public PlanetTile pathDestination;

		public Func<T> arrivalActionGetter;

		public Action<Action> confirmActionProxy;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted)
			{
				caravan.pather.StartPath(pathDestination, arrivalActionGetter(), repathImmediately: true);
			}
			else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, historical: false);
			}
		}

		internal void _003CGetFloatMenuOptions_003Eb__2()
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted)
			{
				caravan.Tile = pathDestination;
				caravan.pather.StopDead();
				arrivalActionGetter().Arrived(caravan);
			}
			else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, historical: false);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_1<T> where T : CaravanArrivalAction
	{
		public Action action;

		public _003C_003Ec__DisplayClass0_0<T> CS_0024_003C_003E8__locals1;

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			CS_0024_003C_003E8__locals1.confirmActionProxy(action);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__0<T> : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable where T : CaravanArrivalAction
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

		public Func<FloatMenuAcceptanceReport> _003C_003E3__acceptanceReportGetter;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		private PlanetTile pathDestination;

		public PlanetTile _003C_003E3__pathDestination;

		private Func<T> arrivalActionGetter;

		public Func<T> _003C_003E3__arrivalActionGetter;

		private Action<Action> confirmActionProxy;

		public Action<Action> _003C_003E3__confirmActionProxy;

		private string label;

		public string _003C_003E3__label;

		private WorldObject revalidateWorldClickTarget;

		public WorldObject _003C_003E3__revalidateWorldClickTarget;

		private _003C_003Ec__DisplayClass0_1<T> _003C_003E8__1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass0_0<T> _003C_003Ec__DisplayClass0_ = new _003C_003Ec__DisplayClass0_0<T>
				{
					acceptanceReportGetter = acceptanceReportGetter,
					caravan = caravan,
					pathDestination = pathDestination,
					arrivalActionGetter = arrivalActionGetter,
					confirmActionProxy = confirmActionProxy
				};
				FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = _003C_003Ec__DisplayClass0_.acceptanceReportGetter();
				if (!floatMenuAcceptanceReport2.Accepted && floatMenuAcceptanceReport2.FailReason.NullOrEmpty() && floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
				{
					break;
				}
				if (!floatMenuAcceptanceReport2.FailReason.NullOrEmpty())
				{
					_003C_003E2__current = new FloatMenuOption(label + " (" + floatMenuAcceptanceReport2.FailReason + ")", null);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E8__1 = new _003C_003Ec__DisplayClass0_1<T>();
				_003C_003E8__1.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass0_;
				_003C_003E8__1.action = delegate
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport3 = _003C_003E8__1.CS_0024_003C_003E8__locals1.acceptanceReportGetter();
					if (floatMenuAcceptanceReport3.Accepted)
					{
						_003C_003E8__1.CS_0024_003C_003E8__locals1.caravan.pather.StartPath(_003C_003E8__1.CS_0024_003C_003E8__locals1.pathDestination, _003C_003E8__1.CS_0024_003C_003E8__locals1.arrivalActionGetter(), repathImmediately: true);
					}
					else if (!floatMenuAcceptanceReport3.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport3.FailMessage, new GlobalTargetInfo(_003C_003E8__1.CS_0024_003C_003E8__locals1.pathDestination), MessageTypeDefOf.RejectInput, historical: false);
					}
				};
				_003C_003E2__current = new FloatMenuOption(label, (_003C_003E8__1.CS_0024_003C_003E8__locals1.confirmActionProxy == null) ? _003C_003E8__1.action : ((Action)delegate
				{
					_003C_003E8__1.CS_0024_003C_003E8__locals1.confirmActionProxy(_003C_003E8__1.action);
				}), MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				if (Prefs.DevMode)
				{
					_003C_003E2__current = new FloatMenuOption(label + " (Dev: instantly)", delegate
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport = _003C_003E8__1.CS_0024_003C_003E8__locals1.acceptanceReportGetter();
						if (floatMenuAcceptanceReport.Accepted)
						{
							_003C_003E8__1.CS_0024_003C_003E8__locals1.caravan.Tile = _003C_003E8__1.CS_0024_003C_003E8__locals1.pathDestination;
							_003C_003E8__1.CS_0024_003C_003E8__locals1.caravan.pather.StopDead();
							_003C_003E8__1.CS_0024_003C_003E8__locals1.arrivalActionGetter().Arrived(_003C_003E8__1.CS_0024_003C_003E8__locals1.caravan);
						}
						else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(_003C_003E8__1.CS_0024_003C_003E8__locals1.pathDestination), MessageTypeDefOf.RejectInput, historical: false);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_01e6;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_01e6;
				}
				IL_01e6:
				_003C_003E8__1 = null;
				break;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__0<T> _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__0<T>(0);
			}
			_003CGetFloatMenuOptions_003Ed__.acceptanceReportGetter = _003C_003E3__acceptanceReportGetter;
			_003CGetFloatMenuOptions_003Ed__.arrivalActionGetter = _003C_003E3__arrivalActionGetter;
			_003CGetFloatMenuOptions_003Ed__.label = _003C_003E3__label;
			_003CGetFloatMenuOptions_003Ed__.caravan = _003C_003E3__caravan;
			_003CGetFloatMenuOptions_003Ed__.pathDestination = _003C_003E3__pathDestination;
			_003CGetFloatMenuOptions_003Ed__.revalidateWorldClickTarget = _003C_003E3__revalidateWorldClickTarget;
			_003CGetFloatMenuOptions_003Ed__.confirmActionProxy = _003C_003E3__confirmActionProxy;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__0<>))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, PlanetTile pathDestination, WorldObject revalidateWorldClickTarget, Action<Action> confirmActionProxy = null) where T : CaravanArrivalAction
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__0<T>(-2)
		{
			_003C_003E3__acceptanceReportGetter = acceptanceReportGetter,
			_003C_003E3__arrivalActionGetter = arrivalActionGetter,
			_003C_003E3__label = label,
			_003C_003E3__caravan = caravan,
			_003C_003E3__pathDestination = pathDestination,
			_003C_003E3__revalidateWorldClickTarget = revalidateWorldClickTarget,
			_003C_003E3__confirmActionProxy = confirmActionProxy
		};
	}
}
