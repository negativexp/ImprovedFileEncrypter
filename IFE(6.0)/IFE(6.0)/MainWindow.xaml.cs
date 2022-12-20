using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace IFE_6._0_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int chunkSize = 500000;
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
            for(int i = 0; i < listboxFiles.Items.Count; i++)
            {
                if(System.IO.Path.GetExtension(listboxFiles.Items[i].ToString()) != ".bin")
                {
                    EncryptFile(listboxFiles.Items[i].ToString(), pass, i);
                }
                else
                {
                    DecryptFile(listboxFiles.Items[i].ToString(), pass);
                }
                if(i == listboxFiles.Items.Count - 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        TextBlockProgress.Text = "Progress: " + "100%" + " " + listboxFiles.Items.Count + "/" + listboxFiles.Items.Count;
                    });
                }
            }
        }

        private void displayProgress(double progress, int count)
        {
            this.Dispatcher.Invoke(() =>
            {
                TextBlockProgress.Text = "Progress: " + progress + "%" + " " + count++ + "/" + listboxFiles.Items.Count;
            });
        }

        private void Grid_dnd(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string s in files)
                {
                    listboxFiles.Items.Add(s);
                }
            }
        }

        private void listboxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (listboxFiles.SelectedIndex != -1)
                {
                    while (listboxFiles.SelectedItems.Count != 0)
                    {
                        TextBlockLastDeleted.Text = "Last deleted: " + System.IO.Path.GetFileName(listboxFiles.SelectedItems[0].ToString());
                        listboxFiles.Items.Remove(listboxFiles.SelectedItems[0]);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pass = TextBoxPassword.Text;
            TextBlockProgress.Text = "Progress : 0%";
            worker.RunWorkerAsync();
        }

        private void EncryptFile(string inputFile, string password, int count)
        {
            long fileSize = new FileInfo(inputFile).Length;
            string path = System.IO.Path.GetDirectoryName(inputFile) + "\\" + System.IO.Path.GetFileName(inputFile) + ".bin";
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var derive = new Rfc2898DeriveBytes(password, salt, 5000))
            {
                byte[] key = derive.GetBytes(32);
                byte[] iv = derive.GetBytes(16);

                using (var input = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        output.Write(salt, 0, salt.Length);

                        using (var aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;

                            using (var encryptor = aes.CreateEncryptor())
                            {
                                using (var cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                                {
                                    //500000 bytes = 1mb
                                    byte[] buffer = new byte[chunkSize];
                                    int bytesRead;
                                    long totalBytesWritten = 0;
                                    while ((bytesRead = input.Read(buffer, 0, chunkSize)) > 0)
                                    {
                                        cryptoStream.Write(buffer, 0, bytesRead);
                                        totalBytesWritten += bytesRead;
                                        double progress = (100 * totalBytesWritten) / fileSize;

                                        displayProgress(progress, count);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DecryptFile(string inputFile, string password)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(inputFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(inputFile);
                byte[] salt = new byte[16];
                using (FileStream input = new FileStream(inputFile, FileMode.Open))
                {
                    input.Read(salt, 0, salt.Length);
                    using (var derive = new Rfc2898DeriveBytes(password, salt, 5000))
                    {
                        byte[] key = derive.GetBytes(32);
                        byte[] iv = derive.GetBytes(16);
                        using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            using (var aes = Aes.Create())
                            {
                                aes.Key = key;
                                aes.IV = iv;
                                using (var decryptor = aes.CreateDecryptor())
                                {
                                    using (var cryptoStream = new CryptoStream(output, decryptor, CryptoStreamMode.Write))
                                    {
                                        //500000 bytes = 1mb
                                        byte[] buffer = new byte[chunkSize];
                                        int bytesRead;
                                        long totalBytesWritten = 0;
                                        while ((bytesRead = input.Read(buffer, 0, chunkSize)) > 0)
                                        {
                                            cryptoStream.Write(buffer, 0, bytesRead);
                                            totalBytesWritten += bytesRead;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong with decrypting the file. This error might be caused by invalid password!", "Decrypting error");
            }


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Designed to Encrypt and Decrypt files using AES encryption." + Environment.NewLine + Environment.NewLine + "Tip: Use a password that you will remember because I'm not responsible for any lost files.","Created by newoutsider <3");
        }
    }
}
