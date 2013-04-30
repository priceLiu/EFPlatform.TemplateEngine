using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public class SegmentCollection : ICollection<ISegment>
	{
		private List<ISegment> itemList;
		private Hashtable segmentFromName;

		public int Count
		{
			get { return this.itemList.Count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public ISegment this[int index]
		{
			get { return this.itemList[index]; }
			set { this.itemList[index] = value; }
		}

		public ISegment this[string name]
		{
			get { return this.FindByName(name); }
			set
			{
				ISegment item = this.FindByName(name);

				if(item != null)
				{
					item = value;
				}
			}
		}

		public SegmentCollection()
		{
			this.itemList = new List<ISegment>();
			this.segmentFromName = new Hashtable();
		}

		public void Add(ISegment item)
		{
			this.itemList.Add(item);

			if(!string.IsNullOrEmpty(item.Name) && !this.segmentFromName.Contains(item.Name))
			{
				this.segmentFromName.Add(item.Name, item);
			}
		}

		public void Clear()
		{
			this.itemList.Clear();
		}

		public bool Contains(ISegment item)
		{
			return this.itemList.Contains(item);
		}

		public void CopyTo(ISegment[] array, int arrayIndex)
		{
			this.itemList.CopyTo(array, arrayIndex);
		}

		private ISegment FindByName(string name)
		{
			ISegment item = null;

			if(!string.IsNullOrEmpty(name) && this.segmentFromName.Contains(name))
			{
				item = this.segmentFromName[name] as ISegment;
			}

			return item;
		}

		public bool Remove(ISegment item)
		{
			return this.itemList.Remove(item);
		}

		public IEnumerator<ISegment> GetEnumerator()
		{
			return this.itemList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.itemList).GetEnumerator();
		}
	}
}
