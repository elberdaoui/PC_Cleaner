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
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO.Compression;
using System.ComponentModel;
using System.IO;

namespace PC_Cleaner_updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        WebClient updater = new WebClient();
        DirectoryInfo temp = new DirectoryInfo(System.IO.Path.GetTempPath());

        string date = DateTime.Now.ToString("M-d-yyyy");

        string localPath = System.IO.Path.GetTempPath() + "update.zip" ;
        //string localPath = @"C:\Users\Administrateur\Desktop\testClean\update.zip";
        string extPath = @"C:\Users\Administrateur\source\repos\PC Cleaner updater\bin\Release";
        string extPathx64 = @"C:/Program Files (x86)/Imad/PC Cleaner Setup/";
        string extPathx86 = @"C:\Program Files\Imad\PC Cleaner Setup\";

        string comprPath = "C:/Users/Administrateur/source/repos/PC Cleaner updater/bin/Release";
        string comprLocal = "C:/Users/Administrateur/source/repos/PC Cleaner updater/bin/old_version.zip";


        public void Delete()
        {
            if (IntPtr.Size == 8)
            {
                DirectoryInfo di = new DirectoryInfo(extPathx64);

                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                /*foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }*/
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(extPathx86);

                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                /*foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }*/
            }


        }

        private void down_Click(object sender, RoutedEventArgs e)
        {

            //BackgroundWorker work = new BackgroundWorker();

            
               var updateMsg =  MessageBox.Show("There is an update, do you want to download it?", "Updates",
               MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (updateMsg == MessageBoxResult.Yes)
                {
                updater.DownloadFile("https://imadelb.weebly.com/uploads/1/3/5/3/135382967/update.zip", localPath);
                Thread.Sleep(10);
                    Task.Run( () =>
                    {
                        try
                        {
                            
                            
                            for (int i = 0; i <= 100; i++)
                            {

                                Thread.Sleep(10);
                                this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                                {
                                    
                                    download_install.Content = "Downloading...";
                                    prog_bar_down.Value = i;
                                    prog_bar_percent.Text = i.ToString() + "%";

                                    /*updater.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                                    updater.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);*/

                                    if (prog_bar_down.Value == 100)
                                    {
                                        MessageBox.Show("Download finished");
                                        var instlMsg = MessageBox.Show("Do you want to install it?", "Updates",
                                            MessageBoxButton.YesNo, MessageBoxImage.Information);
                                        if (instlMsg == MessageBoxResult.Yes)
                                        {
                                            if(IntPtr.Size == 8)
                                            {
                                                if(!Directory.Exists(extPathx64 + "backup"))
                                                {
                                                    System.IO.Directory.CreateDirectory(extPathx64 + "backup");
                                                    ZipFile.CreateFromDirectory(extPathx64, extPathx64 + "backup/" + date + ".zip");
                                                }
                                                
                                            }
                                            else
                                            {
                                                if (!Directory.Exists(extPathx86 + "backup"))
                                                {
                                                    System.IO.Directory.CreateDirectory(extPathx86 + "backup");
                                                    ZipFile.CreateFromDirectory(extPathx86, extPathx86 + "backup");
                                                }
                                            }
                                            
                                            

                                            Thread.Sleep(10);
                                            Task.Run(() =>
                                            {
                                                try
                                                {

                                                    for (int j = 0; j <= 100; j++)
                                                    {

                                                        Thread.Sleep(10);
                                                        this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                                                        {

                                                            Delete();
                                                            download_install.Content = "Installing...";
                                                            prog_bar_down.Value = j;
                                                            prog_bar_percent.Text = j.ToString() + "%";




                                                            if (prog_bar_down.Value == 100)
                                                            {
                                                                if(IntPtr.Size == 8)
                                                                {
                                                                    ZipFile.ExtractToDirectory(localPath, extPathx64);
                                                                    MessageBox.Show("Install finished");

                                                                    Process.Start($"{extPathx64}/PC Cleaner.exe");
                                                                    this.Close();
                                                                }
                                                                else
                                                                {
                                                                    ZipFile.ExtractToDirectory(localPath, extPathx86);
                                                                    MessageBox.Show("Install finished");

                                                                    Process.Start($"{extPathx86}/PC Cleaner.exe");
                                                                    this.Close();
                                                                }
                                                                
                                                            }

                                                        });

                                                    }

                                                }
                                                catch (Exception)
                                                {
                                                    MessageBox.Show("Failed to install");
                                                    Environment.Exit(0);
                                                }
                                                updater.Dispose();

                                            });

                                        }
                                    }



                                });

                            }

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Failed to download");
                            Environment.Exit(0);
                        }
                        updater.Dispose();

                    });
                }
                else
                {
                    Environment.Exit(0);
                }

            
        }

        /*private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("Download Successfull!");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            prog_bar_down.Value = int.Parse(Math.Truncate(percentage).ToString());
        }*/

        private void prog_bar_down_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void prog_bar_down_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            updater.DownloadFile("https://imadelb.weebly.com/uploads/1/3/5/3/135382967/update.zip", localPath);

            if (!Directory.Exists(extPathx64 + "backup"))
            {
                System.IO.Directory.CreateDirectory(extPathx64 + "backup");
                //ZipFile.CreateFromDirectory(extPathx64, extPathx64 + "backup/verr.zip");
                ZipFile.CreateFromDirectory(extPathx64, extPathx64 + "backup/" + date + ".zip");
                MessageBox.Show(date);
            }
            else
            {
                //ZipFile.CreateFromDirectory(extPathx64, extPathx64 + "backup/verr.zip");
                ZipFile.CreateFromDirectory(extPathx64, $"{extPathx64}backup/{date}.zip");
                MessageBox.Show(date);
            }
        }*/
    }
}
