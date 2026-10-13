using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C74 RID: 11380
	public class CompEquippableAbility : CompEquippable
	{
		// Token: 0x1700283B RID: 10299
		// (get) Token: 0x0600FF30 RID: 65328 RVA: 0x0049F5EA File Offset: 0x0049D7EA
		private CompProperties_EquippableAbility Props
		{
			get
			{
				return this.props as CompProperties_EquippableAbility;
			}
		}

		// Token: 0x1700283C RID: 10300
		// (get) Token: 0x0600FF31 RID: 65329 RVA: 0x0049F5F7 File Offset: 0x0049D7F7
		public Ability AbilityForReading
		{
			get
			{
				if (this.ability == null && this.Props.abilityDef != null)
				{
					this.ability = AbilityUtility.MakeAbility(this.Props.abilityDef, base.Holder);
				}
				return this.ability;
			}
		}

		// Token: 0x0600FF32 RID: 65330 RVA: 0x0049F630 File Offset: 0x0049D830
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (base.Holder != null)
			{
				this.AbilityForReading.pawn = base.Holder;
				this.AbilityForReading.verb.caster = base.Holder;
			}
		}

		// Token: 0x0600FF33 RID: 65331 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void UsedOnce()
		{
		}

		// Token: 0x0600FF34 RID: 65332 RVA: 0x0049F668 File Offset: 0x0049D868
		public override void Notify_Equipped(Pawn pawn)
		{
			if (this.AbilityForReading == null)
			{
				return;
			}
			this.AbilityForReading.pawn = pawn;
			this.AbilityForReading.verb.caster = pawn;
			pawn.abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600FF35 RID: 65333 RVA: 0x0049F69B File Offset: 0x0049D89B
		public override void Notify_Unequipped(Pawn pawn)
		{
			if (this.AbilityForReading == null)
			{
				return;
			}
			pawn.abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600FF36 RID: 65334 RVA: 0x0049F6B4 File Offset: 0x0049D8B4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<Ability>(ref this.ability, "ability", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Holder != null && this.AbilityForReading != null)
			{
				this.AbilityForReading.pawn = base.Holder;
				this.AbilityForReading.verb.caster = base.Holder;
			}
		}

		// Token: 0x0400A70D RID: 42765
		private Ability ability;
	}
}
