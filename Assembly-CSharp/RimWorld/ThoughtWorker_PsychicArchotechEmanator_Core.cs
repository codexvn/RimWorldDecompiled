using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BD RID: 6333
	public class ThoughtWorker_PsychicArchotechEmanator_Core : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06009098 RID: 37016 RVA: 0x002A996B File Offset: 0x002A7B6B
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.ArchonexusCore;
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06009099 RID: 37017 RVA: 0x002A9972 File Offset: 0x002A7B72
		protected override int InnerRadius
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x0600909A RID: 37018 RVA: 0x0019C7AC File Offset: 0x0019A9AC
		protected override int OuterRadius
		{
			get
			{
				return 28;
			}
		}
	}
}
