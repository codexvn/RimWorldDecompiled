using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F59 RID: 12121
	public class CompUseEffect_CallBossgroup : CompUseEffect
	{
		// Token: 0x17002C2F RID: 11311
		// (get) Token: 0x0601115F RID: 69983 RVA: 0x004E7FF3 File Offset: 0x004E61F3
		public CompProperties_Useable_CallBossgroup Props
		{
			get
			{
				return (CompProperties_Useable_CallBossgroup)this.props;
			}
		}

		// Token: 0x17002C30 RID: 11312
		// (get) Token: 0x06011160 RID: 69984 RVA: 0x004E8000 File Offset: 0x004E6200
		public bool ShouldSendSpawnLetter
		{
			get
			{
				return !this.Props.spawnLetterLabelKey.NullOrEmpty() && !this.Props.spawnLetterTextKey.NullOrEmpty() && MechanitorUtility.AnyMechanitorInPlayerFaction() && Find.BossgroupManager.lastBossgroupCalled <= 0;
			}
		}

		// Token: 0x06011161 RID: 69985 RVA: 0x004E804C File Offset: 0x004E624C
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.effecterDef != null)
			{
				Effecter effecter = new Effecter(this.Props.effecterDef);
				effecter.Trigger(new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
			}
			Effecter effecter2 = this.prepareEffecter;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.prepareEffecter = null;
			this.CallBossgroup();
		}

		// Token: 0x06011162 RID: 69986 RVA: 0x004E80CC File Offset: 0x004E62CC
		private void CallBossgroup()
		{
			GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
			if (component == null)
			{
				Log.Error("Trying to call bossgroup with no GameComponent_Bossgroup.");
				return;
			}
			this.Props.bossgroupDef.Worker.Resolve(this.parent.Map, component.NumTimesCalledBossgroup(this.Props.bossgroupDef));
		}

		// Token: 0x06011163 RID: 69987 RVA: 0x004E8124 File Offset: 0x004E6324
		public override TaggedString ConfirmMessage(Pawn p)
		{
			GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
			return "BossgroupWarningDialog".Translate(this.Props.bossgroupDef.boss.kindDef.Named("LEADERKIND"), this.Props.bossgroupDef.GetWaveDescription(component.NumTimesCalledBossgroup(this.Props.bossgroupDef)).Named("PAWNS"));
		}

		// Token: 0x06011164 RID: 69988 RVA: 0x004E8190 File Offset: 0x004E6390
		public override void PrepareTick()
		{
			if (this.Props.prepareEffecterDef != null && this.prepareEffecter == null)
			{
				this.prepareEffecter = this.Props.prepareEffecterDef.Spawn(this.parent.Position, this.parent.MapHeld, 1f);
			}
			Effecter effecter = this.prepareEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, TargetInfo.Invalid);
		}

		// Token: 0x06011165 RID: 69989 RVA: 0x004E8204 File Offset: 0x004E6404
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (Faction.OfMechanoids == null || Faction.OfMechanoids.deactivated)
			{
				return "MechsDisabled".Translate();
			}
			if (!MechanitorUtility.IsMechanitor(p))
			{
				return "RequiresMechanitor".Translate();
			}
			return this.Props.bossgroupDef.Worker.CanResolve(p);
		}

		// Token: 0x06011166 RID: 69990 RVA: 0x004E8264 File Offset: 0x004E6464
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Call bossgroup"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (respawningAfterLoad)
			{
				return;
			}
			if (this.ShouldSendSpawnLetter)
			{
				this.Props.SendBossgroupDetailsLetter(this.Props.spawnLetterLabelKey, this.Props.spawnLetterTextKey, this.parent.def);
			}
		}

		// Token: 0x0400B168 RID: 45416
		private Effecter prepareEffecter;
	}
}
