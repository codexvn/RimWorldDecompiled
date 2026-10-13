using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200290D RID: 10509
	public class MetalHellTransition : MusicTransition
	{
		// Token: 0x0600E5ED RID: 58861 RVA: 0x00431658 File Offset: 0x0042F858
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.Map != null && MetalHellTransition.IsValidPocketMap(pocketMapParent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E5EE RID: 58862 RVA: 0x004316D4 File Offset: 0x0042F8D4
		private static bool IsValidPocketMap(PocketMapParent pocketMap)
		{
			return pocketMap.Map.generatorDef == MapGeneratorDefOf.MetalHell;
		}
	}
}
