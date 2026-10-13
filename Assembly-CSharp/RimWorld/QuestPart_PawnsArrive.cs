using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF3 RID: 7411
	public class QuestPart_PawnsArrive : QuestPart
	{
		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x0600A581 RID: 42369 RVA: 0x002F62A7 File Offset: 0x002F44A7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.addPawnsToLookTargets)
				{
					foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
					{
						yield return t;
					}
					IEnumerator<Pawn> enumerator2 = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x0600A582 RID: 42370 RVA: 0x002F62B7 File Offset: 0x002F44B7
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, false);
			}
		}

		// Token: 0x0600A583 RID: 42371 RVA: 0x002F62CC File Offset: 0x002F44CC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				if (this.spawnNear.IsValid)
				{
					this.spawnNear = IntVec3.Invalid;
				}
			}
			if (this.mapParent == null || !this.mapParent.HasMap || !this.pawns.Any<Pawn>())
			{
				return;
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.joinPlayer && this.pawns[i].Faction != Faction.OfPlayer)
				{
					this.pawns[i].SetFaction(Faction.OfPlayer, null);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.mapParent.Map;
			incidentParms.spawnCenter = this.spawnNear;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			if (!pawnsArrivalModeDef.Worker.CanUseOnMap(this.mapParent.Map))
			{
				foreach (PawnsArrivalModeDef pawnsArrivalModeDef2 in DefDatabase<PawnsArrivalModeDef>.AllDefsListForReading.InRandomOrder(null))
				{
					if (pawnsArrivalModeDef2.canBeBackup && pawnsArrivalModeDef2.Worker.CanUseOnMap(this.mapParent.Map))
					{
						pawnsArrivalModeDef = pawnsArrivalModeDef2;
						break;
					}
				}
			}
			if (!pawnsArrivalModeDef.Worker.CanUseOnMap(this.mapParent.Map))
			{
				Log.Error(string.Format("Tried to do pawns arrive on map {0} but could not find a legal arrival mode, current method: {1}", this.mapParent.Map, pawnsArrivalModeDef.defName));
				return;
			}
			pawnsArrivalModeDef.Worker.TryResolveRaidSpawnCenter(incidentParms);
			pawnsArrivalModeDef.Worker.Arrive(this.pawns, incidentParms);
			if (this.sendStandardLetter)
			{
				TaggedString taggedString;
				TaggedString taggedString2;
				if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
				{
					taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
					taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
				}
				else
				{
					if (this.joinPlayer)
					{
						taggedString = "LetterPawnsArriveAndJoin".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
						taggedString2 = "LetterLabelPawnsArriveAndJoin".Translate();
					}
					else
					{
						taggedString = "LetterPawnsArrive".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
						taggedString2 = "LetterLabelPawnsArrive".Translate();
					}
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
				taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
				Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, this.pawns, null, this.quest, null, null, 0, true);
			}
		}

		// Token: 0x0600A584 RID: 42372 RVA: 0x002F66AC File Offset: 0x002F48AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.addPawnsToLookTargets, "addPawnsToLookTargets", true, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A585 RID: 42373 RVA: 0x002F67C0 File Offset: 0x002F49C0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.relations.everSeenByPlayer = true;
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.pawns.Add(pawn);
				this.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.joinPlayer = true;
			}
		}

		// Token: 0x0600A586 RID: 42374 RVA: 0x002F68DD File Offset: 0x002F4ADD
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A587 RID: 42375 RVA: 0x002F68EB File Offset: 0x002F4AEB
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04007254 RID: 29268
		public string inSignal;

		// Token: 0x04007255 RID: 29269
		public MapParent mapParent;

		// Token: 0x04007256 RID: 29270
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04007257 RID: 29271
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x04007258 RID: 29272
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x04007259 RID: 29273
		public bool joinPlayer;

		// Token: 0x0400725A RID: 29274
		public bool addPawnsToLookTargets = true;

		// Token: 0x0400725B RID: 29275
		public string customLetterText;

		// Token: 0x0400725C RID: 29276
		public string customLetterLabel;

		// Token: 0x0400725D RID: 29277
		public LetterDef customLetterDef;

		// Token: 0x0400725E RID: 29278
		public bool sendStandardLetter = true;
	}
}
