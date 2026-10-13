using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200214C RID: 8524
	public class CompStunnable : ThingComp
	{
		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x0600BA35 RID: 47669 RVA: 0x0035B77B File Offset: 0x0035997B
		private CompProperties_Stunnable Props
		{
			get
			{
				return (CompProperties_Stunnable)this.props;
			}
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x0600BA36 RID: 47670 RVA: 0x0035B788 File Offset: 0x00359988
		public StunHandler StunHandler
		{
			get
			{
				return this.stunHandler;
			}
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x0600BA37 RID: 47671 RVA: 0x0035B790 File Offset: 0x00359990
		public bool UseLargeEMPEffecter
		{
			get
			{
				return this.Props.useLargeEMPEffecter;
			}
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x0600BA38 RID: 47672 RVA: 0x0035B79D File Offset: 0x0035999D
		public Vector3? EMPEffecterDimensions
		{
			get
			{
				return this.Props.empEffecterDimensions;
			}
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x0600BA39 RID: 47673 RVA: 0x0035B7AA File Offset: 0x003599AA
		public Vector3? EMPEffecterOffset
		{
			get
			{
				return this.Props.empEffecterOffset;
			}
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x0600BA3A RID: 47674 RVA: 0x0035B7B7 File Offset: 0x003599B7
		public float? EMPChancePerTick
		{
			get
			{
				return this.Props.empChancePerTick;
			}
		}

		// Token: 0x0600BA3B RID: 47675 RVA: 0x0035B7C4 File Offset: 0x003599C4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.stunHandler = new StunHandler(this.parent);
		}

		// Token: 0x0600BA3C RID: 47676 RVA: 0x0035B7DE File Offset: 0x003599DE
		public override void CompTick()
		{
			this.stunHandler.StunHandlerTick();
		}

		// Token: 0x0600BA3D RID: 47677 RVA: 0x0035B7EC File Offset: 0x003599EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<StunHandler>(ref this.stunHandler, "stunHandler", new object[]
			{
				this.parent
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.stunHandler == null)
			{
				this.stunHandler = new StunHandler(this.parent);
			}
		}

		// Token: 0x0600BA3E RID: 47678 RVA: 0x0035B840 File Offset: 0x00359A40
		public override string CompInspectStringExtra()
		{
			if (!this.stunHandler.Stunned)
			{
				return null;
			}
			if (this.stunHandler.Hypnotized)
			{
				return "InTrance".Translate();
			}
			if (this.stunHandler.StunFromEMP)
			{
				return "StunnedByEMP".Translate().CapitalizeFirst() + ": " + this.stunHandler.StunTicksLeft.ToStringSecondsFromTicks();
			}
			return "StunLower".Translate().CapitalizeFirst() + ": " + this.stunHandler.StunTicksLeft.ToStringSecondsFromTicks();
		}

		// Token: 0x0600BA3F RID: 47679 RVA: 0x0035B8F3 File Offset: 0x00359AF3
		public void ApplyDamage(DamageInfo damageInfo)
		{
			this.stunHandler.Notify_DamageApplied(damageInfo);
		}

		// Token: 0x0600BA40 RID: 47680 RVA: 0x0035B901 File Offset: 0x00359B01
		public bool CanBeStunnedByDamage(DamageDef def)
		{
			return this.Props.affectedDamageDefs.Contains(def);
		}

		// Token: 0x0600BA41 RID: 47681 RVA: 0x0035B914 File Offset: 0x00359B14
		public bool CanAdaptToDamage(DamageDef def)
		{
			return this.Props.adaptableDamageDefs.Contains(def);
		}

		// Token: 0x04008060 RID: 32864
		private StunHandler stunHandler;
	}
}
