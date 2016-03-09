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
        private string highlightedText;
        private object selectedListViewItem;

        public MainPage()
        {
            this.InitializeComponent();
            myClicheFinder = new ClicheFinder();
            myClicheFinder.MyDocument = myRichEdit.Document;
            FillListView();
            myRichEdit.Document.GetText(TextGetOptions.None, out highlightedText); 
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
        }

        private async void myTextField_TextChanged(object sender, RoutedEventArgs e)
        {
            myClicheFinder.MyDocument = myRichEdit.Document;

            string myStr;
            myRichEdit.Document.GetText(TextGetOptions.None, out myStr);

            if (highlightedText != myStr)
            {
                myClicheFinder.FindCliches();
                await HighlightClichesAsync();
            }
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
            int myIndex;

            myIndex = myRichEdit.Document.Selection.StartPosition;
            myRichEdit.Document.BatchDisplayUpdates();

            myRichEdit.Document.GetText(TextGetOptions.None, out myStr);
            myRichEdit.Document.Selection.SetRange(0, myStr.Length);
            myRichEdit.Document.Selection.CharacterFormat.BackgroundColor = Colors.White;

            foreach (var item in myClicheFinder.MyCliches)
            {
                if (item.Sum != 0)
                {
                    await ChangeTextColor(item.Str, Color.FromArgb(100, 255, 255, 0));
                }
            }

            highlightedText = myStr;
            myRichEdit.Document.Selection.SetRange(myIndex, myIndex);
            myRichEdit.Document.Selection.CharacterFormat.BackgroundColor = Colors.White;
            myRichEdit.Document.ApplyDisplayUpdates();
        }

        private async Task ChangeTextColor(string text, Color color)
        {
            string textStr;

            myClicheFinder.MyDocument.GetText(TextGetOptions.None, out textStr);

            var myRichEditLength = textStr.Length;

            myRichEdit.Document.Selection.SetRange(0, myRichEditLength);
            int i = 1;
            while (i > 0)
            {
                i = myRichEdit.Document.Selection.FindText(text, myRichEditLength, FindOptions.Case);

                ITextSelection selectedText = myRichEdit.Document.Selection;
                if (selectedText != null)
                {
                    selectedText.CharacterFormat.BackgroundColor = color;
                }
            }
        }

        private void AddClicheButton_Click(object sender, RoutedEventArgs e)
        {
            addClicheButtonFlyout.ShowAt((AppBarButton)sender);
        }

        private void listView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext != null)
            {
                selectedListViewItem = (FrameworkElement)e.OriginalSource;
                lVcontextMenu.ShowAt(listView, e.GetPosition(listView));
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            lVcontextMenu.Hide();
        }

        private void DeleteMenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DeleteListViewItem(selectedListViewItem);
        }

        private void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteListViewItem(selectedListViewItem);
        }

        private void DeleteListViewItem(object item)
        {
            listView.Items.Remove(item);
        }

        private void AddToLVClicheButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(clicheTextBox.Text) == false)
            {
                listView.Items.Add(clicheTextBox.Text);
                clicheTextBox.Text = "";
                RescanScreenClicheList();
            }
        }

        private async void RescanScreenClicheList()
        {
            if (myClicheFinder.FillClichesFromCollection(listView.Items))
            {
                await HighlightClichesAsync();
            }
        }
    }

}
