using System;
using System.ComponentModel;

namespace SV.HRM.Logging.NLogCustom
{
    public enum LogSourceTypeEnums
    {
        [Description("SV.HRM.API")]
        HRM_API = 1,
        [Description("SV.HRM.Service.BaseServices")]
        HRM_Service_BaseServices = 2,
        [Description("SV.HRM.Service.QuanTriDanhMuc")]
        HRM_Service_QuanTriDanhMuc = 4,
        [Description("SV.HRM.Service.Report")]
        HRM_Service_Report = 5,
        [Description("SV.HRM.Service.ThongTinNhanSu")]
        HRM_Service_ThongTinNhanSu = 6,
        [Description("SV.HRM.Service.Attendance")]
        HRM_Service_Attendance = 7,
        [Description("SV.HRM.Service.LeaveManagement")]
        HRM_Service_Leave_Management = 8,
        [Description("SV.HRM.Service.Training")]
        HRM_Service_Training = 9
    }

    public static class EnumConvert
    {
        public static string GetEnumDescription(Enum value)
        {
            try
            {
                var fi = value.GetType().GetField(value.ToString());

                var attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);
                return attributes?.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
