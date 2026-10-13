using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E74 RID: 3700
	public class ThingCountTracker : IExposable
	{
		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005D34 RID: 23860 RVA: 0x001DB1D0 File Offset: 0x001D93D0
		public IReadOnlyDictionary<ThingDef, List<ThingCountTracker.PawnCount>> ReadOnlyPairs
		{
			get
			{
				return this.stuffCount;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005D35 RID: 23861 RVA: 0x001DB1D8 File Offset: 0x001D93D8
		public Thing ParentThing
		{
			get
			{
				return (Thing)this.parent;
			}
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x001DB1E5 File Offset: 0x001D93E5
		public ThingCountTracker()
		{
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x001DB1F8 File Offset: 0x001D93F8
		public ThingCountTracker(IHaulEnroute parent)
		{
			this.parent = parent;
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x001DB214 File Offset: 0x001D9414
		public void Add(Pawn pawn, ThingDef stuff, int amount)
		{
			if (amount <= 0)
			{
				Log.Error(string.Format("{0} tried to add a negative value to the tracker on {1} ({2} {3})", new object[]
				{
					pawn.LabelShort,
					((Thing)this.parent).Label,
					stuff.label,
					amount
				}));
				return;
			}
			List<ThingCountTracker.PawnCount> list;
			if (this.stuffCount.TryGetValue(stuff, out list))
			{
				foreach (ThingCountTracker.PawnCount pawnCount in list)
				{
					if (pawnCount.pawn == pawn)
					{
						pawnCount.count += amount;
						return;
					}
				}
				this.stuffCount[stuff].Add(new ThingCountTracker.PawnCount
				{
					pawn = pawn,
					count = amount
				});
				return;
			}
			this.stuffCount[stuff] = new List<ThingCountTracker.PawnCount>
			{
				new ThingCountTracker.PawnCount
				{
					pawn = pawn,
					count = amount
				}
			};
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x001DB31C File Offset: 0x001D951C
		public void CopyReservations(ThingCountTracker other)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in other.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				ThingDef stuff = thingDef;
				foreach (ThingCountTracker.PawnCount pawnCount in list)
				{
					this.Add(pawnCount.pawn, stuff, pawnCount.count);
				}
			}
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x001DB3C4 File Offset: 0x001D95C4
		public int Get(ThingDef stuff, Pawn excludePawn = null)
		{
			List<ThingCountTracker.PawnCount> list;
			if (this.stuffCount.TryGetValue(stuff, out list))
			{
				int num = 0;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					ThingCountTracker.PawnCount pawnCount = list[i];
					if (pawnCount.pawn != excludePawn)
					{
						num += pawnCount.count;
					}
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x001DB414 File Offset: 0x001D9614
		public void ReleaseFor(Pawn pawn)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				List<ThingCountTracker.PawnCount> list2 = list;
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					if (list2[i].pawn == pawn)
					{
						list2.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x001DB4A0 File Offset: 0x001D96A0
		public bool CanCleanup()
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				using (List<ThingCountTracker.PawnCount>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.count > 0)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001DB540 File Offset: 0x001D9740
		public void InterruptEnroutePawns(Pawn exclude)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				List<ThingCountTracker.PawnCount> list2 = list;
				for (int i = 0; i < list2.Count; i++)
				{
					Pawn pawn = list2[i].pawn;
					if (exclude != pawn)
					{
						for (int j = pawn.jobs.jobQueue.Count - 1; j >= 0; j--)
						{
							Job job = pawn.jobs.jobQueue[j].job;
							if (job.targetB == this.ParentThing && !job.targetQueueB.NullOrEmpty<LocalTargetInfo>())
							{
								if (pawn.mindState.priorityWork.WorkGiver != null && this.ParentThing.OccupiedRect().Contains(pawn.mindState.priorityWork.Cell))
								{
									pawn.mindState.priorityWork.Clear();
								}
								pawn.jobs.jobQueue.Extract(job);
							}
							else if (job.targetB == this.ParentThing)
							{
								job.SetTarget(TargetIndex.B, null);
							}
							else if (job.targetQueueB != null && job.targetQueueB.Contains(this.ParentThing))
							{
								job.targetQueueB.Remove(this.ParentThing);
							}
						}
						if (pawn.CurJob.targetB == this.ParentThing && !pawn.CurJob.targetQueueB.NullOrEmpty<LocalTargetInfo>())
						{
							if (pawn.mindState.priorityWork.WorkGiver != null && this.ParentThing.OccupiedRect().Contains(pawn.mindState.priorityWork.Cell))
							{
								pawn.mindState.priorityWork.Clear();
							}
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
						else if (pawn.CurJob.targetB == this.ParentThing)
						{
							pawn.CurJob.SetTarget(TargetIndex.B, null);
						}
						else if (pawn.CurJob.targetQueueB != null && pawn.CurJob.targetQueueB.Contains(this.ParentThing))
						{
							pawn.CurJob.targetQueueB.Remove(this.ParentThing);
						}
					}
				}
				list2.Clear();
			}
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001DB81C File Offset: 0x001D9A1C
		public void ExposeData()
		{
			Scribe_References.Look<IHaulEnroute>(ref this.parent, "parent", false);
			Scribe_Collections.Look<ThingDef, List<ThingCountTracker.PawnCount>>(ref this.stuffCount, "stuffCount", LookMode.Def, LookMode.Deep, ref this.tmpThings, ref this.tmpCount, true, false, false);
		}

		// Token: 0x04004343 RID: 17219
		private Dictionary<ThingDef, List<ThingCountTracker.PawnCount>> stuffCount = new Dictionary<ThingDef, List<ThingCountTracker.PawnCount>>();

		// Token: 0x04004344 RID: 17220
		public IHaulEnroute parent;

		// Token: 0x04004345 RID: 17221
		private List<ThingDef> tmpThings;

		// Token: 0x04004346 RID: 17222
		private List<List<ThingCountTracker.PawnCount>> tmpCount;

		// Token: 0x02000E75 RID: 3701
		public class PawnCount : IExposable
		{
			// Token: 0x06005D3F RID: 23871 RVA: 0x001DB85B File Offset: 0x001D9A5B
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
				Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			}

			// Token: 0x04004347 RID: 17223
			public Pawn pawn;

			// Token: 0x04004348 RID: 17224
			public int count;
		}
	}
}
