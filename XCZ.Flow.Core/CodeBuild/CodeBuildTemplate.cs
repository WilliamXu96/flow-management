using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XCZ.FormManagement.Core.CodeBuild
{
    public class CodeBuildTemplate
    {

        public static string ApplicationSverviceTemplate { get => GetResourceName($"AppServiceTemplate");  }
        public static string IApplicationSverviceTemplate { get => GetResourceName($"IAppServiceTemplate"); }
        public static string DomianModelTemplate { get => GetResourceName($"DomianModelTemplate"); }
        public static string AutoMapperProfileTemplate { get => GetResourceName($"AutoMapperProfileTemplate"); }
        public static string CreateOrUpdateDtoTemplate { get => GetResourceName($"Dto.CreateOrUpdateDtoTemplate"); }
        public static string InputDtoTemplate { get => GetResourceName($"Dto.InputDtoTemplate"); }
        public static string OutputDtoTemplate { get => GetResourceName($"Dto.OutputDtoTemplate"); }
        public static string ControllerTemplate { get => GetResourceName($"ControllerTemplate"); }
        public static string PermissionTemplate { get => GetResourceName($"Permissions.PermissionsTemplate"); }
        public static string PermissionDefinitionProviderTemplate { get => GetResourceName($"Permissions.PermissionDefinitionProviderTemplate"); }
        public static string DbContextTemplate { get => GetResourceName($"EF.DbContextTemplate"); }
        public static string DbContextModelCreatingExtensionsTemplate { get => GetResourceName($"EF.DbContextModelCreatingExtensionsTemplate"); }
        /// <summary>
        /// 获取txt资源文件的内容
        /// </summary>
        /// <param name="name">txt文件的名称，默认在Template目录，子目录要加前缀例如：Dto.xxxxx</param>
        /// <returns></returns>
        private static string GetResourceName(string name)
        {
            string text = "";
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName;
                var resourceNames = assembly.GetManifestResourceNames();
                resourceName = resourceNames.FirstOrDefault(a => a.Contains($".Template.{name}.txt"));
                Stream stream = assembly.GetManifestResourceStream(resourceName);
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                text = reader.ReadToEnd();
            }
            catch (Exception)
            {

                throw;
            }

            return text;
        }
    }
}
