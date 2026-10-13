using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E97 RID: 11927
	public class CompThrownMoteEmitter : ThingComp
	{
		// Token: 0x17002B2D RID: 11053
		// (get) Token: 0x06010CC6 RID: 68806 RVA: 0x004D602F File Offset: 0x004D422F
		private CompProperties_ThrownMoteEmitter Props
		{
			get
			{
				return (CompProperties_ThrownMoteEmitter)this.props;
			}
		}

		// Token: 0x17002B2E RID: 11054
		// (get) Token: 0x06010CC7 RID: 68807 RVA: 0x004D603C File Offset: 0x004D423C
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x17002B2F RID: 11055
		// (get) Token: 0x06010CC8 RID: 68808 RVA: 0x004D60BD File Offset: 0x004D42BD
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x17002B30 RID: 11056
		// (get) Token: 0x06010CC9 RID: 68809 RVA: 0x004D60E0 File Offset: 0x004D42E0
		private bool IsOn
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
				CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
				if (comp2 != null && comp2.ticksLeft <= 0)
				{
					return false;
				}
				Building_MusicalInstrument building_MusicalInstrument = this.parent as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && !building_MusicalInstrument.IsBeingPlayed)
				{
					return false;
				}
				CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
				if (comp3 != null && !comp3.Initiated)
				{
					return false;
				}
				Skyfaller skyfaller = this.parent as Skyfaller;
				return skyfaller == null || !skyfaller.FadingOut;
			}
		}

		// Token: 0x06010CCA RID: 68810 RVA: 0x004D6180 File Offset: 0x004D4380
		public override void CompTick()
		{
			if (!this.IsOn)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
				if (!this.emittedBefore)
				{
					this.Emit();
					this.emittedBefore = true;
				}
				return;
			}
			if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
			{
				this.Emit();
				this.ticksSinceLastEmitted = 0;
				return;
			}
			this.ticksSinceLastEmitted++;
		}

		// Token: 0x06010CCB RID: 68811 RVA: 0x004D61EC File Offset: 0x004D43EC
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(this.Props.mote, null);
				moteThrown.Scale = this.Props.scale.RandomInRange;
				moteThrown.rotationRate = this.Props.rotationRate.RandomInRange;
				moteThrown.exactPosition = this.parent.DrawPos + this.EmissionOffset;
				moteThrown.instanceColor = this.EmissionColor;
				moteThrown.SetVelocity(this.Props.velocityX.RandomInRange, this.Props.velocityY.RandomInRange);
				if (moteThrown.exactPosition.ToIntVec3().InBounds(this.parent.Map))
				{
					GenSpawn.Spawn(moteThrown, moteThrown.exactPosition.ToIntVec3(), this.parent.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06010CCC RID: 68812 RVA: 0x004D62E1 File Offset: 0x004D44E1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x0400AEEA RID: 44778
		public bool emittedBefore;

		// Token: 0x0400AEEB RID: 44779
		public int ticksSinceLastEmitted;
	}
}
