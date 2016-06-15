using System;

namespace com.lover.common.http
{
	public class HttpHeader
	{
        /// <summary>
        /// ��ȡ�����Ӧ״̬��
        /// </summary>
		public string ResponseStatusCode
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ��תurl
        /// </summary>
		public string Location
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ�Ƿ���Gzipѹ��
        /// </summary>
		public bool IsGzip
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ���ص��ĵ�����
        /// </summary>
		public string ContentType
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ����ʹ�õ��ַ���
        /// </summary>
		public string Charset
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ���ݳ���
        /// </summary>
		public long ContentLength
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ�Ƿ�ֿ鴫��
        /// </summary>
		public bool IsChunk
		{
			get;
			internal set;
		}
	}
}
