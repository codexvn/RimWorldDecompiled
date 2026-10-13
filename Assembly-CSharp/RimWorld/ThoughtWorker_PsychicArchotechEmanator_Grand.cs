using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BC RID: 6332
	public class ThoughtWorker_PsychicArchotechEmanator_Grand : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06009094 RID: 37012 RVA: 0x002A995C File Offset: 0x002A7B5C
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.GrandArchotechStructure;
			}
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06009095 RID: 37013 RVA: 0x002A9963 File Offset: 0x002A7B63
		protected override int InnerRadius
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x06009096 RID: 37014 RVA: 0x002A9967 File Offset: 0x002A7B67
		protected override int OuterRadius
		{
			get
			{
				return 18;
			}
		}
	}
}
