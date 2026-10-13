using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200377E RID: 14206
	public class Verb_FirefoamPop : Verb
	{
		// Token: 0x17003252 RID: 12882
		// (get) Token: 0x06013AE2 RID: 80610 RVA: 0x005D8C7A File Offset: 0x005D6E7A
		private CompExplosive Explosive
		{
			get
			{
				return base.EquipmentSource.GetComp<CompExplosive>();
			}
		}

		// Token: 0x17003253 RID: 12883
		// (get) Token: 0x06013AE3 RID: 80611 RVA: 0x005D8C87 File Offset: 0x005D6E87
		private CompApparelReloadable Reloadable
		{
			get
			{
				return base.EquipmentSource.GetComp<CompApparelReloadable>();
			}
		}

		// Token: 0x17003254 RID: 12884
		// (get) Token: 0x06013AE4 RID: 80612 RVA: 0x005D8C94 File Offset: 0x005D6E94
		private float Radius
		{
			get
			{
				return this.Explosive.ExplosiveRadius();
			}
		}

		// Token: 0x06013AE5 RID: 80613 RVA: 0x005D8CA1 File Offset: 0x005D6EA1
		protected override bool TryCastShot()
		{
			return Verb_FirefoamPop.Pop(this.caster, this.Explosive, this.Reloadable);
		}

		// Token: 0x06013AE6 RID: 80614 RVA: 0x005D8CBA File Offset: 0x005D6EBA
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			return this.Radius;
		}

		// Token: 0x06013AE7 RID: 80615 RVA: 0x005D8CC5 File Offset: 0x005D6EC5
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x06013AE8 RID: 80616 RVA: 0x005D8CD8 File Offset: 0x005D6ED8
		public static bool Pop(Thing caster, CompExplosive explosive, CompApparelReloadable reloadable)
		{
			string text;
			if (reloadable == null || explosive == null || !reloadable.CanBeUsed(out text))
			{
				return false;
			}
			explosive.StartWick(caster);
			reloadable.UsedOnce();
			return true;
		}
	}
}
