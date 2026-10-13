using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF8 RID: 7928
	public class RoomContents_DeadBodyLabyrinth : RoomContents_DeadBody
	{
		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x0600AF8A RID: 44938 RVA: 0x00321337 File Offset: 0x0031F537
		protected override ThingDef KillerThing
		{
			get
			{
				return ThingDefOf.Fingerspike;
			}
		}

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600AF8B RID: 44939 RVA: 0x0032133E File Offset: 0x0031F53E
		protected override DamageDef DamageType
		{
			get
			{
				return DamageDefOf.Scratch;
			}
		}

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600AF8C RID: 44940 RVA: 0x00321345 File Offset: 0x0031F545
		protected override Tool ToolUsed
		{
			get
			{
				return ThingDefOf.Fingerspike.tools[0];
			}
		}
	}
}
