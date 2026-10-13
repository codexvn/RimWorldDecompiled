using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EFC RID: 12028
	public class CompQuality : ThingComp
	{
		// Token: 0x17002BDB RID: 11227
		// (get) Token: 0x06010FAF RID: 69551 RVA: 0x004E2DA1 File Offset: 0x004E0FA1
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x06010FB0 RID: 69552 RVA: 0x004E2DAC File Offset: 0x004E0FAC
		public void SetQuality(QualityCategory q, ArtGenerationContext? source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null && source != null)
			{
				compArt.InitializeArt(source.Value);
			}
			this.parent.PostQualitySet();
			CompFacilityQualityBased compFacilityQualityBased = this.parent.TryGetComp<CompFacilityQualityBased>();
			if (compFacilityQualityBased == null)
			{
				return;
			}
			compFacilityQualityBased.PostQualitySet();
		}

		// Token: 0x06010FB1 RID: 69553 RVA: 0x004E2E05 File Offset: 0x004E1005
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06010FB2 RID: 69554 RVA: 0x004E2E1F File Offset: 0x004E101F
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), new ArtGenerationContext?(ArtGenerationContext.Outsider));
		}

		// Token: 0x06010FB3 RID: 69555 RVA: 0x004E2E34 File Offset: 0x004E1034
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06010FB4 RID: 69556 RVA: 0x004E2E56 File Offset: 0x004E1056
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x06010FB5 RID: 69557 RVA: 0x004E2E70 File Offset: 0x004E1070
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}

		// Token: 0x0400B096 RID: 45206
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
