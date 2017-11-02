using Beadando.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Beadando.ViewModel
{
    public static class Serializer<T> where T : class
    {
        /// <summary>
        /// Deals with de/serializing the whole amount of data accumulated in BL
        /// </summary>
        /// <param name="bl"></param>
        public static void Serialize(T item, string fullPath)
        {

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(fullPath))
            {
                xs.Serialize(wr, item);
            }
        }

        public static void Serialize()
        {
            //foreach (Type t in )
            //{
            //
            //}
        }

        public static T Deserialize(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamReader rd = new StreamReader(fileName))
            {
                return xs.Deserialize(rd) as T;
            }
        }

        public static class SerializeDictionary<TKey, TValue>
        {
            public void AddToSerializable(Dictionary<TKey, TValue> dictToSerialize)
            {

            }
        }



    }
}
