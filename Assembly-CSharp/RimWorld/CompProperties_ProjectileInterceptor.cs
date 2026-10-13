using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DBB RID: 11707
	public class CompProperties_ProjectileInterceptor : CompProperties
	{
		// Token: 0x0601074E RID: 67406 RVA: 0x004BF1EC File Offset: 0x004BD3EC
		public CompProperties_ProjectileInterceptor()
		{
			this.compClass = typeof(CompProjectileInterceptor);
		}

		// Token: 0x0601074F RID: 67407 RVA: 0x004BF249 File Offset: 0x004BD449
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.hitPoints > 0 && this.chargeIntervalTicks > 0)
			{
				yield return "Cannot set both hitpoints and charge interval ticks.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AB9D RID: 43933
		public float radius;

		// Token: 0x0400AB9E RID: 43934
		public int cooldownTicks;

		// Token: 0x0400AB9F RID: 43935
		public int disarmedByEmpForTicks;

		// Token: 0x0400ABA0 RID: 43936
		public bool interceptGroundProjectiles;

		// Token: 0x0400ABA1 RID: 43937
		public bool interceptAirProjectiles;

		// Token: 0x0400ABA2 RID: 43938
		public bool interceptNonHostileProjectiles;

		// Token: 0x0400ABA3 RID: 43939
		public bool interceptOutgoingProjectiles;

		// Token: 0x0400ABA4 RID: 43940
		public bool drawWithNoSelection;

		// Token: 0x0400ABA5 RID: 43941
		public int chargeIntervalTicks;

		// Token: 0x0400ABA6 RID: 43942
		public int chargeDurationTicks;

		// Token: 0x0400ABA7 RID: 43943
		public float minAlpha;

		// Token: 0x0400ABA8 RID: 43944
		public float idlePulseSpeed = 0.7f;

		// Token: 0x0400ABA9 RID: 43945
		public float minIdleAlpha = -1.7f;

		// Token: 0x0400ABAA RID: 43946
		public int hitPoints = -1;

		// Token: 0x0400ABAB RID: 43947
		public int rechargeHitPointsIntervalTicks = 240;

		// Token: 0x0400ABAC RID: 43948
		[NoTranslate]
		public string gizmoTipKey;

		// Token: 0x0400ABAD RID: 43949
		public bool hitPointsRestoreInstantlyAfterCharge;

		// Token: 0x0400ABAE RID: 43950
		public bool startWithMaxHitPoints = true;

		// Token: 0x0400ABAF RID: 43951
		public bool alwaysShowHitpointsGizmo;

		// Token: 0x0400ABB0 RID: 43952
		public bool activated;

		// Token: 0x0400ABB1 RID: 43953
		public int activeDuration;

		// Token: 0x0400ABB2 RID: 43954
		public Color color = Color.white;

		// Token: 0x0400ABB3 RID: 43955
		public EffecterDef reactivateEffect;

		// Token: 0x0400ABB4 RID: 43956
		public EffecterDef interceptEffect;

		// Token: 0x0400ABB5 RID: 43957
		public SoundDef activeSound;
	}
}
