using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NVSHelper.Common.FileManagement
{
    public class FileManagement
    {
        #region "Memory Stream"

        public bool MemoryStreamWrite(string path, MemoryStream ms, ref string messageError)
        {
            try
            {
                File.WriteAllBytes(path, ms.ToArray());
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return false;
            }

            return true;
        }

        public MemoryStream MemoryStreamRead(string path, ref string messageError)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    file.CopyTo(ms);
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return null;
            }

            return ms;
        }

        #endregion


        public bool DeleteFileByPath(string path, ref string messageError)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return false;
            }

            return true;
        }



    }
}
