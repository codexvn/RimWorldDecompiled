using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D14 RID: 7444
	public class RelationshipRecord : IExposable, ILoadReferenceable
	{
		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x0600A678 RID: 42616 RVA: 0x002FA7D7 File Offset: 0x002F89D7
		public int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x0600A679 RID: 42617 RVA: 0x002FA7DF File Offset: 0x002F89DF
		public Gender Gender
		{
			get
			{
				return this.gender;
			}
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x0600A67A RID: 42618 RVA: 0x002FA7E7 File Offset: 0x002F89E7
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600A67B RID: 42619 RVA: 0x002FA7EF File Offset: 0x002F89EF
		public IReadOnlyCollection<Pawn> References
		{
			get
			{
				if (this.references.Contains(null))
				{
					this.references.Remove(null);
				}
				return this.references;
			}
		}

		// Token: 0x0600A67C RID: 42620 RVA: 0x002FA812 File Offset: 0x002F8A12
		public RelationshipRecord()
		{
		}

		// Token: 0x0600A67D RID: 42621 RVA: 0x002FA825 File Offset: 0x002F8A25
		public RelationshipRecord(int id, Gender gender, string name)
		{
			this.id = id;
			this.gender = gender;
			this.name = name;
		}

		// Token: 0x0600A67E RID: 42622 RVA: 0x002FA84D File Offset: 0x002F8A4D
		public void AddReference(Pawn pawn)
		{
			if (pawn == null)
			{
				Log.Error("Attempted to create relationship record reference from a pawn which is null!");
				return;
			}
			this.references.Add(pawn);
		}

		// Token: 0x0600A67F RID: 42623 RVA: 0x002FA86A File Offset: 0x002F8A6A
		public void RemoveReference(Pawn pawn)
		{
			if (this.references.Contains(pawn))
			{
				this.references.Remove(pawn);
			}
		}

		// Token: 0x0600A680 RID: 42624 RVA: 0x002FA888 File Offset: 0x002F8A88
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Collections.Look<Pawn>(ref this.references, true, "references", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.references.Contains(null))
			{
				this.references.Remove(null);
			}
		}

		// Token: 0x0600A681 RID: 42625 RVA: 0x002FA900 File Offset: 0x002F8B00
		public string GetUniqueLoadID()
		{
			return string.Format("RelationshipRecord_{0}", this.ID);
		}

		// Token: 0x04007348 RID: 29512
		private int id;

		// Token: 0x04007349 RID: 29513
		private Gender gender;

		// Token: 0x0400734A RID: 29514
		private string name;

		// Token: 0x0400734B RID: 29515
		private HashSet<Pawn> references = new HashSet<Pawn>();
	}
}
