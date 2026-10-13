using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253E RID: 9534
	public class Thought_IdeoLeaderResentment : Thought_Situational
	{
		// Token: 0x17002010 RID: 8208
		// (get) Token: 0x0600D097 RID: 53399 RVA: 0x003D57CB File Offset: 0x003D39CB
		public Pawn Leader
		{
			get
			{
				return this.pawn.Faction.leader;
			}
		}

		// Token: 0x17002011 RID: 8209
		// (get) Token: 0x0600D098 RID: 53400 RVA: 0x003D57DD File Offset: 0x003D39DD
		public override string LabelCap
		{
			get
			{
				return "IdeoLeaderDifferentIdeoThoughtLabel".Translate(this.Leader.Ideo.memberName);
			}
		}

		// Token: 0x17002012 RID: 8210
		// (get) Token: 0x0600D099 RID: 53401 RVA: 0x003D5803 File Offset: 0x003D3A03
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Leader.Ideo.memberName);
			}
		}
	}
}
