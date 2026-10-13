using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B13 RID: 15123
	public class QuestPart_SleepingMechs : QuestPart
	{
		// Token: 0x06014939 RID: 84281 RVA: 0x00623900 File Offset: 0x00621B00
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				this.SpawnMechs();
				return;
			}
			if (!this.wokeUp)
			{
				if (signal.tag == this.inSignalTookDamage)
				{
					if ((float)this.defendThing.HitPoints < (float)this.defendThing.MaxHitPoints * 0.75f)
					{
						this.TryWakeMechsUp();
						return;
					}
				}
				else if (signal.tag == this.inSignalLockedOut)
				{
					this.TryWakeMechsUp();
				}
			}
		}

		// Token: 0x0601493A RID: 84282 RVA: 0x00623984 File Offset: 0x00621B84
		private void TryWakeMechsUp()
		{
			if (this.wokeUp)
			{
				return;
			}
			this.wokeUp = true;
			if (this.lord != null)
			{
				foreach (Pawn thing in this.lord.ownedPawns)
				{
					thing.TryGetComp<CompCanBeDormant>().WakeUp();
				}
			}
		}

		// Token: 0x0601493B RID: 84283 RVA: 0x006239F8 File Offset: 0x00621BF8
		private void SpawnMechs()
		{
			MapParent mapParent = this.mapParent;
			Map map = (mapParent != null) ? mapParent.Map : null;
			if (map == null)
			{
				return;
			}
			float num = QuestPart_SleepingMechs.ThreatPointsToMechPointsCurve.Evaluate(this.points);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Find.FactionManager.FirstFactionOfDef(FactionDefOf.Mechanoid),
				points = num
			}, true).ToList<Pawn>();
			if (!list.Any<Pawn>())
			{
				return;
			}
			this.lord = LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenMechanoidsDefend(new List<Thing>
			{
				this.defendThing
			}, Faction.OfMechanoids, 40f, this.defendThing.Position, false, false), map, null);
			foreach (Pawn pawn in list)
			{
				GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.defendThing.Position, map, 5, null), map, WipeMode.Vanish);
				this.lord.AddPawn(pawn);
				CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
				compCanBeDormant.ToSleep();
				compCanBeDormant.wakeUpSignalTag = this.inSignal;
			}
			Building building = this.defendThing as Building;
			if (building != null)
			{
				this.lord.AddBuilding(building);
			}
		}

		// Token: 0x0601493C RID: 84284 RVA: 0x00623B54 File Offset: 0x00621D54
		public override void Cleanup()
		{
			this.defendThing = null;
			this.mapParent = null;
			this.lord = null;
		}

		// Token: 0x0601493D RID: 84285 RVA: 0x00623B6C File Offset: 0x00621D6C
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.inSignalTookDamage, "inSignalTookDamage", null, false);
			Scribe_Values.Look<string>(ref this.inSignalLockedOut, "inSignalLockedOut", null, false);
			Scribe_References.Look<Thing>(ref this.defendThing, "defendThing", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x0400E47F RID: 58495
		public string inSignal;

		// Token: 0x0400E480 RID: 58496
		public string inSignalTookDamage;

		// Token: 0x0400E481 RID: 58497
		public string inSignalLockedOut;

		// Token: 0x0400E482 RID: 58498
		public Thing defendThing;

		// Token: 0x0400E483 RID: 58499
		public MapParent mapParent;

		// Token: 0x0400E484 RID: 58500
		public float points;

		// Token: 0x0400E485 RID: 58501
		private Lord lord;

		// Token: 0x0400E486 RID: 58502
		private bool wokeUp;

		// Token: 0x0400E487 RID: 58503
		private const float HPThresholdWakeUp = 0.75f;

		// Token: 0x0400E488 RID: 58504
		private static readonly SimpleCurve ThreatPointsToMechPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(1000f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 2000f),
				true
			}
		};
	}
}
