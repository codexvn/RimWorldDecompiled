using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B23 RID: 15139
	public class QuestPart_SpawnPawnsInStructure : QuestPart
	{
		// Token: 0x0601497C RID: 84348 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_SpawnPawnsInStructure()
		{
		}

		// Token: 0x0601497D RID: 84349 RVA: 0x00624FE1 File Offset: 0x006231E1
		public QuestPart_SpawnPawnsInStructure(List<Pawn> pawns, string inSignal)
		{
			this.pawns = pawns;
			this.inSignal = inSignal;
		}

		// Token: 0x0601497E RID: 84350 RVA: 0x00624FF7 File Offset: 0x006231F7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0601497F RID: 84351 RVA: 0x00625028 File Offset: 0x00623228
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			try
			{
				QuestPart_SpawnPawnsInStructure.<>c__DisplayClass5_0 CS$<>8__locals1 = new QuestPart_SpawnPawnsInStructure.<>c__DisplayClass5_0();
				MapParent mapParent;
				signal.args.TryGetArg<MapParent>("SUBJECT", out mapParent);
				CS$<>8__locals1.map = mapParent.Map;
				CellRect var = MapGenerator.GetVar<CellRect>("SpawnRect");
				foreach (Thing newThing in this.pawns)
				{
					IntVec3 loc;
					var.TryFindRandomCell(out loc, new Predicate<IntVec3>(CS$<>8__locals1.<Notify_QuestSignalReceived>g__Validator|0));
					GenSpawn.Spawn(newThing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				}
				LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_SitePawns(this.pawns.First<Pawn>().Faction, var.CenterCell, 180000), CS$<>8__locals1.map, this.pawns);
				this.pawns.Clear();
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Failed to generate pawns in structure: {0}", arg));
			}
		}

		// Token: 0x0400E4A1 RID: 58529
		private List<Pawn> pawns;

		// Token: 0x0400E4A2 RID: 58530
		private string inSignal;
	}
}
