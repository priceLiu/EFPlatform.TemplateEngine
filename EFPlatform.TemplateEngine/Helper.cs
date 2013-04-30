using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace EFPlatform.TemplateEngine
{
	/// <summary>
	/// StringHelper 的摘要说明
	/// </summary>
	public class Helper
	{
		private static Encoding defaultEncoding = Encoding.UTF8;

		#region GetLength
		public static int GetLength(string sourceString)
		{
			int length = 0;
			char[] chars = sourceString.ToCharArray();

			for(int i = 0; i < chars.Length; i++)
			{
				byte[] bytes = Encoding.Default.GetBytes(chars, i, 1);
				length += bytes.Length;
			}

			return length;
		}
		#endregion

		#region Substring
		public static string Substring(string sourceString, int length)
		{
			int length1 = 0;
			int length2 = 0;
			char[] chars = sourceString.ToCharArray();

			for(int i = 0; i < chars.Length; i++)
			{
				byte[] bytes = Encoding.Default.GetBytes(chars, i, 1);
				length1 += bytes.Length;
				length2 = i + 1;

				if(length1 == length)
				{
					break;
				}
				else if(length1 > length)
				{
					length2 += -1;
					break;
				}
			}

			return sourceString.Substring(0, length2);
		}
		#endregion

		#region CutString
		public static string CutString(string sourceString, int length, string ellipsis)
		{
			string output = string.Empty;

			if(length > 0 && !string.IsNullOrEmpty(sourceString))
			{
				if(ellipsis == null)
				{
					ellipsis = "...";
				}

				int sourceLength = GetLength(sourceString);

				if(sourceLength > length)
				{
					if(ellipsis.Length > length)
					{
						throw new Exception("Ellipsis is longer than output string");
					}
					else
					{
						output = Substring(sourceString, length - ellipsis.Length) + ellipsis;
					}
				}
				else
				{
					output = sourceString;
				}
			}

			return output;
		}
		#endregion

		#region GetBindingValue
		public static object GetBindingValue(object obj, string propertyName)
		{
			object value = null;

			if(string.IsNullOrEmpty(propertyName))
			{
				value = obj;
			}
			else
			{
				PropertyDescriptor pd = TypeDescriptor.GetProperties(obj).Find(propertyName, true);

				if(pd != null)
				{
					value = pd.GetValue(obj);
				}
				else
				{
					value = obj;
				}
			}

			return value;
		}
		#endregion

		#region FillModel
		public static void FillModel(object model, IDataReader dr)
		{
			Type type = model.GetType();
			PropertyInfo pi;

			for(int i = 0; i < dr.FieldCount; i++)
			{
				pi = type.GetProperty(dr.GetName(i));

				if(pi != null)
				{
					pi.SetValue(model, dr[i], null);
				}
			}
		}
		#endregion

		#region ReadTextFile
		/// <summary>
		/// ReadTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <returns>文本内容</returns>
		public static string ReadTextFile(string fileName)
		{
			return ReadTextFile(fileName, defaultEncoding);
		}

		/// <summary>
		/// ReadTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="encoding">编码</param>
		/// <returns>文本内容</returns>
		public static string ReadTextFile(string fileName, Encoding encoding)
		{
			string text;

			using(StreamReader sr = new StreamReader(fileName, encoding))
			{
				text = sr.ReadToEnd();
			}

			return text;
		}
		#endregion

		#region WriteTextFile
		/// <summary>
		/// WriteTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="text">文本内容</param>
		public static void WriteTextFile(string fileName, string text)
		{
			WriteTextFile(fileName, text, false, true, defaultEncoding);
		}

		/// <summary>
		/// WriteTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="text">文本内容</param>
		/// <param name="encoding">编码</param>	
		public static void WriteTextFile(string fileName, string text, Encoding encoding)
		{
			WriteTextFile(fileName, text, false, true, encoding);
		}

		/// <summary>
		/// WriteTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="text">文本内容</param>
		/// <param name="createDir">是否创建目录</param>		
		public static void WriteTextFile(string fileName, string text, bool createDir)
		{
			WriteTextFile(fileName, text, false, createDir, defaultEncoding);
		}

		/// <summary>
		/// WriteTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="text">文本内容</param>
		/// <param name="createDir">是否创建目录</param>
		/// <param name="encoding">编码</param>	
		public static void WriteTextFile(string fileName, string text, bool createDir, Encoding encoding)
		{
			WriteTextFile(fileName, text, false, createDir, encoding);
		}

		/// <summary>
		/// WriteTextFile
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="text">文本内容</param>
		/// <param name="append">是否添加到文本后面</param>
		/// <param name="createDir">是否创建目录</param>
		/// <param name="encoding">编码</param>
		public static void WriteTextFile(string fileName, string text, bool append, bool createDir, Encoding encoding)
		{
			if(createDir)
			{
				string dirName = Path.GetDirectoryName(fileName);

				if(!Directory.Exists(dirName))
				{
					Directory.CreateDirectory(dirName);
				}
			}

			using(StreamWriter sw = new StreamWriter(fileName, append, encoding))
			{
				sw.Write(text);
			}
		}
		#endregion

		#region GetResolvedDataSource
		/// <summary>
		/// ResolveDataSource
		/// </summary>
		/// <param name="dataSource">数据源</param>
		/// <returns>IEnumerable型数据源</returns>
		public static IEnumerable GetResolvedDataSource(object dataSource)
		{
			return GetResolvedDataSource(dataSource, null);
		}
		/// <summary>
		/// GetResolvedDataSource
		/// </summary>
		/// <param name="dataSource">数据源</param>
		/// <param name="dataMember">数据成员</param>
		/// <returns>IEnumerable型数据源</returns>
		public static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
		{
			if(dataSource != null)
			{
				IListSource source1 = dataSource as IListSource;

				if(source1 != null)
				{
					IList list1 = source1.GetList();

					if(!source1.ContainsListCollection)
					{
						return list1;
					}

					if((list1 != null) && (list1 is ITypedList))
					{
						ITypedList list2 = (ITypedList)list1;
						PropertyDescriptorCollection collection1 = list2.GetItemProperties(new PropertyDescriptor[0]);

						if((collection1 == null) || (collection1.Count == 0))
						{
							throw new ArgumentException("ListSource Without DataMembers");
						}

						PropertyDescriptor descriptor1 = null;

						if(string.IsNullOrEmpty(dataMember))
						{
							descriptor1 = collection1[0];
						}
						else
						{
							descriptor1 = collection1.Find(dataMember, true);
						}

						if(descriptor1 != null)
						{
							object obj1 = list1[0];
							object obj2 = descriptor1.GetValue(obj1);

							if((obj2 != null) && (obj2 is IEnumerable))
							{
								return (IEnumerable)obj2;
							}
						}

						throw new ArgumentException("ListSource Missing DataMember");
					}
				}
				else if(dataSource is string)
				{
					return null;
				}
				else if(dataSource is IEnumerable)
				{
					return (IEnumerable)dataSource;
				}
			}

			return null;
		}
		#endregion
	}
}