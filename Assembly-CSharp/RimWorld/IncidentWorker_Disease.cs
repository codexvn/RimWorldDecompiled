using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8F RID: 7567
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		// Token: 0x0600A953 RID: 43347
		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		// Token: 0x0600A954 RID: 43348 RVA: 0x00307507 File Offset: 0x00305707
		protected IEnumerable<Pawn> PotentialVictims(IIncidentTarget target)
		{
			return this.PotentialVictimCandidates(target).Where(delegate(Pawn p)
			{
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					return false;
				}
				if (p.RaceProps.Dryad)
				{
					return false;
				}
				if (!this.def.diseaseDevelopmentStage.Has(p.DevelopmentalStage))
				{
					return false;
				}
				if (!this.def.diseasePartsToAffect.NullOrEmpty<BodyPartDef>())
				{
					bool flag = false;
					for (int i = 0; i < this.def.diseasePartsToAffect.Count; i++)
					{
						if (IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, this.def.diseaseIncident, this.def.diseasePartsToAffect[i]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return p.RaceProps.IsFlesh;
			});
		}

		// Token: 0x0600A955 RID: 43349
		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		// Token: 0x0600A956 RID: 43350 RVA: 0x00307524 File Offset: 0x00305724
		private static bool CanAddHediffToAnyPartOfDef(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			if (pawn.ageTracker.CurLifeStage == LifeStageDefOf.HumanlikeBaby && Find.Storyteller.difficulty.babiesAreHealthy)
			{
				return false;
			}
			if (!hediffDef.canAffectBionicOrImplant && pawn.health.hediffSet.IsBionicOrImplant(partDef))
			{
				return false;
			}
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A957 RID: 43351 RVA: 0x003075D6 File Offset: 0x003057D6
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		// Token: 0x0600A958 RID: 43352 RVA: 0x003075F0 File Offset: 0x003057F0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			string text;
			List<Pawn> list = this.ApplyToPawns(this.ActualVictims(parms).ToList<Pawn>(), out text);
			if (!list.Any<Pawn>() && text.NullOrEmpty())
			{
				return false;
			}
			TaggedString baseLetterLabel = this.def.letterLabel;
			TaggedString taggedString;
			if (list.Any<Pawn>())
			{
				if (this.def.letterSingularForm)
				{
					if (list.Count > 1)
					{
						Log.Error("Incident " + this.def.defName + " is marked to only generate a letter in a singular format, but multiple victims were provided.");
					}
					Pawn pawn = list[0];
					Hediff mostRecentHediff = pawn.health.hediffSet.GetMostRecentHediff(this.def.diseaseIncident);
					baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN"));
					HediffComp_SeverityPerDay hediffComp_SeverityPerDay = mostRecentHediff.TryGetComp<HediffComp_SeverityPerDay>();
					if (hediffComp_SeverityPerDay != null)
					{
						float num = hediffComp_SeverityPerDay.SeverityChangePerDay();
						int value = Mathf.RoundToInt(mostRecentHediff.def.maxSeverity / num);
						taggedString = this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.diseaseIncident.label, mostRecentHediff.Part.Label, value).Resolve();
					}
					else
					{
						taggedString = this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.diseaseIncident.label, mostRecentHediff.Part.Label).Resolve();
					}
					if (mostRecentHediff.IsAnyStageLifeThreatening() && !string.IsNullOrEmpty(this.def.diseaseLethalLetterText))
					{
						taggedString += "\n\n" + this.def.diseaseLethalLetterText.Formatted(pawn.Named("PAWN"));
					}
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < list.Count; i++)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendTagged("  - " + list[i].LabelNoCountColored.Resolve());
					}
					string str = this.def.letterText.Formatted(list.Count.ToString(), Faction.OfPlayer.def.pawnsPlural, this.def.diseaseIncident.label).Resolve();
					string str2 = ":\n\n";
					StringBuilder stringBuilder2 = stringBuilder;
					taggedString = str + str2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
				}
			}
			else
			{
				taggedString = "";
			}
			if (!text.NullOrEmpty())
			{
				if (!taggedString.NullOrEmpty())
				{
					taggedString += "\n\n";
				}
				taggedString += text;
			}
			base.SendStandardLetter(baseLetterLabel, taggedString, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A959 RID: 43353 RVA: 0x00307908 File Offset: 0x00305B08
		public List<Pawn> ApplyToPawns(IEnumerable<Pawn> pawns, out string blockedInfo)
		{
			List<Pawn> list = new List<Pawn>();
			Dictionary<HediffDef, List<Pawn>> dictionary = new Dictionary<HediffDef, List<Pawn>>();
			foreach (Pawn pawn in pawns)
			{
				HediffDef hediffDef = null;
				if (Rand.Chance(pawn.health.immunity.DiseaseContractChanceFactor(this.def.diseaseIncident, out hediffDef, null)))
				{
					HediffGiverUtility.TryApply(pawn, this.def.diseaseIncident, this.def.diseasePartsToAffect, false, 1, null, true);
					TaleRecorder.RecordTale(TaleDefOf.IllnessRevealed, new object[]
					{
						pawn,
						this.def.diseaseIncident
					});
					list.Add(pawn);
				}
				else if (hediffDef != null)
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = new List<Pawn>();
					}
					dictionary[hediffDef].Add(pawn);
				}
			}
			blockedInfo = "";
			foreach (KeyValuePair<HediffDef, List<Pawn>> keyValuePair in dictionary)
			{
				if (keyValuePair.Key != this.def.diseaseIncident)
				{
					if (blockedInfo.Length != 0)
					{
						blockedInfo += "\n\n";
					}
					blockedInfo = blockedInfo + "LetterDisease_Blocked".Translate(keyValuePair.Key.LabelCap, this.def.diseaseIncident.label).Resolve() + ":\n" + (from victim in keyValuePair.Value
					select victim.LabelNoCountColored.Resolve()).ToLineList("  - ", false);
				}
			}
			return list;
		}
	}
}
