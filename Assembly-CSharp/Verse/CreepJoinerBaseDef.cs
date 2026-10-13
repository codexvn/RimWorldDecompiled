using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000220 RID: 544
	public class CreepJoinerBaseDef : Def, ICreepJoinerDef
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00043B23 File Offset: 0x00041D23
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00043B2B File Offset: 0x00041D2B
		public List<CreepJoinerBaseDef> Excludes
		{
			get
			{
				return this.excludes;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00043B33 File Offset: 0x00041D33
		public List<CreepJoinerBaseDef> Requires
		{
			get
			{
				return this.requires;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00043B3B File Offset: 0x00041D3B
		public float MinCombatPoints
		{
			get
			{
				return this.minCombatPoints;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00043B43 File Offset: 0x00041D43
		public bool CanOccurRandomly
		{
			get
			{
				return this.canOccurRandomly;
			}
		}

		// Token: 0x040008DD RID: 2269
		private float weight = 1f;

		// Token: 0x040008DE RID: 2270
		private float minCombatPoints;

		// Token: 0x040008DF RID: 2271
		private bool canOccurRandomly = true;

		// Token: 0x040008E0 RID: 2272
		private List<CreepJoinerBaseDef> excludes = new List<CreepJoinerBaseDef>();

		// Token: 0x040008E1 RID: 2273
		private List<CreepJoinerBaseDef> requires = new List<CreepJoinerBaseDef>();

		// Token: 0x040008E2 RID: 2274
		[MustTranslate]
		public string surgicalInspectionLetterExtra;
	}
}
