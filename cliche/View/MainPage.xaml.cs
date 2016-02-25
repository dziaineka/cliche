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
using Windows.UI.Popups;
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
            myClicheFinder.MyDocument = myRichEdit.Document;
            FillListView();
        }

        private async void ClicheFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            await myClicheFinder.FillClichesFromFileAsync();
            FillListView();
            await HighlightClichesAsync();
        }

        private async void CheckTextFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            string checkText;

            await myClicheFinder.FillCheckTextFromFileAsync();
            myClicheFinder.MyDocument.GetText(TextGetOptions.None, out checkText);
            myRichEdit.Document.SetText(TextSetOptions.None, checkText);
            await HighlightClichesAsync();
        }

        private async void myTextField_TextChanged(object sender, RoutedEventArgs e)
        {
            myClicheFinder.MyDocument = myRichEdit.Document;
            myClicheFinder.FindCliches();
            await HighlightClichesAsync();
        }

        private void FillListView()
        {
            listView.Items.Clear();

            foreach (var item in myClicheFinder.MyCliches)
            {
                listView.Items.Add(item.Str);
            }
        }

        private async Task HighlightClichesAsync()
        {
            string myStr;

            foreach (var item in myClicheFinder.MyCliches)
            {
                myRichEdit.Document.GetText(TextGetOptions.None, out myStr);
                if (item.Sum != 0)
                {
                    await ChangeTextColor(item.Str, Color.FromArgb(100, 255, 255, 0));
                }
            }
        }

        private async Task ChangeTextColor(string text, Color color)
        {
            string textStr;
            bool theEnd = false;
            int startTextPos = 0;
            myRichEdit.Document.GetText(TextGetOptions.None, out textStr);

            while (theEnd == false)
            {
                myRichEdit.Document.GetRange(startTextPos, textStr.Length).GetText(TextGetOptions.None, out textStr);
                var isFinded = myRichEdit.Document.GetRange(startTextPos, textStr.Length).FindText(text, textStr.Length, FindOptions.None);
                myRichEdit.Document.GetRange(startTextPos, textStr.Length).FindText(text, textStr.Length, FindOptions.None);

                if (isFinded != 0)
                {
                    string textStr2;
                    textStr2 = myRichEdit.Document.Selection.Text;

                    var dialog = new MessageDialog(textStr2);
                    await dialog.ShowAsync();

                    myRichEdit.Document.Selection.CharacterFormat.BackgroundColor = color;
                    startTextPos = myRichEdit.Document.Selection.EndPosition;
                    myRichEdit.Document.ApplyDisplayUpdates();
                }
                else
                {
                    theEnd = true;
                }
            } 
        }

        private void AddClicheButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }
    }

}
