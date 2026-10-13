using System;

namespace RimWorld
{
	// Token: 0x02002891 RID: 10385
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x0600E342 RID: 58178 RVA: 0x0042790B File Offset: 0x00425B0B
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x0600E343 RID: 58179
		protected abstract void ApplyRule();
	}
}
