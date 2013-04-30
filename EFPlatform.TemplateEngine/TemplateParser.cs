using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EFPlatform.TemplateEngine
{
	/// <summary>
	/// CodeParser 的摘要说明
	/// </summary>
	public class TemplateParser
	{
		private static string patternRegion = @"\<!--!(?<Name>[#\w]*)--\>(?<Template>.*?)\<!--\k<Name>!--\>"; // example: <!--!Title-->Category: {CategoryName}<!--Title!-->
        private static string patternField = @"{(?<Name>[\w]+)(?<Format>,"".*?"")?(?<Length>,[\d]*)?}"; // example: Category: {CategoryName}
		private static readonly RegexOptions regexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
		private static Regex regexRegion;
		private static Regex regexField;

		static TemplateParser()
		{
			regexRegion = new Regex(patternRegion, regexOptions);
			regexField = new Regex(patternField, regexOptions);
		}

		#region GetSegments
		public static SegmentCollection GetRegions(string code)
		{
			MatchCollection mc = regexRegion.Matches(code);
			SegmentCollection segments = new SegmentCollection();
			string segment;
			int position = 0;

			foreach(Match m in mc)
			{
				segment = code.Substring(position, m.Index - position); //截取到时标志位前的内容
				position = m.Index + m.Length;
				segments.Add(new Segment(segment));  //添加正文片断

                string content = m.Groups["Template"].Value; //除去起始标签的内容部分
                string nameTag = m.Groups["Name"].Value; //起始标签的标识名; 例如： <!--!Title-->   , 结果： Title
                string matchResult = m.Value; //匹配后的整个结果; 例如： 最后结果<!--!Title-->Category: {CategoryName}<!--Title!-->
				segments.Add(new Region(nameTag, matchResult, content)); //添加要替换部分的信息
			}

			segment = code.Substring(position, code.Length - position);  //获取最后剩余的内容部分
			segments.Add(new Segment(segment));
			return segments;


            /*example:
             * content:     <title><!--!Title-->Category: {CategoryName}<!--Title!--> Of Northwind</title>
             * result:
             *      [0] : <title>                                                           --  segment
             *      [1] : <!--!Title-->Category: {CategoryName}<!--Title!-->                --  region
             *                                                                                      [0]:     Category:              -- segment
             *                                                                                      [1]:     {CategoryName}         -- field
             *                                                                                      [2]:     ""                     -- segment
             *      [2] : of Northwind</title>                                              --  segment
             */
        }
		#endregion

		#region GetFields
		public static SegmentCollection GetFields(string code)
		{
			MatchCollection mc = regexField.Matches(code);
			SegmentCollection segments = new SegmentCollection();
			string name;
			string format;
			string length;
			int len = 0;
			string segment;
			int position = 0;

			foreach(Match m in mc)
			{
				segment = code.Substring(position, m.Index - position);
                segments.Add(new Segment(segment));  //添加正文片断
				position = m.Index + m.Length;
				name = m.Groups["Name"].Value;
				format = m.Groups["Format"].Value;
				length = m.Groups["Length"].Value;

				if(string.IsNullOrEmpty(name))
				{
					name = string.Empty;
				}

				if(string.IsNullOrEmpty(format))
				{
					format = string.Empty;
				}
				else
				{
					format = format.Substring(2);
					format = format.Substring(0, format.Length - 1);
				}

				if(!string.IsNullOrEmpty(length))
				{
					int.TryParse(length.Substring(1), out len);
				}

				segments.Add(new Field(m.Value, name, len, format));
			}

			segment = code.Substring(position, code.Length - position);
			segments.Add(new Segment(segment));
			return segments;
		}
		#endregion
	}
}
