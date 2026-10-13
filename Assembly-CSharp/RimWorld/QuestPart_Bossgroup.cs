using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF1 RID: 7153
	public class QuestPart_Bossgroup : QuestPart_MakeLord
	{
		// Token: 0x06009F4D RID: 40781 RVA: 0x002DFFE4 File Offset: 0x002DE1E4
		protected override Lord MakeLord()
		{
			LordJob_BossgroupAssaultColony lordJob = new LordJob_BossgroupAssaultColony(this.faction, this.stageLocation, this.bosses);
			return LordMaker.MakeNewLord(this.faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x06009F4E RID: 40782 RVA: 0x002E0024 File Offset: 0x002DE224
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.stageLocation, "stageLocation", default(IntVec3), false);
			Scribe_Collections.Look<Pawn>(ref this.bosses, "bosses", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.bosses.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006E28 RID: 28200
		public List<Pawn> bosses = new List<Pawn>();

		// Token: 0x04006E29 RID: 28201
		public IntVec3 stageLocation;
	}
}
