using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000791 RID: 1937
	public class PawnCapacitiesHandler
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000F4BC0 File Offset: 0x000F2DC0
		public bool CanBeAwake
		{
			get
			{
				return (this.pawn.RaceProps.alwaysAwake || this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f) && !this.pawn.IsDeactivated();
			}
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000F4BF6 File Offset: 0x000F2DF6
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000F4C05 File Offset: 0x000F2E05
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000F4C10 File Offset: 0x000F2E10
		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
				return 0f;
			}
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
			{
				cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
				try
				{
					cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null, false);
				}
				finally
				{
					cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
				}
			}
			return cacheElement.value;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000F4CBC File Offset: 0x000F2EBC
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000F4CD0 File Offset: 0x000F2ED0
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x0400250C RID: 9484
		private Pawn pawn;

		// Token: 0x0400250D RID: 9485
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02000792 RID: 1938
		private enum CacheStatus
		{
			// Token: 0x0400250F RID: 9487
			Uncached,
			// Token: 0x04002510 RID: 9488
			Caching,
			// Token: 0x04002511 RID: 9489
			Cached
		}

		// Token: 0x02000793 RID: 1939
		private class CacheElement
		{
			// Token: 0x04002512 RID: 9490
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04002513 RID: 9491
			public float value;
		}
	}
}
