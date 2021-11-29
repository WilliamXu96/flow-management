using System;
using System.Collections.Generic;
using System.Text;
using XCZ.Extensions;
using XCZ.FormManagement;
using XCZ.FormManagement.Core.CodeBuild;
using XCZ.Utilities;

namespace XCZ.CodeBuild
{
    public class CodeBuildHelper
    {
        private List<CodeBuildModel> codeBuildModels;
        private Form form;
        private List<FormField> fields;
        private string parentPath;
        private string sysPathSeparator;
        private string sysLineFeed;

        /// <summary>
        /// 代码生成帮助类
        /// </summary>
        /// <param name="_form"></param>
        /// <param name="_fields"></param>
        /// <param name="_parentPath"></param>
        /// <param name="_sysPathSeparator">操作系统的路径分隔符</param>
        /// <param name="_sysLineFeed">操作系统的换行符</param>
        public CodeBuildHelper(Form _form, List<FormField> _fields, string _parentPath, string _sysPathSeparator, string _sysLineFeed)
        {
            codeBuildModels = new List<CodeBuildModel>();
            form = _form;
            fields = _fields;
            parentPath = _parentPath;
            this.sysPathSeparator = _sysPathSeparator;
            this.sysLineFeed = _sysLineFeed;
        }

        public void Build()
        {

            BuildDomainEntity();
            BuildDto();
            BuildAppService();
            //开始把代码写入文件
            StartWrite();
            //生成权限
            BuildPermission();
            BuildPermissionDefinitionProvider();
            //生成DbContext
            BuildDbContext();
            BuildDbContextModelCreatingExtensions();
        }

        private void BuildPermission()
        {
            string path = $@"{parentPath}{sysPathSeparator}{form.Namespace}.Application.Contracts{sysPathSeparator}Permissions{sysPathSeparator}{form.Namespace}Permissions.cs";
            var permissionContent = FileHelper.ReadFile(path);
            var template = CodeBuildTemplate.PermissionTemplate.Replace("{EntityName}", form.EntityName.FirstCharToUpper())
                                                               .Replace("{Namespace}", form.Namespace);
            permissionContent = permissionContent.Replace("//Code generation...", template);
            FileHelper.WriteFile($@"{parentPath}{sysPathSeparator}{form.Namespace}.Application.Contracts{sysPathSeparator}Permissions{sysPathSeparator}", $"{form.Namespace}Permissions.cs", permissionContent);
        }

        private void BuildPermissionDefinitionProvider()
        {
            string path = $@"{parentPath}{sysPathSeparator}{form.Namespace}.Application.Contracts{sysPathSeparator}Permissions{sysPathSeparator}{form.Namespace}PermissionDefinitionProvider.cs";
            var permissionContent = FileHelper.ReadFile(path);
            var template = CodeBuildTemplate.PermissionDefinitionProviderTemplate.Replace("{EntityName}", form.EntityName.FirstCharToUpper())
                                                                                 .Replace("{Namespace}", form.Namespace);
            permissionContent = permissionContent.Replace("//Code generation...", template);
            FileHelper.WriteFile($@"{parentPath}{sysPathSeparator}{form.Namespace}.Application.Contracts{sysPathSeparator}Permissions{sysPathSeparator}", $"{form.Namespace}PermissionDefinitionProvider.cs", permissionContent);
        }

        private void BuildDbContext()
        {
            string path = $@"{parentPath}{sysPathSeparator}{form.Namespace}.EntityFrameworkCore{sysPathSeparator}EntityFrameworkCore{sysPathSeparator}{form.Namespace}DbContext.cs";
            var dbContent = FileHelper.ReadFile(path);
            var template = CodeBuildTemplate.DbContextTemplate.Replace("{EntityName}", form.EntityName.FirstCharToUpper());
            dbContent = dbContent.Replace("//Code generation...", template);
            FileHelper.WriteFile($@"{parentPath}{sysPathSeparator}{form.Namespace}.EntityFrameworkCore{sysPathSeparator}EntityFrameworkCore{sysPathSeparator}", $"{form.Namespace}DbContext.cs", dbContent);
        }

