using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B50 RID: 6992
	public class GameRules : IExposable
	{
		// Token: 0x06009C25 RID: 39973 RVA: 0x002D535C File Offset: 0x002D355C
		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		// Token: 0x06009C26 RID: 39974 RVA: 0x002D53AF File Offset: 0x002D35AF
		public void SetAllowBuilding(ThingDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		// Token: 0x06009C27 RID: 39975 RVA: 0x002D53F0 File Offset: 0x002D35F0
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		// Token: 0x06009C28 RID: 39976 RVA: 0x002D5430 File Offset: 0x002D3630
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		// Token: 0x04006C53 RID: 27731
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		// Token: 0x04006C54 RID: 27732
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
