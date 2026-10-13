using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032AD RID: 12973
	public class Screen_Credits : Window
	{
		// Token: 0x17002F0D RID: 12045
		// (get) Token: 0x0601240C RID: 74764 RVA: 0x0000940B File Offset: 0x0000760B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002F0E RID: 12046
		// (get) Token: 0x0601240D RID: 74765 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002F0F RID: 12047
		// (get) Token: 0x0601240E RID: 74766 RVA: 0x0055F01A File Offset: 0x0055D21A
		private float ViewWidth
		{
			get
			{
				return 800f;
			}
		}

		// Token: 0x17002F10 RID: 12048
		// (get) Token: 0x0601240F RID: 74767 RVA: 0x0055F024 File Offset: 0x0055D224
		private float ViewHeight
		{
			get
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = this.creds.Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth)) + 20f;
				Text.Font = font;
				return result;
			}
		}

		// Token: 0x17002F11 RID: 12049
		// (get) Token: 0x06012410 RID: 74768 RVA: 0x0055F060 File Offset: 0x0055D260
		private float MaxScrollPosition
		{
			get
			{
				return Mathf.Max(this.ViewHeight - (float)UI.screenHeight / 2f, 0f);
			}
		}

		// Token: 0x17002F12 RID: 12050
		// (get) Token: 0x06012411 RID: 74769 RVA: 0x0055F080 File Offset: 0x0055D280
		private float AutoScrollRate
		{
			get
			{
				if (!this.wonGame)
				{
					return 30f;
				}
				if (this.scrollPosition < this.victoryTextHeight - 200f)
				{
					return 20f;
				}
				float num = 130f;
				if (this.EndCreditsSong != null)
				{
					num = this.EndCreditsSong.clip.length + this.songStartDelay - 10f - this.victoryTextHeight / 20f;
				}
				float t = (this.scrollPosition - this.victoryTextHeight) / 200f;
				return Mathf.Lerp(20f, this.MaxScrollPosition / num, t);
			}
		}

		// Token: 0x17002F13 RID: 12051
		// (get) Token: 0x06012412 RID: 74770 RVA: 0x0055F115 File Offset: 0x0055D315
		private SongDef EndCreditsSong
		{
			get
			{
				return this.endCreditsSong;
			}
		}

		// Token: 0x06012413 RID: 74771 RVA: 0x0055F11D File Offset: 0x0055D31D
		public Screen_Credits() : this("", 50)
		{
		}

		// Token: 0x06012414 RID: 74772 RVA: 0x0055F12C File Offset: 0x0055D32C
		public Screen_Credits(string preCreditsMessage, int preCreditsSpace = 50) : base(null)
		{
			this.doWindowBackground = false;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.forcePause = true;
			this.closeOnCancel = false;
			this.creds = CreditsAssembler.AllCredits().ToList<CreditsEntry>();
			this.creds.Insert(0, new CreditRecord_Space(100f));
			if (!preCreditsMessage.NullOrEmpty())
			{
				this.creds.Insert(1, new CreditRecord_Space(200f));
				this.creds.Insert(2, new CreditRecord_Text(preCreditsMessage, TextAnchor.UpperLeft));
				this.creds.Insert(3, new CreditRecord_Space((float)preCreditsSpace));
				Text.Font = GameFont.Medium;
				this.victoryTextHeight = this.creds.Take(4).Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth));
			}
			this.creds.Add(new CreditRecord_Space(300f));
			this.creds.Add(new CreditRecord_Text("ThanksForPlaying".Translate(), TextAnchor.UpperCenter));
			string text = string.Empty;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role == null)
				{
					text = string.Empty;
				}
				else
				{
					creditRecord_Role.displayKey = (text.NullOrEmpty() || creditRecord_Role.roleKey != text);
					text = creditRecord_Role.roleKey;
				}
			}
		}

		// Token: 0x06012415 RID: 74773 RVA: 0x0055F2BC File Offset: 0x0055D4BC
		public override void PreOpen()
		{
			base.PreOpen();
			Screen_Credits.creationRealtime = Time.realtimeSinceStartup;
			Screen_Credits.creditsShowing = true;
			if (this.wonGame)
			{
				this.timeUntilAutoScroll = 10f;
				return;
			}
			this.timeUntilAutoScroll = 1f;
		}

		// Token: 0x06012416 RID: 74774 RVA: 0x0055F2F3 File Offset: 0x0055D4F3
		public override void PostClose()
		{
			base.PostOpen();
			Screen_Credits.creditsShowing = false;
			if (this.exitToMainMenu)
			{
				GenScene.GoToMainMenu();
			}
		}

		// Token: 0x06012417 RID: 74775 RVA: 0x0055F310 File Offset: 0x0055D510
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.timeUntilAutoScroll > 0f)
			{
				this.timeUntilAutoScroll -= Time.deltaTime;
			}
			else
			{
				this.scrollPosition += this.AutoScrollRate * Time.deltaTime;
			}
			if (this.wonGame && this.EndCreditsSong != null && !this.playedMusic && Time.realtimeSinceStartup > Screen_Credits.creationRealtime + this.songStartDelay)
			{
				Find.MusicManagerPlay.ForcePlaySong(this.EndCreditsSong, false);
				this.playedMusic = true;
			}
			if (this.closeDelay > 0f)
			{
				this.closeDelay -= Time.deltaTime;
				if (this.closeDelay <= 0f)
				{
					ScreenFader.StartFade(Color.clear, 3f);
					this.Close(true);
					if (Current.ProgramState == ProgramState.Playing)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					}
				}
			}
		}

		// Token: 0x06012418 RID: 74776 RVA: 0x0055F3F8 File Offset: 0x0055D5F8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			GUI.DrawTexture(rect, BaseContent.BlackTex);
			Rect rect2 = new Rect(rect);
			rect2.yMin += 30f;
			rect2.yMax -= 30f;
			rect2.xMin = rect.center.x - 400f;
			rect2.width = 800f;
			float viewWidth = this.ViewWidth;
			float viewHeight = this.ViewHeight;
			this.scrollPosition = Mathf.Clamp(this.scrollPosition, 0f, this.MaxScrollPosition);
			Widgets.BeginGroup(rect2);
			Rect rect3 = new Rect(0f, 0f, viewWidth, viewHeight);
			rect3.y -= this.scrollPosition;
			Widgets.BeginGroup(rect3);
			Text.Font = GameFont.Medium;
			float num = 0f;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				float num2 = creditsEntry.DrawHeight(rect3.width);
				Rect rect4 = new Rect(0f, num, rect3.width, num2);
				creditsEntry.Draw(rect4);
				num += num2;
			}
			Widgets.EndGroup();
			Widgets.EndGroup();
			if (this.closeDelay < 0f && this.scrollPosition > 0f && Widgets.ButtonText(new Rect(rect.xMax - 200f, rect.yMax - 100f, 150f, 50f), "SkipCredits".Translate(), true, true, true, null))
			{
				this.OnCancelKeyPressed();
			}
			if (Event.current.type == EventType.ScrollWheel)
			{
				this.Scroll(Event.current.delta.y * 25f);
				Event.current.Use();
			}
			if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.DownArrow)
				{
					this.Scroll(250f);
					Event.current.Use();
				}
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					this.Scroll(-250f);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06012419 RID: 74777 RVA: 0x0055F660 File Offset: 0x0055D860
		private void Scroll(float offset)
		{
			this.scrollPosition += offset;
			this.timeUntilAutoScroll = 3f;
		}

		// Token: 0x0601241A RID: 74778 RVA: 0x0055F67B File Offset: 0x0055D87B
		public override void OnCancelKeyPressed()
		{
			Event.current.Use();
			if (this.closeDelay > 0f)
			{
				return;
			}
			this.closeDelay = 3f;
			ScreenFader.StartFade(Color.black, 3f);
		}

		// Token: 0x0400BDF2 RID: 48626
		private List<CreditsEntry> creds;

		// Token: 0x0400BDF3 RID: 48627
		public bool wonGame;

		// Token: 0x0400BDF4 RID: 48628
		public SongDef endCreditsSong;

		// Token: 0x0400BDF5 RID: 48629
		public bool exitToMainMenu;

		// Token: 0x0400BDF6 RID: 48630
		public float songStartDelay = 5f;

		// Token: 0x0400BDF7 RID: 48631
		private float timeUntilAutoScroll;

		// Token: 0x0400BDF8 RID: 48632
		private float scrollPosition;

		// Token: 0x0400BDF9 RID: 48633
		private bool playedMusic;

		// Token: 0x0400BDFA RID: 48634
		private float victoryTextHeight;

		// Token: 0x0400BDFB RID: 48635
		private float closeDelay = -1f;

		// Token: 0x0400BDFC RID: 48636
		public static bool creditsShowing;

		// Token: 0x0400BDFD RID: 48637
		public static float creationRealtime = -1f;

		// Token: 0x0400BDFE RID: 48638
		private const int ColumnWidth = 800;

		// Token: 0x0400BDFF RID: 48639
		private const float InitialAutoScrollDelay = 1f;

		// Token: 0x0400BE00 RID: 48640
		private const float InitialAutoScrollDelayWonGame = 10f;

		// Token: 0x0400BE01 RID: 48641
		private const float AutoScrollDelayAfterManualScroll = 3f;

		// Token: 0x0400BE02 RID: 48642
		private const float VictoryTextScrollSpeed = 20f;

		// Token: 0x0400BE03 RID: 48643
		private const float ScrollSpeedLerpHeight = 200f;

		// Token: 0x0400BE04 RID: 48644
		private const GameFont Font = GameFont.Medium;

		// Token: 0x0400BE05 RID: 48645
		public const float DefaultSongStartDelay = 5f;

		// Token: 0x0400BE06 RID: 48646
		private const int CloseTransitionSeconds = 3;
	}
}
