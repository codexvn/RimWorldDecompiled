using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010CF RID: 4303
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x0600691C RID: 26908 RVA: 0x0021248F File Offset: 0x0021068F
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null, DutyDef skipDuty = null, int? minTicks = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
			this.skipDuty = skipDuty;
			this.minTicks = minTicks;
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x002124C7 File Offset: 0x002106C7
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			base.SourceToilBecameActive(transition, previousToil);
			if (this.minTicks != null)
			{
				this.minTick = new int?(GenTicks.TicksGame + this.minTicks.Value);
			}
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x002124FC File Offset: 0x002106FC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				return false;
			}
			if (this.requireInstigatorWithFaction && (signal.dinfo.Instigator == null || signal.dinfo.Instigator.Faction == null))
			{
				return false;
			}
			if (this.requireInstigatorWithSpecificFaction != null && (signal.dinfo.Instigator == null || signal.dinfo.Instigator.Faction != this.requireInstigatorWithSpecificFaction))
			{
				return false;
			}
			Pawn pawn = signal.dinfo.IntendedTarget as Pawn;
			if (pawn != null)
			{
				Pawn_MindState mindState = pawn.mindState;
				DutyDef dutyDef;
				if (mindState == null)
				{
					dutyDef = null;
				}
				else
				{
					PawnDuty duty = mindState.duty;
					dutyDef = ((duty != null) ? duty.def : null);
				}
				if (dutyDef == this.skipDuty)
				{
					return false;
				}
			}
			return (this.minTick == null || GenTicks.TicksGame >= this.minTick.Value) && Rand.Value < this.chance;
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x002125DC File Offset: 0x002107DC
		public static bool SignalIsHarm(TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnDamaged)
			{
				return signal.dinfo.Def.ExternalViolenceFor(signal.Pawn);
			}
			if (signal.type == TriggerSignalType.PawnLost)
			{
				return signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.Killed;
			}
			return signal.type == TriggerSignalType.PawnArrestAttempted;
		}

		// Token: 0x04004A68 RID: 19048
		public float chance = 1f;

		// Token: 0x04004A69 RID: 19049
		public bool requireInstigatorWithFaction;

		// Token: 0x04004A6A RID: 19050
		public Faction requireInstigatorWithSpecificFaction;

		// Token: 0x04004A6B RID: 19051
		public DutyDef skipDuty;

		// Token: 0x04004A6C RID: 19052
		public int? minTicks;

		// Token: 0x04004A6D RID: 19053
		private int? minTick;
	}
}
