using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A87 RID: 6791
	public abstract class MainTabWindow : Window
	{
		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x06009852 RID: 38994 RVA: 0x002C6FD9 File Offset: 0x002C51D9
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06009853 RID: 38995 RVA: 0x00002501 File Offset: 0x00000701
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06009854 RID: 38996 RVA: 0x002C6FEC File Offset: 0x002C51EC
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(UI.screenHeight - 35))
				{
					requestedTabSize.y = (float)(UI.screenHeight - 35);
				}
				if (requestedTabSize.x > (float)UI.screenWidth)
				{
					requestedTabSize.x = (float)UI.screenWidth;
				}
				return requestedTabSize;
			}
		}

		// Token: 0x06009855 RID: 38997 RVA: 0x002C703D File Offset: 0x002C523D
		public MainTabWindow() : base(null)
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = SoundDefOf.TabClose;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06009856 RID: 38998 RVA: 0x002C7074 File Offset: 0x002C5274
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.windowRect.x = 0f;
			}
			else
			{
				this.windowRect.x = (float)UI.screenWidth - this.windowRect.width;
			}
			this.windowRect.y = (float)(UI.screenHeight - 35) - this.windowRect.height;
		}

		// Token: 0x06009857 RID: 38999 RVA: 0x002C70DE File Offset: 0x002C52DE
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.def.closesWorldView)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x04006658 RID: 26200
		public MainButtonDef def;
	}
}
