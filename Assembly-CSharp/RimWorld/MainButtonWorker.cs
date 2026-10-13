using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A86 RID: 6790
	public abstract class MainButtonWorker
	{
		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x0600984B RID: 38987 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x0600984C RID: 38988 RVA: 0x002C6C7C File Offset: 0x002C4E7C
		public virtual bool Disabled
		{
			get
			{
				return (Find.CurrentMap == null && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.WorldRoutePlanner.Active && Find.WorldRoutePlanner.FormingCaravan && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.TilePicker.Active && !Find.TilePicker.AllowEscape && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World));
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x0600984D RID: 38989 RVA: 0x002C6D15 File Offset: 0x002C4F15
		public virtual bool Visible
		{
			get
			{
				return (!ModsConfig.IdeologyActive || this.def.validWithClassicIdeo || !Find.IdeoManager.classicMode) && this.def.buttonVisible;
			}
		}

		// Token: 0x0600984E RID: 38990
		public abstract void Activate();

		// Token: 0x0600984F RID: 38991 RVA: 0x002C6D44 File Offset: 0x002C4F44
		public virtual void InterfaceTryActivate()
		{
			if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				return;
			}
			if (this.def.closesWorldView && Find.TilePicker.Active && !Find.TilePicker.AllowEscape)
			{
				Messages.Message("MessagePlayerMustSelectTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.Activate();
		}

		// Token: 0x06009850 RID: 38992 RVA: 0x002C6DE4 File Offset: 0x002C4FE4
		public virtual void DoButton(Rect rect)
		{
			Text.Font = GameFont.Small;
			string text = this.def.LabelCap;
			float num = this.def.LabelCapWidth;
			if (num > rect.width - 2f)
			{
				text = this.def.ShortenedLabelCap;
				num = this.def.ShortenedLabelCapWidth;
			}
			if (this.Disabled)
			{
				Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
				{
					Event.current.Use();
					return;
				}
			}
			else
			{
				bool flag = num > 0.85f * rect.width - 1f;
				Rect rect2 = rect;
				string label = (this.def.Icon == null) ? text : "";
				float textLeftMargin = flag ? 2f : -1f;
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2), null, false))
				{
					this.InterfaceTryActivate();
				}
				if (this.def.Icon != null)
				{
					Vector2 vector = rect.center;
					float num2 = 16f;
					if (Mouse.IsOver(rect))
					{
						vector += new Vector2(2f, -2f);
					}
					GUI.DrawTexture(new Rect(vector.x - num2, vector.y - num2, 32f, 32f), this.def.Icon);
				}
				if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
				{
					UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
				}
				if (Mouse.IsOver(rect) && !this.def.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, this.def.LabelCap.Colorize(ColorLibrary.Yellow) + "\n\n" + this.def.description);
				}
			}
		}

		// Token: 0x04006655 RID: 26197
		public MainButtonDef def;

		// Token: 0x04006656 RID: 26198
		private const float CompactModeMargin = 2f;

		// Token: 0x04006657 RID: 26199
		private const float IconSize = 32f;
	}
}
