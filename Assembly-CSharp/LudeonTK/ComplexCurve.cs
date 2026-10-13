using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000067 RID: 103
	public class ComplexCurve : IEquatable<ComplexCurve>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000C6F3 File Offset: 0x0000A8F3
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000C700 File Offset: 0x0000A900
		public UnityEngine.Keyframe[] Keys
		{
			get
			{
				return this.curve.keys;
			}
			set
			{
				this.curve.keys = value;
				this.keyframes = value.ToList<UnityEngine.Keyframe>();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000C71A File Offset: 0x0000A91A
		public int Length
		{
			get
			{
				return this.curve.length;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000C727 File Offset: 0x0000A927
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000C734 File Offset: 0x0000A934
		public WrapMode PreWrapMode
		{
			get
			{
				return this.curve.preWrapMode;
			}
			set
			{
				this.curve.preWrapMode = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000C742 File Offset: 0x0000A942
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000C74F File Offset: 0x0000A94F
		public WrapMode PostWrapMode
		{
			get
			{
				return this.curve.postWrapMode;
			}
			set
			{
				this.curve.postWrapMode = value;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C75D File Offset: 0x0000A95D
		public ComplexCurve()
		{
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C773 File Offset: 0x0000A973
		public ComplexCurve(params UnityEngine.Keyframe[] keyframes)
		{
			this.curve = new AnimationCurve(keyframes);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C795 File Offset: 0x0000A995
		public ComplexCurve(AnimationCurve curve)
		{
			this.curve = curve;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C7B2 File Offset: 0x0000A9B2
		public float Evaluate(float time)
		{
			return this.curve.Evaluate(time);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
		public int AddKey(float time, float value)
		{
			return this.curve.AddKey(time, value);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C7CF File Offset: 0x0000A9CF
		public int AddKey(UnityEngine.Keyframe keyframe)
		{
			return this.curve.AddKey(keyframe);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C7DD File Offset: 0x0000A9DD
		public int MoveKey(int index, UnityEngine.Keyframe keyframe)
		{
			return this.curve.MoveKey(index, keyframe);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C7EC File Offset: 0x0000A9EC
		public void RemoveKey(int index)
		{
			this.curve.RemoveKey(index);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C7FA File Offset: 0x0000A9FA
		public void SmoothTangents(int index, float weight)
		{
			this.curve.SmoothTangents(index, weight);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C809 File Offset: 0x0000AA09
		public static ComplexCurve Constant(float timeStart, float timeEnd, float value)
		{
			return new ComplexCurve(AnimationCurve.Constant(timeStart, timeEnd, value));
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C818 File Offset: 0x0000AA18
		public static ComplexCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new ComplexCurve(AnimationCurve.Linear(timeStart, valueStart, timeEnd, valueEnd));
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000C828 File Offset: 0x0000AA28
		public static ComplexCurve LinearNormalized
		{
			get
			{
				return new ComplexCurve(AnimationCurve.Linear(0f, 0f, 1f, 1f));
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000C848 File Offset: 0x0000AA48
		public static ComplexCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new ComplexCurve(AnimationCurve.EaseInOut(timeStart, valueStart, timeEnd, valueEnd));
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000C858 File Offset: 0x0000AA58
		public static ComplexCurve Empty()
		{
			return new ComplexCurve(new AnimationCurve());
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C864 File Offset: 0x0000AA64
		public AnimationCurve GetInternalCurveCopy()
		{
			return new AnimationCurve(this.curve.keys)
			{
				preWrapMode = this.curve.preWrapMode,
				postWrapMode = this.curve.postWrapMode
			};
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C898 File Offset: 0x0000AA98
		public void PostLoad()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.preWrapMode = this.preWrapMode;
			animationCurve.postWrapMode = this.postWrapMode;
			List<UnityEngine.Keyframe> list = this.keyframes;
			animationCurve.keys = ((list != null) ? list.ToArray() : null);
			this.curve = animationCurve;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C8D5 File Offset: 0x0000AAD5
		public override bool Equals(object o)
		{
			return o != null && (this == o || (o.GetType() == base.GetType() && this.Equals((ComplexCurve)o)));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C903 File Offset: 0x0000AB03
		public bool Equals(ComplexCurve other)
		{
			return other != null && this.curve.Equals(other.curve);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C91B File Offset: 0x0000AB1B
		public override int GetHashCode()
		{
			return this.curve.GetHashCode();
		}

		// Token: 0x04000115 RID: 277
		private List<UnityEngine.Keyframe> keyframes;

		// Token: 0x04000116 RID: 278
		private WrapMode preWrapMode = WrapMode.ClampForever;

		// Token: 0x04000117 RID: 279
		private WrapMode postWrapMode = WrapMode.ClampForever;

		// Token: 0x04000118 RID: 280
		[Unsaved(false)]
		private AnimationCurve curve;
	}
}
