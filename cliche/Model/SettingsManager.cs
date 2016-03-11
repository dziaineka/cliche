using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cliche.Model
{
    static class SettingsManager
    {
        const string clicheFileName = "cliche.txt";

        static public string ClicheFileString
        {
            get
            {
                if (IsClicheExist())
                {
                    IsolatedStorageFile clicheFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    IsolatedStorageFileStream clicheFileStream = clicheFileStorage.OpenFile(clicheFileName, FileMode.Open);

                    StreamReader sr = new StreamReader(clicheFileStream);
                    string clicheFileString = sr.ReadToEnd();
                    sr.Dispose();
                    clicheFileStream.Dispose();
                    return clicheFileString;
                }

                return null;
            }

            set
            {
                IsolatedStorageFile clicheFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream clicheFileStream = clicheFileStorage.CreateFile(clicheFileName);

                StreamWriter sw = new StreamWriter(clicheFileStream);
                sw.Write(value);
                sw.Dispose();

                clicheFileStream.Dispose();
            }
        }

        static bool IsClicheExist()
        {
            IsolatedStorageFile clicheFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            return clicheFileStorage.FileExists(clicheFileName);
        }
    }
}
