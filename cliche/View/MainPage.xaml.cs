using cliche.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace cliche
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ClicheFinder myClicheFinder;

        public MainPage()
        {
            this.InitializeComponent();
            myClicheFinder = new ClicheFinder();
            myClicheFinder.MyDocument = myTextField.Document;
            FillListView();
        }

        private async void ClicheFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            await myClicheFinder.FillClichesFromFileAsync();
            FillListView();
            HighlightCliches();
        }

        private void CheckTextFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            string checkText;

            myClicheFinder.FillCheckTextFromFileAsync();
            myClicheFinder.MyDocument.GetText(TextGetOptions.None, out checkText);
            myTextField.Document.SetText(TextSetOptions.None, checkText);
        }

        private void myTextField_TextChanged(object sender, RoutedEventArgs e)
        {
            myClicheFinder.MyDocument = myTextField.Document;
            myClicheFinder.FindCliches();
        }

        private void FillListView()
        {
            listView.Items.Clear();

            foreach (var item in myClicheFinder.MyCliches)
            {
                listView.Items.Add(item.Str);
            }
        }

        private void HighlightCliches()
        {
            foreach (var item in myClicheFinder.MyCliches)
            {
                ChangeTextColor(item.Str, Color.FromArgb(100,255,255,0));
            }
        }

        private void ChangeTextColor(string text, Color color)
        {
            string textStr;
            myTextField.Document.GetText(TextGetOptions.None, out textStr);

            //здесь сделать круговую обработк поиск

            myTextField.Document.Selection.FindText(text, textStr.Length, FindOptions.None);
            myTextField.Document.Selection.CharacterFormat.BackgroundColor = color;
            myTextField.Document.ApplyDisplayUpdates();
        }
    }

}
