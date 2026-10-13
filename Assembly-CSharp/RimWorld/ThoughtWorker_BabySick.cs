using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EA RID: 6378
	public class ThoughtWorker_BabySick : ThoughtWorker
	{
		// Token: 0x0600911E RID: 37150 RVA: 0x002AB024 File Offset: 0x002A9224
		public override string PostProcessLabel(Pawn p, string label)
		{
			IEnumerable<Pawn> source = p.relations.Children.Where(new Func<Pawn, bool>(this.IsSickBaby));
			if (source.Count<Pawn>() > 1)
			{
				return base.PostProcessLabel(p, label) + " x" + Mathf.RoundToInt(this.MoodMultiplier(p)).ToString();
			}
			Pawn arg = source.First<Pawn>();
			return label.Formatted(p.Named("PAWN"), arg.Named("BABY"));
		}

		// Token: 0x0600911F RID: 37151 RVA: 0x002AB0A8 File Offset: 0x002A92A8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			int num = this.BabiesSickCount(p);
			if (num > 1)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num == 1)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06009120 RID: 37152 RVA: 0x002AB0E8 File Offset: 0x002A92E8
		private int BabiesSickCount(Pawn pawn)
		{
			int num = 0;
			foreach (Pawn pawn2 in pawn.relations.Children)
			{
				if (!pawn2.Dead && this.IsSickBaby(pawn2))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06009121 RID: 37153 RVA: 0x002AB14C File Offset: 0x002A934C
		private bool IsSickBaby(Pawn pawn)
		{
			return pawn.DevelopmentalStage.Baby() && pawn.health.hediffSet.HasHediff(HediffDefOf.InfantIllness, false);
		}

		// Token: 0x06009122 RID: 37154 RVA: 0x002AB173 File Offset: 0x002A9373
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.BabiesSickCount(p));
		}

		// Token: 0x04005C71 RID: 23665
		private const int SingleBabySickStage = 0;

		// Token: 0x04005C72 RID: 23666
		private const int SeveralBabiesSickStage = 1;
	}
}
