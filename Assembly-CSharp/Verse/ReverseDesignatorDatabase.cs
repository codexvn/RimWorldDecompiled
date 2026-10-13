using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200091C RID: 2332
	public class ReverseDesignatorDatabase
	{
		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003C83 RID: 15491 RVA: 0x0013D348 File Offset: 0x0013B548
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x0013D35E File Offset: 0x0013B55E
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x0013D368 File Offset: 0x0013B568
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0013D3C4 File Offset: 0x0013B5C4
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_DeconstructConduit());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_PlantsHarvestWood());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_MineVein());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_Open());
			this.desList.Add(new Designator_EjectFuel());
			this.desList.Add(new Designator_SmoothSurface());
			this.desList.Add(new Designator_SmoothFloors());
			this.desList.Add(new Designator_SmoothWalls());
			this.desList.Add(new Designator_ReleaseAnimalToWild());
			this.desList.Add(new Designator_ExtractTree());
			this.desList.Add(new Designator_RemovePaint());
			if (ModsConfig.BiotechActive)
			{
				this.desList.Add(new Designator_MechControlGroup());
				this.desList.Add(new Designator_Adopt());
			}
			this.desList.Add(new Designator_FillIn());
			this.desList.Add(new Designator_ExtractSkull());
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x04002B85 RID: 11141
		private List<Designator> desList;
	}
}
