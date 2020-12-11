/*

 Copyright (c) SEUNGEE Co.,Ltd. All Rights Reserved.

 * @Author: jeffpan 
 * @Last Modified by:   jeffpan

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ServiceStack;
using ServiceStack.Text;

namespace StockSimulateCore.Utils
{
    /// <summary>
    /// 程序集相关的帮助类
    /// </summary>
    public static class AssemblyUtil
    {
        public static Assembly LoadAssembly(string fullPath)
        {
            //var assb= AssemblyUtils.LoadAssembly(fullPath);
            var assb = Assembly.LoadFrom(fullPath);
            return assb;
        }

        /// <summary>
        /// 搜索指定目录下的所有DLL
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Assembly> LoadAssemblies(string folderName, Func<FileInfo, bool> filter = null)
        {
            if (filter == null) filter = f => f.Length > 0;

            var pathInfo = new DirectoryInfo(folderName);
            return pathInfo.GetFiles("*.dll", SearchOption.AllDirectories)
                           .Where(filter)
                           .Select(f => LoadAssembly(f.FullName))
                           .ToList();
        }
    }
}