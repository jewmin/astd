using System;

namespace com.lover.astd.common.model.attack
{
    /// <summary>
    /// ������
    /// </summary>
	public class UserToken
	{
        /// <summary>
        /// Id,Э����
        /// </summary>
		public int id;
        /// <summary>
        /// ����,-1:δ����,1:������,2:�ƻ���,4:������,5:�̰���,7:��ȡ��,8:ս����,9:��ɨ��
        /// </summary>
		public int tokenid;
        /// <summary>
        /// ����
        /// </summary>
		public string name;
        /// <summary>
        /// �ȼ�
        /// </summary>
		public int level;
        /// <summary>
        /// Ч��
        /// </summary>
        public float effect;
	}
}
