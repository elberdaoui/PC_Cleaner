using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PC_Cleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebClient updater = new WebClient();
        public string myDir = $@"C:\Users\Administrateur\Desktop\testClean";
        DirectoryInfo winTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
        string sizeToString;

        string Pathx64 = @"C:/Program Files (x86)/Imad/PC Cleaner Setup/";
        string Pathx86 = @"C:\Program Files\Imad\PC Cleaner Setup\";
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ScanStart()
        {
            
            prog_grid.Visibility = Visibility.Visible;
            prog_percentage.Visibility = Visibility.Visible;
            prog_bar.Visibility = Visibility.Visible;
            btn_analyse.IsEnabled = false;
            lbl_analyse.Visibility = Visibility.Hidden;
            lbl_clean.Visibility = Visibility.Hidden;
            lbl_ttl_analyse.Visibility = Visibility.Hidden;
            lbl_ttl_update.Visibility = Visibility.Hidden;
            lbl_update.Visibility = Visibility.Hidden;
            lbl_ttl_clean.Visibility = Visibility.Hidden;
            btn_analyse_menu.IsEnabled = false;
            btn_history.IsEnabled = false;
            btn_home.IsEnabled = false;
            btn_optins.IsEnabled = false;
            btn_home_history.IsEnabled = false;
            btn_home_clean.IsEnabled = false;
            btn_home_update.IsEnabled = false;
            
        }

        public void ScanComplete()
        {
            
            prog_grid.Visibility = Visibility.Hidden;
            prog_percentage.Visibility = Visibility.Hidden;
            prog_bar.Visibility = Visibility.Hidden;
            btn_analyse.IsEnabled = true;
            lbl_analyse.Visibility = Visibility.Visible;
            lbl_clean.Visibility = Visibility.Visible;
            lbl_ttl_analyse.Visibility = Visibility.Visible;
            lbl_ttl_update.Visibility = Visibility.Visible;
            lbl_update.Visibility = Visibility.Visible;
            lbl_ttl_clean.Visibility = Visibility.Visible;
            btn_analyse_menu.IsEnabled = true;
            btn_history.IsEnabled = true;
            btn_home.IsEnabled = true;
            btn_optins.IsEnabled = true;
            btn_home_history.IsEnabled = true;
            btn_home_clean.IsEnabled = true;
            btn_home_update.IsEnabled = true;
            frst_ttl.Content = "Analyse du PC nécessaire";
            lbl_analyse.Content = DateTime.Now.ToString();
        }

        public string Size()
        {
            
            long size = 0;
            

            //var dirInfo = new DirectoryInfo(myDir);

            foreach (FileInfo fi in winTemp.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                size += fi.Length;
            }

            float sizeToMb = (size / 1024f) / 1024f;
            sizeToString = sizeToMb.ToString("F",
                  CultureInfo.InvariantCulture);

            lbl_clean.Content = $"{sizeToString} MB";

            return $"{sizeToString} MB";

        }

        public void dataLog()
        {
            using (StreamWriter sw = File.AppendText("History.txt"))
            {
                sw.WriteLine($"Size of analyse is {sizeToString} MB");
                sw.WriteLine("Date of analyse: " + DateTime.Now.ToString());
                sw.WriteLine("==================");
            }
        } 

        public void Delete()
        {
            //DirectoryInfo di = new DirectoryInfo(myDir);

            foreach (FileInfo file in winTemp.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in winTemp.EnumerateDirectories())
            {
                dir.Delete(true);
            }
                     
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Analyse will start");
            ScanStart();
            frst_ttl.Content = "Analyse in progress";


            Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {                

                    Thread.Sleep(50);
                    this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                    {
                        prog_bar.Value = i;
                        prog_percentage.Text = i.ToString() + "%";

                        Size();

                        if (prog_bar.Value == 100)
                        {
                            MessageBox.Show("Analyse complete");
                            ScanComplete();
                        }
                                            
                    });
                    
                }
             
            });
          
        }

        private void prog_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            

        }

        private void prog_bar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btn_analyse_menu_Click(object sender, RoutedEventArgs e)
        {
            Button_Click_1(sender, e);
        }

        private void btn_home_clean_Click(object sender, RoutedEventArgs e)
        {
            if (prog_bar.Value == 100)
            {
                var msgdel = MessageBox.Show("You want to delete files and folders?", "Warning",
               MessageBoxButton.YesNo, MessageBoxImage.Warning);
                

                if (msgdel == MessageBoxResult.Yes)
                {
                    Task.Run(() =>
                    {
                        for (int i = 0; i <= 100; i++)
                        {

                            Thread.Sleep(50);
                            this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                            {
                                prog_bar.Value = i;
                                prog_percentage.Text = i.ToString() + "%";

                                Size();
                                ScanStart();
                                frst_ttl.Content = "Deleting in progress";


                                if (prog_bar.Value == 100)
                                {
                                    dataLog();
                                    
                                    Delete();
                                    MessageBox.Show("Deleting is completed");
                                    ScanComplete();

                                }
                            });
                        }
                    });



                    
                }
                else
                {                  
                    ScanComplete();
                }
            }
            else
            {
                var msgscn = MessageBox.Show("You should start analyser first", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgscn == MessageBoxResult.Yes)
                {
                    Button_Click_1(sender, e);
                    Size();
                    dataLog();
                    Delete();
                }
                else
                {             
                    ScanComplete();
                }
                
            }
            
        }

        private void History(object sender, RoutedEventArgs e)
        {

            /*var analyseDate = lbl_analyse.Content;
            var analyseSize = $"{sizeToString} MB";*/

            /*Console.WriteLine($"{sizeToString} MB");*/
            /*MessageBox.Show($"{sizeToString} MB");*/


            Process.Start("History.txt");


        }

        private void Button_Analuse_Home(object sender, RoutedEventArgs e)
        {

        }

        private void btn_home_update_Click(object sender, RoutedEventArgs e)
        {
            /*Process.Start("");*/

            if (!updater.DownloadString("https://pastebin.com/raw/3BN82nAN").Contains("1.0"))
            {
                
                if (IntPtr.Size == 8)
                {
                    

                    Process.Start($"{Pathx64}/PC Cleaner.exe");
                    this.Close();
                }
                else
                {
                    

                    Process.Start($"{Pathx86}/PC Cleaner.exe");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("You are in the latest version");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
