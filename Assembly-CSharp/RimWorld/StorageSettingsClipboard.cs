using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029BE RID: 10686
	public static class StorageSettingsClipboard
	{
		// Token: 0x1700244D RID: 9293
		// (get) Token: 0x0600EB93 RID: 60307 RVA: 0x00446FDA File Offset: 0x004451DA
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x0600EB94 RID: 60308 RVA: 0x00446FE1 File Offset: 0x004451E1
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
			Messages.Message("StorageSettingsCopiedToClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600EB95 RID: 60309 RVA: 0x0044700F File Offset: 0x0044520F
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
			Messages.Message("StorageSettingsPastedFromClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600EB96 RID: 60310 RVA: 0x00447037 File Offset: 0x00445237
		public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true),
				defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
				defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
				action = delegate()
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(s);
				},
				hotKey = KeyBindingDefOf.Misc4
			};
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true);
			command_Action.defaultLabel = "CommandPasteZoneSettingsLabel".Translate();
			command_Action.defaultDesc = "CommandPasteZoneSettingsDesc".Translate();
			command_Action.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				StorageSettingsClipboard.PasteInto(s);
			};
			command_Action.hotKey = KeyBindingDefOf.Misc5;
			if (!StorageSettingsClipboard.HasCopiedSettings)
			{
				command_Action.Disable(null);
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x04009C59 RID: 40025
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x04009C5A RID: 40026
		private static bool copied = false;
	}
}
