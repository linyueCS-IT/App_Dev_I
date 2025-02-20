using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    /// <summary>
    /// Manages the files used in the Budget project.
    /// </summary>
    public class BudgetFiles
    {
        private static String DefaultSavePath = @"Budget\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================

        /// <summary>
        /// Verifies the file path for reading a file. If the provided file path is null,using a default file path 
        /// which is constructed using the application's default directories.If the provided file doesn't exist, 
        /// throw a exception. Return valided file path
        /// </summary>
        /// <param name="FilePath"> The file path to verify. </param>
        /// <param name="DefaultFileName">
        /// The default file name to use when pass file path is null.
        /// </param>
        /// <returns> The verified file path as a string. </returns>
        /// <exception cref="FileNotFoundException">Thrown if the resolved file path does not exist.  </exception>
        /// <remarks>
        /// - If <paramref name="FilePath"/> is null, the method constructs the path using 
        ///   a combination of `DefaultAppData`, `DefaultSavePath`, and `DefaultFileName`.
        /// - The method ensures the resolved file path exists before returning it.
        /// </remarks>
        /// <example>
        /// <code>
        /// string filePath = "./Data/MyFile.txt";
        /// string DefaultFileName = "./../../defultFile.txt";
        /// try
        /// {
        ///     string verifiedFilePath = BudgetFiles.VerifyReadFromFileName(filePath,DefaultFileName);
        ///     Console.WriteLine($"Verfied file path: {verifiedFilePath}");       
        /// }
        /// catch(Exception ex)
        /// {
        ///     Console.WriteLine($"Error in {ex.Message}");    
        /// }        
        /// </code>
        /// </example>
        public static String VerifyReadFromFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }
        
            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;
        }

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it writable
        // ====================================================================

        /// <summary>
        /// Verifies the file path for writing a file. If the provided file path is null,using a default file path 
        /// which is constructed using the application's default directories.If the provided file doesn't exist, 
        /// throw a exception. Return valided file path    
        /// </summary>
        /// <param name="FilePath"> The file path to verify.</param>
        /// <param name="DefaultFileName">The default file name to use when pass file path is null.</param>
        /// <returns> The verified file path as a string.</returns>
        /// <exception cref="Exception">Thrown if the resolved file path does not exist 
        /// or save to directory which doen't exsit
        /// </exception>
        /// <example>
        /// <code>
        /// string filePath = "./Data/MyFile.txt";
        /// string DefaultFileName = "./../../defultFile.txt";
        /// try
        /// {
        ///     string verifiedFilePath = BudgetFiles.VerifyWriteToFileName(filePath,DefaultFileName);
        ///     Console.WriteLine($"Verfied file path: {verifiedFilePath}");       
        /// }
        /// catch(Exception ex)
        /// {
        ///     Console.WriteLine($"Error in {ex.Message}");     
        /// }        
        /// </code>
        /// </example>
        public static String VerifyWriteToFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Budget directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }
            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;
        }
    }
}
