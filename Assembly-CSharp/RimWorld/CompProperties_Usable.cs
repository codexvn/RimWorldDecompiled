using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001989 RID: 6537
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x0600944A RID: 37962 RVA: 0x002B9018 File Offset: 0x002B7218
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x04005FD4 RID: 24532
		public JobDef useJob;

		// Token: 0x04005FD5 RID: 24533
		[MustTranslate]
		public string useLabel;

		// Token: 0x04005FD6 RID: 24534
		[MustTranslate]
		public string useMessage;

		// Token: 0x04005FD7 RID: 24535
		public int useDuration = 100;

		// Token: 0x04005FD8 RID: 24536
		public HediffDef userMustHaveHediff;

		// Token: 0x04005FD9 RID: 24537
		public MenuOptionPriority floatMenuOptionPriority = MenuOptionPriority.Default;

		// Token: 0x04005FDA RID: 24538
		public FactionDef floatMenuFactionIcon;

		// Token: 0x04005FDB RID: 24539
		public ThingDef warmupMote;

		// Token: 0x04005FDC RID: 24540
		public ThingDef finalizeMote;

		// Token: 0x04005FDD RID: 24541
		public bool ignoreOtherReservations;

		// Token: 0x04005FDE RID: 24542
		public bool showUseGizmo;

		// Token: 0x04005FDF RID: 24543
		public List<MutantDef> allowedMutants = new List<MutantDef>();

		// Token: 0x04005FE0 RID: 24544
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04005FE1 RID: 24545
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();
	}
}
