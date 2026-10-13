using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200378C RID: 14220
	public class Verb_Spawn : Verb_CastBase
	{
		// Token: 0x06013B2D RID: 80685 RVA: 0x005DA5F4 File Offset: 0x005D87F4
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			GenSpawn.Spawn(this.verbProps.spawnDef, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.colonyWideTaleDef != null)
			{
				Pawn pawn = this.caster.Map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				TaleRecorder.RecordTale(this.verbProps.colonyWideTaleDef, new object[]
				{
					this.caster,
					pawn
				});
			}
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
