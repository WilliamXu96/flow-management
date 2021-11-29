using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCZ.CodeBuild
{
    public class CodeBuildModel
    {
        /// <summary>
        /// 生成的程序路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 代码cs文件的名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 生成的程序代码
        /// </summary>
        public string Code { get; set; }
    }
}
