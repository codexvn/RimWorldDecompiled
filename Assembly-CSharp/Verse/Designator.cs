using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000912 RID: 2322
	public abstract class Designator : Command
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x0013BE03 File Offset: 0x0013A003
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003C33 RID: 15411 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool DrawHighlight
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AlwaysDoGuiControls
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003C37 RID: 15415 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003C39 RID: 15417 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x0013BE0C File Offset: 0x0013A00C
		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				string result;
				if ((result = this.cachedTutorTagSelect) == null)
				{
					result = (this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag);
				}
				return result;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x0013BE48 File Offset: 0x0013A048
		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				string result;
				if ((result = this.cachedTutorTagDesignate) == null)
				{
					result = (this.cachedTutorTagDesignate = "Designate-" + this.tutorTag);
				}
				return result;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x0013BE82 File Offset: 0x0013A082
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x0013BEB0 File Offset: 0x0013A0B0
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in base.RightClickFloatMenuOptions)
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				if (this.hasDesignateAllFloatMenuOption)
				{
					int num = 0;
					List<Thing> things = this.Map.listerThings.AllThings;
					for (int i = 0; i < things.Count; i++)
					{
						Thing t = things[i];
						if (!t.Fogged() && this.CanDesignateThing(t).Accepted)
						{
							num++;
						}
					}
					if (num > 0)
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(num) + ")", delegate()
						{
							for (int k = 0; k < things.Count; k++)
							{
								Thing t2 = things[k];
								if (!t2.Fogged() && this.CanDesignateThing(t2).Accepted)
								{
									this.DesignateThing(things[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				DesignationDef designation = this.Designation;
				if (this.Designation != null)
				{
					this.tmpAllDesignations.Clear();
					this.tmpAllDesignations.AddRange(this.Map.designationManager.designationsByDef[designation]);
					if (this.removeAllOtherDesignationDefs != null)
					{
						foreach (DesignationDef def in this.removeAllOtherDesignationDefs)
						{
							this.tmpAllDesignations.AddRange(this.Map.designationManager.designationsByDef[def]);
						}
					}
					int num2 = 0;
					foreach (Designation designation2 in this.tmpAllDesignations)
					{
						if (this.RemoveAllDesignationsAffects(designation2.target))
						{
							num2++;
						}
					}
					if (num2 > 0)
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + num2.ToString() + ")", delegate()
						{
							for (int k = this.tmpAllDesignations.Count - 1; k >= 0; k--)
							{
								this.Map.designationManager.RemoveDesignation(this.tmpAllDesignations[k]);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0013BEC0 File Offset: 0x0013A0C0
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x0013BEFE File Offset: 0x0013A0FE
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x0013BF1C File Offset: 0x0013A11C
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0013BF3C File Offset: 0x0013A13C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (DebugViewSettings.showArchitectMenuOrder)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(topLeft.x, topLeft.y + 5f, this.GetWidth(maxWidth), 15f), this.Order.ToString());
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			return result;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0013BFA8 File Offset: 0x0013A1A8
		public Command_Action CreateReverseDesignationGizmo(Thing t)
		{
			AcceptanceReport acceptanceReport = this.CanDesignateThing(t);
			if (acceptanceReport.Accepted || (this.showReverseDesignatorDisabledReason && !acceptanceReport.Reason.NullOrEmpty()))
			{
				float iconAngle;
				Vector2 iconOffset;
				return new Command_Action
				{
					defaultLabel = this.LabelCapReverseDesignating(t),
					icon = this.IconReverseDesignating(t, out iconAngle, out iconOffset),
					iconAngle = iconAngle,
					iconOffset = iconOffset,
					defaultDesc = (acceptanceReport.Reason.NullOrEmpty() ? this.DescReverseDesignating(t) : acceptanceReport.Reason),
					Order = ((this is Designator_Uninstall) ? -11f : -20f),
					Disabled = !acceptanceReport.Accepted,
					action = delegate()
					{
						if (!TutorSystem.AllowAction(this.TutorTagDesignate))
						{
							return;
						}
						if (this.Designation != null && this.Designation.targetType == TargetType.Thing && this.Map.designationManager.DesignationOn(t, this.Designation) != null)
						{
							return;
						}
						this.DesignateThing(t);
						this.Finalize(true);
					},
					hotKey = this.hotKey,
					groupKeyIgnoreContent = this.groupKeyIgnoreContent,
					groupKey = this.groupKey
				};
			}
			return null;
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0013C0C1 File Offset: 0x0013A2C1
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003C45 RID: 15429
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x06003C46 RID: 15430 RVA: 0x0013C0C8 File Offset: 0x0013A2C8
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
			bool somethingSucceeded = false;
			bool flag = false;
			foreach (IntVec3 intVec in cells)
			{
				if (this.CanDesignateCell(intVec).Accepted)
				{
					this.DesignateSingleCell(intVec);
					somethingSucceeded = true;
					if (!flag)
					{
						flag = this.ShowWarningForCell(intVec);
					}
				}
			}
			this.Finalize(somethingSucceeded);
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
			}
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x0013C170 File Offset: 0x0013A370
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x0013C182 File Offset: 0x0013A382
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0013C198 File Offset: 0x0013A398
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x0013C1E4 File Offset: 0x0013A3E4
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x0013C1EC File Offset: 0x0013A3EC
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0013C1F4 File Offset: 0x0013A3F4
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return (Texture2D)this.icon;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0013C218 File Offset: 0x0013A418
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.hideMouseIcon ? null : this.icon, this.mouseText, this.iconAngle, this.iconOffset, null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x0013C27C File Offset: 0x0013A47C
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
			Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
			DrawStyleDef style = Find.DesignatorManager.SelectedStyle;
			DrawStyleCategoryDef drawStyleCategory = this.DrawStyleCategory;
			List<DrawStyleDef> list = (drawStyleCategory != null) ? drawStyleCategory.styles : null;
			if (style == null || list == null || list.Count <= 1)
			{
				return;
			}
			Find.WindowStack.ImmediateWindow(415111, winRect, WindowLayer.GameUI, delegate
			{
				using (new TextBlock(GameFont.Medium, TextAnchor.MiddleCenter))
				{
					Rect winRect = winRect;
					winRect.x = 0f;
					winRect.y = 0f;
					Rect rect = winRect.MiddlePartPixels(48f, 48f);
					if (Widgets.ButtonImage(rect, style.uiIcon, true, null))
					{
						List<FloatMenuOption> list2 = new List<FloatMenuOption>();
						foreach (DrawStyleDef drawStyleDef in this.DrawStyleCategory.styles)
						{
							DrawStyleDef lDef = drawStyleDef;
							list2.Add(new FloatMenuOption(drawStyleDef.LabelCap, delegate()
							{
								Find.DesignatorManager.SelectedStyle = lDef;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list2));
					}
					Rect rect2 = winRect.MiddlePartPixels(60f, 60f);
					rect2.x = rect.xMin - 16f - rect2.width;
					Rect rect3 = rect;
					rect3.x = rect2.xMin + 27f;
					rect3.xMax = rect2.xMax + 27f;
					Rect butRect = rect2;
					butRect.x = rect.xMax + 16f;
					Rect rect4 = rect;
					rect4.x = butRect.xMin - 27f;
					rect4.xMax = butRect.xMax - 27f;
					if (Widgets.ButtonImage(rect2, TexUI.ConcaveArrowTexLeft, true, null))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						Find.DesignatorManager.PreviousDrawStyle();
						Event.current.Use();
					}
					if (!SteamDeck.IsSteamDeck)
					{
						Widgets.Label(rect3, KeyBindingDefOf.Designator_PreviousDrawStyle.MainKeyLabel);
					}
					if (Widgets.ButtonImage(butRect, TexUI.ConcaveArrowTexRight, true, null))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						Find.DesignatorManager.AdvanceDrawStyle();
						Event.current.Use();
					}
					if (!SteamDeck.IsSteamDeck)
					{
						Widgets.Label(rect4, KeyBindingDefOf.Designator_NextDrawStyle.MainKeyLabel);
					}
				}
			}, true, false, 1f, null, false);
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0013C318 File Offset: 0x0013A518
		public virtual void SelectedProcessInput(Event ev)
		{
			if (KeyBindingDefOf.Designator_NextDrawStyle.KeyDownEvent)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Find.DesignatorManager.AdvanceDrawStyle();
			}
			if (KeyBindingDefOf.Designator_PreviousDrawStyle.KeyDownEvent)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Find.DesignatorManager.PreviousDrawStyle();
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Selected()
		{
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Deselected()
		{
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0013C367 File Offset: 0x0013A567
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x04002B59 RID: 11097
		protected bool useMouseIcon;

		// Token: 0x04002B5A RID: 11098
		public bool isOrder;

		// Token: 0x04002B5B RID: 11099
		public SoundDef soundDragSustain;

		// Token: 0x04002B5C RID: 11100
		public SoundDef soundDragChanged;

		// Token: 0x04002B5D RID: 11101
		public SoundDef soundSucceeded;

		// Token: 0x04002B5E RID: 11102
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04002B5F RID: 11103
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04002B60 RID: 11104
		protected string designateAllLabel;

		// Token: 0x04002B61 RID: 11105
		protected bool showReverseDesignatorDisabledReason;

		// Token: 0x04002B62 RID: 11106
		protected DesignationDef[] removeAllOtherDesignationDefs;

		// Token: 0x04002B63 RID: 11107
		private string cachedTutorTagSelect;

		// Token: 0x04002B64 RID: 11108
		private string cachedTutorTagDesignate;

		// Token: 0x04002B65 RID: 11109
		protected string cachedHighlightTag;

		// Token: 0x04002B66 RID: 11110
		private List<Designation> tmpAllDesignations = new List<Designation>();

		// Token: 0x04002B67 RID: 11111
		private const float IconButtonSize = 48f;

		// Token: 0x04002B68 RID: 11112
		private const float IconButtonGap = 16f;

		// Token: 0x04002B69 RID: 11113
		private const float StyleButtonSize = 60f;

		// Token: 0x04002B6A RID: 11114
		private const float StyleButtonLabelOffset = 27f;
	}
}
