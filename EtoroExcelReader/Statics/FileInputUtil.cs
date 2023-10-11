using System;
using System.IO;

namespace ExcelReader.Statics
{
    public static class FileInputUtil
    {
        /// <summary>
        /// Returns a fileinfo with the full path of the requested file
        /// </summary>
        /// <param name="directory">A subdirectory</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string directory, string file)
        {
            return new FileInfo(Path.Combine( directory, file));
        }

        public static DirectoryInfo GetDirectory(string directory)
        {
            return new DirectoryInfo(directory);
        }
    }
}