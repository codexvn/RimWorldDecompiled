using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002639 RID: 9785
	public class PreceptComp_DevelopmentPoints : PreceptComp
	{
		// Token: 0x170020C6 RID: 8390
		// (get) Token: 0x0600D66A RID: 54890 RVA: 0x003F9F68 File Offset: 0x003F8168
		private string Label
		{
			get
			{
				if (this.eventLabel.NullOrEmpty())
				{
					return this.eventDef.label;
				}
				return this.eventLabel.Formatted(Array.Empty<NamedArgument>()).Resolve();
			}
		}

		// Token: 0x170020C7 RID: 8391
		// (get) Token: 0x0600D66B RID: 54891 RVA: 0x003F9FA8 File Offset: 0x003F81A8
		private string LabelCap
		{
			get
			{
				return (!this.eventLabel.NullOrEmpty()) ? this.eventLabel.Formatted(Array.Empty<NamedArgument>()).CapitalizeFirst() : this.eventDef.LabelCap;
			}
		}

		// Token: 0x0600D66C RID: 54892 RVA: 0x003F9FEC File Offset: 0x003F81EC
		public override void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
		{
			if (ev.def != this.eventDef || precept.ideo != Faction.OfPlayer.ideos.FluidIdeo || !precept.ideo.development.CanBeDevelopedNow)
			{
				return;
			}
			Ideo ideo;
			ev.args.TryGetArg<Ideo>(HistoryEventArgsNames.Ideo, out ideo);
			if (ideo == null || ideo == precept.ideo)
			{
				int value = precept.ideo.development.Points;
				if (precept.ideo.development.TryAddDevelopmentPoints(this.points))
				{
					Pawn t;
					ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out t);
					Quest quest;
					ev.args.TryGetArg<Quest>(HistoryEventArgsNames.Quest, out quest);
					Messages.Message("MessageDevelopmentPointsEarned".Translate(value, precept.ideo.development.Points, this.Label), t, MessageTypeDefOf.PositiveEvent, quest, true);
				}
			}
		}

		// Token: 0x0600D66D RID: 54893 RVA: 0x003FA0EC File Offset: 0x003F82EC
		public override IEnumerable<string> GetDescriptions()
		{
			yield return this.LabelCap + ": " + this.points.ToStringWithSign();
			yield break;
		}

		// Token: 0x040091BE RID: 37310
		public HistoryEventDef eventDef;

		// Token: 0x040091BF RID: 37311
		public int points;

		// Token: 0x040091C0 RID: 37312
		[MustTranslate]
		public string eventLabel;
	}
}
