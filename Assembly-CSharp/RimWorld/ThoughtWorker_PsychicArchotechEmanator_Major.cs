using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BB RID: 6331
	public class ThoughtWorker_PsychicArchotechEmanator_Major : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06009090 RID: 37008 RVA: 0x002A9946 File Offset: 0x002A7B46
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.MajorArchotechStructureStudiable;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06009091 RID: 37009 RVA: 0x002A994D File Offset: 0x002A7B4D
		protected override int InnerRadius
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x06009092 RID: 37010 RVA: 0x002A9950 File Offset: 0x002A7B50
		protected override int OuterRadius
		{
			get
			{
				return 14;
			}
		}
	}
}
