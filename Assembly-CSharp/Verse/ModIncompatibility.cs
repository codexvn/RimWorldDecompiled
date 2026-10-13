using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000584 RID: 1412
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000BF24E File Offset: 0x000BD44E
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x000BF269 File Offset: 0x000BD469
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId, false) == null;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000BF27C File Offset: 0x000BD47C
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null && modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000BF2E8 File Offset: 0x000BD4E8
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier != null && modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}
	}
}
