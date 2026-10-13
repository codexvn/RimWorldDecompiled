using System;

namespace RimWorld
{
	// Token: 0x02003198 RID: 12696
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x17002E6F RID: 11887
		// (get) Token: 0x06011DDB RID: 73179 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x06011DDC RID: 73180 RVA: 0x005301FC File Offset: 0x0052E3FC
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06011DDD RID: 73181 RVA: 0x00530253 File Offset: 0x0052E453
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06011DDE RID: 73182 RVA: 0x0053025B File Offset: 0x0052E45B
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
