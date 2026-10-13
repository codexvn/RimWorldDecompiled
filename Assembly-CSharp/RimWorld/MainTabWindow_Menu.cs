using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003403 RID: 13315
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x17003115 RID: 12565
		// (get) Token: 0x06012B9F RID: 76703 RVA: 0x0058BB74 File Offset: 0x00589D74
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x17003116 RID: 12566
		// (get) Token: 0x06012BA0 RID: 76704 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06012BA1 RID: 76705 RVA: 0x0058BB85 File Offset: 0x00589D85
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
			this.layer = WindowLayer.Super;
		}

		// Token: 0x06012BA2 RID: 76706 RVA: 0x0058BB9B File Offset: 0x00589D9B
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			if (ModsConfig.IdeologyActive)
			{
				ArchonexusCountdown.CancelCountdown();
			}
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06012BA3 RID: 76707 RVA: 0x0058BBC9 File Offset: 0x00589DC9
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06012BA4 RID: 76708 RVA: 0x0058BBD6 File Offset: 0x00589DD6
		public override void DoWindowContents(Rect rect)
		{
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x0400C27A RID: 49786
		private bool anyGameFiles;
	}
}
