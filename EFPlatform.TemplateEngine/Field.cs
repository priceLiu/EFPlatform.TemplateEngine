using System;
using System.Collections.Generic;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public class Field : ISegment
	{
        /*
         * ÐèÒªÌæ»»µÄ×Ö¶Î
         */ 

		public static readonly string IndexName = "#Index";

		#region Fields
		private string name;
		private object value;
		private int length;
		private string format;
		private string code;
		private SegmentType type = SegmentType.Field;
		#endregion

		#region Properties
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public object Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public int Length
		{
			get { return this.length; }
			set { this.length = value; }
		}

		public string Format
		{
			get { return this.format; }
			set { this.format = value; }
		}

		public string Code
		{
			get { return this.code; }
			set { this.code = value; }
		}

		public SegmentType Type
		{
			get { return type; }
		}

		#endregion

		public Field() { }

		public Field(string code, string name, int length, string format)
		{
			this.code = code;
			this.name = name;
			this.length = length;
			this.format = format;
		}

		public string GetData()
		{
			object value = Helper.GetBindingValue(this.value, this.name);
			string data = string.Empty;

			if(value != null)
			{
				if(string.IsNullOrEmpty(this.format))
				{
					data = value.ToString();
				}
				else
				{
					data = string.Format(this.format, value);
				}

				if(this.length > 0)
				{
					data = Helper.CutString(data, this.length, string.Empty);
				}
			}

			return data;
		}

		public void GetData(StringBuilder builder)
		{
			builder.Append(this.GetData());
		}
	}
}
