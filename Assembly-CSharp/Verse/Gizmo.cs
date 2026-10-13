using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B90 RID: 2960
	public abstract class Gizmo
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x00181823 File Offset: 0x0017FA23
		// (set) Token: 0x06004BB1 RID: 19377 RVA: 0x0018182B File Offset: 0x0017FA2B
		public virtual bool Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004BB3 RID: 19379 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004BB4 RID: 19380 RVA: 0x00181834 File Offset: 0x0017FA34
		// (set) Token: 0x06004BB5 RID: 19381 RVA: 0x0018183C File Offset: 0x0017FA3C
		public virtual float Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x06004BB6 RID: 19382
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms);

		// Token: 0x06004BB7 RID: 19383 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06004BB8 RID: 19384
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06004BB9 RID: 19385 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ProcessGroupInput(Event ev, List<Gizmo> group)
		{
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShowPawnDetailsWith(Gizmo gizmo)
		{
			return false;
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x00181845 File Offset: 0x0017FA45
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0018184D File Offset: 0x0017FA4D
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x00181856 File Offset: 0x0017FA56
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x040035F9 RID: 13817
		protected bool disabled;

		// Token: 0x040035FA RID: 13818
		public string disabledReason;

		// Token: 0x040035FB RID: 13819
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x040035FC RID: 13820
		private float order;

		// Token: 0x040035FD RID: 13821
		public const float Height = 75f;
	}
}
