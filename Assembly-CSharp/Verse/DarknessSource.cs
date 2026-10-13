using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020005D1 RID: 1489
	public class DarknessSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x0600263E RID: 9790 RVA: 0x000C9ECC File Offset: 0x000C80CC
		public DarknessSource(Map map) : base(map)
		{
			map.events.GlowChanged += this.NotifyGlowChanged;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000C9EF7 File Offset: 0x000C80F7
		private void NotifyGlowChanged(IntVec3 cell)
		{
			if (this.map.GameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>() != null)
			{
				this.dirtyCells.Add(cell);
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000C9F18 File Offset: 0x000C8118
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (!GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(this.map))
			{
				return;
			}
			this.wasActive = true;
			for (int i = 0; i < this.cellCount; i++)
			{
				this.data.Set(i, GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(this.map.cellIndices[i], this.map));
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000C9F88 File Offset: 0x000C8188
		public override bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			bool flag = GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(this.map);
			bool result = false;
			if (flag && this.dirtyCells.Any<IntVec3>())
			{
				foreach (IntVec3 cell in this.dirtyCells)
				{
					this.data.Set(this.map.cellIndices[cell], GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(cell, this.map));
				}
				result = true;
				this.dirtyCells.Clear();
			}
			if (!flag && !this.wasActive)
			{
				return false;
			}
			if (flag && this.wasActive)
			{
				return result;
			}
			if (!flag)
			{
				this.data.Clear();
			}
			else
			{
				for (int i = 0; i < this.cellCount; i++)
				{
					this.data.Set(i, GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(this.map.cellIndices[i], this.map));
				}
			}
			this.wasActive = flag;
			return true;
		}

		// Token: 0x04001FB5 RID: 8117
		private bool wasActive;

		// Token: 0x04001FB6 RID: 8118
		private readonly HashSet<IntVec3> dirtyCells = new HashSet<IntVec3>();
	}
}
