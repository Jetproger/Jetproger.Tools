using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        public const string IconDefault = @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
        public const string ImageDefault = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";
        public static readonly DateTime MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);

        public static readonly JsonSerializer JsonSerializer = new JsonSerializer
        {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static readonly CultureInfo Formatter = new CultureInfo("en-us")
        {
            NumberFormat =
            {
                NumberGroupSeparator = string.Empty,
                NumberDecimalSeparator = "."
            },
            DateTimeFormat =
            {
                DateSeparator = "-",
                TimeSeparator = ":"
            }
        };

        private static readonly Dictionary<Type, Func<object, object>> Convertors = new Dictionary<Type, Func<object, object>>
        {
            { typeof(bool), o => AsBool(o) },
            { typeof(bool?), o => AsBoolNull(o) },
            { typeof(byte), o => AsByte(o) },
            { typeof(byte?), o => AsByteNull(o) },
            { typeof(sbyte), o => AsSbyte(o) },
            { typeof(sbyte?), o => AsSbyteNull(o) },
            { typeof(byte[]), o => AsBytes(o) },
            { typeof(char), o => AsChar(o) },
            { typeof(char?), o => AsCharNull(o) },
            { typeof(char[]), o => AsChars(o) },
            { typeof(short), o => AsShort(o) },
            { typeof(short?), o => AsShortNull(o) },
            { typeof(ushort), o => AsUshort(o) },
            { typeof(ushort?), o => AsUshortNull(o) },
            { typeof(int), o => AsInt(o) },
            { typeof(int?), o => AsIntNull(o) },
            { typeof(uint), o => AsUint(o) },
            { typeof(uint?), o => AsUintNull(o) },
            { typeof(long), o => AsLong(o) },
            { typeof(long?), o => AsLongNull(o) },
            { typeof(ulong), o => AsUlong(o) },
            { typeof(ulong?), o => AsUlongNull(o) },
            { typeof(Guid), o => AsGuid(o) },
            { typeof(Guid?), o => AsGuidNull(o) },
            { typeof(float), o => AsFloat(o) },
            { typeof(float?), o => AsFloatNull(o) },
            { typeof(decimal), o => AsDecimal(o) },
            { typeof(decimal?), o => AsDecimalNull(o) },
            { typeof(double), o => AsDouble(o) },
            { typeof(double?), o => AsDoubleNull(o) },
            { typeof(DateTime), o => AsDateTime(o) },
            { typeof(DateTime?), o => AsDateTimeNull(o) },
            { typeof(string), o => AsString(o) }
        };

        private static readonly Dictionary<Type, object> Defaults = new Dictionary<Type, object>
        {
            { typeof(bool), default(bool) },
            { typeof(bool?), default(bool?) },
            { typeof(byte), default(byte) },
            { typeof(byte?), default(byte?) },
            { typeof(sbyte), default(sbyte) },
            { typeof(sbyte?), default(sbyte?) },
            { typeof(byte[]), default(byte[]) },
            { typeof(short), default(short) },
            { typeof(short?), default(short?) },
            { typeof(ushort), default(ushort) },
            { typeof(ushort?), default(ushort?) },
            { typeof(int), default(int) },
            { typeof(int?), default(int?) },
            { typeof(uint), default(uint) },
            { typeof(uint?), default(uint?) },
            { typeof(long), default(long) },
            { typeof(long?), default(long?) },
            { typeof(ulong), default(ulong) },
            { typeof(ulong?), default(ulong?) },
            { typeof(Guid), default(Guid) },
            { typeof(Guid?), default(Guid?) },
            { typeof(float), default(float) },
            { typeof(float?), default(float?) },
            { typeof(decimal), default(decimal) },
            { typeof(decimal?), default(decimal?) },
            { typeof(double), default(double) },
            { typeof(double?), default(double?) },
            { typeof(DateTime), default(DateTime) },
            { typeof(DateTime?), default(DateTime?) },
            { typeof(char), default(char) },
            { typeof(char?), default(char?) },
            { typeof(char[]), default(char[]) },
            { typeof(string), default(string) }
        };
    }
}