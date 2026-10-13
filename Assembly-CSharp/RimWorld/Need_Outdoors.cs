using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200248A RID: 9354
	public class Need_Outdoors : Need
	{
		// Token: 0x17001EDD RID: 7901
		// (get) Token: 0x0600CAE5 RID: 51941 RVA: 0x003B6FD7 File Offset: 0x003B51D7
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17001EDE RID: 7902
		// (get) Token: 0x0600CAE6 RID: 51942 RVA: 0x003B6FF0 File Offset: 0x003B51F0
		public OutdoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.8f)
				{
					return OutdoorsCategory.Free;
				}
				if (this.CurLevel > 0.6f)
				{
					return OutdoorsCategory.NeedFreshAir;
				}
				if (this.CurLevel > 0.4f)
				{
					return OutdoorsCategory.CabinFeverLight;
				}
				if (this.CurLevel >= 0.2f)
				{
					return OutdoorsCategory.CabinFeverSevere;
				}
				if (this.CurLevel > 0.05f)
				{
					return OutdoorsCategory.Trapped;
				}
				return OutdoorsCategory.Entombed;
			}
		}

		// Token: 0x17001EDF RID: 7903
		// (get) Token: 0x0600CAE7 RID: 51943 RVA: 0x003B7049 File Offset: 0x003B5249
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17001EE0 RID: 7904
		// (get) Token: 0x0600CAE8 RID: 51944 RVA: 0x003B7054 File Offset: 0x003B5254
		private bool Disabled
		{
			get
			{
				return this.pawn.Dead || this.pawn.needs.PrefersIndoors;
			}
		}

		// Token: 0x0600CAE9 RID: 51945 RVA: 0x003B7078 File Offset: 0x003B5278
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x0600CAEA RID: 51946 RVA: 0x003B5485 File Offset: 0x003B3685
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x0600CAEB RID: 51947 RVA: 0x003B70E8 File Offset: 0x003B52E8
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			float b = 0.2f;
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 5f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = -0.32f;
				}
				else
				{
					num = -0.45f;
					b = 0f;
				}
			}
			else if (roofDef == null)
			{
				num = 8f;
			}
			else if (roofDef.isThickRoof)
			{
				num = -0.4f;
			}
			else
			{
				num = 1f;
			}
			if (this.pawn.InBed() && num < 0f)
			{
				num *= 0.2f;
			}
			num *= 0.0025f;
			float curLevel = this.CurLevel;
			if (num < 0f)
			{
				this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
			}
			else
			{
				this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
			}
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x04008A52 RID: 35410
		private const float Delta_IndoorsThickRoof = -0.45f;

		// Token: 0x04008A53 RID: 35411
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04008A54 RID: 35412
		private const float Delta_IndoorsThinRoof = -0.32f;

		// Token: 0x04008A55 RID: 35413
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x04008A56 RID: 35414
		private const float Delta_OutdoorsThinRoof = 1f;

		// Token: 0x04008A57 RID: 35415
		private const float Delta_IndoorsNoRoof = 5f;

		// Token: 0x04008A58 RID: 35416
		private const float Delta_OutdoorsNoRoof = 8f;

		// Token: 0x04008A59 RID: 35417
		private const float DeltaFactor_InBed = 0.2f;

		// Token: 0x04008A5A RID: 35418
		private float lastEffectiveDelta;
	}
}
