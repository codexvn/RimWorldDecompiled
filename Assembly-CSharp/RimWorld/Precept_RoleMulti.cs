using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002812 RID: 10258
	public class Precept_RoleMulti : Precept_Role
	{
		// Token: 0x0600E065 RID: 57445 RVA: 0x0041E583 File Offset: 0x0041C783
		public override IEnumerable<Pawn> ChosenPawns()
		{
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				yield return ideoRoleInstance.pawn;
			}
			List<IdeoRoleInstance>.Enumerator enumerator = default(List<IdeoRoleInstance>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600E066 RID: 57446 RVA: 0x00002C42 File Offset: 0x00000E42
		public override Pawn ChosenPawnSingle()
		{
			return null;
		}

		// Token: 0x0600E067 RID: 57447 RVA: 0x0041E593 File Offset: 0x0041C793
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, generatingFor);
			this.active = true;
		}

		// Token: 0x0600E068 RID: 57448 RVA: 0x0041E5A4 File Offset: 0x0041C7A4
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (!this.IsAssigned(p))
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawnsCache.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
				if (ideoRoleInstance != null)
				{
					this.chosenPawnsCache.Remove(ideoRoleInstance);
					this.chosenPawns.Add(ideoRoleInstance);
				}
				else
				{
					this.chosenPawns.Add(new IdeoRoleInstance(this)
					{
						pawn = p
					});
					this.FillOrUpdateAbilities();
				}
				base.Notify_PawnAssigned(p);
			}
		}

		// Token: 0x0600E069 RID: 57449 RVA: 0x0041E634 File Offset: 0x0041C834
		public override void FillOrUpdateAbilities()
		{
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(ideoRoleInstance.pawn, ideoRoleInstance.abilities);
			}
		}

		// Token: 0x0600E06A RID: 57450 RVA: 0x0041E698 File Offset: 0x0041C898
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return this.chosenPawns[i].abilities;
				}
			}
			return null;
		}

		// Token: 0x0600E06B RID: 57451 RVA: 0x0041E6E4 File Offset: 0x0041C8E4
		public override bool IsAssigned(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E06C RID: 57452 RVA: 0x0041E720 File Offset: 0x0041C920
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			if (!this.IsAssigned(p))
			{
				return;
			}
			IdeoRoleInstance ideoRoleInstance = this.chosenPawns.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
			if (ideoRoleInstance != null)
			{
				this.chosenPawns.Remove(ideoRoleInstance);
				this.chosenPawnsCache.Add(ideoRoleInstance);
				Pawn_NeedsTracker needs = p.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
							}
						}
					}
				}
				base.Notify_PawnUnassigned(p);
			}
		}

		// Token: 0x0600E06D RID: 57453 RVA: 0x0041E7CC File Offset: 0x0041C9CC
		public override void RecacheActivity()
		{
			this.pawnsToRemove.Clear();
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				if (ideoRoleInstance.pawn == null || !base.ValidatePawn(ideoRoleInstance.pawn))
				{
					this.pawnsToRemove.Add(ideoRoleInstance);
				}
			}
			foreach (IdeoRoleInstance ideoRoleInstance2 in this.pawnsToRemove)
			{
				this.Unassign(ideoRoleInstance2.pawn, false);
				if (this.chosenPawns.Contains(ideoRoleInstance2))
				{
					this.chosenPawns.Remove(ideoRoleInstance2);
				}
			}
		}

		// Token: 0x0600E06E RID: 57454 RVA: 0x0041E8A8 File Offset: 0x0041CAA8
		public override void ExposeData()
		{
			base.ExposeData();
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawns, "chosenPawns", LookMode.Deep, new object[]
				{
					this
				});
				Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawnsCache, "chosenPawnsCache", LookMode.Deep, new object[]
				{
					this
				});
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.chosenPawns.RemoveAll((IdeoRoleInstance c) => c.pawn == null || !base.ValidatePawn(c.pawn));
				this.chosenPawnsCache.RemoveAll((IdeoRoleInstance c) => c.pawn == null);
				foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
				{
					ideoRoleInstance.sourceRole = this;
				}
			}
		}

		// Token: 0x0600E06F RID: 57455 RVA: 0x0041E988 File Offset: 0x0041CB88
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_RoleMulti precept_RoleMulti = (Precept_RoleMulti)precept;
			precept_RoleMulti.chosenPawns.Clear();
			precept_RoleMulti.chosenPawns.AddRange(this.chosenPawns);
			precept_RoleMulti.chosenPawnsCache.Clear();
			precept_RoleMulti.chosenPawnsCache.AddRange(this.chosenPawnsCache);
		}

		// Token: 0x040096EA RID: 38634
		public List<IdeoRoleInstance> chosenPawns = new List<IdeoRoleInstance>();

		// Token: 0x040096EB RID: 38635
		private List<IdeoRoleInstance> chosenPawnsCache = new List<IdeoRoleInstance>();

		// Token: 0x040096EC RID: 38636
		private List<IdeoRoleInstance> pawnsToRemove = new List<IdeoRoleInstance>();
	}
}
