using System;

namespace com.lover.common.http
{
	public class HttpHeader
	{
        /// <summary>
        /// 获取请求回应状态码
        /// </summary>
		public string ResponseStatusCode
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取跳转url
        /// </summary>
		public string Location
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取是否由Gzip压缩
        /// </summary>
		public bool IsGzip
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取返回的文档类型
        /// </summary>
		public string ContentType
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取内容使用的字符集
        /// </summary>
		public string Charset
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取内容长度
        /// </summary>
		public long ContentLength
		{
			get;
			internal set;
		}

        /// <summary>
        /// 获取是否分块传输
        /// </summary>
		public bool IsChunk
		{
			get;
			internal set;
		}
	}
}
