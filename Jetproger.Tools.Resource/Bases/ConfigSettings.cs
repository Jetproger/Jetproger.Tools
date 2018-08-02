using System.Data.SqlClient;
using System.Globalization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Resource.Bases
{
    public class CultureSetting : ExSetting
    {
        public CultureSetting() : base(null)
        {
            Name = "en-US";
        }
    }

    public class HoatPortSetting : ExSetting
    {
        public HoatPortSetting() : base(null)
        {
            Name = "1234";
        }

        public override void Validate()
        {
            try
            {
                int x;
                IsValid = int.TryParse(Name, NumberStyles.Any, StringExtensions.Formatter, out x);
            }
            catch
            {
                IsValid = false;
            }
        }
    }

    public class MaxReservedDomainSetting : ExSetting
    {
        public MaxReservedDomainSetting() : base(null)
        {
            Name = "4";
        }

        public override void Validate()
        {
            try
            {
                int x;
                IsValid = int.TryParse(Name, NumberStyles.Any, StringExtensions.Formatter, out x);
            }
            catch
            {
                IsValid = false;
            }
        }
    }

    public class ConnectionStringSetting : ExSetting
    {
        public ConnectionStringSetting() : base(null)
        {
        }

        public override void Validate()
        {
            try
            {
                var csb = new SqlConnectionStringBuilder(Name);
                csb.AsynchronousProcessing = true;
                csb.MultipleActiveResultSets = true;
                Name = csb.ToString();
            }
            catch
            {
                IsValid = false;
                return;
            }
            try
            {
                using (var cnc = new SqlConnection(Name))
                {
                    cnc.Open();
                    using (var cmd = new SqlCommand("SELECT 1", cnc))
                    {
                        int result = cmd.ExecuteScalar().As<int>();
                        IsValid = result == 1;
                    }
                }
            }
            catch
            {
                IsValid = false;
            }
        }
    }
}