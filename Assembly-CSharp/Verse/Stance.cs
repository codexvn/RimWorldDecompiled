using System;

namespace Verse
{
	// Token: 0x020007EE RID: 2030
	public abstract class Stance : IExposable
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x00110577 File Offset: 0x0010E777
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void StanceTick()
		{
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void StanceDraw()
		{
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400272C RID: 10028
		public Pawn_StanceTracker stanceTracker;
	}
}
