using System;
using System.Collections;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public class Region : ISegment
	{
		private string name;
		private string code;
		private SegmentType type = SegmentType.Region;
		private string template;
		private object dataSource;
		private SegmentCollection segments;

		#region
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

		public string Template
		{
			get { return template; }
			set { template = value; }
		}

		public object Value
		{
			get { return this.dataSource; }
			set { this.dataSource = value; }
		}

		public object DataSource
		{
			get { return this.dataSource; }
			set { this.dataSource = value; }
		}

		public SegmentCollection Fields
		{
			get { return segments; }
		} 
		#endregion

		public Region(string name, string code, string template)
		{
			this.name = name;
			this.code = code;
			this.template = template;
			this.segments = TemplateParser.GetFields(this.template);
		}

		public string GetData()
		{
			StringBuilder sb = new StringBuilder();
			this.GetData(sb);
			return sb.ToString();
		}

		public void GetData(StringBuilder builder)
		{
			if(this.dataSource != null)
			{
				IEnumerable dataSource = Helper.GetResolvedDataSource(this.dataSource);

				if(dataSource != null)
				{
					int index = 1;

					foreach(object data in dataSource)
					{
						index = this.GetData(builder, data, index);
					}
				}
				else
				{
					this.GetData(builder, this.dataSource, 0);
				}
			}
		}

		private int GetData(StringBuilder builder, object data, int index)
		{
			foreach(ISegment segment in this.segments)
			{
				if(segment.Type == SegmentType.Field)
				{
					if(segment.Name == Field.IndexName)
					{
						segment.Value = index;
						index++;
					}
					else
					{
						segment.Value = data;
					}
				}

				segment.GetData(builder);
			}

			return index;
		}
	}
}
