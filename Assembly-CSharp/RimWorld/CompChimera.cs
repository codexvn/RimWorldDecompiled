using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class CompChimera : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__9 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompChimera _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__9(int _003C_003E1__state)
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
				CompChimera CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals5).CompGetGizmosExtra().GetEnumerator();
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
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Switch stalk/attack mode",
						action = delegate
						{
							Lord lord = CS_0024_003C_003E8__locals5.Pawn.GetLord();
							if (lord == null)
							{
								lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ChimeraAssault(), CS_0024_003C_003E8__locals5.Pawn.Map, new List<Pawn> { CS_0024_003C_003E8__locals5.Pawn });
								CS_0024_003C_003E8__locals5.Pawn.SetFaction(Faction.OfEntities);
							}
							if (lord.LordJob is LordJob_ChimeraAssault lordJob_ChimeraAssault)
							{
								lordJob_ChimeraAssault.SwitchMode();
							}
						}
					};
					_003C_003E1__state = 2;
					return true;
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
			_003CCompGetGizmosExtra_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__9(0)
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

	private float totalDamageTaken;

	public CompProperties_Chimera Props => (CompProperties_Chimera)props;

	public Pawn Pawn => (Pawn)parent;

	public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		if (Pawn.Dead)
		{
			return;
		}
		totalDamageTaken += totalDamageDealt;
		if (!Pawn.Dead && totalDamageTaken > 0f && !Pawn.health.hediffSet.HasHediff(HediffDefOf.RageSpeed) && !Pawn.health.Downed)
		{
			Pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.RageSpeed, Pawn), null, null);
			if (Pawn.Spawned)
			{
				EffecterDefOf.ChimeraRage.Spawn(Pawn.Position, Pawn.Map).Cleanup();
			}
		}
	}

	public override void Notify_Downed()
	{
		Hediff firstHediffOfDef = Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed);
		if (firstHediffOfDef != null)
		{
			Pawn.health.RemoveHediff(firstHediffOfDef);
		}
	}

	public override void CompTickRare()
	{
		base.CompTickRare();
		if (!Pawn.Dead && Pawn.health.summaryHealth.SummaryHealthPercent >= Props.rageEndHealthPercentThreshold)
		{
			Hediff firstHediffOfDef = Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed);
			if (firstHediffOfDef != null)
			{
				Pawn.health.RemoveHediff(firstHediffOfDef);
				totalDamageTaken = 0f;
			}
		}
	}

	public Thought_MemoryObservation GiveObservedThought(Pawn observer)
	{
		Thought_MemoryObservation_Chimera obj = (Thought_MemoryObservation_Chimera)ThoughtMaker.MakeThought(ThoughtDefOf.Chimera);
		obj.Target = Pawn;
		return obj;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__9))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref totalDamageTaken, "totalDamageTaken", 0f);
	}
}
