using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB5 RID: 15285
	public class QuestPart_CleanUpVoidAwakening : QuestPart
	{
		// Token: 0x06014C1F RID: 85023 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_CleanUpVoidAwakening()
		{
		}

		// Token: 0x06014C20 RID: 85024 RVA: 0x0063827F File Offset: 0x0063647F
		public QuestPart_CleanUpVoidAwakening(string inSignal, Map map)
		{
			this.inSignal = inSignal;
			this.map = map;
		}

		// Token: 0x06014C21 RID: 85025 RVA: 0x00638298 File Offset: 0x00636498
		private void DestroyThingsOfDef(ThingDef def)
		{
			QuestPart_CleanUpVoidAwakening.tmpToDestroy.AddRange(this.map.listerThings.ThingsOfDef(def));
			for (int i = 0; i < QuestPart_CleanUpVoidAwakening.tmpToDestroy.Count; i++)
			{
				if (!QuestPart_CleanUpVoidAwakening.tmpToDestroy[i].Destroyed)
				{
					QuestPart_CleanUpVoidAwakening.tmpToDestroy[i].Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestPart_CleanUpVoidAwakening.tmpToDestroy.Clear();
		}

		// Token: 0x06014C22 RID: 85026 RVA: 0x00638304 File Offset: 0x00636504
		private void RemoveTerrainOfDef(TerrainDef def)
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				if (c.GetTerrain(this.map) == def)
				{
					this.map.terrainGrid.RemoveTopLayer(c, false);
				}
			}
		}

		// Token: 0x06014C23 RID: 85027 RVA: 0x00638370 File Offset: 0x00636570
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Thing.allowDestroyNonDestroyable = true;
			this.DestroyThingsOfDef(ThingDefOf.VoidStructure);
			this.DestroyThingsOfDef(ThingDefOf.PitBurrowSpawner);
			this.DestroyThingsOfDef(ThingDefOf.PitBurrow);
			this.DestroyThingsOfDef(ThingDefOf.Fleshbulb);
			this.DestroyThingsOfDef(ThingDefOf.VoidmetalMassSmall);
			this.DestroyThingsOfDef(ThingDefOf.VoidmetalMassMedium);
			Thing.allowDestroyNonDestroyable = false;
			this.RemoveTerrainOfDef(TerrainDefOf.Voidmetal);
			VoidAwakeningUtility.KillAllFreeEntities(this.map);
			GameCondition activeCondition = this.map.gameConditionManager.GetActiveCondition(GameConditionDefOf.UnnaturalDarkness);
			if (activeCondition == null)
			{
				return;
			}
			activeCondition.End();
		}

		// Token: 0x06014C24 RID: 85028 RVA: 0x00638414 File Offset: 0x00636614
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x06014C25 RID: 85029 RVA: 0x0063843F File Offset: 0x0063663F
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
		}

		// Token: 0x0400E71B RID: 59163
		private string inSignal;

		// Token: 0x0400E71C RID: 59164
		private Map map;

		// Token: 0x0400E71D RID: 59165
		private static List<Thing> tmpToDestroy = new List<Thing>();
	}
}
