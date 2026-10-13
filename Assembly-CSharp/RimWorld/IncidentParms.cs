using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D59 RID: 7513
	public class IncidentParms : IExposable
	{
		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x0600A869 RID: 43113 RVA: 0x0030308C File Offset: 0x0030128C
		public int PawnGroupCount
		{
			get
			{
				Dictionary<Pawn, int> dictionary = this.pawnGroups;
				if (dictionary == null)
				{
					return 0;
				}
				return dictionary.Last<KeyValuePair<Pawn, int>>().Value + 1;
			}
		}

		// Token: 0x0600A86A RID: 43114 RVA: 0x003030B4 File Offset: 0x003012B4
		public IncidentParms ShallowCopy()
		{
			return (IncidentParms)base.MemberwiseClone();
		}

		// Token: 0x0600A86B RID: 43115 RVA: 0x003030C4 File Offset: 0x003012C4
		public void ExposeData()
		{
			Scribe_References.Look<IIncidentTarget>(ref this.target, "target", false);
			Scribe_Values.Look<float>(ref this.points, "threatPoints", 0f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.forced, "forced", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.letterHyperlinkThingDefs, "letterHyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.letterHyperlinkHediffDefs, "letterHyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalEnd, "inSignalEnd", null, false);
			Scribe_Values.Look<bool>(ref this.silent, "silent", false, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.pawnGroups != null)
			{
				this.pawnGroups.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key == null || x.Key.Destroyed);
			}
			Scribe_Values.Look<IntVec3>(ref this.spawnCenter, "spawnCenter", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.spawnRotation, "spawnRotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.dontUseSingleUseRocketLaunchers, "dontUseSingleUseRocketLaunchers", false, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.raidArrivalMode, "raidArrivalMode");
			Scribe_Values.Look<bool>(ref this.raidForceOneDowned, "raidForceOneDowned", false, false);
			Scribe_Values.Look<bool>(ref this.raidNeverFleeIndividual, "raidNeverFleeIndividual", false, false);
			Scribe_Values.Look<bool>(ref this.raidArrivalModeForQuickMilitaryAid, "raidArrivalModeForQuickMilitaryAid", false, false);
			Scribe_Defs.Look<RaidAgeRestrictionDef>(ref this.raidAgeRestriction, "raidAgeRestriction");
			Scribe_Values.Look<float>(ref this.biocodeWeaponsChance, "biocodeWeaponsChance", 0f, false);
			Scribe_Values.Look<float>(ref this.biocodeApparelChance, "biocodeApparelChance", 0f, false);
			Scribe_Collections.Look<Pawn, int>(ref this.pawnGroups, "pawnGroups", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpGroups, true, false, false);
			Scribe_Values.Look<int?>(ref this.pawnGroupMakerSeed, "pawnGroupMakerSeed", null, false);
			Scribe_References.Look<Ideo>(ref this.pawnIdeo, "pawnIdeo", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Defs.Look<PawnKindDef>(ref this.pawnKind, "pawnKind");
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Defs.Look<PawnGroupKindDef>(ref this.pawnGroupKind, "pawnGroupKind");
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScriptDef, "questScriptDef");
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.mechClusterSketch, "mechClusterSketch", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_References.Look<Pawn>(ref this.controllerPawn, "controllerPawn", false);
			Scribe_Values.Look<IntVec3?>(ref this.infestationLocOverride, "infestationLocOverride", null, false);
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.gifts, "gifts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.totalBodySize, "totalBodySize", 0f, false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.psychicRitualDef, "psychicRitualDef");
			Scribe_Values.Look<float>(ref this.pointMultiplier, "pointMultiplier", 1f, false);
			Scribe_Values.Look<bool>(ref this.bypassStorytellerSettings, "bypassStorytellerSettings", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.gifts;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0600A86C RID: 43116 RVA: 0x00303520 File Offset: 0x00301720
		public override string ToString()
		{
			string text = "";
			if (this.target != null)
			{
				string str = text;
				string str2 = "target=";
				IIncidentTarget incidentTarget = this.target;
				text = str + str2 + ((incidentTarget != null) ? incidentTarget.ToString() : null);
			}
			if (this.points >= 0f)
			{
				text = text + ", points=" + this.points.ToString();
			}
			if (this.faction != null)
			{
				text = string.Concat(new string[]
				{
					text,
					", faction=",
					this.faction.GetUniqueLoadID(),
					" (",
					this.faction.def.defName,
					")"
				});
			}
			if (this.generateFightersOnly)
			{
				text = text + ", generateFightersOnly=" + this.generateFightersOnly.ToString();
			}
			if (this.raidStrategy != null)
			{
				text = text + ", raidStrategy=" + this.raidStrategy.defName;
			}
			if (this.questScriptDef != null)
			{
				string str3 = text;
				string str4 = ", questScriptDef=";
				QuestScriptDef questScriptDef = this.questScriptDef;
				text = str3 + str4 + ((questScriptDef != null) ? questScriptDef.ToString() : null);
			}
			if (this.raidAgeRestriction != null)
			{
				string str5 = text;
				string str6 = ", raidAgeRestriction=";
				RaidAgeRestrictionDef raidAgeRestrictionDef = this.raidAgeRestriction;
				text = str5 + str6 + ((raidAgeRestrictionDef != null) ? raidAgeRestrictionDef.ToString() : null);
			}
			return text;
		}

		// Token: 0x04007474 RID: 29812
		public IIncidentTarget target;

		// Token: 0x04007475 RID: 29813
		public float points = -1f;

		// Token: 0x04007476 RID: 29814
		public Faction faction;

		// Token: 0x04007477 RID: 29815
		public bool forced;

		// Token: 0x04007478 RID: 29816
		public string customLetterLabel;

		// Token: 0x04007479 RID: 29817
		public string customLetterText;

		// Token: 0x0400747A RID: 29818
		public LetterDef customLetterDef;

		// Token: 0x0400747B RID: 29819
		public bool sendLetter = true;

		// Token: 0x0400747C RID: 29820
		public List<ThingDef> letterHyperlinkThingDefs;

		// Token: 0x0400747D RID: 29821
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x0400747E RID: 29822
		public string inSignalEnd;

		// Token: 0x0400747F RID: 29823
		public bool silent;

		// Token: 0x04007480 RID: 29824
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x04007481 RID: 29825
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x04007482 RID: 29826
		public bool generateFightersOnly;

		// Token: 0x04007483 RID: 29827
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04007484 RID: 29828
		public RaidStrategyDef raidStrategy;

		// Token: 0x04007485 RID: 29829
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x04007486 RID: 29830
		[LoadAlias("raidForceOneIncap")]
		public bool raidForceOneDowned;

		// Token: 0x04007487 RID: 29831
		public bool raidNeverFleeIndividual;

		// Token: 0x04007488 RID: 29832
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x04007489 RID: 29833
		public RaidAgeRestrictionDef raidAgeRestriction;

		// Token: 0x0400748A RID: 29834
		public float biocodeWeaponsChance;

		// Token: 0x0400748B RID: 29835
		public float biocodeApparelChance;

		// Token: 0x0400748C RID: 29836
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x0400748D RID: 29837
		public int? pawnGroupMakerSeed;

		// Token: 0x0400748E RID: 29838
		public Ideo pawnIdeo;

		// Token: 0x0400748F RID: 29839
		public Lord lord;

		// Token: 0x04007490 RID: 29840
		public PawnKindDef pawnKind;

		// Token: 0x04007491 RID: 29841
		public int pawnCount;

		// Token: 0x04007492 RID: 29842
		public PawnGroupKindDef pawnGroupKind;

		// Token: 0x04007493 RID: 29843
		public TraderKindDef traderKind;

		// Token: 0x04007494 RID: 29844
		public int podOpenDelay = 140;

		// Token: 0x04007495 RID: 29845
		public Quest quest;

		// Token: 0x04007496 RID: 29846
		public QuestScriptDef questScriptDef;

		// Token: 0x04007497 RID: 29847
		public string questTag;

		// Token: 0x04007498 RID: 29848
		public MechClusterSketch mechClusterSketch;

		// Token: 0x04007499 RID: 29849
		public bool canTimeoutOrFlee = true;

		// Token: 0x0400749A RID: 29850
		public bool canSteal = true;

		// Token: 0x0400749B RID: 29851
		public bool canKidnap = true;

		// Token: 0x0400749C RID: 29852
		public Pawn controllerPawn;

		// Token: 0x0400749D RID: 29853
		public IntVec3? infestationLocOverride;

		// Token: 0x0400749E RID: 29854
		public List<Thing> attackTargets;

		// Token: 0x0400749F RID: 29855
		public List<Thing> gifts;

		// Token: 0x040074A0 RID: 29856
		public float totalBodySize;

		// Token: 0x040074A1 RID: 29857
		public PsychicRitualDef psychicRitualDef;

		// Token: 0x040074A2 RID: 29858
		public float pointMultiplier = 1f;

		// Token: 0x040074A3 RID: 29859
		public bool bypassStorytellerSettings;

		// Token: 0x040074A4 RID: 29860
		public bool? canRoofPunch;

		// Token: 0x040074A5 RID: 29861
		public int dropInRadius = -1;

		// Token: 0x040074A6 RID: 29862
		public List<Pawn> storeGeneratedNeutralPawns;

		// Token: 0x040074A7 RID: 29863
		private List<Pawn> tmpPawns;

		// Token: 0x040074A8 RID: 29864
		private List<int> tmpGroups;
	}
}
