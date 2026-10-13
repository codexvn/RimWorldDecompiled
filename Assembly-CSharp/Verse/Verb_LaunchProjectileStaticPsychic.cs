using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D80 RID: 3456
	public class Verb_LaunchProjectileStaticPsychic : Verb_LaunchProjectileStatic
	{
		// Token: 0x0600582F RID: 22575 RVA: 0x001C6824 File Offset: 0x001C4A24
		public override void OnGUI(LocalTargetInfo target)
		{
			base.OnGUI(target);
			if (this.caster.Spawned && target.IsValid && this.CanHitTarget(target))
			{
				bool flag;
				float num = this.HighlightFieldRadiusAroundTarget(out flag);
				if (num > 0.2f)
				{
					foreach (IntVec3 c in DamageDefOf.Bomb.Worker.ExplosionCellsToHit(target.Cell, Find.CurrentMap, num, null, null, null))
					{
						if (!c.Fogged(Find.CurrentMap))
						{
							Pawn firstPawn = c.GetFirstPawn(Find.CurrentMap);
							if (firstPawn != null && firstPawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) < 1E-45f && !firstPawn.IsHiddenFromPlayer())
							{
								Verb_CastPsycast.DrawIneffectiveWarningStatic(firstPawn, null);
							}
						}
					}
				}
			}
		}
	}
}
