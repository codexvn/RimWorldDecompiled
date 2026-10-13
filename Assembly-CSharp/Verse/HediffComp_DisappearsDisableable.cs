using System;

namespace Verse
{
	// Token: 0x020006A3 RID: 1699
	public class HediffComp_DisappearsDisableable : HediffComp_DisappearsPausable
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002ABA RID: 10938 RVA: 0x000E0590 File Offset: 0x000DE790
		public new HediffCompProperties_DisappearsDisableable Props
		{
			get
			{
				return (HediffCompProperties_DisappearsDisableable)this.props;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000E059D File Offset: 0x000DE79D
		protected override bool Paused
		{
			get
			{
				return this.disabled;
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000E05A5 File Offset: 0x000DE7A5
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.disabled = this.Props.initiallyDisabled;
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000E05BE File Offset: 0x000DE7BE
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.disabled)
				{
					return base.CompLabelInBracketsExtra;
				}
				return null;
			}
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000E05D0 File Offset: 0x000DE7D0
		public override void CopyFrom(HediffComp other)
		{
			base.CopyFrom(other);
			HediffComp_DisappearsDisableable hediffComp_DisappearsDisableable = other as HediffComp_DisappearsDisableable;
			if (hediffComp_DisappearsDisableable != null)
			{
				this.disabled = hediffComp_DisappearsDisableable.disabled;
			}
		}

		// Token: 0x04002292 RID: 8850
		public bool disabled;
	}
}
