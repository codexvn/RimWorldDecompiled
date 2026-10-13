using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020024A7 RID: 9383
	public class ApparelRequirement : IExposable
	{
		// Token: 0x17001F11 RID: 7953
		// (get) Token: 0x0600CBB7 RID: 52151 RVA: 0x003BB095 File Offset: 0x003B9295
		public bool IsValid
		{
			get
			{
				if (this.isValid == null)
				{
					this.isValid = new bool?(this.AllRequiredApparel(Gender.None).Any<ThingDef>());
				}
				return this.isValid.Value;
			}
		}

		// Token: 0x0600CBB8 RID: 52152 RVA: 0x003BB0C8 File Offset: 0x003B92C8
		private bool MatchesBodyPartGroups(ThingDef apparel)
		{
			foreach (BodyPartGroupDef item in this.bodyPartGroupsMatchAny)
			{
				if (apparel.apparel.bodyPartGroups.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CBB9 RID: 52153 RVA: 0x003BB130 File Offset: 0x003B9330
		private bool HasRequiredTag(ThingDef apparel)
		{
			if (this.requiredTags != null)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if (this.requiredTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600CBBA RID: 52154 RVA: 0x003BB1A0 File Offset: 0x003B93A0
		private static bool WearsAny(Pawn p, ThingDef apparel)
		{
			using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CBBB RID: 52155 RVA: 0x003BB200 File Offset: 0x003B9400
		public bool AllowedForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			if (!apparel.IsApparel)
			{
				return false;
			}
			if (!apparel.apparel.PawnCanWear(p, ignoreGender))
			{
				return false;
			}
			if (apparel.apparel.tags == null)
			{
				return false;
			}
			bool flag = this.requiredDefs != null && this.requiredDefs.Contains(apparel);
			if (!flag)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if ((this.requiredTags != null && this.requiredTags.Contains(item)) || (this.allowedTags != null && this.allowedTags.Contains(item)))
					{
						flag = true;
						break;
					}
				}
			}
			return flag && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x0600CBBC RID: 52156 RVA: 0x003BB2D4 File Offset: 0x003B94D4
		public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (this.AllowedForPawn(p, thingDef, ignoreGender) && (includeWorn || !ApparelRequirement.WearsAny(p, thingDef)))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CBBD RID: 52157 RVA: 0x003BB2FC File Offset: 0x003B94FC
		public bool RequiredForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			return apparel.IsApparel && apparel.apparel.PawnCanWear(p, ignoreGender) && apparel.apparel.tags != null && ((this.requiredDefs != null && this.requiredDefs.Contains(apparel)) || this.HasRequiredTag(apparel)) && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x0600CBBE RID: 52158 RVA: 0x003BB35E File Offset: 0x003B955E
		public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (this.RequiredForPawn(p, thingDef, ignoreGender) && (includeWorn || !ApparelRequirement.WearsAny(p, thingDef)))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CBBF RID: 52159 RVA: 0x003BB383 File Offset: 0x003B9583
		public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (thingDef.IsApparel && thingDef.apparel.tags != null && ((this.requiredDefs != null && this.requiredDefs.Contains(thingDef)) || this.HasRequiredTag(thingDef)) && thingDef.apparel.PawnCanWear(gender, DevelopmentalStage.Adult) && this.MatchesBodyPartGroups(thingDef))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CBC0 RID: 52160 RVA: 0x003BB39C File Offset: 0x003B959C
		public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
		{
			bool flag = false;
			for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
			{
				if (thingDef.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return allowUnmatched;
			}
			if (this.requiredDefs != null && this.requiredDefs.Contains(thingDef))
			{
				return true;
			}
			if (this.requiredTags != null)
			{
				for (int j = 0; j < this.requiredTags.Count; j++)
				{
					if (thingDef.apparel.tags.Contains(this.requiredTags[j]))
					{
						return true;
					}
				}
			}
			if (this.allowedTags != null)
			{
				for (int k = 0; k < this.allowedTags.Count; k++)
				{
					if (thingDef.apparel.tags.Contains(this.allowedTags[k]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CBC1 RID: 52161 RVA: 0x003BB480 File Offset: 0x003B9680
		public bool IsMet(Pawn p)
		{
			foreach (Apparel apparel in p.apparel.WornApparel)
			{
				bool flag = false;
				for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
				{
					if (apparel.def.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (this.requiredDefs != null && this.requiredDefs.Contains(apparel.def))
					{
						return true;
					}
					if (this.requiredTags != null)
					{
						for (int j = 0; j < this.requiredTags.Count; j++)
						{
							if (apparel.def.apparel.tags.Contains(this.requiredTags[j]))
							{
								return true;
							}
						}
					}
					if (this.allowedTags != null)
					{
						for (int k = 0; k < this.allowedTags.Count; k++)
						{
							if (apparel.def.apparel.tags.Contains(this.allowedTags[k]))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600CBC2 RID: 52162 RVA: 0x003BB5E8 File Offset: 0x003B97E8
		public bool SameApparelAs(ApparelRequirement other)
		{
			return this.requiredDefs.SetsEqual(other.requiredDefs) && this.requiredTags.SetsEqual(other.requiredTags) && this.allowedTags.SetsEqual(other.allowedTags);
		}

		// Token: 0x0600CBC3 RID: 52163 RVA: 0x003BB623 File Offset: 0x003B9823
		public bool IsActive(Pawn forPawn)
		{
			return forPawn.apparel != null && forPawn.apparel.ActiveRequirementsForReading.Contains(this);
		}

		// Token: 0x0600CBC4 RID: 52164 RVA: 0x003BB640 File Offset: 0x003B9840
		public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
		{
			ThingDef result = null;
			Predicate<BodyPartGroupDef> <>9__2;
			Predicate<string> <>9__3;
			if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
			{
				if (a.IsApparel && a.apparel.tags != null)
				{
					List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
					Predicate<BodyPartGroupDef> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)));
					}
					if (bodyPartGroups.Any(predicate))
					{
						List<string> tags = a.apparel.tags;
						Predicate<string> predicate2;
						if ((predicate2 = <>9__3) == null)
						{
							predicate2 = (<>9__3 = ((string t) => this.requiredTags.Contains(t)));
						}
						if (tags.Any(predicate2) && a.apparel.PawnCanWear(p, false))
						{
							return validator == null || validator(a);
						}
					}
				}
				return false;
			}).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600CBC5 RID: 52165 RVA: 0x003BB6AC File Offset: 0x003B98AC
		public override string ToString()
		{
			string text = "";
			if (this.requiredTags != null)
			{
				text += string.Join(",", this.requiredTags.ToArray());
			}
			if (this.allowedTags != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join(",", this.allowedTags.ToArray());
			}
			if (this.requiredDefs != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join<TaggedString>(",", (from d in this.requiredDefs
				select d.LabelCap).ToArray<TaggedString>());
			}
			return "(" + string.Join(",", (from a in this.bodyPartGroupsMatchAny
			select a.defName).ToArray<string>()) + ") -> " + text;
		}

		// Token: 0x0600CBC6 RID: 52166 RVA: 0x003BB7C0 File Offset: 0x003B99C0
		public void ExposeData()
		{
			Scribe_Collections.Look<BodyPartGroupDef>(ref this.bodyPartGroupsMatchAny, "bodyPartGroupsMatchAny", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.requiredDefs, "requiredDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.requiredTags, "requiredTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.allowedTags, "allowedTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.groupLabel, "groupLabel", null, false);
		}

		// Token: 0x0600CBC7 RID: 52167 RVA: 0x003BB838 File Offset: 0x003B9A38
		public virtual void CopyTo(ApparelRequirement other)
		{
			if (this.bodyPartGroupsMatchAny != null)
			{
				if (other.bodyPartGroupsMatchAny == null)
				{
					other.bodyPartGroupsMatchAny = new List<BodyPartGroupDef>();
				}
				other.bodyPartGroupsMatchAny.Clear();
				other.bodyPartGroupsMatchAny.AddRange(this.bodyPartGroupsMatchAny);
			}
			else
			{
				other.bodyPartGroupsMatchAny = null;
			}
			if (this.requiredTags != null)
			{
				if (other.requiredTags == null)
				{
					other.requiredTags = new List<string>();
				}
				other.requiredTags.Clear();
				other.requiredTags.AddRange(this.requiredTags);
			}
			else
			{
				other.requiredTags = null;
			}
			if (this.allowedTags != null)
			{
				if (other.allowedTags == null)
				{
					other.allowedTags = new List<string>();
				}
				other.allowedTags.Clear();
				other.allowedTags.AddRange(this.allowedTags);
			}
			else
			{
				other.allowedTags = null;
			}
			if (this.requiredDefs != null)
			{
				if (other.requiredDefs == null)
				{
					other.requiredDefs = new List<ThingDef>();
				}
				other.requiredDefs.Clear();
				other.requiredDefs.AddRange(this.requiredDefs);
			}
			else
			{
				other.requiredDefs = null;
			}
			other.groupLabel = this.groupLabel;
		}

		// Token: 0x04008AE1 RID: 35553
		public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

		// Token: 0x04008AE2 RID: 35554
		public List<string> requiredTags;

		// Token: 0x04008AE3 RID: 35555
		public List<string> allowedTags;

		// Token: 0x04008AE4 RID: 35556
		public List<ThingDef> requiredDefs;

		// Token: 0x04008AE5 RID: 35557
		[MustTranslate]
		public string groupLabel;

		// Token: 0x04008AE6 RID: 35558
		[Unsaved(false)]
		private bool? isValid;
	}
}
