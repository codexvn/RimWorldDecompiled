using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003847 RID: 14407
	public abstract class GeneGizmo_Resource : Gizmo_Slider
	{
		// Token: 0x170032DD RID: 13021
		// (get) Token: 0x06013ECA RID: 81610 RVA: 0x005E94A3 File Offset: 0x005E76A3
		protected override Color BarColor { get; }

		// Token: 0x170032DE RID: 13022
		// (get) Token: 0x06013ECB RID: 81611 RVA: 0x005E94AB File Offset: 0x005E76AB
		protected override Color BarHighlightColor { get; }

		// Token: 0x170032DF RID: 13023
		// (get) Token: 0x06013ECC RID: 81612 RVA: 0x005E94B3 File Offset: 0x005E76B3
		protected override bool IsDraggable
		{
			get
			{
				return this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x170032E0 RID: 13024
		// (get) Token: 0x06013ECD RID: 81613 RVA: 0x005E94D9 File Offset: 0x005E76D9
		protected override string BarLabel
		{
			get
			{
				return string.Format("{0} / {1}", this.gene.ValueForDisplay, this.gene.MaxForDisplay);
			}
		}

		// Token: 0x170032E1 RID: 13025
		// (get) Token: 0x06013ECE RID: 81614 RVA: 0x005E9505 File Offset: 0x005E7705
		protected override int Increments
		{
			get
			{
				return this.gene.MaxForDisplay / 10;
			}
		}

		// Token: 0x170032E2 RID: 13026
		// (get) Token: 0x06013ECF RID: 81615 RVA: 0x005E9515 File Offset: 0x005E7715
		protected override float ValuePercent
		{
			get
			{
				return this.gene.ValuePercent;
			}
		}

		// Token: 0x170032E3 RID: 13027
		// (get) Token: 0x06013ED0 RID: 81616 RVA: 0x005E9522 File Offset: 0x005E7722
		protected override FloatRange DragRange
		{
			get
			{
				return new FloatRange(0f, this.gene.Max);
			}
		}

		// Token: 0x170032E4 RID: 13028
		// (get) Token: 0x06013ED1 RID: 81617 RVA: 0x005E9539 File Offset: 0x005E7739
		// (set) Token: 0x06013ED2 RID: 81618 RVA: 0x005E9552 File Offset: 0x005E7752
		protected override float Target
		{
			get
			{
				return this.gene.targetValue / this.gene.Max;
			}
			set
			{
				this.gene.SetTargetValuePct(value);
			}
		}

		// Token: 0x170032E5 RID: 13029
		// (get) Token: 0x06013ED3 RID: 81619 RVA: 0x005E9560 File Offset: 0x005E7760
		protected override string Title
		{
			get
			{
				string text = this.gene.ResourceLabel.CapitalizeFirst();
				if (Find.Selector.SelectedPawns.Count != 1)
				{
					text = text + " (" + this.gene.pawn.LabelShort + ")";
				}
				return text;
			}
		}

		// Token: 0x06013ED4 RID: 81620 RVA: 0x005E95B2 File Offset: 0x005E77B2
		public GeneGizmo_Resource(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barHighlightColor)
		{
			this.gene = gene;
			this.drainGenes = drainGenes;
			this.BarColor = barColor;
			this.BarHighlightColor = barHighlightColor;
		}

		// Token: 0x06013ED5 RID: 81621 RVA: 0x005E95D7 File Offset: 0x005E77D7
		protected override IEnumerable<float> GetBarThresholds()
		{
			int num;
			for (int i = 0; i < this.gene.def.resourceGizmoThresholds.Count; i = num + 1)
			{
				yield return this.gene.def.resourceGizmoThresholds[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0400DC18 RID: 56344
		protected Gene_Resource gene;

		// Token: 0x0400DC19 RID: 56345
		protected List<IGeneResourceDrain> drainGenes;
	}
}
