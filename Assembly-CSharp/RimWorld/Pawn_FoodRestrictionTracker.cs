using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200250B RID: 9483
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		// Token: 0x17001FC7 RID: 8135
		// (get) Token: 0x0600CF26 RID: 53030 RVA: 0x003CD008 File Offset: 0x003CB208
		// (set) Token: 0x0600CF27 RID: 53031 RVA: 0x003CD05E File Offset: 0x003CB25E
		public FoodPolicy CurrentFoodPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
				}
				return this.curPolicy;
			}
			set
			{
				this.curPolicy = value;
			}
		}

		// Token: 0x17001FC8 RID: 8136
		// (get) Token: 0x0600CF28 RID: 53032 RVA: 0x003CD068 File Offset: 0x003CB268
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		// Token: 0x0600CF29 RID: 53033 RVA: 0x003CD0BC File Offset: 0x003CB2BC
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF2A RID: 53034 RVA: 0x000024DF File Offset: 0x000006DF
		public Pawn_FoodRestrictionTracker()
		{
		}

		// Token: 0x0600CF2B RID: 53035 RVA: 0x003CD0CC File Offset: 0x003CB2CC
		public FoodPolicy GetCurrentRespectedRestriction(Pawn getter = null)
		{
			if (!this.Configurable)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer && (getter == null || getter.Faction != Faction.OfPlayer))
			{
				return null;
			}
			if (this.pawn.InMentalState)
			{
				return null;
			}
			return this.CurrentFoodPolicy;
		}

		// Token: 0x0600CF2C RID: 53036 RVA: 0x003CD11C File Offset: 0x003CB31C
		public bool BabyFoodAllowed(ThingDef food)
		{
			this.TrySetupAllowedBabyFoodTypes();
			if (!ITab_Pawn_Feeding.BabyConsumableFoods.Contains(food))
			{
				return false;
			}
			if (!this.allowedBabyFoodTypes.ContainsKey(food))
			{
				this.allowedBabyFoodTypes.Add(food, true);
			}
			return this.allowedBabyFoodTypes[food];
		}

		// Token: 0x0600CF2D RID: 53037 RVA: 0x003CD15A File Offset: 0x003CB35A
		public void SetBabyFoodAllowed(ThingDef food, bool allowed)
		{
			this.TrySetupAllowedBabyFoodTypes();
			if (!ITab_Pawn_Feeding.BabyConsumableFoods.Contains(food))
			{
				return;
			}
			this.allowedBabyFoodTypes.SetOrAdd(food, allowed);
		}

		// Token: 0x0600CF2E RID: 53038 RVA: 0x003CD180 File Offset: 0x003CB380
		private void TrySetupAllowedBabyFoodTypes()
		{
			if (this.allowedBabyFoodTypes == null)
			{
				this.allowedBabyFoodTypes = new Dictionary<ThingDef, bool>();
				foreach (ThingDef key in ITab_Pawn_Feeding.BabyConsumableFoods)
				{
					this.allowedBabyFoodTypes.Add(key, true);
				}
			}
		}

		// Token: 0x0600CF2F RID: 53039 RVA: 0x003CD1EC File Offset: 0x003CB3EC
		public void ExposeData()
		{
			Scribe_References.Look<FoodPolicy>(ref this.curPolicy, "curRestriction", false);
			Scribe_Collections.Look<ThingDef, bool>(ref this.allowedBabyFoodTypes, "allowedBabyFoodTypes", LookMode.Def, LookMode.Value);
		}

		// Token: 0x04008CF1 RID: 36081
		public Pawn pawn;

		// Token: 0x04008CF2 RID: 36082
		private FoodPolicy curPolicy;

		// Token: 0x04008CF3 RID: 36083
		private Dictionary<ThingDef, bool> allowedBabyFoodTypes;
	}
}
