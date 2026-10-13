using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E33 RID: 15923
	[Obsolete]
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x17003658 RID: 13912
		// (get) Token: 0x06015C1B RID: 89115 RVA: 0x00680F0B File Offset: 0x0067F10B
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x17003659 RID: 13913
		// (get) Token: 0x06015C1C RID: 89116 RVA: 0x00680F19 File Offset: 0x0067F119
		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					return "";
				}
				return TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
			}
		}

		// Token: 0x06015C1D RID: 89117 RVA: 0x00680F34 File Offset: 0x0067F134
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06015C1E RID: 89118 RVA: 0x00680F4E File Offset: 0x0067F14E
		public void ResetForceExitAndRemoveMapCountdown()
		{
			this.ticksLeftToForceExitAndRemoveMap = -1;
		}

		// Token: 0x06015C1F RID: 89119 RVA: 0x00680F57 File Offset: 0x0067F157
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06015C20 RID: 89120 RVA: 0x00680F60 File Offset: 0x0067F160
		public override string CompInspectStringExtra()
		{
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				return "ForceExitAndRemoveMapCountdown".Translate(this.ForceExitAndRemoveMapCountdownTimeLeftString) + ".";
			}
			return null;
		}

		// Token: 0x06015C21 RID: 89121 RVA: 0x00680F90 File Offset: 0x0067F190
		public override void CompTickInterval(int delta)
		{
			MapParent mapParent = (MapParent)this.parent;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (mapParent.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap <= 0)
					{
						TimedForcedExit.ForceReform(mapParent);
						return;
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		// Token: 0x06015C22 RID: 89122 RVA: 0x00680CFD File Offset: 0x0067EEFD
		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x06015C23 RID: 89123 RVA: 0x00680FE0 File Offset: 0x0067F1E0
		public static void ForceReform(MapParent mapParent)
		{
			if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
			{
				Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
				Current.Game.CurrentMap = mapParent.Map;
				Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
				{
					if (mapParent.HasMap)
					{
						mapParent.Destroy();
					}
				}, true, null);
				Find.WindowStack.Add(window);
				return;
			}
			TimedForcedExit.tmpPawns.Clear();
			TimedForcedExit.tmpPawns.AddRange(from x in mapParent.Map.mapPawns.AllPawns
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			if (TimedForcedExit.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				CaravanExitMapUtility.ExitMapAndCreateCaravan(TimedForcedExit.tmpPawns, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, PlanetTile.Invalid, true);
			}
			TimedForcedExit.tmpPawns.Clear();
			mapParent.Destroy();
		}

		// Token: 0x0400F15D RID: 61789
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x0400F15E RID: 61790
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
