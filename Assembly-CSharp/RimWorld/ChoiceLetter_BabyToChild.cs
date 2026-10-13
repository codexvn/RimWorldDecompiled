using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003363 RID: 13155
	public class ChoiceLetter_BabyToChild : ChoiceLetter
	{
		// Token: 0x1700303C RID: 12348
		// (get) Token: 0x060127E6 RID: 75750 RVA: 0x00574E7F File Offset: 0x0057307F
		public override bool CanShowInLetterStack
		{
			get
			{
				Faction faction = this.pawn.Faction;
				return faction != null && faction.IsPlayer;
			}
		}

		// Token: 0x1700303D RID: 12349
		// (get) Token: 0x060127E7 RID: 75751 RVA: 0x00574E97 File Offset: 0x00573097
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					Faction faction = this.pawn.Faction;
					if (faction != null && faction.IsPlayer)
					{
						yield return new DiaOption(this.ChoseColonistLabel)
						{
							action = new Action(this.ChoseColonist),
							disabled = (this.bornSlave != this.pawn.IsSlave),
							disabledReason = "CannotChangeChildStatusReason".Translate(this.pawn),
							resolveTree = true
						};
						yield return new DiaOption(this.ChoseSlaveLabel)
						{
							action = new Action(this.ChoseSlave),
							disabled = (this.bornSlave != this.pawn.IsSlave),
							disabledReason = "CannotChangeChildStatusReason".Translate(this.pawn),
							resolveTree = true
						};
					}
					if (this.bornSlave != this.pawn.IsSlave)
					{
						yield return base.Option_Close;
					}
					else
					{
						yield return base.Option_Postpone;
					}
				}
				else
				{
					yield return base.Option_Close;
				}
				yield return base.Option_JumpToLocationAndPostpone;
				yield break;
			}
		}

		// Token: 0x060127E8 RID: 75752 RVA: 0x00574EA8 File Offset: 0x005730A8
		public void Start()
		{
			base.StartTimeout(30000);
			this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			this.bornSlave = this.pawn.IsSlave;
			if (this.bornSlave)
			{
				this.ChoseColonistLabel = "Emancipate".Translate().CapitalizeFirst();
				this.ChoseSlaveLabel = "RemainX".Translate(this.pawn.LegalStatus).CapitalizeFirst();
				return;
			}
			this.ChoseColonistLabel = "RemainX".Translate(this.pawn.LegalStatus).CapitalizeFirst();
			this.ChoseSlaveLabel = "Enslave".Translate().CapitalizeFirst();
		}

		// Token: 0x060127E9 RID: 75753 RVA: 0x00574F78 File Offset: 0x00573178
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.bornSlave, "bornSlave", false, false);
			Scribe_Values.Look<TaggedString>(ref this.ChoseColonistLabel, "ChoseColonistLabel", default(TaggedString), false);
			Scribe_Values.Look<TaggedString>(ref this.ChoseSlaveLabel, "ChoseSlaveLabel", default(TaggedString), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			}
		}

		// Token: 0x060127EA RID: 75754 RVA: 0x00574FF7 File Offset: 0x005731F7
		private void ChoseColonist()
		{
			if (this.bornSlave)
			{
				GenGuest.SlaveRelease(this.pawn);
			}
			Find.LetterStack.RemoveLetter(this);
		}

		// Token: 0x060127EB RID: 75755 RVA: 0x00575017 File Offset: 0x00573217
		private void ChoseSlave()
		{
			if (!this.bornSlave)
			{
				this.pawn.guest.SetGuestStatus(this.pawn.Faction, GuestStatus.Slave);
			}
			Find.LetterStack.RemoveLetter(this);
		}

		// Token: 0x0400C020 RID: 49184
		private const int TimeoutTicks = 30000;

		// Token: 0x0400C021 RID: 49185
		private Pawn pawn;

		// Token: 0x0400C022 RID: 49186
		private bool bornSlave;

		// Token: 0x0400C023 RID: 49187
		private TaggedString ChoseColonistLabel;

		// Token: 0x0400C024 RID: 49188
		private TaggedString ChoseSlaveLabel;
	}
}
