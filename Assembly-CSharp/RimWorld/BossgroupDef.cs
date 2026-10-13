using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0E RID: 6670
	public class BossgroupDef : Def
	{
		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06009668 RID: 38504 RVA: 0x002C03DF File Offset: 0x002BE5DF
		public BossgroupWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (BossgroupWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06009669 RID: 38505 RVA: 0x002C041F File Offset: 0x002BE61F
		public string LeaderDescription
		{
			get
			{
				return this.leaderDescription.Formatted(this.boss.kindDef.Named("LEADERKIND"));
			}
		}

		// Token: 0x0600966A RID: 38506 RVA: 0x002C0448 File Offset: 0x002BE648
		public int GetWaveIndex(int timesSummoned)
		{
			int num = timesSummoned;
			if (num >= this.waves.Count)
			{
				Rand.PushState(Gen.HashCombine<int>(Find.World.info.Seed, timesSummoned));
				num = Rand.Range(this.repeatWaveStartIndex, this.waves.Count);
				Rand.PopState();
			}
			return num;
		}

		// Token: 0x0600966B RID: 38507 RVA: 0x002C049C File Offset: 0x002BE69C
		public BossGroupWave GetWave(int index)
		{
			return this.waves[this.GetWaveIndex(index)];
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x002C04B0 File Offset: 0x002BE6B0
		public string GetWaveDescription(int waveIndex)
		{
			BossGroupWave wave = this.GetWave(this.GetWaveIndex(waveIndex));
			this.tmpEntries.Clear();
			string text = GenLabel.BestKindLabel(this.boss.kindDef, Gender.None, false, -1).CapitalizeFirst();
			if (!wave.bossApparel.NullOrEmpty<ThingDef>())
			{
				text = "BossWithApparel".Translate(text.Named("BOSS"), (from a in wave.bossApparel
				select a.label).ToCommaList(true, false).Named("APPAREL"));
			}
			this.tmpEntries.Add(text + " x" + wave.bossCount.ToString());
			foreach (PawnKindDefCount pawnKindDefCount in wave.escorts)
			{
				this.tmpEntries.Add(GenLabel.BestKindLabel(pawnKindDefCount.kindDef, Gender.None, false, -1).CapitalizeFirst() + " x" + pawnKindDefCount.count.ToString());
			}
			return this.tmpEntries.ToLineList("  - ");
		}

		// Token: 0x0600966D RID: 38509 RVA: 0x002C05F8 File Offset: 0x002BE7F8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.boss == null)
			{
				yield return "boss required for all bossgroups";
			}
			if (this.waves.NullOrEmpty<BossGroupWave>())
			{
				yield return "no waves defined.";
			}
			if (this.repeatWaveStartIndex >= this.waves.Count)
			{
				yield return "repeatWaveStartIndex must be lower than wave count.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040062B1 RID: 25265
		public BossDef boss;

		// Token: 0x040062B2 RID: 25266
		public List<BossGroupWave> waves = new List<BossGroupWave>();

		// Token: 0x040062B3 RID: 25267
		public int repeatWaveStartIndex;

		// Token: 0x040062B4 RID: 25268
		public Type workerClass = typeof(BossgroupWorker);

		// Token: 0x040062B5 RID: 25269
		[MustTranslate]
		public string leaderDescription;

		// Token: 0x040062B6 RID: 25270
		public QuestScriptDef quest;

		// Token: 0x040062B7 RID: 25271
		public ThingDef rewardDef;

		// Token: 0x040062B8 RID: 25272
		private BossgroupWorker workerInt;

		// Token: 0x040062B9 RID: 25273
		private List<string> tmpEntries = new List<string>();
	}
}
