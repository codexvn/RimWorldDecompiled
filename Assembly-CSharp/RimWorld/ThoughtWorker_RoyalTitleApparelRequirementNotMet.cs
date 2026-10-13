using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189F RID: 6303
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		// Token: 0x06009030 RID: 36912 RVA: 0x002A879C File Offset: 0x002A699C
		private static RoyalTitle Validate(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				return null;
			}
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				if (royalTitle.def.requiredApparel != null && royalTitle.def.requiredApparel.Count > 0)
				{
					for (int i = 0; i < royalTitle.def.requiredApparel.Count; i++)
					{
						ApparelRequirement apparelRequirement = royalTitle.def.requiredApparel[i];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							return royalTitle;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06009031 RID: 36913 RVA: 0x002A886C File Offset: 0x002A6A6C
		private static IEnumerable<string> GetAllRequiredApparelPerGroup(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				yield break;
			}
			foreach (RoyalTitle t in p.royalty.AllTitlesInEffectForReading)
			{
				if (t.def.requiredApparel != null && t.def.requiredApparel.Count > 0)
				{
					int num;
					for (int i = 0; i < t.def.requiredApparel.Count; i = num + 1)
					{
						ApparelRequirement apparelRequirement = t.def.requiredApparel[i];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							IEnumerable<ThingDef> enumerable = apparelRequirement.AllRequiredApparelForPawn(p, false, false);
							foreach (ThingDef thingDef in enumerable)
							{
								yield return thingDef.LabelCap;
							}
							IEnumerator<ThingDef> enumerator2 = null;
						}
						num = i;
					}
				}
				t = null;
			}
			List<RoyalTitle>.Enumerator enumerator = default(List<RoyalTitle>.Enumerator);
			yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			if (ModsConfig.BiotechActive)
			{
				yield return "ApparelRequirementAnyMechlordApparel".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x06009032 RID: 36914 RVA: 0x002A887C File Offset: 0x002A6A7C
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06009033 RID: 36915 RVA: 0x002A88C4 File Offset: 0x002A6AC4
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetAllRequiredApparelPerGroup(p).Distinct<string>().ToLineList("- ", false), royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06009034 RID: 36916 RVA: 0x002A8925 File Offset: 0x002A6B25
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
