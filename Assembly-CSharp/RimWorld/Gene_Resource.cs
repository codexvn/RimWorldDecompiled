using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200383D RID: 14397
	public abstract class Gene_Resource : Gene
	{
		// Token: 0x170032B5 RID: 12981
		// (get) Token: 0x06013E69 RID: 81513 RVA: 0x005E88E4 File Offset: 0x005E6AE4
		public virtual string ResourceLabel
		{
			get
			{
				return this.def.resourceLabel;
			}
		}

		// Token: 0x170032B6 RID: 12982
		// (get) Token: 0x06013E6A RID: 81514
		public abstract float InitialResourceMax { get; }

		// Token: 0x170032B7 RID: 12983
		// (get) Token: 0x06013E6B RID: 81515
		protected abstract Color BarColor { get; }

		// Token: 0x170032B8 RID: 12984
		// (get) Token: 0x06013E6C RID: 81516
		protected abstract Color BarHighlightColor { get; }

		// Token: 0x170032B9 RID: 12985
		// (get) Token: 0x06013E6D RID: 81517
		public abstract float MinLevelForAlert { get; }

		// Token: 0x170032BA RID: 12986
		// (get) Token: 0x06013E6E RID: 81518 RVA: 0x005E88F1 File Offset: 0x005E6AF1
		public virtual int ValueForDisplay
		{
			get
			{
				return this.PostProcessValue(this.cur);
			}
		}

		// Token: 0x170032BB RID: 12987
		// (get) Token: 0x06013E6F RID: 81519 RVA: 0x005E88FF File Offset: 0x005E6AFF
		public virtual int MaxForDisplay
		{
			get
			{
				return this.PostProcessValue(this.max);
			}
		}

		// Token: 0x170032BC RID: 12988
		// (get) Token: 0x06013E70 RID: 81520 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float MaxLevelOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170032BD RID: 12989
		// (get) Token: 0x06013E71 RID: 81521 RVA: 0x005E890D File Offset: 0x005E6B0D
		public virtual float Max
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x170032BE RID: 12990
		// (get) Token: 0x06013E72 RID: 81522 RVA: 0x005E8915 File Offset: 0x005E6B15
		// (set) Token: 0x06013E73 RID: 81523 RVA: 0x005E891D File Offset: 0x005E6B1D
		public virtual float Value
		{
			get
			{
				return this.cur;
			}
			set
			{
				this.cur = Mathf.Clamp(value, 0f, this.max);
			}
		}

		// Token: 0x170032BF RID: 12991
		// (get) Token: 0x06013E74 RID: 81524 RVA: 0x005E8936 File Offset: 0x005E6B36
		// (set) Token: 0x06013E75 RID: 81525 RVA: 0x005E8945 File Offset: 0x005E6B45
		public virtual float ValuePercent
		{
			get
			{
				return this.cur / this.max;
			}
			set
			{
				this.cur = this.max * value;
			}
		}

		// Token: 0x170032C0 RID: 12992
		// (get) Token: 0x06013E76 RID: 81526 RVA: 0x005E8958 File Offset: 0x005E6B58
		private List<IGeneResourceDrain> DrainGenes
		{
			get
			{
				this.tmpDrainGenes.Clear();
				List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					IGeneResourceDrain geneResourceDrain = genesListForReading[i] as IGeneResourceDrain;
					if (geneResourceDrain != null && geneResourceDrain.Resource == this)
					{
						this.tmpDrainGenes.Add(geneResourceDrain);
					}
				}
				return this.tmpDrainGenes;
			}
		}

		// Token: 0x06013E77 RID: 81527 RVA: 0x005E89BD File Offset: 0x005E6BBD
		public override void PostAdd()
		{
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x06013E78 RID: 81528 RVA: 0x005E89CB File Offset: 0x005E6BCB
		public void SetMax(float newMax)
		{
			this.max = newMax;
			this.cur = Mathf.Clamp(this.cur, 0f, this.max);
			this.SetTargetValuePct(this.targetValue);
		}

		// Token: 0x06013E79 RID: 81529 RVA: 0x005E89FC File Offset: 0x005E6BFC
		public void ResetMax()
		{
			this.max = this.InitialResourceMax;
			this.cur = Mathf.Clamp(this.cur, 0f, this.max);
			this.SetTargetValuePct(this.targetValue);
		}

		// Token: 0x06013E7A RID: 81530 RVA: 0x005E8A34 File Offset: 0x005E6C34
		public override void Reset()
		{
			this.cur = (this.max = this.InitialResourceMax);
			this.targetValue = 0.5f;
		}

		// Token: 0x06013E7B RID: 81531 RVA: 0x005E8A61 File Offset: 0x005E6C61
		public virtual void SetTargetValuePct(float val)
		{
			this.targetValue = val * this.Max;
		}

		// Token: 0x06013E7C RID: 81532 RVA: 0x005E8A71 File Offset: 0x005E6C71
		public virtual int PostProcessValue(float value)
		{
			return Mathf.RoundToInt(value * 100f);
		}

		// Token: 0x06013E7D RID: 81533 RVA: 0x005E8A7F File Offset: 0x005E6C7F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!this.Active)
			{
				yield break;
			}
			if (this.gizmo == null)
			{
				this.gizmo = (GeneGizmo_Resource)Activator.CreateInstance(this.def.resourceGizmoType, new object[]
				{
					this,
					this.DrainGenes,
					this.BarColor,
					this.BarHighlightColor
				});
			}
			if ((Find.Selector.SelectedPawns.Count == 1 || this.def.showGizmoOnMultiSelect) && (!this.pawn.Drafted || this.def.showGizmoWhenDrafted))
			{
				yield return this.gizmo;
			}
			yield break;
		}

		// Token: 0x06013E7E RID: 81534 RVA: 0x005E8A90 File Offset: 0x005E6C90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cur, "cur", 0f, false);
			Scribe_Values.Look<float>(ref this.max, "max", 0f, false);
			Scribe_Values.Look<float>(ref this.targetValue, "targetValue", 0.5f, false);
		}

		// Token: 0x0400DBFA RID: 56314
		public float targetValue = 0.5f;

		// Token: 0x0400DBFB RID: 56315
		protected float cur;

		// Token: 0x0400DBFC RID: 56316
		protected float max;

		// Token: 0x0400DBFD RID: 56317
		[Unsaved(false)]
		protected GeneGizmo_Resource gizmo;

		// Token: 0x0400DBFE RID: 56318
		[Unsaved(false)]
		private List<IGeneResourceDrain> tmpDrainGenes = new List<IGeneResourceDrain>();
	}
}
