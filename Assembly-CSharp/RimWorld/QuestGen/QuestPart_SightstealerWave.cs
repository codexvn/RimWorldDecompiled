using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B9A RID: 15258
	public class QuestPart_SightstealerWave : QuestPart_MakeLord
	{
		// Token: 0x06014B8D RID: 84877 RVA: 0x002DE416 File Offset: 0x002DC616
		public QuestPart_SightstealerWave()
		{
		}

		// Token: 0x06014B8E RID: 84878 RVA: 0x006348C3 File Offset: 0x00632AC3
		public QuestPart_SightstealerWave(string inSignal, MapParent mapParent, IEnumerable<Pawn> sightstealers, string waveTag)
		{
			this.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			this.mapParent = mapParent;
			this.pawns.AddRange(sightstealers);
			this.waveTag = waveTag;
		}

		// Token: 0x06014B8F RID: 84879 RVA: 0x00634902 File Offset: 0x00632B02
		protected override Lord MakeLord()
		{
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_SightstealerAssault(), this.mapParent.Map, null);
			QuestUtility.AddQuestTag(lord, this.waveTag);
			return lord;
		}

		// Token: 0x06014B90 RID: 84880 RVA: 0x0063492B File Offset: 0x00632B2B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.waveTag, "waveTag", null, false);
		}

		// Token: 0x0400E684 RID: 59012
		public string waveTag;
	}
}
