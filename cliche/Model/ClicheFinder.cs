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
using System.IO.IsolatedStorage;

namespace cliche.Model
{
    class ClicheFinder
    {
        public class Cliche
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
            myCliches = new List<Cliche>();

            if (FillClichesFromSettings(SettingsManager.ClicheFileString))
            {

            }
            else
            {
                myCliches = new List<Cliche>
                {
                    new Cliche("Ни для кого не секрет",0),
                    new Cliche("Вершина айсберга",0),
                    new Cliche("Разобраться в хитросплетениях",0),
                    new Cliche("Воссоздать атмосферу",0),
                    new Cliche("Окунуться в атмосферу",0),
                    new Cliche("Как снег на голову",0),
                    new Cliche("По праву считается",0),
                    new Cliche("Крепкая ячейка общества",0),
                    new Cliche("Отличается оригинальностью решений",0),
                    new Cliche("Высокие темпы роста",0),
                    new Cliche("Уделяет особое внимание",0),
                    new Cliche("Не имеет аналогов за рубежом",0),
                    new Cliche("Широкий ассортимент",0),
                    new Cliche("Гибкая ценовая политика",0),
                    new Cliche("Обсудить волнующие вопросы",0),
                    new Cliche("Путешествие по закоулкам памяти",0),
                    new Cliche("В сухом остатке",0),
                    new Cliche("Яблоку негде упасть",0),
                    new Cliche("Средь бела дня",0),
                    new Cliche("На самом деле",0),
                    new Cliche("Помимо этого",0),
                    new Cliche("На сегодняшний день",0),
                    new Cliche("Оставляет желать лучшего",0),
                    new Cliche("Чиновники заверили",0),
                    new Cliche("Аномальная погода",0),
                    new Cliche("Служители Мельпомены",0),
                    new Cliche("Небесная канцелярия",0),
                    new Cliche("Остается надеяться",0),
                    new Cliche("Стало доброй традицией",0),
                    new Cliche("Главное не победа, а участие",0),
                    new Cliche("Ваш покорный слуга",0),
                    new Cliche("Выразил надежду",0),
                    new Cliche("Новый формат",0),
                    new Cliche("Кузница чемпионов",0),
                    new Cliche("Планы на будущее",0),
                    new Cliche("Он и представить не мог",0),
                    new Cliche("Рано или поздно",0)
                };
            }
        }

        public List<Cliche> MyCliches
        {
            get
            {
                return myCliches;
            }
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
            if (doc != null)
            {
                doc.GetText(TextGetOptions.None, out myText);

                foreach (Cliche theCliche in myCliches)
                {
                    theCliche.Sum = new Regex(theCliche.Str.ToUpper()).Matches(myText.ToUpper()).Count;
                }

                return true;
            }
            else
            {
                return false;
            }
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

                SettingsManager.ClicheFileString = await FileIO.ReadTextAsync(storageFile);
                FindCliches();
            }
        }

        public bool FillClichesFromSettings(string clicheFileString)
        {
            if (clicheFileString != null)
            {
                string clicheLine;
                StringReader strReader = new StringReader(clicheFileString);
                myCliches.Clear();

                while ((clicheLine = strReader.ReadLine()) != null)
                {
                    myCliches.Add(new Cliche(clicheLine, 0));
                }

                FindCliches();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FillClichesFromCollection(ItemCollection clicheCollection)
        {
            if (clicheCollection != null)
            {
                string strToStorageCliches = "";
                myCliches.Clear();

                foreach (var item in clicheCollection)
                {
                    myCliches.Add(new Cliche((item as String), 0));
                    strToStorageCliches += ((item as string)+"\r");
                }

                SettingsManager.ClicheFileString = strToStorageCliches;
                FindCliches();
                return true;
            }
            else
            {
                return false;
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

        public async Task FillClichesFromFileAsync()
        {
            var clichesFile = await OpenTxtFileAsync();
            await FillClichesFromFileAsynk(clichesFile); 
        }


        public async Task FillCheckTextFromFileAsync()
        {
            var textFile = await OpenTxtFileAsync();
            await FillCheckTextFromFileAsync(textFile);
        }
    }
}
