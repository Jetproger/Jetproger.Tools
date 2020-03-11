using System;
using System.Collections; 
using System.Linq;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jc
{
    public class NotBetweenMatch : BetweenMatch { public NotBetweenMatch() : base(true) { } }
    public class BetweenMatch : QueryMatch
    {
        public BetweenMatch() : base(false) { }
        public BetweenMatch(bool isNot) : base(isNot) { }

        protected override bool Matching(object value, object sample)
        {
            var items = sample as IEnumerable;
            if (items == null) return false;
            object x1 = null, x2 = null;
            var i = 0;
            foreach (var item in items)
            {
                if (i == 0) x1 = item;
                if (i == 1) x2 = item;
                if (i++ > 1) break;
            }
            if (x1 == null || x1 == DBNull.Value || x2 == null || x2 == DBNull.Value) return false;
            return (new MoreEqualMatch()).IsMatch(value, x1) && (new LessEqualMatch()).IsMatch(value, x2);
        }
    }

    public class NotLessMatch : MoreEqualMatch { public NotLessMatch() : base(false) { } }
    public class LessMatch : MoreEqualMatch { public LessMatch() : base(true) { } }
    public class NotMoreEqualMatch : MoreEqualMatch { public NotMoreEqualMatch() : base(true) { } }
    public class MoreEqualMatch : QueryMatch
    {
        public MoreEqualMatch() : base(false) { }
        public MoreEqualMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return (new EqualMatch()).IsMatch(value, sample) || (new MoreMatch()).IsMatch(value, sample); }
    }

    public class NotLessEqualMatch : MoreMatch { public NotLessEqualMatch() : base(false) { } }
    public class LessEqualMatch : MoreMatch { public LessEqualMatch() : base(true) { } }
    public class NotMoreMatch : MoreMatch { public NotMoreMatch() : base(true) { } }
    public class MoreMatch : QueryMatch
    {
        public MoreMatch() : base(false) { }
        public MoreMatch(bool isNot) : base(isNot) { }

        protected override bool Matching(object value, object sample)
        {
            IComparable comparableValue, comparableSample;
            ToComparables(value, sample, out comparableValue, out comparableSample);
            return comparableValue.CompareTo(comparableSample) > 0;
        }
    }

    public class NotContainsMatch : ContainsMatch { public NotContainsMatch() : base(true) { } }
    public class ContainsMatch : QueryMatch
    {
        public ContainsMatch() : base(false) { }
        public ContainsMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return value != null && value != DBNull.Value && sample != null && sample != DBNull.Value && value.ToString().ToLower().Contains(sample.ToString().ToLower()); }
    }

    public class NotEndWithMatch : EndWithMatch { public NotEndWithMatch() : base(true) { } }
    public class EndWithMatch : QueryMatch
    {
        public EndWithMatch() : base(false) { }
        public EndWithMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return value != null && value != DBNull.Value && sample != null && sample != DBNull.Value && value.ToString().ToLower().EndsWith(sample.ToString().ToLower()); }
    }

    public class NotStartWithMatch : StartWithMatch { public NotStartWithMatch() : base(true) { } }
    public class StartWithMatch : QueryMatch
    {
        public StartWithMatch() : base(false) { }
        public StartWithMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return value != null && value != DBNull.Value && sample != null && sample != DBNull.Value && value.ToString().ToLower().StartsWith(sample.ToString().ToLower()); }
    }

    public class NotInMatch : InMatch { public NotInMatch() : base(true) { } }
    public class InMatch : QueryMatch
    {
        public InMatch() : base(false) { }
        protected InMatch(bool isNot) : base(isNot) { }

        protected override bool Matching(object value, object sample)
        {
            var items = sample as IEnumerable;
            if (items == null) return false;
            var comparator = new EqualMatch();
            return items.Cast<object>().Any(x => comparator.IsMatch(value, x));
        }
    }

    public class NotEqualMatch : EqualMatch { public NotEqualMatch() : base(true) { } }
    public class EqualMatch : QueryMatch
    {
        public EqualMatch() : base(false) { }
        protected EqualMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return ((value == null || value == DBNull.Value) && (sample == null || sample == DBNull.Value)) || (value != null && value != DBNull.Value && sample != null && sample != DBNull.Value && value.Equals(sample)); } 
    }

    public class NotNullMatch : NullMatch { public NotNullMatch() : base(true) { } } 
    public class NullMatch : QueryMatch
    {
        public NullMatch() : base(false) { }
        protected NullMatch(bool isNot) : base(isNot) { }
        protected override bool Matching(object value, object sample) { return IsMatch(value); }
        public bool IsMatch(object value) { return value == null || value == DBNull.Value; }
    }

    public abstract class QueryMatch
    {
        private readonly bool _isNot; 
        protected QueryMatch(bool isNot) { _isNot = isNot; }
        protected abstract bool Matching(object value, object sample);
        public bool IsMatch(object value, object sample) { return !_isNot ? Matching(value, sample) : !Matching(value, sample); }

        private static readonly QueryMatch Null = new NullMatch();
        private static readonly QueryMatch NotNull = new NotNullMatch();
        private static readonly QueryMatch Equal = new EqualMatch();
        private static readonly QueryMatch NotEqual = new NotEqualMatch();
        private static readonly QueryMatch More = new MoreMatch();
        private static readonly QueryMatch NotMore = new NotMoreMatch();
        private static readonly QueryMatch MoreEqual = new MoreEqualMatch();
        private static readonly QueryMatch NotMoreEqual = new NotMoreEqualMatch();
        private static readonly QueryMatch Less = new LessMatch();
        private static readonly QueryMatch NotLess = new NotLessMatch();
        private static readonly QueryMatch LessEqual = new LessEqualMatch();
        private static readonly QueryMatch NotLessEqual = new NotLessEqualMatch();
        private static readonly QueryMatch StartWith = new StartWithMatch();
        private static readonly QueryMatch NotStartWith = new NotStartWithMatch();
        private static readonly QueryMatch EndWith = new EndWithMatch();
        private static readonly QueryMatch NotEndWith = new NotEndWithMatch();
        private static readonly QueryMatch Contains = new ContainsMatch();
        private static readonly QueryMatch NotContains = new NotContainsMatch();
        private static readonly QueryMatch In = new InMatch();
        private static readonly QueryMatch NotIn = new NotInMatch();
        private static readonly QueryMatch Between = new BetweenMatch();
        private static readonly QueryMatch NotBetween = new NotBetweenMatch();

        public static bool Is(object value, QueryOp op, object sample)
        {
            switch (op)
            {
                case QueryOp.Null: return Null.IsMatch(value, sample);
                case QueryOp.NotNull: return NotNull.IsMatch(value, sample);
                case QueryOp.Equal: return Equal.IsMatch(value, sample);
                case QueryOp.NotEqual: return NotEqual.IsMatch(value, sample);
                case QueryOp.More: return More.IsMatch(value, sample);
                case QueryOp.NotMore: return NotMore.IsMatch(value, sample);
                case QueryOp.MoreEqual: return MoreEqual.IsMatch(value, sample);
                case QueryOp.NotMoreEqual: return NotMoreEqual.IsMatch(value, sample);
                case QueryOp.Less: return Less.IsMatch(value, sample);
                case QueryOp.NotLess: return NotLess.IsMatch(value, sample);
                case QueryOp.LessEqual: return LessEqual.IsMatch(value, sample);
                case QueryOp.NotLessEqual: return NotLessEqual.IsMatch(value, sample);
                case QueryOp.StartWith: return StartWith.IsMatch(value, sample);
                case QueryOp.NotStartWith: return NotStartWith.IsMatch(value, sample);
                case QueryOp.EndWith: return EndWith.IsMatch(value, sample);
                case QueryOp.NotEndWith: return NotEndWith.IsMatch(value, sample);
                case QueryOp.Contains: return Contains.IsMatch(value, sample);
                case QueryOp.NotContains: return NotContains.IsMatch(value, sample);
                case QueryOp.In: return In.IsMatch(value, sample);
                case QueryOp.NotIn: return NotIn.IsMatch(value, sample);
                case QueryOp.Between: return Between.IsMatch(value, sample);
                case QueryOp.NotBetween: return NotBetween.IsMatch(value, sample);
                default: return false;
            }
        }

        protected void ToComparables(object value1, object value2, out IComparable comparable1, out IComparable comparable2)
        {
            comparable1 = StretchType(value1);
            comparable2 = StretchType(value2);
            if (comparable1.GetType() == comparable2.GetType())
            {
                return;
            }
            comparable1 = Je.Txt.Of(value1);
            comparable2 = Je.Txt.Of(value2);
        }

        private static IComparable StretchType(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            if (value is bool) return (double)((bool)value ? 1 : 0);
            if (value is long) return (double)(long)value;
            if (value is decimal) return (double)(decimal)value;
            if (value is int) return (double)(int)value;
            if (value is short) return (double)(short)value;
            if (value is byte) return (double)(byte)value;
            if (value is ulong) return (double)(ulong)value;
            if (value is uint) return (double)(uint)value;
            if (value is ushort) return (double)(ushort)value;
            if (value is sbyte) return (double)(sbyte)value;
            if (value is float) return (double)(float)value;
            if (value is double) return (double)value;
            if (value is DateTime) return ((DateTime)value).ToOADate();
            return Je.Txt.Of(value);
        }
    }
}