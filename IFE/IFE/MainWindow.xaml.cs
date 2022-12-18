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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using Path = System.IO.Path;

namespace IFE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        string pass = "";
        BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string item in listboxFiles.Items)
            {
                EncryptFile(item, pass);
            }
        }

        private void displayProgress(long total, long filesize)
        {
            this.Dispatcher.Invoke(() =>
            {
                double progress = (100 * total) / filesize;
            TextBlockProgress.Text = "progress: " + progress;
            });
        }

        private void Grid_dnd(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach(string s in files)
                {
                    listboxFiles.Items.Add(s);
                }
            }
        }

        private void listboxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                if(listboxFiles.SelectedIndex != -1)
                {
                    while(listboxFiles.SelectedItems.Count != 0)
                    {
                        listboxFiles.Items.Remove(listboxFiles.SelectedItems[0]);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pass = TextBoxPassword.Text;
            worker.RunWorkerAsync();
        }

        private void EncryptFile(string inputFile, string password)
        {
            long fileSize = new FileInfo(inputFile).Length;
            string path = Path.GetDirectoryName(inputFile) + "\\" +Path.GetFileNameWithoutExtension(inputFile) + ".bin";
            byte[] salt = new byte[16];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using(var derive = new Rfc2898DeriveBytes(password, salt, 5000))
            {
                byte[] key = derive.GetBytes(32);
                byte[] iv = derive.GetBytes(16);

                using(var input = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    using(var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        output.Write(salt, 0, salt.Length);

                        using(var aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;

                            using(var encryptor = aes.CreateEncryptor())
                            {
                                using(var cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                                {
                                    const int chunkSize = 4096;
                                    byte[] buffer = new byte[chunkSize];
                                    int bytesRead;
                                    long totalBytesWritten = 0;
                                    while((bytesRead = input.Read(buffer, 0, chunkSize)) > 0)
                                    {
                                        cryptoStream.Write(buffer, 0, bytesRead);
                                        totalBytesWritten += bytesRead;
                                        
                                        displayProgress(totalBytesWritten, fileSize);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
