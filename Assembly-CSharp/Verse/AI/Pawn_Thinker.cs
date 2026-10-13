using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FCC RID: 4044
	public class Pawn_Thinker : IExposable
	{
		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060063D3 RID: 25555 RVA: 0x001FAD98 File Offset: 0x001F8F98
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				Pawn_MutantTracker mutant = this.pawn.mutant;
				ThinkTreeDef result;
				if ((result = ((mutant != null) ? mutant.GetThinkTrees().main : null)) == null)
				{
					LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
					result = (((curLifeStage != null) ? curLifeStage.thinkTreeMainOverride : null) ?? this.pawn.RaceProps.thinkTreeMain);
				}
				return result;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x060063D4 RID: 25556 RVA: 0x001FADF5 File Offset: 0x001F8FF5
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.MainThinkTree.thinkRoot;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060063D5 RID: 25557 RVA: 0x001FAE04 File Offset: 0x001F9004
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				Pawn_MutantTracker mutant = this.pawn.mutant;
				ThinkTreeDef result;
				if ((result = ((mutant != null) ? mutant.GetThinkTrees().constant : null)) == null)
				{
					LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
					result = (((curLifeStage != null) ? curLifeStage.thinkTreeConstantOverride : null) ?? this.pawn.RaceProps.thinkTreeConstant);
				}
				return result;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060063D6 RID: 25558 RVA: 0x001FAE61 File Offset: 0x001F9061
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.ConstantThinkTree.thinkRoot;
			}
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x001FAE6E File Offset: 0x001F906E
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x001FAE7D File Offset: 0x001F907D
		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			return this.MainThinkNodeRoot.FirstNodeOfType<T>(false);
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x001FAE8C File Offset: 0x001F908C
		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T t = this.TryGetMainTreeThinkNode<T>();
			if (t == null)
			{
				Pawn pawn = this.pawn;
				string str = (pawn != null) ? pawn.ToString() : null;
				string str2 = " looked for ThinkNode of type ";
				Type typeFromHandle = typeof(T);
				Log.Warning(str + str2 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " and didn't find it.");
			}
			return t;
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x000026EA File Offset: 0x000008EA
		public void ExposeData()
		{
		}

		// Token: 0x040047AB RID: 18347
		public Pawn pawn;
	}
}
