using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Library
{
    public static class ConfigManager
    {
        public static string GetRunner()
        {
            return AppSettingsUtil.GetValue<string>("GeneralConfig:ApplicationName");
        }

        public static int GetMaxFileSizeInMB()
        { 
            int maxSizeInMB = AppSettingsUtil.GetValue<int>("GeneralConfig:MaxFileSizeInMb");
            return maxSizeInMB * 1024 * 1024;
        }

        public static Boolean GetLogInfo()
        {
            return AppSettingsUtil.GetValue<Boolean>("GeneralConfig:LogInfo");
        }

        public static Boolean GetLogDebug()
        {
            return AppSettingsUtil.GetValue<Boolean>("GeneralConfig:LogDebug");
        }

        public static Boolean GetCreateStoreIfNotExisted()
        {
            return AppSettingsUtil.GetValue<Boolean>("GeneralConfig:CreateProductIfNotExisted");
        }

        public static Boolean GetCreateProductIfNotExisted()
        {
            return AppSettingsUtil.GetValue<Boolean>("GeneralConfig:CreateStoreifNotExisted");
        }
    }
}
