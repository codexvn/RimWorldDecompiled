using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028B8 RID: 10424
	public class ScenPart_GameStartDialog : ScenPart
	{
		// Token: 0x0600E40D RID: 58381 RVA: 0x0042A390 File Offset: 0x00428590
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 5f);
			this.text = Widgets.TextArea(scenPartRect, this.text, false);
		}

		// Token: 0x0600E40E RID: 58382 RVA: 0x0042A3C3 File Offset: 0x004285C3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.textKey, "textKey", null, false);
			Scribe_Defs.Look<SoundDef>(ref this.closeSound, "closeSound");
		}

		// Token: 0x0600E40F RID: 58383 RVA: 0x0042A400 File Offset: 0x00428600
		public override void PostGameStart()
		{
			if (Find.GameInitData.startedFromEntry)
			{
				Find.MusicManagerPlay.disabled = true;
				Find.WindowStack.Notify_GameStartDialogOpened();
				DiaNode diaNode = new DiaNode(this.text.NullOrEmpty() ? this.textKey.TranslateSimple() : this.text);
				DiaOption diaOption = new DiaOption();
				diaOption.resolveTree = true;
				diaOption.clickSound = null;
				diaNode.options.Add(diaOption);
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.soundClose = ((this.closeSound != null) ? this.closeSound : SoundDefOf.GameStartSting);
				dialog_NodeTree.closeAction = delegate()
				{
					Find.MusicManagerPlay.ForceSilenceFor(7f);
					Find.MusicManagerPlay.disabled = false;
					Find.WindowStack.Notify_GameStartDialogClosed();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					TutorSystem.Notify_Event("GameStartDialogClosed");
				};
				Find.WindowStack.Add(dialog_NodeTree);
				Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
			}
		}

		// Token: 0x0600E410 RID: 58384 RVA: 0x0042A4EE File Offset: 0x004286EE
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.text.GetHashCodeSafe() ^ this.textKey.GetHashCodeSafe() ^ ((this.closeSound != null) ? this.closeSound.GetHashCode() : 0);
		}

		// Token: 0x040098AB RID: 39083
		private string text = "";

		// Token: 0x040098AC RID: 39084
		private string textKey;

		// Token: 0x040098AD RID: 39085
		private SoundDef closeSound;
	}
}
