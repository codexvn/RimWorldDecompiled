using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004CD RID: 1229
	public sealed class PlanManager : IExposable
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000A6BFF File Offset: 0x000A4DFF
		public List<Plan> AllPlans
		{
			get
			{
				return this.allPlans;
			}
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000A6C07 File Offset: 0x000A4E07
		public PlanManager(Map map)
		{
			this.map = map;
			this.planGrid = new Plan[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000A6C37 File Offset: 0x000A4E37
		public void ExposeData()
		{
			Scribe_Collections.Look<Plan>(ref this.allPlans, "allPlans", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdatePlanManagerLinks();
				this.RebuildPlanGrid();
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000A6C64 File Offset: 0x000A4E64
		private void UpdatePlanManagerLinks()
		{
			for (int i = 0; i < this.allPlans.Count; i++)
			{
				this.allPlans[i].planManager = this;
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000A6C9C File Offset: 0x000A4E9C
		private void RebuildPlanGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.planGrid = new Plan[cellIndices.NumGridCells];
			foreach (Plan plan in this.allPlans)
			{
				foreach (IntVec3 c in plan)
				{
					this.planGrid[cellIndices.CellToIndex(c)] = plan;
				}
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000A6D48 File Offset: 0x000A4F48
		public void RegisterPlan(Plan newPlan)
		{
			this.allPlans.Add(newPlan);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000A6D56 File Offset: 0x000A4F56
		public void DeregisterPlan(Plan oldPlan)
		{
			this.allPlans.Remove(oldPlan);
			if (Find.Selector.SelectedPlan == oldPlan)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000A6D7C File Offset: 0x000A4F7C
		internal void AddPlanGridCell(Plan plan, IntVec3 c)
		{
			this.planGrid[this.map.cellIndices.CellToIndex(c)] = plan;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000A6D97 File Offset: 0x000A4F97
		internal void ClearPlanGridCell(IntVec3 c)
		{
			this.planGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000A6DB2 File Offset: 0x000A4FB2
		public Plan PlanAt(IntVec3 c)
		{
			return this.planGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000A6DCC File Offset: 0x000A4FCC
		public bool TryGetPlan(IntVec3 cell, out Plan plan)
		{
			plan = this.planGrid[this.map.cellIndices.CellToIndex(cell)];
			return plan != null;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000A6DF0 File Offset: 0x000A4FF0
		public string NewPlanName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!this.allPlans.Any((Plan z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of plan names.");
			return "Plan X";
		}

		// Token: 0x04001B4A RID: 6986
		public readonly Map map;

		// Token: 0x04001B4B RID: 6987
		private List<Plan> allPlans = new List<Plan>();

		// Token: 0x04001B4C RID: 6988
		private Plan[] planGrid;
	}
}
