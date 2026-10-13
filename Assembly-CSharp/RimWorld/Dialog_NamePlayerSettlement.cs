using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200319D RID: 12701
	public class Dialog_NamePlayerSettlement : Dialog_GiveName
	{
		// Token: 0x17002E73 RID: 11891
		// (get) Token: 0x06011DF4 RID: 73204 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x06011DF5 RID: 73205 RVA: 0x005305AC File Offset: 0x0052E7AC
		public Dialog_NamePlayerSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x06011DF6 RID: 73206 RVA: 0x0053063F File Offset: 0x0052E83F
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x06011DF7 RID: 73207 RVA: 0x005304A5 File Offset: 0x0052E6A5
		protected override bool IsValidName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x06011DF8 RID: 73208 RVA: 0x00530669 File Offset: 0x0052E869
		protected override void Named(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x0400B8D2 RID: 47314
		private Settlement settlement;
	}
}
