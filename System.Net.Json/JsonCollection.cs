using System;
using System.Collections.Generic;
using System.Text;


using System.IO;

namespace System.Net.Json
{
    /// <summary>
    /// Implements a collection object.
    /// Object and Array collections derives from this class.
    /// </summary>
    public abstract class JsonCollection : JsonObject, IList<JsonObject>
    {
        private bool? _isArray = new bool?();
        private bool IsArray
        {
            get
            {
                if (!_isArray.HasValue)
                {
                    _isArray = this.GetType() == typeof(JsonArrayCollection);
                }

                return _isArray.Value;
            }
        }

        private List<JsonObject> _list = new List<JsonObject>();

        /// <summary>
        /// Initializes a new instance of the JsonCollection class.
        /// </summary>
        public JsonCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the JsonCollection class.
        /// </summary>
        /// <param name="name">Collection's name,
        /// used then current collection is nested in other json object.</param>
        public JsonCollection(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the JsonCollection class.
        /// </summary>
        /// <param name="collection">Collection of nested objects.</param>
        public JsonCollection(IEnumerable<JsonObject> collection)
        {
            _list.AddRange(collection);
        }

        /// <summary>
        /// Initializes a new instance of the JsonCollection class.
        /// </summary>
        /// <param name="name">Collection's name,
        /// used then current collection is nested in other json object.</param>
        /// <param name="collection">Collection of nested objects.</param>
        public JsonCollection(string name, IEnumerable<JsonObject> collection)
        {
            Name = name;
            _list.AddRange(collection);
        }

        /// <summary>
        /// Returns string that represents object is json format.
        /// </summary>
        /// <returns>String that represents object is json format.</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Begin collection marker. '[' for array, '{' for objects.
        /// </summary>
        protected abstract char BeginCollection { get; }

        /// <summary>
        /// End collection marker. ']' for array, '}' for objects.
        /// </summary>
        protected abstract char EndCollection { get; }

        /// <summary>
        /// Writes current JsonCollection in json data format to TextWriter.
        /// </summary>
        /// <param name="writer">Textwriter where to write current JsonCollection.</param>
        public override void WriteTo(TextWriter writer)
        {
            if (Name != string.Empty)
            {
                writer.Write(JsonUtility.quote);
                writer.Write(Name);
                writer.Write(JsonUtility.quote);
                writer.Write(JsonUtility.name_separator);
                JsonUtility.WriteSpace(writer);
            }

            writer.Write(BeginCollection);

            JsonUtility.WriteLine(writer);
            JsonUtility.IndentDepth++;
            JsonUtility.WriteIndent(writer);

            for (int i = 0; i < this.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(JsonUtility.value_separator);
                    JsonUtility.WriteLine(writer);
                    JsonUtility.WriteIndent(writer);
                }

                this[i].WriteTo(writer);
            }

            JsonUtility.WriteLine(writer);
            JsonUtility.IndentDepth--;
            JsonUtility.WriteIndent(writer);

            writer.Write(EndCollection);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override object GetValue()
        {
            return _list;
        }

        #region IList<JsonObject> Members

        public int IndexOf(JsonObject item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, JsonObject item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public JsonObject this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        #endregion

        #region ICollection<JsonObject> Members

        public void Add(JsonObject item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(JsonObject item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(JsonObject[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(JsonObject item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<JsonObject> Members

        public IEnumerator<JsonObject> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}