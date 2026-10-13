using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002416 RID: 9238
	public static class PawnRelationUtility
	{
		// Token: 0x0600C81F RID: 51231 RVA: 0x003A71BB File Offset: 0x003A53BB
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			if (me == other)
			{
				yield break;
			}
			if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
			{
				yield break;
			}
			if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				yield break;
			}
			bool anyNonKinFamilyByBloodRelation = false;
			List<PawnRelationDef> defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
			int i = 0;
			int count = defs.Count;
			while (i < count)
			{
				PawnRelationDef pawnRelationDef = defs[i];
				if (pawnRelationDef != PawnRelationDefOf.Kin && pawnRelationDef.Worker.InRelation(me, other))
				{
					if (pawnRelationDef.familyByBloodRelation)
					{
						anyNonKinFamilyByBloodRelation = true;
					}
					yield return pawnRelationDef;
				}
				int num = i;
				i = num + 1;
			}
			if (!anyNonKinFamilyByBloodRelation && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
			{
				yield return PawnRelationDefOf.Kin;
			}
			defs = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C820 RID: 51232 RVA: 0x003A71D4 File Offset: 0x003A53D4
		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef pawnRelationDef2 in me.GetRelations(other))
			{
				if (pawnRelationDef == null || pawnRelationDef2.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = pawnRelationDef2;
				}
			}
			return pawnRelationDef;
		}

		// Token: 0x0600C821 RID: 51233 RVA: 0x003A7234 File Offset: 0x003A5434
		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> list = (from x in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x).ToList<Pawn>();
			bool flag = false;
			foreach (Pawn pawn in seenPawns)
			{
				if (pawn.RaceProps.IsFlesh && (informEvenIfSeenBefore || !pawn.relations.everSeenByPlayer) && (!pawn.Spawned || !pawn.Fogged()))
				{
					pawn.relations.everSeenByPlayer = true;
					bool flag2 = false;
					foreach (Pawn pawn2 in list)
					{
						if (pawn != pawn2)
						{
							PawnRelationDef mostImportantRelation = pawn2.GetMostImportantRelation(pawn);
							if (mostImportantRelation != null)
							{
								if (!flag2)
								{
									flag2 = true;
									if (flag)
									{
										stringBuilder.AppendLine();
									}
									if (writeSeenPawnsNames)
									{
										stringBuilder.AppendLine(pawn.KindLabel.CapitalizeFirst() + " " + pawn.Name.ToStringShort.Colorize(ColoredText.NameColor) + ":");
									}
								}
								flag = true;
								stringBuilder.AppendLine("  - " + "Relationship".Translate(mostImportantRelation.GetGenderSpecificLabelCap(pawn), pawn2.KindLabel + " " + pawn2.NameShortColored.Resolve(), pawn2));
							}
						}
					}
				}
			}
			if (flag)
			{
				pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
				return;
			}
			pawnRelationsInfo = null;
		}

		// Token: 0x0600C822 RID: 51234 RVA: 0x003A742C File Offset: 0x003A562C
		public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref TaggedString letterLabel, ref TaggedString letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				if (letterLabel.NullOrEmpty())
				{
					letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
				}
				else
				{
					letterLabel += ": " + "RelationshipAppendedLetterSuffix".Translate().CapitalizeFirst();
				}
				if (!letterText.NullOrEmpty())
				{
					letterText += "\n\n";
				}
				letterText += relationsInfoHeader + "\n\n" + text;
			}
		}

		// Token: 0x0600C823 RID: 51235 RVA: 0x003A74D4 File Offset: 0x003A56D4
		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			TaggedString label = "";
			TaggedString text = "";
			seenPawns = seenPawns.ToList<Pawn>();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref label, ref text, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				Pawn pawn = null;
				foreach (Pawn pawn2 in seenPawns)
				{
					if (PawnRelationUtility.GetMostImportantColonyRelative(pawn2) != null)
					{
						pawn = pawn2;
						break;
					}
				}
				if (pawn == null)
				{
					pawn = seenPawns.FirstOrDefault<Pawn>();
				}
				Find.LetterStack.ReceiveLetter(label, text, letterDef, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600C824 RID: 51236 RVA: 0x003A7580 File Offset: 0x003A5780
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, Pawn pawn)
		{
			TaggedString taggedString = null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref taggedString, pawn);
		}

		// Token: 0x0600C825 RID: 51237 RVA: 0x003A75A0 File Offset: 0x003A57A0
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, ref TaggedString title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative == null)
			{
				return false;
			}
			if (title != null)
			{
				title += " (" + "RelationshipAppendedLetterSuffix".Translate() + ")";
			}
			PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
			string text2 = (mostImportantRelation != null) ? mostImportantRelation.GetGenderSpecificLabel(pawn) : null;
			if (text2 == null)
			{
				return false;
			}
			if (mostImportantColonyRelative.IsColonist)
			{
				text += "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			else
			{
				text += "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			return true;
		}

		// Token: 0x0600C826 RID: 51238 RVA: 0x003A76D0 File Offset: 0x003A58D0
		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return null;
			}
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			float num = 0f;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in enumerable)
			{
				PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(pawn3);
				if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
				{
					num = mostImportantRelation.importance;
					pawn2 = pawn3;
				}
			}
			return pawn2;
		}

		// Token: 0x0600C827 RID: 51239 RVA: 0x003A7780 File Offset: 0x003A5980
		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0f)
			{
				return -1f;
			}
			return num;
		}

		// Token: 0x0600C828 RID: 51240 RVA: 0x003A77A6 File Offset: 0x003A59A6
		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
