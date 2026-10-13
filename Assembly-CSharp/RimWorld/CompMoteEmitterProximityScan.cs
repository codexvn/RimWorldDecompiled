using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002D6A RID: 11626
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		// Token: 0x1700298F RID: 10639
		// (get) Token: 0x0601055A RID: 66906 RVA: 0x004B71DF File Offset: 0x004B53DF
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		// Token: 0x17002990 RID: 10640
		// (get) Token: 0x0601055B RID: 66907 RVA: 0x004B71EC File Offset: 0x004B53EC
		private CompSendSignalOnMotion ProximityComp
		{
			get
			{
				CompSendSignalOnMotion result;
				if ((result = this.proximityCompCached) == null)
				{
					result = (this.proximityCompCached = this.parent.GetComp<CompSendSignalOnMotion>());
				}
				return result;
			}
		}

		// Token: 0x0601055C RID: 66908 RVA: 0x004B7217 File Offset: 0x004B5417
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0601055D RID: 66909 RVA: 0x004B723C File Offset: 0x004B543C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.ProximityComp == null || this.ProximityComp.Sent)
			{
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
				return;
			}
			if (this.mote == null)
			{
				this.Emit();
			}
			if (!this.Props.soundEmitting.NullOrUndefined())
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundEmitting.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
			}
			if (this.mote == null)
			{
				return;
			}
			this.mote.Maintain();
			float a;
			if (!this.ProximityComp.Enabled)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
				float num = (float)this.ticksSinceLastEmitted / 60f;
				if (num <= this.Props.warmupPulseFadeInTime)
				{
					if (this.Props.warmupPulseFadeInTime > 0f)
					{
						a = num / this.Props.warmupPulseFadeInTime;
					}
					else
					{
						a = 1f;
					}
				}
				else if (num <= this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime)
				{
					a = 1f;
				}
				else if (this.Props.warmupPulseFadeOutTime > 0f)
				{
					a = 1f - Mathf.InverseLerp(this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime, this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime + this.Props.warmupPulseFadeOutTime, num);
				}
				else
				{
					a = 1f;
				}
			}
			else
			{
				a = 1f;
			}
			this.mote.instanceColor.a = a;
		}

		// Token: 0x0400AA69 RID: 43625
		private CompSendSignalOnMotion proximityCompCached;

		// Token: 0x0400AA6A RID: 43626
		private Sustainer sustainer;
	}
}
