using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020027CD RID: 10189
	public class ActionOnTick_ReleaseLantern : ActionOnTick
	{
		// Token: 0x0600DF0F RID: 57103 RVA: 0x004190A0 File Offset: 0x004172A0
		public override void Apply(LordJob_Ritual ritual)
		{
			if (!ritual.lord.ownedPawns.Contains(this.pawn))
			{
				return;
			}
			if (this.pawn.carryTracker.CarriedCount(ThingDefOf.WoodLog) >= this.woodCost)
			{
				this.pawn.carryTracker.DestroyCarriedThing();
				Thing newThing = ThingMaker.MakeThing(ThingDefOf.SkyLantern, null);
				IntVec3 intVec = this.pawn.Position + new IntVec3(-1, 0, 1);
				if (intVec.InBounds(this.pawn.Map))
				{
					GenSpawn.Spawn(newThing, intVec, this.pawn.Map, WipeMode.Vanish);
					ritual.AddTagForPawn(this.pawn, "LaunchedSkyLantern");
				}
				SoundDefOf.Interact_ReleaseSkylantern.PlayOneShot(new TargetInfo(intVec, this.pawn.Map, false));
			}
		}

		// Token: 0x0600DF10 RID: 57104 RVA: 0x00419174 File Offset: 0x00417374
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.woodCost, "woodCost", 0, false);
		}

		// Token: 0x0400962F RID: 38447
		public Pawn pawn;

		// Token: 0x04009630 RID: 38448
		public int woodCost = 4;
	}
}
