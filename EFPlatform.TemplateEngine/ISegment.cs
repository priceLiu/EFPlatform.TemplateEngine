using System;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	public interface ISegment
	{
		string Name { get;set;}
		string Code { get;set;}
		object Value { get;set;}
		SegmentType Type { get;}

		string GetData();
		void GetData(StringBuilder builder);
	}

	public enum SegmentType
	{
		Segment,
		Region,
		Field
	}
}
