using System;

namespace Verse
{
	// Token: 0x0200045F RID: 1119
	public abstract class MapComponent : IExposable
	{
		// Token: 0x06001D57 RID: 7511 RVA: 0x0009462F File Offset: 0x0009282F
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapComponentDraw()
		{
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapGenerated()
		{
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04001988 RID: 6536
		public readonly Map map;
	}
}
