using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000547 RID: 1351
	public abstract class WeatherEvent
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600230E RID: 8974
		public abstract bool Expired { get; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x000B694B File Offset: 0x000B4B4B
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x000B695A File Offset: 0x000B4B5A
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000B6964 File Offset: 0x000B4B64
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000B697A File Offset: 0x000B4B7A
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002314 RID: 8980
		public abstract void FireEvent();

		// Token: 0x06002315 RID: 8981
		public abstract void WeatherEventTick();

		// Token: 0x06002316 RID: 8982 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04001D4A RID: 7498
		protected Map map;
	}
}
