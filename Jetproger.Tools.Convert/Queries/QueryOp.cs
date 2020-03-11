using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public enum QueryOp
    {
        [DataMember][Description("NULL")]Null,
        [DataMember][Description("!NULL")]NotNull,
        [DataMember][Description("=")]Equal,
        [DataMember][Description("!=")]NotEqual,
        [DataMember][Description(">")]More,
        [DataMember][Description("<=")]NotMore,
        [DataMember][Description(">=")]MoreEqual,
        [DataMember][Description("<")]NotMoreEqual,
        [DataMember][Description("<")]Less,
        [DataMember][Description(">=")]NotLess,
        [DataMember][Description("<=")]LessEqual,
        [DataMember][Description(">")]NotLessEqual,
        [DataMember][Description("s%")]StartWith,
        [DataMember][Description("!s%")]NotStartWith,
        [DataMember][Description("%s")]EndWith,
        [DataMember][Description("!%s")]NotEndWith,
        [DataMember][Description("%s%")]Contains,
        [DataMember][Description("!%s%")]NotContains,
        [DataMember][Description("(,)")]In,
        [DataMember][Description("!(,)")]NotIn,
        [DataMember][Description("><")]Between,
        [DataMember][Description("!><")]NotBetween,
    }
}