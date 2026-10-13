using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A64 RID: 10852
	public class CompDeathrestBindable : ThingComp
	{
		// Token: 0x170025A4 RID: 9636
		// (get) Token: 0x0600F13E RID: 61758 RVA: 0x0045F9FF File Offset: 0x0045DBFF
		public CompProperties_DeathrestBindable Props
		{
			get
			{
				return (CompProperties_DeathrestBindable)this.props;
			}
		}

		// Token: 0x170025A5 RID: 9637
		// (get) Token: 0x0600F13F RID: 61759 RVA: 0x0045FA0C File Offset: 0x0045DC0C
		public Pawn BoundPawn
		{
			get
			{
				return this.boundPawn;
			}
		}

		// Token: 0x170025A6 RID: 9638
		// (get) Token: 0x0600F140 RID: 61760 RVA: 0x0045FA14 File Offset: 0x0045DC14
		public bool CanIncreasePresence
		{
			get
			{
				if (this.PowerTraderComp != null && !this.PowerTraderComp.PowerOn)
				{
					return false;
				}
				if (this.RefuelableComp != null && !this.RefuelableComp.HasFuel)
				{
					return false;
				}
				if (!this.boundPawn.InBed())
				{
					return false;
				}
				Building_Bed building_Bed = this.boundPawn.CurrentBed();
				if (building_Bed == this.parent)
				{
					return true;
				}
				CompDeathrestBindable compDeathrestBindable = building_Bed.TryGetComp<CompDeathrestBindable>();
				return compDeathrestBindable != null && compDeathrestBindable.BoundPawn == this.boundPawn;
			}
		}

		// Token: 0x170025A7 RID: 9639
		// (get) Token: 0x0600F141 RID: 61761 RVA: 0x0045FA90 File Offset: 0x0045DC90
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				if (this.cachedPowerComp == null)
				{
					this.cachedPowerComp = this.parent.TryGetComp<CompPowerTrader>();
				}
				return this.cachedPowerComp;
			}
		}

		// Token: 0x170025A8 RID: 9640
		// (get) Token: 0x0600F142 RID: 61762 RVA: 0x0045FAB1 File Offset: 0x0045DCB1
		private CompRefuelable RefuelableComp
		{
			get
			{
				if (this.cachedRefuelableComp == null)
				{
					this.cachedRefuelableComp = this.parent.TryGetComp<CompRefuelable>();
				}
				return this.cachedRefuelableComp;
			}
		}

		// Token: 0x170025A9 RID: 9641
		// (get) Token: 0x0600F143 RID: 61763 RVA: 0x0045FAD2 File Offset: 0x0045DCD2
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				if (this.cachedDeathrestGene == null)
				{
					Pawn pawn = this.boundPawn;
					Gene_Deathrest gene_Deathrest;
					if (pawn == null)
					{
						gene_Deathrest = null;
					}
					else
					{
						Pawn_GeneTracker genes = pawn.genes;
						gene_Deathrest = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null);
					}
					this.cachedDeathrestGene = gene_Deathrest;
				}
				return this.cachedDeathrestGene;
			}
		}

		// Token: 0x170025AA RID: 9642
		// (get) Token: 0x0600F144 RID: 61764 RVA: 0x0045FB06 File Offset: 0x0045DD06
		private Material HoseMat
		{
			get
			{
				if (this.cachedHoseMat == null)
				{
					this.cachedHoseMat = MaterialPool.MatFrom("Other/DeathrestBuildingConnection");
				}
				return this.cachedHoseMat;
			}
		}

		// Token: 0x0600F145 RID: 61765 RVA: 0x0045FB2C File Offset: 0x0045DD2C
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Deathrest binding"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600F146 RID: 61766 RVA: 0x0045FB4D File Offset: 0x0045DD4D
		public bool CanBindTo(Pawn pawn)
		{
			return (!this.Props.mustBeLayingInToBind || pawn.CurrentBed() == this.parent) && (this.boundPawn == null || this.boundPawn == pawn);
		}

		// Token: 0x0600F147 RID: 61767 RVA: 0x0045FB7F File Offset: 0x0045DD7F
		public void BindTo(Pawn pawn)
		{
			this.boundPawn = pawn;
		}

		// Token: 0x0600F148 RID: 61768 RVA: 0x0045FB88 File Offset: 0x0045DD88
		public void Notify_DeathrestGeneRemoved()
		{
			this.cachedDeathrestGene = null;
			this.boundPawn = null;
			this.presenceTicks = 0;
		}

		// Token: 0x0600F149 RID: 61769 RVA: 0x0045FBA0 File Offset: 0x0045DDA0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad || this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.presenceTicks = 0;
			foreach (Pawn pawn in this.parent.Map.mapPawns.PawnsInFaction(this.parent.Faction))
			{
				if (pawn.Deathresting)
				{
					Pawn_GeneTracker genes = pawn.genes;
					Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
					if (gene_Deathrest != null && gene_Deathrest.CanBindToBindable(this))
					{
						gene_Deathrest.BindTo(this);
					}
				}
			}
		}

		// Token: 0x0600F14A RID: 61770 RVA: 0x0045FC4C File Offset: 0x0045DE4C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.presenceTicks = 0;
			if (mode != DestroyMode.WillReplace && this.boundPawn != null)
			{
				Pawn_GeneTracker genes = this.boundPawn.genes;
				if (genes != null)
				{
					Gene_Deathrest firstGeneOfType = genes.GetFirstGeneOfType<Gene_Deathrest>();
					if (firstGeneOfType != null)
					{
						firstGeneOfType.Notify_BoundBuildingDeSpawned(this.parent);
					}
				}
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x0600F14B RID: 61771 RVA: 0x0045FCC8 File Offset: 0x0045DEC8
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CompTickRare();
			}
			if (this.presenceTicks > 0)
			{
				if (!this.Props.soundWorking.NullOrUndefined())
				{
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = this.Props.soundWorking.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainer.Maintain();
				}
				CompRefuelable refuelableComp = this.RefuelableComp;
				if (refuelableComp == null)
				{
					return;
				}
				refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x0600F14C RID: 61772 RVA: 0x0045FD60 File Offset: 0x0045DF60
		public override void CompTickRare()
		{
			if (this.PowerTraderComp != null)
			{
				this.PowerTraderComp.PowerOutput = ((this.presenceTicks > 0) ? (-this.PowerTraderComp.Props.PowerConsumption) : (-this.PowerTraderComp.Props.idlePowerDraw));
			}
		}

		// Token: 0x0600F14D RID: 61773 RVA: 0x0045FDB0 File Offset: 0x0045DFB0
		public override void PostDraw()
		{
			if (!this.Props.mustBeLayingInToBind && this.boundPawn != null && this.boundPawn.Map == this.parent.Map && this.boundPawn.Deathresting && this.DeathrestGene != null && this.DeathrestGene.BoundComps.Contains(this) && this.CanIncreasePresence)
			{
				Vector3 a = this.boundPawn.CurrentBed().TrueCenter();
				Vector3 b = this.parent.TrueCenter();
				a.y = (b.y = AltitudeLayer.SmallWire.AltitudeFor());
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS((a + b) / 2f, Quaternion.Euler(0f, a.AngleToFlat(b) + 90f, 0f), new Vector3(1f, 1f, (a - b).MagnitudeHorizontal()));
				Graphics.DrawMesh(MeshPool.plane10, identity, this.HoseMat, 0);
			}
		}

		// Token: 0x0600F14E RID: 61774 RVA: 0x0045FECC File Offset: 0x0045E0CC
		public void Apply()
		{
			if (this.boundPawn != null)
			{
				if (this.Props.hediffToApply != null)
				{
					this.boundPawn.health.AddHediff(this.Props.hediffToApply, null, null, null);
				}
				if (this.Props.hemogenLimitOffset > 0f)
				{
					Pawn_GeneTracker genes = this.boundPawn.genes;
					Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
					if (gene_Hemogen != null)
					{
						gene_Hemogen.SetMax(gene_Hemogen.Max + this.Props.hemogenLimitOffset);
					}
				}
			}
		}

		// Token: 0x0600F14F RID: 61775 RVA: 0x0045FF5C File Offset: 0x0045E15C
		public void TryIncreasePresence(int delta)
		{
			if (!this.CanIncreasePresence)
			{
				return;
			}
			if (this.presenceTicks <= 0)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				if (!this.Props.soundWorking.NullOrUndefined() && (this.sustainer == null || this.sustainer.Ended))
				{
					this.sustainer = this.Props.soundWorking.TrySpawnSustainer(info);
				}
				if (!this.Props.soundStart.NullOrUndefined())
				{
					this.Props.soundStart.PlayOneShot(info);
				}
			}
			this.presenceTicks += delta;
		}

		// Token: 0x0600F150 RID: 61776 RVA: 0x0045FFFC File Offset: 0x0045E1FC
		public void Notify_DeathrestEnded()
		{
			this.presenceTicks = 0;
			if (this.parent.Spawned)
			{
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
				if (!this.Props.soundEnd.NullOrUndefined())
				{
					this.Props.soundEnd.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
			}
		}

		// Token: 0x0600F151 RID: 61777 RVA: 0x00460070 File Offset: 0x0045E270
		public override string CompInspectStringExtra()
		{
			string text = null;
			if (this.boundPawn != null && this.DeathrestGene != null)
			{
				text = text + ("BoundTo".Translate() + ": " + this.boundPawn.NameShortColored).Resolve() + string.Format(" ({0}/{1} {2})", this.DeathrestGene.CurrentCapacity, this.DeathrestGene.DeathrestCapacity, "DeathrestCapacity".Translate());
				if (this.Props.displayTimeActive && this.presenceTicks > 0 && this.DeathrestGene.deathrestTicks > 0)
				{
					float f = Mathf.Clamp01((float)this.presenceTicks / (float)this.DeathrestGene.deathrestTicks);
					text += string.Format("\n{0}: {1} / {2} ({3})\n{4}", new object[]
					{
						"TimeActiveThisDeathrest".Translate(),
						this.presenceTicks.ToStringTicksToPeriod(true, true, true, true, false),
						this.DeathrestGene.deathrestTicks.ToStringTicksToPeriod(true, true, true, true, false),
						f.ToStringPercent(),
						"MinimumNeededToApply".Translate(0.75f.ToStringPercent())
					});
				}
			}
			else
			{
				text += "WillBindOnFirstUse".Translate();
			}
			return text;
		}

		// Token: 0x0600F152 RID: 61778 RVA: 0x004601DF File Offset: 0x0045E3DF
		public override void PostExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.boundPawn, "boundPawn", false);
			Scribe_Values.Look<int>(ref this.presenceTicks, "presenceTicks", 0, false);
		}

		// Token: 0x04009F26 RID: 40742
		private Pawn boundPawn;

		// Token: 0x04009F27 RID: 40743
		public int presenceTicks;

		// Token: 0x04009F28 RID: 40744
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04009F29 RID: 40745
		[Unsaved(false)]
		private CompRefuelable cachedRefuelableComp;

		// Token: 0x04009F2A RID: 40746
		[Unsaved(false)]
		private Gene_Deathrest cachedDeathrestGene;

		// Token: 0x04009F2B RID: 40747
		[Unsaved(false)]
		private Material cachedHoseMat;

		// Token: 0x04009F2C RID: 40748
		[Unsaved(false)]
		private Sustainer sustainer;
	}
}
