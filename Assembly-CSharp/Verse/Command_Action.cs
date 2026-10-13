using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B80 RID: 2944
	public class Command_Action : Command
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x001806FC File Offset: 0x0017E8FC
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.iconDrawColorOverride;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00180727 File Offset: 0x0017E927
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x0018073B File Offset: 0x0017E93B
		public override void GizmoUpdateOnMouseover()
		{
			if (this.onHover != null)
			{
				this.onHover();
			}
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x00180750 File Offset: 0x0017E950
		public void SetColorOverride(Color color)
		{
			this.iconDrawColorOverride = new Color?(color);
		}

		// Token: 0x040035C7 RID: 13767
		public Action action;

		// Token: 0x040035C8 RID: 13768
		public Action onHover;

		// Token: 0x040035C9 RID: 13769
		private Color? iconDrawColorOverride;
	}
}
