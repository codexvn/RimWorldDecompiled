using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193F RID: 6463
	public abstract class ReadingOutcomeDoer
	{
		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x060092F5 RID: 37621 RVA: 0x002B3D44 File Offset: 0x002B1F44
		// (set) Token: 0x060092F6 RID: 37622 RVA: 0x002B3D4C File Offset: 0x002B1F4C
		public ThingWithComps Parent { get; private set; }

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x060092F7 RID: 37623 RVA: 0x002B3D55 File Offset: 0x002B1F55
		// (set) Token: 0x060092F8 RID: 37624 RVA: 0x002B3D5D File Offset: 0x002B1F5D
		public CompReadable Readable { get; private set; }

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x060092F9 RID: 37625 RVA: 0x002B3D66 File Offset: 0x002B1F66
		public ReadingOutcomeProperties Props
		{
			get
			{
				return this.props;
			}
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x002B3D6E File Offset: 0x002B1F6E
		public virtual void Initialize(ThingWithComps parent, ReadingOutcomeProperties props)
		{
			this.Parent = parent;
			this.Readable = parent.GetComp<CompReadable>();
			this.props = props;
		}

		// Token: 0x060092FB RID: 37627 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMake()
		{
		}

		// Token: 0x060092FC RID: 37628 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnReadingTick(Pawn reader, float factor)
		{
		}

		// Token: 0x060092FD RID: 37629 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostExposeData()
		{
		}

		// Token: 0x04005DB2 RID: 23986
		protected ReadingOutcomeProperties props;
	}
}
