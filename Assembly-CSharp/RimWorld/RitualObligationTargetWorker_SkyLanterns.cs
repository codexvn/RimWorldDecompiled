using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200270E RID: 9998
	public class RitualObligationTargetWorker_SkyLanterns : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x0600DB63 RID: 56163 RVA: 0x0040B609 File Offset: 0x00409809
		public RitualObligationTargetWorker_SkyLanterns()
		{
		}

		// Token: 0x0600DB64 RID: 56164 RVA: 0x0040B611 File Offset: 0x00409811
		public RitualObligationTargetWorker_SkyLanterns(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB65 RID: 56165 RVA: 0x0040B9F3 File Offset: 0x00409BF3
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Skylantern target"))
			{
				yield break;
			}
			List<Thing> ritualSpot = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			int num;
			for (int i = 0; i < ritualSpot.Count; i = num + 1)
			{
				yield return ritualSpot[i];
				num = i;
			}
			List<Thing> campfire = map.listerThings.ThingsOfDef(ThingDefOf.Campfire);
			for (int i = 0; i < campfire.Count; i = num + 1)
			{
				yield return campfire[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DB66 RID: 56166 RVA: 0x0040BA04 File Offset: 0x00409C04
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			if (thing.def != ThingDefOf.RitualSpot && thing.def != ThingDefOf.Campfire)
			{
				return false;
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, (float)this.def.unroofedCellSearchRadius, false))
			{
				if (intVec.InBounds(target.Map) && !intVec.Roofed(target.Map) && intVec.GetRoom(thing.Map) == room)
				{
					num++;
					if (num >= this.def.minUnroofedCells)
					{
						break;
					}
				}
			}
			if (num < this.def.minUnroofedCells)
			{
				return "RitualTargetNeedUnroofedCells".Translate(this.def.minUnroofedCells);
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.Campfire)
			{
				if (target.Cell.Roofed(target.Map))
				{
					return "RitualTargetCampfireMustBeUnroofed".Translate();
				}
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					if (compRefuelable.HasFuel)
					{
						return true;
					}
					return "RitualTargetCampfireNoFuel".Translate();
				}
			}
			return false;
		}

		// Token: 0x0600DB67 RID: 56167 RVA: 0x0040BBA8 File Offset: 0x00409DA8
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetCampfirePartyInfo".Translate();
			yield return ThingDefOf.RitualSpot.label;
			yield break;
		}

		// Token: 0x0600DB68 RID: 56168 RVA: 0x0040BBB4 File Offset: 0x00409DB4
		public override bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
		{
			if (RitualObligationTargetWorker_SkyLanterns.blocked.Contains(pawn) && assignments.PawnParticipating(pawn))
			{
				reason = "RitualTargetWoodInfoWarning".Translate(pawn.Named("PAWN"));
				return true;
			}
			reason = TaggedString.Empty;
			return false;
		}

		// Token: 0x0600DB69 RID: 56169 RVA: 0x0040BC00 File Offset: 0x00409E00
		public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
		{
			Dictionary<Thing, int> dictionary = new Dictionary<Thing, int>();
			List<Thing> list = target.Map.listerThings.ThingsOfDef(ThingDefOf.WoodLog);
			bool flag = true;
			int num = 0;
			RitualObligationTargetWorker_SkyLanterns.blocked.Clear();
			List<Pawn> participants = assignments.Participants;
			foreach (Pawn pawn in participants)
			{
				int num2 = Math.Max(this.def.woodPerParticipant - pawn.inventory.Count(ThingDefOf.WoodLog), 0);
				bool flag2 = false;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
					{
						int num3 = thing.stackCount;
						int num4;
						if (dictionary.TryGetValue(thing, out num4))
						{
							num3 = Math.Max(num3 - num4, 0);
						}
						if (num3 >= num2)
						{
							if (dictionary.ContainsKey(thing))
							{
								dictionary[thing] += num2;
							}
							else
							{
								dictionary[thing] = num2;
							}
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					RitualObligationTargetWorker_SkyLanterns.blocked.Add(pawn);
					flag = false;
					num++;
				}
			}
			if (!flag)
			{
				TaggedString taggedString = "RitualTargetWoodInfo".Translate(this.def.woodPerParticipant, this.def.woodPerParticipant * participants.Count<Pawn>());
				if (num == 1)
				{
					taggedString += string.Format(" {0}", "RitualTargetWoodInfoAppend".Translate(num));
				}
				else
				{
					taggedString += string.Format(" {0}", "RitualTargetWoodInfoAppendMultiple".Translate(num));
				}
				yield return taggedString;
			}
			yield break;
		}

		// Token: 0x04009461 RID: 37985
		private static readonly HashSet<Pawn> blocked = new HashSet<Pawn>();
	}
}
