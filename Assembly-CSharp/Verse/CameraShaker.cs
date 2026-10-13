using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public class CameraShaker
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0003E2F1 File Offset: 0x0003C4F1
		// (set) Token: 0x06000D1B RID: 3355 RVA: 0x0003E2F9 File Offset: 0x0003C4F9
		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, this.GetMaxShakeMag());
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0003E314 File Offset: 0x0003C514
		public Vector3 ShakeOffset
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return Vector3.zero;
				}
				if (Find.TickManager.Paused && !WorldComponent_GravshipController.CutsceneInProgress)
				{
					return Vector3.zero;
				}
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, 0f, z);
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0003E388 File Offset: 0x0003C588
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0003E3A1 File Offset: 0x0003C5A1
		public void DoShake(float mag, int durationTicks)
		{
			this.extendedShakeRequests.Add(new ExtendedShakeRequest(mag, durationTicks));
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003E3B5 File Offset: 0x0003C5B5
		public float GetMaxShakeMag()
		{
			return 0.2f * Prefs.ScreenShakeIntensity;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003E3C2 File Offset: 0x0003C5C2
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0003E3D6 File Offset: 0x0003C5D6
		public void StopAllShaking()
		{
			this.CurShakeMag = 0f;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0003E3E4 File Offset: 0x0003C5E4
		public void Update()
		{
			for (int i = this.extendedShakeRequests.Count - 1; i >= 0; i--)
			{
				ExtendedShakeRequest extendedShakeRequest = this.extendedShakeRequests[i];
				if (Find.TickManager.TicksGame > extendedShakeRequest.StartTick + extendedShakeRequest.Duration)
				{
					this.extendedShakeRequests.RemoveAt(i);
				}
				else
				{
					this.SetMinShake(extendedShakeRequest.Mag);
				}
			}
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003E47C File Offset: 0x0003C67C
		public void Expose()
		{
			Scribe_Collections.Look<ExtendedShakeRequest>(ref this.extendedShakeRequests, "extendedShakeRequests", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<ExtendedShakeRequest>(ref this.unpausedExtendedShakeRequests, "unpausedExtendedShakeRequests", LookMode.Deep, Array.Empty<object>());
			if (this.extendedShakeRequests == null)
			{
				this.extendedShakeRequests = new List<ExtendedShakeRequest>();
			}
			if (this.unpausedExtendedShakeRequests == null)
			{
				this.unpausedExtendedShakeRequests = new List<ExtendedShakeRequest>();
			}
		}

		// Token: 0x0400079A RID: 1946
		private float curShakeMag;

		// Token: 0x0400079B RID: 1947
		private List<ExtendedShakeRequest> extendedShakeRequests = new List<ExtendedShakeRequest>();

		// Token: 0x0400079C RID: 1948
		private List<ExtendedShakeRequest> unpausedExtendedShakeRequests = new List<ExtendedShakeRequest>();

		// Token: 0x0400079D RID: 1949
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x0400079E RID: 1950
		private const float ShakeFrequency = 24f;

		// Token: 0x0400079F RID: 1951
		private const float MaxShakeMag = 0.2f;
	}
}
