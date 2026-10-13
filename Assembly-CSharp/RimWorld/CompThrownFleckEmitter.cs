using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E94 RID: 11924
	public class CompThrownFleckEmitter : ThingComp
	{
		// Token: 0x17002B27 RID: 11047
		// (get) Token: 0x06010CB4 RID: 68788 RVA: 0x004D5C17 File Offset: 0x004D3E17
		private CompProperties_ThrownFleckEmitter Props
		{
			get
			{
				return (CompProperties_ThrownFleckEmitter)this.props;
			}
		}

		// Token: 0x17002B28 RID: 11048
		// (get) Token: 0x06010CB5 RID: 68789 RVA: 0x004D5C24 File Offset: 0x004D3E24
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x17002B29 RID: 11049
		// (get) Token: 0x06010CB6 RID: 68790 RVA: 0x004D5CA5 File Offset: 0x004D3EA5
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x17002B2A RID: 11050
		// (get) Token: 0x06010CB7 RID: 68791 RVA: 0x004D5CC8 File Offset: 0x004D3EC8
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
				CompLoudspeaker comp4 = this.parent.GetComp<CompLoudspeaker>();
				if (comp4 != null && !comp4.Active)
				{
					return false;
				}
				CompHackable comp5 = this.parent.GetComp<CompHackable>();
				if (comp5 != null && comp5.IsHacked)
				{
					return false;
				}
				Building_Crate building_Crate = this.parent as Building_Crate;
				return building_Crate == null || building_Crate.HasAnyContents;
			}
		}

		// Token: 0x06010CB8 RID: 68792 RVA: 0x004D5DA0 File Offset: 0x004D3FA0
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

		// Token: 0x06010CB9 RID: 68793 RVA: 0x004D5E0C File Offset: 0x004D400C
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.parent.DrawPos + this.EmissionOffset, this.parent.Map, this.Props.fleck, this.Props.scale.RandomInRange);
				dataStatic.rotationRate = this.Props.rotationRate.RandomInRange;
				dataStatic.instanceColor = new Color?(this.EmissionColor);
				dataStatic.velocityAngle = this.Props.velocityX.RandomInRange;
				dataStatic.velocitySpeed = this.Props.velocityY.RandomInRange;
				this.parent.Map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x06010CBA RID: 68794 RVA: 0x004D5EE4 File Offset: 0x004D40E4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x0400AED9 RID: 44761
		public bool emittedBefore;

		// Token: 0x0400AEDA RID: 44762
		public int ticksSinceLastEmitted;
	}
}
