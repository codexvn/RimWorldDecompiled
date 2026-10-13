using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008FA RID: 2298
	public class CompColorable : ThingComp
	{
		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003B53 RID: 15187 RVA: 0x00139D04 File Offset: 0x00137F04
		// (set) Token: 0x06003B54 RID: 15188 RVA: 0x00139D0C File Offset: 0x00137F0C
		public Color? DesiredColor
		{
			get
			{
				return this.desiredColor;
			}
			set
			{
				this.desiredColor = value;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003B55 RID: 15189 RVA: 0x00139D15 File Offset: 0x00137F15
		public Color Color
		{
			get
			{
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x00139D3B File Offset: 0x00137F3B
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x00139D44 File Offset: 0x00137F44
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.SetColor(this.parent.def.colorGenerator.NewRandomizedColor());
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x00139DAC File Offset: 0x00137FAC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Color?>(ref this.desiredColor, "desiredColor", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x00139E16 File Offset: 0x00138016
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x00139E34 File Offset: 0x00138034
		public void Recolor()
		{
			if (this.desiredColor == null)
			{
				Log.Error(string.Format("Tried recoloring apparel {0} which does not have a desired color set!", this.parent));
				return;
			}
			this.SetColor(this.DesiredColor.Value);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x00139E78 File Offset: 0x00138078
		public void Disable()
		{
			this.active = false;
			this.color = Color.white;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x00139EA3 File Offset: 0x001380A3
		public void SetColor(Color value)
		{
			if (value == this.color)
			{
				return;
			}
			this.active = true;
			this.color = value;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x04002AE7 RID: 10983
		private Color? desiredColor;

		// Token: 0x04002AE8 RID: 10984
		private Color color = Color.white;

		// Token: 0x04002AE9 RID: 10985
		private bool active;
	}
}
