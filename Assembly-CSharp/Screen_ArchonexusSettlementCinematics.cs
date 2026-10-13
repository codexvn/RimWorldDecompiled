using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

// Token: 0x02000040 RID: 64
public class Screen_ArchonexusSettlementCinematics : Window
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000152 RID: 338 RVA: 0x0000940B File Offset: 0x0000760B
	public override Vector2 InitialSize
	{
		get
		{
			return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000153 RID: 339 RVA: 0x00006AAC File Offset: 0x00004CAC
	protected override float Margin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000154 RID: 340 RVA: 0x0000941E File Offset: 0x0000761E
	private float FadeToBlackEndTime
	{
		get
		{
			return this.ScreenStartTime + 2f + 0.2f;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000155 RID: 341 RVA: 0x00009432 File Offset: 0x00007632
	private float MessageDisplayEndTime
	{
		get
		{
			return this.FadeToBlackEndTime + 7f;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009440 File Offset: 0x00007640
	public Screen_ArchonexusSettlementCinematics(Action cameraJumpAction, Action nextStepAction) : base(null)
	{
		this.doWindowBackground = false;
		this.doCloseButton = false;
		this.doCloseX = false;
		this.forcePause = true;
		this.cameraJumpAction = cameraJumpAction;
		this.nextStepAction = nextStepAction;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009474 File Offset: 0x00007674
	public override void PreOpen()
	{
		base.PreOpen();
		Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(11.2f, 1.5f, false);
		SoundDefOf.ArchonexusNewColonyAccept.PlayOneShotOnCamera(null);
		ScreenFader.StartFade(Color.black, 2f);
		this.ScreenStartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000094C1 File Offset: 0x000076C1
	public override void PostClose()
	{
		base.PostOpen();
		ScreenFader.SetColor(Color.black);
		this.nextStepAction();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000094E0 File Offset: 0x000076E0
	public override void DoWindowContents(Rect inRect)
	{
		if (!this.IsFinishedFadingIn())
		{
			return;
		}
		if (!this.FadeInLatch)
		{
			this.FadeInLatch = true;
			this.cameraJumpAction();
			ScreenFader.SetColor(Color.clear);
		}
		if (this.IsFinishedDisplayMessage())
		{
			this.Close(false);
			return;
		}
		Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
		GUI.DrawTexture(rect, BaseContent.BlackTex);
		Rect rect2 = new Rect(rect);
		rect2.xMin = rect.center.x - 400f;
		rect2.width = 800f;
		rect2.yMin = rect.center.y;
		GameFont font = Text.Font;
		TextAnchor anchor = Text.Anchor;
		Color color = GUI.color;
		Text.Font = GameFont.Medium;
		GUI.color = Color.white;
		Text.Anchor = TextAnchor.MiddleCenter;
		Widgets.Label(new Rect(inRect), "SoldColonyDescription".Translate());
		Text.Font = font;
		GUI.color = color;
		Text.Anchor = anchor;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000095DD File Offset: 0x000077DD
	public bool IsFinishedFadingIn()
	{
		return Time.realtimeSinceStartup > this.FadeToBlackEndTime;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000095EC File Offset: 0x000077EC
	public bool IsFinishedDisplayMessage()
	{
		return Time.realtimeSinceStartup > this.MessageDisplayEndTime;
	}

	// Token: 0x0400009C RID: 156
	private Action cameraJumpAction;

	// Token: 0x0400009D RID: 157
	private Action nextStepAction;

	// Token: 0x0400009E RID: 158
	private bool FadeInLatch;

	// Token: 0x0400009F RID: 159
	private float ScreenStartTime;

	// Token: 0x040000A0 RID: 160
	public const float FadeSecs = 2f;

	// Token: 0x040000A1 RID: 161
	private const float MessageDisplaySecs = 7f;

	// Token: 0x040000A2 RID: 162
	private const float FadeBuffer = 0.2f;

	// Token: 0x040000A3 RID: 163
	private const int MessageWidth = 800;
}
