using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C3E RID: 11326
	public class CompCauseHediff_AoE : ThingComp
	{
		// Token: 0x170027F6 RID: 10230
		// (get) Token: 0x0600FDF1 RID: 65009 RVA: 0x0049A6D1 File Offset: 0x004988D1
		public CompProperties_CauseHediff_AoE Props
		{
			get
			{
				return (CompProperties_CauseHediff_AoE)this.props;
			}
		}

		// Token: 0x170027F7 RID: 10231
		// (get) Token: 0x0600FDF2 RID: 65010 RVA: 0x00493805 File Offset: 0x00491A05
		private CompPowerTrader PowerTrader
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x0600FDF3 RID: 65011 RVA: 0x0049A6E0 File Offset: 0x004988E0
		private bool IsPawnAffected(Pawn target)
		{
			return (this.PowerTrader == null || this.PowerTrader.PowerOn) && !target.Dead && target.health != null && (target != this.parent || this.Props.canTargetSelf) && (!this.Props.ignoreMechs || !target.RaceProps.IsMechanoid) && (!this.Props.onlyTargetMechs || target.RaceProps.IsMechanoid) && target.PositionHeld.DistanceTo(this.parent.PositionHeld) <= this.range;
		}

		// Token: 0x0600FDF4 RID: 65012 RVA: 0x0049A78A File Offset: 0x0049898A
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.range = this.Props.range;
		}

		// Token: 0x0600FDF5 RID: 65013 RVA: 0x0049A7A4 File Offset: 0x004989A4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.range, "range", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.range <= 0f)
			{
				this.range = this.Props.range;
			}
		}

		// Token: 0x0600FDF6 RID: 65014 RVA: 0x0049A7F4 File Offset: 0x004989F4
		public override void CompTick()
		{
			this.MaintainSustainer();
			if (!this.parent.IsHashIntervalTick(this.Props.checkInterval))
			{
				return;
			}
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null && !compPowerTrader.PowerOn)
			{
				return;
			}
			this.lastIntervalActive = false;
			if (!this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (this.IsPawnAffected(pawn))
				{
					this.GiveOrUpdateHediff(pawn);
				}
				Pawn pawn2 = pawn.carryTracker.CarriedThing as Pawn;
				if (pawn2 != null && this.IsPawnAffected(pawn2))
				{
					this.GiveOrUpdateHediff(pawn2);
				}
			}
		}

		// Token: 0x0600FDF7 RID: 65015 RVA: 0x0049A8CC File Offset: 0x00498ACC
		private void GiveOrUpdateHediff(Pawn target)
		{
			Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
			if (hediff == null)
			{
				hediff = target.health.AddHediff(this.Props.hediff, target.health.hediffSet.GetBrain(), null, null);
				hediff.Severity = 1f;
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.drawConnection = false;
					hediffComp_Link.other = this.parent;
				}
			}
			HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears == null)
			{
				Log.ErrorOnce("CompCauseHediff_AoE has a hediff in props which does not have a HediffComp_Disappears", 78945945);
			}
			else
			{
				hediffComp_Disappears.ticksToDisappear = this.Props.checkInterval + 5;
			}
			this.lastIntervalActive = true;
		}

		// Token: 0x0600FDF8 RID: 65016 RVA: 0x0049A988 File Offset: 0x00498B88
		private void MaintainSustainer()
		{
			if (this.lastIntervalActive && this.Props.activeSound != null)
			{
				if (this.activeSustainer == null || this.activeSustainer.Ended)
				{
					this.activeSustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.parent), MaintenanceType.None));
				}
				this.activeSustainer.Maintain();
				return;
			}
			if (this.activeSustainer != null)
			{
				this.activeSustainer.End();
				this.activeSustainer = null;
			}
		}

		// Token: 0x0600FDF9 RID: 65017 RVA: 0x0049AA0C File Offset: 0x00498C0C
		public override void PostDraw()
		{
			if (!this.Props.drawLines)
			{
				return;
			}
			int num = Mathf.Max(this.parent.Map.Size.x, this.parent.Map.Size.y);
			if (Find.Selector.SelectedObjectsListForReading.Contains(this.parent) && this.range < (float)num)
			{
				foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (this.IsPawnAffected(pawn))
					{
						GenDraw.DrawLineBetween(pawn.DrawPos, this.parent.DrawPos);
					}
				}
			}
		}

		// Token: 0x0400A67C RID: 42620
		public float range;

		// Token: 0x0400A67D RID: 42621
		private Sustainer activeSustainer;

		// Token: 0x0400A67E RID: 42622
		private bool lastIntervalActive;
	}
}
