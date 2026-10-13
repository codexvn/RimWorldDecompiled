using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF9 RID: 7929
	public class RoomContents_ReactorCorpseRoom : RoomContents_DeadBody
	{
		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600AF8E RID: 44942 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override ThingDef KillerThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600AF8F RID: 44943 RVA: 0x0032135F File Offset: 0x0031F55F
		protected override DamageDef DamageType
		{
			get
			{
				return DamageDefOf.Bullet;
			}
		}

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600AF90 RID: 44944 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override Tool ToolUsed
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x0600AF91 RID: 44945 RVA: 0x00321366 File Offset: 0x0031F566
		protected override FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(300f, 900f);
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x0600AF92 RID: 44946 RVA: 0x0001BC74 File Offset: 0x00019E74
		protected override IntRange SurvivalPacksCountRange
		{
			get
			{
				return IntRange.Zero;
			}
		}

		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x0600AF93 RID: 44947 RVA: 0x00321377 File Offset: 0x0031F577
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(4, 6);
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x0600AF94 RID: 44948 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool AllHaveSameDeathAge
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AF95 RID: 44949 RVA: 0x00321380 File Offset: 0x0031F580
		protected override IEnumerable<PawnKindDef> GetPossibleKinds()
		{
			yield return PawnKindDefOf.Pirate;
			yield break;
		}
	}
}
