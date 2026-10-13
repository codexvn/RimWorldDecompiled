using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002751 RID: 10065
	public class RitualOutcomeComp_RitualTargetDefs : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x0600DCC6 RID: 56518 RVA: 0x0040F908 File Offset: 0x0040DB08
		public override bool Applies(LordJob_Ritual ritual)
		{
			if (this.autoApplyInClassicMode && Find.IdeoManager.classicMode)
			{
				return true;
			}
			Thing thing = ritual.selectedTarget.Thing;
			if (thing == null)
			{
				return false;
			}
			if (!this.CheckIdeoCorrectness((ritual != null) ? ritual.Ritual : null, thing))
			{
				return false;
			}
			if (this.allowAltars && thing.def.isAltar)
			{
				return true;
			}
			if (!this.defs.NullOrEmpty<ThingDef>())
			{
				List<ThingDef> list = this.defs;
				Thing thing2 = ritual.selectedTarget.Thing;
				return list.Contains((thing2 != null) ? thing2.def : null);
			}
			return false;
		}

		// Token: 0x0600DCC7 RID: 56519 RVA: 0x0040F99C File Offset: 0x0040DB9C
		protected bool CheckIdeoCorrectness(Precept_Ritual ritual, Thing thing)
		{
			Ideo ideo = (ritual != null) ? ritual.ideo : null;
			return thing.StyleSourcePrecept == null || (ideo != null && thing.StyleSourcePrecept.ideo == ideo);
		}

		// Token: 0x0600DCC8 RID: 56520 RVA: 0x0040F9D4 File Offset: 0x0040DBD4
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (this.autoApplyInClassicMode && Find.IdeoManager.classicMode)
			{
				return new QualityFactor
				{
					quality = this.qualityOffset
				};
			}
			Thing thing = ritualTarget.Thing;
			if (thing == null)
			{
				return null;
			}
			bool flag = this.CheckIdeoCorrectness(ritual, thing) && ((this.allowAltars && thing.def.isAltar) || (!this.defs.NullOrEmpty<ThingDef>() && this.defs.Contains(ritualTarget.Thing.def)));
			TaggedString taggedString = "RitualOutcomeCompTip_RitualTargetDefsPart1".Translate(ritual.LabelCap, this.expectedThingLabelTip);
			if (!flag)
			{
				taggedString += "\n\n" + "RitualOutcomeCompTip_RitualTargetDefsPart2".Translate(ritual.LabelCap, thing);
			}
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				positive = flag,
				present = flag,
				quality = (flag ? this.qualityOffset : 0f),
				priority = 2f,
				toolTip = taggedString
			};
		}

		// Token: 0x0600DCC9 RID: 56521 RVA: 0x0040FB14 File Offset: 0x0040DD14
		public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			Thing thing = target.Thing;
			if (thing != null && this.autoPoweredNeedsToBeOn)
			{
				CompAutoPowered compAutoPowered = thing.TryGetComp<CompAutoPowered>();
				if (compAutoPowered != null && !compAutoPowered.AppearsPowered)
				{
					yield return "RitualOutcomeBlockedNoPower".Translate(thing.LabelShort).CapitalizeFirst();
				}
			}
			yield break;
		}

		// Token: 0x0600DCCA RID: 56522 RVA: 0x0040FB2B File Offset: 0x0040DD2B
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x040094E7 RID: 38119
		[MustTranslate]
		public string expectedThingLabelTip;

		// Token: 0x040094E8 RID: 38120
		public List<ThingDef> defs;

		// Token: 0x040094E9 RID: 38121
		public bool allowAltars;

		// Token: 0x040094EA RID: 38122
		public bool autoPoweredNeedsToBeOn;

		// Token: 0x040094EB RID: 38123
		public bool autoApplyInClassicMode;
	}
}
