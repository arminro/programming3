// <copyright file="Serializer.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System.IO;
    using System.Xml.Serialization;

    public static class Serializer<T>
        where T : class
    {
        /// <summary>
        /// XML Serialization: This class was to be responsible for serialization, until a better alternative was found
        /// </summary>
        /// <param name="item">The item to be serialized</param>
        /// <param name="fullPath">The full path of the file to be created</param>
        public static void Serialize(T item, string fullPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(fullPath))
            {
                xs.Serialize(wr, item);
            }
        }

        public static T Deserialize(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamReader rd = new StreamReader(fileName))
            {
                return xs.Deserialize(rd) as T;
            }
        }
    }
}
