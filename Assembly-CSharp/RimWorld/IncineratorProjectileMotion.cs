using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AA2 RID: 10914
	public struct IncineratorProjectileMotion
	{
		// Token: 0x17002604 RID: 9732
		// (get) Token: 0x0600F324 RID: 62244 RVA: 0x00469728 File Offset: 0x00467928
		public float Alpha
		{
			get
			{
				return Mathf.Clamp01((float)this.ticks / (float)this.lifespanTicks);
			}
		}

		// Token: 0x17002605 RID: 9733
		// (get) Token: 0x0600F325 RID: 62245 RVA: 0x0046973E File Offset: 0x0046793E
		public Vector3 LerpdPos
		{
			get
			{
				return Vector3.Lerp(this.worldSource, this.worldTarget, this.Alpha);
			}
		}

		// Token: 0x0600F326 RID: 62246 RVA: 0x00469758 File Offset: 0x00467958
		public void Tick(Map map)
		{
			this.ticks++;
			Vector3 lerpdPos = this.LerpdPos;
			IntVec3 cell = lerpdPos.ToIntVec3();
			Vector3 offsetA = lerpdPos - cell.ToVector3Shifted();
			Vector3 vector = this.LerpdPos - this.moveVector * 2f;
			IntVec3 cell2 = vector.ToIntVec3();
			Vector3 offsetB = vector - cell2.ToVector3Shifted();
			this.mote.Scale = Mathf.Lerp(this.startScale, this.endScale, this.Alpha);
			this.mote.UpdateTargets(new TargetInfo(cell, map, false), new TargetInfo(cell2, map, false), offsetA, offsetB);
			if (this.Alpha < 1f)
			{
				this.mote.Maintain();
			}
		}

		// Token: 0x0400A022 RID: 40994
		public MoteDualAttached mote;

		// Token: 0x0400A023 RID: 40995
		public Vector3 worldSource;

		// Token: 0x0400A024 RID: 40996
		public Vector3 worldTarget;

		// Token: 0x0400A025 RID: 40997
		public IntVec3 targetDest;

		// Token: 0x0400A026 RID: 40998
		public Vector3 moveVector;

		// Token: 0x0400A027 RID: 40999
		public float startScale;

		// Token: 0x0400A028 RID: 41000
		public float endScale;

		// Token: 0x0400A029 RID: 41001
		public int ticks;

		// Token: 0x0400A02A RID: 41002
		public int lifespanTicks;
	}
}
