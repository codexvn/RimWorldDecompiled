using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023ED RID: 9197
	public abstract class RoyalTitlePermitWorker_Targeted : RoyalTitlePermitWorker, ITargetingSource
	{
		// Token: 0x17001E30 RID: 7728
		// (get) Token: 0x0600C763 RID: 51043 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E31 RID: 7729
		// (get) Token: 0x0600C764 RID: 51044 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E32 RID: 7730
		// (get) Token: 0x0600C765 RID: 51045 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E33 RID: 7731
		// (get) Token: 0x0600C766 RID: 51046 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E34 RID: 7732
		// (get) Token: 0x0600C767 RID: 51047 RVA: 0x00002501 File Offset: 0x00000701
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E35 RID: 7733
		// (get) Token: 0x0600C768 RID: 51048 RVA: 0x003A1C47 File Offset: 0x0039FE47
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17001E36 RID: 7734
		// (get) Token: 0x0600C769 RID: 51049 RVA: 0x003A1C47 File Offset: 0x0039FE47
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17001E37 RID: 7735
		// (get) Token: 0x0600C76A RID: 51050 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001E38 RID: 7736
		// (get) Token: 0x0600C76B RID: 51051 RVA: 0x00002C42 File Offset: 0x00000E42
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001E39 RID: 7737
		// (get) Token: 0x0600C76C RID: 51052 RVA: 0x003A1C4F File Offset: 0x0039FE4F
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		// Token: 0x17001E3A RID: 7738
		// (get) Token: 0x0600C76D RID: 51053 RVA: 0x00002C42 File Offset: 0x00000E42
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001E3B RID: 7739
		// (get) Token: 0x0600C76E RID: 51054 RVA: 0x003A1C57 File Offset: 0x0039FE57
		protected float RangeClamped
		{
			get
			{
				return Mathf.Min(this.def.royalAid.targetingRange, this.map.weatherManager.CurWeatherMaxRangeCap);
			}
		}

		// Token: 0x0600C76F RID: 51055 RVA: 0x003A1C80 File Offset: 0x0039FE80
		public bool CanHitTarget(LocalTargetInfo target)
		{
			if (this.def.royalAid.targetingRequireLOS && !GenSight.LineOfSight(this.caller.Position, target.Cell, this.map, true, null, 0, 0))
			{
				bool flag = false;
				ShootLeanUtility.LeanShootingSourcesFromTo(this.caller.Position, target.Cell, this.map, RoyalTitlePermitWorker_Targeted.tempSourceList);
				for (int i = 0; i < RoyalTitlePermitWorker_Targeted.tempSourceList.Count; i++)
				{
					if (GenSight.LineOfSight(RoyalTitlePermitWorker_Targeted.tempSourceList[i], target.Cell, this.map, true, null, 0, 0))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C770 RID: 51056 RVA: 0x003A1D2C File Offset: 0x0039FF2C
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!this.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600C771 RID: 51057 RVA: 0x003A1D82 File Offset: 0x0039FF82
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.RangeClamped, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600C772 RID: 51058 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
		}

		// Token: 0x0600C773 RID: 51059 RVA: 0x003A1DB0 File Offset: 0x0039FFB0
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != null && this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x040087FC RID: 34812
		protected bool free;

		// Token: 0x040087FD RID: 34813
		protected Pawn caller;

		// Token: 0x040087FE RID: 34814
		protected Map map;

		// Token: 0x040087FF RID: 34815
		protected TargetingParameters targetingParameters;

		// Token: 0x04008800 RID: 34816
		private static List<IntVec3> tempSourceList = new List<IntVec3>();
	}
}
