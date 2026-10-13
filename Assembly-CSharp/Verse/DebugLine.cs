using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DC RID: 988
	internal struct DebugLine
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x0007FCEC File Offset: 0x0007DEEC
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0007FD03 File Offset: 0x0007DF03
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0007FD2D File Offset: 0x0007DF2D
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color, 0.2f);
		}

		// Token: 0x0400165A RID: 5722
		public Vector3 a;

		// Token: 0x0400165B RID: 5723
		public Vector3 b;

		// Token: 0x0400165C RID: 5724
		private int deathTick;

		// Token: 0x0400165D RID: 5725
		private SimpleColor color;
	}
}
