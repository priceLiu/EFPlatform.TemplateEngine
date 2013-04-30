using System;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public class Segment : ISegment
	{
        /*
         * ÕýÎÄÆ¬¶Ï
         */ 

		private string name = null;
		private string code;
		private SegmentType type = SegmentType.Segment;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Code
		{
			get { return code; }
			set { code = value; }
		}

		public SegmentType Type
		{
			get { return type; }
		}

		public object Value
		{
			get { return code; }
			set { code = value.ToString(); }
		}

		public Segment(string code)
		{
			this.code = code;
		}

		public string GetData()
		{
			return this.code;
		}

		public void GetData(StringBuilder builder)
		{
			builder.Append(this.code);
		}

	}
}
