using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002D68 RID: 11624
	public class CompMoteEmitter : ThingComp
	{
		// Token: 0x1700298D RID: 10637
		// (get) Token: 0x06010551 RID: 66897 RVA: 0x004B6E3F File Offset: 0x004B503F
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		// Token: 0x1700298E RID: 10638
		// (get) Token: 0x06010552 RID: 66898 RVA: 0x004B6E4C File Offset: 0x004B504C
		public bool MoteLive
		{
			get
			{
				return this.mote != null && !this.mote.Destroyed;
			}
		}

		// Token: 0x06010553 RID: 66899 RVA: 0x004B6E66 File Offset: 0x004B5066
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.Props.ticksSinceLastEmittedMaxOffset > 0)
			{
				this.ticksSinceLastEmitted = Rand.Range(0, this.Props.ticksSinceLastEmittedMaxOffset);
			}
		}

		// Token: 0x06010554 RID: 66900 RVA: 0x004B6E94 File Offset: 0x004B5094
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			Skyfaller skyfaller = this.parent as Skyfaller;
			if (skyfaller != null && skyfaller.FadingOut)
			{
				return;
			}
			if (this.Props.emissionInterval != -1 && !this.Props.maintain)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
			}
			else if (this.mote == null || this.mote.Destroyed)
			{
				this.Emit();
			}
			if (this.mote != null && !this.mote.Destroyed)
			{
				if (typeof(MoteAttached).IsAssignableFrom(this.Props.mote.thingClass) && skyfaller != null)
				{
					this.mote.exactRotation = skyfaller.DrawAngle();
				}
				if (this.Props.maintain)
				{
					this.Maintain();
				}
			}
		}

		// Token: 0x06010555 RID: 66901 RVA: 0x004B6FD5 File Offset: 0x004B51D5
		public void Maintain()
		{
			this.mote.Maintain();
		}

		// Token: 0x06010556 RID: 66902 RVA: 0x004B6FE4 File Offset: 0x004B51E4
		public virtual void Emit()
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Thing tried spawning mote without being spawned!");
				return;
			}
			Vector3 vector = this.Props.offset + this.Props.RotationOffset(this.parent.Rotation);
			if (this.Props.offsetMin != Vector3.zero || this.Props.offsetMax != Vector3.zero)
			{
				vector = this.Props.EmissionOffset;
			}
			ThingDef thingDef = this.Props.RotationMote(this.parent.Rotation) ?? this.Props.mote;
			if (typeof(MoteAttached).IsAssignableFrom(thingDef.thingClass))
			{
				this.mote = MoteMaker.MakeAttachedOverlay(this.parent, thingDef, vector, 1f, -1f);
			}
			else
			{
				Vector3 vector2 = this.parent.DrawPos + vector;
				if (vector2.InBounds(this.parent.Map))
				{
					this.mote = MoteMaker.MakeStaticMote(vector2, this.parent.Map, thingDef, 1f, false, 0f);
				}
			}
			if (this.mote != null && this.Props.useParentRotation)
			{
				this.mote.exactRotation = this.parent.Rotation.AsAngle;
			}
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x06010557 RID: 66903 RVA: 0x004B7174 File Offset: 0x004B5374
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x0400AA63 RID: 43619
		public int ticksSinceLastEmitted;

		// Token: 0x0400AA64 RID: 43620
		protected Mote mote;
	}
}
