using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030ED RID: 12525
	public class Designator_Plan_Add : Designator_Plan
	{
		// Token: 0x17002DA5 RID: 11685
		// (get) Token: 0x0601197F RID: 72063 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DA6 RID: 11686
		// (get) Token: 0x06011980 RID: 72064 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanRightClickToggleVisibility
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DA7 RID: 11687
		// (get) Token: 0x06011981 RID: 72065 RVA: 0x0050CFF8 File Offset: 0x0050B1F8
		public static IEnumerable<ColorDef> Colors
		{
			get
			{
				return from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Planning
				orderby x.displayOrder
				select x;
			}
		}

		// Token: 0x17002DA8 RID: 11688
		// (get) Token: 0x06011982 RID: 72066 RVA: 0x0050D054 File Offset: 0x0050B254
		public override Color IconDrawColor
		{
			get
			{
				Color color = this.colorDef.color;
				color.a = 0.8f;
				return color;
			}
		}

		// Token: 0x17002DA9 RID: 11689
		// (get) Token: 0x06011983 RID: 72067 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanSelectColor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DAA RID: 11690
		// (get) Token: 0x06011984 RID: 72068 RVA: 0x0050D07A File Offset: 0x0050B27A
		// (set) Token: 0x06011985 RID: 72069 RVA: 0x0050D0A0 File Offset: 0x0050B2A0
		protected Plan SelectedPlan
		{
			get
			{
				Plan selectedPlan = Find.Selector.SelectedPlan;
				if (selectedPlan != null)
				{
					this.colorDef = Find.Selector.SelectedPlan.Color;
				}
				return selectedPlan;
			}
			set
			{
				if (Find.Selector.SelectedPlan == value)
				{
					return;
				}
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
					if (Find.Selector.SelectedPlan != null)
					{
						this.colorDef = Find.Selector.SelectedPlan.Color;
					}
				}
			}
		}

		// Token: 0x06011986 RID: 72070 RVA: 0x0050D0F8 File Offset: 0x0050B2F8
		public Designator_Plan_Add()
		{
			this.colorDef = Designator_Plan_Add.Colors.FirstOrDefault<ColorDef>();
			this.defaultLabel = "DesignatorPlan".Translate();
			this.defaultDesc = "DesignatorPlanDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanAdd;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.hotKey = KeyBindingDefOf.Misc9;
			this.useMouseIcon = true;
		}

		// Token: 0x06011987 RID: 72071 RVA: 0x0050D19F File Offset: 0x0050B39F
		protected virtual Plan CreateNewPlan()
		{
			return new Plan(this.colorDef, Find.CurrentMap.planManager);
		}

		// Token: 0x06011988 RID: 72072 RVA: 0x0050D1B6 File Offset: 0x0050B3B6
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			return true;
		}

		// Token: 0x06011989 RID: 72073 RVA: 0x0050D1F4 File Offset: 0x0050B3F4
		public override void DrawMouseAttachments()
		{
			if (!this.useMouseIcon)
			{
				return;
			}
			string text = this.mouseText;
			if (!Input.GetKey(KeyCode.Mouse0) && text.NullOrEmpty())
			{
				Plan selectedPlan = Find.Selector.SelectedPlan;
				text = ((selectedPlan != null) ? "ExpandOrCreatePlan".Translate(selectedPlan.label) : "CreateNewPlan".Translate());
			}
			GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, null, false, default(Color), null, null);
		}

		// Token: 0x0601198A RID: 72074 RVA: 0x0050D2A0 File Offset: 0x0050B4A0
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			if (Find.Selector.SelectedPlan != null && Find.Selector.SelectedPlan.Color != this.colorDef)
			{
				Find.Selector.Deselect(Find.Selector.SelectedPlan);
			}
			if (!base.Map.IsPocketMap && !base.Map.Tile.LayerDef.ignoreNoBuildArea)
			{
				GenDraw.DrawNoBuildEdgeLines();
			}
		}

		// Token: 0x0601198B RID: 72075 RVA: 0x0050D31C File Offset: 0x0050B51C
		public override void ProcessInput(Event ev)
		{
			if (!this.CanSelectColor)
			{
				base.ProcessInput(ev);
				return;
			}
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (ev.button == 0)
			{
				List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
				foreach (ColorDef newCol2 in Designator_Plan_Add.Colors)
				{
					ColorDef newCol = newCol2;
					Color color = newCol.color;
					color.a = 0.8f;
					list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.colorDef = newCol;
					}, new Color?(color), new TipSignal?(newCol.LabelCap)));
				}
				Find.WindowStack.Add(new FloatMenuGrid(list));
				Find.DesignatorManager.Select(this);
				return;
			}
			if (ev.button == 1 && this.CanRightClickToggleVisibility)
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>(Command_Hide_Plans.GetHideOptions());
				Find.WindowStack.Add(new FloatMenu(options));
			}
		}

		// Token: 0x17002DAB RID: 11691
		// (get) Token: 0x0601198C RID: 72076 RVA: 0x0050D464 File Offset: 0x0050B664
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				if (!this.CanRightClickToggleVisibility)
				{
					yield break;
				}
				foreach (FloatMenuOption floatMenuOption in Command_Hide_Plans.GetHideOptions())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0601198D RID: 72077 RVA: 0x0050D474 File Offset: 0x0050B674
		public override void DesignateSingleCell(IntVec3 c)
		{
			Plan plan = base.Map.planManager.PlanAt(c);
			if (plan != null && plan.Color == this.colorDef)
			{
				this.SelectedPlan = plan;
				base.Finalize(false);
				return;
			}
			this.PlanCells(new List<IntVec3>
			{
				c
			});
		}

		// Token: 0x0601198E RID: 72078 RVA: 0x0050D4C8 File Offset: 0x0050B6C8
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (cells.Count<IntVec3>() == 1)
			{
				Plan plan = base.Map.planManager.PlanAt(cells.First<IntVec3>());
				if (plan != null && plan.Color == this.colorDef)
				{
					this.SelectedPlan = plan;
					base.Finalize(false);
					return;
				}
			}
			this.PlanCells(cells);
		}

		// Token: 0x0601198F RID: 72079 RVA: 0x0050D51C File Offset: 0x0050B71C
		protected void PlanCells(IEnumerable<IntVec3> cells)
		{
			this.modified.Clear();
			this.unsetCells.Clear();
			this.unsetCells.AddRange(cells);
			if (this.SelectedPlan == null)
			{
				Plan plan = null;
				foreach (IntVec3 c in cells)
				{
					Plan plan2 = base.Map.planManager.PlanAt(c);
					if (plan2 != null && plan2.Color == this.colorDef)
					{
						if (plan == null)
						{
							plan = plan2;
						}
						else if (plan2 != plan)
						{
							plan = null;
							break;
						}
					}
				}
				this.SelectedPlan = plan;
			}
			if (this.unsetCells.Count == 0)
			{
				return;
			}
			if (this.SelectedPlan == null)
			{
				Plan plan3 = base.Map.planManager.PlanAt(this.unsetCells[0]);
				if (plan3 != null)
				{
					plan3.RemoveCell(this.unsetCells[0]);
					this.modified.Add(plan3);
				}
				this.SelectedPlan = this.CreateNewPlan();
				this.SelectedPlan.AddCell(this.unsetCells[0]);
				this.unsetCells.RemoveAt(0);
			}
			this.modified.Add(this.SelectedPlan);
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = this.unsetCells.Count;
				for (int i = this.unsetCells.Count - 1; i >= 0; i--)
				{
					IntVec3 intVec = this.unsetCells[i];
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = intVec + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map))
						{
							Plan plan4 = base.Map.planManager.PlanAt(c2);
							if (plan4 != null && plan4.Color == this.colorDef && plan4 != this.SelectedPlan)
							{
								this.SelectedPlan.MergeIn(plan4);
							}
						}
					}
					Plan plan5 = base.Map.planManager.PlanAt(intVec);
					if (plan5 != null && plan5 != this.SelectedPlan)
					{
						plan5.RemoveCell(intVec);
						this.modified.Add(plan5);
					}
					if (!this.SelectedPlan.ContainsCell(intVec))
					{
						this.SelectedPlan.AddCell(intVec);
					}
					this.unsetCells.RemoveAt(i);
				}
				if (this.unsetCells.Count == 0)
				{
					break;
				}
				if (this.unsetCells.Count == count)
				{
					this.SelectedPlan = this.CreateNewPlan();
					this.SelectedPlan.AddCell(this.unsetCells[0]);
					this.unsetCells.RemoveAt(0);
				}
			}
			foreach (Plan plan6 in this.modified)
			{
				plan6.CheckContiguous();
			}
			base.Finalize(somethingSucceeded);
			this.modified.Clear();
			this.unsetCells.Clear();
		}

		// Token: 0x0400B560 RID: 46432
		protected ColorDef colorDef;

		// Token: 0x0400B561 RID: 46433
		public const float UIAlpha = 0.8f;

		// Token: 0x0400B562 RID: 46434
		private readonly List<IntVec3> unsetCells = new List<IntVec3>();

		// Token: 0x0400B563 RID: 46435
		private readonly HashSet<Plan> modified = new HashSet<Plan>();
	}
}
