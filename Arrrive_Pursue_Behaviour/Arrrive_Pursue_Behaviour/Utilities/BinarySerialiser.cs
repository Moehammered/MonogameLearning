using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arrrive_Pursue_Behaviour.Utilities
{
    class BinarySerialiser
    {
        private MemoryStream memoryStream;
        private BinaryFormatter formatter;

        public BinarySerialiser()
        {
            formatter = new BinaryFormatter();
        }

        public T loadData<T>(string serial)
        {
            object data = null;
            if (serial == string.Empty)
            {
                data = "";
                return default(T);
            }
            else
            {
                using (memoryStream = new MemoryStream(System.Convert.FromBase64String(serial)))
                {
                    data = formatter.Deserialize(memoryStream);
                }
            }

            return (T)data;
        }

        public object loadData(string serial)
        {
            object data = null;
            if (serial == string.Empty)
            {
                return null;
            }
            else
            {
                using (memoryStream = new MemoryStream(Convert.FromBase64String(serial)))
                {
                    data = formatter.Deserialize(memoryStream);
                }
            }

            return data;
        }

        public string serialiseObject(object obj)
        {
            using (memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, obj);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        
    }
}
