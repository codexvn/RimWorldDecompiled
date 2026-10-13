using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ABA RID: 6842
	public class PreceptApparelRequirement : IExposable
	{
		// Token: 0x06009933 RID: 39219 RVA: 0x002CA1C8 File Offset: 0x002C83C8
		public bool CanAddRequirement(Precept precept, List<PreceptApparelRequirement> currentRequirements, out string cannotAddReason, FactionDef generatingFor = null)
		{
			AcceptanceReport report = this.Compatible(precept.ideo, generatingFor);
			if (!report)
			{
				if (!string.IsNullOrWhiteSpace(report.Reason))
				{
					cannotAddReason = report.Reason;
				}
				else
				{
					cannotAddReason = null;
				}
				return false;
			}
			if (this.requirement.AllRequiredApparel(Gender.None).Any((ThingDef x) => currentRequirements.Any((PreceptApparelRequirement y) => y.requirement.AllRequiredApparel(Gender.None).Contains(x))))
			{
				cannotAddReason = "AlreadyAssigned".Translate();
				return false;
			}
			cannotAddReason = null;
			return true;
		}

		// Token: 0x06009934 RID: 39220 RVA: 0x002CA250 File Offset: 0x002C8450
		public bool CanWearTogetherWith(ThingDef apparel)
		{
			foreach (ThingDef thingDef in this.requirement.AllRequiredApparel(Gender.None))
			{
				if (ApparelUtility.CanWearTogether(thingDef, apparel, BodyDefOf.Human) || thingDef == apparel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009935 RID: 39221 RVA: 0x002CA2B8 File Offset: 0x002C84B8
		public bool RequirementOverlapsOther(List<PreceptApparelRequirement> currentRequirements, out string compatibilityReason)
		{
			foreach (IEnumerable<ThingDef> enumerable in from x in currentRequirements.Except(this)
			select x.requirement.AllRequiredApparel(Gender.None))
			{
				foreach (ThingDef a in this.requirement.AllRequiredApparel(Gender.None))
				{
					foreach (ThingDef thingDef in enumerable)
					{
						if (!ApparelUtility.CanWearTogether(a, thingDef, BodyDefOf.Human))
						{
							compatibilityReason = "CannotBeWornWith".Translate(thingDef);
							return true;
						}
					}
				}
			}
			compatibilityReason = null;
			return false;
		}

		// Token: 0x06009936 RID: 39222 RVA: 0x002CA3D0 File Offset: 0x002C85D0
		public AcceptanceReport Compatible(Ideo ideo, FactionDef forFaction)
		{
			if (!this.requirement.IsValid)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = this.<Compatible>g__CheckFaction|7_0(forFaction);
			if (!acceptanceReport.Accepted)
			{
				return acceptanceReport;
			}
			if (Find.World != null && Find.FactionManager != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def != forFaction && faction.ideos != null && faction.ideos.PrimaryIdeo == ideo)
					{
						acceptanceReport = this.<Compatible>g__CheckFaction|7_0(faction.def);
						if (!acceptanceReport)
						{
							return acceptanceReport;
						}
					}
				}
			}
			if (this.anyMemeRequired != null && !ideo.memes.Any((MemeDef m) => this.anyMemeRequired.Contains(m)))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleAnyMemeRequired".Translate() + ": " + (from m in this.anyMemeRequired
				select m.label).ToCommaListOr(false).CapitalizeFirst());
			}
			return true;
		}

		// Token: 0x06009937 RID: 39223 RVA: 0x002CA50C File Offset: 0x002C870C
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.allowedFactionCategoryTags, "allowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.disallowedFactionCategoryTags, "disallowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<MemeDef>(ref this.anyMemeRequired, "anyMemeRequired", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<ApparelRequirement>(ref this.requirement, "requirement", Array.Empty<object>());
		}

		// Token: 0x06009938 RID: 39224 RVA: 0x002CA570 File Offset: 0x002C8770
		public virtual void CopyTo(PreceptApparelRequirement other)
		{
			if (this.allowedFactionCategoryTags != null)
			{
				if (other.allowedFactionCategoryTags == null)
				{
					other.allowedFactionCategoryTags = new List<string>();
				}
				other.allowedFactionCategoryTags.Clear();
				other.allowedFactionCategoryTags.AddRange(this.allowedFactionCategoryTags);
			}
			else
			{
				other.allowedFactionCategoryTags = null;
			}
			if (this.disallowedFactionCategoryTags != null)
			{
				if (other.disallowedFactionCategoryTags == null)
				{
					other.disallowedFactionCategoryTags = new List<string>();
				}
				other.disallowedFactionCategoryTags.Clear();
				other.disallowedFactionCategoryTags.AddRange(this.disallowedFactionCategoryTags);
			}
			else
			{
				other.disallowedFactionCategoryTags = null;
			}
			if (this.anyMemeRequired != null)
			{
				if (other.anyMemeRequired == null)
				{
					other.anyMemeRequired = new List<MemeDef>();
				}
				other.anyMemeRequired.Clear();
				other.anyMemeRequired.AddRange(this.anyMemeRequired);
			}
			else
			{
				other.anyMemeRequired = null;
			}
			if (this.requirement != null)
			{
				ApparelRequirement other2 = new ApparelRequirement();
				this.requirement.CopyTo(other2);
				other.requirement = other2;
				return;
			}
			other.requirement = null;
		}

		// Token: 0x0600993A RID: 39226 RVA: 0x002CA668 File Offset: 0x002C8868
		[CompilerGenerated]
		private AcceptanceReport <Compatible>g__CheckFaction|7_0(FactionDef faction)
		{
			if (faction != null && this.allowedFactionCategoryTags != null && !this.allowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in this.allowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Allowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			if (faction != null && this.disallowedFactionCategoryTags != null && this.disallowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in this.disallowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Disallowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			return true;
		}

		// Token: 0x04006830 RID: 26672
		public List<string> allowedFactionCategoryTags;

		// Token: 0x04006831 RID: 26673
		public List<string> disallowedFactionCategoryTags;

		// Token: 0x04006832 RID: 26674
		public List<MemeDef> anyMemeRequired;

		// Token: 0x04006833 RID: 26675
		public ApparelRequirement requirement;
	}
}
