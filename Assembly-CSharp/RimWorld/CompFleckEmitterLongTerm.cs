using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D20 RID: 11552
	public class CompFleckEmitterLongTerm : ThingComp
	{
		// Token: 0x1700293A RID: 10554
		// (get) Token: 0x060103B1 RID: 66481 RVA: 0x004B17B3 File Offset: 0x004AF9B3
		// (set) Token: 0x060103B2 RID: 66482 RVA: 0x004B17BB File Offset: 0x004AF9BB
		public bool Enabled { get; set; }

		// Token: 0x1700293B RID: 10555
		// (get) Token: 0x060103B3 RID: 66483 RVA: 0x004B17C4 File Offset: 0x004AF9C4
		private CompProperties_FleckEmitterLongTerm Props
		{
			get
			{
				return (CompProperties_FleckEmitterLongTerm)this.props;
			}
		}

		// Token: 0x1700293C RID: 10556
		// (get) Token: 0x060103B4 RID: 66484 RVA: 0x004B17D1 File Offset: 0x004AF9D1
		private bool ShouldEmit
		{
			get
			{
				return this.Enabled || this.Props.forceEnabled;
			}
		}

		// Token: 0x060103B5 RID: 66485 RVA: 0x004B17E8 File Offset: 0x004AF9E8
		private void EmissionTick(IFleckCreator fleckDestination)
		{
			if (this.ShouldEmit && Rand.Value < this.Props.spawnChance)
			{
				CompProperties_FleckEmitterLongTerm props = this.Props;
				Vector3 vector = this.parent.TrueCenter() + props.spawnOffsetFromCenter + Rand.InsideUnitCircleVec3 * props.spawnRadius;
				if (!vector.ToIntVec3().ShouldSpawnMotesAt(this.parent.Map, true))
				{
					return;
				}
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, this.parent.Map, props.fleckDef, props.fleckScale);
				dataStatic.rotationRate = Rand.Range(props.minRotationRate, props.maxRotationRate);
				dataStatic.velocityAngle = Rand.Range(props.minVelocityAngle, props.maxVelocityAngle);
				dataStatic.velocitySpeed = Rand.Range(props.minVelocitySpeed, props.maxVelocitySpeed);
				if (fleckDestination is FleckSystem)
				{
					dataStatic.spawnPosition.y = dataStatic.def.altitudeLayer.AltitudeFor(dataStatic.def.altitudeLayerIncOffset);
				}
				fleckDestination.CreateFleck(dataStatic);
			}
		}

		// Token: 0x060103B6 RID: 66486 RVA: 0x004B1900 File Offset: 0x004AFB00
		public void Prewarm()
		{
			if (!this.ShouldEmit || this.Props.prewarmCycles <= 0f)
			{
				return;
			}
			FleckSystem system = this.parent.Map.flecks.CreateFleckSystemFor(this.Props.fleckDef);
			system.Prewarm(this.Props.fleckDef.Lifetime * this.Props.prewarmCycles, null, delegate
			{
				this.EmissionTick(system);
			});
			this.parent.Map.flecks.HandOverSystem(system);
		}

		// Token: 0x060103B7 RID: 66487 RVA: 0x004B19AA File Offset: 0x004AFBAA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.Prewarm();
		}

		// Token: 0x060103B8 RID: 66488 RVA: 0x004B19B2 File Offset: 0x004AFBB2
		public override void CompTick()
		{
			this.EmissionTick(this.parent.Map.flecks);
		}
	}
}
