using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206D RID: 8301
	public static class ManhunterPackGenStepUtility
	{
		// Token: 0x0600B621 RID: 46625 RVA: 0x0034509D File Offset: 0x0034329D
		public static bool TryGetAnimalsKind(float points, PlanetTile tile, out PawnKindDef animalKind)
		{
			return AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, tile, out animalKind) || AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, PlanetTile.Invalid, out animalKind);
		}
	}
}
