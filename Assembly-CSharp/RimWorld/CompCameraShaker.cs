using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C34 RID: 11316
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170027E9 RID: 10217
		// (get) Token: 0x0600FDB9 RID: 64953 RVA: 0x004997BF File Offset: 0x004979BF
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x0600FDBA RID: 64954 RVA: 0x004997CC File Offset: 0x004979CC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned && this.parent.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
