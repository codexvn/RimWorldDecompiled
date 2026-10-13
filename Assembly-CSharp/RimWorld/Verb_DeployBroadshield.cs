using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200377A RID: 14202
	public class Verb_DeployBroadshield : Verb
	{
		// Token: 0x06013AD4 RID: 80596 RVA: 0x005D8A91 File Offset: 0x005D6C91
		protected override bool TryCastShot()
		{
			return Verb_DeployBroadshield.Deploy(base.ReloadableCompSource);
		}

		// Token: 0x06013AD5 RID: 80597 RVA: 0x005D8AA0 File Offset: 0x005D6CA0
		public static bool Deploy(CompApparelReloadable comp)
		{
			if (!ModLister.CheckRoyalty("Projectile interceptors"))
			{
				return false;
			}
			string text;
			if (comp == null || !comp.CanBeUsed(out text))
			{
				return false;
			}
			Pawn wearer = comp.Wearer;
			Map map = wearer.Map;
			int num = GenRadial.NumCellsInRadius(4f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = wearer.Position + GenRadial.RadialPattern[i];
				if (intVec.IsValid && intVec.InBounds(map))
				{
					Verb_DeployBroadshield.SpawnEffect(GenSpawn.Spawn(ThingDefOf.BroadshieldProjector, intVec, map, WipeMode.Vanish));
					comp.UsedOnce();
					return true;
				}
			}
			Messages.Message("AbilityNotEnoughFreeSpace".Translate(), wearer, MessageTypeDefOf.RejectInput, false);
			return false;
		}

		// Token: 0x06013AD6 RID: 80598 RVA: 0x005D8B5B File Offset: 0x005D6D5B
		private static void SpawnEffect(Thing projector)
		{
			FleckMaker.Static(projector.TrueCenter(), projector.Map, FleckDefOf.BroadshieldActivation, 1f);
			SoundDefOf.Broadshield_Startup.PlayOneShot(new TargetInfo(projector.Position, projector.Map, false));
		}
	}
}
