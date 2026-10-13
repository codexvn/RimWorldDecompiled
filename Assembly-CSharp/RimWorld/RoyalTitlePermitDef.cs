using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037BB RID: 14267
	public class RoyalTitlePermitDef : Def
	{
		// Token: 0x17003283 RID: 12931
		// (get) Token: 0x06013C51 RID: 80977 RVA: 0x005DE18D File Offset: 0x005DC38D
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		// Token: 0x17003284 RID: 12932
		// (get) Token: 0x06013C52 RID: 80978 RVA: 0x005DE19C File Offset: 0x005DC39C
		public RoyalTitlePermitWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (RoyalTitlePermitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x06013C53 RID: 80979 RVA: 0x005DE1D0 File Offset: 0x005DC3D0
		public bool AvailableForPawn(Pawn pawn, Faction faction)
		{
			if (pawn.royalty == null)
			{
				return false;
			}
			if (pawn.royalty.HasPermit(this, faction))
			{
				return false;
			}
			if (this.prerequisite != null && !pawn.royalty.HasPermit(this.prerequisite, faction))
			{
				return false;
			}
			if (pawn.royalty.GetPermitPoints(faction) < this.permitPointCost)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			return (currentTitle == null && this.minTitle == null) || ((currentTitle != null || this.minTitle == null) && currentTitle.seniority >= this.minTitle.seniority);
		}

		// Token: 0x06013C54 RID: 80980 RVA: 0x005DE26C File Offset: 0x005DC46C
		public bool IsPrerequisiteOfHeldPermit(Pawn pawn, Faction faction)
		{
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == this && allFactionPermits[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013C55 RID: 80981 RVA: 0x005DE2BC File Offset: 0x005DC4BC
		public override IEnumerable<string> ConfigErrors()
		{
			if (!typeof(RoyalTitlePermitWorker).IsAssignableFrom(this.workerClass))
			{
				yield return string.Format("RoyalTitlePermitDef {0} has worker class {1}, which is not deriving from {2}", this.defName, this.workerClass, typeof(RoyalTitlePermitWorker).FullName);
			}
			if (this.royalAid != null)
			{
				if (this.royalAid.pawnKindDef != null && this.royalAid.pawnCount <= 0)
				{
					yield return "pawnCount should be greater than 0, if you specify pawnKindDef";
				}
				if (!this.royalAid.itemsToDrop.NullOrEmpty<ThingDefCountClass>())
				{
					int num;
					for (int i = 0; i < this.royalAid.itemsToDrop.Count; i = num + 1)
					{
						if (this.royalAid.itemsToDrop[i].count <= 0)
						{
							yield return "item count should be greater than 0.";
						}
						if (this.royalAid.itemsToDrop[i].thingDef == null)
						{
							yield return "thingDef not defined.";
						}
						num = i;
					}
				}
				if (this.royalAid.favorCost <= 0)
				{
					yield return "favor cost should be greater than 0.";
				}
			}
			yield break;
		}

		// Token: 0x0400DAEC RID: 56044
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		// Token: 0x0400DAED RID: 56045
		public RoyalAid royalAid;

		// Token: 0x0400DAEE RID: 56046
		public float cooldownDays;

		// Token: 0x0400DAEF RID: 56047
		public RoyalTitleDef minTitle;

		// Token: 0x0400DAF0 RID: 56048
		public int permitPointCost;

		// Token: 0x0400DAF1 RID: 56049
		public FactionDef faction;

		// Token: 0x0400DAF2 RID: 56050
		public bool usableOnWorldMap;

		// Token: 0x0400DAF3 RID: 56051
		public RoyalTitlePermitDef prerequisite;

		// Token: 0x0400DAF4 RID: 56052
		public Vector2 uiPosition;

		// Token: 0x0400DAF5 RID: 56053
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x0400DAF6 RID: 56054
		private RoyalTitlePermitWorker worker;
	}
}
