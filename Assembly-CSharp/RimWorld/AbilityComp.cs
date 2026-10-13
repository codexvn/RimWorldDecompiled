using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020021E3 RID: 8675
	public abstract class AbilityComp
	{
		// Token: 0x17001CFB RID: 7419
		// (get) Token: 0x0600BDF3 RID: 48627 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanCast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDF4 RID: 48628 RVA: 0x00371B6B File Offset: 0x0036FD6B
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600BDF5 RID: 48629 RVA: 0x00371B74 File Offset: 0x0036FD74
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = base.GetType().Name;
			array[1] = "(parent=";
			int num = 2;
			Ability ability = this.parent;
			array[num] = ((ability != null) ? ability.ToString() : null);
			array[3] = " at=";
			array[4] = ((this.parent != null) ? this.parent.pawn.Position : IntVec3.Invalid).ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600BDF6 RID: 48630 RVA: 0x00132D70 File Offset: 0x00130F70
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600BDF7 RID: 48631 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			return 0f;
		}

		// Token: 0x0600BDF8 RID: 48632 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTick()
		{
		}

		// Token: 0x0600BDF9 RID: 48633 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x0600BDFA RID: 48634 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x0600BDFB RID: 48635 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600BDFC RID: 48636 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0400829F RID: 33439
		public Ability parent;

		// Token: 0x040082A0 RID: 33440
		public AbilityCompProperties props;
	}
}