        private void BuildDbContextModelCreatingExtensions()
        {
            string path = $@"{parentPath}{sysPathSeparator}{form.Namespace}.EntityFrameworkCore{sysPathSeparator}EntityFrameworkCore{sysPathSeparator}{form.Namespace}DbContextModelCreatingExtensions.cs";
            var dbContent = FileHelper.ReadFile(path);

            var propertiesBuilder = new StringBuilder();
            var indexBuilder = new StringBuilder();

            foreach (var field in fields)
            {
                if (field.DataType == "string" && (field.IsRequired || field.Maxlength.HasValue))
                {
                    propertiesBuilder.Append($"{sysLineFeed}                b.Property(x => x.{field.FieldName.FirstCharToUpper()})");
                    if (field.IsRequired)
                    {
                        propertiesBuilder.Append(".IsRequired()");
                    }
                    if (field.Maxlength.HasValue)
                    {
                        propertiesBuilder.Append($".HasMaxLength({field.Maxlength})");
                    }
                    propertiesBuilder.Append($";");
                }

                if (field.IsIndex)
                {
                    indexBuilder.Append($"{sysLineFeed}                b.HasIndex(x => x.{field.FieldName.FirstCharToUpper()});{sysLineFeed}");
                }
            }

            var template = CodeBuildTemplate.DbContextModelCreatingExtensionsTemplate
                                            .Replace("{EntityName}", form.EntityName.FirstCharToUpper())
                                            .Replace("{TableName}", form.TableName)
                                            .Replace("{Properties}", propertiesBuilder.ToString() + indexBuilder.ToString());
            dbContent = dbContent.Replace("//Code generation...", template);
            FileHelper.WriteFile($@"{parentPath}{sysPathSeparator}{form.Namespace}.EntityFrameworkCore{sysPathSeparator}EntityFrameworkCore{sysPathSeparator}", $"{form.Namespace}DbContextModelCreatingExtensions.cs", dbContent);
        }

        #region 基础方法
        /// <summary>
        /// 开始写文件
        /// </summary>
        private void StartWrite()
        {
            foreach (var item in codeBuildModels)
            {
                FileHelper.WriteFile(@$"{parentPath}{sysPathSeparator}{item.Path}", $"{item.FileName}.cs", $"{ item.Code}");
                Console.WriteLine(item.Path + item.FileName);
            }
        }
        /// <summary>
        /// 创建普通代码，替换{Namespace}、{EntityName}
        /// </summary>
        private CodeBuildModel BuildCode(string path, string content, string fileName, bool hasAtrrbute = false)
        {
            string tempContent = content
                                 .Replace("{DisplayName}", form.DisplayName)
                                 .Replace("{Namespace}", form.Namespace.FirstCharToUpper())
                                 .Replace("{EntityName}", form.EntityName.FirstCharToUpper());
            if (hasAtrrbute)
            {
                StringBuilder AttributeBuilder = GetAttributes();
                tempContent = tempContent.Replace("{AttributeList}", AttributeBuilder.ToString());
            }
            CodeBuildModel codeBuildModel = new CodeBuildModel
            {
                Path = path,
                FileName = fileName,
                Code = tempContent
            };
            return codeBuildModel;
        }
        /// <summary>
        /// 获取属性代码
        /// </summary>
        /// <returns></returns>
        private StringBuilder GetAttributes()
        {
            var AttributeBuilder = new StringBuilder();
            foreach (var field in fields)
            {
                AttributeBuilder.Append($"{sysLineFeed}        /// <summary>");
                AttributeBuilder.Append($"{sysLineFeed}        /// " + field.Label + "");
                AttributeBuilder.Append($"{sysLineFeed}        /// </summary>");
                if (field.IsRequired)
                {
                    AttributeBuilder.Append($"{sysLineFeed}        [Required]");
                }
                AttributeBuilder.Append($"{sysLineFeed}        public {field.DataType} {field.FieldName.FirstCharToUpper()} {{ get; set; }}");
                AttributeBuilder.Append($"{sysLineFeed}        ");
            }
            return AttributeBuilder;
        }
        #endregion

