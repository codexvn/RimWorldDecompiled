using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C84 RID: 11396
	public class CompFacilityQualityBased : CompFacility
	{
		// Token: 0x1700285F RID: 10335
		// (get) Token: 0x0600FFD3 RID: 65491 RVA: 0x004A1EA2 File Offset: 0x004A00A2
		public new CompProperties_FacilityQualityBased Props
		{
			get
			{
				return (CompProperties_FacilityQualityBased)this.props;
			}
		}

		// Token: 0x17002860 RID: 10336
		// (get) Token: 0x0600FFD4 RID: 65492 RVA: 0x004A1EAF File Offset: 0x004A00AF
		public override List<StatModifier> StatOffsets
		{
			get
			{
				return this.statOffsets;
			}
		}

		// Token: 0x0600FFD5 RID: 65493 RVA: 0x004A1EB7 File Offset: 0x004A00B7
		public virtual void PostQualitySet()
		{
			this.SetStatOffsets();
		}

		// Token: 0x0600FFD6 RID: 65494 RVA: 0x004A1EC0 File Offset: 0x004A00C0
		private void SetStatOffsets()
		{
			this.statOffsets.Clear();
			QualityCategory key;
			if (!this.parent.TryGetQuality(out key))
			{
				Log.Error("Could not get parent quality for CompFacilityQualityBased of " + this.parent.def.defName);
				return;
			}
			foreach (KeyValuePair<StatDef, Dictionary<QualityCategory, float>> keyValuePair in this.Props.statOffsetsPerQuality)
			{
				this.statOffsets.Add(new StatModifier
				{
					stat = keyValuePair.Key,
					value = keyValuePair.Value[key]
				});
			}
		}

		// Token: 0x0600FFD7 RID: 65495 RVA: 0x004A1F7C File Offset: 0x004A017C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetStatOffsets();
			}
		}

		// Token: 0x0400A74F RID: 42831
		private List<StatModifier> statOffsets = new List<StatModifier>();
	}
}
