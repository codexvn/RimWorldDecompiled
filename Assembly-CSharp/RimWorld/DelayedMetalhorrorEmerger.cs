using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A93 RID: 10899
	public class DelayedMetalhorrorEmerger : Thing
	{
		// Token: 0x0600F2AA RID: 62122 RVA: 0x00467755 File Offset: 0x00465955
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.emergeTick = GenTicks.TicksGame + this.emergeDelayTicks;
			}
		}

		// Token: 0x0600F2AB RID: 62123 RVA: 0x00467774 File Offset: 0x00465974
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (GenTicks.TicksGame >= this.emergeTick)
			{
				Pawn[] array = base.Map.mapPawns.AllHumanlike.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					MetalhorrorUtility.TryEmerge(array[i], null, false);
				}
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F2AC RID: 62124 RVA: 0x004677CC File Offset: 0x004659CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelayTicks, "emergeDelayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.emergeTick, "emergeTick", 0, false);
		}

		// Token: 0x0600F2AD RID: 62125 RVA: 0x004677F8 File Offset: 0x004659F8
		public static DelayedMetalhorrorEmerger Spawn(Map map, int delayTicks)
		{
			DelayedMetalhorrorEmerger delayedMetalhorrorEmerger = (DelayedMetalhorrorEmerger)ThingMaker.MakeThing(ThingDefOf.DelayedMetalhorrorEmerger, null);
			delayedMetalhorrorEmerger.emergeDelayTicks = delayTicks;
			IntVec3 loc;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out loc, map, 0f, true, null))
			{
				return null;
			}
			GenSpawn.Spawn(delayedMetalhorrorEmerger, loc, map, WipeMode.Vanish);
			return delayedMetalhorrorEmerger;
		}

		// Token: 0x04009FE3 RID: 40931
		public int emergeDelayTicks;

		// Token: 0x04009FE4 RID: 40932
		private int emergeTick;
	}
}
