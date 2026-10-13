using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002F56 RID: 12118
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x17002C2E RID: 11310
		// (get) Token: 0x06011153 RID: 69971 RVA: 0x004E7DB1 File Offset: 0x004E5FB1
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		// Token: 0x06011154 RID: 69972 RVA: 0x004E7DC0 File Offset: 0x004E5FC0
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(this.parent.Position, usedBy.MapHeld, false));
			}
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
