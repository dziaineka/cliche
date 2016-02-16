using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace cliche.Model
{
    class ClicheFinder
    {
        private class Cliche
        {
            public string Str { get; set; }
            public int Sum { get; set; }

            public Cliche(string str, int sum)
            {
                Str = str;
                Sum = sum;
            }
        }

        private ITextDocument myDocument;
        private string myText;
        private List<Cliche> myCliches;

        public ClicheFinder()
        {
            myCliches = new List<Cliche>
            {
                new Cliche("высокие темпы роста",0),
                new Cliche("труженики полей",0),
                new Cliche("на сегодняшний день",0)
            };
        }
        
        public ITextDocument MyDocument
        {
            get
            {
                return myDocument;
            }

            set
            {
                myDocument = value;
            }
        }

        public bool FindCliches(ITextDocument doc)
        {
            doc.GetText(TextGetOptions.None, out myText);

            foreach (Cliche theCliche in myCliches)
            {
                theCliche.Sum = new Regex(theCliche.Str.ToUpper()).Matches(myText.ToUpper()).Count;
            }

            return true;
        }

        public bool FindCliches()
        {
            FindCliches(MyDocument);
            return true;
        }

        public async Task<StorageFile> OpenTxtFileAsync()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;                
        }

        public async Task FillClichesFromFileAsynk(StorageFile storageFile)
        {
            if (storageFile != null)
            {                
                var massiveOfString = await FileIO.ReadLinesAsync(storageFile);
                myCliches.Clear();

                foreach (string clicheStr in massiveOfString)
                {
                    myCliches.Add(new Cliche(clicheStr, 0));
                }
            }
        }

        public async Task FillCheckTextFromFileAsync(StorageFile storageFile)
        {
            if (storageFile != null)
            {
                string checkText;
                checkText = await FileIO.ReadTextAsync(storageFile);
                MyDocument.SetText(TextSetOptions.None, checkText);
                
            }          
        }

        public async void FillClichesFromFile()
        {
            var clichesFile = await OpenTxtFileAsync();
            await FillClichesFromFileAsynk(clichesFile);
        }


        public async void FillCheckTextFromFile()
        {
            var textFile = await OpenTxtFileAsync();
            await FillCheckTextFromFileAsync(textFile);
        }
    }
}
