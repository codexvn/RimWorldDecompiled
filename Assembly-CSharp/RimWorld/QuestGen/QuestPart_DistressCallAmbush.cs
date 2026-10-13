using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B47 RID: 15175
	public class QuestPart_DistressCallAmbush : QuestPart
	{
		// Token: 0x06014A11 RID: 84497 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_DistressCallAmbush()
		{
		}

		// Token: 0x06014A12 RID: 84498 RVA: 0x0062902A File Offset: 0x0062722A
		public QuestPart_DistressCallAmbush(string inSignal, Site site, float points)
		{
			this.inSignal = inSignal;
			this.site = site;
			this.points = points;
		}

		// Token: 0x06014A13 RID: 84499 RVA: 0x00629048 File Offset: 0x00627248
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			List<Thing> list = (from b in this.site.Map.listerThings.ThingsOfDef(ThingDefOf.PitBurrow)
			where !b.Fogged()
			select b).ToList<Thing>();
			if (list.NullOrEmpty<Thing>())
			{
				return;
			}
			List<Pawn> source = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = this.points,
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), this.site.Map, null);
			Thing burrow = list.RandomElement<Thing>();
			Predicate<IntVec3> <>9__2;
			List<IntVec3> spawnPositions = source.Select(delegate(Pawn p)
			{
				IntVec3 position = burrow.Position;
				Map map = this.site.Map;
				int squareRadius = 2;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => c.Walkable(this.site.Map) && !c.Fogged(this.site.Map)));
				}
				IntVec3 result;
				if (!CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out result, -1))
				{
					return burrow.Position;
				}
				return result;
			}).ToList<IntVec3>();
			SpawnRequest request = new SpawnRequest(source.Cast<Thing>().ToList<Thing>(), spawnPositions, 1, 0.5f, lord);
			this.site.Map.deferredSpawner.AddRequest(request, true);
			Find.LetterStack.ReceiveLetter("DistressSignalAmbushLabel".Translate(), "DistressSignalAmbushText".Translate(), LetterDefOf.ThreatBig, burrow, null, null, null, null, 0, true);
		}

		// Token: 0x06014A14 RID: 84500 RVA: 0x006291A8 File Offset: 0x006273A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x0400E515 RID: 58645
		private string inSignal;

		// Token: 0x0400E516 RID: 58646
		private Site site;

		// Token: 0x0400E517 RID: 58647
		private float points;
	}
}
