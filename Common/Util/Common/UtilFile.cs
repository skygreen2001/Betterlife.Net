#region Refrences
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace Util.Common
{
    /// <summary>
    /// 文件管理类
    /// </summary>
    /// <see cref="http://www.cftea.com/c/2009/04/NC13BFY46B5BM714.asp"/>
    public static class UtilFile
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="Path"></param>
        public static void FileCreate(string Path)
        {
            FileStream file = File.Create(Path);
            file.Close();
            file = null;
        }


        public static bool CreateDir(string fullPathFileName)
        {
            if (string.IsNullOrEmpty(fullPathFileName))return false;
            if (File.Exists(fullPathFileName))
            {
                return false;
            }else{
                Directory.CreateDirectory(Path.GetDirectoryName(fullPathFileName));
            }
            return true;
        }

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDreictory"></param>
        public static void MoveDirectoryTo( string sourceDirectory, string destDreictory )
        {
            if ( !Directory.Exists( destDreictory ) )
            {
                Directory.CreateDirectory( destDreictory );
            }
            DirectoryInfo source = new DirectoryInfo( sourceDirectory );
            FileInfo[] files = source.GetFiles();
            foreach ( FileInfo file in files )
            {
                file.CopyTo( Path.Combine( destDreictory, file.Name ), true );
                file.Delete();
            }
            DirectoryInfo[] subDrectorys = source.GetDirectories();
            foreach ( DirectoryInfo direct in subDrectorys )
            {
                MoveDirectoryTo( direct.FullName, Path.Combine( destDreictory, direct.Name ) );
            }
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDreictory"></param>
        public static void CopyDirectoryTo( string sourceDirectory, string destDreictory )
        {
            CopyDirectoryTo( sourceDirectory, destDreictory, true );
        }

        /// <summary>
        /// 复制目录，提供是否覆盖目标的选项参数
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="copySubDirs"></param>
        public static void CopyDirectoryTo( string sourceDirectory, string destDirectory, bool copySubDirs )
        {
            //直接异步开始并结束，没有回调
            //IAsyncResult result = BeginCopyDirectoryTo( sourceDirectory, destDreictory, copySubDirs, null, null );
            //EndCopyDirectoryTo( result );

            if ( String.IsNullOrEmpty( sourceDirectory ) ) throw new ArgumentNullException( "sourceDirectory" );
            if ( String.IsNullOrEmpty( destDirectory ) ) throw new ArgumentNullException( "destDreictory" );
            if ( sourceDirectory == Path.GetPathRoot( sourceDirectory ) ) throw new IOException( "不能复制根目录!" );
            if ( sourceDirectory == destDirectory ) throw new IOException( "目标目录与源目录相同!" );

            DirectoryInfo source = new DirectoryInfo( sourceDirectory );
            if ( !source.Exists ) throw new DirectoryNotFoundException( sourceDirectory );

            DirectoryInfo dest = new DirectoryInfo( destDirectory );
            if ( !dest.Exists )
            {
                Directory.CreateDirectory( destDirectory );
            }
            else
            {
                dest.Attributes &= FileAttributes.ReadOnly;
            }

            //逐一复制文件
            foreach ( FileInfo file in source.GetFiles() )
            {
                string destFullName = Path.Combine( destDirectory, file.Name );
                FileInfo destInfo = new FileInfo( destFullName );
                //若已存在，需要将目标文件的只读属性去掉
                if ( destInfo.Exists ) destInfo.IsReadOnly = false;
                //可覆盖拷贝
                file.CopyTo( destFullName, true );
            }

            //若要拷贝子目录，则递归调用本函数
            if ( copySubDirs )
            {
                DirectoryInfo[] dirs = source.GetDirectories();
                foreach ( DirectoryInfo subdir in dirs )
                {
                    string temppath = Path.Combine( destDirectory, subdir.Name );
                    CopyDirectoryTo( subdir.FullName, temppath, copySubDirs );
                }
            }
        }
        
        /// <summary>
        /// 异步复制目录结束
        /// </summary>
        /// <param name="result"></param>
        public static void EndCopyDirectoryTo( IAsyncResult result )
        {
            if ( result.IsCompleted ) return;
            if ( result.AsyncWaitHandle.WaitOne( 60 * 1000 ) ) return;
            throw new TimeoutException( "拷贝文件未响应!" );
        }

        /// <summary>
        /// 获取指定路径下的文件
        /// </summary>
        /// <param name="directPath"></param>
        /// <param name="containSubDirectory"></param>
        /// <param name="fileFilter"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles( string directPath, bool containSubDirectory, string fileFilter )
        {
            DirectoryInfo directInfo = new DirectoryInfo( directPath );
            if ( !directInfo.Exists )
            {
                return new FileInfo[ 0 ];
            }
            SearchOption option =  ( containSubDirectory ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly );
            if ( string.IsNullOrEmpty( fileFilter ) )
            {
                fileFilter = "*.*";
            }
            return directInfo.GetFiles( fileFilter, option );
        }

        /// <summary>
        /// 写文本到文件中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="txt"></param>
        /// <param name="append"></param>
        public static void WriteTextToFile( string path, string txt, bool append )
        {
            WriteTextToFile( path, txt, Encoding.Default, append );
        }

        /// <summary>
        /// 写文本到文件中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="txt"></param>
        /// <param name="encoding"></param>
        /// <param name="append"></param>
        public static void WriteTextToFile( string path, string txt, Encoding encoding, bool append )
        {
            using (StreamWriter stream = new StreamWriter(path, append, encoding))
            {
                stream.Write(txt);
                stream.Close();
            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void Write( string path, byte[] data )
        {
            string directoryPath = Path.GetDirectoryName( path );
            if ( !Directory.Exists( directoryPath ) )
            {
                Directory.CreateDirectory( directoryPath );
            }
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
        }

        /// <summary>
        /// 读文件到二进制数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] Read( string path )
        {
            string directoryPath = Path.GetDirectoryName( path );
            if ( !Directory.Exists( directoryPath ) )
            {
                Directory.CreateDirectory( directoryPath );
            }
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
                return data;
            }
        }

        /// <summary>
        /// 获取指定目录下的文件数
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static int GetFileCount( this DirectoryInfo dir )
        {
            return GetFileCount( dir, true );
        }

        /// <summary>
        /// 获取文件数目
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="includeSub"></param>
        /// <returns></returns>
        public static int GetFileCount( this DirectoryInfo dir, bool includeSub )
        {
            int files = dir.GetFiles().Length;
            if ( includeSub )
                foreach ( DirectoryInfo subDir in dir.GetDirectories() )
                    files += GetFileCount( subDir, true );
            return files;
        }

        /// <summary>
        /// 将文件内容读入到字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile2String( string path )
        {
            if ( !File.Exists( path ) )
            {
                return String.Empty;
            }
            string value = String.Empty;
            using ( StreamReader reader = new StreamReader( path ) )
            {
                value = reader.ReadToEnd();
            }
            return value;
        }

        /// <summary>
        /// 将字符串内容写入到文件
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="Path"></param>
        public static void WriteString2File(string Path,  string Content)
        {
            using (StreamWriter wrtier = new StreamWriter(Path))
            {
                wrtier.Write(Content);
                wrtier.Flush();
            }
            
        }

        /// <summary>
        /// 将字符串内容写入到文件[文件编码格式为GB2312]
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="Path"></param>
        public static void WriteString2FileEncodingGbk(string Path, string Content)
        {
            using (StreamWriter wrtier = new StreamWriter(Path, false, Encoding.GetEncoding("GB2312")))
            {
                wrtier.Write(Content);
                wrtier.Flush();
            }
        }

        /// <summary>
        /// 将内容一行行写入
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static bool WriteLineString(string Path,params string[] lines)
        {
            if (File.Exists(Path))
            {           
                File.WriteAllLines(Path, lines);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据文件名命名规则验证字符串是否符合文件名格式
        ///不能以 “.” 开头， “nul”不能为文件夹/文件的名称.
        /// </summary>
        /// 其中：RegexPatterns引自uoLib.common
        /// <see cref="http://udnz.com/Works/uolib/Default.aspx"/>
        /// 正则表达式有篇好文章：
        /// <see cref="http://www.radsoftware.com.au/articles/regexlearnsyntax.aspx"/>
        public static bool IsFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            else
            {
                // 不能以 “.” 开头
                fileName = fileName.Trim().ToLower();
                if (fileName.StartsWith("."))
                {
                    return false;
                }
                if (!fileName.Contains("."))
                {
                    return false;
                }

                //“con”不能为文件夹/文件的名称
                if (fileName == "con")
                {
                    return false;
                }

                string onlyFileName= Path.GetFileName(fileName);
               
                if (!string.IsNullOrEmpty(onlyFileName))
                {
                    Regex re = new Regex(UtilRegex.FileName, RegexOptions.IgnoreCase);
                    return re.IsMatch(onlyFileName);
                }
                else
                {
                    return false;
                }
                
            }
        }
        
        /// <summary>
        /// ToTest：
        /// 替换文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="targetFile"></param>
        public static void UpdateFile(string srcFile, string targetFile)
        {
            //取得更新程序的路径、源文件、目标文件
            string _updatePath = AppDomain.CurrentDomain.BaseDirectory;
            string _srcFile = Path.Combine(_updatePath, srcFile);
            string _targetFile = Path.Combine(_updatePath, targetFile);

            //如果系统目录下存在文件UpdateDown.exe则将当前的UpdateDown.exe换掉
            if (File.Exists(_srcFile))
            {
                if (File.Exists(_targetFile))
                {
                    File.SetAttributes(_targetFile, FileAttributes.Normal);
                }
                File.Copy(_srcFile, _targetFile, true);
                File.Delete(_srcFile);
            }
        }
    
    }

}
