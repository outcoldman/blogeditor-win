// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------
// 
namespace OutcoldSolutions.BlogEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.ServiceModel.Channels;
    using System.Xml;

    internal class XmlRpcDataContractSerializationHelper
    {
        private const string DefaultDateTimeFormat = "yyyyMMddTHH:mm:ss";

        internal static XmlDictionaryReader CreateFaultReader(MessageFault fault)
        {
            return null;
        }

        private static object DeserializeStruct(XmlDictionaryReader reader, Type targetType)
        {
            if (targetType.GetTypeInfo().IsDefined(typeof(DataContractAttribute), false))
            {
                Dictionary<string, MemberInfo> dataMembers = GetDataMembers(targetType);
                object targetObject = Activator.CreateInstance(targetType);

                reader.ReadStartElement(XmlRpcProtocol.Struct);

                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        reader.ReadContentAsString();
                        continue;
                    }

                    string memberName;

                    reader.ReadStartElement(XmlRpcProtocol.Member);
                    reader.ReadStartElement(XmlRpcProtocol.Name);
                    memberName = reader.ReadContentAsString();
                    reader.ReadEndElement();

                    reader.ReadStartElement(XmlRpcProtocol.Value);
                    reader.MoveToContent();
                    if (dataMembers.ContainsKey(memberName))
                    {
                        MemberInfo member = dataMembers[memberName];
                        if (member is PropertyInfo)
                        {
                            ((PropertyInfo)member).SetValue(
                                targetObject,
                                Deserialize(reader, ((PropertyInfo)member).PropertyType));
                        }
                        else if (member is FieldInfo)
                        {
                            ((FieldInfo)member).SetValue(
                                targetObject,
                                Deserialize(reader, ((FieldInfo)member).FieldType));
                        }
                    }
                    reader.ReadEndElement(); // value
                    reader.ReadEndElement(); // member
                }
                reader.ReadEndElement(); // struct
                reader.MoveToContent();
                return targetObject;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static Dictionary<string, MemberInfo> GetDataMembers(Type targetType)
        {
            Dictionary<string, MemberInfo> dataMembers = new Dictionary<string, MemberInfo>();
            foreach (MemberInfo member in targetType.GetTypeInfo().DeclaredProperties)
            {
                object[] attributes = member.GetCustomAttributes(typeof(DataMemberAttribute), true).ToArray();
                if (attributes.Length == 1)
                {
                    DataMemberAttribute dataMember = (DataMemberAttribute)attributes[0];
                    if (!string.IsNullOrEmpty(dataMember.Name))
                    {
                        dataMembers.Add(dataMember.Name, member);
                    }
                    else
                    {
                        dataMembers.Add(member.Name, member);
                    }
                }
            }
            return dataMembers;
        }

        public static object Deserialize(XmlDictionaryReader reader, Type targetType)
        {
            object returnValue = null;

            if (reader.IsStartElement())
            {
                switch (reader.LocalName)
                {
                    case XmlRpcProtocol.Nil:
                        returnValue = null;
                        break;
                    case XmlRpcProtocol.Bool:
                        returnValue = Convert.ChangeType((reader.ReadElementContentAsInt() == 1), targetType);
                        break;
                    case XmlRpcProtocol.ByteArray:
                        if (targetType == typeof(Stream))
                        {
                            returnValue = new MemoryStream(reader.ReadElementContentAsBase64());
                        }
                        else
                        {
                            returnValue = Convert.ChangeType(reader.ReadElementContentAsBase64(), targetType);
                        }
                        break;
                    case XmlRpcProtocol.DateTime:
                        string dateTime = reader.ReadElementContentAsString();
                        returnValue = ToDateTime(dateTime, DefaultDateTimeFormat);
                        //returnValue = Convert.ChangeType(reader.ReadElementContentAsDateTime(), targetType);
                        break;
                    case XmlRpcProtocol.Double:
                        returnValue = Convert.ChangeType(reader.ReadElementContentAsDouble(), targetType);
                        break;
                    case XmlRpcProtocol.Int32:
                    case XmlRpcProtocol.Integer:
                        returnValue = Convert.ChangeType(reader.ReadElementContentAsString(), targetType);
                        break;
                    case XmlRpcProtocol.String:
                        if (targetType == typeof(Uri))
                        {
                            returnValue = new Uri(reader.ReadElementContentAsString());
                        }
                        else
                        {
                            returnValue = Convert.ChangeType(reader.ReadElementContentAsString(), targetType);
                        }
                        break;
                    case XmlRpcProtocol.Struct:
                        returnValue = DeserializeStruct(reader, targetType);
                        break;
                    case XmlRpcProtocol.Array:
                        if (targetType.IsArray || targetType is IEnumerable || targetType is IList || targetType is ICollection)
                        {
                            reader.ReadStartElement(XmlRpcProtocol.Array);
                            List<object> arrayData = new List<object>();
                            reader.ReadStartElement(XmlRpcProtocol.Data);
                            reader.MoveToContent();
                            while (reader.IsStartElement(XmlRpcProtocol.Value))
                            {
                                reader.ReadStartElement();
                                arrayData.Add(Deserialize(reader, targetType.GetElementType()));
                                reader.ReadEndElement();
                                reader.MoveToContent();
                            }
                            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == XmlRpcProtocol.Data)
                            {
                                reader.ReadEndElement();
                            }
                            reader.ReadEndElement();

                            if (targetType is IEnumerable || targetType is IList || targetType is ICollection)
                            {
                                returnValue = arrayData;
                            }
                            else
                            {
                                returnValue = arrayData.ToArray();
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                        break;
                }
            }
            return returnValue;
        }

        public static DateTime ToDateTime(object obj, string format)
        {
            if (obj == null)
                return default(DateTime);
            try
            {
                IFormatProvider culture = new CultureInfo("en-US");
                return DateTime.ParseExact(obj.ToString(), format, culture);
            }
            catch (Exception e)
            {

                return default(DateTime);
            }

        }

        public static void SerializeStruct(XmlDictionaryWriter writer, object value)
        {
            Type valueType = value.GetType();
            Dictionary<string, MemberInfo> dataMembers = GetDataMembers(valueType);
            if (valueType.GetTypeInfo().IsDefined(typeof(DataContractAttribute), false))
            {
                writer.WriteStartElement(XmlRpcProtocol.Struct);
                foreach (KeyValuePair<string, MemberInfo> member in dataMembers)
                {
                    object elementValue = null;

                    if (member.Value is PropertyInfo)
                    {
                        elementValue = ((PropertyInfo)member.Value).GetValue(value);
                    }
                    else if (member.Value is FieldInfo)
                    {
                        elementValue = ((FieldInfo)member.Value).GetValue(value);
                    }

                    if (elementValue != null)
                    {
                        writer.WriteStartElement(XmlRpcProtocol.Member);
                        writer.WriteStartElement(XmlRpcProtocol.Name);
                        writer.WriteString(member.Key);
                        writer.WriteEndElement();
                        writer.WriteStartElement(XmlRpcProtocol.Value);
                        Serialize(writer, elementValue);
                        writer.WriteEndElement(); // value
                        writer.WriteEndElement(); // member
                    }
                }
                writer.WriteEndElement(); // struct
            }
        }

        public static void Serialize(XmlDictionaryWriter writer, object value)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType.GetTypeInfo().IsDefined(typeof(DataContractAttribute), false))
                {
                    SerializeStruct(writer, value);
                }
                else if (valueType == typeof(Int32))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Integer);
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(double) || valueType == typeof(float))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Double);
                    writer.WriteValue(((double)value).ToString("r"));
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(DateTime))
                {
                    writer.WriteStartElement(XmlRpcProtocol.DateTime);
                    writer.WriteValue(((DateTime)value).ToString(DefaultDateTimeFormat));
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Boolean))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Bool);
                    writer.WriteValue(((bool)value) ? 1 : 0);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(String) ||
                         valueType == typeof(TimeSpan))
                {
                    writer.WriteStartElement(XmlRpcProtocol.String);
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Uri))
                {
                    writer.WriteStartElement(XmlRpcProtocol.String);
                    writer.WriteValue((value).ToString());
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(byte[]))
                {
                    writer.WriteStartElement(XmlRpcProtocol.ByteArray);
                    writer.WriteBase64((byte[])value, 0, ((byte[])value).Length);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Stream))
                {
                    int chunkSize = 1024 * 5;
                    byte[] buffer = new byte[chunkSize];
                    int offset = 0;
                    int bytesRead;

                    writer.WriteStartElement(XmlRpcProtocol.ByteArray);
                    do
                    {
                        bytesRead = ((Stream)value).Read(buffer, offset, buffer.Length);
                        writer.WriteBase64(buffer, 0, bytesRead);
                        offset += bytesRead;
                    } while (bytesRead == buffer.Length);
                    writer.WriteEndElement();
                }
                else if (value is IEnumerable)
                {
                    writer.WriteStartElement(XmlRpcProtocol.Array);
                    writer.WriteStartElement(XmlRpcProtocol.Data);
                    foreach (object obj in (IEnumerable)value)
                    {
                        writer.WriteStartElement(XmlRpcProtocol.Value);
                        Serialize(writer, obj);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement(XmlRpcProtocol.Nil);
                writer.WriteEndElement();
            }
        }
    }
}