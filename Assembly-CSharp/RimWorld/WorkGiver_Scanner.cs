using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001549 RID: 5449
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06008150 RID: 33104 RVA: 0x0026F78A File Offset: 0x0026D98A
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06008151 RID: 33105 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06008152 RID: 33106 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008153 RID: 33107 RVA: 0x0026F792 File Offset: 0x0026D992
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return Enumerable.Empty<IntVec3>();
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06008155 RID: 33109 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06008156 RID: 33110 RVA: 0x000E96BB File Offset: 0x000E78BB
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008157 RID: 33111 RVA: 0x0026F799 File Offset: 0x0026D999
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x06008158 RID: 33112 RVA: 0x0026F7A1 File Offset: 0x0026D9A1
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x06008159 RID: 33113 RVA: 0x0026F7AF File Offset: 0x0026D9AF
		public virtual string JobInfo(Pawn pawn, Job job)
		{
			return string.Empty;
		}

		// Token: 0x0600815A RID: 33114 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x0600815B RID: 33115 RVA: 0x0026F7B6 File Offset: 0x0026D9B6
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x0600815C RID: 33116 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x0600815D RID: 33117 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x0600815E RID: 33118 RVA: 0x0026F7C4 File Offset: 0x0026D9C4
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		// Token: 0x0600815F RID: 33119 RVA: 0x0026F7D1 File Offset: 0x0026D9D1
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}

		// Token: 0x06008160 RID: 33120 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ReservationLayerDef GetReservationLayer(Pawn pawn, LocalTargetInfo target)
		{
			return null;
		}
	}
}
