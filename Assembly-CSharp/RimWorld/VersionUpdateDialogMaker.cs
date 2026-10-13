using System;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003776 RID: 14198
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x06013ACB RID: 80587 RVA: 0x005D8798 File Offset: 0x005D6998
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x06013ACC RID: 80588 RVA: 0x005D87D8 File Offset: 0x005D69D8
		private static void CreateNewVersionDialog()
		{
			string value = LastPlayedVersion.Version.Major.ToString() + "." + LastPlayedVersion.Version.Minor.ToString();
			string value2 = VersionControl.CurrentMajor.ToString() + "." + VersionControl.CurrentMinor.ToString();
			string text = "GameUpdatedToNewVersionInitial".Translate(value, value2);
			text += "\n\n";
			if (BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString()))
			{
				text += "GameUpdatedToNewVersionSavesCompatible".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSavesIncompatible".Translate();
			}
			text += "\n\n";
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "GameUpdatedToNewVersionSteamController".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSteam".Translate();
			}
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			VersionUpdateDialogMaker.dialogDone = true;
		}

		// Token: 0x0400DA01 RID: 55809
		private static bool dialogDone;
	}
}
