using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200319B RID: 12699
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x17002E70 RID: 11888
		// (get) Token: 0x06011DE5 RID: 73189 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x17002E71 RID: 11889
		// (get) Token: 0x06011DE6 RID: 73190 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected override int SecondCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x06011DE7 RID: 73191 RVA: 0x005303A8 File Offset: 0x0052E5A8
		public Dialog_NamePlayerFactionAndSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidSecondName), false, null, null, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x06011DE8 RID: 73192 RVA: 0x0053047B File Offset: 0x0052E67B
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x06011DE9 RID: 73193 RVA: 0x00530253 File Offset: 0x0052E453
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06011DEA RID: 73194 RVA: 0x005304A5 File Offset: 0x0052E6A5
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x06011DEB RID: 73195 RVA: 0x0053025B File Offset: 0x0052E45B
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x06011DEC RID: 73196 RVA: 0x005304AD File Offset: 0x0052E6AD
		protected override void NamedSecond(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x0400B8D0 RID: 47312
		private Settlement settlement;
	}
}
