using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002643 RID: 9795
	public class PreceptComp_UnwillingToDo : PreceptComp
	{
		// Token: 0x170020D2 RID: 8402
		// (get) Token: 0x0600D699 RID: 54937 RVA: 0x003FAB13 File Offset: 0x003F8D13
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				if (this.nullifyingTraits != null)
				{
					int num;
					for (int i = 0; i < this.nullifyingTraits.Count; i = num + 1)
					{
						yield return this.nullifyingTraits[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600D69A RID: 54938 RVA: 0x003FAB24 File Offset: 0x003F8D24
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			if (this.eventDef != null && ev.def != this.eventDef)
			{
				return true;
			}
			Pawn pawn;
			if (!ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn))
			{
				return false;
			}
			if (!this.preceptDef.enabledForNPCFactions && pawn != null && !pawn.Faction.IsPlayer)
			{
				return true;
			}
			if (this.nullifyingTraits != null && ((pawn != null) ? pawn.story : null) != null)
			{
				for (int i = 0; i < this.nullifyingTraits.Count; i++)
				{
					if (this.nullifyingTraits[i].HasTrait(pawn))
					{
						return true;
					}
				}
			}
			if (this.nullifyingHediffs != null)
			{
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_HealthTracker health = pawn.health;
					flag = (((health != null) ? health.hediffSet : null) != null);
				}
				if (flag)
				{
					for (int j = 0; j < this.nullifyingHediffs.Count; j++)
					{
						if (!pawn.health.hediffSet.HasHediff(this.nullifyingHediffs[j], false))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D69B RID: 54939 RVA: 0x003FAC1B File Offset: 0x003F8E1B
		public virtual string GetProhibitionText()
		{
			return this.description ?? this.eventDef.LabelCap;
		}

		// Token: 0x0600D69C RID: 54940 RVA: 0x003FAC37 File Offset: 0x003F8E37
		public override IEnumerable<string> GetDescriptions()
		{
			yield return "UnwillingToDoIdeoAction".Translate() + ": " + this.eventDef.LabelCap;
			yield break;
		}

		// Token: 0x040091DC RID: 37340
		public HistoryEventDef eventDef;

		// Token: 0x040091DD RID: 37341
		public List<TraitRequirement> nullifyingTraits;

		// Token: 0x040091DE RID: 37342
		public List<HediffDef> nullifyingHediffs;
	}
}
