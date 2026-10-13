using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020024C6 RID: 9414
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x17001F4E RID: 8014
		// (get) Token: 0x0600CCF4 RID: 52468 RVA: 0x003C19BA File Offset: 0x003BFBBA
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600CCF5 RID: 52469 RVA: 0x003C19C2 File Offset: 0x003BFBC2
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CCF6 RID: 52470 RVA: 0x003C19DC File Offset: 0x003BFBDC
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
		}

		// Token: 0x0600CCF7 RID: 52471 RVA: 0x003C19F4 File Offset: 0x003BFBF4
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x0600CCF8 RID: 52472 RVA: 0x003C1A58 File Offset: 0x003BFC58
		private void ChooseMeleeVerb(Thing target)
		{
			bool flag = Rand.Chance(0.04f);
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(flag);
			bool flag2 = false;
			VerbEntry verbEntry;
			if (updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry))
			{
				flag2 = true;
			}
			else if (flag)
			{
				updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(false);
				flag2 = updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry);
			}
			if (flag2)
			{
				this.SetCurMeleeVerb(verbEntry.verb, target);
				return;
			}
			Log.ErrorOnce(string.Concat(new string[]
			{
				this.pawn.ToStringSafe<Pawn>(),
				" has no available melee attack, spawned=",
				this.pawn.Spawned.ToString(),
				" dead=",
				this.pawn.Dead.ToString(),
				" downed=",
				this.pawn.Downed.ToString(),
				" curJob=",
				this.pawn.CurJob.ToStringSafe<Job>(),
				" verbList=",
				updatedAvailableVerbsList.ToStringSafeEnumerable(),
				" bodyVerbs=",
				this.pawn.verbTracker.AllVerbs.ToStringSafeEnumerable()
			}), this.pawn.thingIDNumber ^ 195867354);
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x0600CCF9 RID: 52473 RVA: 0x003C1BC0 File Offset: 0x003BFDC0
		public bool TryMeleeAttack(Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (this.pawn.stances.FullBodyBusy)
			{
				return false;
			}
			if (!this.pawn.kindDef.canMeleeAttack)
			{
				return false;
			}
			if (verbToUse != null)
			{
				if (!verbToUse.IsStillUsableBy(this.pawn))
				{
					return false;
				}
				if (!verbToUse.IsMeleeAttack)
				{
					string[] array = new string[7];
					array[0] = "Pawn ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " tried to melee attack ";
					array[3] = ((target != null) ? target.ToString() : null);
					array[4] = " with non melee-attack verb ";
					array[5] = ((verbToUse != null) ? verbToUse.ToString() : null);
					array[6] = ".";
					Log.Warning(string.Concat(array));
					return false;
				}
			}
			Verb verb = verbToUse ?? this.TryGetMeleeVerb(target);
			if (verb == null)
			{
				return false;
			}
			verb.TryStartCastOn(target, surpriseAttack, true, false, false);
			return true;
		}

		// Token: 0x0600CCFA RID: 52474 RVA: 0x003C1C9C File Offset: 0x003BFE9C
		public List<VerbEntry> GetUpdatedAvailableVerbsList(bool terrainTools)
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
			if (!terrainTools)
			{
				List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs[i]))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs[i]);
					}
				}
				if (this.pawn.equipment != null)
				{
					List<ThingWithComps> allEquipmentListForReading = this.pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						CompEquippable comp = allEquipmentListForReading[j].GetComp<CompEquippable>();
						List<Verb> list = (comp != null) ? comp.AllVerbs : null;
						if (list != null)
						{
							for (int k = 0; k < list.Count; k++)
							{
								if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(list[k]))
								{
									Pawn_MeleeVerbs.verbsToAdd.Add(list[k]);
								}
							}
						}
					}
				}
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						CompEquippable comp2 = wornApparel[l].GetComp<CompEquippable>();
						List<Verb> list2 = (comp2 != null) ? comp2.AllVerbs : null;
						if (list2 != null)
						{
							for (int m = 0; m < list2.Count; m++)
							{
								if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(list2[m]))
								{
									Pawn_MeleeVerbs.verbsToAdd.Add(list2[m]);
								}
							}
						}
					}
				}
				foreach (Verb verb in this.pawn.health.hediffSet.GetHediffsVerbs())
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb);
					}
				}
				if (!ModsConfig.AnomalyActive || !this.pawn.IsMutant)
				{
					goto IL_2E9;
				}
				using (List<Verb>.Enumerator enumerator = this.pawn.mutant.AllVerbs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Verb verb2 = enumerator.Current;
						if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb2))
						{
							Pawn_MeleeVerbs.verbsToAdd.Add(verb2);
						}
					}
					goto IL_2E9;
				}
			}
			if (this.pawn.Spawned && !this.pawn.IsSubhuman)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.terrainVerbs == null || this.terrainVerbs.def != terrain)
				{
					this.terrainVerbs = Pawn_MeleeVerbs_TerrainSource.Create(this, terrain);
				}
				List<Verb> allVerbs2 = this.terrainVerbs.tracker.AllVerbs;
				for (int n = 0; n < allVerbs2.Count; n++)
				{
					Verb verb3 = allVerbs2[n];
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb3))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb3);
					}
				}
			}
			IL_2E9:
			float num = 0f;
			foreach (Verb v in Pawn_MeleeVerbs.verbsToAdd)
			{
				float num2 = VerbUtility.InitialVerbWeight(v, this.pawn);
				if (num2 > num)
				{
					num = num2;
				}
			}
			foreach (Verb verb4 in Pawn_MeleeVerbs.verbsToAdd)
			{
				Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(verb4, this.pawn, Pawn_MeleeVerbs.verbsToAdd, num));
			}
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		// Token: 0x0600CCFB RID: 52475 RVA: 0x003C2064 File Offset: 0x003C0264
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x0600CCFC RID: 52476 RVA: 0x003C2064 File Offset: 0x003C0264
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x0600CCFD RID: 52477 RVA: 0x003C206E File Offset: 0x003C026E
		public void Notify_UsedTerrainBasedVerb()
		{
			this.lastTerrainBasedVerbUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600CCFE RID: 52478 RVA: 0x003C2080 File Offset: 0x003C0280
		private void SetCurMeleeVerb(Verb v, Thing target)
		{
			this.curMeleeVerb = v;
			this.curMeleeVerbTarget = target;
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.curMeleeVerbUpdateTick = 0;
				return;
			}
			this.curMeleeVerbUpdateTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600CCFF RID: 52479 RVA: 0x003C20B0 File Offset: 0x003C02B0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.curMeleeVerb = null;
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
			Scribe_Deep.Look<Pawn_MeleeVerbs_TerrainSource>(ref this.terrainVerbs, "terrainVerbs", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTerrainBasedVerbUseTick, "lastTerrainBasedVerbUseTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.terrainVerbs != null)
			{
				this.terrainVerbs.parent = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curMeleeVerb != null && this.curMeleeVerb.BuggedAfterLoading)
			{
				this.curMeleeVerb = null;
				Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had a bugged melee verb after loading.");
			}
		}

		// Token: 0x0600CD01 RID: 52481 RVA: 0x003C21A5 File Offset: 0x003C03A5
		[CompilerGenerated]
		private bool <GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(Verb v)
		{
			return v.IsMeleeAttack && v.IsStillUsableBy(this.pawn);
		}

		// Token: 0x04008B97 RID: 35735
		private Pawn pawn;

		// Token: 0x04008B98 RID: 35736
		private Verb curMeleeVerb;

		// Token: 0x04008B99 RID: 35737
		private Thing curMeleeVerbTarget;

		// Token: 0x04008B9A RID: 35738
		private int curMeleeVerbUpdateTick;

		// Token: 0x04008B9B RID: 35739
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs;

		// Token: 0x04008B9C RID: 35740
		public int lastTerrainBasedVerbUseTick = -99999;

		// Token: 0x04008B9D RID: 35741
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x04008B9E RID: 35742
		private static List<Verb> verbsToAdd = new List<Verb>();

		// Token: 0x04008B9F RID: 35743
		private const int BestMeleeVerbUpdateInterval = 60;

		// Token: 0x04008BA0 RID: 35744
		public const int TerrainBasedVerbUseDelay = 1200;

		// Token: 0x04008BA1 RID: 35745
		private const float TerrainBasedVerbChooseChance = 0.04f;
	}
}