        private void BuildDomainEntity()
        {
            string path = @$"{form.Namespace}.Domain{sysPathSeparator}Models{sysPathSeparator}";
            var codeBuildModel = BuildCode(path, CodeBuildTemplate.DomianModelTemplate, form.EntityName, true);
            codeBuildModels.Add(codeBuildModel);

            ////添加属性、注释、定义
            //var ParamBuilder = new StringBuilder();//构造函数参数
            //var AssignmentBuilder = new StringBuilder();//构造函数属性赋值
            //foreach (var field in fields)
            //{
            //    var param = field.FieldName.ToLower();
            //    if (field.DataType == "string")
            //    {
            //        ParamBuilder.Append(", " + field.DataType + " " + param);
            //    }
            //    else
            //    {
            //        ParamBuilder.Append(", " + field.DataType + (field.IsRequired ? " " : "? ") + param);
            //    }

            //    AssignmentBuilder.Append($"{_sysLineFeed}            {field.FieldName} = {param};");
            //}
            //domainContent = domainContent
            //    .Replace("{Constructor}", ParamBuilder.ToString())
            //    .Replace("{Assignment}", AssignmentBuilder.ToString());


        }

        private void BuildDto()
        {

            string path = @$"{form.Namespace}.Application.Contracts{sysPathSeparator}{form.EntityName}Management{sysPathSeparator}Dto{sysPathSeparator}";


            var CreateOrUpdateDtoTemplate = BuildCode(path, CodeBuildTemplate.CreateOrUpdateDtoTemplate, $"CreateOrUpdate{form.EntityName.FirstCharToUpper()}Dto", true);
            codeBuildModels.Add(CreateOrUpdateDtoTemplate);

            var OutputDtoTemplate = BuildCode(path, CodeBuildTemplate.OutputDtoTemplate, $"{form.EntityName.FirstCharToUpper()}Dto", true);
            codeBuildModels.Add(OutputDtoTemplate);

            var InputDtoTemplate = BuildCode(path, CodeBuildTemplate.InputDtoTemplate, $"Get{form.EntityName.FirstCharToUpper()}InputDto");
            codeBuildModels.Add(InputDtoTemplate);
        }


        private void BuildAppService()
        {
            string path = @$"{form.Namespace}.Application{sysPathSeparator}{form.EntityName}Management{sysPathSeparator}";
            string content = CodeBuildTemplate.ApplicationSverviceTemplate;
            string fileName = $"{form.EntityName}AppService";
            for (int i = 0; i < 2; i++)
            {
                //IAppService
                if (i == 1)
                {
                    path = @$"{form.Namespace}.Application.Contracts{sysPathSeparator}{form.EntityName}Management{sysPathSeparator}";
                    content = CodeBuildTemplate.IApplicationSverviceTemplate;
                    fileName = $"I{fileName}";
                }
                //AppService
                var codeBuildModel = BuildCode(path, content, fileName);
                codeBuildModels.Add(codeBuildModel);
            }
            //创建AutoMapperProfile
            path = @$"{form.Namespace}.Application{sysPathSeparator}{form.EntityName}Management{sysPathSeparator}";
            var codeBuildModel2 = BuildCode(path, CodeBuildTemplate.AutoMapperProfileTemplate, $"{form.EntityName}AutoMapperProfile");
            codeBuildModels.Add(codeBuildModel2);

            //创建控制器
            path = @$"{form.Namespace}.HttpApi{sysPathSeparator}Controllers{sysPathSeparator}";
            var codeBuildModel3 = BuildCode(path, CodeBuildTemplate.ControllerTemplate, $"{form.EntityName}Controller");
            codeBuildModels.Add(codeBuildModel3);
        }

    }
}
