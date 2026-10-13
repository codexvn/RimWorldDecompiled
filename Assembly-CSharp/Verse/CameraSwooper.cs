using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public class CameraSwooper
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x0003E4F9 File Offset: 0x0003C6F9
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0003E531 File Offset: 0x0003C731
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0003E548 File Offset: 0x0003C748
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0003E598 File Offset: 0x0003C798
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x040007A0 RID: 1952
		public bool Swooping;

		// Token: 0x040007A1 RID: 1953
		private bool SwoopingTo;

		// Token: 0x040007A2 RID: 1954
		private float TimeSinceSwoopStart;

		// Token: 0x040007A3 RID: 1955
		private Vector3 FinalOffset;

		// Token: 0x040007A4 RID: 1956
		private float FinalOrthoSizeOffset;

		// Token: 0x040007A5 RID: 1957
		private float TotalSwoopTime;

		// Token: 0x040007A6 RID: 1958
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
