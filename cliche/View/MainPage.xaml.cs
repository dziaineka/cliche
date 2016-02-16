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

        private void ClicheFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            myClicheFinder.FillClichesFromFile();
            FillListView();
        }

        private void CheckTextFileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            string checkText;

            myClicheFinder.FillCheckTextFromFile();
            myClicheFinder.MyDocument.GetText(TextGetOptions.None, out checkText);
            myTextField.Document.SetText(TextSetOptions.None, checkText);
        }

        private void myTextField_TextChanged(object sender, RoutedEventArgs e)
        {
            myClicheFinder.MyDocument = myTextField.Document;
            myClicheFinder.FindCliches();
        }

        //Todo: repear listview
        private void FillListView()
        {
            foreach (var item in myClicheFinder.MyCliches)
            {
                listView.Items.Add(item.Str);
            }
        }
    }
}
