using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002589 RID: 9609
	public class FactionIdeosTracker : IExposable
	{
		// Token: 0x17002066 RID: 8294
		// (get) Token: 0x0600D26B RID: 53867 RVA: 0x003DFD63 File Offset: 0x003DDF63
		public List<Ideo> IdeosMinorListForReading
		{
			get
			{
				return this.ideosMinor;
			}
		}

		// Token: 0x17002067 RID: 8295
		// (get) Token: 0x0600D26C RID: 53868 RVA: 0x003DFD6B File Offset: 0x003DDF6B
		public Ideo PrimaryIdeo
		{
			get
			{
				return this.primaryIdeo;
			}
		}

		// Token: 0x17002068 RID: 8296
		// (get) Token: 0x0600D26D RID: 53869 RVA: 0x003DFD73 File Offset: 0x003DDF73
		public IEnumerable<Ideo> AllIdeos
		{
			get
			{
				if (this.primaryIdeo != null)
				{
					yield return this.primaryIdeo;
				}
				int num;
				for (int i = 0; i < this.ideosMinor.Count; i = num + 1)
				{
					yield return this.ideosMinor[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17002069 RID: 8297
		// (get) Token: 0x0600D26E RID: 53870 RVA: 0x003DFD83 File Offset: 0x003DDF83
		public CultureDef PrimaryCulture
		{
			get
			{
				if (this.PrimaryIdeo == null)
				{
					return null;
				}
				return this.PrimaryIdeo.culture;
			}
		}

		// Token: 0x1700206A RID: 8298
		// (get) Token: 0x0600D26F RID: 53871 RVA: 0x003DFD9C File Offset: 0x003DDF9C
		public Ideo FluidIdeo
		{
			get
			{
				if (this.primaryIdeo != null && this.primaryIdeo.Fluid)
				{
					return this.primaryIdeo;
				}
				for (int i = 0; i < this.ideosMinor.Count; i++)
				{
					if (this.ideosMinor[i].Fluid)
					{
						return this.ideosMinor[i];
					}
				}
				return null;
			}
		}

		// Token: 0x0600D270 RID: 53872 RVA: 0x003DFDFC File Offset: 0x003DDFFC
		public FactionIdeosTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x0600D271 RID: 53873 RVA: 0x003DFE16 File Offset: 0x003DE016
		public FactionIdeosTracker()
		{
		}

		// Token: 0x0600D272 RID: 53874 RVA: 0x003DFE29 File Offset: 0x003DE029
		public bool Has(Ideo ideo)
		{
			return ideo == this.primaryIdeo || this.ideosMinor.Contains(ideo);
		}

		// Token: 0x0600D273 RID: 53875 RVA: 0x003DFE44 File Offset: 0x003DE044
		public bool HasAnyIdeoWithMeme(MemeDef meme)
		{
			if (this.primaryIdeo.memes.Contains(meme))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = this.ideosMinor.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.memes.Contains(meme))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D274 RID: 53876 RVA: 0x003DFEB8 File Offset: 0x003DE0B8
		public bool IsPrimary(Ideo ideo)
		{
			return ideo == this.primaryIdeo;
		}

		// Token: 0x0600D275 RID: 53877 RVA: 0x003DFEC3 File Offset: 0x003DE0C3
		public bool IsMinor(Ideo ideo)
		{
			return this.ideosMinor.Contains(ideo);
		}

		// Token: 0x0600D276 RID: 53878 RVA: 0x003DFED1 File Offset: 0x003DE0D1
		public void Notify_MemberGainedOrLost()
		{
			if (this.faction.IsPlayer)
			{
				this.RecalculateIdeosBasedOnPlayerPawns();
			}
		}

		// Token: 0x0600D277 RID: 53879 RVA: 0x003DFEE6 File Offset: 0x003DE0E6
		public void Notify_ColonistChangedIdeo()
		{
			this.RecalculateIdeosBasedOnPlayerPawns();
		}

		// Token: 0x0600D278 RID: 53880 RVA: 0x003DFEF0 File Offset: 0x003DE0F0
		public void RecalculateIdeosBasedOnPlayerPawns()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return;
			}
			this.ideosMinor.Clear();
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count; i++)
			{
				if (allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].HomeFaction == Faction.OfPlayer)
				{
					Ideo ideo = allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].Ideo;
					if (ideo != null)
					{
						FactionIdeosTracker.tmpPlayerIdeos.Increment(ideo);
					}
				}
			}
			int num = 0;
			Ideo ideo2 = null;
			foreach (KeyValuePair<Ideo, int> keyValuePair in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
					ideo2 = keyValuePair.Key;
				}
			}
			if (ideo2 != null && num > FactionIdeosTracker.tmpPlayerIdeos.TryGetValue(this.primaryIdeo, 0))
			{
				if (this.primaryIdeo != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO")), "LetterNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO"), Faction.OfPlayer.Named("FACTION")), LetterDefOf.NeutralEvent, null, 0, true);
					this.primaryIdeo.Notify_NotPrimaryAnymore(ideo2);
				}
				if (this.primaryIdeo != null && ideo2.ColonistBelieverCountCached < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsActivated".Translate(ideo2), "LetterTitleObligationsActivatedIdeoBecameMajor".Translate(ideo2.Named("IDEO")), LetterDefOf.NeutralEvent, null, 0, true);
				}
				this.primaryIdeo = ideo2;
			}
			foreach (KeyValuePair<Ideo, int> keyValuePair2 in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair2.Key != this.primaryIdeo && !this.ideosMinor.Contains(keyValuePair2.Key))
				{
					this.ideosMinor.Add(keyValuePair2.Key);
				}
			}
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
		}

		// Token: 0x0600D279 RID: 53881 RVA: 0x003E0140 File Offset: 0x003DE340
		public void SetPrimary(Ideo ideo)
		{
			this.primaryIdeo = ideo;
		}

		// Token: 0x0600D27A RID: 53882 RVA: 0x003E014C File Offset: 0x003DE34C
		public void ChooseOrGenerateIdeo(IdeoGenerationParms parms)
		{
			this.primaryIdeo = null;
			this.ideosMinor.Clear();
			if (!ModsConfig.IdeologyActive || parms.forceNoExpansionIdeo)
			{
				Ideo ideo = IdeoGenerator.GenerateNoExpansionIdeo((!this.faction.def.allowedCultures.NullOrEmpty<CultureDef>()) ? this.faction.def.allowedCultures[0] : DefDatabase<CultureDef>.AllDefs.RandomElement<CultureDef>(), parms);
				this.primaryIdeo = ideo;
				Find.IdeoManager.Add(ideo);
				return;
			}
			if (parms.fixedIdeo)
			{
				Ideo ideo2 = IdeoGenerator.MakeFixedIdeo(parms);
				ideo2.primaryFactionColor = new Color?(this.faction.Color);
				this.primaryIdeo = ideo2;
				Find.IdeoManager.Add(ideo2);
				return;
			}
			if (Find.IdeoManager.classicMode && Faction.OfPlayer != null && Faction.OfPlayer.ideos.PrimaryIdeo != null)
			{
				this.primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
				return;
			}
			if (!Rand.Chance(0.2f))
			{
				if (Find.IdeoManager.IdeosListForReading.Count((Ideo i) => !i.solid) < 10 && !this.faction.def.hidden)
				{
					goto IL_18C;
				}
			}
			Ideo ideo3;
			if ((from x in Find.IdeoManager.IdeosListForReading
			where IdeoUtility.CanUseIdeo(this.faction.def, x, parms) && !x.solid
			select x).TryRandomElement(out ideo3))
			{
				this.primaryIdeo = ideo3;
				return;
			}
			IL_18C:
			Ideo ideo4 = IdeoGenerator.GenerateIdeo(parms);
			ideo4.primaryFactionColor = new Color?(this.faction.Color);
			this.primaryIdeo = ideo4;
			Find.IdeoManager.Add(ideo4);
		}

		// Token: 0x0600D27B RID: 53883 RVA: 0x003E031E File Offset: 0x003DE51E
		public Ideo GetRandomIdeoForNewPawn()
		{
			return this.AllIdeos.RandomElementByWeightWithFallback(delegate(Ideo x)
			{
				if (!this.IsPrimary(x))
				{
					return 1f;
				}
				return 4f;
			}, null);
		}

		// Token: 0x0600D27C RID: 53884 RVA: 0x003E0338 File Offset: 0x003DE538
		public Precept GetPrecept(PreceptDef precept)
		{
			foreach (Ideo ideo in this.AllIdeos)
			{
				foreach (Precept precept2 in ideo.PreceptsListForReading)
				{
					if (precept2.def == precept)
					{
						return precept2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600D27D RID: 53885 RVA: 0x003E03C8 File Offset: 0x003DE5C8
		public bool AnyPreceptWithRequiredScars()
		{
			using (IEnumerator<Ideo> enumerator = this.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.RequiredScars > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D27E RID: 53886 RVA: 0x003E041C File Offset: 0x003DE61C
		public static IdeoGenerationParms IdeoGenerationParmsForFaction_BackCompatibility(FactionDef factionDef, bool forceNoExpansion = false)
		{
			IdeoGenerationParms result = new IdeoGenerationParms(factionDef, forceNoExpansion, null, null, null, false, false, false, false, "", null, null, false, "", false);
			if (factionDef.isPlayer)
			{
				result.disallowedPrecepts = (from x in DefDatabase<PreceptDef>.AllDefs
				where x.impact == PreceptImpact.High
				select x).ToList<PreceptDef>();
				result.disallowedMemes = (from x in DefDatabase<MemeDef>.AllDefs
				where !x.allowDuringTutorial
				select x).ToList<MemeDef>();
			}
			return result;
		}

		// Token: 0x0600D27F RID: 53887 RVA: 0x003E04BA File Offset: 0x003DE6BA
		public void RemoveAll()
		{
			this.primaryIdeo = null;
			this.ideosMinor.Clear();
		}

		// Token: 0x0600D280 RID: 53888 RVA: 0x003E04D0 File Offset: 0x003DE6D0
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.primaryIdeo, "primaryIdeo", false);
			Scribe_Collections.Look<Ideo>(ref this.ideosMinor, "ideosMinor", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideosMinor.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligion references were null after loading.");
				}
				if (this.primaryIdeo == null)
				{
					Log.Error("Faction had no ideoligions after loading. Adding random one.");
					Ideo ideo;
					if (Find.IdeoManager.IdeosListForReading.TryRandomElement(out ideo))
					{
						this.primaryIdeo = ideo;
					}
				}
				if (this.primaryIdeo != null && this.primaryIdeo.createdFromNoExpansionGame && ModsConfig.IdeologyActive)
				{
					IdeoFoundationDef ideoFoundationDef = DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>();
					this.primaryIdeo.foundation = (IdeoFoundation)Activator.CreateInstance(ideoFoundationDef.foundationClass);
					this.primaryIdeo.foundation.def = ideoFoundationDef;
					this.primaryIdeo.foundation.ideo = this.primaryIdeo;
					this.primaryIdeo.foundation.Init(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(this.faction.def, false));
					this.primaryIdeo.createdFromNoExpansionGame = false;
				}
			}
		}

		// Token: 0x04008EEB RID: 36587
		private Faction faction;

		// Token: 0x04008EEC RID: 36588
		private Ideo primaryIdeo;

		// Token: 0x04008EED RID: 36589
		private List<Ideo> ideosMinor = new List<Ideo>();

		// Token: 0x04008EEE RID: 36590
		private const float MajorIdeoSelectionWeight = 4f;

		// Token: 0x04008EEF RID: 36591
		private const float MinorIdeoSelectionWeight = 1f;

		// Token: 0x04008EF0 RID: 36592
		private const float ChanceToReuseExistingIdeo = 0.2f;

		// Token: 0x04008EF1 RID: 36593
		private const int MaxIdeos = 10;

		// Token: 0x04008EF2 RID: 36594
		private static Dictionary<Ideo, int> tmpPlayerIdeos = new Dictionary<Ideo, int>();
	}
}
