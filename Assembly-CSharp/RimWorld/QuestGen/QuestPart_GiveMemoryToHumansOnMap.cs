using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA8 RID: 15272
	public class QuestPart_GiveMemoryToHumansOnMap : QuestPart
	{
		// Token: 0x06014BD6 RID: 84950 RVA: 0x00636514 File Offset: 0x00634714
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				MapParent mapParent = this.mapParent;
				if (((mapParent != null) ? mapParent.Map : null) != null)
				{
					foreach (Pawn pawn in this.mapParent.Map.mapPawns.AllPawnsSpawned)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									MemoryThoughtHandler memories = thoughts.memories;
									if (memories != null)
									{
										memories.TryGainMemory(this.memory, null, null);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06014BD7 RID: 84951 RVA: 0x006365CC File Offset: 0x006347CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.memory, "memory");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0400E6D8 RID: 59096
		public ThoughtDef memory;

		// Token: 0x0400E6D9 RID: 59097
		public string inSignal;

		// Token: 0x0400E6DA RID: 59098
		public MapParent mapParent;
	}
}
