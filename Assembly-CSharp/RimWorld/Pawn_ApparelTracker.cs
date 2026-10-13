using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024A1 RID: 9377
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x17001EFB RID: 7931
		// (get) Token: 0x0600CB62 RID: 52066 RVA: 0x003B97AF File Offset: 0x003B79AF
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001EFC RID: 7932
		// (get) Token: 0x0600CB63 RID: 52067 RVA: 0x003B97B7 File Offset: 0x003B79B7
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17001EFD RID: 7933
		// (get) Token: 0x0600CB64 RID: 52068 RVA: 0x003B97C4 File Offset: 0x003B79C4
		public List<ApparelRequirement> ActiveRequirementsForReading
		{
			get
			{
				if (this.activeRequirementsDirty)
				{
					this.activeRequirements.Clear();
					foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.AllRequirements)
					{
						string text;
						if (ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this.pawn, out text))
						{
							this.activeRequirements.Add(apparelRequirementWithSource.requirement);
						}
					}
					this.activeRequirementsDirty = false;
				}
				return this.activeRequirements;
			}
		}

		// Token: 0x17001EFE RID: 7934
		// (get) Token: 0x0600CB65 RID: 52069 RVA: 0x003B9860 File Offset: 0x003B7A60
		public List<ApparelRequirementWithSource> AllRequirements
		{
			get
			{
				if (this.allRequirementsDirty)
				{
					this.allRequirements.Clear();
					if (ModsConfig.RoyaltyActive && this.pawn.royalty != null)
					{
						foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesForReading)
						{
							if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								foreach (ApparelRequirement apparelRequirement in royalTitle.def.requiredApparel)
								{
									if (apparelRequirement.IsValid)
									{
										this.allRequirements.Add(new ApparelRequirementWithSource(apparelRequirement, royalTitle));
									}
								}
							}
						}
					}
					if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
					{
						Precept_Role role = this.pawn.Ideo.GetRole(this.pawn);
						if (role != null && !role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
						{
							foreach (PreceptApparelRequirement preceptApparelRequirement in role.apparelRequirements)
							{
								if (preceptApparelRequirement.requirement.IsValid)
								{
									this.allRequirements.Add(new ApparelRequirementWithSource(preceptApparelRequirement.requirement, role));
								}
							}
						}
					}
					this.allRequirementsDirty = false;
				}
				return this.allRequirements;
			}
		}

		// Token: 0x17001EFF RID: 7935
		// (get) Token: 0x0600CB66 RID: 52070 RVA: 0x003B9A08 File Offset: 0x003B7C08
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17001F00 RID: 7936
		// (get) Token: 0x0600CB67 RID: 52071 RVA: 0x003B9A15 File Offset: 0x003B7C15
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		// Token: 0x17001F01 RID: 7937
		// (get) Token: 0x0600CB68 RID: 52072 RVA: 0x003B9A28 File Offset: 0x003B7C28
		public bool AnyClothing
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.countsAsClothingForNudity)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001F02 RID: 7938
		// (get) Token: 0x0600CB69 RID: 52073 RVA: 0x003B9A8C File Offset: 0x003B7C8C
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		// Token: 0x17001F03 RID: 7939
		// (get) Token: 0x0600CB6A RID: 52074 RVA: 0x003B9A9C File Offset: 0x003B7C9C
		public bool AnyApparelUnlocked
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.AnyApparel;
				}
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F04 RID: 7940
		// (get) Token: 0x0600CB6B RID: 52075 RVA: 0x003B9AE8 File Offset: 0x003B7CE8
		public bool AllApparelLocked
		{
			get
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001F05 RID: 7941
		// (get) Token: 0x0600CB6C RID: 52076 RVA: 0x003B9B22 File Offset: 0x003B7D22
		public List<Apparel> LockedApparel
		{
			get
			{
				if (this.lockedApparel == null)
				{
					return Pawn_ApparelTracker.EmptyApparel;
				}
				return this.lockedApparel;
			}
		}

		// Token: 0x17001F06 RID: 7942
		// (get) Token: 0x0600CB6D RID: 52077 RVA: 0x003B9B38 File Offset: 0x003B7D38
		public IEnumerable<Apparel> UnlockedApparel
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.WornApparel;
				}
				return from x in this.WornApparel
				where !this.IsLocked(x)
				select x;
			}
		}

		// Token: 0x17001F07 RID: 7943
		// (get) Token: 0x0600CB6E RID: 52078 RVA: 0x003B9B70 File Offset: 0x003B7D70
		public bool PsychologicallyNude
		{
			get
			{
				if (this.pawn.gender == Gender.None)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return false;
				}
				bool flag;
				bool flag2;
				this.HasBasicApparel(out flag, out flag2);
				if (!flag)
				{
					bool flag3 = false;
					using (IEnumerator<BodyPartRecord> enumerator = this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsInGroup(BodyPartGroupDefOf.Legs))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						flag = true;
					}
				}
				if (this.pawn.gender == Gender.Male)
				{
					return !flag;
				}
				return this.pawn.gender == Gender.Female && (!flag || !flag2);
			}
		}

		// Token: 0x17001F08 RID: 7944
		// (get) Token: 0x0600CB6F RID: 52079 RVA: 0x003B9C38 File Offset: 0x003B7E38
		public bool AnyApparelNeedsRecoloring
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.DesiredColor != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001F09 RID: 7945
		// (get) Token: 0x0600CB70 RID: 52080 RVA: 0x003B9C9C File Offset: 0x003B7E9C
		public IEnumerable<Verb> AllApparelVerbs
		{
			get
			{
				List<Apparel> list = this.WornApparel;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					Apparel apparel = list[i];
					CompApparelVerbOwner comp = apparel.GetComp<CompApparelVerbOwner>();
					List<Verb> verbs = (comp != null) ? comp.AllVerbs : null;
					if (verbs != null)
					{
						for (int j = 0; j < verbs.Count; j = num + 1)
						{
							yield return verbs[j];
							num = j;
						}
						verbs = null;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001F0A RID: 7946
		// (get) Token: 0x0600CB71 RID: 52081 RVA: 0x003B9CAC File Offset: 0x003B7EAC
		public Verb FirstApparelVerb
		{
			get
			{
				List<Apparel> list = this.WornApparel;
				for (int i = 0; i < list.Count; i++)
				{
					CompApparelVerbOwner comp = list[i].GetComp<CompApparelVerbOwner>();
					List<Verb> list2 = (comp != null) ? comp.AllVerbs : null;
					if (list2 != null && list2.Count != 0)
					{
						return list2[0];
					}
				}
				return null;
			}
		}

		// Token: 0x0600CB72 RID: 52082 RVA: 0x003B9D00 File Offset: 0x003B7F00
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x0600CB73 RID: 52083 RVA: 0x003B9D54 File Offset: 0x003B7F54
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Apparel>>(ref this.wornApparel, "wornApparel", new object[]
			{
				this
			});
			Scribe_Collections.Look<Apparel>(ref this.lockedApparel, "lockedApparel", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastApparelWearoutTick, "lastApparelWearoutTick", 0, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.SortWornApparelIntoDrawOrder();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.lockedApparel != null)
				{
					this.lockedApparel.RemoveAll((Apparel x) => x == null);
				}
				foreach (Apparel apparel in this.WornApparel)
				{
					CompApparelVerbOwner comp = apparel.GetComp<CompApparelVerbOwner>();
					if (comp != null)
					{
						foreach (Verb verb in comp.AllVerbs)
						{
							verb.caster = this.pawn;
						}
					}
				}
			}
		}

		// Token: 0x0600CB74 RID: 52084 RVA: 0x003B9E7C File Offset: 0x003B807C
		public void ApparelTrackerTickRare()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.lastApparelWearoutTick < 0)
			{
				this.lastApparelWearoutTick = ticksGame;
			}
			if (ticksGame - this.lastApparelWearoutTick >= 60000)
			{
				if (!this.pawn.IsWorldPawn())
				{
					for (int i = this.wornApparel.Count - 1; i >= 0; i--)
					{
						this.TakeWearoutDamageForDay(this.wornApparel[i]);
					}
				}
				this.lastApparelWearoutTick = ticksGame;
			}
		}

		// Token: 0x0600CB75 RID: 52085 RVA: 0x003B9EF4 File Offset: 0x003B80F4
		public void ApparelTrackerTickInterval(int delta)
		{
			if (this.pawn.IsColonist && this.pawn.Spawned && !this.pawn.Dead && this.PsychologicallyNude && this.pawn.IsHashIntervalTick(60000, delta))
			{
				TaleRecorder.RecordTale(TaleDefOf.WalkedNaked, new object[]
				{
					this.pawn
				});
			}
			if (this.lockedApparel == null)
			{
				return;
			}
			for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
			{
				if (this.lockedApparel[i].def.useHitPoints && (float)this.lockedApparel[i].HitPoints / (float)this.lockedApparel[i].MaxHitPoints < 0.5f)
				{
					this.Unlock(this.lockedApparel[i]);
				}
			}
		}

		// Token: 0x0600CB76 RID: 52086 RVA: 0x003B9FD4 File Offset: 0x003B81D4
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		// Token: 0x0600CB77 RID: 52087 RVA: 0x003B9FEC File Offset: 0x003B81EC
		public void Lock(Apparel apparel)
		{
			if (this.IsLocked(apparel))
			{
				return;
			}
			if (this.lockedApparel == null)
			{
				this.lockedApparel = new List<Apparel>();
			}
			this.lockedApparel.Add(apparel);
		}

		// Token: 0x0600CB78 RID: 52088 RVA: 0x003BA017 File Offset: 0x003B8217
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		// Token: 0x0600CB79 RID: 52089 RVA: 0x003BA030 File Offset: 0x003B8230
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		// Token: 0x0600CB7A RID: 52090 RVA: 0x003BA068 File Offset: 0x003B8268
		public void UnlockAll()
		{
			if (!this.lockedApparel.NullOrEmpty<Apparel>())
			{
				for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
				{
					this.Unlock(this.lockedApparel[i]);
				}
			}
		}

		// Token: 0x0600CB7B RID: 52091 RVA: 0x003BA0AC File Offset: 0x003B82AC
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				Messages.Message("MessageWornApparelDeterioratedAway".Translate(GenLabel.ThingLabel(ap.def, ap.Stuff, 1), this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600CB7C RID: 52092 RVA: 0x003BA168 File Offset: 0x003B8368
		public bool CanWearWithoutDroppingAnything(ThingDef apDef)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(apDef, this.wornApparel[i].def, this.pawn.RaceProps.body))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CB7D RID: 52093 RVA: 0x003BA1B8 File Offset: 0x003B83B8
		public void Wear(Apparel newApparel, bool dropReplacedApparel = true, bool locked = false)
		{
			newApparel.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (!ApparelUtility.HasPartsToWear(this.pawn, newApparel.def))
			{
				Pawn pawn = this.pawn;
				Log.Warning(((pawn != null) ? pawn.ToString() : null) + " tried to wear " + ((newApparel != null) ? newApparel.ToString() : null) + " but he has no body parts required to wear it.");
				return;
			}
			if (CompBiocodable.IsBiocoded(newApparel) && !CompBiocodable.IsBiocodedFor(newApparel, this.pawn))
			{
				CompBiocodable compBiocodable = newApparel.TryGetComp<CompBiocodable>();
				string[] array = new string[6];
				int num = 0;
				Pawn pawn2 = this.pawn;
				array[num] = ((pawn2 != null) ? pawn2.ToString() : null);
				array[1] = " tried to wear ";
				array[2] = ((newApparel != null) ? newApparel.ToString() : null);
				array[3] = " but it is biocoded for ";
				array[4] = compBiocodable.CodedPawnLabel;
				array[5] = " .";
				Log.Warning(string.Concat(array));
				return;
			}
			if (!newApparel.PawnCanWear(this.pawn, true))
			{
				Pawn pawn3 = this.pawn;
				Log.Warning(((pawn3 != null) ? pawn3.ToString() : null) + " tried to wear " + ((newApparel != null) ? newApparel.ToString() : null) + " but is not allowed to.");
				return;
			}
			for (int i = this.wornApparel.Count - 1; i >= 0; i--)
			{
				Apparel apparel = this.wornApparel[i];
				if (!ApparelUtility.CanWearTogether(newApparel.def, apparel.def, this.pawn.RaceProps.body))
				{
					if (dropReplacedApparel)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (this.pawn.Map == null)
						{
							this.wornApparel.Remove(apparel);
						}
						else if (!this.TryDrop(apparel, out apparel2, this.pawn.PositionHeld, forbid))
						{
							Pawn pawn4 = this.pawn;
							string str = (pawn4 != null) ? pawn4.ToString() : null;
							string str2 = " could not drop ";
							Apparel apparel3 = apparel;
							Log.Error(str + str2 + ((apparel3 != null) ? apparel3.ToString() : null));
							return;
						}
					}
					else
					{
						this.Remove(apparel);
					}
				}
			}
			if (newApparel.Wearer != null)
			{
				string[] array2 = new string[6];
				int num2 = 0;
				Pawn pawn5 = this.pawn;
				array2[num2] = ((pawn5 != null) ? pawn5.ToString() : null);
				array2[1] = " is trying to wear ";
				array2[2] = ((newApparel != null) ? newApparel.ToString() : null);
				array2[3] = " but this apparel already has a wearer (";
				int num3 = 4;
				Pawn wearer = newApparel.Wearer;
				array2[num3] = ((wearer != null) ? wearer.ToString() : null);
				array2[5] = "). This may or may not cause bugs.";
				Log.Warning(string.Concat(array2));
			}
			this.wornApparel.TryAdd(newApparel, false);
			if (locked)
			{
				this.Lock(newApparel);
			}
		}

		// Token: 0x0600CB7E RID: 52094 RVA: 0x003BA433 File Offset: 0x003B8633
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x0600CB7F RID: 52095 RVA: 0x003BA444 File Offset: 0x003B8644
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x0600CB80 RID: 52096 RVA: 0x003BA45A File Offset: 0x003B865A
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x0600CB81 RID: 52097 RVA: 0x003BA470 File Offset: 0x003B8670
		public bool TryDrop(Apparel ap, out Apparel resultingAp, IntVec3 pos, bool forbid = true)
		{
			if (this.wornApparel.TryDrop(ap, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingAp, null, null))
			{
				if (resultingAp != null)
				{
					resultingAp.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600CB82 RID: 52098 RVA: 0x003BA4A1 File Offset: 0x003B86A1
		public bool TryMoveToInventory(Apparel apparel)
		{
			if (!this.wornApparel.Contains(apparel))
			{
				return false;
			}
			this.Remove(apparel);
			return this.pawn.inventory.innerContainer.TryAdd(apparel, true);
		}

		// Token: 0x0600CB83 RID: 52099 RVA: 0x003BA4D4 File Offset: 0x003B86D4
		public void DropAll(IntVec3 pos, bool forbid = true, bool dropLocked = true, Predicate<Apparel> selector = null)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if ((dropLocked || !this.IsLocked(this.wornApparel[i])) && (selector == null || selector(this.wornApparel[i])))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				Apparel apparel;
				this.TryDrop(Pawn_ApparelTracker.tmpApparelList[j], out apparel, pos, forbid);
			}
		}

		// Token: 0x0600CB84 RID: 52100 RVA: 0x003BA574 File Offset: 0x003B8774
		public void MoveAllToInventory(bool moveLocked = true, Predicate<Apparel> selector = null)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if ((moveLocked || !this.IsLocked(this.wornApparel[i])) && (selector == null || selector(this.wornApparel[i])))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				this.TryMoveToInventory(Pawn_ApparelTracker.tmpApparelList[j]);
			}
		}

		// Token: 0x0600CB85 RID: 52101 RVA: 0x003BA60C File Offset: 0x003B880C
		public void DropAllOrMoveAllToInventory(Predicate<Apparel> selector = null)
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				this.DropAll(this.pawn.PositionHeld, false, true, selector);
				return;
			}
			this.MoveAllToInventory(true, selector);
		}

		// Token: 0x0600CB86 RID: 52102 RVA: 0x003BA638 File Offset: 0x003B8838
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600CB87 RID: 52103 RVA: 0x003BA646 File Offset: 0x003B8846
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x0600CB88 RID: 52104 RVA: 0x003BA654 File Offset: 0x003B8854
		public bool Wearing(Thing apparel)
		{
			using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CB89 RID: 52105 RVA: 0x003BA6AC File Offset: 0x003B88AC
		public bool WouldReplaceLockedApparel(Apparel newApparel)
		{
			if (!this.AnyApparelLocked)
			{
				return false;
			}
			for (int i = 0; i < this.lockedApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, this.lockedApparel[i].def, this.pawn.RaceProps.body))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CB8A RID: 52106 RVA: 0x003BA70C File Offset: 0x003B890C
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn))
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x0600CB8B RID: 52107 RVA: 0x003BA804 File Offset: 0x003B8A04
		public void Notify_LostBodyPart()
		{
			Pawn_ApparelTracker.tmpApparel.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparel.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparel.Count; j++)
			{
				Apparel apparel = Pawn_ApparelTracker.tmpApparel[j];
				if (!ApparelUtility.HasPartsToWear(this.pawn, apparel.def))
				{
					this.Remove(apparel);
				}
			}
		}

		// Token: 0x0600CB8C RID: 52108 RVA: 0x003BA882 File Offset: 0x003B8A82
		public void Notify_TitleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x0600CB8D RID: 52109 RVA: 0x003BA882 File Offset: 0x003B8A82
		public void Notify_RoleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x0600CB8E RID: 52110 RVA: 0x003BA882 File Offset: 0x003B8A82
		public void Notify_IdeoChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x003BA892 File Offset: 0x003B8A92
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x0600CB90 RID: 52112 RVA: 0x003BA8C4 File Offset: 0x003B8AC4
		public void HasBasicApparel(out bool hasPants, out bool hasShirt)
		{
			hasShirt = false;
			hasPants = false;
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						hasShirt = true;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						hasPants = true;
					}
					if (hasShirt & hasPants)
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600CB91 RID: 52113 RVA: 0x003BA968 File Offset: 0x003B8B68
		public Apparel FirstApparelOnBodyPartGroup(BodyPartGroupDef g)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == g)
					{
						return apparel;
					}
				}
			}
			return null;
		}

		// Token: 0x0600CB92 RID: 52114 RVA: 0x003BA9D4 File Offset: 0x003B8BD4
		public bool TryGetFirstApparelOnBodyPart(BodyPartRecord record, out Apparel apparel, Predicate<Apparel> validator = null)
		{
			apparel = this.FirstApparelOnBodyPart(record, validator);
			return apparel != null;
		}

		// Token: 0x0600CB93 RID: 52115 RVA: 0x003BA9E8 File Offset: 0x003B8BE8
		public Apparel FirstApparelOnBodyPart(BodyPartRecord record, Predicate<Apparel> validator = null)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				if (apparel.def.apparel.CoversBodyPart(record) && (validator == null || validator(apparel)))
				{
					return apparel;
				}
			}
			return null;
		}

		// Token: 0x0600CB94 RID: 52116 RVA: 0x003BAA3C File Offset: 0x003B8C3C
		public bool BodyPartGroupIsCovered(BodyPartGroupDef bp, Predicate<Apparel> validator = null)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == bp && (validator == null || validator(apparel)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CB95 RID: 52117 RVA: 0x003BAAB4 File Offset: 0x003B8CB4
		public IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.wornApparel.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.wornApparel[i].GetWornGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600CB96 RID: 52118 RVA: 0x003BAAC4 File Offset: 0x003B8CC4
		public void Notify_ApparelChanged()
		{
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			Pawn_AbilityTracker abilities = this.pawn.abilities;
			if (abilities != null)
			{
				abilities.Notify_TemporaryAbilitiesChanged();
			}
			if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.Notify_ApparelChanged();
			}
		}

		// Token: 0x0600CB97 RID: 52119 RVA: 0x003BAB20 File Offset: 0x003B8D20
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.Notify_ApparelChanged();
			CompApparelVerbOwner_Charged comp = apparel.GetComp<CompApparelVerbOwner_Charged>();
			List<Verb> list = (comp != null) ? comp.AllVerbs : null;
			if (list != null)
			{
				foreach (Verb verb in list)
				{
					verb.caster = this.pawn;
					verb.Notify_PickedUp();
				}
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
			apparel.Notify_Equipped(this.pawn);
		}

		// Token: 0x0600CB98 RID: 52120 RVA: 0x003BABCC File Offset: 0x003B8DCC
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.Notify_ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
			if (this.IsLocked(apparel))
			{
				this.Unlock(apparel);
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
			apparel.Notify_Unequipped(this.pawn);
		}

		// Token: 0x0600CB99 RID: 52121 RVA: 0x003BAC58 File Offset: 0x003B8E58
		public void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.wornApparel[i].Notify_BulletImpactNearby(impactData);
			}
		}

		// Token: 0x0600CB9A RID: 52122 RVA: 0x003BAC8D File Offset: 0x003B8E8D
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x0600CB9B RID: 52123 RVA: 0x003BAC95 File Offset: 0x003B8E95
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04008ABC RID: 35516
		public Pawn pawn;

		// Token: 0x04008ABD RID: 35517
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x04008ABE RID: 35518
		private List<Apparel> lockedApparel;

		// Token: 0x04008ABF RID: 35519
		private int lastApparelWearoutTick = -1;

		// Token: 0x04008AC0 RID: 35520
		private List<ApparelRequirementWithSource> allRequirements = new List<ApparelRequirementWithSource>();

		// Token: 0x04008AC1 RID: 35521
		private List<ApparelRequirement> activeRequirements = new List<ApparelRequirement>();

		// Token: 0x04008AC2 RID: 35522
		private bool activeRequirementsDirty = true;

		// Token: 0x04008AC3 RID: 35523
		private bool allRequirementsDirty = true;

		// Token: 0x04008AC4 RID: 35524
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x04008AC5 RID: 35525
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		// Token: 0x04008AC6 RID: 35526
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		// Token: 0x04008AC7 RID: 35527
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x04008AC8 RID: 35528
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
