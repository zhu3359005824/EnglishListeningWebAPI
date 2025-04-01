using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZHZ.EventBus.Handler;

namespace ZHZ.Tools
{
    public static class EventHandlerScanner
    {
        // 可配置的黑名单关键词（根据需求扩展）
        private static readonly HashSet<string> _assemblyBlacklist = new(StringComparer.OrdinalIgnoreCase)
    {
        "Microsoft.",    // 排除所有Microsoft官方库
        "System.",       // 排除System命名空间库
        "Newtonsoft.",   // 排除Json.NET
        "AspNetCore.",   // 排除ASP.NET Core基础库
        "WindowsBase"    // 排除特定不需要的程序集
    };

        // 可配置的白名单关键词（黑名单优先于白名单）
        private static readonly HashSet<string> _assemblyWhitelist = new(StringComparer.OrdinalIgnoreCase)
    {
        "MediaEncoder.", // 允许自有库
        "EnglishListening" // 允许主项目库
    };

        public static IEnumerable<Type> FindEventHandlerTypes(string assembliesPath = null)
        {
            var handlerTypes = new List<Type>();
            var assemblies = GetAssemblies(assembliesPath);

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        if (IsValidEventHandlerType(type))
                        {
                            handlerTypes.Add(type);
                            Console.WriteLine($"已注册处理器: {type.FullName}");
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    LogLoadError(assembly, $"类型加载失败: {ex.LoaderExceptions.FirstOrDefault()?.Message}");
                }
                catch (Exception ex)
                {
                    LogLoadError(assembly, $"程序集扫描异常: {ex.Message}");
                }
            }

            return handlerTypes;
        }

        private static IEnumerable<Assembly> GetAssemblies(string path = null)
        {
            var assemblies = new List<Assembly>();

            if (string.IsNullOrEmpty(path))
            {
                // 扫描已加载程序集时也应用过滤
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(asm => IsAllowedAssembly(asm.GetName().Name));
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"警告：目录 {path} 不存在");
                return assemblies;
            }

            var assemblyFiles = Directory.GetFiles(path, "*.dll")
                .Where(IsAllowedAssembly);

            foreach (var file in assemblyFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    assemblies.Add(assembly);
                    Console.WriteLine($"成功加载: {Path.GetFileName(file)}");
                }
                catch (BadImageFormatException)
                {
                    Console.WriteLine($"忽略非托管DLL: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载失败 [{Path.GetFileName(file)}]: {ex.Message}");
                }
            }

            return assemblies;
        }

        private static bool IsAllowedAssembly(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            // 黑名单优先检查
            var isBlacklisted = _assemblyBlacklist
                .Any(blackWord => fileName.StartsWith(blackWord, StringComparison.OrdinalIgnoreCase));

            // 白名单检查（当白名单非空时）
            var isWhitelisted = _assemblyWhitelist.Count == 0 ||
                _assemblyWhitelist.Any(whiteWord => fileName.StartsWith(whiteWord, StringComparison.OrdinalIgnoreCase));

            return !isBlacklisted && isWhitelisted;
        }

        private static bool IsValidEventHandlerType(Type type)
        {
            return typeof(IIntergrationEventHandler).IsAssignableFrom(type) &&
                   !type.IsAbstract &&
                   !type.IsInterface &&
                   type.IsPublic;
        }

        private static void LogLoadError(Assembly assembly, string message)
        {
            Console.WriteLine($"[{assembly.GetName().Name}] 错误: {message}");
        }
    }
}


