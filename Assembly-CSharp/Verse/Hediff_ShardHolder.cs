using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200076D RID: 1901
	public class Hediff_ShardHolder : HediffWithComps
	{
		// Token: 0x06002EAC RID: 11948 RVA: 0x000F0287 File Offset: 0x000EE487
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Shard holder"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000F02B0 File Offset: 0x000EE4B0
		public override void Notify_PawnKilled()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			Thing thing;
			if (!GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), this.pawn.PositionHeld, this.pawn.MapHeld, ThingPlaceMode.Near, out thing, null, null, true))
			{
				return;
			}
			thing.SetForbidden(!thing.MapHeld.areaManager.Home[thing.PositionHeld], true);
			string text = this.pawn.LabelShort;
			if (this.pawn.IsMutant)
			{
				text = Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.mutant.Def.label, false, false);
			}
			else if (this.pawn.Name == null)
			{
				text = Find.ActiveLanguageWorker.WithDefiniteArticle(text, false, false);
			}
			Messages.Message("MessageShardDropped".Translate(text).CapitalizeFirst(), thing, MessageTypeDefOf.NeutralEvent, true);
		}
	}
}
