using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000204 RID: 516
	public class CurveKeyAnimationPart : AnimationPart
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x000419DB File Offset: 0x0003FBDB
		protected override Type DefaultWorker
		{
			get
			{
				return typeof(AnimationWorker_Curves);
			}
		}

		// Token: 0x04000817 RID: 2071
		public List<CurveKey> keyframes;

		// Token: 0x04000818 RID: 2072
		public bool normalized = true;

		// Token: 0x04000819 RID: 2073
		public Vector3Curve offset = new Vector3Curve();

		// Token: 0x0400081A RID: 2074
		public ComplexCurve angle = new ComplexCurve();

		// Token: 0x0400081B RID: 2075
		public Vector3Curve scale = new Vector3Curve();
	}
}
