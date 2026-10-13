using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020025F1 RID: 9713
	public class IdeoManager : IExposable
	{
		// Token: 0x170020B2 RID: 8370
		// (get) Token: 0x0600D50B RID: 54539 RVA: 0x003EEA3B File Offset: 0x003ECC3B
		public List<Ideo> IdeosListForReading
		{
			get
			{
				return this.ideos;
			}
		}

		// Token: 0x170020B3 RID: 8371
		// (get) Token: 0x0600D50C RID: 54540 RVA: 0x003EEA44 File Offset: 0x003ECC44
		public IEnumerable<Ideo> IdeosInViewOrder
		{
			get
			{
				IEnumerable<Faction> factions = Find.FactionManager.AllFactionsInViewOrder;
				return (from i in this.IdeosListForReading
				where !i.hidden
				select i).OrderBy(delegate(Ideo ideo)
				{
					int num = 0;
					int num2 = int.MaxValue;
					foreach (Faction faction in factions)
					{
						if (faction.ideos != null && faction.ideos.IsPrimary(ideo) && !ideo.hidden)
						{
							if (faction.IsPlayer)
							{
								return int.MinValue;
							}
							num2 = Mathf.Min(num2, num);
						}
						num++;
					}
					return num2;
				});
			}
		}

		// Token: 0x170020B4 RID: 8372
		// (get) Token: 0x0600D50D RID: 54541 RVA: 0x003EEAA4 File Offset: 0x003ECCA4
		public Ideo Horaxian
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return null;
				}
				if (this.horaxianInt == null)
				{
					for (int i = 0; i < this.ideos.Count; i++)
					{
						if (this.ideos[i].solid)
						{
							this.horaxianInt = this.ideos[i];
							break;
						}
					}
				}
				return this.horaxianInt;
			}
		}

		// Token: 0x0600D50E RID: 54542 RVA: 0x003EEB05 File Offset: 0x003ECD05
		public bool Add(Ideo ideo)
		{
			if (ideo == null)
			{
				Log.Error("Tried to add a null ideoligion.");
				return false;
			}
			if (this.ideos.Contains(ideo))
			{
				Log.Error("Tried to add the same ideoligion twice.");
				return false;
			}
			this.ideos.Add(ideo);
			return true;
		}

		// Token: 0x0600D50F RID: 54543 RVA: 0x003EEB40 File Offset: 0x003ECD40
		public bool Remove(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					Log.Error(string.Concat(new string[]
					{
						"Faction ",
						faction.Name,
						" contains ideo ",
						ideo.name,
						" which was removed!"
					}));
				}
			}
			if (this.ideos.Remove(ideo))
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					Pawn_IdeoTracker ideo2 = pawn.ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_IdeoRemoved(ideo);
					}
				}
				Find.PlayLog.Notify_IdeoRemoved(ideo);
				return true;
			}
			return false;
		}

		// Token: 0x0600D510 RID: 54544 RVA: 0x003EEC48 File Offset: 0x003ECE48
		public List<Faction> GetFactionsWithIdeo(Ideo ideo, bool onlyPrimary = false, bool onlyNpcFactions = false)
		{
			this.npcWithIdeoTmp.Clear();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if ((!onlyNpcFactions || faction != Faction.OfPlayer) && faction.ideos != null)
				{
					if (onlyPrimary)
					{
						if (faction.ideos.IsPrimary(ideo))
						{
							this.npcWithIdeoTmp.Add(faction);
						}
					}
					else if (faction.ideos.Has(ideo))
					{
						this.npcWithIdeoTmp.Add(faction);
					}
				}
			}
			return this.npcWithIdeoTmp;
		}

		// Token: 0x0600D511 RID: 54545 RVA: 0x003EECF0 File Offset: 0x003ECEF0
		public List<LordJob_Ritual> GetActiveRituals(Map map)
		{
			this.activeRitualsTmp.Clear();
			if (map == null)
			{
				return this.activeRitualsTmp;
			}
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Ritual lordJob_Ritual = lords[i].LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null)
				{
					this.activeRitualsTmp.Add(lordJob_Ritual);
				}
			}
			return this.activeRitualsTmp;
		}

		// Token: 0x0600D512 RID: 54546 RVA: 0x003EED58 File Offset: 0x003ECF58
		public LordJob_Ritual GetActiveRitualOn(TargetInfo target)
		{
			if (target.HasThing && !target.Thing.Spawned)
			{
				return null;
			}
			foreach (LordJob_Ritual lordJob_Ritual in this.GetActiveRituals(target.Map))
			{
				if (lordJob_Ritual.selectedTarget == target)
				{
					return lordJob_Ritual;
				}
			}
			return null;
		}

		// Token: 0x0600D513 RID: 54547 RVA: 0x003EEDDC File Offset: 0x003ECFDC
		public void SortIdeos()
		{
			this.ideos.SortByDescending(delegate(Ideo x)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				Ideo ideo;
				if (ofPlayerSilentFail == null)
				{
					ideo = null;
				}
				else
				{
					FactionIdeosTracker factionIdeosTracker = ofPlayerSilentFail.ideos;
					ideo = ((factionIdeosTracker != null) ? factionIdeosTracker.PrimaryIdeo : null);
				}
				if (x == ideo)
				{
					return 999;
				}
				int num = 0;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int i = 0; i < allFactionsListForReading.Count; i++)
				{
					if (allFactionsListForReading[i].ideos != null && allFactionsListForReading[i].ideos.IsPrimary(x))
					{
						num++;
					}
				}
				return num;
			});
		}

		// Token: 0x0600D514 RID: 54548 RVA: 0x003EEE08 File Offset: 0x003ED008
		public void IdeoManagerTick()
		{
			for (int i = 0; i < this.ideos.Count; i++)
			{
				this.ideos[i].IdeoTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Ideo ideo = this.toRemove[j];
				this.toRemove.RemoveAt(j);
				this.Remove(ideo);
			}
		}

		// Token: 0x0600D515 RID: 54549 RVA: 0x003EEE78 File Offset: 0x003ED078
		public void RemoveUnusedStartingIdeos()
		{
			for (int i = this.ideos.Count - 1; i >= 0; i--)
			{
				if (this.CanRemoveIdeo(this.ideos[i]))
				{
					this.Remove(this.ideos[i]);
				}
			}
		}

		// Token: 0x0600D516 RID: 54550 RVA: 0x003EEEC4 File Offset: 0x003ED0C4
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x0600D517 RID: 54551 RVA: 0x003EEEC4 File Offset: 0x003ED0C4
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x0600D518 RID: 54552 RVA: 0x003EEED4 File Offset: 0x003ED0D4
		public void Notify_FactionRemoved(Faction faction)
		{
			if (faction.ideos != null)
			{
				foreach (Ideo ideo in faction.ideos.AllIdeos)
				{
					this.TryQueueIdeoRemoval(ideo);
				}
			}
		}

		// Token: 0x0600D519 RID: 54553 RVA: 0x003EEF30 File Offset: 0x003ED130
		private bool TryQueueIdeoRemoval(Ideo ideo)
		{
			if (ideo == null)
			{
				return false;
			}
			if (!this.CanRemoveIdeo(ideo))
			{
				return false;
			}
			if (!this.toRemove.Contains(ideo))
			{
				this.toRemove.Add(ideo);
			}
			return true;
		}

		// Token: 0x0600D51A RID: 54554 RVA: 0x003EEF60 File Offset: 0x003ED160
		private bool CanRemoveIdeo(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					return false;
				}
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.ideo != null && pawn.ideo.Ideo == ideo)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D51B RID: 54555 RVA: 0x003EF024 File Offset: 0x003ED224
		public void Notify_GameStarted()
		{
			foreach (Ideo ideo in this.ideos)
			{
				ideo.Notify_GameStarted();
			}
		}

		// Token: 0x0600D51C RID: 54556 RVA: 0x003EF074 File Offset: 0x003ED274
		public void ExposeData()
		{
			Scribe_Collections.Look<Ideo>(ref this.ideos, "ideos", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Ideo>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.classicMode, "classicMode", false, false);
			Scribe_Collections.Look<StyleCategoryDef>(ref this.selectedStyleCategories, "selectedStyleCategories", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToNextGauranlenSpawn, "ticksToNextGauranlenSpawn", 3600000, false);
			Scribe_Values.Look<int>(ref this.lastPsychicRitualPerformedTick, "lastPsychicRitualPerformedTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastResettledTick, "lastResettledTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligions were null after loading.");
				}
				if (this.toRemove == null)
				{
					this.toRemove = new List<Ideo>();
				}
				if (this.classicMode && this.selectedStyleCategories == null)
				{
					this.selectedStyleCategories = new List<StyleCategoryDef>();
				}
				BackCompatibility.IdeoManagerPostloadInit();
			}
		}

		// Token: 0x040090B7 RID: 37047
		private List<Faction> npcWithIdeoTmp = new List<Faction>();

		// Token: 0x040090B8 RID: 37048
		private List<LordJob_Ritual> activeRitualsTmp = new List<LordJob_Ritual>();

		// Token: 0x040090B9 RID: 37049
		[Unsaved(false)]
		private Ideo horaxianInt;

		// Token: 0x040090BA RID: 37050
		private List<Ideo> ideos = new List<Ideo>();

		// Token: 0x040090BB RID: 37051
		private List<Ideo> toRemove = new List<Ideo>();

		// Token: 0x040090BC RID: 37052
		public bool classicMode;

		// Token: 0x040090BD RID: 37053
		public List<StyleCategoryDef> selectedStyleCategories = new List<StyleCategoryDef>();

		// Token: 0x040090BE RID: 37054
		public int ticksToNextGauranlenSpawn = 3600000;

		// Token: 0x040090BF RID: 37055
		public int lastPsychicRitualPerformedTick;

		// Token: 0x040090C0 RID: 37056
		public int lastResettledTick;
	}
}
