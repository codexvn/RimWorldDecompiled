using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003092 RID: 12434
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : DesignatorWithEyedropper
	{
		// Token: 0x17002D2A RID: 11562
		// (get) Token: 0x0601172B RID: 71467 RVA: 0x00503549 File Offset: 0x00501749
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		// Token: 0x17002D2B RID: 11563
		// (get) Token: 0x0601172C RID: 71468 RVA: 0x0050356F File Offset: 0x0050176F
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x17002D2C RID: 11564
		// (get) Token: 0x0601172D RID: 71469 RVA: 0x0050357C File Offset: 0x0050177C
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x17002D2D RID: 11565
		// (get) Token: 0x0601172E RID: 71470 RVA: 0x0050358C File Offset: 0x0050178C
		public override bool Visible
		{
			get
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002D2E RID: 11566
		// (get) Token: 0x0601172F RID: 71471 RVA: 0x005035C5 File Offset: 0x005017C5
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x17002D2F RID: 11567
		// (get) Token: 0x06011730 RID: 71472 RVA: 0x005035CD File Offset: 0x005017CD
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x06011731 RID: 71473 RVA: 0x005035DA File Offset: 0x005017DA
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		// Token: 0x06011732 RID: 71474 RVA: 0x005035F2 File Offset: 0x005017F2
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		// Token: 0x06011733 RID: 71475 RVA: 0x0050362D File Offset: 0x0050182D
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x06011734 RID: 71476 RVA: 0x0050364C File Offset: 0x0050184C
		public void SetActiveDesignator(Designator des, bool explicitySet = true)
		{
			this.activeDesignator = des;
			this.icon = des.icon;
			this.iconDrawScale = des.iconDrawScale;
			this.iconProportions = des.iconProportions;
			this.iconTexCoords = des.iconTexCoords;
			this.iconAngle = des.iconAngle;
			this.iconOffset = des.iconOffset;
			if (explicitySet)
			{
				this.activeDesignatorSet = true;
			}
		}

		// Token: 0x06011735 RID: 71477 RVA: 0x005036B2 File Offset: 0x005018B2
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x06011736 RID: 71478 RVA: 0x005036C0 File Offset: 0x005018C0
		public override void ProcessInput(Event ev)
		{
			Window window = this.elements.Any(delegate(Designator x)
			{
				Designator_Place designator_Place = x as Designator_Place;
				if (designator_Place != null)
				{
					BuildableDef placingDef = designator_Place.PlacingDef;
					if (placingDef != null)
					{
						return placingDef.designatorDropdown.useGridMenu;
					}
				}
				return false;
			}) ? this.SetupGridMenu(ev) : this.SetupFloatMenu(ev);
			Find.WindowStack.Add(window);
			Find.DesignatorManager.Select(this.activeDesignator);
		}

		// Token: 0x06011737 RID: 71479 RVA: 0x00503728 File Offset: 0x00501928
		private Window SetupGridMenu(Event ev)
		{
			List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
			if (this.elements.Any(delegate(Designator x)
			{
				Designator_Place designator_Place2 = x as Designator_Place;
				if (designator_Place2 != null)
				{
					TerrainDef terrainDef = designator_Place2.PlacingDef as TerrainDef;
					if (terrainDef != null)
					{
						return terrainDef.designatorDropdown.includeEyeDropperTool;
					}
				}
				return false;
			}))
			{
				if (this.eyedropper == null)
				{
					this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
					{
						for (int j = 0; j < this.elements.Count; j++)
						{
							Designator designator2 = this.elements[j];
							Designator_Place designator_Place2 = designator2 as Designator_Place;
							if (designator_Place2 != null)
							{
								TerrainDef terrainDef = designator_Place2.PlacingDef as TerrainDef;
								if (terrainDef != null && terrainDef.colorDef == newCol)
								{
									this.GetDesignatorSelectAction(ev, designator2)();
									return;
								}
							}
						}
					}, "SelectColoredFloor".Translate(), "DesignatorEyeDropperDesc_Carpet".Translate());
				}
				List<FloatMenuGridOption> list2 = list;
				Texture2D eyeDropperTex = Designator_Eyedropper.EyeDropperTex;
				Action action = delegate()
				{
					Find.DesignatorManager.Select(this.eyedropper);
				};
				TipSignal? tooltip = new TipSignal?("DesignatorEyeDropperDesc_Carpet".Translate());
				list2.Add(new FloatMenuGridOption(eyeDropperTex, action, null, tooltip));
			}
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator designator = this.elements[i];
				if (designator.Visible)
				{
					Designator_Place designator_Place = designator as Designator_Place;
					if (designator_Place != null)
					{
						BuildableDef placingDef = designator_Place.PlacingDef;
						if (placingDef != null)
						{
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Cost)
							{
								ThingDef designatorCost = this.GetDesignatorCost(designator);
								if (designatorCost != null)
								{
									List<FloatMenuGridOption> list3 = list;
									Texture2D iconFor = Widgets.GetIconFor(designatorCost, null, null, null);
									Action designatorSelectAction = this.GetDesignatorSelectAction(ev, designator);
									TipSignal? tooltip = new TipSignal?(designator.LabelCap);
									list3.Add(new FloatMenuGridOption(iconFor, designatorSelectAction, null, tooltip));
									goto IL_1EE;
								}
							}
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Placed)
							{
								FloatMenuGridOption floatMenuGridOption = new FloatMenuGridOption((Texture2D)designator_Place.icon, this.GetDesignatorSelectAction(ev, designator), new Color?(designator_Place.IconDrawColor), new TipSignal?(designator.LabelCap));
								if (placingDef is TerrainDef)
								{
									floatMenuGridOption.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)designator_Place.icon);
								}
								list.Add(floatMenuGridOption);
								goto IL_1EE;
							}
							goto IL_1EE;
						}
					}
					Log.Error("Trying to setup grid float menu with designator without icon.");
				}
				IL_1EE:;
			}
			return new FloatMenuGrid(list)
			{
				onCloseCallback = delegate()
				{
					this.activeDesignatorSet = true;
				}
			};
		}

		// Token: 0x06011738 RID: 71480 RVA: 0x00503954 File Offset: 0x00501B54
		private Window SetupFloatMenu(Event ev)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator designator = this.elements[i];
				if (designator.Visible)
				{
					Designator_Place designator_Place = designator as Designator_Place;
					if (designator_Place != null)
					{
						BuildableDef placingDef = designator_Place.PlacingDef;
						if (placingDef != null)
						{
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Cost)
							{
								ThingDef designatorCost = this.GetDesignatorCost(designator);
								if (designatorCost != null)
								{
									list.Add(new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), designatorCost, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
									goto IL_126;
								}
							}
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Placed)
							{
								FloatMenuOption floatMenuOption = new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), (Texture2D)designator_Place.icon, designator_Place.IconDrawColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								if (placingDef is TerrainDef)
								{
									floatMenuOption.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)designator_Place.icon);
								}
								list.Add(floatMenuOption);
								goto IL_126;
							}
						}
					}
					list.Add(new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				IL_126:;
			}
			return new FloatMenu(list)
			{
				onCloseCallback = delegate()
				{
					this.activeDesignatorSet = true;
				}
			};
		}

		// Token: 0x06011739 RID: 71481 RVA: 0x00503AB4 File Offset: 0x00501CB4
		private Action GetDesignatorSelectAction(Event ev, Designator des)
		{
			return delegate()
			{
				this.<>n__0(ev);
				Find.DesignatorManager.Select(des);
				this.SetActiveDesignator(des, true);
			};
		}

		// Token: 0x0601173A RID: 71482 RVA: 0x00503ADB File Offset: 0x00501CDB
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Designator designator = this.activeDesignator;
			if (designator == null)
			{
				return;
			}
			designator.DrawIcon(rect, buttonMat, parms);
		}

		// Token: 0x0601173B RID: 71483 RVA: 0x00503AF0 File Offset: 0x00501CF0
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x0601173C RID: 71484 RVA: 0x00503AFE File Offset: 0x00501CFE
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x0601173D RID: 71485 RVA: 0x00503B0B File Offset: 0x00501D0B
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x0601173E RID: 71486 RVA: 0x00503B1C File Offset: 0x00501D1C
		private ThingDef GetDesignatorCost(Designator des)
		{
			Designator_Place designator_Place = des as Designator_Place;
			if (designator_Place != null)
			{
				BuildableDef placingDef = designator_Place.PlacingDef;
				if (placingDef.CostList != null && placingDef.CostList.Count > 0)
				{
					return placingDef.CostList.MaxBy((ThingDefCountClass c) => c.thingDef.BaseMarketValue * (float)c.count).thingDef;
				}
			}
			return null;
		}

		// Token: 0x0400B494 RID: 46228
		private List<Designator> elements = new List<Designator>();

		// Token: 0x0400B495 RID: 46229
		private Designator activeDesignator;

		// Token: 0x0400B496 RID: 46230
		private bool activeDesignatorSet;

		// Token: 0x0400B497 RID: 46231
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		// Token: 0x0400B498 RID: 46232
		private const float ButSize = 16f;

		// Token: 0x0400B499 RID: 46233
		private const float ButPadding = 1f;
	}
}
