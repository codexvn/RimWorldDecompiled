using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020008FF RID: 2303
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x0013A160 File Offset: 0x00138360
		protected Pawn Holder
		{
			get
			{
				ThingWithComps parent = this.parent;
				Pawn_EquipmentTracker pawn_EquipmentTracker = ((parent != null) ? parent.ParentHolder : null) as Pawn_EquipmentTracker;
				if (pawn_EquipmentTracker == null)
				{
					return null;
				}
				return pawn_EquipmentTracker.pawn;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003B6F RID: 15215 RVA: 0x0013A184 File Offset: 0x00138384
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003B70 RID: 15216 RVA: 0x0013A191 File Offset: 0x00138391
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003B71 RID: 15217 RVA: 0x0013A19E File Offset: 0x0013839E
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06003B72 RID: 15218 RVA: 0x0013A1A6 File Offset: 0x001383A6
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003B73 RID: 15219 RVA: 0x0013A1B8 File Offset: 0x001383B8
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x00002C42 File Offset: 0x00000E42
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003B75 RID: 15221 RVA: 0x0013A1CA File Offset: 0x001383CA
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0013A1D1 File Offset: 0x001383D1
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0013A1E5 File Offset: 0x001383E5
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands();
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0013A1F4 File Offset: 0x001383F4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0013A246 File Offset: 0x00138446
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0013A268 File Offset: 0x00138468
		public override void CompTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0013A278 File Offset: 0x00138478
		public override void Notify_Unequipped(Pawn p)
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0013A2A9 File Offset: 0x001384A9
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0013A2C0 File Offset: 0x001384C0
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x04002AF2 RID: 10994
		public VerbTracker verbTracker;
	}
}
