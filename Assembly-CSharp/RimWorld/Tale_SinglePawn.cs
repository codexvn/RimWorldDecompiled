using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002934 RID: 10548
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x17002312 RID: 8978
		// (get) Token: 0x0600E6E9 RID: 59113 RVA: 0x00434FD7 File Offset: 0x004331D7
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x17002313 RID: 8979
		// (get) Token: 0x0600E6EA RID: 59114 RVA: 0x00434FE4 File Offset: 0x004331E4
		public override string ShortSummary
		{
			get
			{
				string str = this.def.LabelCap + ": ";
				Name name = this.pawnData.name;
				return str + ((name != null) ? name.ToString() : null);
			}
		}

		// Token: 0x0600E6EB RID: 59115 RVA: 0x00434518 File Offset: 0x00432718
		public Tale_SinglePawn()
		{
		}

		// Token: 0x0600E6EC RID: 59116 RVA: 0x0043501C File Offset: 0x0043321C
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x0600E6ED RID: 59117 RVA: 0x0043504F File Offset: 0x0043324F
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x0600E6EE RID: 59118 RVA: 0x0043506A File Offset: 0x0043326A
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.pawnData.faction == faction)
			{
				this.pawnData.faction = null;
			}
		}

		// Token: 0x0600E6EF RID: 59119 RVA: 0x00435086 File Offset: 0x00433286
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		// Token: 0x0600E6F0 RID: 59120 RVA: 0x004350A3 File Offset: 0x004332A3
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in this.pawnData.GetRules("ANYPAWN", outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.pawnData.GetRules("PAWN", outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E6F1 RID: 59121 RVA: 0x004350BA File Offset: 0x004332BA
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom(false);
		}

		// Token: 0x04009A59 RID: 39513
		public TaleData_Pawn pawnData;
	}
}
