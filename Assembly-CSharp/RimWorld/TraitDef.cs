using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1B RID: 6939
	public class TraitDef : Def
	{
		// Token: 0x06009B0A RID: 39690 RVA: 0x002D0410 File Offset: 0x002CE610
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06009B0B RID: 39691 RVA: 0x002D041C File Offset: 0x002CE61C
		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(this.defName + " found no data at degree " + degree.ToString() + ", returning first defined.");
			return this.degreeDatas[0];
		}

		// Token: 0x06009B0C RID: 39692 RVA: 0x002D048D File Offset: 0x002CE68D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			int num;
			for (int i = 0; i < this.degreeDatas.Count; i = num + 1)
			{
				TraitDef.<>c__DisplayClass17_0 CS$<>8__locals1 = new TraitDef.<>c__DisplayClass17_0();
				CS$<>8__locals1.dd = this.degreeDatas[i];
				if (this.degreeDatas.Count((TraitDegreeData dd2) => dd2.degree == CS$<>8__locals1.dd.degree) > 1)
				{
					yield return ">1 datas for degree " + CS$<>8__locals1.dd.degree.ToString();
				}
				if (!CS$<>8__locals1.dd.ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
				{
					foreach (IngestibleModifiers ingestibleModifiers in CS$<>8__locals1.dd.ingestibleModifiers)
					{
						if (((ingestibleModifiers != null) ? ingestibleModifiers.ingestible : null) == null)
						{
							yield return "ingestible override has a null target ingestible";
						}
					}
					List<IngestibleModifiers>.Enumerator enumerator2 = default(List<IngestibleModifiers>.Enumerator);
				}
				CS$<>8__locals1 = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06009B0D RID: 39693 RVA: 0x002D04A0 File Offset: 0x002CE6A0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (TraitDegreeData traitDegreeData in this.degreeDatas)
			{
				traitDegreeData.ResolveReferences();
			}
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x002D04F8 File Offset: 0x002CE6F8
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		// Token: 0x06009B0F RID: 39695 RVA: 0x002D0508 File Offset: 0x002CE708
		public bool ConflictsWith(TraitDef other)
		{
			if ((other.conflictingTraits != null && other.conflictingTraits.Contains(this)) || (this.conflictingTraits != null && this.conflictingTraits.Contains(other)))
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009B10 RID: 39696 RVA: 0x002D0585 File Offset: 0x002CE785
		public bool CanSuppress(Trait other)
		{
			return this.ConflictsWith(other) || other.def == this;
		}

		// Token: 0x06009B11 RID: 39697 RVA: 0x002D059B File Offset: 0x002CE79B
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		// Token: 0x06009B12 RID: 39698 RVA: 0x002D05B3 File Offset: 0x002CE7B3
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		// Token: 0x06009B13 RID: 39699 RVA: 0x002D05CB File Offset: 0x002CE7CB
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		// Token: 0x04006B4C RID: 27468
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x04006B4D RID: 27469
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x04006B4E RID: 27470
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04006B4F RID: 27471
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		// Token: 0x04006B50 RID: 27472
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		// Token: 0x04006B51 RID: 27473
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04006B52 RID: 27474
		public WorkTags requiredWorkTags;

		// Token: 0x04006B53 RID: 27475
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04006B54 RID: 27476
		public WorkTags disabledWorkTags;

		// Token: 0x04006B55 RID: 27477
		public AnimalType? disableHostilityFromAnimalType;

		// Token: 0x04006B56 RID: 27478
		public FactionDef disableHostilityFromFaction;

		// Token: 0x04006B57 RID: 27479
		public bool canBeSuppressed = true;

		// Token: 0x04006B58 RID: 27480
		private float commonality = 1f;

		// Token: 0x04006B59 RID: 27481
		private float commonalityFemale = -1f;

		// Token: 0x04006B5A RID: 27482
		public bool allowOnHostileSpawn = true;
	}
}
