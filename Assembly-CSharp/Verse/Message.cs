using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BC8 RID: 3016
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0018893B File Offset: 0x00186B3B
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06004D2A RID: 19754 RVA: 0x00188949 File Offset: 0x00186B49
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06004D2B RID: 19755 RVA: 0x00188957 File Offset: 0x00186B57
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x00188969 File Offset: 0x00186B69
		public float Alpha
		{
			get
			{
				if (this.TimeLeft < 0.6f)
				{
					return this.TimeLeft / 0.6f;
				}
				return 1f;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004D2D RID: 19757 RVA: 0x0018898C File Offset: 0x00186B8C
		private static bool ShouldDrawBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x00002C42 File Offset: 0x00000E42
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004D2F RID: 19759 RVA: 0x000440FD File Offset: 0x000422FD
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x001889CB File Offset: 0x00186BCB
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x001889D8 File Offset: 0x00186BD8
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x001889E0 File Offset: 0x00186BE0
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004D33 RID: 19763 RVA: 0x001889E8 File Offset: 0x00186BE8
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x001889F3 File Offset: 0x00186BF3
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x001889FB File Offset: 0x00186BFB
		public Message()
		{
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x00188A18 File Offset: 0x00186C18
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.ResetTimer();
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextMessageID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x00188A77 File Offset: 0x00186C77
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x00188A88 File Offset: 0x00186C88
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x00188A9C File Offset: 0x00186C9C
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x00188B40 File Offset: 0x00186D40
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ExpandedBy(6f, 2f);
			return this.lastDrawRect;
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x00188BBC File Offset: 0x00186DBC
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()) || (this.quest != null && !this.quest.hidden))
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				if (this.flashTime + 0.6f > RealTime.LastRealTime)
				{
					float transparency = 1f - (RealTime.LastRealTime - this.flashTime) / 0.6f;
					Widgets.DrawTextHighlight(rect, 0f, new Color?(ColorLibrary.Yellow.ToTransparent(transparency)));
				}
				Widgets.Label(new Rect(6f, 2f, rect.width - 6f, rect.height - 2f), this.text);
				if (Current.ProgramState == ProgramState.Playing && Widgets.ButtonInvisible(rect, true))
				{
					if (Event.current.button == 1)
					{
						this.startingTime = -99999f;
					}
					else if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					else if (this.quest != null && !this.quest.hidden)
					{
						if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Quests)
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						}
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f, null, false);
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x00188C1C File Offset: 0x00186E1C
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x00188C4C File Offset: 0x00186E4C
		public void ResetTimer()
		{
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x00188C6F File Offset: 0x00186E6F
		public void Flash()
		{
			this.flashTime = RealTime.LastRealTime;
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x00188C7C File Offset: 0x00186E7C
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID.ToString();
		}

		// Token: 0x040036DE RID: 14046
		public MessageTypeDef def;

		// Token: 0x040036DF RID: 14047
		private int ID;

		// Token: 0x040036E0 RID: 14048
		public string text;

		// Token: 0x040036E1 RID: 14049
		private float startingTime;

		// Token: 0x040036E2 RID: 14050
		public int startingFrame;

		// Token: 0x040036E3 RID: 14051
		public int startingTick;

		// Token: 0x040036E4 RID: 14052
		private float flashTime;

		// Token: 0x040036E5 RID: 14053
		public LookTargets lookTargets;

		// Token: 0x040036E6 RID: 14054
		public Quest quest;

		// Token: 0x040036E7 RID: 14055
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x040036E8 RID: 14056
		public Rect lastDrawRect;

		// Token: 0x040036E9 RID: 14057
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x040036EA RID: 14058
		private const float FadeoutDuration = 0.6f;

		// Token: 0x040036EB RID: 14059
		private const float FlashDuration = 0.6f;
	}
}
