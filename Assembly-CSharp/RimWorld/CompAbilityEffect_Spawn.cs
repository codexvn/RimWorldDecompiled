using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200227F RID: 8831
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		// Token: 0x17001D67 RID: 7527
		// (get) Token: 0x0600C064 RID: 49252 RVA: 0x0037B7E7 File Offset: 0x003799E7
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		// Token: 0x0600C065 RID: 49253 RVA: 0x0037B7F4 File Offset: 0x003799F4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			if (this.Props.sendSkipSignal)
			{
				CompAbilityEffect_Teleport.SendSkipUsedSignal(target, this.parent.pawn);
			}
		}

		// Token: 0x0600C066 RID: 49254 RVA: 0x0037B850 File Offset: 0x00379A50
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map) || (!this.Props.allowOnBuildings && target.Cell.GetEdifice(this.parent.pawn.Map) != null))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
