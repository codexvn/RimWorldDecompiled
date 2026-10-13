using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D03 RID: 7427
	public class QuestPart_RandomRaid : QuestPart
	{
		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x0600A5F0 RID: 42480 RVA: 0x002F8147 File Offset: 0x002F6347
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
				yield break;
				yield break;
			}
		}

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x0600A5F1 RID: 42481 RVA: 0x002F8157 File Offset: 0x002F6357
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A5F2 RID: 42482 RVA: 0x002F8168 File Offset: 0x002F6368
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.fallbackToPlayerHomeMap && (this.mapParent == null || !this.mapParent.HasMap) && Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.AnyPlayerHomeMap.Parent;
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.forced = true;
				incidentParms.quest = this.quest;
				incidentParms.target = this.mapParent.Map;
				incidentParms.points = (this.useCurrentThreatPoints ? (StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) * this.currentThreatPointsFactor) : this.pointsRange.RandomInRange);
				incidentParms.faction = this.faction;
				incidentParms.customLetterLabel = signal.args.GetFormattedText(this.customLetterLabel);
				incidentParms.customLetterText = signal.args.GetFormattedText(this.customLetterText).Resolve();
				incidentParms.attackTargets = this.attackTargets;
				incidentParms.generateFightersOnly = this.generateFightersOnly;
				incidentParms.sendLetter = this.sendLetter;
				if (this.arrivalMode != null)
				{
					incidentParms.raidArrivalMode = this.arrivalMode;
				}
				IncidentDef incidentDef;
				if (this.faction == null || this.faction.HostileTo(Faction.OfPlayer))
				{
					incidentDef = IncidentDefOf.RaidEnemy;
				}
				else
				{
					incidentDef = IncidentDefOf.RaidFriendly;
				}
				if (this.raidStrategy != null)
				{
					incidentParms.raidStrategy = this.raidStrategy;
				}
				if (this.faction != null)
				{
					incidentParms.points = Mathf.Max(incidentParms.points, this.faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
				}
				if (incidentDef.Worker.CanFireNow(incidentParms))
				{
					incidentDef.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x0600A5F3 RID: 42483 RVA: 0x002F8350 File Offset: 0x002F6550
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", true, false);
			Scribe_Values.Look<bool>(ref this.fallbackToPlayerHomeMap, "fallbackToPlayerHomeMap", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0600A5F4 RID: 42484 RVA: 0x002F84A4 File Offset: 0x002F66A4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.pointsRange = new FloatRange(500f, 1500f);
			}
		}

		// Token: 0x040072A4 RID: 29348
		public string inSignal;

		// Token: 0x040072A5 RID: 29349
		public MapParent mapParent;

		// Token: 0x040072A6 RID: 29350
		public FloatRange pointsRange;

		// Token: 0x040072A7 RID: 29351
		public Faction faction;

		// Token: 0x040072A8 RID: 29352
		public bool useCurrentThreatPoints;

		// Token: 0x040072A9 RID: 29353
		public float currentThreatPointsFactor = 1f;

		// Token: 0x040072AA RID: 29354
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x040072AB RID: 29355
		public RaidStrategyDef raidStrategy;

		// Token: 0x040072AC RID: 29356
		public string customLetterLabel;

		// Token: 0x040072AD RID: 29357
		public string customLetterText;

		// Token: 0x040072AE RID: 29358
		public List<Thing> attackTargets;

		// Token: 0x040072AF RID: 29359
		public bool generateFightersOnly;

		// Token: 0x040072B0 RID: 29360
		public bool fallbackToPlayerHomeMap;

		// Token: 0x040072B1 RID: 29361
		public bool sendLetter = true;
	}
}
