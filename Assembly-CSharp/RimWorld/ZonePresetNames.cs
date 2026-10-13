using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021D9 RID: 8665
	public static class ZonePresetNames
	{
		// Token: 0x0600BD75 RID: 48501 RVA: 0x0036EBE4 File Offset: 0x0036CDE4
		public static string PresetName(this StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				return "DumpingStockpile".Translate();
			}
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				return "Stockpile".Translate();
			}
			if (preset == StorageSettingsPreset.CorpseStockpile)
			{
				return "CorpseStockpile".Translate();
			}
			return "Zone".Translate();
		}
	}
}
