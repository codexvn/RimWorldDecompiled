using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000278 RID: 632
	public class EffecterDef : Def
	{
		// Token: 0x06000FFF RID: 4095 RVA: 0x0004B8B4 File Offset: 0x00049AB4
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0004B8BC File Offset: 0x00049ABC
		public Effecter SpawnMaintained(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = this.Spawn(target, map, scale);
			if (this.maintainTicks != 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, target, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0004B8F8 File Offset: 0x00049AF8
		public Effecter SpawnMaintained(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = this.Spawn(target, map, scale);
			if (this.maintainTicks != 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, target, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0004B934 File Offset: 0x00049B34
		public Effecter SpawnMaintained(TargetInfo A, TargetInfo B, float scale = 1f)
		{
			Effecter effecter = this.Spawn(A, B, scale);
			if (this.maintainTicks != 0)
			{
				A.Map.effecterMaintainer.AddEffecterToMaintain(effecter, A, B, this.maintainTicks);
			}
			else
			{
				effecter.Cleanup();
			}
			return effecter;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0004B978 File Offset: 0x00049B78
		public Effecter Spawn(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0004B9A8 File Offset: 0x00049BA8
		public Effecter Spawn(IntVec3 targetA, IntVec3 targetB, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo a = new TargetInfo(targetA, map, false);
			effecter.scale = scale;
			effecter.Trigger(a, new TargetInfo(targetB, map, false), -1);
			return effecter;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0004B9E0 File Offset: 0x00049BE0
		public Effecter Spawn(IntVec3 target, Map map, Vector3 offset, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.offset = offset;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0004BA18 File Offset: 0x00049C18
		public Effecter Spawn(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = ((this.offsetMode == EffecterDef.OffsetMode.TrueCenterCompensated) ? (target.TrueCenter() - target.Position.ToVector3Shifted()) : Vector3.zero);
			effecter.scale = scale;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0004BA79 File Offset: 0x00049C79
		public Effecter Spawn(TargetInfo targetA, TargetInfo targetB, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.scale = scale;
			effecter.Trigger(targetA, targetB, -1);
			return effecter;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0004BA94 File Offset: 0x00049C94
		public Effecter SpawnAttached(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = ((this.offsetMode == EffecterDef.OffsetMode.TrueCenterCompensated) ? (target.TrueCenter() - target.Position.ToVector3Shifted()) : Vector3.zero);
			effecter.scale = scale;
			effecter.Trigger(target, target, -1);
			return effecter;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0004BAF0 File Offset: 0x00049CF0
		public Effecter Spawn(Thing target, Map map, Vector3 offset)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = offset;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x04000C4D RID: 3149
		public List<SubEffecterDef> children;

		// Token: 0x04000C4E RID: 3150
		public float positionRadius;

		// Token: 0x04000C4F RID: 3151
		public FloatRange offsetTowardsTarget;

		// Token: 0x04000C50 RID: 3152
		public int maintainTicks;

		// Token: 0x04000C51 RID: 3153
		public float randomWeight = 1f;

		// Token: 0x04000C52 RID: 3154
		public EffecterDef.OffsetMode offsetMode;

		// Token: 0x02000279 RID: 633
		public enum OffsetMode
		{
			// Token: 0x04000C54 RID: 3156
			TrueCenterCompensated,
			// Token: 0x04000C55 RID: 3157
			Free
		}
	}
}
