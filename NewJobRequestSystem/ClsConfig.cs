using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewJobRequestSystem
{
    public class ClsConfig
    {
        public static String JobRequestConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["JobRequestDB"].ConnectionString; }
        }

        public static String PISConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["HRISDB"].ConnectionString; }
        }

        public static String SomsConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SOMSDB"].ConnectionString; }
        }
    }
}