using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.model
{
    public class Specialtreasure : AsObject
    {
        public int upgradestate_;
        public int consecratestatus_;
        public string generalname_;
        public int attribute_lea_;
        public int attribute_str_;
        public int attribute_int_;
        public int additionalattributelvmax_;
        public int maxadd_;
        public string intro_;
        public int quality_;
        public float succprob_;

        public Specialtreasure()
        {
            _id = 0;
            upgradestate_ = 0;
            consecratestatus_ = 0;
            generalname_ = "";
            attribute_lea_ = 0;
            attribute_str_ = 0;
            attribute_int_ = 0;
            additionalattributelvmax_ = 0;
            maxadd_ = 0;
            intro_ = "";
            quality_ = 0;
            _name = "";
            succprob_ = 0.0f;
        }

        public string NameWithGeneral
        {
            get { return string.Format("{0}({1})", this._name, this.generalname_); }
        }

        public string Attributes
        {
            get { return string.Format("统+{0} 勇+{1} 智+{2}", this.attribute_lea_, this.attribute_str_, this.attribute_int_); }
        }

        public bool CanUpgrade
        {
            get { return (this.attribute_lea_ < this.maxadd_ || this.attribute_str_ < this.maxadd_ || this.attribute_int_ < this.maxadd_); }
        }
    }
}
