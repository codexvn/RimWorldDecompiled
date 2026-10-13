using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200075E RID: 1886
	public class Hediff_Pregnant : HediffWithParents
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000ED883 File Offset: 0x000EBA83
		// (set) Token: 0x06002E3D RID: 11837 RVA: 0x000EDCE8 File Offset: 0x000EBEE8
		public float GestationProgress
		{
			get
			{
				return this.Severity;
			}
			private set
			{
				this.Severity = value;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x000EDCF4 File Offset: 0x000EBEF4
		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(this.pawn);
					}
				}
				return num > 38f * this.pawn.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x000EDDCC File Offset: 0x000EBFCC
		public PregnancyAttitude? Attitude
		{
			get
			{
				if (this.comps == null || !this.pawn.RaceProps.Humanlike)
				{
					return null;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					HediffComp_PregnantHuman hediffComp_PregnantHuman = this.comps[i] as HediffComp_PregnantHuman;
					if (hediffComp_PregnantHuman != null)
					{
						return hediffComp_PregnantHuman.Attitude;
					}
				}
				return null;
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000EDE38 File Offset: 0x000EC038
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.lastStage = this.CurStageIndex;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000EDE50 File Offset: 0x000EC050
		public override void TickInterval(int delta)
		{
			this.ageTicks += delta;
			if (this.CurStageIndex != this.lastStage)
			{
				this.NotifyPlayerOfTrimesterPassing();
				this.lastStage = this.CurStageIndex;
			}
			if ((!this.pawn.RaceProps.Humanlike || !Find.Storyteller.difficulty.babiesAreHealthy) && this.pawn.IsHashIntervalTick(1000, delta))
			{
				Need_Food food = this.pawn.needs.food;
				if (food != null && food.CurCategory == HungerCategory.Starving)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.1f && Rand.MTBEventOccurs(2f, 60000f, 1000f))
					{
						if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							string value = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
							Messages.Message("MessageMiscarriedStarvation".Translate(value, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
						}
						this.Miscarry();
						return;
					}
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(2f, 60000f, 1000f))
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						string value2 = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
						Messages.Message("MessageMiscarriedPoorHealth".Translate(value2, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
			}
			float num = PawnUtility.BodyResourceGrowthSpeed(this.pawn) / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			this.GestationProgress += num * (float)delta;
			if (this.GestationProgress >= 1f)
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageGaveBirth".Translate(this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
					}
					Hediff_Pregnant.DoBirthSpawn(this.pawn, base.Father);
				}
				else
				{
					this.StartLabor();
				}
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000EE144 File Offset: 0x000EC344
		public void Miscarry()
		{
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.Miscarried, null, null);
						}
					}
				}
			}
			Pawn mother = base.Mother;
			Pawn father = base.Father;
			if (mother != this.pawn && mother != null)
			{
				Pawn_NeedsTracker needs2 = mother.needs;
				if (needs2 != null)
				{
					Need_Mood mood2 = needs2.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory(ThoughtDefOf.PartnerMiscarried, this.pawn, null);
							}
						}
					}
				}
			}
			if (father != this.pawn && father != null)
			{
				Pawn_NeedsTracker needs3 = father.needs;
				if (needs3 != null)
				{
					Need_Mood mood3 = needs3.mood;
					if (mood3 != null)
					{
						ThoughtHandler thoughts3 = mood3.thoughts;
						if (thoughts3 != null)
						{
							MemoryThoughtHandler memories3 = thoughts3.memories;
							if (memories3 != null)
							{
								memories3.TryGainMemory(ThoughtDefOf.PartnerMiscarried, this.pawn, null);
							}
						}
					}
				}
			}
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000EE244 File Offset: 0x000EC444
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			if (mother.RaceProps.Humanlike && !ModsConfig.BiotechActive)
			{
				return;
			}
			int num = (mother.RaceProps.litterSizeCurve != null) ? Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)) : 1;
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother, null))
				{
					if (pawn.playerSettings != null && mother.playerSettings != null)
					{
						pawn.playerSettings.AreaRestrictionInPawnCurrentMap = mother.playerSettings.AreaRestrictionInPawnCurrentMap;
					}
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
						if (father != null)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
						}
					}
					if (mother.Spawned)
					{
						Lord lord = mother.GetLord();
						if (lord != null)
						{
							lord.AddPawn(pawn);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
				{
					mother,
					pawn
				});
			}
			if (mother.Spawned)
			{
				FilthMaker.TryMakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5, FilthSourceFlags.None);
				Pawn_CallTracker caller = mother.caller;
				if (caller != null)
				{
					caller.DoCall(false);
				}
				Pawn_CallTracker caller2 = pawn.caller;
				if (caller2 == null)
				{
					return;
				}
				caller2.DoCall(false);
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000EE444 File Offset: 0x000EC644
		public void StartLabor()
		{
			if (!ModLister.CheckBiotech("labor"))
			{
				return;
			}
			((Hediff_Labor)this.pawn.health.AddHediff(HediffDefOf.PregnancyLabor, null, null, null)).SetParents(base.Mother, base.Father, this.geneSet);
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorningSickness, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyMood, false);
			if (firstHediffOfDef2 != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterColonistPregnancyLaborLabel".Translate(this.pawn), "LetterColonistPregnancyLabor".Translate(this.pawn), LetterDefOf.NeutralEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000EE551 File Offset: 0x000EC751
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastStage, "lastStage", 0, false);
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000EE56C File Offset: 0x000EC76C
		private void NotifyPlayerOfTrimesterPassing()
		{
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message(((this.lastStage == 0) ? "MessageColonistReaching2ndTrimesterPregnancy" : "MessageColonistReaching3rdTrimesterPregnancy").Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				if (this.lastStage == 1 && !Find.History.everThirdTrimesterPregnancy)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelThirdTrimester".Translate(this.pawn), "LetterTextThirdTrimester".Translate(this.pawn), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
					Find.History.everThirdTrimesterPregnancy = true;
				}
			}
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000EE651 File Offset: 0x000EC851
		public override void PostDebugAdd()
		{
			if (ModsConfig.BiotechActive && this.pawn.RaceProps.Humanlike)
			{
				base.SetParents(this.pawn, null, PregnancyUtility.GetInheritedGeneSet(null, this.pawn));
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000EE688 File Offset: 0x000EC888
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod(true, false, true, true, false));
			return stringBuilder.ToString();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000EE707 File Offset: 0x000EC907
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.CurStageIndex < 2)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Next trimester",
						action = delegate()
						{
							HediffStage hediffStage = this.def.stages[this.CurStageIndex + 1];
							this.severityInt = hediffStage.minSeverity;
						}
					};
				}
				if (ModsConfig.BiotechActive && this.pawn.RaceProps.Humanlike && this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyLabor, false) == null)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Start Labor",
						action = delegate()
						{
							this.StartLabor();
							this.pawn.health.RemoveHediff(this);
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04002458 RID: 9304
		private int lastStage;

		// Token: 0x04002459 RID: 9305
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x0400245A RID: 9306
		private const float MTBMiscarryStarvingDays = 2f;

		// Token: 0x0400245B RID: 9307
		private const float MTBMiscarryWoundedDays = 2f;

		// Token: 0x0400245C RID: 9308
		private const float MalnutritionMinSeverityForMiscarry = 0.1f;
	}
}
