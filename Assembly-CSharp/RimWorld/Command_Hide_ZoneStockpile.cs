using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030E9 RID: 12521
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneStockpile : Command_Hide
	{
		// Token: 0x06011969 RID: 72041 RVA: 0x0050C61B File Offset: 0x0050A81B
		public Command_Hide_ZoneStockpile(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x0601196A RID: 72042 RVA: 0x0050CD6F File Offset: 0x0050AF6F
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneStockpile.GetHideOptions();
		}

		// Token: 0x0601196B RID: 72043 RVA: 0x0050CD76 File Offset: 0x0050AF76
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
			foreach (FloatMenuOption floatMenuOption in Command_Hide.ZoneTypeOptions<Zone_Stockpile>("StockpileGroup".Translate(), Command_Hide_ZoneStockpile.IconTex))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400B558 RID: 46424
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
	}
}
