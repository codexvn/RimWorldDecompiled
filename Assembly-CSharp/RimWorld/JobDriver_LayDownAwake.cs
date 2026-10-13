using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012AA RID: 4778
	public class JobDriver_LayDownAwake : JobDriver_LayDown
	{
		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x0600751A RID: 29978 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSleep
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x0600751B RID: 29979 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x0600751C RID: 29980 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool LookForOtherJobs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x0600751D RID: 29981 RVA: 0x0023CDB4 File Offset: 0x0023AFB4
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null)
				{
					return thing.Rotation.Rotated(RotationDirection.Clockwise);
				}
				return base.ForcedLayingRotation;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x0600751E RID: 29982 RVA: 0x0023CDF0 File Offset: 0x0023AFF0
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null && thing.def.Size.z > 1)
				{
					return new Vector3(0f, 0f, 0.5f).RotatedBy(thing.Rotation);
				}
				return base.ForcedBodyOffset;
			}
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x0023CE4E File Offset: 0x0023B04E
		public override string GetReport()
		{
			return "ReportLayingDown".Translate();
		}
	}
}
