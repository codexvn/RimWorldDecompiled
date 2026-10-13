using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002800 RID: 10240
	public class CompRitualEffect_Lightball : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x17002230 RID: 8752
		// (get) Token: 0x0600DFFB RID: 57339 RVA: 0x0041C38C File Offset: 0x0041A58C
		public new CompProperties_RitualEffectLightball Props
		{
			get
			{
				return (CompProperties_RitualEffectLightball)this.props;
			}
		}

		// Token: 0x0600DFFC RID: 57340 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x0600DFFD RID: 57341 RVA: 0x0041C39C File Offset: 0x0041A59C
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			CompPowerTrader compPowerTrader = ritual.selectedTarget.Thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader == null || !compPowerTrader.PowerOn)
			{
				return;
			}
			float num = Rand.Range(0f, 360f);
			float num2 = num + 180f;
			float num3 = (num + num2) / 2f + (float)Rand.Range(-55, 55);
			Vector3 a = this.parent.ritual.selectedTarget.Cell.ToVector3Shifted();
			Vector3 b = Quaternion.AngleAxis(num, Vector3.up) * Vector3.forward * this.Props.radius;
			Vector3 b2 = Quaternion.AngleAxis(num2, Vector3.up) * Vector3.forward * this.Props.radius;
			Vector3 b3 = Quaternion.AngleAxis(num3, Vector3.up) * Vector3.forward * this.Props.radius;
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b), new float?(num - 45f));
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b2), new float?(num2 - 45f));
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b3), new float?(num3 - 45f));
			this.lastSpawnTick = GenTicks.TicksGame;
		}
	}
}
