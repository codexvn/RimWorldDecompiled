using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002AC6 RID: 10950
	[StaticConstructorOnStartup]
	public class TunnelHiveSpawner : GroundSpawner
	{
		// Token: 0x0600F3FF RID: 62463 RVA: 0x0046E810 File Offset: 0x0046CA10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.spawnHive, "spawnHive", true, false);
			Scribe_Values.Look<float>(ref this.insectsPoints, "insectsPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
		}

		// Token: 0x0600F400 RID: 62464 RVA: 0x0046E860 File Offset: 0x0046CA60
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.spawnHive)
			{
				HiveUtility.SpawnHive(loc, map, WipeMode.FullRefund, false, true, true, false, true, true, false).questTags = this.questTags;
			}
			if (this.insectsPoints > 0f)
			{
				this.insectsPoints = Mathf.Max(this.insectsPoints, Hive.spawnablePawnKinds.Min((PawnKindDef x) => x.combatPower));
				float pointsLeft = this.insectsPoints;
				List<Pawn> list = new List<Pawn>();
				int num = 0;
				Func<PawnKindDef, bool> <>9__1;
				while (pointsLeft > 0f)
				{
					num++;
					if (num > 1000)
					{
						Log.Error("Too many iterations.");
						break;
					}
					IEnumerable<PawnKindDef> spawnablePawnKinds = Hive.spawnablePawnKinds;
					Func<PawnKindDef, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnKindDef x) => x.combatPower <= pointsLeft));
					}
					PawnKindDef pawnKindDef;
					if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
					{
						break;
					}
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfInsects, null);
					GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
					pawn.mindState.spawnedByInfestationThingComp = this.spawnedByInfestationThingComp;
					list.Add(pawn);
					pointsLeft -= pawnKindDef.combatPower;
					if (ModsConfig.BiotechActive)
					{
						PollutionUtility.Notify_TunnelHiveSpawnedInsect(pawn);
					}
				}
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(Faction.OfInsects, true, false, false, false, true, false, false), map, list);
				}
			}
		}

		// Token: 0x0400A0D9 RID: 41177
		public bool spawnHive = true;

		// Token: 0x0400A0DA RID: 41178
		public float insectsPoints;

		// Token: 0x0400A0DB RID: 41179
		public bool spawnedByInfestationThingComp;
	}
}
