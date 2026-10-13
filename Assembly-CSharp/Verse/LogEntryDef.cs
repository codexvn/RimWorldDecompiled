using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029E RID: 670
	public class LogEntryDef : Def
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x0004EE76 File Offset: 0x0004D076
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		// Token: 0x04000DDD RID: 3549
		[NoTranslate]
		public string iconMiss;

		// Token: 0x04000DDE RID: 3550
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x04000DDF RID: 3551
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x04000DE0 RID: 3552
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x04000DE1 RID: 3553
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x04000DE2 RID: 3554
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}
