using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BA7 RID: 2983
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x001838E3 File Offset: 0x00181AE3
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x001838F8 File Offset: 0x00181AF8
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x001838FF File Offset: 0x00181AFF
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x0018390C File Offset: 0x00181B0C
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x00183919 File Offset: 0x00181B19
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004C36 RID: 19510 RVA: 0x00183926 File Offset: 0x00181B26
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004C37 RID: 19511 RVA: 0x0018392E File Offset: 0x00181B2E
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004C38 RID: 19512 RVA: 0x001838E3 File Offset: 0x00181AE3
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x00183936 File Offset: 0x00181B36
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06004C3B RID: 19515 RVA: 0x0018393E File Offset: 0x00181B3E
		// (set) Token: 0x06004C3C RID: 19516 RVA: 0x00183946 File Offset: 0x00181B46
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value.CapitalizeFirst();
			}
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x00183958 File Offset: 0x00181B58
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.relatedFaction != null && !Find.FactionManager.AllFactionsListForReading.Contains(this.relatedFaction))
			{
				this.relatedFaction = null;
			}
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x00183A08 File Offset: 0x00181C08
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f && num2 % 5f < 1f)
			{
				float num3 = (float)UI.screenWidth * 0.06f;
				float num4 = 2f * (num2 % 1f) - 1f;
				float num5 = num3 * (1f - num4 * num4);
				rect2.x -= num5;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				GUI.DrawTexture(new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f), TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Rect rect3 = new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f);
				Text.WordWrap = false;
				Widgets.Label(rect3, text);
				Text.WordWrap = true;
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (this.CanDismissWithRightClick && Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x00183D40 File Offset: 0x00181F40
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.Resolve().NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					float y = Mathf.Max(topY - num2 / 2f, 0f);
					Rect infoRect = new Rect(x, y, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect rect = infoRect.AtZero().ContractedBy(10f);
						Widgets.BeginGroup(rect);
						Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), mouseoverText.Resolve());
						Widgets.EndGroup();
					}, true, false, 1f, null, false);
				}
			}
		}

		// Token: 0x06004C40 RID: 19520
		protected abstract string GetMouseoverText();

		// Token: 0x06004C41 RID: 19521
		public abstract void OpenLetter();

		// Token: 0x06004C42 RID: 19522 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Received()
		{
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Removed()
		{
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x00183919 File Offset: 0x00181B19
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x00183E3E File Offset: 0x0018203E
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x00183E46 File Offset: 0x00182046
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID.ToString();
		}

		// Token: 0x0400365D RID: 13917
		public int ID;

		// Token: 0x0400365E RID: 13918
		public LetterDef def;

		// Token: 0x0400365F RID: 13919
		private TaggedString label;

		// Token: 0x04003660 RID: 13920
		public LookTargets lookTargets;

		// Token: 0x04003661 RID: 13921
		public Faction relatedFaction;

		// Token: 0x04003662 RID: 13922
		public int arrivalTick;

		// Token: 0x04003663 RID: 13923
		public float arrivalTime;

		// Token: 0x04003664 RID: 13924
		public string debugInfo;

		// Token: 0x04003665 RID: 13925
		public const float DrawWidth = 38f;

		// Token: 0x04003666 RID: 13926
		public const float DrawHeight = 30f;

		// Token: 0x04003667 RID: 13927
		private const float FallTime = 1f;

		// Token: 0x04003668 RID: 13928
		private const float FallDistance = 200f;
	}
}
