using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002466 RID: 9318
	public class Pawn_MutantTracker : IExposable, IVerbOwner
	{
		// Token: 0x17001E83 RID: 7811
		// (get) Token: 0x0600CA04 RID: 51716 RVA: 0x003B25B9 File Offset: 0x003B07B9
		public MutantDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001E84 RID: 7812
		// (get) Token: 0x0600CA05 RID: 51717 RVA: 0x003B25C1 File Offset: 0x003B07C1
		public bool HasTurned
		{
			get
			{
				return this.hasTurned;
			}
		}

		// Token: 0x17001E85 RID: 7813
		// (get) Token: 0x0600CA06 RID: 51718 RVA: 0x003B25C9 File Offset: 0x003B07C9
		public bool IsPassive
		{
			get
			{
				return this.def.passive;
			}
		}

		// Token: 0x17001E86 RID: 7814
		// (get) Token: 0x0600CA07 RID: 51719 RVA: 0x003B25D6 File Offset: 0x003B07D6
		public Hediff Hediff
		{
			get
			{
				return this.mutantHediff;
			}
		}

		// Token: 0x17001E87 RID: 7815
		// (get) Token: 0x0600CA08 RID: 51720 RVA: 0x003B25DE File Offset: 0x003B07DE
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17001E88 RID: 7816
		// (get) Token: 0x0600CA09 RID: 51721 RVA: 0x003B25EB File Offset: 0x003B07EB
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17001E89 RID: 7817
		// (get) Token: 0x0600CA0A RID: 51722 RVA: 0x003B25F3 File Offset: 0x003B07F3
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Def.verbs;
			}
		}

		// Token: 0x17001E8A RID: 7818
		// (get) Token: 0x0600CA0B RID: 51723 RVA: 0x003B2600 File Offset: 0x003B0800
		public List<Tool> Tools
		{
			get
			{
				return this.Def.tools;
			}
		}

		// Token: 0x17001E8B RID: 7819
		// (get) Token: 0x0600CA0C RID: 51724 RVA: 0x0036EC81 File Offset: 0x0036CE81
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17001E8C RID: 7820
		// (get) Token: 0x0600CA0D RID: 51725 RVA: 0x003B260D File Offset: 0x003B080D
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600CA0E RID: 51726 RVA: 0x003B2615 File Offset: 0x003B0815
		public string UniqueVerbOwnerID()
		{
			return this.pawn.GetUniqueLoadID() + "_mutant";
		}

		// Token: 0x0600CA0F RID: 51727 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x17001E8D RID: 7821
		// (get) Token: 0x0600CA10 RID: 51728 RVA: 0x003B262C File Offset: 0x003B082C
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.abilities == null)
				{
					this.abilities = new List<Ability>();
					foreach (AbilityDef abilityDef in this.def.abilities)
					{
						this.abilities.Add(AbilityUtility.MakeAbility(abilityDef, this.pawn));
					}
				}
				return this.abilities;
			}
		}

		// Token: 0x0600CA11 RID: 51729 RVA: 0x003B26B0 File Offset: 0x003B08B0
		public Pawn_MutantTracker(Pawn pawn, MutantDef def, RotStage rotStage)
		{
			this.pawn = pawn;
			this.def = def;
			this.rotStage = rotStage;
			this.verbTracker = new VerbTracker(this);
			this.originalFaction = pawn.HomeFaction;
			if (pawn.RaceProps.Humanlike)
			{
				Ideo ideo;
				if ((ideo = pawn.Ideo) == null)
				{
					Faction faction = pawn.Faction;
					Ideo ideo2;
					if (faction == null)
					{
						ideo2 = null;
					}
					else
					{
						FactionIdeosTracker ideos = faction.ideos;
						ideo2 = ((ideos != null) ? ideos.PrimaryIdeo : null);
					}
					ideo = (ideo2 ?? Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>());
				}
				this.originalIdeo = ideo;
				Faction faction2;
				if ((faction2 = this.originalFaction) == null)
				{
					if ((faction2 = Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderCivil)) == null)
					{
						if ((faction2 = Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderRough)) == null)
						{
							faction2 = (from f in Find.FactionManager.AllFactionsListForReading
							where !f.IsPlayer && f.def.humanlikeFaction && !f.Hidden && !f.temporary
							select f).RandomElement<Faction>();
						}
					}
				}
				this.originalFaction = faction2;
			}
		}

		// Token: 0x0600CA12 RID: 51730 RVA: 0x003B27EB File Offset: 0x003B09EB
		public Pawn_MutantTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CA13 RID: 51731 RVA: 0x003B27FC File Offset: 0x003B09FC
		public Pawn_MutantTracker.ThinkTrees GetThinkTrees()
		{
			return new Pawn_MutantTracker.ThinkTrees
			{
				main = this.def.thinkTree,
				constant = this.def.thinkTreeConstant
			};
		}

		// Token: 0x0600CA14 RID: 51732 RVA: 0x003B2836 File Offset: 0x003B0A36
		public void MutantTrackerTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x0600CA15 RID: 51733 RVA: 0x003B2844 File Offset: 0x003B0A44
		public void MutantTrackerTickInterval(int delta)
		{
			if (!this.pawn.Drawer.renderer.HasAnimation && this.Def.standingAnimation != null)
			{
				this.pawn.Drawer.renderer.SetAnimation(this.Def.standingAnimation);
			}
			if (this.pawn.Spawned && this.pawn.GetPosture() == PawnPosture.Standing)
			{
				if (this.rotStage == RotStage.Rotting && Rand.MTBEventOccurs(5f, 2500f, 1f))
				{
					FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, ThingDefOf.Filth_CorpseBile, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
				}
				float summaryHealthPercent = this.pawn.health.summaryHealth.SummaryHealthPercent;
				if (!this.Def.canBleed && this.Def.bloodDef != null && this.rotStage != RotStage.Dessicated && summaryHealthPercent < 1f && Rand.MTBEventOccurs(Pawn_MutantTracker.DropBloodMTBSecondsFromHealthPctCurve.Evaluate(summaryHealthPercent), 60f, 1f))
				{
					this.pawn.health.DropBloodFilth();
				}
			}
			if (this.Def.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
			{
				for (int i = 0; i < this.Def.hediffGivers.Count; i++)
				{
					this.Def.hediffGivers[i].OnIntervalPassed(this.pawn, this.mutantHediff);
				}
			}
		}

		// Token: 0x0600CA16 RID: 51734 RVA: 0x003B29CC File Offset: 0x003B0BCC
		public void Turn(bool clearLord = false)
		{
			if (this.hasTurned)
			{
				string str = "Tried to turn mutant ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " again");
				return;
			}
			this.hasTurned = true;
			if (this.def.hediff != null)
			{
				this.mutantHediff = this.pawn.health.GetOrAddHediff(this.def.hediff, null, null, null);
			}
			if (!this.def.givesHediffs.NullOrEmpty<StartingHediff>())
			{
				HealthUtility.AddStartingHediffs(this.pawn, this.def.givesHediffs);
			}
			if (this.def.terminatePregnancy)
			{
				PregnancyUtility.ForceEndPregnancy(this.pawn, true);
			}
			if (this.pawn.caller == null)
			{
				this.pawn.caller = new Pawn_CallTracker(this.pawn);
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.def.disablesIdeo)
			{
				Pawn_IdeoTracker ideo = this.pawn.ideo;
				if (ideo != null)
				{
					ideo.SetIdeo(null);
				}
			}
			Pawn_GuestTracker guest = this.pawn.guest;
			if (guest != null)
			{
				guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			if (this.pawn.royalty != null)
			{
				this.pawn.royalty.Notify_PawnKilled();
				this.pawn.royalty.UpdateAvailableAbilities();
			}
			Pawn_PlayerSettings playerSettings = this.pawn.playerSettings;
			if (playerSettings != null)
			{
				playerSettings.ResetMedicalCare();
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (clearLord)
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.Killed, null);
				}
			}
			if (!this.Def.passive && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
			}
			this.pawn.ownership.UnclaimAll();
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this.pawn);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "BecameMutant", this.pawn.Named("SUBJECT"));
			Find.QuestManager.Notify_PawnKilled(this.pawn, null);
			if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(this.pawn))
			{
				Find.History.Notify_MechanitorDied();
			}
			if (this.Def.relativeTurnedThought != null)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn2.needs != null && pawn2.RaceProps.IsFlesh && pawn2.needs.mood != null && (pawn2.relations.OpinionOf(this.pawn) >= 20 || pawn2.relations.FamilyByBlood.Contains(this.pawn)))
					{
						pawn2.needs.mood.thoughts.memories.TryGainMemory(this.Def.relativeTurnedThought, this.pawn, null);
					}
				}
			}
			Pawn_GeneTracker genes = this.pawn.genes;
			if (genes != null)
			{
				genes.Reset();
			}
			Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
			if (pawn_AbilityTracker != null)
			{
				pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
			}
			if (this.pawn.outfits != null)
			{
				this.pawn.outfits.CurrentApparelPolicy = null;
			}
			if (this.pawn.foodRestriction != null)
			{
				this.pawn.foodRestriction.CurrentFoodPolicy = null;
			}
			if (this.pawn.drugs != null)
			{
				this.pawn.drugs.CurrentPolicy = null;
			}
			if (this.pawn.reading != null)
			{
				this.pawn.reading.CurrentPolicy = null;
			}
			this.HandleEquipment();
			this.ResolveGraphics();
			this.pawn.Drawer.renderer.SetAnimation(this.Def.standingAnimation);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.codexEntry != null)
			{
				Find.EntityCodex.SetDiscovered(this.def.codexEntry, this.pawn.def, this.pawn);
			}
			if (this.def.clearsEgo)
			{
				this.pawn.everLostEgo = true;
			}
		}

		// Token: 0x0600CA17 RID: 51735 RVA: 0x003B2E5C File Offset: 0x003B105C
		public void Revert(bool beingKilled = false)
		{
			if (!this.hasTurned)
			{
				string str = "Tried to revert mutant ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who hasn't turned");
				return;
			}
			this.hasTurned = false;
			this.pawn.mutant = null;
			if (this.mutantHediff != null)
			{
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = this.mutantHediff.TryGetComp<HediffComp_DisappearsAndKills>();
				if (hediffComp_DisappearsAndKills != null)
				{
					hediffComp_DisappearsAndKills.disabled = true;
				}
				this.pawn.health.RemoveHediff(this.mutantHediff);
			}
			foreach (StartingHediff startingHediff in this.def.givesHediffs)
			{
				Hediff hediff;
				if (this.pawn.health.hediffSet.TryGetHediff(startingHediff.def, out hediff))
				{
					this.pawn.health.RemoveHediff(hediff);
				}
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.Faction != this.originalFaction)
			{
				this.pawn.SetFaction(this.originalFaction, null);
			}
			Pawn_IdeoTracker ideo = this.pawn.ideo;
			if (ideo != null)
			{
				ideo.SetIdeo(this.originalIdeo);
			}
			if (this.pawn.royalty != null)
			{
				if (!beingKilled)
				{
					this.pawn.royalty.Notify_Resurrected();
				}
				this.pawn.royalty.UpdateAvailableAbilities();
			}
			this.pawn.Drawer.renderer.SetAnimation(null);
			this.abilities = null;
			Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
			if (pawn_AbilityTracker != null)
			{
				pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
			}
			Pawn_GeneTracker genes = this.pawn.genes;
			if (genes != null)
			{
				genes.Reset();
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x0600CA18 RID: 51736 RVA: 0x003B3050 File Offset: 0x003B1250
		private void ResolveGraphics()
		{
			if (!this.Def.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
			{
				this.pawn.story.TryGetRandomHeadFromSet(this.Def.forcedHeadTypes);
			}
			if (this.Def.hairTagFilter != null)
			{
				this.pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.pawn);
			}
			if (this.Def.beardTagFilter != null)
			{
				this.pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.pawn);
			}
			if (this.Def.hairColorOverride != null)
			{
				this.pawn.story.HairColor = this.Def.hairColorOverride.Value;
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600CA19 RID: 51737 RVA: 0x003B3124 File Offset: 0x003B1324
		private void HandleEquipment()
		{
			Pawn_EquipmentTracker equipment = this.pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				ThingWithComps thingWithComps;
				this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
			}
			if (this.pawn.apparel != null)
			{
				if (this.def.disableApparel)
				{
					if (this.pawn.MapHeld != null)
					{
						this.pawn.apparel.DropAll(this.pawn.Position, true, true, null);
					}
					else
					{
						this.pawn.apparel.DestroyAll(DestroyMode.Vanish);
					}
				}
				if (this.def.isConsideredCorpse)
				{
					foreach (Apparel apparel in this.pawn.apparel.WornApparel)
					{
						if (apparel.def.apparel.careIfWornByCorpse)
						{
							apparel.WornByCorpse = true;
						}
					}
				}
			}
		}

		// Token: 0x0600CA1A RID: 51738 RVA: 0x003B3244 File Offset: 0x003B1444
		public bool HediffGiversCanGive(HediffDef hediffDef)
		{
			if (this.Def.preventIllnesses && hediffDef.chronic)
			{
				return false;
			}
			List<HediffDef> removesHediffs = this.Def.removesHediffs;
			return removesHediffs == null || !removesHediffs.Contains(hediffDef);
		}

		// Token: 0x0600CA1B RID: 51739 RVA: 0x003B327A File Offset: 0x003B147A
		public void Notify_Downed()
		{
			this.pawn.Drawer.renderer.SetAnimation(null);
		}

		// Token: 0x0600CA1C RID: 51740 RVA: 0x003B3294 File Offset: 0x003B1494
		public void Notify_Died(Corpse corpse, DamageInfo? dinfo, Hediff hediff = null)
		{
			if (this.def.isConsideredCorpse)
			{
				if (this.rotStage == RotStage.Fresh)
				{
					this.rotStage = RotStage.Rotting;
				}
				CompRottable compRottable = (corpse != null) ? corpse.GetComp<CompRottable>() : null;
				if (compRottable != null)
				{
					compRottable.RotImmediately(this.rotStage);
				}
			}
			if (this.pawn.Faction == Faction.OfPlayer && this.def.deathLetter != null)
			{
				TaggedString label = "Death".Translate(this.pawn.LabelShortCap) + ": " + this.pawn.LabelShortCap;
				TaggedString text = this.def.deathLetter + " " + HealthUtility.GetDiedLetterText(this.pawn, dinfo, hediff) + "\n\n" + this.def.deathLetterExtra;
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.Death, corpse, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600CA1D RID: 51741 RVA: 0x003B3394 File Offset: 0x003B1594
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			if (this.pawn.Faction == Faction.OfPlayer && this.Def == MutantDefOf.Ghoul)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ColonyGhouls, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600CA1E RID: 51742 RVA: 0x003B33E6 File Offset: 0x003B15E6
		public IEnumerable<Gizmo> GetGizmos()
		{
			Gizmo gizmo = AnomalyUtility.OpenCodexGizmo(this.pawn);
			if (gizmo != null)
			{
				yield return gizmo;
			}
			yield break;
		}

		// Token: 0x0600CA1F RID: 51743 RVA: 0x003B33F6 File Offset: 0x003B15F6
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.pawn.TryGetComp<CompProducesBioferrite>() != null)
			{
				yield break;
			}
			if (this.def.producesBioferrite)
			{
				StatDrawEntry statDrawEntry = CompProducesBioferrite.BioferriteStatDrawEntry(this.pawn);
				statDrawEntry.overridesHideStats = true;
				yield return statDrawEntry;
			}
			yield break;
		}

		// Token: 0x0600CA20 RID: 51744 RVA: 0x003B3408 File Offset: 0x003B1608
		public string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.def.showJobReport && this.pawn.Faction == Faction.OfPlayer)
			{
				string jobReport = this.pawn.GetJobReport();
				if (!jobReport.NullOrEmpty())
				{
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.Append(jobReport);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600CA21 RID: 51745 RVA: 0x003B3464 File Offset: 0x003B1664
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.originalFaction != null && !Find.FactionManager.AllFactions.Contains(this.originalFaction))
				{
					this.originalFaction = null;
				}
				if (this.originalIdeo != null && !Find.IdeoManager.IdeosListForReading.Contains(this.originalIdeo))
				{
					this.originalIdeo = null;
				}
			}
			Scribe_Defs.Look<MutantDef>(ref this.def, "shamblerType");
			Scribe_Values.Look<RotStage>(ref this.rotStage, "rotStage", RotStage.Fresh, false);
			Scribe_Values.Look<bool>(ref this.hasTurned, "hasTurned", false, false);
			Scribe_References.Look<Hediff>(ref this.mutantHediff, "mutantHediff", false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_References.Look<Faction>(ref this.originalFaction, "originalFaction", false);
			Scribe_References.Look<Ideo>(ref this.originalIdeo, "originalIdeo", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.abilities != null)
				{
					foreach (Ability ability in this.abilities)
					{
						ability.pawn = this.pawn;
						ability.verb.caster = this.pawn;
					}
				}
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
			}
		}

		// Token: 0x04008984 RID: 35204
		private const float RottenDropCorpseBileMTBHours = 5f;

		// Token: 0x04008985 RID: 35205
		private static readonly SimpleCurve DropBloodMTBSecondsFromHealthPctCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(1f, 120f),
				true
			}
		};

		// Token: 0x04008986 RID: 35206
		private Pawn pawn;

		// Token: 0x04008987 RID: 35207
		private MutantDef def;

		// Token: 0x04008988 RID: 35208
		public RotStage rotStage;

		// Token: 0x04008989 RID: 35209
		private bool hasTurned;

		// Token: 0x0400898A RID: 35210
		private Hediff mutantHediff;

		// Token: 0x0400898B RID: 35211
		public VerbTracker verbTracker;

		// Token: 0x0400898C RID: 35212
		private Faction originalFaction;

		// Token: 0x0400898D RID: 35213
		private Ideo originalIdeo;

		// Token: 0x0400898E RID: 35214
		private List<Ability> abilities;

		// Token: 0x02002467 RID: 9319
		public struct ThinkTrees
		{
			// Token: 0x0400898F RID: 35215
			public ThinkTreeDef main;

			// Token: 0x04008990 RID: 35216
			public ThinkTreeDef constant;
		}
	}
}
