using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200086A RID: 2154
	public abstract class AttachableThing : ThingWithComps, IThingGlower
	{
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003653 RID: 13907 RVA: 0x00123752 File Offset: 0x00121952
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.03292683f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003654 RID: 13908
		public abstract string InspectStringAddon { get; }

		// Token: 0x06003655 RID: 13909 RVA: 0x00123782 File Offset: 0x00121982
		public virtual bool ShouldBeLitNow()
		{
			return this.parent != null;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0012378D File Offset: 0x0012198D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x001237C4 File Offset: 0x001219C4
		public virtual void AttachTo(Thing newParent)
		{
			this.parent = newParent;
			CompAttachBase compAttachBase;
			if (!newParent.TryGetComp(out compAttachBase))
			{
				Log.Error(string.Concat(new string[]
				{
					"Cannot attach ",
					(this != null) ? this.ToString() : null,
					" to ",
					(newParent != null) ? newParent.ToString() : null,
					": parent has no CompAttachBase."
				}));
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x00123832 File Offset: 0x00121A32
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x040028E6 RID: 10470
		public Thing parent;
	}
}
