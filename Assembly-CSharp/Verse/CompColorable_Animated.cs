using System;

namespace Verse
{
	// Token: 0x020008FE RID: 2302
	public class CompColorable_Animated : CompColorable
	{
		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003B69 RID: 15209 RVA: 0x0013A05F File Offset: 0x0013825F
		public CompProperties_ColorableAnimated Props
		{
			get
			{
				return (CompProperties_ColorableAnimated)this.props;
			}
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0013A06C File Offset: 0x0013826C
		public override void Initialize(CompProperties props)
		{
			this.props = props;
			if (this.Props.startWithRandom)
			{
				this.colorOffset = Rand.RangeInclusive(0, this.Props.colors.Count - 1);
			}
			base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0013A0D8 File Offset: 0x001382D8
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.Props.changeInterval))
			{
				base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
				this.colorOffset++;
			}
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0013A13E File Offset: 0x0013833E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.colorOffset, "colorOffset", 0, false);
		}

		// Token: 0x04002AF1 RID: 10993
		public int colorOffset;
	}
}
