using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B50 RID: 11088
	public class PassengerShuttleIncoming : ShuttleIncoming
	{
		// Token: 0x170026E5 RID: 9957
		// (get) Token: 0x0600F7DF RID: 63455 RVA: 0x004813AE File Offset: 0x0047F5AE
		public Building_PassengerShuttle Shuttle
		{
			get
			{
				return (Building_PassengerShuttle)this.innerContainer.FirstOrDefault<Thing>();
			}
		}

		// Token: 0x170026E6 RID: 9958
		// (get) Token: 0x0600F7E0 RID: 63456 RVA: 0x004813C0 File Offset: 0x0047F5C0
		public override Color DrawColor
		{
			get
			{
				return this.Shuttle.DrawColor;
			}
		}

		// Token: 0x0600F7E1 RID: 63457 RVA: 0x004813CD File Offset: 0x0047F5CD
		protected override void Impact()
		{
			CompLaunchable compLaunchable = this.Shuttle.TryGetComp<CompLaunchable>();
			if (compLaunchable != null)
			{
				compLaunchable.Notify_Arrived();
			}
			base.Impact();
		}

		// Token: 0x0600F7E2 RID: 63458 RVA: 0x004813EB File Offset: 0x0047F5EB
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.angle = PassengerShuttleIncoming.GetAngle(0f, base.Rotation);
		}

		// Token: 0x0600F7E3 RID: 63459 RVA: 0x00481417 File Offset: 0x0047F617
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.hasImpacted)
			{
				Log.Error("Destroying passenger shuttle skyfaller without ever having impacted");
			}
			base.Destroy(mode);
		}

		// Token: 0x0600F7E4 RID: 63460 RVA: 0x00481434 File Offset: 0x0047F634
		protected override void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			this.angle = PassengerShuttleIncoming.GetAngle(base.TimeInAnimation, base.Rotation);
			switch (base.Rotation.AsInt)
			{
			case 1:
				extraRotation += this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			case 3:
				extraRotation -= this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			}
			drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(base.TimeInAnimation);
		}

		// Token: 0x0600F7E5 RID: 63461 RVA: 0x004814F0 File Offset: 0x0047F6F0
		public override float DrawAngle()
		{
			float num = 0f;
			switch (base.Rotation.AsInt)
			{
			case 1:
				num += this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			case 3:
				num -= this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			}
			return num;
		}

		// Token: 0x0600F7E6 RID: 63462 RVA: 0x0048156C File Offset: 0x0047F76C
		private static float GetAngle(float timeInAnimation, Rot4 rotation)
		{
			int asInt = rotation.AsInt;
			if (asInt == 1)
			{
				return rotation.Opposite.AsAngle + PassengerShuttleIncoming.AngleCurve.Evaluate(timeInAnimation);
			}
			if (asInt != 3)
			{
				return rotation.Opposite.AsAngle;
			}
			return rotation.Opposite.AsAngle - PassengerShuttleIncoming.AngleCurve.Evaluate(timeInAnimation);
		}

		// Token: 0x0400A30D RID: 41741
		private static readonly SimpleCurve AngleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};
	}
}
