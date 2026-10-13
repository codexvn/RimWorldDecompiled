using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F07 RID: 12039
	public abstract class CompScanner : ThingComp
	{
		// Token: 0x17002BE0 RID: 11232
		// (get) Token: 0x06010FF8 RID: 69624 RVA: 0x004E3E83 File Offset: 0x004E2083
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		// Token: 0x17002BE1 RID: 11233
		// (get) Token: 0x06010FF9 RID: 69625 RVA: 0x004E3E90 File Offset: 0x004E2090
		public virtual AcceptanceReport CanUseNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (this.powerComp != null && !this.powerComp.PowerOn)
				{
					return false;
				}
				if (RoofUtility.IsAnyCellUnderRoof(this.parent))
				{
					return "CannotUseScannerRoofed".Translate();
				}
				if (this.forbiddable != null && this.forbiddable.Forbidden)
				{
					return false;
				}
				return this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06010FFA RID: 69626 RVA: 0x004E3F20 File Offset: 0x004E2120
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		// Token: 0x06010FFB RID: 69627 RVA: 0x004E3F75 File Offset: 0x004E2175
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		// Token: 0x06010FFC RID: 69628 RVA: 0x004E3FA0 File Offset: 0x004E21A0
		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.");
			}
			this.lastScanTick = (float)Find.TickManager.TicksGame;
			this.lastUserSpeed = 1f;
			if (this.Props.scanSpeedStat != null)
			{
				this.lastUserSpeed = worker.GetStatValue(this.Props.scanSpeedStat, true, -1);
			}
			this.daysWorkingSinceLastFinding += this.lastUserSpeed / 60000f;
			if (this.TickDoesFind(this.lastUserSpeed))
			{
				this.DoFind(worker);
				this.daysWorkingSinceLastFinding = 0f;
			}
		}

		// Token: 0x06010FFD RID: 69629 RVA: 0x004E4040 File Offset: 0x004E2240
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return this.parent.IsHashIntervalTick(59) && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		// Token: 0x06010FFE RID: 69630 RVA: 0x004E40A4 File Offset: 0x004E22A4
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		// Token: 0x06010FFF RID: 69631
		protected abstract void DoFind(Pawn worker);

		// Token: 0x06011000 RID: 69632 RVA: 0x004E419B File Offset: 0x004E239B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Find now",
					action = delegate()
					{
						this.DoFind(PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>());
					}
				};
			}
			yield break;
		}

		// Token: 0x0400B0BD RID: 45245
		protected float daysWorkingSinceLastFinding;

		// Token: 0x0400B0BE RID: 45246
		protected float lastUserSpeed = 1f;

		// Token: 0x0400B0BF RID: 45247
		protected float lastScanTick = -1f;

		// Token: 0x0400B0C0 RID: 45248
		protected CompPowerTrader powerComp;

		// Token: 0x0400B0C1 RID: 45249
		protected CompForbiddable forbiddable;
	}
}
