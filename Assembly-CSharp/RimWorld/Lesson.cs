using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034E4 RID: 13540
	public abstract class Lesson : IExposable
	{
		// Token: 0x1700318A RID: 12682
		// (get) Token: 0x06013060 RID: 77920 RVA: 0x005A3988 File Offset: 0x005A1B88
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x1700318B RID: 12683
		// (get) Token: 0x06013061 RID: 77921 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700318C RID: 12684
		// (get) Token: 0x06013062 RID: 77922 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700318D RID: 12685
		// (get) Token: 0x06013063 RID: 77923 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06013064 RID: 77924 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x06013065 RID: 77925 RVA: 0x005A39AE File Offset: 0x005A1BAE
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06013066 RID: 77926 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06013067 RID: 77927
		public abstract void LessonOnGUI();

		// Token: 0x06013068 RID: 77928 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x06013069 RID: 77929 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x0601306A RID: 77930 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x0601306B RID: 77931 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x1700318E RID: 12686
		// (get) Token: 0x0601306C RID: 77932 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400C524 RID: 50468
		public float startRealTime = -999f;

		// Token: 0x0400C525 RID: 50469
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
