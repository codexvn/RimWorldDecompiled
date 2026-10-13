using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D2B RID: 11563
	public class CompMechanoid : CompCanBeDormant
	{
		// Token: 0x17002946 RID: 10566
		// (get) Token: 0x060103DD RID: 66525 RVA: 0x004B20F8 File Offset: 0x004B02F8
		public bool Deactivated
		{
			get
			{
				if (this.deactivated)
				{
					return true;
				}
				Faction ofMechanoids = Faction.OfMechanoids;
				return ofMechanoids != null && ofMechanoids.deactivated && (this.parent.Faction == Faction.OfMechanoids || this.parent.Faction == null);
			}
		}

		// Token: 0x17002947 RID: 10567
		// (get) Token: 0x060103DE RID: 66526 RVA: 0x004B2146 File Offset: 0x004B0346
		protected override bool ShowZs
		{
			get
			{
				return !this.Deactivated && base.Props.showSleepingZs && (base.Props.delayedWakeUpDoesZs || this.wakeUpOnTick == int.MinValue);
			}
		}

		// Token: 0x17002948 RID: 10568
		// (get) Token: 0x060103DF RID: 66527 RVA: 0x004B217C File Offset: 0x004B037C
		protected override JobDef SleepJob
		{
			get
			{
				Faction faction = this.parent.Faction;
				if (faction == null || !faction.deactivated)
				{
					return base.SleepJob;
				}
				return JobDefOf.Deactivated;
			}
		}

		// Token: 0x17002949 RID: 10569
		// (get) Token: 0x060103E0 RID: 66528 RVA: 0x0048AE48 File Offset: 0x00489048
		private Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x1700294A RID: 10570
		// (get) Token: 0x060103E1 RID: 66529 RVA: 0x004992E2 File Offset: 0x004974E2
		private Building Building
		{
			get
			{
				return this.parent as Building;
			}
		}

		// Token: 0x060103E2 RID: 66530 RVA: 0x004B21AC File Offset: 0x004B03AC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", true, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			Scribe_References.Look<Thing>(ref this.chargerBeforeTakeoff, "chargerBeforeTakeoff", false);
		}

		// Token: 0x060103E3 RID: 66531 RVA: 0x004B21E9 File Offset: 0x004B03E9
		public override void CompTick()
		{
			base.CompTick();
			if (this.Deactivated && this.active)
			{
				this.ToSleep();
			}
		}

		// Token: 0x060103E4 RID: 66532 RVA: 0x004B2208 File Offset: 0x004B0408
		public void Deactivate()
		{
			this.deactivated = true;
			this.ToSleep();
			this.Pawn.health.CheckForStateChange(null, null);
		}

		// Token: 0x060103E5 RID: 66533 RVA: 0x004B223C File Offset: 0x004B043C
		public override void ToSleep()
		{
			base.ToSleep();
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Pawn pawn2 = this.Pawn;
			if (pawn2 != null)
			{
				Lord lord = pawn2.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.Pawn, PawnLostCondition.Incapped, null);
				}
			}
			Building building = this.Building;
			if (building != null)
			{
				Lord lord2 = building.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_BuildingLost(this.Building, null);
				}
			}
			this.active = false;
		}

		// Token: 0x060103E6 RID: 66534 RVA: 0x004B22C7 File Offset: 0x004B04C7
		public override void WakeUp()
		{
			if (this.Deactivated)
			{
				return;
			}
			base.WakeUp();
			this.active = true;
		}

		// Token: 0x060103E7 RID: 66535 RVA: 0x004B22DF File Offset: 0x004B04DF
		public override void PreSwapMap()
		{
			Pawn_NeedsTracker needs = this.Pawn.needs;
			Thing thing;
			if (needs == null)
			{
				thing = null;
			}
			else
			{
				Need_MechEnergy energy = needs.energy;
				thing = ((energy != null) ? energy.currentCharger : null);
			}
			this.chargerBeforeTakeoff = thing;
		}

		// Token: 0x060103E8 RID: 66536 RVA: 0x004B230C File Offset: 0x004B050C
		public override void PostSwapMap()
		{
			if (this.chargerBeforeTakeoff != null)
			{
				Job newJob = JobMaker.MakeJob(JobDefOf.MechCharge, this.chargerBeforeTakeoff);
				this.Pawn.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				this.chargerBeforeTakeoff = null;
			}
		}

		// Token: 0x060103E9 RID: 66537 RVA: 0x004B236C File Offset: 0x004B056C
		public override string CompInspectStringExtra()
		{
			if (this.Deactivated)
			{
				return "Deactivated".Translate() + ".";
			}
			return null;
		}

		// Token: 0x0400A9A1 RID: 43425
		private bool active = true;

		// Token: 0x0400A9A2 RID: 43426
		private bool deactivated;

		// Token: 0x0400A9A3 RID: 43427
		private Thing chargerBeforeTakeoff;
	}
}
