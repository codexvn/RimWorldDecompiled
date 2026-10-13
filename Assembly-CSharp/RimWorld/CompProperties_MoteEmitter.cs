using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D66 RID: 11622
	public class CompProperties_MoteEmitter : CompProperties
	{
		// Token: 0x06010544 RID: 66884 RVA: 0x004B6BE8 File Offset: 0x004B4DE8
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		// Token: 0x1700298A RID: 10634
		// (get) Token: 0x06010545 RID: 66885 RVA: 0x004B6C54 File Offset: 0x004B4E54
		public Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.offsetMin.x, this.offsetMax.x), Rand.Range(this.offsetMin.y, this.offsetMax.y), Rand.Range(this.offsetMin.z, this.offsetMax.z));
			}
		}

		// Token: 0x06010546 RID: 66886 RVA: 0x004B6CB8 File Offset: 0x004B4EB8
		public Vector3 RotationOffset(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.offsetNorth;
			case 1:
				return this.offsetEast;
			case 2:
				return this.offsetSouth;
			case 3:
				return this.offsetWest;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06010547 RID: 66887 RVA: 0x004B6D06 File Offset: 0x004B4F06
		public ThingDef RotationMote(Rot4 rot)
		{
			List<ThingDef> list = this.perRotationMotes;
			if (list == null)
			{
				return null;
			}
			return list[rot.AsInt];
		}

		// Token: 0x06010548 RID: 66888 RVA: 0x004B6D20 File Offset: 0x004B4F20
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null && this.perRotationMotes.NullOrEmpty<ThingDef>())
			{
				yield return "CompMoteEmitter must have a mote assigned.";
			}
			if (!this.perRotationMotes.NullOrEmpty<ThingDef>() && this.perRotationMotes.Count != 4)
			{
				yield return "perRotationMotes must contain 4 elements for North, East, South, West";
			}
			yield break;
		}

		// Token: 0x0400AA50 RID: 43600
		public ThingDef mote;

		// Token: 0x0400AA51 RID: 43601
		public List<ThingDef> perRotationMotes;

		// Token: 0x0400AA52 RID: 43602
		public Vector3 offset;

		// Token: 0x0400AA53 RID: 43603
		public Vector3 offsetMin = Vector3.zero;

		// Token: 0x0400AA54 RID: 43604
		public Vector3 offsetMax = Vector3.zero;

		// Token: 0x0400AA55 RID: 43605
		public Vector3 offsetNorth = Vector3.zero;

		// Token: 0x0400AA56 RID: 43606
		public Vector3 offsetSouth = Vector3.zero;

		// Token: 0x0400AA57 RID: 43607
		public Vector3 offsetEast = Vector3.zero;

		// Token: 0x0400AA58 RID: 43608
		public Vector3 offsetWest = Vector3.zero;

		// Token: 0x0400AA59 RID: 43609
		public bool useParentRotation;

		// Token: 0x0400AA5A RID: 43610
		public SoundDef soundOnEmission;

		// Token: 0x0400AA5B RID: 43611
		public int emissionInterval = -1;

		// Token: 0x0400AA5C RID: 43612
		public int ticksSinceLastEmittedMaxOffset;

		// Token: 0x0400AA5D RID: 43613
		public bool maintain;

		// Token: 0x0400AA5E RID: 43614
		[NoTranslate]
		public string saveKeysPrefix;
	}
}
