using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 *  Date            Author      Description
 *  Oct 25 2014     Mishi       Initial Version
 * 
 * 
 */

namespace Mishi.Utilities
{
    /// <summary>
    /// Represents a logging utility that handles the logging of supplied string data to a prefix_yyyyMMdd.log file in a specified directory.
    /// </summary>
    public class Logger
    {
        private string fpath;
        private string fprefix;
        private string src;
        private string pid;
        private DateTime dtToday;
        private TextWriter tw;
        private string fn;

        /// <summary>
        /// Initialize a Logger object.
        /// </summary>
        /// <param name="path">The path of the log file.</param>
        /// <param name="prefix">The prefix to the filename of the log file.</param>
        /// <param name="source">The name of the resource that uses this object.</param>
        /// <param name="processid">The ID of the resource that uses this object.</param>
        public Logger(string path, string prefix, string source, string processid)
        {
            //set the properties
            this.fpath = path;
            this.fprefix = prefix;
            this.src = source;
            this.pid = processid;
            dtToday = DateTime.Today.Date;

            //Open and lock the file
            OpenAndLockFile();
        }

        private bool OpenAndLockFile()
        {
            bool isReady = false;

            try
            {
                //get the expected filename for today = prefix + dtToday
                fn = this.fpath + this.fprefix + "_" + this.dtToday.ToString("yyyyMMdd") + ".log";

                //check if the path exists
                if (!Directory.Exists(@fpath))
                {
                    Directory.CreateDirectory(@fpath);
                }

                //check if file exists, put headers
                if (!File.Exists(@fn))
                {
                    LogHeaders();
                }

                isReady = true;
            }
            catch (Exception e)
            {
                isReady = false;
            }

            return isReady;
        }

        private void LogHeaders()
        {
            //Create headers
            //INFORMATION - DT - SOURCE - PID - MSG
            tw = new StreamWriter(@fn, true);
            tw.WriteLine(string.Format("{0,-10}{1,-25}{2,-25}{3,-4}{4,-50}", "TYPE", "DATE", "SOURCE", "PID", "MESSAGE"));
            tw.Close();
        }

        /// <summary>
        /// Logs an informative message
        /// </summary>
        /// <param name="msg">message to log</param>
        public void LogInfo(string msg)
        {
            //INFORMATION - DT - SOURCE - PID - MSG
            tw = new StreamWriter(@fn, true);
            tw.WriteLine(string.Format("{0,-10}{1,-25}{2,-25}{3,-4}{4,-50}","Info", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), this.src, this.pid, msg));
            tw.Close();
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="msg">message to log</param>
        public void LogError(string msg)
        {
            //ERROR - DT - SOURCE - PID - MSG
            tw = new StreamWriter(@fn, true);
            tw.WriteLine(string.Format("{0,-10}{1,-25}{2,-25}{3,-4}{4,-50}", "Error", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), this.src, this.pid, msg));
            tw.Close();
        }

        /// <summary>
        /// Disposes the logger object
        /// </summary>
        public void Close()
        {
            tw.Flush();
            tw.Dispose();           
        }
        
    }
}
