using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003361 RID: 13153
	public class ChoiceLetter_BabyBirth : ChoiceLetter
	{
		// Token: 0x17003037 RID: 12343
		// (get) Token: 0x060127D7 RID: 75735 RVA: 0x005749C4 File Offset: 0x00572BC4
		public override bool CanShowInLetterStack
		{
			get
			{
				Pawn pawn = this.pawn;
				bool flag;
				if (pawn == null)
				{
					flag = false;
				}
				else
				{
					Faction faction = pawn.Faction;
					bool? flag2 = (faction != null) ? new bool?(faction.IsPlayer) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					Pawn pawn2 = this.pawn;
					if (pawn2 != null && pawn2.babyNamingDeadline >= 0)
					{
						return base.CanShowInLetterStack;
					}
				}
				return false;
			}
		}

		// Token: 0x17003038 RID: 12344
		// (get) Token: 0x060127D8 RID: 75736 RVA: 0x00574A35 File Offset: 0x00572C35
		public override bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				Pawn pawn = this.pawn;
				return pawn != null && pawn.babyNamingDeadline >= 0 && base.ShouldAutomaticallyOpenLetter;
			}
		}

		// Token: 0x17003039 RID: 12345
		// (get) Token: 0x060127D9 RID: 75737 RVA: 0x00574A59 File Offset: 0x00572C59
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					Faction faction = this.pawn.Faction;
					if (faction != null && faction.IsPlayer)
					{
						yield return new DiaOption("NameBaby".Translate().CapitalizeFirst())
						{
							action = new Action(this.Rename),
							disabled = (this.pawn.babyNamingDeadline < Find.TickManager.TicksGame),
							disabledReason = "BabyAlreadyNamed".Translate(this.pawn),
							resolveTree = true
						};
					}
				}
				if (base.ArchivedOnly)
				{
					yield return base.Option_JumpToLocation;
				}
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
				}
				else if (this.LastTickBeforeTimeout)
				{
					if (this.pawn.babyNamingDeadline < 0 || this.pawn.Dead)
					{
						yield return base.Option_Close;
					}
				}
				else if (this.pawn.babyNamingDeadline < Find.TickManager.TicksGame)
				{
					yield return base.Option_JumpToLocationAndPostpone;
					yield return base.Option_Close;
				}
				else
				{
					yield return base.Option_JumpToLocationAndPostpone;
					yield return base.Option_Postpone;
				}
				yield break;
			}
		}

		// Token: 0x060127DA RID: 75738 RVA: 0x00574A6C File Offset: 0x00572C6C
		public void Start()
		{
			this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			if (this.pawn.babyNamingDeadline >= Find.TickManager.TicksGame)
			{
				base.StartTimeout(this.pawn.babyNamingDeadline - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x060127DB RID: 75739 RVA: 0x00574ACC File Offset: 0x00572CCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			if (Scribe.mode != LoadSaveMode.Saving && this.pawn == null)
			{
				this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			}
		}

		// Token: 0x060127DC RID: 75740 RVA: 0x00574B20 File Offset: 0x00572D20
		private void Rename()
		{
			string initialFirstNameOverride = null;
			NameTriple nameTriple = this.pawn.Name as NameTriple;
			if (nameTriple != null && nameTriple.First == "Baby".Translate().CapitalizeFirst())
			{
				Rand.PushState();
				Name name;
				try
				{
					Rand.Seed = this.pawn.thingIDNumber;
					Pawn pawn = this.pawn;
					NameStyle style = NameStyle.Full;
					string forcedLastName = null;
					bool forceNoNick = false;
					Pawn_GeneTracker genes = this.pawn.genes;
					name = PawnBioAndNameGenerator.GeneratePawnName(pawn, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
				}
				finally
				{
					Rand.PopState();
				}
				NameTriple nameTriple2 = name as NameTriple;
				initialFirstNameOverride = ((nameTriple2 != null) ? nameTriple2.First : ((NameSingle)name).Name);
			}
			Find.WindowStack.Add(this.pawn.NamePawnDialog(initialFirstNameOverride));
		}

		// Token: 0x0400C01B RID: 49179
		private Pawn pawn;
	}
}
