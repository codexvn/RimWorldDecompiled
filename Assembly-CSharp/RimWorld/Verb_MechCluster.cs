using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003783 RID: 14211
	public class Verb_MechCluster : Verb_CastBase
	{
		// Token: 0x06013AFC RID: 80636 RVA: 0x005D9174 File Offset: 0x005D7374
		protected override bool TryCastShot()
		{
			if (Faction.OfMechanoids == null)
			{
				Messages.Message("MessageNoFactionForVerbMechCluster".Translate(), this.caster, MessageTypeDefOf.RejectInput, null, false);
				return false;
			}
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			MechClusterUtility.SpawnCluster(this.currentTarget.Cell, this.caster.Map, MechClusterGenerator.GenerateClusterSketch(2500f, this.caster.Map, true, true), true, false, null);
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x06013AFD RID: 80637 RVA: 0x005D8A87 File Offset: 0x005D6C87
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x0400DA08 RID: 55816
		public const float Points = 2500f;
	}
}
