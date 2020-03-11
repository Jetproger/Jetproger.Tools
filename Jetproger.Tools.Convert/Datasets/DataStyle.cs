using System;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public enum DataStyle
    {
        [DataMember]String,
        [DataMember]Char,
        [DataMember]NullChar,
        [DataMember]Bool,
        [DataMember]NullBool,
        [DataMember]Byte,
        [DataMember]NullByte,
        [DataMember]Sbyte,
        [DataMember]NullSbyte,
        [DataMember]Short,
        [DataMember]NullShort,
        [DataMember]Ushort,
        [DataMember]NullUshort,
        [DataMember]Int,
        [DataMember]NullInt,
        [DataMember]Uint,
        [DataMember]NullUint,
        [DataMember]Long,
        [DataMember]NullLong,
        [DataMember]Ulong,
        [DataMember]NullUlong,
        [DataMember]Guid,
        [DataMember]NullGuid,
        [DataMember]Float,
        [DataMember]NullFloat,
        [DataMember]Decimal,
        [DataMember]NullDecimal,
        [DataMember]Double,
        [DataMember]NullDouble,
        [DataMember]Datetime,
        [DataMember]NullDatetime,
    }
}