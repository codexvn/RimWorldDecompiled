using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D6D RID: 11629
	public class CompMoteEmitterRandomColor : CompMoteEmitter
	{
		// Token: 0x17002993 RID: 10643
		// (get) Token: 0x0601056B RID: 66923 RVA: 0x004B761B File Offset: 0x004B581B
		public CompProperties_MoteEmitterRandomColor Props
		{
			get
			{
				return (CompProperties_MoteEmitterRandomColor)this.props;
			}
		}

		// Token: 0x0601056C RID: 66924 RVA: 0x004B7628 File Offset: 0x004B5828
		public override void Emit()
		{
			base.Emit();
			this.mote.instanceColor = this.Props.colors.RandomElement<Color>();
		}
	}
}
