using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB5 RID: 3253
	public class SubEffecter
	{
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06005354 RID: 21332 RVA: 0x001AD7D0 File Offset: 0x001AB9D0
		public Color EffectiveColor
		{
			get
			{
				Color? color = this.colorOverride;
				if (color == null)
				{
					return this.def.color;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06005355 RID: 21333 RVA: 0x001AD800 File Offset: 0x001ABA00
		public MoteSpawnLocType EffectiveSpawnLocType
		{
			get
			{
				MoteSpawnLocType? moteSpawnLocType = this.spawnLocOverride;
				if (moteSpawnLocType == null)
				{
					return this.def.spawnLocType;
				}
				return moteSpawnLocType.GetValueOrDefault();
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06005356 RID: 21334 RVA: 0x001AD830 File Offset: 0x001ABA30
		public Vector3? EffectiveDimensions
		{
			get
			{
				Vector3? result = this.dimensionsOverride;
				if (result == null)
				{
					return this.def.positionDimensions;
				}
				return result;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06005357 RID: 21335 RVA: 0x001AD85C File Offset: 0x001ABA5C
		public Vector3 EffectiveOffset
		{
			get
			{
				Vector3? vector = this.offsetOverride;
				if (vector == null)
				{
					return this.def.positionOffset;
				}
				return vector.GetValueOrDefault();
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06005358 RID: 21336 RVA: 0x001AD88C File Offset: 0x001ABA8C
		public float EffectiveChancePerTick
		{
			get
			{
				float? num = this.chanceOverride;
				if (num == null)
				{
					return this.def.chancePerTick;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x001AD8BC File Offset: 0x001ABABC
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04003C2A RID: 15402
		public Effecter parent;

		// Token: 0x04003C2B RID: 15403
		public SubEffecterDef def;

		// Token: 0x04003C2C RID: 15404
		public Color? colorOverride;

		// Token: 0x04003C2D RID: 15405
		public MoteSpawnLocType? spawnLocOverride;

		// Token: 0x04003C2E RID: 15406
		public Vector3? dimensionsOverride;

		// Token: 0x04003C2F RID: 15407
		public Vector3? offsetOverride;

		// Token: 0x04003C30 RID: 15408
		public float? chanceOverride;
	}
}
