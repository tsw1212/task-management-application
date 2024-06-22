namespace Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }

    public static DateTime? cStartProject { get => XMLTools.GetDateProject(s_data_config_xml, "startProject"); set => XMLTools.SetDateProject(value, s_data_config_xml, "startProject"); }
    public static DateTime? cEndProject { get => XMLTools.GetDateProject(s_data_config_xml, "endProject"); set => XMLTools.SetDateProject(value, s_data_config_xml, "endProject"); }

}
