using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200008D RID: 141
	[StaticConstructorOnStartup]
	public class Dialog_DevMusic : Window_Dev
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0001044A File Offset: 0x0000E64A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(275f, 360f);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001045C File Offset: 0x0000E65C
		public Dialog_DevMusic()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000104BC File Offset: 0x0000E6BC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			DevGUI.Label(rect, "Music Debugger");
			float y = rect.height + 6f;
			Text.Font = GameFont.Tiny;
			MusicManagerPlay musicManagerPlay = Find.MusicManagerPlay;
			this.PrintLabel(string.Format("State: {0}", musicManagerPlay.State), inRect, ref y);
			this.PrintLabel("", inRect, ref y);
			this.PrintLabel("Song: " + (musicManagerPlay.IsPlaying ? string.Format("{0} ({1:0}/{2:0}s)", musicManagerPlay.CurrentSong.defName, musicManagerPlay.SongTime, musicManagerPlay.SongDuration) : "NA"), inRect, ref y);
			if (musicManagerPlay.State == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.PrintLabel(string.Format("Fadeout Progress: {0} ({1:0.0}s)", musicManagerPlay.FadeoutPercent.ToStringPercent("0"), musicManagerPlay.FadeoutDuration), inRect, ref y);
			}
			else if (musicManagerPlay.IsPlaying)
			{
				this.PrintLabel("Volume: " + musicManagerPlay.CurSanitizedVolume.ToStringPercent("0"), inRect, ref y);
			}
			else
			{
				this.PrintLabel("Next Song: " + ((!musicManagerPlay.IsPlaying && musicManagerPlay.NextSongTimer > 0f) ? string.Format("{0:0.0}s", musicManagerPlay.NextSongTimer) : "NA"), inRect, ref y);
			}
			this.PrintLabel("Pause Vol Factor: " + musicManagerPlay.PausedVolumeFactor.ToStringPercent("0"), inRect, ref y);
			Rect rect2 = inRect;
			rect2.y = y;
			rect2.height = 18f;
			rect2.xMin = rect2.width * 0.65f;
			rect2.xMax -= 2f;
			this.PrintLabel(string.Format("Danger Mode: {0}", musicManagerPlay.DangerMusicMode), inRect, ref y);
			if (DevGUI.ButtonText(rect2, musicManagerPlay.OverrideDangerMode ? "Override On" : "Override Off", null))
			{
				musicManagerPlay.OverrideDangerMode = !musicManagerPlay.OverrideDangerMode;
			}
			this.PrintLabel("", inRect, ref y);
			this.PrintLabel("Source Transition: " + ((musicManagerPlay.TriggeredTransition != null) ? musicManagerPlay.TriggeredTransition.def.defName : "NA"), inRect, ref y);
			this.PrintLabel("Current Sequence: " + ((musicManagerPlay.MusicSequenceWorker != null) ? musicManagerPlay.MusicSequenceWorker.def.defName : "NA"), inRect, ref y);
			MusicSequenceWorker musicSequenceWorker = musicManagerPlay.MusicSequenceWorker;
			if (musicSequenceWorker != null)
			{
				this.PrintLabel("   - Next Sequence: " + ((musicSequenceWorker.def.nextSequence != null) ? musicSequenceWorker.def.nextSequence.defName : "NA"), inRect, ref y);
				this.PrintLabel(string.Format("   - Loop: {0}", musicSequenceWorker.ShouldLoop()), inRect, ref y);
				this.PrintLabel(string.Format("   - Can Transition: {0}", musicSequenceWorker.ShouldTransition()), inRect, ref y);
				this.PrintLabel(string.Format("   - Interruptible: {0}", musicSequenceWorker.CanBeInterrupted()), inRect, ref y);
				this.PrintLabel(string.Format("   - Times Looped: {0}", musicSequenceWorker.timesLooped), inRect, ref y);
			}
			Rect rect3 = inRect;
			rect3.yMin = rect3.yMax - 30f - 2f;
			rect3.yMax -= 2f;
			rect3.xMin = 2f;
			rect3.xMax = inRect.width / 2f - 2f;
			Rect rect4 = inRect;
			rect4.yMin = rect4.yMax - 30f - 2f;
			rect4.yMax -= 2f;
			rect4.xMin = inRect.width / 2f + 2f;
			rect4.xMax -= 2f;
			if (DevGUI.ButtonText(rect3, musicManagerPlay.IsPlaying ? "Stop" : "Start", new TextAnchor?(TextAnchor.MiddleCenter)))
			{
				if (musicManagerPlay.IsPlaying)
				{
					musicManagerPlay.Stop();
				}
				else
				{
					musicManagerPlay.ScheduleNewSong();
				}
			}
			if (musicManagerPlay.MusicSequenceWorker == null)
			{
				if (DevGUI.ButtonText(rect4, "Trigger Transition", new TextAnchor?(TextAnchor.MiddleCenter)))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (MusicTransitionDef local2 in DefDatabase<MusicTransitionDef>.AllDefs)
					{
						MusicTransitionDef local = local2;
						list.Add(new FloatMenuOption(local.defName, delegate()
						{
							Find.MusicManagerPlay.ForceTriggerTransition(local);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					FloatMenu window = new FloatMenu(list, "Select transition", false);
					Find.WindowStack.Add(window);
					return;
				}
			}
			else
			{
				bool flag = musicManagerPlay.MusicSequenceWorker.def.nextSequence != null;
				if (DevGUI.ButtonText(rect4, flag ? "Next Sequence" : "Next Song", new TextAnchor?(TextAnchor.MiddleCenter)))
				{
					musicManagerPlay.ForceTriggerNextSongOrSequence();
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0001016B File Offset: 0x0000E36B
		private void PrintLabel(string text, Rect container, ref float y)
		{
			DevGUI.Label(new Rect(container.x, y, container.width, 20f), text);
			y += 20f;
		}

		// Token: 0x040001BE RID: 446
		private Vector2 windowPosition;

		// Token: 0x040001BF RID: 447
		private const string Title = "Music Debugger";

		// Token: 0x040001C0 RID: 448
		private const float ButtonHeight = 30f;
	}
}
