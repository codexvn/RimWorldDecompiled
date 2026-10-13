using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E74 RID: 11892
	[StaticConstructorOnStartup]
	public class CompTargetingBeam : ThingComp
	{
		// Token: 0x17002AFD RID: 11005
		// (get) Token: 0x06010BF2 RID: 68594 RVA: 0x004D383B File Offset: 0x004D1A3B
		private CompProperties_TargetingBeam Props
		{
			get
			{
				return (CompProperties_TargetingBeam)this.props;
			}
		}

		// Token: 0x06010BF3 RID: 68595 RVA: 0x004D3848 File Offset: 0x004D1A48
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.parent != null)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (pawn.stances != null)
					{
						Stance_Warmup stance_Warmup = pawn.stances.curStance as Stance_Warmup;
						if (stance_Warmup != null && stance_Warmup.verb != null && stance_Warmup.verb is Verb_Shoot)
						{
							GenDraw.DrawLineBetween(this.parent.TrueCenter(), stance_Warmup.verb.CurrentTarget.CenterVector3, CompTargetingBeam.LaserLineMat, 0.1f);
						}
					}
					return;
				}
			}
		}

		// Token: 0x0400AE7F RID: 44671
		private static Material LaserLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x0400AE80 RID: 44672
		private const float LineWidth = 0.1f;
	}
}
