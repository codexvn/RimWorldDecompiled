using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030E0 RID: 12512
	[StaticConstructorOnStartup]
	public class Command_Hide_Plans : Command_Hide
	{
		// Token: 0x06011936 RID: 71990 RVA: 0x0050C61B File Offset: 0x0050A81B
		public Command_Hide_Plans(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x06011937 RID: 71991 RVA: 0x0050C624 File Offset: 0x0050A824
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_Plans.GetHideOptions();
		}

		// Token: 0x06011938 RID: 71992 RVA: 0x0050C62B File Offset: 0x0050A82B
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
			yield return new FloatMenuOption("ShowAllZoneTypes".Translate("Planning".Translate()), delegate()
			{
				Command_Hide.TogglePlans(false);
			}, Command_Hide_Plans.PlanTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield return new FloatMenuOption("HideAllZoneTypes".Translate("Planning".Translate()), delegate()
			{
				Command_Hide.TogglePlans(true);
			}, Command_Hide_Plans.PlanTex, Color.gray, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield break;
		}

		// Token: 0x0400B53E RID: 46398
		private static readonly Texture2D PlanTex = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
	}
}
