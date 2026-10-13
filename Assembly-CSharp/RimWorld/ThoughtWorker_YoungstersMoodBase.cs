using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DA RID: 6362
	public abstract class ThoughtWorker_YoungstersMoodBase : ThoughtWorker
	{
		// Token: 0x060090EC RID: 37100
		protected abstract FloatRange MoodRange();

		// Token: 0x060090ED RID: 37101 RVA: 0x002AA834 File Offset: 0x002A8A34
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x060090EE RID: 37102 RVA: 0x002AA874 File Offset: 0x002A8A74
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (!p.IsColonist || p.Suspended || !p.DevelopmentalStage.Adult())
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenWithMoodInColony(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x060090EF RID: 37103 RVA: 0x002AA8C8 File Offset: 0x002A8AC8
		private int ChildrenWithMoodInColony(Pawn pawn)
		{
			int num = 0;
			ThoughtWorker_YoungstersMoodBase.tmpPawns.Clear();
			if (pawn.Spawned)
			{
				ThoughtWorker_YoungstersMoodBase.tmpPawns.AddRange(pawn.Map.mapPawns.FreeColonistsSpawned);
			}
			else
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null)
				{
					ThoughtWorker_YoungstersMoodBase.tmpPawns.AddRange(from p in caravan.PawnsListForReading
					where p.IsColonist
					select p);
				}
			}
			for (int i = 0; i < ThoughtWorker_YoungstersMoodBase.tmpPawns.Count; i++)
			{
				if (ThoughtWorker_YoungstersMoodBase.tmpPawns[i].RaceProps.Humanlike && !ThoughtWorker_YoungstersMoodBase.tmpPawns[i].DevelopmentalStage.Adult() && !PawnRelationDefOf.Parent.Worker.InRelation(ThoughtWorker_YoungstersMoodBase.tmpPawns[i], pawn) && ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(ThoughtWorker_YoungstersMoodBase.tmpPawns[i], this.MoodRange()))
				{
					num++;
				}
			}
			ThoughtWorker_YoungstersMoodBase.tmpPawns.Clear();
			return num;
		}

		// Token: 0x060090F0 RID: 37104 RVA: 0x002AA9CA File Offset: 0x002A8BCA
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenWithMoodInColony(p));
		}

		// Token: 0x04005C64 RID: 23652
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
