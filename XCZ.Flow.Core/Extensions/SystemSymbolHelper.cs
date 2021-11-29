using System;

namespace XCZ.Extensions
{
    public static class SystemSymbolHelper
    {
        /// <summary>
        /// 根据系统类型设定路径分隔符
        /// 注：Uninx（linux）或MacOSX 分隔符与Win不通
        /// </summary>
        /// <returns></returns>
        public static string GetSysPathSeparator()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID platformId= os.Platform;
            if (platformId == PlatformID.Unix || platformId == PlatformID.MacOSX)
            {
                return "/";
            }
            else
            {
                return @"\";
            }
        }
        
        /// <summary>
        /// 根据系统类型设定换行符
        /// 注：Uninx（linux）或MacOSX 符号与Win不通
        /// </summary>
        /// <returns></returns>
        public static string GetSysLineFeed()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID platformId= os.Platform;
            if (platformId == PlatformID.Unix || platformId == PlatformID.MacOSX)
            {
                return "\r";
            }
            else
            {
                return "\r\n";
            }
        }
    }
}