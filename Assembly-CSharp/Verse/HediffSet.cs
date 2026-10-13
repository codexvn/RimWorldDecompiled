using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000785 RID: 1925
	public class HediffSet : IExposable
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002F00 RID: 12032 RVA: 0x000F186F File Offset: 0x000EFA6F
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002F01 RID: 12033 RVA: 0x000F1890 File Offset: 0x000EFA90
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002F02 RID: 12034 RVA: 0x000F18B4 File Offset: 0x000EFAB4
		public bool HasHead
		{
			get
			{
				bool value = this.cachedHasHead.GetValueOrDefault();
				if (this.cachedHasHead == null)
				{
					value = this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head);
					this.cachedHasHead = new bool?(value);
				}
				return this.cachedHasHead.Value;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002F03 RID: 12035 RVA: 0x000F1920 File Offset: 0x000EFB20
		public bool HasRegeneration
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				bool value = this.cachedHasRegeneration.GetValueOrDefault();
				if (this.cachedHasRegeneration == null)
				{
					value = this.hediffs.Any((Hediff hd) => hd.CurStage != null && hd.CurStage.regeneration > 0f);
					this.cachedHasRegeneration = new bool?(value);
				}
				return this.cachedHasRegeneration.Value;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002F04 RID: 12036 RVA: 0x000F1994 File Offset: 0x000EFB94
		public bool HasPreventsDeath
		{
			get
			{
				using (List<Hediff>.Enumerator enumerator = this.hediffs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.preventsDeath)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x000F19F4 File Offset: 0x000EFBF4
		public bool AnyHediffPreventsCrawling
		{
			get
			{
				bool value = this.cachedCanCrawl.GetValueOrDefault();
				if (this.cachedCanCrawl == null)
				{
					value = this.hediffs.Any((Hediff hd) => hd.def.preventsCrawling);
					this.cachedCanCrawl = new bool?(value);
				}
				return this.cachedCanCrawl.Value;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002F06 RID: 12038 RVA: 0x000F1A5C File Offset: 0x000EFC5C
		public float HungerRateFactor
		{
			get
			{
				return this.GetHungerRateFactor(null);
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002F07 RID: 12039 RVA: 0x000F1A68 File Offset: 0x000EFC68
		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002F08 RID: 12040 RVA: 0x000F1AF4 File Offset: 0x000EFCF4
		public float? OverrideMoodBase
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].CurStage != null && this.hediffs[i].CurStage.overrideMoodBase >= 0f)
					{
						return new float?(this.hediffs[i].CurStage.overrideMoodBase);
					}
				}
				return null;
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000F1B6C File Offset: 0x000EFD6C
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000F1BF4 File Offset: 0x000EFDF4
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.hediffs.RemoveAll(delegate(Hediff hediff)
				{
					if (((hediff != null) ? hediff.def : null) != null)
					{
						return false;
					}
					Log.Error(hediff.ToStringSafe<Hediff>() + " on " + this.pawn.ToStringSafe<Pawn>() + " had a null def.");
					return true;
				});
				this.DirtyCache();
			}
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000F1CC0 File Offset: 0x000EFEC0
		public float FactorForDamage(DamageInfo dinfo)
		{
			if (dinfo.Def == null || this.hediffs.NullOrEmpty<Hediff>())
			{
				return 1f;
			}
			float result;
			if (this.cachedDamageFactors.TryGetValue(dinfo.Def, out result))
			{
				return result;
			}
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].CurStage != null && !this.hediffs[i].CurStage.damageFactors.NullOrEmpty<DamageFactor>())
				{
					for (int j = 0; j < this.hediffs[i].CurStage.damageFactors.Count; j++)
					{
						DamageFactor damageFactor = this.hediffs[i].CurStage.damageFactors[j];
						if (damageFactor.damageDef == dinfo.Def)
						{
							num *= damageFactor.factor;
						}
					}
				}
			}
			this.cachedDamageFactors.Add(dinfo.Def, num);
			return num;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000F1DC5 File Offset: 0x000EFFC5
		public bool HasBodyPart(BodyPartRecord part)
		{
			return this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000F1DD8 File Offset: 0x000EFFD8
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.");
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				string str = "Tried to add health diff to missing part ";
				BodyPartRecord part = hediff.Part;
				Log.Error(str + ((part != null) ? part.ToString() : null));
				return;
			}
			hediff.ageTicks = 0;
			hediff.pawn = this.pawn;
			bool flag = false;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TryMergeWith(hediff))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.hediffs.Add(hediff);
				hediff.PostAdd(dinfo);
				if (this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			bool flag2 = hediff is Hediff_MissingPart;
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
			{
				bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = !flag3;
				hediff_MissingPart.lastInjury = hediff.def;
				this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
				if (damageResult != null)
				{
					damageResult.AddHediff(hediff_MissingPart);
				}
				if (flag3)
				{
					if (dinfo != null)
					{
						hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
					}
					else
					{
						hediff_MissingPart.lastInjury = null;
					}
				}
				flag2 = true;
			}
			this.DirtyCache();
			if (flag2 && this.pawn.apparel != null)
			{
				this.pawn.apparel.Notify_LostBodyPart();
			}
			HediffStage curStage = hediff.CurStage;
			if (hediff.def.chemicalNeed != null && !this.pawn.Dead)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			else if (curStage != null && (!curStage.enablesNeeds.NullOrEmpty<NeedDef>() || !curStage.disablesNeeds.NullOrEmpty<NeedDef>()) && !this.pawn.Dead)
			{
				Pawn_NeedsTracker needs2 = this.pawn.needs;
				if (needs2 != null)
				{
					needs2.AddOrRemoveNeedsAsAppropriate();
				}
			}
			if (hediff.def.HasDefinedGraphicProperties || hediff.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000F20B8 File Offset: 0x000F02B8
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.CacheThoughtsNullified();
			this.CacheNeeds();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.cachedCanCrawl = null;
			this.cachedHasRegeneration = null;
			this.cachedDamageFactors.Clear();
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000F2182 File Offset: 0x000F0382
		public void DirtyPainCache()
		{
			this.cachedPain = -1f;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000F2190 File Offset: 0x000F0390
		public void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			for (int i = this.hediffs.Count - 1; i >= 0; i--)
			{
				this.hediffs[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000F21C8 File Offset: 0x000F03C8
		public void Notify_Regenerated(float amount)
		{
			foreach (Hediff hediff in this.hediffs)
			{
				hediff.Notify_Regenerated(amount);
			}
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000F221C File Offset: 0x000F041C
		public float GetHungerRateFactor(HediffDef ignore = null)
		{
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def != ignore)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j].def != ignore)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000F22D0 File Offset: 0x000F04D0
		public int GetHediffCount(HediffDef def)
		{
			int num = 0;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000F2310 File Offset: 0x000F0510
		public void GetHediffs<T>(ref List<T> resultHediffs, Predicate<T> filter = null) where T : Hediff
		{
			resultHediffs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && (filter == null || filter(t)))
				{
					resultHediffs.Add(t);
				}
			}
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000F2370 File Offset: 0x000F0570
		public Hediff GetMostRecentHediff(HediffDef def)
		{
			Hediff hediff = null;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (hediff == null || this.hediffs[i].ageTicks < hediff.ageTicks))
				{
					hediff = this.hediffs[i];
				}
			}
			return hediff;
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000F23D4 File Offset: 0x000F05D4
		public T GetFirstHediff<T>() where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x000F2424 File Offset: 0x000F0624
		public bool TryGetHediff(HediffDef def, out Hediff hediff)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					hediff = this.hediffs[i];
					return true;
				}
			}
			hediff = null;
			return false;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000F2470 File Offset: 0x000F0670
		public bool TryGetHediff<T>(out T hediff) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					hediff = t;
					return true;
				}
			}
			hediff = default(T);
			return false;
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000F24C4 File Offset: 0x000F06C4
		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000F251F File Offset: 0x000F071F
		public IEnumerable<T> GetHediffComps<T>() where T : HediffComp
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				T t;
				if (this.hediffs[i].TryGetComp(out t))
				{
					yield return t;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000F2530 File Offset: 0x000F0730
		public T GetFirstHediffMatchingPart<T>(BodyPartRecord part) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && t.Part == part)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000F258C File Offset: 0x000F078C
		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000F25DC File Offset: 0x000F07DC
		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						if (hediff_Injury.destroysBodyParts)
						{
							num -= hediff_Injury.Severity;
						}
						else
						{
							num = Mathf.Max(num - hediff_Injury.Severity, 1f);
						}
					}
				}
			}
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000F26C8 File Offset: 0x000F08C8
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x000F2730 File Offset: 0x000F0930
		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000F2780 File Offset: 0x000F0980
		public bool HasHediff<T>(bool mustBeVisible = false) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is T && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000F27CF File Offset: 0x000F09CF
		public bool TryGetNeedEnablingHediff(NeedDef def, out Hediff hediff)
		{
			if (this.cachedEnabledNeeds == null)
			{
				hediff = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out hediff);
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000F27EB File Offset: 0x000F09EB
		public bool TryGetNeedDisablingHediff(NeedDef def, out Hediff hediff)
		{
			if (this.cachedDisabledNeeds == null)
			{
				hediff = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out hediff);
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000F2807 File Offset: 0x000F0A07
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000F281B File Offset: 0x000F0A1B
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000F2830 File Offset: 0x000F0A30
		public bool HasHediffOrWillBecome(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!mustBeVisible || this.hediffs[i].Visible)
				{
					if (this.hediffs[i].def == def)
					{
						return true;
					}
					if (this.CheckHediffDefAndChildren(def, this.hediffs[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000F289C File Offset: 0x000F0A9C
		private bool CheckHediffDefAndChildren(HediffDef toFind, HediffDef hediff)
		{
			if (hediff == toFind)
			{
				return true;
			}
			if (!hediff.comps.NullOrEmpty<HediffCompProperties>())
			{
				foreach (HediffCompProperties hediffCompProperties in hediff.comps)
				{
					HediffCompProperties_ReplaceHediff hediffCompProperties_ReplaceHediff = hediffCompProperties as HediffCompProperties_ReplaceHediff;
					if (hediffCompProperties_ReplaceHediff != null)
					{
						foreach (HediffCompProperties_ReplaceHediff.TriggeredHediff triggeredHediff in hediffCompProperties_ReplaceHediff.hediffs)
						{
							if (this.CheckHediffDefAndChildren(toFind, triggeredHediff.hediff))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000F2958 File Offset: 0x000F0B58
		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000F29BC File Offset: 0x000F0BBC
		public bool TryGetBodyPartRecord(BodyPartDef def, out BodyPartRecord record)
		{
			record = this.GetBodyPartRecord(def);
			return record != null;
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000F29CC File Offset: 0x000F0BCC
		public BodyPartRecord GetBodyPartRecord(BodyPartDef partDef)
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def == partDef)
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000F2A28 File Offset: 0x000F0C28
		public bool IsBionicOrImplant(BodyPartDef partDef)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				if (hediff.Part != null && hediff.Part.def == partDef && hediff.def.countsAsAddedPartOrImplant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000F2A80 File Offset: 0x000F0C80
		public List<Verb> GetHediffsVerbs()
		{
			this.tmpHediffVerbs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				HediffComp_VerbGiver hediffComp_VerbGiver = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (hediffComp_VerbGiver != null)
				{
					List<Verb> allVerbs = hediffComp_VerbGiver.VerbTracker.AllVerbs;
					for (int j = 0; j < allVerbs.Count; j++)
					{
						this.tmpHediffVerbs.Add(allVerbs[j]);
					}
				}
			}
			return this.tmpHediffVerbs;
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000F2AF8 File Offset: 0x000F0CF8
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000F2B08 File Offset: 0x000F0D08
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000F2B60 File Offset: 0x000F0D60
		public bool HasHediffBlocksSleeping()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.blocksSleeping)
				{
					return true;
				}
				HediffStage curStage = this.hediffs[i].CurStage;
				if (curStage != null && curStage.blocksSleeping)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000F2BC0 File Offset: 0x000F0DC0
		public bool HasHediffPreventsPregnancy()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.preventsPregnancy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000F2C00 File Offset: 0x000F0E00
		public bool HasPregnancyHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.pregnant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000F2C3E File Offset: 0x000F0E3E
		public IEnumerable<HediffComp> GetAllComps()
		{
			foreach (Hediff hediff in this.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						yield return hediffComp;
					}
					List<HediffComp>.Enumerator enumerator2 = default(List<HediffComp>.Enumerator);
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000F2C50 File Offset: 0x000F0E50
		public bool HasHediffShowGizmosOnCorpse()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.showGizmosOnCorpse)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000F2C90 File Offset: 0x000F0E90
		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000F2CD4 File Offset: 0x000F0ED4
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000F2D24 File Offset: 0x000F0F24
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000F2D90 File Offset: 0x000F0F90
		public List<BodyPartRecord> GetInjuredParts()
		{
			HediffSet.tmpInjuredParts.Clear();
			foreach (Hediff hediff in this.hediffs)
			{
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				if (hediff_Injury != null && !HediffSet.tmpInjuredParts.Contains(hediff_Injury.Part))
				{
					HediffSet.tmpInjuredParts.Add(hediff_Injury.Part);
				}
			}
			return HediffSet.tmpInjuredParts;
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000F2E18 File Offset: 0x000F1018
		public List<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			HediffSet.tmpNaturallyHealingInjuredParts.Clear();
			foreach (BodyPartRecord bodyPartRecord in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && this.hediffs[i].Part == bodyPartRecord && hediff_Injury.CanHealNaturally())
					{
						HediffSet.tmpNaturallyHealingInjuredParts.Add(bodyPartRecord);
						break;
					}
				}
			}
			return HediffSet.tmpNaturallyHealingInjuredParts;
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000F2EC8 File Offset: 0x000F10C8
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000F2EE0 File Offset: 0x000F10E0
		public bool HasMissingPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000F2F30 File Offset: 0x000F1130
		public Hediff GetMissingPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000F2F88 File Offset: 0x000F1188
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			int num;
			for (int i = 0; i < allPartsList.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = allPartsList[i];
				if (!this.PartIsMissing(bodyPartRecord) && (height == BodyPartHeight.Undefined || bodyPartRecord.height == height) && (depth == BodyPartDepth.Undefined || bodyPartRecord.depth == depth) && (tag == null || bodyPartRecord.def.tags.Contains(tag)) && (partParent == null || bodyPartRecord.parent == partParent))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000F2FB8 File Offset: 0x000F11B8
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000F3098 File Offset: 0x000F1298
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000F31F0 File Offset: 0x000F13F0
		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord bodyPartRecord = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord))
				{
					Hediff_MissingPart firstHediffMatchingPart = this.GetFirstHediffMatchingPart<Hediff_MissingPart>(bodyPartRecord);
					if (firstHediffMatchingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(firstHediffMatchingPart);
					}
					else
					{
						for (int i = 0; i < bodyPartRecord.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(bodyPartRecord.parts[i]);
						}
					}
				}
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000F32B8 File Offset: 0x000F14B8
		private void CacheNeeds()
		{
			Dictionary<NeedDef, Hediff> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Hediff> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				if (hediff.def.chemicalNeed != null)
				{
					if (this.cachedEnabledNeeds == null)
					{
						this.cachedEnabledNeeds = new Dictionary<NeedDef, Hediff>();
					}
					this.cachedEnabledNeeds[hediff.def.chemicalNeed] = hediff;
				}
				if (hediff.CurStage != null)
				{
					if (!hediff.CurStage.enablesNeeds.NullOrEmpty<NeedDef>())
					{
						for (int j = 0; j < hediff.CurStage.enablesNeeds.Count; j++)
						{
							NeedDef key = hediff.CurStage.enablesNeeds[j];
							if (this.cachedEnabledNeeds == null)
							{
								this.cachedEnabledNeeds = new Dictionary<NeedDef, Hediff>();
							}
							this.cachedEnabledNeeds[key] = hediff;
						}
					}
					if (!hediff.CurStage.disablesNeeds.NullOrEmpty<NeedDef>())
					{
						for (int k = 0; k < hediff.CurStage.disablesNeeds.Count; k++)
						{
							NeedDef key2 = hediff.CurStage.disablesNeeds[k];
							if (this.cachedDisabledNeeds == null)
							{
								this.cachedDisabledNeeds = new Dictionary<NeedDef, Hediff>();
							}
							this.cachedDisabledNeeds[key2] = hediff;
						}
					}
				}
			}
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000F3418 File Offset: 0x000F1618
		public void CacheThoughtsNullified()
		{
			this.cachedMaxPctConditionalThoughtsNullified = 0f;
			this.cachedMaxPctAllThoughtsNullified = 0f;
			this.cachedConditionalThoughtNullifyingHediff = null;
			this.cachedAllThoughtNullifyingHediff = null;
			foreach (Hediff hediff in this.hediffs)
			{
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					if (curStage.pctConditionalThoughtsNullified > this.cachedMaxPctConditionalThoughtsNullified)
					{
						this.cachedMaxPctConditionalThoughtsNullified = curStage.pctConditionalThoughtsNullified;
						this.cachedConditionalThoughtNullifyingHediff = hediff;
					}
					if (curStage.pctAllThoughtNullification > this.cachedMaxPctAllThoughtsNullified)
					{
						this.cachedMaxPctAllThoughtsNullified = curStage.pctAllThoughtNullification;
						this.cachedAllThoughtNullifyingHediff = hediff;
					}
				}
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000F34D4 File Offset: 0x000F16D4
		public Hediff ThoughtNullifyingHediff(ThoughtDef def)
		{
			if (def.nullifyingHediffs != null)
			{
				for (int i = 0; i < def.nullifyingHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(def.nullifyingHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						return firstHediffOfDef;
					}
				}
			}
			float num = 0f;
			Hediff result = null;
			if (!def.IsMemory)
			{
				num = this.cachedMaxPctConditionalThoughtsNullified;
				result = this.cachedConditionalThoughtNullifyingHediff;
			}
			if (this.cachedMaxPctAllThoughtsNullified > num)
			{
				num = this.cachedMaxPctAllThoughtsNullified;
				result = this.cachedAllThoughtNullifyingHediff;
			}
			if (num == 0f)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = this.pawn.thingIDNumber * 31 + (int)(def.index * 139);
			bool flag = Rand.Value < num;
			Rand.PopState();
			if (!flag)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000F35A0 File Offset: 0x000F17A0
		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000F35F0 File Offset: 0x000F17F0
		public Hediff GetDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000F3648 File Offset: 0x000F1848
		public bool TryGetDirectlyAddedPartFor(BodyPartRecord part, out Hediff hediff)
		{
			hediff = this.GetDirectlyAddedPartFor(part);
			return hediff != null;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000F3658 File Offset: 0x000F1858
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000F367E File Offset: 0x000F187E
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000F3699 File Offset: 0x000F1899
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000F36AC File Offset: 0x000F18AC
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000F3728 File Offset: 0x000F1928
		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x000F37B4 File Offset: 0x000F19B4
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000F3805 File Offset: 0x000F1A05
		public bool InLabor(bool includePostpartumExhaustion = true)
		{
			return ModsConfig.BiotechActive && (this.HasHediff(HediffDefOf.PregnancyLabor, false) || this.HasHediff(HediffDefOf.PregnancyLaborPushing, false) || (includePostpartumExhaustion && this.HasHediff(HediffDefOf.PostpartumExhaustion, false)));
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000F3842 File Offset: 0x000F1A42
		public bool IsPsychicallySuppressed
		{
			get
			{
				return ModsConfig.RoyaltyActive && this.HasHediff(HediffDefOf.PsychicSuppression, false);
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000F385C File Offset: 0x000F1A5C
		private float CalculateBleedRate()
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.pawn.Deathresting)
			{
				return 0f;
			}
			if (!this.pawn.health.CanBleed)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					num *= curStage.totalBleedFactor;
				}
				num2 += hediff.BleedRate;
			}
			return num2 * num / this.pawn.HealthScale;
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000F390C File Offset: 0x000F1B0C
		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].PainOffset;
			}
			if (this.pawn.genes != null)
			{
				num += this.pawn.genes.PainOffset;
			}
			Pawn_StoryTracker story = this.pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				List<Trait> allTraits = this.pawn.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					num += allTraits[j].CurrentData.painOffset;
				}
			}
			for (int k = 0; k < this.hediffs.Count; k++)
			{
				num *= this.hediffs[k].PainFactor;
			}
			if (this.pawn.genes != null)
			{
				num *= this.pawn.genes.PainFactor;
			}
			Pawn_StoryTracker story2 = this.pawn.story;
			if (((story2 != null) ? story2.traits : null) != null)
			{
				List<Trait> allTraits2 = this.pawn.story.traits.allTraits;
				for (int l = 0; l < allTraits2.Count; l++)
				{
					num *= allTraits2[l].CurrentData.painFactor;
				}
			}
			return Mathf.Clamp(num, 0f, 1f);
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000F3AA0 File Offset: 0x000F1CA0
		public Color GetSkinColor(Color baseColor)
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.def.skinColorTint != null)
				{
					baseColor = baseColor * (1f - hediff.def.skinColorTintStrength) + hediff.def.skinColorTint.Value * hediff.def.skinColorTintStrength;
				}
				if (hediff.def.skinColorOverride != null)
				{
					return hediff.def.skinColorOverride.Value;
				}
			}
			return baseColor;
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000F3B78 File Offset: 0x000F1D78
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		// Token: 0x040024BE RID: 9406
		public Pawn pawn;

		// Token: 0x040024BF RID: 9407
		public List<Hediff> hediffs = new List<Hediff>();

		// Token: 0x040024C0 RID: 9408
		private Dictionary<DamageDef, float> cachedDamageFactors = new Dictionary<DamageDef, float>();

		// Token: 0x040024C1 RID: 9409
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		// Token: 0x040024C2 RID: 9410
		private Dictionary<NeedDef, Hediff> cachedEnabledNeeds;

		// Token: 0x040024C3 RID: 9411
		private Dictionary<NeedDef, Hediff> cachedDisabledNeeds;

		// Token: 0x040024C4 RID: 9412
		private float cachedPain = -1f;

		// Token: 0x040024C5 RID: 9413
		private float cachedBleedRate = -1f;

		// Token: 0x040024C6 RID: 9414
		private bool? cachedHasHead;

		// Token: 0x040024C7 RID: 9415
		private bool? cachedCanCrawl;

		// Token: 0x040024C8 RID: 9416
		private bool? cachedHasRegeneration;

		// Token: 0x040024C9 RID: 9417
		private List<Verb> tmpHediffVerbs = new List<Verb>();

		// Token: 0x040024CA RID: 9418
		private static List<BodyPartRecord> tmpInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040024CB RID: 9419
		private static List<BodyPartRecord> tmpNaturallyHealingInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040024CC RID: 9420
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		// Token: 0x040024CD RID: 9421
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		// Token: 0x040024CE RID: 9422
		private readonly Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();

		// Token: 0x040024CF RID: 9423
		private float cachedMaxPctConditionalThoughtsNullified = -1f;

		// Token: 0x040024D0 RID: 9424
		private Hediff cachedConditionalThoughtNullifyingHediff;

		// Token: 0x040024D1 RID: 9425
		private float cachedMaxPctAllThoughtsNullified = -1f;

		// Token: 0x040024D2 RID: 9426
		private Hediff cachedAllThoughtNullifyingHediff;
	}
}
