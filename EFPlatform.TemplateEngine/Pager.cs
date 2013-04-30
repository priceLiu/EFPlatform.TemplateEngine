using System;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public class Pager
	{
		private int currentPage = 1;
		private int pageSize;
		private int pageCount;
		private int recordCount;

		#region Property
		public int CurrentPage
		{
			get { return currentPage; }
			set { currentPage = value; }
		}

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}

		public int PageCount
		{
			get { return pageCount; }
			set { pageCount = value; }
		}

		public int RecordCount
		{
			get { return recordCount; }
			set { recordCount = value; }
		}

		public int FirstPage
		{
			get { return 1; }
		}

		public int PreviousPage
		{
			get { return this.GetPreviousPage(); }
		}

		public int NextPage
		{
			get { return this.GetNextPage(); }
		}

		public int LastPage
		{
			get { return this.pageCount; }
		}
		#endregion

		public Pager(int pageSize, int recordCount) : this(1, pageSize, recordCount) { }

		public Pager(int currentPage, int pageSize, int recordCount)
		{
			this.currentPage = currentPage;
			this.pageSize = pageSize;
			this.recordCount = recordCount;
			this.Init();
		}

		private void Init()
		{
			if(this.pageSize > 0 && this.recordCount > 0)
			{
				this.pageCount = (int)Math.Ceiling((double)this.recordCount / this.PageSize);

				if(this.currentPage > this.pageCount)
				{
					this.currentPage = this.pageCount;
				}

				if(this.currentPage < 1)
				{
					this.currentPage = 1;
				}
			}
		}

		private int GetPreviousPage()
		{
				return this.currentPage > 1 ? this.currentPage - 1 : 1;
		}

		private int GetNextPage()
		{
			return this.currentPage < this.pageCount ? this.currentPage + 1 : this.currentPage;
		}
	}
}
