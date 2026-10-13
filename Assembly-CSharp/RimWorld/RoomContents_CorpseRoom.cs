using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF5 RID: 7925
	public class RoomContents_CorpseRoom : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600AF70 RID: 44912 RVA: 0x00321086 File Offset: 0x0031F286
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(5, 10);
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600AF71 RID: 44913 RVA: 0x00321090 File Offset: 0x0031F290
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(4, 10);
			}
		}
	}
}
