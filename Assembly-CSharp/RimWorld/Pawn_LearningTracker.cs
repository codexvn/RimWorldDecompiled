using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020024C4 RID: 9412
	public class Pawn_LearningTracker : IExposable
	{
		// Token: 0x17001F4C RID: 8012
		// (get) Token: 0x0600CCE8 RID: 52456 RVA: 0x003C1865 File Offset: 0x003BFA65
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001F4D RID: 8013
		// (get) Token: 0x0600CCE9 RID: 52457 RVA: 0x003C186D File Offset: 0x003BFA6D
		public List<LearningDesireDef> ActiveLearningDesires
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600CCEA RID: 52458 RVA: 0x003C1875 File Offset: 0x003BFA75
		public Pawn_LearningTracker()
		{
		}

		// Token: 0x0600CCEB RID: 52459 RVA: 0x003C1888 File Offset: 0x003BFA88
		public Pawn_LearningTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CCEC RID: 52460 RVA: 0x003C18A4 File Offset: 0x003BFAA4
		public void AddNewLearningDesire()
		{
			LearningDesireDef item = (from ld in DefDatabase<LearningDesireDef>.AllDefsListForReading
			where !this.active.Contains(ld) && ld.Worker.CanGiveDesire
			select ld).RandomElementByWeight((LearningDesireDef ld) => ld.selectionWeight);
			if (this.active.Count >= 2)
			{
				this.active.RemoveAt(0);
			}
			this.active.Add(item);
		}

		// Token: 0x0600CCED RID: 52461 RVA: 0x003C1912 File Offset: 0x003BFB12
		public void LearningTickInterval(int delta)
		{
			this.newLearningDesireTicksLeft -= delta;
			if (this.newLearningDesireTicksLeft <= 0)
			{
				this.AddNewLearningDesire();
				this.newLearningDesireTicksLeft = Rand.Range(20000, 40000);
			}
		}

		// Token: 0x0600CCEE RID: 52462 RVA: 0x003C1946 File Offset: 0x003BFB46
		public void Debug_SetLearningDesire(LearningDesireDef desire)
		{
			this.active.Clear();
			this.active.Add(desire);
		}

		// Token: 0x0600CCEF RID: 52463 RVA: 0x003C195F File Offset: 0x003BFB5F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.newLearningDesireTicksLeft, "newLearningDesireTicksLeft", 0, false);
			Scribe_Collections.Look<LearningDesireDef>(ref this.active, "active", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04008B8F RID: 35727
		private const int MaxActiveLearningDesires = 2;

		// Token: 0x04008B90 RID: 35728
		private const int MinLearningDesireIntervalTicks = 20000;

		// Token: 0x04008B91 RID: 35729
		private const int MaxLearningDesireIntervalTicks = 40000;

		// Token: 0x04008B92 RID: 35730
		private Pawn pawn;

		// Token: 0x04008B93 RID: 35731
		private List<LearningDesireDef> active = new List<LearningDesireDef>();

		// Token: 0x04008B94 RID: 35732
		private int newLearningDesireTicksLeft;
	}
}
