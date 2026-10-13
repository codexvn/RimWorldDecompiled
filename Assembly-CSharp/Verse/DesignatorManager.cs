using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200091B RID: 2331
	public class DesignatorManager
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003C75 RID: 15477 RVA: 0x0013CFC1 File Offset: 0x0013B1C1
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x0013CFC9 File Offset: 0x0013B1C9
		// (set) Token: 0x06003C77 RID: 15479 RVA: 0x0013CFD4 File Offset: 0x0013B1D4
		public DrawStyleDef SelectedStyle
		{
			get
			{
				return this.selectedStyle;
			}
			set
			{
				this.selectedStyle = value;
				this.dragger.UpdateDragCellsIfNeeded();
				DrawStyleCategoryDef drawStyleCategory = this.selectedDesignator.DrawStyleCategory;
				if (drawStyleCategory != null && drawStyleCategory.styles.Contains(value))
				{
					this.previouslySelected[drawStyleCategory] = value;
				}
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x0013D01D File Offset: 0x0013B21D
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x0013D028 File Offset: 0x0013B228
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			DrawStyleCategoryDef drawStyleCategory = des.DrawStyleCategory;
			if (drawStyleCategory != null && !drawStyleCategory.styles.NullOrEmpty<DrawStyleDef>())
			{
				DrawStyleDef drawStyleDef;
				if (Prefs.RememberDrawStlyes && this.previouslySelected.TryGetValue(drawStyleCategory, out drawStyleDef))
				{
					this.selectedStyle = drawStyleDef;
				}
				else
				{
					this.selectedStyle = drawStyleCategory.styles[0];
				}
			}
			else
			{
				this.selectedStyle = null;
			}
			this.selectedDesignator.Selected();
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0013D09F File Offset: 0x0013B29F
		public void Deselect()
		{
			if (this.selectedDesignator == null)
			{
				return;
			}
			this.selectedDesignator.Deselected();
			this.selectedDesignator = null;
			this.dragger.EndDrag();
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x0013D0C7 File Offset: 0x0013B2C7
		private bool CheckSelectedDesignatorValid()
		{
			if (this.selectedDesignator == null)
			{
				return false;
			}
			if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				return false;
			}
			return true;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0013D0EC File Offset: 0x0013B2EC
		public void ProcessInputEvents()
		{
			if (!this.CheckSelectedDesignatorValid())
			{
				return;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if (this.SelectedStyle == null)
				{
					Designator designator = this.selectedDesignator;
					AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
					if (acceptanceReport.Accepted)
					{
						designator.DesignateSingleCell(UI.MouseCell());
						designator.Finalize(true);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
						this.selectedDesignator.Finalize(false);
					}
				}
				else
				{
					this.dragger.StartDrag();
				}
				Event.current.Use();
			}
			if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.Deselect();
				this.dragger.EndDrag();
				Event.current.Use();
				TutorSystem.Notify_Event("ClearDesignatorSelection");
			}
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && (this.dragger.Dragging || (this.selectedStyle != null && this.selectedStyle.DrawStyleWorker.SingleCell)))
			{
				this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
				this.dragger.EndDrag();
				Event.current.Use();
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0013D251 File Offset: 0x0013B451
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
				DrawStyleDef drawStyleDef = this.selectedStyle;
				if (drawStyleDef == null)
				{
					return;
				}
				drawStyleDef.DrawStyleWorker.Draw();
			}
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x0013D286 File Offset: 0x0013B486
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x0013D2A6 File Offset: 0x0013B4A6
		public void AdvanceDrawStyle()
		{
			this.ChangeDrawStyle(1);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0013D2AF File Offset: 0x0013B4AF
		public void PreviousDrawStyle()
		{
			this.ChangeDrawStyle(-1);
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0013D2B8 File Offset: 0x0013B4B8
		private void ChangeDrawStyle(int delta)
		{
			if (this.SelectedStyle == null || this.selectedDesignator.DrawStyleCategory == null || this.selectedDesignator.DrawStyleCategory.styles.NullOrEmpty<DrawStyleDef>())
			{
				return;
			}
			List<DrawStyleDef> styles = this.selectedDesignator.DrawStyleCategory.styles;
			int index = GenMath.PositiveMod(styles.IndexOf(this.SelectedStyle) + delta, styles.Count);
			this.SelectedStyle = styles[index];
		}

		// Token: 0x04002B81 RID: 11137
		private Designator selectedDesignator;

		// Token: 0x04002B82 RID: 11138
		private DrawStyleDef selectedStyle;

		// Token: 0x04002B83 RID: 11139
		private Dictionary<DrawStyleCategoryDef, DrawStyleDef> previouslySelected = new Dictionary<DrawStyleCategoryDef, DrawStyleDef>();

		// Token: 0x04002B84 RID: 11140
		private readonly DesignationDragger dragger = new DesignationDragger();
	}
}
