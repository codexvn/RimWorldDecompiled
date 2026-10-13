using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200292D RID: 10541
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x1700230A RID: 8970
		// (get) Token: 0x0600E6B1 RID: 59057 RVA: 0x0043449B File Offset: 0x0043269B
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x1700230B RID: 8971
		// (get) Token: 0x0600E6B2 RID: 59058 RVA: 0x004344A8 File Offset: 0x004326A8
		public override string ShortSummary
		{
			get
			{
				string str = this.def.LabelCap + ": ";
				Name name = this.firstPawnData.name;
				string text = str + ((name != null) ? name.ToString() : null);
				if (this.secondPawnData != null)
				{
					string str2 = text;
					string str3 = ", ";
					Name name2 = this.secondPawnData.name;
					text = str2 + str3 + ((name2 != null) ? name2.ToString() : null);
				}
				return text;
			}
		}

		// Token: 0x0600E6B3 RID: 59059 RVA: 0x00434518 File Offset: 0x00432718
		public Tale_DoublePawn()
		{
		}

		// Token: 0x0600E6B4 RID: 59060 RVA: 0x00434520 File Offset: 0x00432720
		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		// Token: 0x0600E6B5 RID: 59061 RVA: 0x0043456D File Offset: 0x0043276D
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x0600E6B6 RID: 59062 RVA: 0x004345A0 File Offset: 0x004327A0
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.firstPawnData.faction == faction)
			{
				this.firstPawnData.faction = null;
			}
			if (this.secondPawnData != null && this.secondPawnData.faction == faction)
			{
				this.secondPawnData.faction = null;
			}
		}

		// Token: 0x0600E6B7 RID: 59063 RVA: 0x004345DE File Offset: 0x004327DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", Array.Empty<object>());
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", Array.Empty<object>());
		}

		// Token: 0x0600E6B8 RID: 59064 RVA: 0x00434610 File Offset: 0x00432810
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants = null)
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				TaleDef def = this.def;
				Log.Error(((def != null) ? def.ToString() : null) + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set");
			}
			foreach (Rule rule in this.firstPawnData.GetRules("ANYPAWN", null))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol, outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			if (this.secondPawnData != null)
			{
				foreach (Rule rule3 in this.firstPawnData.GetRules("ANYPAWN", null))
				{
					yield return rule3;
				}
				enumerator = null;
				foreach (Rule rule4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol, outConstants))
				{
					yield return rule4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E6B9 RID: 59065 RVA: 0x00434627 File Offset: 0x00432827
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom(false);
			this.secondPawnData = TaleData_Pawn.GenerateRandom(false);
		}

		// Token: 0x04009A40 RID: 39488
		public TaleData_Pawn firstPawnData;

		// Token: 0x04009A41 RID: 39489
		public TaleData_Pawn secondPawnData;
	}
}
