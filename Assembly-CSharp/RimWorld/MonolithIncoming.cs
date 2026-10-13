using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A90 RID: 10896
	public class MonolithIncoming : GroundSpawner
	{
		// Token: 0x170025E8 RID: 9704
		// (get) Token: 0x0600F29D RID: 62109 RVA: 0x00467448 File Offset: 0x00465648
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				return new IntRange(EffecterDefOf.VoidStructureIncoming.maintainTicks);
			}
		}

		// Token: 0x170025E9 RID: 9705
		// (get) Token: 0x0600F29E RID: 62110 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override SoundDef SustainerSound
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170025EA RID: 9706
		// (get) Token: 0x0600F29F RID: 62111 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool SpawnRubble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600F2A0 RID: 62112 RVA: 0x0046745C File Offset: 0x0046565C
		protected override void Spawn(Map map, IntVec3 pos)
		{
			Building_VoidMonolith t = Find.Anomaly.SpawnNewMonolith(pos, map);
			TaggedString taggedString = "MonolithArrivalText".Translate();
			if (Find.Anomaly.Level > 0)
			{
				taggedString += "\n\n" + "MonolithArrivalTextExt".Translate();
			}
			Find.LetterStack.ReceiveLetter("MonolithArrivalLabel".Translate(), taggedString, LetterDefOf.NeutralEvent, t, null, null, null, null, 0, true);
		}
	}
}
