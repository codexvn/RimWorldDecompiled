using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E8F RID: 7823
	public class Area_Home : Area
	{
		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600ADB0 RID: 44464 RVA: 0x0031913C File Offset: 0x0031733C
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x0600ADB1 RID: 44465 RVA: 0x0031914D File Offset: 0x0031734D
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x0600ADB2 RID: 44466 RVA: 0x00319163 File Offset: 0x00317363
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x0600ADB3 RID: 44467 RVA: 0x0031916A File Offset: 0x0031736A
		public Area_Home()
		{
		}

		// Token: 0x0600ADB4 RID: 44468 RVA: 0x00319172 File Offset: 0x00317372
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600ADB5 RID: 44469 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x0600ADB6 RID: 44470 RVA: 0x0031917B File Offset: 0x0031737B
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_Home";
		}

		// Token: 0x0600ADB7 RID: 44471 RVA: 0x00319197 File Offset: 0x00317397
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] == val)
			{
				return;
			}
			base.Set(c, val);
			base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
		}
	}
}
