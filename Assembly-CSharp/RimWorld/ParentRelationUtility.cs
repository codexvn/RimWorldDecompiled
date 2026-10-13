using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002415 RID: 9237
	public static class ParentRelationUtility
	{
		// Token: 0x0600C812 RID: 51218 RVA: 0x003A6DA0 File Offset: 0x003A4FA0
		public static TaggedString GetParentage(this Pawn pawn)
		{
			ParentRelationUtility.workingParentagePieces.Clear();
			Pawn mother = pawn.GetMother();
			if (mother != null)
			{
				ParentRelationUtility.workingParentagePieces.Add(ParentRelationUtility.<GetParentage>g__FactionDesc|1_0(mother));
			}
			Pawn father = pawn.GetFather();
			if (father != null)
			{
				ParentRelationUtility.workingParentagePieces.Add(ParentRelationUtility.<GetParentage>g__FactionDesc|1_0(father));
			}
			string value = ParentRelationUtility.workingParentagePieces.ToCommaList(true, true);
			if (string.IsNullOrEmpty(value))
			{
				value = "ParentsUnknown".Translate();
			}
			return "BornOfParents".Translate(value);
		}

		// Token: 0x0600C813 RID: 51219 RVA: 0x003A6E20 File Offset: 0x003A5020
		public static Pawn GetFather(this Pawn pawn)
		{
			return pawn.GetParent(Gender.Male);
		}

		// Token: 0x0600C814 RID: 51220 RVA: 0x003A6E29 File Offset: 0x003A5029
		public static Pawn GetMother(this Pawn pawn)
		{
			return pawn.GetParent(Gender.Female);
		}

		// Token: 0x0600C815 RID: 51221 RVA: 0x003A6E32 File Offset: 0x003A5032
		public static bool HasSameFather(this Pawn pawn, Pawn other)
		{
			return ParentRelationUtility.HasSameParent(pawn, other, Gender.Male);
		}

		// Token: 0x0600C816 RID: 51222 RVA: 0x003A6E3C File Offset: 0x003A503C
		public static bool HasSameMother(this Pawn pawn, Pawn other)
		{
			return ParentRelationUtility.HasSameParent(pawn, other, Gender.Female);
		}

		// Token: 0x0600C817 RID: 51223 RVA: 0x003A6E48 File Offset: 0x003A5048
		private static Pawn GetParent(this Pawn pawn, Gender parentGender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender == parentGender)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x0600C818 RID: 51224 RVA: 0x003A6EB8 File Offset: 0x003A50B8
		private static bool HasSameParent(Pawn pawn, Pawn other, Gender parentGender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (pawn.relations == null)
			{
				return false;
			}
			Pawn parent = pawn.GetParent(parentGender);
			Pawn parent2 = other.GetParent(parentGender);
			if (parent != null && parent2 != null && parent == parent2)
			{
				return true;
			}
			VirtualPawnRelation virtualParent = ParentRelationUtility.GetVirtualParent(pawn, parentGender);
			VirtualPawnRelation virtualParent2 = ParentRelationUtility.GetVirtualParent(other, parentGender);
			return virtualParent != null && virtualParent2 != null && virtualParent.record.ID == virtualParent2.record.ID;
		}

		// Token: 0x0600C819 RID: 51225 RVA: 0x003A6F2C File Offset: 0x003A512C
		private static VirtualPawnRelation GetVirtualParent(Pawn pawn, Gender gender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<VirtualPawnRelation> virtualRelations = pawn.relations.VirtualRelations;
			for (int i = 0; i < virtualRelations.Count; i++)
			{
				VirtualPawnRelation virtualPawnRelation = virtualRelations[i];
				if (virtualPawnRelation.def == PawnRelationDefOf.Parent && virtualPawnRelation.record.Gender == gender)
				{
					return virtualPawnRelation;
				}
			}
			return null;
		}

		// Token: 0x0600C81A RID: 51226 RVA: 0x003A6F98 File Offset: 0x003A5198
		public static Pawn GetBirthParent(this Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.ParentBirth)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x0600C81B RID: 51227 RVA: 0x003A7004 File Offset: 0x003A5204
		public static void SetFather(this Pawn pawn, Pawn newFather)
		{
			if (newFather != null && newFather.gender == Gender.Female)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ",
					(newFather != null) ? newFather.ToString() : null,
					" with gender ",
					newFather.gender.ToString(),
					" as ",
					(pawn != null) ? pawn.ToString() : null,
					"'s father."
				}));
				return;
			}
			Pawn father = pawn.GetFather();
			if (father != newFather)
			{
				if (father != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, father);
				}
				if (newFather != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newFather);
				}
			}
		}

		// Token: 0x0600C81C RID: 51228 RVA: 0x003A70B8 File Offset: 0x003A52B8
		public static void SetMother(this Pawn pawn, Pawn newMother)
		{
			if (newMother != null && newMother.gender != Gender.Female)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ",
					(newMother != null) ? newMother.ToString() : null,
					" with gender ",
					newMother.gender.ToString(),
					" as ",
					(pawn != null) ? pawn.ToString() : null,
					"'s mother."
				}));
				return;
			}
			Pawn mother = pawn.GetMother();
			if (mother != newMother)
			{
				if (mother != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, mother);
				}
				if (newMother != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newMother);
				}
			}
		}

		// Token: 0x0600C81E RID: 51230 RVA: 0x003A7178 File Offset: 0x003A5378
		[CompilerGenerated]
		internal static string <GetParentage>g__FactionDesc|1_0(Pawn parent)
		{
			return parent.FactionDesc(parent.NameFullColored, false, parent.NameFullColored, parent.gender.GetLabel(parent.RaceProps.Animal)).Resolve();
		}

		// Token: 0x04008863 RID: 34915
		private static List<string> workingParentagePieces = new List<string>();
	}
}
