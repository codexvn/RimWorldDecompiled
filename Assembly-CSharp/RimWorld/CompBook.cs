using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001923 RID: 6435
	public class CompBook : CompReadable
	{
		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x0600925D RID: 37469 RVA: 0x002B218E File Offset: 0x002B038E
		public new CompProperties_Book Props
		{
			get
			{
				return (CompProperties_Book)this.props;
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x0600925E RID: 37470 RVA: 0x002B219B File Offset: 0x002B039B
		public new IEnumerable<BookOutcomeDoer> Doers
		{
			get
			{
				return this.doers.OfType<BookOutcomeDoer>();
			}
		}

		// Token: 0x0600925F RID: 37471 RVA: 0x002B21A8 File Offset: 0x002B03A8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (BookOutcomeDoer bookOutcomeDoer in this.Doers)
			{
				foreach (Gizmo gizmo in bookOutcomeDoer.GetGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator2 = null;
			}
			IEnumerator<BookOutcomeDoer> enumerator = null;
			yield break;
			yield break;
		}
	}
}
