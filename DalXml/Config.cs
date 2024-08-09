using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    internal static class Config
    {
        static string s_data_config_xml = "data-config";
        internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
        internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }


        internal static DateTime? startProject = XMLTools.LoadListFromXMLElement(@"data-config").ToDateTimeNullable("startProject");
        internal static DateTime? deadlineProject = XMLTools.LoadListFromXMLElement(@"data-config").ToDateTimeNullable("deadlineProject");
    }
}
