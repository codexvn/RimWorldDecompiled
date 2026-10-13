using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002AA3 RID: 10915
	public class InsectAmbush : Thing
	{
		// Token: 0x0600F327 RID: 62247 RVA: 0x00469813 File Offset: 0x00467A13
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
		}

		// Token: 0x0600F328 RID: 62248 RVA: 0x00469834 File Offset: 0x00467A34
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.radius = InsectAmbush.RadiusRange.RandomInRange;
		}

		// Token: 0x0600F329 RID: 62249 RVA: 0x0046985C File Offset: 0x00467A5C
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			int num = GenRadial.NumCellsInRadius(this.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = base.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					foreach (Thing thing in c.GetThingList(map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonistPlayerControlled && GenSight.LineOfSightToThing(pawn.Position, this, base.Map, false, null))
						{
							this.Activate();
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600F32A RID: 62250 RVA: 0x0046992C File Offset: 0x00467B2C
		private void Activate()
		{
			float points = InsectAmbush.AmbushThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				points = points,
				faction = Faction.OfInsects
			}, true).ToList<Pawn>();
			List<Thing> list2 = new List<Thing>();
			foreach (Pawn pawnToSpawn in list)
			{
				IntVec3 loc;
				if (CellFinder.TryFindRandomReachableNearbyCell(base.Position, base.Map, 5f, TraverseParms.For(TraverseMode.ByPawn, Danger.Deadly, false, false, false, true, false), (IntVec3 cell) => cell.Standable(base.Map), null, out loc, 999999))
				{
					PawnGroundSpawner pawnGroundSpawner = (PawnGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PawnGroundSpawner, null);
					pawnGroundSpawner.Init(pawnToSpawn, InsectAmbush.TunnelDelayTicks);
					GenSpawn.Spawn(pawnGroundSpawner, loc, base.Map, WipeMode.Vanish);
					list2.Add(pawnGroundSpawner);
				}
			}
			Find.LetterStack.ReceiveLetter("InsectAmbushLetter".Translate(), "InsectAmbushLetterText".Translate(), LetterDefOf.ThreatBig, list2, null, null, null, null, 0, true);
			LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(), base.Map, list);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0400A02B RID: 41003
		private static readonly IntRange TunnelDelayTicks = new IntRange(240, 480);

		// Token: 0x0400A02C RID: 41004
		private static readonly FloatRange RadiusRange = new FloatRange(5f, 8f);

		// Token: 0x0400A02D RID: 41005
		private static readonly SimpleCurve AmbushThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 300f),
				true
			},
			{
				new CurvePoint(5000f, 1500f),
				true
			}
		};

		// Token: 0x0400A02E RID: 41006
		private float radius;
	}
}
