using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003845 RID: 14405
	public class Gene_HemogenDrain : Gene, IGeneResourceDrain
	{
		// Token: 0x170032D6 RID: 13014
		// (get) Token: 0x06013EB9 RID: 81593 RVA: 0x005E92EF File Offset: 0x005E74EF
		public Gene_Resource Resource
		{
			get
			{
				if (this.cachedHemogenGene == null || !this.cachedHemogenGene.Active)
				{
					this.cachedHemogenGene = this.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
				}
				return this.cachedHemogenGene;
			}
		}

		// Token: 0x170032D7 RID: 13015
		// (get) Token: 0x06013EBA RID: 81594 RVA: 0x005E8C5B File Offset: 0x005E6E5B
		public bool CanOffset
		{
			get
			{
				return this.Active && !this.pawn.Deathresting;
			}
		}

		// Token: 0x170032D8 RID: 13016
		// (get) Token: 0x06013EBB RID: 81595 RVA: 0x005E8CA5 File Offset: 0x005E6EA5
		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		// Token: 0x170032D9 RID: 13017
		// (get) Token: 0x06013EBC RID: 81596 RVA: 0x005E8C53 File Offset: 0x005E6E53
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170032DA RID: 13018
		// (get) Token: 0x06013EBD RID: 81597 RVA: 0x005E8C75 File Offset: 0x005E6E75
		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		// Token: 0x06013EBE RID: 81598 RVA: 0x005E8DAF File Offset: 0x005E6FAF
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			GeneResourceDrainUtility.TickResourceDrainInterval(this, delta);
		}

		// Token: 0x06013EBF RID: 81599 RVA: 0x005E9322 File Offset: 0x005E7522
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!this.Active)
			{
				yield break;
			}
			foreach (Gizmo gizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400DC11 RID: 56337
		[Unsaved(false)]
		private Gene_Hemogen cachedHemogenGene;

		// Token: 0x0400DC12 RID: 56338
		private const float MinAgeForDrain = 3f;
	}
}
