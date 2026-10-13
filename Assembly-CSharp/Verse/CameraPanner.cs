using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public struct CameraPanner
	{
		// Token: 0x06000D0C RID: 3340 RVA: 0x0003E11A File Offset: 0x0003C31A
		public void PanTo(CameraPanner.Interpolant source, CameraPanner.Interpolant destination, float duration = 0.25f, PanCompletionCallback completion = null)
		{
			this.Moving = true;
			this.TimeSinceStart = 0f;
			this.Source = source;
			this.Destination = destination;
			this.Duration = duration;
			this.Completion = completion;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0003E14B File Offset: 0x0003C34B
		public void JumpOnNextUpdate()
		{
			this.TimeSinceStart = this.Duration;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0003E15C File Offset: 0x0003C35C
		public CameraPanner.Interpolant? Update()
		{
			if (!this.Moving)
			{
				return null;
			}
			this.TimeSinceStart += Time.deltaTime;
			if (this.TimeSinceStart >= this.Duration)
			{
				this.Moving = false;
				PanCompletionCallback completion = this.Completion;
				if (completion != null)
				{
					completion();
				}
				this.Completion = null;
			}
			return new CameraPanner.Interpolant?(this.GetCurrentInterpolation());
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0003E1C8 File Offset: 0x0003C3C8
		private CameraPanner.Interpolant GetCurrentInterpolation()
		{
			float t = CameraPanner.SmootherStep(this.TimeSinceStart / this.Duration);
			return new CameraPanner.Interpolant(Vector3.LerpUnclamped(this.Source.Position, this.Destination.Position, t), Mathf.LerpUnclamped(this.Source.Size, this.Destination.Size, t));
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0003E228 File Offset: 0x0003C428
		public static float SmootherStep(float t)
		{
			float num = Mathf.Clamp01(t);
			return num * num * num * (num * (num * 6f - 15f) + 10f);
		}

		// Token: 0x0400078E RID: 1934
		public bool Moving;

		// Token: 0x0400078F RID: 1935
		private float TimeSinceStart;

		// Token: 0x04000790 RID: 1936
		private CameraPanner.Interpolant Source;

		// Token: 0x04000791 RID: 1937
		private CameraPanner.Interpolant Destination;

		// Token: 0x04000792 RID: 1938
		private float Duration;

		// Token: 0x04000793 RID: 1939
		private PanCompletionCallback Completion;

		// Token: 0x04000794 RID: 1940
		public const float DefaultDuration = 0.25f;

		// Token: 0x020001E5 RID: 485
		public readonly struct Interpolant
		{
			// Token: 0x06000D11 RID: 3345 RVA: 0x0003E257 File Offset: 0x0003C457
			public Interpolant(Vector3 position, float size)
			{
				this.Position = position;
				this.Size = size;
			}

			// Token: 0x17000205 RID: 517
			// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0003E267 File Offset: 0x0003C467
			public Vector3 Position { get; }

			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0003E26F File Offset: 0x0003C46F
			public float Size { get; }
		}
	}
}
