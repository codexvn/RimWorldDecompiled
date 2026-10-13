using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030E3 RID: 12515
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneFishing : Command_Hide
	{
		// Token: 0x06011948 RID: 72008 RVA: 0x0050C61B File Offset: 0x0050A81B
		public Command_Hide_ZoneFishing(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x06011949 RID: 72009 RVA: 0x0050C897 File Offset: 0x0050AA97
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneFishing.GetHideOptions();
		}

		// Token: 0x0601194A RID: 72010 RVA: 0x0050C89E File Offset: 0x0050AA9E
		public static IEnumerable<FloatMenuOption> GetHideOptions()
		{
			yield return new FloatMenuOption("ShowAllZones".Translate(), delegate()
			{
				Command_Hide.ToggleAll(false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield return new FloatMenuOption("HideAllZones".Translate(), delegate()
			{
				Command_Hide.ToggleAll(true);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (Command_Hide_ZoneFishing.cachedIcon == null)
			{
				Command_Hide_ZoneFishing.cachedIcon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Fishing", true);
			}
			foreach (FloatMenuOption floatMenuOption in Command_Hide.ZoneTypeOptions<Zone_Fishing>("FishingGroup".Translate(), Command_Hide_ZoneFishing.cachedIcon))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400B547 RID: 46407
		private const string IconTexPath = "UI/Designators/ZoneCreate_Fishing";

		// Token: 0x0400B548 RID: 46408
		private static Texture2D cachedIcon;
	}
}
