using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000F5B RID: 3931
	public class MentalBreaker : IExposable
	{
		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06006133 RID: 24883 RVA: 0x001EC76D File Offset: 0x001EA96D
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5) * 0.14285715f;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06006134 RID: 24884 RVA: 0x001EC787 File Offset: 0x001EA987
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5) * 0.5714286f;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06006135 RID: 24885 RVA: 0x001EC7A1 File Offset: 0x001EA9A1
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5);
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06006136 RID: 24886 RVA: 0x001EC7B8 File Offset: 0x001EA9B8
		public bool CanDoRandomMentalBreaks
		{
			get
			{
				LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
				if (curLifeStage == null)
				{
					return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
				}
				return curLifeStage.canDoRandomMentalBreaks;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x001EC80D File Offset: 0x001EAA0D
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme && !this.Blocked;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06006138 RID: 24888 RVA: 0x001EC835 File Offset: 0x001EAA35
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor && !this.Blocked;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06006139 RID: 24889 RVA: 0x001EC865 File Offset: 0x001EAA65
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor && !this.Blocked;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x0600613A RID: 24890 RVA: 0x001EC89D File Offset: 0x001EAA9D
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f && !this.Blocked;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600613B RID: 24891 RVA: 0x001EC8D3 File Offset: 0x001EAAD3
		public bool Blocked
		{
			get
			{
				return this.pawn.mindState.mentalStateHandler.MentalBreaksBlocked();
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600613C RID: 24892 RVA: 0x001EC8EA File Offset: 0x001EAAEA
		public float CurMood
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return 0.5f;
				}
				return this.pawn.needs.mood.CurLevel;
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x0600613D RID: 24893 RVA: 0x001EC919 File Offset: 0x001EAB19
		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				foreach (MentalBreakDef mentalBreakDef in this.GetBreaksForIntensity(this.CurrentDesiredMoodBreakIntensity, this.ChooseAnomalyBreak, true))
				{
					yield return mentalBreakDef;
				}
				IEnumerator<MentalBreakDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x0600613E RID: 24894 RVA: 0x001EC92C File Offset: 0x001EAB2C
		private bool ChooseAnomalyBreak
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				if (this.pawn.IsCreepJoiner)
				{
					return true;
				}
				MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
				return levelDef != null && Rand.Chance(levelDef.anomalyMentalBreakChance);
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x0600613F RID: 24895 RVA: 0x001EC96C File Offset: 0x001EAB6C
		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				if (this.ticksBelowExtreme >= 2000)
				{
					return MentalBreakIntensity.Extreme;
				}
				if (this.ticksBelowMajor >= 2000)
				{
					return MentalBreakIntensity.Major;
				}
				if (this.ticksBelowMinor >= 2000)
				{
					return MentalBreakIntensity.Minor;
				}
				return MentalBreakIntensity.None;
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x000024DF File Offset: 0x000006DF
		public MentalBreaker()
		{
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x001EC99C File Offset: 0x001EAB9C
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x001EC9AB File Offset: 0x001EABAB
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
			this.ticksBelowMinor = 0;
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x001EC9C4 File Offset: 0x001EABC4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06006144 RID: 24900 RVA: 0x001ECA19 File Offset: 0x001EAC19
		private IEnumerable<MentalBreakDef> GetBreaksForIntensity(MentalBreakIntensity intensity, bool anomalyBreak, bool tryNonAnomalyBreakIfNoneAvailable = true)
		{
			if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
			{
				bool flag = false;
				HashSet<MentalBreakDef> cachedPossibleMentalBreaks = this.pawn.Ideo.cachedPossibleMentalBreaks;
				foreach (MentalBreakDef mentalBreakDef in cachedPossibleMentalBreaks)
				{
					if (mentalBreakDef.Worker.BreakCanOccur(this.pawn))
					{
						yield return mentalBreakDef;
						flag = true;
					}
				}
				HashSet<MentalBreakDef>.Enumerator enumerator = default(HashSet<MentalBreakDef>.Enumerator);
				if (flag)
				{
					yield break;
				}
			}
			while (intensity != MentalBreakIntensity.None)
			{
				MentalBreakIntensity intensityInner = intensity;
				IEnumerable<MentalBreakDef> enumerable = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
				where d.intensity == intensityInner && d.Worker.BreakCanOccur(this.pawn) && d.anomalousBreak == anomalyBreak
				select d;
				if (anomalyBreak && enumerable.EnumerableNullOrEmpty<MentalBreakDef>() && tryNonAnomalyBreakIfNoneAvailable)
				{
					enumerable = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
					where d.intensity == intensityInner && d.Worker.BreakCanOccur(this.pawn) && !d.anomalousBreak
					select d;
				}
				bool flag2 = false;
				foreach (MentalBreakDef mentalBreakDef2 in enumerable)
				{
					yield return mentalBreakDef2;
					flag2 = true;
				}
				IEnumerator<MentalBreakDef> enumerator2 = null;
				if (flag2)
				{
					yield break;
				}
				MentalBreakIntensity mentalBreakIntensity = intensity;
				intensity = (MentalBreakIntensity)(mentalBreakIntensity - MentalBreakIntensity.Minor);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x001ECA40 File Offset: 0x001EAC40
		public void MentalBreakerTickInterval(int delta)
		{
			if (this.ticksUntilCanDoMentalBreak > 0 && this.pawn.Awake())
			{
				this.ticksUntilCanDoMentalBreak -= delta;
			}
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150, delta))
			{
				if (!DebugSettings.enableRandomMentalStates)
				{
					return;
				}
				if (this.CurMood < this.BreakThresholdExtreme)
				{
					this.ticksBelowExtreme += 150;
				}
				else
				{
					this.ticksBelowExtreme = 0;
				}
				if (this.CurMood < this.BreakThresholdMajor)
				{
					this.ticksBelowMajor += 150;
				}
				else
				{
					this.ticksBelowMajor = 0;
				}
				if (this.CurMood < this.BreakThresholdMinor)
				{
					this.ticksBelowMinor += 150;
				}
				else
				{
					this.ticksBelowMinor = 0;
				}
				if (this.TestMoodMentalBreak())
				{
					if (AlwaysBerserkUtility.TryTriggerBerserkBloodRage(this.pawn) || AlwaysBerserkUtility.TryTriggerBerserkFrenzyInducer(this.pawn))
					{
						return;
					}
					if (this.TryDoRandomMoodCausedMentalBreak())
					{
						return;
					}
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int i = 0; i < allTraits.Count; i++)
					{
						if (!allTraits[i].Suppressed && allTraits[i].CurrentData.MentalStateGiver.CheckGive(this.pawn, 150))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x001ECBB8 File Offset: 0x001EADB8
		private bool TestMoodMentalBreak()
		{
			if (this.ticksUntilCanDoMentalBreak > 0)
			{
				return false;
			}
			if (this.ticksBelowExtreme > 2000)
			{
				return Rand.MTBEventOccurs(0.5f, 60000f, 150f);
			}
			if (this.ticksBelowMajor > 2000)
			{
				return Rand.MTBEventOccurs(0.8f, 60000f, 150f);
			}
			return this.ticksBelowMinor > 2000 && Rand.MTBEventOccurs(4f, 60000f, 150f);
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x001ECC38 File Offset: 0x001EAE38
		public bool TryDoRandomMoodCausedMentalBreak()
		{
			if (!this.CanHaveMentalBreak())
			{
				return false;
			}
			MentalBreakDef breakDef;
			if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out breakDef))
			{
				return false;
			}
			Thought thought = this.RandomFinalStraw();
			TaggedString taggedString = "MentalStateReason_Mood".Translate();
			if (thought != null)
			{
				taggedString += "\n\n" + "FinalStraw".Translate(thought.LabelCap);
			}
			return this.TryDoMentalBreak(taggedString, breakDef);
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x001ECCB4 File Offset: 0x001EAEB4
		public bool TryDoMentalBreak(string reason, MentalBreakDef breakDef)
		{
			return this.CanHaveMentalBreak() && breakDef.Worker.TryStart(this.pawn, reason, true);
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x001ECCD3 File Offset: 0x001EAED3
		public bool TryGetRandomMentalBreak(MentalBreakIntensity intensity, out MentalBreakDef breakDef)
		{
			return this.GetBreaksForIntensity(intensity, this.ChooseAnomalyBreak, true).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out breakDef);
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x001ECCF8 File Offset: 0x001EAEF8
		private bool CanHaveMentalBreak()
		{
			return this.CanDoRandomMentalBreaks && !this.pawn.Downed && this.pawn.Awake() && !this.pawn.InMentalState && (!this.pawn.IsMutant || !this.pawn.mutant.Def.preventsMentalBreaks) && (this.pawn.Faction == Faction.OfPlayer || this.CurrentDesiredMoodBreakIntensity == MentalBreakIntensity.Extreme) && !QuestUtility.AnyQuestDisablesRandomMoodCausedMentalBreaksFor(this.pawn) && !this.Blocked;
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x001ECD94 File Offset: 0x001EAF94
		private Thought RandomFinalStraw()
		{
			this.pawn.needs.mood.thoughts.GetAllMoodThoughts(MentalBreaker.tmpThoughts);
			float num = 0f;
			for (int i = 0; i < MentalBreaker.tmpThoughts.Count; i++)
			{
				float num2 = MentalBreaker.tmpThoughts[i].MoodOffset();
				if (num2 < num)
				{
					num = num2;
				}
			}
			float maxMoodOffset = num * 0.5f;
			Thought result;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight((Thought x) => -x.MoodOffset(), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x001ECE50 File Offset: 0x001EB050
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalBreak = 15000;
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x001ECE5D File Offset: 0x001EB05D
		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			switch (intensity)
			{
			case MentalBreakIntensity.Minor:
				return this.BreakThresholdMinor;
			case MentalBreakIntensity.Major:
				return this.BreakThresholdMajor;
			case MentalBreakIntensity.Extreme:
				return this.BreakThresholdExtreme;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x001ECE90 File Offset: 0x001EB090
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine("   ticksUntilCanDoMentalBreak=" + this.ticksUntilCanDoMentalBreak.ToString());
			stringBuilder.AppendLine("   ticksBelowExtreme=" + this.ticksBelowExtreme.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine("   ticksBelowSerious=" + this.ticksBelowMajor.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine("   ticksBelowMinor=" + this.ticksBelowMinor.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current desired mood break intensity: " + this.CurrentDesiredMoodBreakIntensity.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current possible mood breaks:");
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn, true)).Sum();
			foreach (MentalBreakDef mentalBreakDef in this.CurrentPossibleMoodBreaks)
			{
				float num2 = mentalBreakDef.Worker.CommonalityFor(this.pawn, true);
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				MentalBreakDef mentalBreakDef2 = mentalBreakDef;
				stringBuilder2.AppendLine(str + ((mentalBreakDef2 != null) ? mentalBreakDef2.ToString() : null) + "     " + (num2 / num).ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x001ED040 File Offset: 0x001EB240
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			MentalBreakIntensity currentDesiredMoodBreakIntensity = this.CurrentDesiredMoodBreakIntensity;
			StringBuilder stringBuilder2 = stringBuilder;
			Pawn pawn = this.pawn;
			stringBuilder2.AppendLine(((pawn != null) ? pawn.ToString() : null) + " current possible mood mental breaks:").AppendLine("CurrentDesiredMoodBreakIntensity: " + currentDesiredMoodBreakIntensity.ToString());
			foreach (MentalBreakDef mentalBreakDef in this.GetBreaksForIntensity(currentDesiredMoodBreakIntensity, false, true))
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str = "  ";
				MentalBreakDef mentalBreakDef2 = mentalBreakDef;
				stringBuilder3.AppendLine(str + ((mentalBreakDef2 != null) ? mentalBreakDef2.ToString() : null));
			}
			if (ModsConfig.AnomalyActive)
			{
				MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
				if (levelDef != null && levelDef.anomalyMentalBreakChance > 0f)
				{
					foreach (MentalBreakDef mentalBreakDef3 in this.GetBreaksForIntensity(currentDesiredMoodBreakIntensity, true, false))
					{
						StringBuilder stringBuilder4 = stringBuilder;
						string str2 = "  ";
						MentalBreakDef mentalBreakDef4 = mentalBreakDef3;
						stringBuilder4.AppendLine(str2 + ((mentalBreakDef4 != null) ? mentalBreakDef4.ToString() : null));
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04004601 RID: 17921
		private Pawn pawn;

		// Token: 0x04004602 RID: 17922
		private int ticksUntilCanDoMentalBreak;

		// Token: 0x04004603 RID: 17923
		private int ticksBelowExtreme;

		// Token: 0x04004604 RID: 17924
		private int ticksBelowMajor;

		// Token: 0x04004605 RID: 17925
		private int ticksBelowMinor;

		// Token: 0x04004606 RID: 17926
		private const int CheckInterval = 150;

		// Token: 0x04004607 RID: 17927
		private const float ExtremeBreakMTBDays = 0.5f;

		// Token: 0x04004608 RID: 17928
		private const float MajorBreakMTBDays = 0.8f;

		// Token: 0x04004609 RID: 17929
		private const float MinorBreakMTBDays = 4f;

		// Token: 0x0400460A RID: 17930
		private const int MinTicksBelowToBreak = 2000;

		// Token: 0x0400460B RID: 17931
		private const int MinTicksSinceRecoveryToBreak = 15000;

		// Token: 0x0400460C RID: 17932
		private const float MajorBreakMoodFraction = 0.5714286f;

		// Token: 0x0400460D RID: 17933
		private const float ExtremeBreakMoodFraction = 0.14285715f;

		// Token: 0x0400460E RID: 17934
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
