using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200251F RID: 9503
	public class Pawn_RelationsTracker : IExposable
	{
		// Token: 0x17001FE2 RID: 8162
		// (get) Token: 0x0600CFA1 RID: 53153 RVA: 0x003CF733 File Offset: 0x003CD933
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		// Token: 0x17001FE3 RID: 8163
		// (get) Token: 0x0600CFA2 RID: 53154 RVA: 0x003CF73B File Offset: 0x003CD93B
		public List<VirtualPawnRelation> VirtualRelations
		{
			get
			{
				return this.virtualRelations;
			}
		}

		// Token: 0x17001FE4 RID: 8164
		// (get) Token: 0x0600CFA3 RID: 53155 RVA: 0x003CF743 File Offset: 0x003CD943
		public IEnumerable<Pawn> Children
		{
			get
			{
				foreach (Pawn pawn in this.pawnsWithDirectRelationsWithMe)
				{
					List<DirectPawnRelation> list = pawn.relations.directRelations;
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].otherPawn == this.pawn && list[i].def == PawnRelationDefOf.Parent)
						{
							yield return pawn;
							break;
						}
					}
				}
				HashSet<Pawn>.Enumerator enumerator = default(HashSet<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17001FE5 RID: 8165
		// (get) Token: 0x0600CFA4 RID: 53156 RVA: 0x003CF753 File Offset: 0x003CD953
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		// Token: 0x17001FE6 RID: 8166
		// (get) Token: 0x0600CFA5 RID: 53157 RVA: 0x003CF760 File Offset: 0x003CD960
		public bool IsTryRomanceOnCooldown
		{
			get
			{
				return this.romanceEnableTick > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001FE7 RID: 8167
		// (get) Token: 0x0600CFA6 RID: 53158 RVA: 0x003CF774 File Offset: 0x003CD974
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.virtualRelations.Any<VirtualPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		// Token: 0x17001FE8 RID: 8168
		// (get) Token: 0x0600CFA7 RID: 53159 RVA: 0x003CF7A0 File Offset: 0x003CD9A0
		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						foreach (Pawn item in this.FamilyByBlood_Internal)
						{
							this.cachedFamilyByBlood.Add(item);
						}
						this.familyByBloodIsCached = true;
					}
					return this.cachedFamilyByBlood;
				}
				return this.FamilyByBlood_Internal;
			}
		}

		// Token: 0x17001FE9 RID: 8169
		// (get) Token: 0x0600CFA8 RID: 53160 RVA: 0x003CF824 File Offset: 0x003CDA24
		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> familyStack = null;
				List<Pawn> familyChildrenStack = null;
				HashSet<Pawn> familyVisited = null;
				try
				{
					familyStack = SimplePool<List<Pawn>>.Get();
					familyChildrenStack = SimplePool<List<Pawn>>.Get();
					familyVisited = SimplePool<HashSet<Pawn>>.Get();
					familyStack.Add(this.pawn);
					familyVisited.Add(this.pawn);
					while (familyStack.Any<Pawn>())
					{
						Pawn p = familyStack[familyStack.Count - 1];
						familyStack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						Pawn father = p.GetFather();
						if (father != null && !familyVisited.Contains(father))
						{
							familyStack.Add(father);
							familyVisited.Add(father);
						}
						Pawn mother = p.GetMother();
						if (mother != null && !familyVisited.Contains(mother))
						{
							familyStack.Add(mother);
							familyVisited.Add(mother);
						}
						familyChildrenStack.Clear();
						familyChildrenStack.Add(p);
						while (familyChildrenStack.Any<Pawn>())
						{
							Pawn child = familyChildrenStack[familyChildrenStack.Count - 1];
							familyChildrenStack.RemoveLast<Pawn>();
							if (child != p && child != this.pawn)
							{
								yield return child;
							}
							foreach (Pawn item in child.relations.Children)
							{
								if (!familyVisited.Contains(item))
								{
									familyChildrenStack.Add(item);
									familyVisited.Add(item);
								}
							}
							child = null;
						}
						p = null;
					}
				}
				finally
				{
					familyStack.Clear();
					SimplePool<List<Pawn>>.Return(familyStack);
					familyChildrenStack.Clear();
					SimplePool<List<Pawn>>.Return(familyChildrenStack);
					familyVisited.Clear();
					SimplePool<HashSet<Pawn>>.Return(familyVisited);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001FEA RID: 8170
		// (get) Token: 0x0600CFA9 RID: 53161 RVA: 0x003CF834 File Offset: 0x003CDA34
		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> stack = null;
				HashSet<Pawn> visited = null;
				try
				{
					stack = SimplePool<List<Pawn>>.Get();
					visited = SimplePool<HashSet<Pawn>>.Get();
					stack.Add(this.pawn);
					visited.Add(this.pawn);
					while (stack.Any<Pawn>())
					{
						Pawn p = stack[stack.Count - 1];
						stack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						for (int i = 0; i < p.relations.directRelations.Count; i++)
						{
							Pawn otherPawn = p.relations.directRelations[i].otherPawn;
							if (otherPawn != null && !visited.Contains(otherPawn))
							{
								stack.Add(otherPawn);
								visited.Add(otherPawn);
							}
						}
						foreach (VirtualPawnRelation virtualPawnRelation in p.relations.VirtualRelations)
						{
							foreach (Pawn pawn in virtualPawnRelation.record.References)
							{
								if (pawn != this.pawn && pawn != null && !visited.Contains(pawn))
								{
									stack.Add(pawn);
									visited.Add(pawn);
								}
							}
						}
						foreach (Pawn item in p.relations.pawnsWithDirectRelationsWithMe)
						{
							if (!visited.Contains(item))
							{
								stack.Add(item);
								visited.Add(item);
							}
						}
						p = null;
					}
				}
				finally
				{
					stack.Clear();
					SimplePool<List<Pawn>>.Return(stack);
					visited.Clear();
					SimplePool<HashSet<Pawn>>.Return(visited);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001FEB RID: 8171
		// (get) Token: 0x0600CFAA RID: 53162 RVA: 0x003CF844 File Offset: 0x003CDA44
		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				this.canCacheFamilyByBlood = true;
				this.familyByBloodIsCached = false;
				this.cachedFamilyByBlood.Clear();
				try
				{
					foreach (Pawn pawn in this.PotentiallyRelatedPawns)
					{
						if ((this.familyByBloodIsCached && this.cachedFamilyByBlood.Contains(pawn)) || this.pawn.GetRelations(pawn).Any<PawnRelationDef>())
						{
							yield return pawn;
						}
					}
					IEnumerator<Pawn> enumerator = null;
				}
				finally
				{
					this.canCacheFamilyByBlood = false;
					this.familyByBloodIsCached = false;
					this.cachedFamilyByBlood.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600CFAB RID: 53163 RVA: 0x003CF854 File Offset: 0x003CDA54
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CFAC RID: 53164 RVA: 0x003CF8CC File Offset: 0x003CDACC
		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<VirtualPawnRelation>(ref this.virtualRelations, "virtualRelations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.virtualRelations == null)
				{
					this.virtualRelations = new List<VirtualPawnRelation>();
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						string[] array = new string[5];
						array[0] = "Pawn ";
						int num = 1;
						Pawn pawn = this.pawn;
						array[num] = ((pawn != null) ? pawn.ToString() : null);
						array[2] = " has relation \"";
						array[3] = this.directRelations[i].def.defName;
						array[4] = "\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships).";
						Log.Warning(string.Concat(array));
					}
				}
				this.directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					this.directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				}
				if (this.pregnancyApproaches == null)
				{
					this.pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();
				}
				this.pregnancyApproaches.RemoveAll((KeyValuePair<Pawn, PregnancyApproach> x) => x.Key == null);
				for (int k = this.virtualRelations.Count - 1; k >= 0; k--)
				{
					if (this.virtualRelations[k].record == null)
					{
						this.virtualRelations.RemoveAt(k);
					}
				}
			}
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", true, false);
			Scribe_Values.Look<bool>(ref this.canGetRescuedThought, "canGetRescuedThought", true, false);
			Scribe_Values.Look<MarriageNameChange>(ref this.nextMarriageNameChange, "nextMarriageNameChange", MarriageNameChange.NoChange, false);
			Scribe_References.Look<Pawn>(ref this.relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest", false);
			Scribe_Values.Look<bool>(ref this.hidePawnRelations, "hidePawnRelations", false, false);
			Scribe_Collections.Look<Pawn, PregnancyApproach>(ref this.pregnancyApproaches, "pregnancyApproaches", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpPregnancyApproaches, false, false, false);
			Scribe_Values.Look<int>(ref this.romanceEnableTick, "romanceEnableTick", 0, false);
		}

		// Token: 0x0600CFAD RID: 53165 RVA: 0x003CFB1C File Offset: 0x003CDD1C
		public PregnancyApproach GetPregnancyApproachForPartner(Pawn partner)
		{
			if (!this.pregnancyApproaches.ContainsKey(partner))
			{
				PregnancyApproach value;
				if (partner.relations.pregnancyApproaches.TryGetValue(this.pawn, out value))
				{
					this.pregnancyApproaches.Add(partner, value);
				}
				else
				{
					this.SetPregnancyApproach(partner, PregnancyApproach.Normal);
				}
			}
			return this.pregnancyApproaches[partner];
		}

		// Token: 0x0600CFAE RID: 53166 RVA: 0x003CFB74 File Offset: 0x003CDD74
		public void SetPregnancyApproach(Pawn partner, PregnancyApproach mode)
		{
			this.pregnancyApproaches[partner] = mode;
			partner.relations.pregnancyApproaches[this.pawn] = mode;
		}

		// Token: 0x0600CFAF RID: 53167 RVA: 0x003CFB9A File Offset: 0x003CDD9A
		public void RelationsTrackerTickInterval(int delta)
		{
			if (this.pawn.Dead)
			{
				return;
			}
			this.Tick_CheckStartMarriageCeremony(delta);
			this.Tick_CheckDevelopBondRelation(delta);
		}

		// Token: 0x0600CFB0 RID: 53168 RVA: 0x003CFBB8 File Offset: 0x003CDDB8
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return null;
			}
			foreach (DirectPawnRelation directPawnRelation in this.directRelations)
			{
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return directPawnRelation;
				}
			}
			return null;
		}

		// Token: 0x0600CFB1 RID: 53169 RVA: 0x003CFC44 File Offset: 0x003CDE44
		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return null;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x0600CFB2 RID: 53170 RVA: 0x003CFCBC File Offset: 0x003CDEBC
		public int GetDirectRelationsCount(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			int num = 0;
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return num;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600CFB3 RID: 53171 RVA: 0x003CFD34 File Offset: 0x003CDF34
		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFB4 RID: 53172 RVA: 0x003CFDA0 File Offset: 0x003CDFA0
		public bool VirtualRelationExists(PawnRelationDef def, RelationshipRecord record)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.virtualRelations.Count; i++)
			{
				VirtualPawnRelation virtualPawnRelation = this.virtualRelations[i];
				if (virtualPawnRelation.def == def && virtualPawnRelation.record.ID == record.ID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFB5 RID: 53173 RVA: 0x003CFE18 File Offset: 0x003CE018
		public void GetDirectRelations(PawnRelationDef def, ref List<Pawn> list, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					list.Add(directPawnRelation.otherPawn);
				}
			}
		}

		// Token: 0x0600CFB6 RID: 53174 RVA: 0x003CFE94 File Offset: 0x003CE094
		public void ElevateToVirtualRelation(DirectPawnRelation relation)
		{
			RelationshipRecord orCreateRecord = Find.RelationshipRecords.GetOrCreateRecord(relation.otherPawn);
			this.virtualRelations.Add(new VirtualPawnRelation(relation.def, orCreateRecord, relation.startTicks));
			orCreateRecord.AddReference(this.pawn);
			this.directRelations.Remove(relation);
		}

		// Token: 0x0600CFB7 RID: 53175 RVA: 0x003CFEE8 File Offset: 0x003CE0E8
		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				string[] array = new string[6];
				array[0] = "Tried to directly add implied pawn relation ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = ", pawn=";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				array[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array));
				return;
			}
			if (otherPawn == this.pawn)
			{
				string str = "Tried to add pawn relation ";
				string str2 = (def != null) ? def.ToString() : null;
				string str3 = " with self, pawn=";
				Pawn pawn2 = this.pawn;
				Log.Warning(str + str2 + str3 + ((pawn2 != null) ? pawn2.ToString() : null));
				return;
			}
			if (this.DirectRelationExists(def, otherPawn))
			{
				string[] array2 = new string[6];
				array2[0] = "Tried to add the same relation twice: ";
				array2[1] = ((def != null) ? def.ToString() : null);
				array2[2] = ", pawn=";
				int num2 = 3;
				Pawn pawn3 = this.pawn;
				array2[num2] = ((pawn3 != null) ? pawn3.ToString() : null);
				array2[4] = ", otherPawn=";
				array2[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array2));
				return;
			}
			int startTicks = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
			def.Worker.OnRelationCreated(this.pawn, otherPawn);
			this.directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
			otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
			if (def.reflexive)
			{
				otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, this.pawn, startTicks));
				this.pawnsWithDirectRelationsWithMe.Add(otherPawn);
			}
			this.GainedOrLostDirectRelation();
			otherPawn.relations.GainedOrLostDirectRelation();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (!this.pawn.Dead && this.pawn.health != null)
				{
					for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						this.pawn.health.hediffSet.hediffs[i].Notify_RelationAdded(otherPawn, def);
					}
				}
				if (!otherPawn.Dead && otherPawn.health != null)
				{
					for (int j = otherPawn.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
					{
						otherPawn.health.hediffSet.hediffs[j].Notify_RelationAdded(this.pawn, def);
					}
				}
			}
		}

		// Token: 0x0600CFB8 RID: 53176 RVA: 0x003D0158 File Offset: 0x003CE358
		public void RemoveRelation(RelationshipRecord record)
		{
			for (int i = this.virtualRelations.Count - 1; i >= 0; i--)
			{
				if (this.virtualRelations[i].record == record)
				{
					this.virtualRelations.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600CFB9 RID: 53177 RVA: 0x003D019D File Offset: 0x003CE39D
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		// Token: 0x0600CFBA RID: 53178 RVA: 0x003D01B4 File Offset: 0x003CE3B4
		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				string[] array = new string[6];
				array[0] = "Could not remove relation ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " because it's not here. pawn=";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				array[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array));
			}
		}

		// Token: 0x0600CFBB RID: 53179 RVA: 0x003D0228 File Offset: 0x003CE428
		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				string[] array = new string[6];
				array[0] = "Tried to remove implied pawn relation ";
				int num = 1;
				PawnRelationDef def2 = def;
				array[num] = ((def2 != null) ? def2.ToString() : null);
				array[2] = ", pawn=";
				int num2 = 3;
				Pawn pawn = this.pawn;
				array[num2] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				int num3 = 5;
				Pawn otherPawn2 = otherPawn;
				array[num3] = ((otherPawn2 != null) ? otherPawn2.ToString() : null);
				Log.Warning(string.Concat(array));
				return false;
			}
			Predicate<DirectPawnRelation> <>9__1;
			Predicate<DirectPawnRelation> <>9__2;
			Predicate<DirectPawnRelation> <>9__0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
				{
					if (def.reflexive)
					{
						List<DirectPawnRelation> list = otherPawn.relations.directRelations;
						Predicate<DirectPawnRelation> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn));
						}
						DirectPawnRelation directPawnRelation = list.Find(match);
						directPawnRelation.def.Worker.OnRelationRemoved(otherPawn, this.pawn);
						list.Remove(directPawnRelation);
						Predicate<DirectPawnRelation> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((DirectPawnRelation x) => x.otherPawn == this.pawn));
						}
						if (list.Find(match2) == null)
						{
							this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
						}
					}
					this.directRelations[i].def.Worker.OnRelationRemoved(this.pawn, otherPawn);
					this.directRelations.RemoveAt(i);
					List<DirectPawnRelation> list2 = this.directRelations;
					Predicate<DirectPawnRelation> match3;
					if ((match3 = <>9__0) == null)
					{
						match3 = (<>9__0 = ((DirectPawnRelation x) => x.otherPawn == otherPawn));
					}
					if (list2.Find(match3) == null)
					{
						otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(this.pawn);
					}
					this.GainedOrLostDirectRelation();
					otherPawn.relations.GainedOrLostDirectRelation();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFBC RID: 53180 RVA: 0x003D0450 File Offset: 0x003CE650
		public int OpinionOf(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return 0;
			}
			if (this.pawn.Dead)
			{
				return 0;
			}
			int num = 0;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
			{
				num += pawnRelationDef.opinionOffset;
			}
			if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				num += this.pawn.needs.mood.thoughts.TotalOpinionOffset(other);
			}
			if (num != 0)
			{
				float num2 = 1f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].CurStage != null)
					{
						num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
					}
				}
				num = Mathf.RoundToInt((float)num * num2);
			}
			if (num > 0 && this.pawn.HostileTo(other))
			{
				num = 0;
			}
			return Mathf.Clamp(num, -100, 100);
		}

		// Token: 0x0600CFBD RID: 53181 RVA: 0x003D0594 File Offset: 0x003CE794
		public string OpinionExplanation(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("OpinionOf".Translate(other.LabelShort).Colorize(ColoredText.TipSectionTitleColor) + ": " + this.OpinionOf(other).ToStringWithSign());
			string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, this.pawn);
			if (!pawnSituationLabel.NullOrEmpty())
			{
				stringBuilder.AppendLine(" (" + pawnSituationLabel + ")");
			}
			else
			{
				stringBuilder.AppendLine();
			}
			bool flag = false;
			if (this.pawn.Dead)
			{
				stringBuilder.AppendLine(" - " + "IAmDead".Translate());
				flag = true;
			}
			else
			{
				foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
				{
					stringBuilder.AppendLine(" - " + pawnRelationDef.GetGenderSpecificLabelCap(other) + ": " + pawnRelationDef.opinionOffset.ToStringWithSign());
					flag = true;
				}
				if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
				{
					ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
					thoughts.GetDistinctSocialThoughtGroups(other, Pawn_RelationsTracker.tmpSocialThoughts);
					for (int i = 0; i < Pawn_RelationsTracker.tmpSocialThoughts.Count; i++)
					{
						ISocialThought socialThought = Pawn_RelationsTracker.tmpSocialThoughts[i];
						int num = 1;
						Thought thought = (Thought)socialThought;
						if (thought.def.IsMemory)
						{
							num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
						}
						stringBuilder.Append(" - " + thought.LabelCapSocial);
						if (num != 1)
						{
							stringBuilder.Append(" x" + num.ToString());
						}
						stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
						flag = true;
					}
				}
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int j = 0; j < hediffs.Count; j++)
				{
					HediffStage curStage = hediffs[j].CurStage;
					if (curStage != null && curStage.opinionOfOthersFactor != 1f)
					{
						stringBuilder.Append(" - " + hediffs[j].LabelBaseCap);
						if (curStage.opinionOfOthersFactor != 0f)
						{
							stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
						}
						else
						{
							stringBuilder.AppendLine();
						}
						flag = true;
					}
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine(" - " + "NoneBrackets".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600CFBE RID: 53182 RVA: 0x003D08B0 File Offset: 0x003CEAB0
		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			if (this.pawn.story != null && this.pawn.story.traits != null)
			{
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					return 0f;
				}
				if (!this.pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
				{
					if (this.pawn.story.traits.HasTrait(TraitDefOf.Gay))
					{
						if (otherPawn.gender != this.pawn.gender)
						{
							return 0f;
						}
					}
					else if (otherPawn.gender == this.pawn.gender)
					{
						return 0f;
					}
				}
			}
			if (this.pawn.ageTracker.AgeBiologicalYearsFloat < 16f || otherPawn.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				return 0f;
			}
			return this.LovinAgeFactor(otherPawn) * this.PrettinessFactor(otherPawn);
		}

		// Token: 0x0600CFBF RID: 53183 RVA: 0x003D09D0 File Offset: 0x003CEBD0
		public float PrettinessFactor(Pawn otherPawn)
		{
			float num = 0f;
			if (otherPawn.RaceProps.Humanlike)
			{
				num = otherPawn.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			}
			if (num < 0f)
			{
				return 0.3f;
			}
			if (num > 0f)
			{
				return 2.3f;
			}
			return 1f;
		}

		// Token: 0x0600CFC0 RID: 53184 RVA: 0x003D0A20 File Offset: 0x003CEC20
		public float LovinAgeFactor(Pawn otherPawn)
		{
			float num = 1f;
			float ageBiologicalYearsFloat = this.pawn.ageTracker.AgeBiologicalYearsFloat;
			float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
			if (this.pawn.gender == Gender.Male)
			{
				float min = ageBiologicalYearsFloat - 30f;
				float lower = ageBiologicalYearsFloat - 10f;
				float upper = ageBiologicalYearsFloat + 3f;
				float max = ageBiologicalYearsFloat + 10f;
				num = GenMath.FlatHill(0.2f, min, lower, upper, max, 0.2f, ageBiologicalYearsFloat2);
			}
			else if (this.pawn.gender == Gender.Female)
			{
				float min2 = ageBiologicalYearsFloat - 10f;
				float lower2 = ageBiologicalYearsFloat - 3f;
				float upper2 = ageBiologicalYearsFloat + 10f;
				float max2 = ageBiologicalYearsFloat + 30f;
				num = GenMath.FlatHill(0.2f, min2, lower2, upper2, max2, 0.2f, ageBiologicalYearsFloat2);
			}
			float num2 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat);
			float num3 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat2);
			return num * num2 * num3;
		}

		// Token: 0x0600CFC1 RID: 53185 RVA: 0x003D0B14 File Offset: 0x003CED14
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.romanceChanceFactor;
			}
			float num2 = 1f;
			HediffWithTarget hediffWithTarget = (HediffWithTarget)this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (hediffWithTarget != null && hediffWithTarget.target == otherPawn)
			{
				num2 = 10f;
			}
			float num3 = 1f;
			if (ModsConfig.BiotechActive && this.pawn.genes != null)
			{
				Pawn_StoryTracker story = otherPawn.story;
				if (((story != null) ? story.traits : null) == null || !otherPawn.story.traits.HasTrait(TraitDefOf.Kind))
				{
					List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].def.missingGeneRomanceChanceFactor != 1f && (otherPawn.genes == null || !otherPawn.genes.HasActiveGene(genesListForReading[i].def)))
						{
							num3 *= genesListForReading[i].def.missingGeneRomanceChanceFactor;
						}
					}
				}
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num * num2 * num3;
		}

		// Token: 0x0600CFC2 RID: 53186 RVA: 0x003D0C84 File Offset: 0x003CEE84
		public float CompatibilityWith(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
			float num = Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x), -0.45f, 0.45f);
			float num2 = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
			return num + num2;
		}

		// Token: 0x0600CFC3 RID: 53187 RVA: 0x003D0D0D File Offset: 0x003CEF0D
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600CFC4 RID: 53188 RVA: 0x003D0D44 File Offset: 0x003CEF44
		public void ClearAllRelations()
		{
			this.CleanupVirtualRelationReferences();
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x0600CFC5 RID: 53189 RVA: 0x003D0DFC File Offset: 0x003CEFFC
		private void CleanupVirtualRelationReferences()
		{
			foreach (VirtualPawnRelation virtualPawnRelation in this.virtualRelations)
			{
				virtualPawnRelation.record.RemoveReference(this.pawn);
			}
			this.virtualRelations.Clear();
		}

		// Token: 0x0600CFC6 RID: 53190 RVA: 0x003D0E64 File Offset: 0x003CF064
		public void ClearAllNonBloodRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.familyByBloodRelation)
				{
					this.RemoveDirectRelation(list[i]);
				}
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn && !list3[j].def.familyByBloodRelation)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x0600CFC7 RID: 53191 RVA: 0x003D0F40 File Offset: 0x003CF140
		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			this.RemoveRelationsOnDeath();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn) && !this.pawn.RaceProps.Animal)
			{
				this.AffectBondedAnimalsOnMyDeath();
			}
			this.Notify_FailedRescueQuest();
		}

		// Token: 0x0600CFC8 RID: 53192 RVA: 0x003D0FF4 File Offset: 0x003CF1F4
		public void Notify_PawnDestroyed(DestroyMode mode)
		{
			this.RemoveRelationsOnDeath();
		}

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x003D0FFC File Offset: 0x003CF1FC
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		// Token: 0x0600CFCA RID: 53194 RVA: 0x003D1012 File Offset: 0x003CF212
		public void Notify_PawnLeftBehind()
		{
			this.RemoveRelationsOnLeftBehind();
		}

		// Token: 0x0600CFCB RID: 53195 RVA: 0x003D101A File Offset: 0x003CF21A
		public void Notify_ExitedMap()
		{
			this.CheckRescued();
		}

		// Token: 0x0600CFCC RID: 53196 RVA: 0x003D1022 File Offset: 0x003CF222
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.CheckRescued();
			}
		}

		// Token: 0x0600CFCD RID: 53197 RVA: 0x003D103C File Offset: 0x003CF23C
		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThoughts != null)
					{
						if (mostImportantRelation == PawnRelationDefOf.Bond)
						{
							this.pawn.relations.RemoveDirectRelation(mostImportantRelation, pawn);
						}
						foreach (ThoughtDef def in mostImportantRelation.soldThoughts)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(def, playerNegotiator, null);
						}
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x0600CFCE RID: 53198 RVA: 0x003D1138 File Offset: 0x003CF338
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x0600CFCF RID: 53199 RVA: 0x003D1140 File Offset: 0x003CF340
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.pawn.needs.mood != null && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer, null);
				this.canGetRescuedThought = false;
			}
		}

		// Token: 0x0600CFD0 RID: 53200 RVA: 0x003D11A4 File Offset: 0x003CF3A4
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x0600CFD1 RID: 53201 RVA: 0x003D1278 File Offset: 0x003CF478
		private void CheckRescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x003D134C File Offset: 0x003CF54C
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		// Token: 0x0600CFD3 RID: 53203 RVA: 0x003D136E File Offset: 0x003CF56E
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		// Token: 0x0600CFD4 RID: 53204 RVA: 0x003D1390 File Offset: 0x003CF590
		private void RemoveMySpouseMarriageRelatedThoughts()
		{
			foreach (Pawn pawn in this.pawn.GetSpouses(false))
			{
				if (pawn.needs.mood != null)
				{
					MemoryThoughtHandler memories = pawn.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
					memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
				}
			}
		}

		// Token: 0x0600CFD5 RID: 53205 RVA: 0x003D141C File Offset: 0x003CF61C
		private void RemoveRelationsOnDeath()
		{
			for (int i = this.directRelations.Count - 1; i >= 0; i--)
			{
				if (this.directRelations[i].def.removeOnDeath)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					Pawn otherPawn = directPawnRelation.otherPawn;
					this.RemoveDirectRelation(this.directRelations[i]);
					directPawnRelation.def.Worker.Notify_PostRemovedByDeath(this.pawn, otherPawn);
				}
			}
		}

		// Token: 0x0600CFD6 RID: 53206 RVA: 0x003D1494 File Offset: 0x003CF694
		private void RemoveRelationsOnLeftBehind()
		{
			for (int i = this.directRelations.Count - 1; i >= 0; i--)
			{
				if (this.directRelations[i].def.removeOnLeftBehind)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					Pawn otherPawn = directPawnRelation.otherPawn;
					this.RemoveDirectRelation(this.directRelations[i]);
					directPawnRelation.def.Worker.Notify_PostRemovedLeftBehind(this.pawn, otherPawn);
				}
			}
		}

		// Token: 0x0600CFD7 RID: 53207 RVA: 0x003D150C File Offset: 0x003CF70C
		public void CheckAppendBondedAnimalDiedInfo(ref TaggedString letter, ref TaggedString label)
		{
			if (!this.pawn.RaceProps.Animal || !this.everSeenByPlayer || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				return;
			}
			Predicate<Pawn> isAffected = (Pawn x) => !x.Dead && (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath));
			int num = 0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			TaggedString t;
			if (num == 1)
			{
				Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
				t = "LetterPartBondedAnimalDied".Translate(this.pawn.LabelDefinite(), firstDirectRelationPawn.LabelShort, this.pawn.Named("ANIMAL"), firstDirectRelationPawn.Named("HUMAN")).CapitalizeFirst();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
					{
						stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
					}
				}
				t = "LetterPartBondedAnimalDiedMulti".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			label += " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
			if (!letter.NullOrEmpty())
			{
				letter += "\n\n";
			}
			letter += t;
		}

		// Token: 0x0600CFD8 RID: 53208 RVA: 0x003D1730 File Offset: 0x003CF930
		private void AffectBondedAnimalsOnMyDeath()
		{
			int num = 0;
			Pawn pawn = null;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && this.directRelations[i].otherPawn.Spawned)
				{
					pawn = this.directRelations[i].otherPawn;
					num++;
					float value = Rand.Value;
					MentalStateDef stateDef;
					if (value < 0.25f)
					{
						stateDef = MentalStateDefOf.Wander_Sad;
					}
					else if (value < 0.5f)
					{
						stateDef = MentalStateDefOf.Wander_Psychotic;
					}
					else if (value < 0.75f)
					{
						stateDef = MentalStateDefOf.Berserk;
					}
					else
					{
						stateDef = MentalStateDefOf.Manhunter;
					}
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "MentalStateReason_BondedHumanDeath".Translate(this.pawn).Resolve(), false, true, false, null, false, false, false);
				}
			}
			if (num == 1)
			{
				TaggedString taggedString;
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					taggedString = "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabelIndefinite(), pawn.Name.ToStringShort, this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				else
				{
					taggedString = "MessageBondedAnimalMentalBreak".Translate(pawn.LabelIndefinite(), this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				Messages.Message(taggedString.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
				return;
			}
			if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort, this.pawn.Named("HUMAN")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x0600CFD9 RID: 53209 RVA: 0x003D194C File Offset: 0x003CFB4C
		private void Tick_CheckStartMarriageCeremony(int delta)
		{
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(1017, delta))
			{
				int ticksGame = Find.TickManager.TicksGame;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					float num = (float)(ticksGame - this.directRelations[i].startTicks) / 60000f;
					if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn, this.directRelations[i].otherPawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn, this.pawn))
					{
						this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
					}
				}
			}
		}

		// Token: 0x0600CFDA RID: 53210 RVA: 0x003D1AEC File Offset: 0x003CFCEC
		private void Tick_CheckDevelopBondRelation(int delta)
		{
			if (!this.pawn.Spawned || !this.pawn.IsAnimal || this.pawn.Faction != Faction.OfPlayer || this.pawn.playerSettings.RespectedMaster == null)
			{
				return;
			}
			Pawn respectedMaster = this.pawn.playerSettings.RespectedMaster;
			if (this.pawn.IsHashIntervalTick(2500, delta) && this.pawn.Map == respectedMaster.Map && this.pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(this.pawn.Position, respectedMaster.Position, this.pawn.Map))
			{
				RelationsUtility.TryDevelopBondRelation(respectedMaster, this.pawn, 0.001f);
			}
		}

		// Token: 0x0600CFDB RID: 53211 RVA: 0x003D1BC4 File Offset: 0x003CFDC4
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x04008D63 RID: 36195
		private Pawn pawn;

		// Token: 0x04008D64 RID: 36196
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		// Token: 0x04008D65 RID: 36197
		private List<VirtualPawnRelation> virtualRelations = new List<VirtualPawnRelation>();

		// Token: 0x04008D66 RID: 36198
		public bool everSeenByPlayer;

		// Token: 0x04008D67 RID: 36199
		public bool canGetRescuedThought = true;

		// Token: 0x04008D68 RID: 36200
		public Pawn relativeInvolvedInRescueQuest;

		// Token: 0x04008D69 RID: 36201
		public MarriageNameChange nextMarriageNameChange;

		// Token: 0x04008D6A RID: 36202
		public bool hidePawnRelations;

		// Token: 0x04008D6B RID: 36203
		[LoadAlias("rhythmModes")]
		private Dictionary<Pawn, PregnancyApproach> pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();

		// Token: 0x04008D6C RID: 36204
		public int romanceEnableTick = -1;

		// Token: 0x04008D6D RID: 36205
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		// Token: 0x04008D6E RID: 36206
		private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

		// Token: 0x04008D6F RID: 36207
		private bool familyByBloodIsCached;

		// Token: 0x04008D70 RID: 36208
		private bool canCacheFamilyByBlood;

		// Token: 0x04008D71 RID: 36209
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		// Token: 0x04008D72 RID: 36210
		private const float MaxBondRelationCheckDist = 12f;

		// Token: 0x04008D73 RID: 36211
		private const float BondRelationPerIntervalChance = 0.001f;

		// Token: 0x04008D74 RID: 36212
		public const int FriendOpinionThreshold = 20;

		// Token: 0x04008D75 RID: 36213
		public const int RivalOpinionThreshold = -20;

		// Token: 0x04008D76 RID: 36214
		public const int MinLovinAge = 16;

		// Token: 0x04008D77 RID: 36215
		private const float MinAgeFactor = 0.2f;

		// Token: 0x04008D78 RID: 36216
		public const float PsychicLoveFactor = 10f;

		// Token: 0x04008D79 RID: 36217
		private List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04008D7A RID: 36218
		private List<PregnancyApproach> tmpPregnancyApproaches = new List<PregnancyApproach>();

		// Token: 0x04008D7B RID: 36219
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
