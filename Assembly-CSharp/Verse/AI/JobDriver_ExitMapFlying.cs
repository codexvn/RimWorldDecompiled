using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E52 RID: 3666
	public class JobDriver_ExitMapFlying : JobDriver
	{
		// Token: 0x06005C5C RID: 23644 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x001D7FF3 File Offset: 0x001D61F3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.waitTicks < 0)
			{
				this.waitTicks = Rand.Range(0, 10);
			}
			yield return Toils_General.Wait(this.waitTicks, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				Map map = this.pawn.Map;
				IntVec3 position = this.pawn.Position;
				this.pawn.DeSpawn(DestroyMode.Vanish);
				Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.FlyerLeaving, this.pawn);
				GenSpawn.Spawn(skyfaller, position, map, WipeMode.Vanish);
				bool @bool = Rand.Bool;
				skyfaller.OverrideFlightFlippedHorizontal = new bool?(@bool);
				this.pawn.Rotation = (@bool ? Rot4.West : Rot4.East);
			});
			yield break;
		}

		// Token: 0x06005C5E RID: 23646 RVA: 0x001D8003 File Offset: 0x001D6203
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.waitTicks, "waitTicks", -1, false);
		}

		// Token: 0x040042D8 RID: 17112
		private int waitTicks = -1;
	}
}
