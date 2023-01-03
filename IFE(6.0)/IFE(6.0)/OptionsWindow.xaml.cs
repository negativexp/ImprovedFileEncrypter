using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace IFE_6._0_
{
    /// <summary>
    /// Interakční logika pro OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private readonly MainWindow _mainWindow;
        CommonOpenFileDialog dlg = new CommonOpenFileDialog();
        int buffersize;
        public OptionsWindow(MainWindow mw)
        {
            InitializeComponent();
            _mainWindow = mw;
            dlg.Title = "Select folder";
            dlg.IsFolderPicker = true;
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextBoxBuffer.Text, out buffersize))
            {
                _mainWindow.changeSettings(buffersize, TextBoxLoaction.Text);
                MessageBox.Show("settings changed");
            }
            else
            {
                MessageBox.Show("Please enter right buffer size or loaction!", "Error");
            }

        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            if(dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TextBoxLoaction.Text = dlg.FileName;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxLoaction.IsEnabled = false;
            ButtonBrowse.IsEnabled = false;
            TextBoxLoaction.Text = "";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TextBoxLoaction.IsEnabled = true;
            ButtonBrowse.IsEnabled = true;
        }
    }
}
