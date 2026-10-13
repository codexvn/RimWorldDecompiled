using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AE1 RID: 10977
	public class Genepack : GeneSetHolderBase
	{
		// Token: 0x1700263B RID: 9787
		// (get) Token: 0x0600F4AE RID: 62638 RVA: 0x004720AD File Offset: 0x004702AD
		public override string LabelNoCount
		{
			get
			{
				if (this.geneSet != null && this.geneSet.Label != "ERR")
				{
					return this.geneSet.Label;
				}
				return base.LabelNoCount;
			}
		}

		// Token: 0x1700263C RID: 9788
		// (get) Token: 0x0600F4AF RID: 62639 RVA: 0x004720E0 File Offset: 0x004702E0
		private CompGenepackContainer ParentContainer
		{
			get
			{
				IThingHolder parentHolder = base.ParentHolder;
				if (parentHolder != null)
				{
					CompGenepackContainer compGenepackContainer = parentHolder as CompGenepackContainer;
					if (compGenepackContainer != null)
					{
						return compGenepackContainer;
					}
				}
				return null;
			}
		}

		// Token: 0x1700263D RID: 9789
		// (get) Token: 0x0600F4B0 RID: 62640 RVA: 0x00472104 File Offset: 0x00470304
		public bool Deteriorating
		{
			get
			{
				CompGenepackContainer parentContainer = this.ParentContainer;
				return parentContainer == null || !parentContainer.PowerOn;
			}
		}

		// Token: 0x1700263E RID: 9790
		// (get) Token: 0x0600F4B1 RID: 62641 RVA: 0x00472126 File Offset: 0x00470326
		public bool AutoLoad
		{
			get
			{
				return this.autoLoad;
			}
		}

		// Token: 0x0600F4B2 RID: 62642 RVA: 0x00472130 File Offset: 0x00470330
		public void Initialize(List<GeneDef> genes)
		{
			this.geneSet = new GeneSet();
			foreach (GeneDef gene in genes)
			{
				this.geneSet.AddGene(gene);
			}
		}

		// Token: 0x0600F4B3 RID: 62643 RVA: 0x00472190 File Offset: 0x00470390
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("genepack"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
			this.geneSet = GeneUtility.GenerateGeneSet(null);
		}

		// Token: 0x0600F4B4 RID: 62644 RVA: 0x004721CC File Offset: 0x004703CC
		public override void TickRare()
		{
			base.TickRare();
			if (this.ParentContainer != null)
			{
				if (this.Deteriorating)
				{
					float statValue = this.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
					if (statValue > 0.001f && base.ParentHolder != null)
					{
						this.deteriorationPct += statValue * 250f / 60000f;
						if (this.deteriorationPct >= 1f)
						{
							this.deteriorationPct -= 1f;
							SteadyEnvironmentEffects.DoDeteriorationDamage(this, base.PositionHeld, base.MapHeld, true);
							return;
						}
					}
				}
				else if (this.HitPoints < base.MaxHitPoints)
				{
					this.hpRecoveryPct += 0.004166667f;
					if (this.hpRecoveryPct >= 1f)
					{
						this.hpRecoveryPct -= 1f;
						this.HitPoints++;
						if (this.HitPoints == base.MaxHitPoints)
						{
							this.hpRecoveryPct = 0f;
							return;
						}
					}
				}
			}
			else if (base.Spawned && !this.autoLoad && this.targetContainer != null)
			{
				this.targetContainer = null;
			}
		}

		// Token: 0x0600F4B5 RID: 62645 RVA: 0x004722EC File Offset: 0x004704EC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.targetContainer != null && this.targetContainer.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.DrawPos, this.targetContainer.DrawPos);
			}
		}

		// Token: 0x0600F4B6 RID: 62646 RVA: 0x00472325 File Offset: 0x00470525
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Toggle
			{
				defaultLabel = "GenepackAutoLoad".Translate(),
				defaultDesc = "GenepackAutoLoadDesc".Translate(),
				icon = Genepack.IconTex.Texture,
				isActive = (() => this.autoLoad),
				toggleAction = delegate()
				{
					this.autoLoad = !this.autoLoad;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600F4B7 RID: 62647 RVA: 0x00472338 File Offset: 0x00470538
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.targetContainer, "targetContainer", false);
			Scribe_Values.Look<float>(ref this.hpRecoveryPct, "hpRecoveryPct", 0f, false);
			Scribe_Values.Look<float>(ref this.deteriorationPct, "deteriorationPct", 0f, false);
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
		}

		// Token: 0x0400A143 RID: 41283
		public Thing targetContainer;

		// Token: 0x0400A144 RID: 41284
		private float hpRecoveryPct;

		// Token: 0x0400A145 RID: 41285
		private float deteriorationPct;

		// Token: 0x0400A146 RID: 41286
		private bool autoLoad = true;

		// Token: 0x0400A147 RID: 41287
		private const int HitPointRecoveryPerDayInGeneBankPerDay = 1;

		// Token: 0x0400A148 RID: 41288
		private static readonly CachedTexture IconTex = new CachedTexture("UI/Gizmos/AutoLoadGenepack");
	}
}
