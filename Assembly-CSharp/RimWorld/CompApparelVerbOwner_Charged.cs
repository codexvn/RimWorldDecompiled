using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using Verse;

namespace RimWorld;

public class CompApparelVerbOwner_Charged : CompApparelVerbOwner, ICompWithCharges
{
	[CompilerGenerated]
	private sealed class _003CCompGetWornGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompApparelVerbOwner_Charged _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetWornGizmosExtra_003Ed__15(int _003C_003E1__state)
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
				CompApparelVerbOwner_Charged CS_0024_003C_003E8__locals3 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompApparelVerbOwner)CS_0024_003C_003E8__locals3).CompGetWornGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Reload to full",
							action = delegate
							{
								CS_0024_003C_003E8__locals3.remainingCharges = CS_0024_003C_003E8__locals3.MaxCharges;
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetWornGizmosExtra_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetWornGizmosExtra_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__13 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompApparelVerbOwner_Charged _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__13(int _003C_003E1__state)
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
				CompApparelVerbOwner_Charged compApparelVerbOwner_Charged = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<StatDrawEntry> enumerable = ((ThingComp)compApparelVerbOwner_Charged).SpecialDisplayStats();
					if (enumerable != null)
					{
						_003C_003E7__wrap1 = enumerable.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0079;
					}
					goto IL_0093;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0079;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0079:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_0093;
					IL_0093:
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadChargesRemaining_Name".Translate(compApparelVerbOwner_Charged.Props.ChargeNounArgument), compApparelVerbOwner_Charged.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(compApparelVerbOwner_Charged.Props.ChargeNounArgument), 2749);
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	protected int remainingCharges;

	public new CompProperties_ApparelVerbOwnerCharged Props => props as CompProperties_ApparelVerbOwnerCharged;

	public int RemainingCharges => remainingCharges;

	public int MaxCharges => Props.maxCharges;

	public string LabelRemaining => $"{RemainingCharges} / {MaxCharges}";

	public override string GizmoExtraLabel => LabelRemaining;

	public override void PostPostMake()
	{
		base.PostPostMake();
		remainingCharges = MaxCharges;
	}

	public override string CompInspectStringExtra()
	{
		return "ChargesRemaining".Translate(Props.ChargeNounArgument) + ": " + LabelRemaining;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__13))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref remainingCharges, "remainingCharges", -999);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && remainingCharges == -999)
		{
			remainingCharges = MaxCharges;
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetWornGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetWornGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompTipStringExtra()
	{
		TaggedString value = "Stat_Thing_ReloadChargesRemaining_Name".Translate(Props.ChargeNounArgument).CapitalizeFirst();
		return $"\n\n{value}: {RemainingCharges} / {MaxCharges}";
	}

	public override void UsedOnce()
	{
		base.UsedOnce();
		if (remainingCharges > 0)
		{
			remainingCharges--;
		}
		if (Props.destroyOnEmpty && remainingCharges == 0 && !parent.Destroyed)
		{
			parent.Destroy();
		}
	}
}
