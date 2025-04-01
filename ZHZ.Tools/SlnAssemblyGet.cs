using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZHZ.Tools
{
    public static class SlnAssemblyGet
    {
        /// <summary>
        /// 获取解决方案中所有项目的程序集路径
        /// </summary>
        /// <param name="solutionPath">解决方案文件路径</param>
        /// <param name="configuration">构建配置（如 Debug 或 Release，默认为 Debug）</param>
        /// <param name="platform">构建平台（如 AnyCPU 或 x64，默认为 AnyCPU）</param>
        /// <returns>程序集路径列表</returns>
        public static List<string> GetAssembliesFromSolution(
            string solutionPath,
            string configuration = "Debug",
            string platform = "AnyCPU")
        {
            if (!File.Exists(solutionPath))
                throw new FileNotFoundException("解决方案文件不存在", solutionPath);

            var assemblyPaths = new List<string>();

            // 解析解决方案文件，获取所有项目路径
            var projectPaths = ParseSolution(solutionPath);

            foreach (var projectPath in projectPaths)
            {
                try
                {
                    // 加载项目文件
                    var project = new Project(projectPath);

                    // 获取项目输出路径
                    var outputType = project.GetPropertyValue("OutputType");
                    if (outputType != "Library" && outputType != "Exe")
                        continue; // 跳过非程序集项目（如网站项目）

                    var outputDir = project.GetPropertyValue("OutputPath");
                    var assemblyName = project.GetPropertyValue("AssemblyName");
                    var targetFramework = project.GetPropertyValue("TargetFramework");

                    // 构建程序集路径
                    string assemblyExtension = outputType == "Exe" ? ".exe" : ".dll";
                    string assemblyPath = Path.Combine(
                        Path.GetDirectoryName(projectPath),
                        outputDir,
                        $"{assemblyName}{assemblyExtension}");

                    // 检查文件是否存在
                    if (File.Exists(assemblyPath))
                    {
                        assemblyPaths.Add(assemblyPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"解析项目 {projectPath} 时出错: {ex.Message}");
                }
            }

            return assemblyPaths;
        }

        /// <summary>
        /// 解析解决方案文件，获取所有项目路径
        /// </summary>
        /// <param name="solutionPath">解决方案文件路径</param>
        /// <returns>项目路径列表</returns>
        private static List<string> ParseSolution(string solutionPath)
        {
            var projectPaths = new List<string>();
            var solutionDir = Path.GetDirectoryName(solutionPath);

            var pattern = @"Project\("".*""\) = ""(.*)"", ""(.*)"", ""{.*}""";
            var lines = File.ReadAllLines(solutionPath);

            foreach (var line in lines)
            {
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    string projectFile = match.Groups[2].Value;
                    string projectPath = Path.Combine(solutionDir, projectFile);

                    if (File.Exists(projectPath))
                    {
                        projectPaths.Add(projectPath);
                    }
                }
            }

            return projectPaths;
        }
    }
}
