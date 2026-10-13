using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001A5A RID: 6746
	public class InteractionDef : Def
	{
		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600979C RID: 38812 RVA: 0x002C494F File Offset: 0x002C2B4F
		public InteractionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InteractionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.interaction = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x0600979D RID: 38813 RVA: 0x002C4981 File Offset: 0x002C2B81
		private Texture2D Symbol
		{
			get
			{
				if (this.symbolTex == null)
				{
					this.symbolTex = ContentFinder<Texture2D>.Get(this.symbol, true);
				}
				return this.symbolTex;
			}
		}

		// Token: 0x0600979E RID: 38814 RVA: 0x002C49AC File Offset: 0x002C2BAC
		public Texture2D GetSymbol(Faction initiatorFaction = null, Ideo initatorIdeo = null)
		{
			InteractionSymbolSource interactionSymbolSource = this.symbolSource;
			if (interactionSymbolSource != InteractionSymbolSource.InitiatorIdeo)
			{
				if (interactionSymbolSource != InteractionSymbolSource.InitiatorFaction)
				{
					return this.Symbol;
				}
				if (initiatorFaction == null)
				{
					return null;
				}
				return initiatorFaction.def.FactionIcon;
			}
			else
			{
				if (Find.IdeoManager.classicMode)
				{
					return this.Symbol;
				}
				if (initatorIdeo == null)
				{
					return null;
				}
				return initatorIdeo.Icon;
			}
		}

		// Token: 0x0600979F RID: 38815 RVA: 0x002C4A00 File Offset: 0x002C2C00
		public Color? GetSymbolColor(Faction initiatorFaction = null)
		{
			if (initiatorFaction != null && this.symbolSource == InteractionSymbolSource.InitiatorFaction)
			{
				return new Color?(initiatorFaction.Color);
			}
			return null;
		}

		// Token: 0x060097A0 RID: 38816 RVA: 0x002C4A2E File Offset: 0x002C2C2E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x040064EA RID: 25834
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x040064EB RID: 25835
		public ThingDef interactionMote;

		// Token: 0x040064EC RID: 25836
		public float socialFightBaseChance;

		// Token: 0x040064ED RID: 25837
		public ThoughtDef initiatorThought;

		// Token: 0x040064EE RID: 25838
		public SkillDef initiatorXpGainSkill;

		// Token: 0x040064EF RID: 25839
		public int initiatorXpGainAmount;

		// Token: 0x040064F0 RID: 25840
		public ThoughtDef recipientThought;

		// Token: 0x040064F1 RID: 25841
		public SkillDef recipientXpGainSkill;

		// Token: 0x040064F2 RID: 25842
		public int recipientXpGainAmount;

		// Token: 0x040064F3 RID: 25843
		public bool ignoreTimeSinceLastInteraction;

		// Token: 0x040064F4 RID: 25844
		[NoTranslate]
		private string symbol;

		// Token: 0x040064F5 RID: 25845
		public InteractionSymbolSource symbolSource;

		// Token: 0x040064F6 RID: 25846
		public RulePack logRulesInitiator;

		// Token: 0x040064F7 RID: 25847
		public RulePack logRulesRecipient;

		// Token: 0x040064F8 RID: 25848
		[Unsaved(false)]
		private InteractionWorker workerInt;

		// Token: 0x040064F9 RID: 25849
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
