using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001D2F RID: 7471
	public class GameCondition_PsychicSuppression : GameCondition
	{
		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x0600A759 RID: 42841 RVA: 0x002FF827 File Offset: 0x002FDA27
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x0600A75A RID: 42842 RVA: 0x002FF84F File Offset: 0x002FDA4F
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x0600A75B RID: 42843 RVA: 0x002FF877 File Offset: 0x002FDA77
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x0600A75C RID: 42844 RVA: 0x002FF87F File Offset: 0x002FDA7F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x0600A75D RID: 42845 RVA: 0x002FF89C File Offset: 0x002FDA9C
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		// Token: 0x0600A75E RID: 42846 RVA: 0x002FF8F4 File Offset: 0x002FDAF4
		public override void GameConditionTick()
		{
			foreach (Map map in base.AffectedMaps)
			{
				List<Pawn> allPawns = map.mapPawns.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					GameCondition_PsychicSuppression.CheckPawn(allPawns[i], this.gender);
				}
			}
		}

		// Token: 0x0600A75F RID: 42847 RVA: 0x002FF970 File Offset: 0x002FDB70
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicSuppressorGender", this.gender.GetLabel(false)));
		}

		// Token: 0x040073E7 RID: 29671
		public Gender gender;
	}
}
