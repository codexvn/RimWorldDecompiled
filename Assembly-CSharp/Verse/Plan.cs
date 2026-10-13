using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004C7 RID: 1223
	public class Plan : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
	{
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x000A5F31 File Offset: 0x000A4131
		// (set) Token: 0x06001F9F RID: 8095 RVA: 0x000A5F43 File Offset: 0x000A4143
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.baseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x000A5F4C File Offset: 0x000A414C
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000A5F54 File Offset: 0x000A4154
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x000A5F5C File Offset: 0x000A415C
		public Map Map
		{
			get
			{
				return this.planManager.map;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x000A5F69 File Offset: 0x000A4169
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x000A5F74 File Offset: 0x000A4174
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
				this.hidden = value;
				foreach (IntVec3 loc in this.Cells)
				{
					this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Plan);
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000A5FE4 File Offset: 0x000A41E4
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x000A5FEC File Offset: 0x000A41EC
		public ColorDef Color
		{
			get
			{
				return this.color;
			}
			private set
			{
				if (this.color == value)
				{
					return;
				}
				this.color = value;
				this.materialInt = this.CreateMaterial();
				this.Dirty();
			}
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000A6014 File Offset: 0x000A4214
		public void MergeIn(Plan plan)
		{
			for (int i = plan.Cells.Count - 1; i >= 0; i--)
			{
				IntVec3 c = plan.Cells[i];
				plan.RemoveCell(c);
				this.AddCell(c);
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000A6054 File Offset: 0x000A4254
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = this.CreateMaterial();
				}
				return this.materialInt;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000A6070 File Offset: 0x000A4270
		protected virtual Material CreateMaterial()
		{
			return MaterialPool.MatFrom(new MaterialRequest
			{
				BaseTexPath = "Designations/Plan",
				shader = ShaderDatabase.MetaOverlay,
				color = this.color.color
			});
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000A60B5 File Offset: 0x000A42B5
		public IEnumerator<IntVec3> GetEnumerator()
		{
			return this.cells.GetEnumerator();
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000A60C7 File Offset: 0x000A42C7
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x000A60E9 File Offset: 0x000A42E9
		public int CellCount
		{
			get
			{
				return this.cells.Count;
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000A60F6 File Offset: 0x000A42F6
		public Plan()
		{
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000A6110 File Offset: 0x000A4310
		public Plan(ColorDef color, PlanManager planManager)
		{
			this.baseLabel = "NewPlan".Translate();
			this.label = planManager.NewPlanName("NewPlan".Translate());
			this.planManager = planManager;
			this.ID = Find.UniqueIDsManager.GetNextPlanID();
			this.Map.planManager.RegisterPlan(this);
			this.color = color;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000A6194 File Offset: 0x000A4394
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Defs.Look<ColorDef>(ref this.color, "color");
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000A6218 File Offset: 0x000A4418
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error(string.Format("Adding cell to zone which already has it. c={0}, zone={1}", c, this));
				return;
			}
			this.cells.Add(c);
			this.planManager.AddPlanGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
			this.cellsShuffled = false;
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000A6288 File Offset: 0x000A4488
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error(string.Format("Cannot remove cell from zone which doesn't have it. c={0}, zone={1}", c, this));
				return;
			}
			this.cells.Remove(c);
			this.planManager.ClearPlanGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000A6308 File Offset: 0x000A4508
		public void Dirty()
		{
			foreach (IntVec3 loc in this.cells)
			{
				this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Plan);
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000A6370 File Offset: 0x000A4570
		public void Delete()
		{
			this.Delete(true);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000A637C File Offset: 0x000A457C
		public virtual void Delete(bool playSound)
		{
			if (playSound)
			{
				SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			}
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells.First<IntVec3>());
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000A63DB File Offset: 0x000A45DB
		public void Deregister()
		{
			this.planManager.DeregisterPlan(this);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000A63E9 File Offset: 0x000A45E9
		public bool ContainsCell(IntVec3 c)
		{
			return this.cells.Contains(c);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000A63F8 File Offset: 0x000A45F8
		public virtual string GetInspectString()
		{
			int num = 0;
			for (int i = 0; i < this.planManager.AllPlans.Count; i++)
			{
				Plan plan = this.planManager.AllPlans[i];
				num += plan.CellCount;
			}
			string text = string.Format("{0}: {1}", "Size".Translate().CapitalizeFirst(), this.CellCount);
			if (this.planManager.AllPlans.Count > 1)
			{
				text += string.Format("\n{0}: {1}", "TotalPlanningArea".Translate().CapitalizeFirst(), num);
			}
			return text;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000A64AE File Offset: 0x000A46AE
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Designator_Plan_Expand expand = DesignatorUtility.FindAllowedDesignator<Designator_Plan_Expand>();
			expand.Initialize(this.color);
			yield return new Command_Hide_Plans(this)
			{
				icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanHide", true),
				defaultDesc = "CommandHidePlanDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2
			};
			yield return expand;
			yield return DesignatorUtility.FindAllowedDesignator<Designator_Plan_Shrink>();
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanChangeColor", true),
				defaultLabel = "CommandChangeColorPlanLabel".Translate(),
				defaultDesc = "CommandChangeColorPlanDesc".Translate(),
				action = delegate()
				{
					List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
					foreach (ColorDef newCol2 in Designator_Plan_Add.Colors)
					{
						ColorDef newCol = newCol2;
						Color value = newCol.color;
						value.a = 0.8f;
						list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate()
						{
							this.Color = newCol;
							this.CheckContiguous();
						}, new Color?(value), new TipSignal?(newCol.LabelCap)));
					}
					Find.WindowStack.Add(new FloatMenuGrid(list));
				}
			};
			yield return new Command_Action
			{
				icon = TexButton.Delete,
				defaultLabel = "CommandDeletePlanLabel".Translate(),
				defaultDesc = "CommandDeletePlanDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Designator_Deconstruct
			};
			yield return DesignatorUtility.FindAllowedDesignator<Designator_Plan_Copy>();
			yield return DesignatorUtility.FindAllowedDesignator<Designator_Plan_CopySelection>();
			yield break;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000A64C0 File Offset: 0x000A46C0
		public void CheckContiguous()
		{
			Plan.<>c__DisplayClass49_0 CS$<>8__locals1 = new Plan.<>c__DisplayClass49_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Plan.extantGrid == null)
			{
				Plan.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Plan.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Plan.foundGrid == null)
			{
				Plan.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Plan.foundGrid.ClearAndResizeTo(this.Map);
			}
			foreach (IntVec3 c in this.Map.AllCells)
			{
				Plan plan = c.GetPlan(this.Map);
				if (plan != null && plan == this)
				{
					Plan.extantGrid.Set(c, true);
				}
			}
			CS$<>8__locals1.numFound = 0;
			this.Map.floodFiller.FloodFill(this.cells.First<IntVec3>(), new Predicate<IntVec3>(Plan.<CheckContiguous>g__PassCheck|49_0), new Action<IntVec3>(CS$<>8__locals1.<CheckContiguous>g__Processor|1), int.MaxValue, false, null);
			if (CS$<>8__locals1.numFound < this.cells.Count)
			{
				foreach (IntVec3 intVec in this.Map.AllCells)
				{
					if (Plan.extantGrid[intVec] && !Plan.foundGrid[intVec])
					{
						FloodFiller floodFiller = this.Map.floodFiller;
						IntVec3 root = intVec;
						Predicate<IntVec3> passCheck = (IntVec3 cell) => Plan.extantGrid[cell] && !Plan.foundGrid[cell];
						Action<IntVec3> processor;
						if ((processor = CS$<>8__locals1.<>9__3) == null)
						{
							processor = (CS$<>8__locals1.<>9__3 = delegate(IntVec3 cell)
							{
								if (CS$<>8__locals1.<>4__this.Map.planManager.PlanAt(cell) == CS$<>8__locals1.<>4__this)
								{
									Plan.toSplit.Add(cell);
								}
								Plan.extantGrid[cell] = false;
							});
						}
						floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
						Plan plan2 = null;
						foreach (IntVec3 c2 in Plan.toSplit)
						{
							if (plan2 == null)
							{
								plan2 = new Plan(this.color, this.planManager);
							}
							plan2.RenamableLabel = this.RenamableLabel;
							this.RemoveCell(c2);
							plan2.AddCell(c2);
						}
						Plan.toSplit.Clear();
					}
				}
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000A6750 File Offset: 0x000A4950
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000A6759 File Offset: 0x000A4959
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000A6761 File Offset: 0x000A4961
		public string GetUniqueLoadID()
		{
			return string.Format("Plan_{0}", this.ID);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000A6844 File Offset: 0x000A4A44
		[CompilerGenerated]
		internal static bool <CheckContiguous>g__PassCheck|49_0(IntVec3 c)
		{
			return Plan.extantGrid[c] && !Plan.foundGrid[c];
		}

		// Token: 0x04001B2F RID: 6959
		public PlanManager planManager;

		// Token: 0x04001B30 RID: 6960
		private ColorDef color;

		// Token: 0x04001B31 RID: 6961
		private int ID = -1;

		// Token: 0x04001B32 RID: 6962
		public string label;

		// Token: 0x04001B33 RID: 6963
		private string baseLabel;

		// Token: 0x04001B34 RID: 6964
		private List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04001B35 RID: 6965
		private bool hidden;

		// Token: 0x04001B36 RID: 6966
		private Material materialInt;

		// Token: 0x04001B37 RID: 6967
		private bool cellsShuffled;

		// Token: 0x04001B38 RID: 6968
		private static BoolGrid extantGrid;

		// Token: 0x04001B39 RID: 6969
		private static BoolGrid foundGrid;

		// Token: 0x04001B3A RID: 6970
		private static readonly HashSet<IntVec3> toSplit = new HashSet<IntVec3>();
	}
}
