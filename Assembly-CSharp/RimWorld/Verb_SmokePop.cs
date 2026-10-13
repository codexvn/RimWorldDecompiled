using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200378B RID: 14219
	public class Verb_SmokePop : Verb
	{
		// Token: 0x06013B28 RID: 80680 RVA: 0x005DA51C File Offset: 0x005D871C
		protected override bool TryCastShot()
		{
			Verb_SmokePop.Pop(base.ReloadableCompSource);
			return true;
		}

		// Token: 0x06013B29 RID: 80681 RVA: 0x005DA52A File Offset: 0x005D872A
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return base.EquipmentSource.GetStatValue(StatDefOf.PackRadius, true, -1);
		}

		// Token: 0x06013B2A RID: 80682 RVA: 0x005D8CC5 File Offset: 0x005D6EC5
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x06013B2B RID: 80683 RVA: 0x005DA544 File Offset: 0x005D8744
		public static void Pop(CompApparelReloadable comp)
		{
			string text;
			if (comp == null || !comp.CanBeUsed(out text))
			{
				return;
			}
			Thing parent = comp.parent;
			Pawn wearer = comp.Wearer;
			float statValue = parent.GetStatValue(StatDefOf.PackRadius, true, -1);
			GenExplosion.DoExplosion(wearer.Position, wearer.Map, statValue, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.BlindSmoke), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			comp.UsedOnce();
		}
	}
}
