using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC4 RID: 12228
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x17002CAD RID: 11437
		// (get) Token: 0x060113C5 RID: 70597 RVA: 0x004F040B File Offset: 0x004EE60B
		protected override Color BGColor
		{
			get
			{
				return Alert_Critical.BgColor();
			}
		}

		// Token: 0x060113C6 RID: 70598 RVA: 0x004F0412 File Offset: 0x004EE612
		public static Color BgColor()
		{
			float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
			return new Color(num, num, num) * Color.red;
		}

		// Token: 0x17002CAE RID: 11438
		// (get) Token: 0x060113C7 RID: 70599 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool DoMessage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060113C8 RID: 70600 RVA: 0x004F0440 File Offset: 0x004EE640
		public override void AlertActiveUpdate()
		{
			if (this.DoMessage && this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(this.GetReport().AllCulprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x060113C9 RID: 70601 RVA: 0x004F04AC File Offset: 0x004EE6AC
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x0400B2EE RID: 45806
		private int lastActiveFrame = -1;

		// Token: 0x0400B2EF RID: 45807
		private const float PulseFreq = 0.5f;

		// Token: 0x0400B2F0 RID: 45808
		private const float PulseAmpCritical = 0.6f;
	}
}
