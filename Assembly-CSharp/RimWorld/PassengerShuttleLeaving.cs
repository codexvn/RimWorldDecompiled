using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B4F RID: 11087
	public class PassengerShuttleLeaving : FlyShipLeaving
	{
		// Token: 0x170026E4 RID: 9956
		// (get) Token: 0x0600F7D8 RID: 63448 RVA: 0x004811A8 File Offset: 0x0047F3A8
		public override Color DrawColor
		{
			get
			{
				return base.Contents.GetShuttle().DrawColor;
			}
		}

		// Token: 0x0600F7D9 RID: 63449 RVA: 0x004811BA File Offset: 0x0047F3BA
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.angle = PassengerShuttleLeaving.GetAngle(0f, base.Rotation);
		}

		// Token: 0x0600F7DA RID: 63450 RVA: 0x004811E8 File Offset: 0x0047F3E8
		protected override void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			this.angle = PassengerShuttleLeaving.GetAngle(base.TimeInAnimation, base.Rotation);
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

		// Token: 0x0600F7DB RID: 63451 RVA: 0x004812A4 File Offset: 0x0047F4A4
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

		// Token: 0x0600F7DC RID: 63452 RVA: 0x00481320 File Offset: 0x0047F520
		private static float GetAngle(float timeInAnimation, Rot4 rotation)
		{
			int asInt = rotation.AsInt;
			if (asInt == 1)
			{
				return rotation.AsAngle - PassengerShuttleLeaving.AngleCurve.Evaluate(timeInAnimation);
			}
			if (asInt != 3)
			{
				return rotation.AsAngle;
			}
			return rotation.AsAngle + PassengerShuttleLeaving.AngleCurve.Evaluate(timeInAnimation);
		}

		// Token: 0x0400A30C RID: 41740
		private static readonly SimpleCurve AngleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};
	}
}
