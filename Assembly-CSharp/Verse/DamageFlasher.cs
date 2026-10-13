using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005EA RID: 1514
	public class DamageFlasher
	{
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000CD6AC File Offset: 0x000CB8AC
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x000CD6C2 File Offset: 0x000CB8C2
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x000CD6D0 File Offset: 0x000CB8D0
		public Color CurColor
		{
			get
			{
				return Color.Lerp(Color.white, DamagedMatPool.DamagedMatStartingColor, (float)this.DamageFlashTicksLeft / 16f);
			}
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x000CD6EE File Offset: 0x000CB8EE
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000CD701 File Offset: 0x000CB901
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000CD716 File Offset: 0x000CB916
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04002021 RID: 8225
		private int lastDamageTick = -9999;

		// Token: 0x04002022 RID: 8226
		private const int DamagedMatTicksTotal = 16;
	}
}
