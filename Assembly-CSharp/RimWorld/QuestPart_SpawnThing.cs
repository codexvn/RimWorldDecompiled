using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB0 RID: 7344
	public class QuestPart_SpawnThing : QuestPart
	{
		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x0600A397 RID: 41879 RVA: 0x002EECC8 File Offset: 0x002ECEC8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.questLookTarget)
				{
					yield return this.innerSkyfallerThing ?? this.thing;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x0600A398 RID: 41880 RVA: 0x002EECD8 File Offset: 0x002ECED8
		public override bool IncreasesPopulation
		{
			get
			{
				Pawn pawn = this.thing as Pawn;
				return pawn != null && PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle<Pawn>(pawn), false, false);
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600A399 RID: 41881 RVA: 0x002EED03 File Offset: 0x002ECF03
		public MapParent MapParent
		{
			get
			{
				if (this.mapParentOfPawn == null)
				{
					return this.mapParent;
				}
				Map mapHeld = this.mapParentOfPawn.MapHeld;
				if (mapHeld == null)
				{
					return null;
				}
				return mapHeld.Parent;
			}
		}

		// Token: 0x0600A39A RID: 41882 RVA: 0x002EED2C File Offset: 0x002ECF2C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			if (this.MapParent == null || this.MapParent.Destroyed)
			{
				this.mapParentOfPawn = null;
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				this.cell = IntVec3.Invalid;
			}
			if (!this.MapParent.HasMap)
			{
				return;
			}
			IntVec3 center = IntVec3.Invalid;
			if (this.cell.IsValid)
			{
				center = this.cell;
			}
			else
			{
				Thing thing;
				if (this.tryLandInShipLandingZone && !DropCellFinder.TryFindShipLandingArea(this.MapParent.Map, out center, out thing))
				{
					if (thing != null)
					{
						Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(thing).CapitalizeFirst()), thing, MessageTypeDefOf.NeutralEvent, true);
					}
					center = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.MapParent.Map, this.thing.def.Size, this.factionForFindingSpot, 2);
				}
				if (!center.IsValid && this.tryLandNearThing != null)
				{
					DropCellFinder.FindSafeLandingSpotNearAvoidingHostiles(this.tryLandNearThing, this.MapParent.Map, out center, 35, 15, 25, new IntVec2?(this.thing.def.size), null);
				}
				if (!center.IsValid && (!this.lookForSafeSpot || !DropCellFinder.FindSafeLandingSpot(out center, this.factionForFindingSpot, this.MapParent.Map, 35, 15, 25, new IntVec2?(this.thing.def.size), null)))
				{
					IntVec3 intVec = DropCellFinder.RandomDropSpot(this.MapParent.Map, true);
					if (!DropCellFinder.TryFindDropSpotNear(intVec, this.MapParent.Map, out center, false, false, false, new IntVec2?(this.thing.def.size), true))
					{
						center = intVec;
					}
				}
			}
			GenPlace.TryPlaceThing(this.thing, center, this.MapParent.Map, ThingPlaceMode.Near, null, null, null, 1);
			this.spawned = true;
			if (this.spawnEffecter != null)
			{
				this.spawnEffecter.SpawnMaintained(this.thing, this.thing.Map, 1f);
			}
			Skyfaller skyfaller = this.thing as Skyfaller;
			if (skyfaller != null && skyfaller.innerContainer.Count == 1)
			{
				this.innerSkyfallerThing = skyfaller.innerContainer.First<Thing>();
				return;
			}
			this.innerSkyfallerThing = null;
		}

		// Token: 0x0600A39B RID: 41883 RVA: 0x002EEFB9 File Offset: 0x002ED1B9
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.thing || (this.thing is Skyfaller && ((Skyfaller)this.thing).innerContainer.Contains(p));
		}

		// Token: 0x0600A39C RID: 41884 RVA: 0x002EEFEC File Offset: 0x002ED1EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			if (!this.spawned && (this.thing == null || !(this.thing is Pawn)))
			{
				Scribe_Deep.Look<Thing>(ref this.thing, "thing", Array.Empty<object>());
			}
			else
			{
				Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.lookForSafeSpot, "lookForSafeSpot", false, false);
			Scribe_References.Look<Faction>(ref this.factionForFindingSpot, "factionForFindingSpot", false);
			Scribe_Values.Look<bool>(ref this.questLookTarget, "questLookTarget", true, false);
			Scribe_References.Look<Thing>(ref this.innerSkyfallerThing, "innerSkyfallerThing", false);
			Scribe_Values.Look<bool>(ref this.tryLandInShipLandingZone, "tryLandInShipLandingZone", false, false);
			Scribe_References.Look<Thing>(ref this.tryLandNearThing, "tryLandNearThing", false);
			Scribe_References.Look<Pawn>(ref this.mapParentOfPawn, "mapParentOfPawn", false);
			Scribe_Defs.Look<EffecterDef>(ref this.spawnEffecter, "spawnEffecter");
		}

		// Token: 0x0600A39D RID: 41885 RVA: 0x002EF120 File Offset: 0x002ED320
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x04007100 RID: 28928
		public string inSignal;

		// Token: 0x04007101 RID: 28929
		public Thing thing;

		// Token: 0x04007102 RID: 28930
		public Faction factionForFindingSpot;

		// Token: 0x04007103 RID: 28931
		public MapParent mapParent;

		// Token: 0x04007104 RID: 28932
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x04007105 RID: 28933
		public bool questLookTarget = true;

		// Token: 0x04007106 RID: 28934
		public bool lookForSafeSpot;

		// Token: 0x04007107 RID: 28935
		public bool tryLandInShipLandingZone;

		// Token: 0x04007108 RID: 28936
		public Thing tryLandNearThing;

		// Token: 0x04007109 RID: 28937
		public Pawn mapParentOfPawn;

		// Token: 0x0400710A RID: 28938
		public EffecterDef spawnEffecter;

		// Token: 0x0400710B RID: 28939
		private Thing innerSkyfallerThing;

		// Token: 0x0400710C RID: 28940
		private bool spawned;
	}
}
