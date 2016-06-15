using System;
using System.Collections.Generic;
using System.Text;


using System.IO;

namespace System.Net.Json
{
    public class JsonObjectCollection : JsonCollection
    {

        public JsonObjectCollection()
            : base()
        {
        }

        public JsonObjectCollection(string name)
            : base(name)
        {
        }

        public JsonObjectCollection(IEnumerable<JsonObject> collection)
            : base(collection)
        {
        }

        public JsonObjectCollection(string name, IEnumerable<JsonObject> collection)
            : base(name, collection)
        {
        }

        protected override char BeginCollection
        {
            get { return '{'; }
        }

        protected override char EndCollection
        {
            get { return '}'; }
        }

        public JsonObject this[string name]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name == name)
                    {
                        return this[i];
                    }
                }

                return null;
            }
        }
    }
}