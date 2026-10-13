using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020024AD RID: 9389
	public static class ApparelUtility
	{
		// Token: 0x0600CBED RID: 52205 RVA: 0x003BBF94 File Offset: 0x003BA194
		public static bool IsRequirementActive(ApparelRequirement requirement, ApparelRequirementSource source, Pawn pawn, out string disabledByLabel)
		{
			bool flag = false;
			if (pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Nudist))
			{
				flag = true;
			}
			else if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersNudity())
			{
				flag = true;
			}
			if (flag)
			{
				disabledByLabel = "Nudism".Translate();
				return false;
			}
			if (source != ApparelRequirementSource.Title && pawn.royalty != null && !pawn.royalty.AllTitlesForReading.NullOrEmpty<RoyalTitle>())
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
					{
						disabledByLabel = royalTitle.def.GetLabelCapFor(pawn);
						return false;
					}
				}
			}
			disabledByLabel = null;
			return true;
		}

		// Token: 0x0600CBEE RID: 52206 RVA: 0x003BC08C File Offset: 0x003BA28C
		public static Apparel GetApparelReplacedByNewApparel(Pawn pawn, Apparel newApparel)
		{
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, pawn.apparel.WornApparel[i].def, pawn.RaceProps.body))
				{
					return pawn.apparel.WornApparel[i];
				}
			}
			return null;
		}

		// Token: 0x0600CBEF RID: 52207 RVA: 0x003BC0F8 File Offset: 0x003BA2F8
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
			List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
			BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
			BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
			for (int k = 0; k < bodyPartGroups.Count; k++)
			{
				if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
				{
					return false;
				}
			}
			for (int l = 0; l < bodyPartGroups2.Count; l++)
			{
				if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CBF0 RID: 52208 RVA: 0x003BC1FC File Offset: 0x003BA3FC
		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		// Token: 0x0600CBF1 RID: 52209 RVA: 0x003BC260 File Offset: 0x003BA460
		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			List<BodyPartGroupDef> groups = apparel.apparel.bodyPartGroups;
			int i;
			Func<BodyPartRecord, bool> <>9__0;
			int j;
			for (i = 0; i < groups.Count; i = j + 1)
			{
				IEnumerable<BodyPartRecord> source = notMissingParts;
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.IsInGroup(groups[i])));
				}
				if (source.Any(predicate))
				{
					return true;
				}
				j = i;
			}
			return false;
		}

		// Token: 0x020024AE RID: 9390
		public struct LayerGroupPair : IEquatable<ApparelUtility.LayerGroupPair>
		{
			// Token: 0x0600CBF2 RID: 52210 RVA: 0x003BC2EF File Offset: 0x003BA4EF
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x0600CBF3 RID: 52211 RVA: 0x003BC2FF File Offset: 0x003BA4FF
			public override bool Equals(object rhs)
			{
				return rhs is ApparelUtility.LayerGroupPair && this.Equals((ApparelUtility.LayerGroupPair)rhs);
			}

			// Token: 0x0600CBF4 RID: 52212 RVA: 0x003BC317 File Offset: 0x003BA517
			public bool Equals(ApparelUtility.LayerGroupPair other)
			{
				return other.layer == this.layer && other.group == this.group;
			}

			// Token: 0x0600CBF5 RID: 52213 RVA: 0x003BC337 File Offset: 0x003BA537
			public override int GetHashCode()
			{
				return (17 * 23 + this.layer.GetHashCode()) * 23 + this.group.GetHashCode();
			}

			// Token: 0x04008B0D RID: 35597
			private readonly ApparelLayerDef layer;

			// Token: 0x04008B0E RID: 35598
			private readonly BodyPartGroupDef group;
		}
	}
}
