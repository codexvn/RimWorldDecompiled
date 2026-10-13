using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030E6 RID: 12518
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneGrow : Command_Hide
	{
		// Token: 0x06011958 RID: 72024 RVA: 0x0050C61B File Offset: 0x0050A81B
		public Command_Hide_ZoneGrow(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x06011959 RID: 72025 RVA: 0x0050CB07 File Offset: 0x0050AD07
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneGrow.GetHideOptions();
		}

		// Token: 0x0601195A RID: 72026 RVA: 0x0050CB0E File Offset: 0x0050AD0E
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
			foreach (FloatMenuOption floatMenuOption in Command_Hide.ZoneTypeOptions<Zone_Growing>("GrowingGroup".Translate(), Command_Hide_ZoneGrow.IconTex))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400B550 RID: 46416
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
	}
}
