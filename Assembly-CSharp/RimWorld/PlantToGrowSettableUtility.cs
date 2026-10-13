using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021D3 RID: 8659
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x0600BD3E RID: 48446 RVA: 0x0036E647 File Offset: 0x0036C847
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			return new Command_SetPlantToGrow
			{
				defaultDesc = "CommandSelectPlantToGrowDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc12,
				settable = settable
			};
		}
	}
}
