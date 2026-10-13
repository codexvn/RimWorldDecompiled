using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020025EC RID: 9708
	public struct IdeoGenerationParms
	{
		// Token: 0x0600D4F8 RID: 54520 RVA: 0x003EE3E8 File Offset: 0x003EC5E8
		public IdeoGenerationParms(FactionDef forFaction, bool forceNoExpansionIdeo = false, List<PreceptDef> disallowedPrecepts = null, List<MemeDef> disallowedMemes = null, List<MemeDef> forcedMemes = null, bool classicExtra = false, bool forceNoWeaponPreference = false, bool forNewFluidIdeo = false, bool fixedIdeo = false, string name = "", List<StyleCategoryDef> styles = null, List<DeityPreset> deities = null, bool hidden = false, string description = "", bool requiredPreceptsOnly = false)
		{
			this.forFaction = forFaction;
			this.forceNoExpansionIdeo = forceNoExpansionIdeo;
			this.disallowedPrecepts = disallowedPrecepts;
			this.disallowedMemes = disallowedMemes;
			this.forcedMemes = forcedMemes;
			this.classicExtra = classicExtra;
			this.forceNoWeaponPreference = forceNoWeaponPreference;
			this.forNewFluidIdeo = forNewFluidIdeo;
			this.fixedIdeo = fixedIdeo;
			this.name = name;
			this.styles = styles;
			this.deities = deities;
			this.hidden = hidden;
			this.description = description;
			this.requiredPreceptsOnly = requiredPreceptsOnly;
		}

		// Token: 0x040090A0 RID: 37024
		public FactionDef forFaction;

		// Token: 0x040090A1 RID: 37025
		public bool forceNoExpansionIdeo;

		// Token: 0x040090A2 RID: 37026
		public bool classicExtra;

		// Token: 0x040090A3 RID: 37027
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x040090A4 RID: 37028
		public List<MemeDef> disallowedMemes;

		// Token: 0x040090A5 RID: 37029
		public List<MemeDef> forcedMemes;

		// Token: 0x040090A6 RID: 37030
		public bool forceNoWeaponPreference;

		// Token: 0x040090A7 RID: 37031
		public bool forNewFluidIdeo;

		// Token: 0x040090A8 RID: 37032
		public bool fixedIdeo;

		// Token: 0x040090A9 RID: 37033
		public string name;

		// Token: 0x040090AA RID: 37034
		public List<StyleCategoryDef> styles;

		// Token: 0x040090AB RID: 37035
		public List<DeityPreset> deities;

		// Token: 0x040090AC RID: 37036
		public bool hidden;

		// Token: 0x040090AD RID: 37037
		public string description;

		// Token: 0x040090AE RID: 37038
		public bool requiredPreceptsOnly;
	}
}
