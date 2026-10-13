using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014FA RID: 5370
	public class JoyGiver_ViewArt : JoyGiver
	{
		// Token: 0x06008007 RID: 32775 RVA: 0x00268B9C File Offset: 0x00266D9C
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Job result;
			try
			{
				JoyGiver_ViewArt.candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate(Thing thing)
				{
					if (!base.<TryGiveJob>g__Validator|0(thing))
					{
						return false;
					}
					CompArt compArt = thing.TryGetComp<CompArt>();
					if (compArt == null)
					{
						Log.Error(string.Format("No CompArt on thing being considered for viewing: {0}", thing));
						return false;
					}
					if (!compArt.CanShowArt || !compArt.Props.canBeEnjoyedAsArt)
					{
						return false;
					}
					Room room = thing.GetRoom(RegionType.Set_All);
					if (room == null)
					{
						return false;
					}
					if (room.Role.avoidViewingArtIfUnowned)
					{
						Pawn_Ownership ownership = pawn.ownership;
						if (((ownership != null) ? ownership.OwnedRoom : null) == null || pawn.ownership.OwnedRoom != room)
						{
							return false;
						}
					}
					return true;
				}));
				Thing t;
				if (!JoyGiver_ViewArt.candidates.TryRandomElementByWeight((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty, true, -1), 0.5f), out t))
				{
					result = null;
				}
				else
				{
					result = JobMaker.MakeJob(this.def.jobDef, t);
				}
			}
			finally
			{
				JoyGiver_ViewArt.candidates.Clear();
			}
			return result;
		}

		// Token: 0x0400559B RID: 21915
		private static readonly List<Thing> candidates = new List<Thing>();
	}
}
