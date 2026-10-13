using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008C9 RID: 2249
	public class Projectile_SpawnsPawnZeroAge : Projectile
	{
		// Token: 0x060038EC RID: 14572 RVA: 0x0012F9F4 File Offset: 0x0012DBF4
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			IntVec3 loc = base.Position;
			if (this.def.projectile.tryAdjacentFreeSpaces && base.Position.GetFirstBuilding(map) != null)
			{
				foreach (IntVec3 intVec in GenAdjFast.AdjacentCells8Way(base.Position))
				{
					if (intVec.GetFirstBuilding(map) == null && intVec.Standable(map))
					{
						loc = intVec;
						break;
					}
				}
			}
			PawnKindDef spawnsPawnKind = this.def.projectile.spawnsPawnKind;
			Faction faction = base.Launcher.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedChronologicalAge = new float?(0f);
			float? fixedBiologicalAge = new float?(0f);
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(spawnsPawnKind, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), loc, map, WipeMode.Vanish);
		}
	}
}
