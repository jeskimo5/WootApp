using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace WootAlert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region WootVars
        string title;
        string price;
        string description;
        string purchaseUrl;
        string stdImageUrl;
        string scrollColor;
        string webBackColor;
        bool wootOff;
        bool soldOut;
        string discURL;
        string detailImageUrl;
        IList<string> additionalImages = new List<string>();
        bool isPicWindowOpen;
        decimal soldPercent;
        bool windowIsUp = false;
        string previousWootTitle;
        DispatcherTimer dispatch;
        Settings settings;
        Window imageWindow;
        ColorScheme currentColorScheme;
        Theme wootTheme;
        Theme kidsTheme;
        Theme shirtTheme;
        Theme wineTheme;
        TaskbarIcon forBaloonIcon = new TaskbarIcon();

        public bool IsPicWindowOpen { get => isPicWindowOpen; set => isPicWindowOpen = value; }
        public Window ImageWindow { get => imageWindow; set => imageWindow = value; }
        public Theme KidsTheme { get => kidsTheme; set => kidsTheme = value; }

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            //add event handler for the tool tip
            MyNotifyIcon.TrayBalloonTipClicked += new RoutedEventHandler(MyNotifyIcon_TrayBalloonTipClicked);
            // Update Timer Logic
            forBaloonIcon.IconSource = BitmapFrame.Create(new Uri("pack://application:,,,/WootAlert;component/Themes/Common/tray.ico"));
            forBaloonIcon.Visibility = System.Windows.Visibility.Hidden;
            //need this handler to do proper resizing
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        public void SetTimerInterval()
        {
            if (wootOff == true)
            {
                dispatch.Interval = new TimeSpan(0, 0, (int)settings.WootOffRefresh);
            }
            else
            {
                dispatch.Interval = new TimeSpan(0, (int)settings.WootRefresh, 0);
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            currentColorScheme = ColorScheme.Woot;
            settings = new Settings();
            //enter all of the colors for teh 4 themes
            wootTheme = new Theme("#FF91AB62", "#91ab62", "#FF72961D", "#61861E", "Woot");
            KidsTheme = new Theme("#FFFFE086", "#ffe086", "#FFBF6D0C", "#bf6d0c", "Kids");
            shirtTheme = new Theme("#FF5A8BB0", "#5a8bb0", "#FF216294", "#206294", "Shirt");
            wineTheme = new Theme("#FFAD5D62", "#ad5d62", "#FF8A1B26", "#8a1b26", "Wine");

            this.Top = SystemParameters.WorkArea.Height - this.ActualHeight;
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth - 1;
            Themer(ColorScheme.Woot);
            WootXml(true);
            try
            {
                Update();
                dispatch = new DispatcherTimer();
                SetTimerInterval();
                dispatch.Tick += new EventHandler(Dispatch_Tick);
                dispatch.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "////" + error.InnerException);
            }

        }

        //shows a balloon tip if there is a new woot item. also updates the the window if you are in woot theme.
        void Dispatch_Tick(object sender, EventArgs e)
        {
            if (currentColorScheme == ColorScheme.Woot)
            {
                WootXml(false);
            }
            else if (currentColorScheme == ColorScheme.Shirt)
            {
                ShirtXml(false);
            }
            else if (currentColorScheme == ColorScheme.Wine)
            {
                WineXml(false);
            }
            else if (currentColorScheme == ColorScheme.Kids)
            {
                KidsXml(false);
            }
            if (!wootOff)
            {
                if (previousWootTitle != title)
                {
                    previousWootTitle = title;
                    //if (currentColorScheme == ColorScheme.Woot)
                    //{
                        Update();
                        ShowBalloonTip();
                        if (settings.PlayAlertSound)
                        {
                            SystemSounds.Asterisk.Play();
                        }
                    //}
                    //else
                    //{
                    //    ShowBalloonTip();
                    //    if (settings.PlayAlertSound)
                    //    {
                    //        SystemSounds.Asterisk.Play();
                    //    }
                    //}
                }

            }
            else
            {

                //if (currentColorScheme == ColorScheme.Woot)
                //{
                    Update();
                    if (previousWootTitle != title)
                    {
                        ShowBalloonTip();
                        if (settings.PlayAlertSound)
                        {
                            SystemSounds.Asterisk.Play();
                        }
                    }
                //}
                //else
                //{
                //    if (previousWootTitle != title)
                //    {
                //        ShowBalloonTip();
                //        if (settings.PlayAlertSound)
                //        {
                //            SystemSounds.Asterisk.Play();
                //        }
                //    }
                //}
                previousWootTitle = title;
            }
        }
        /// <summary>
        /// This method takes all of the data pulled from the different xml modules and puts it into the window.
        /// It only queries for the new images if it has to.
        /// </summary>
        public void Update()
        {//catch for if offline
            if (title != null)
            {
                if (productTitleBlock.Text != title)
                {
                    productTitleBlock.Text = title;
                    label2.Content = "Price: " + price;

                    try
                    {
                        productImage.Source = loadPic(stdImageUrl).Source;
                    }
                    catch (NullReferenceException)
                    { }
                    finally { }

                    progressBar1.Value = (double)soldPercent * 100;
                    description = Regex.Replace(description, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    description = "<html><head><style type=\"text/css\">body {font-family:Segoe UI;font-size=13px;} /r/n <!--html {scrollbar-base-color:" + scrollColor + "}-->"
                        + "</style><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><script type=\"text/javascript\">function lightboxImage(b){window.open(b);}</script> </head><body bgcolor=" + webBackColor + ">" + description + "</body></html>";
                    webBrowser1.NavigateToString(description);
                    // webBrowser1.Source= new Uri("http://www.woot.com");
                }
                //wootoff logic, determines if wootoff controls are shown
                if (wootOff)
                {
                    image6.Visibility = System.Windows.Visibility.Visible;
                    image7.Visibility = System.Windows.Visibility.Visible;
                    MyNotifyIcon.IconSource = BitmapFrame.Create(new Uri("pack://application:,,,/WootAlert;component/Themes/Common/favicon.ico"));
                    progressBar1.Visibility = System.Windows.Visibility.Visible;
                    progressBar1.Value = 100 - ((double)soldPercent * 100);
                    if (progressBar1.Value <= 20)
                    {
                        progressBar1.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.Colors.Red);
                    }
                    else if (progressBar1.Value > 20 & progressBar1.Value <= 40)
                    {
                        progressBar1.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFFF00"));
                    }
                    else
                    {
                        progressBar1.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF01D328"));
                    }

                }
                else
                {
                    image6.Visibility = System.Windows.Visibility.Hidden;
                    image7.Visibility = System.Windows.Visibility.Hidden;
                    MyNotifyIcon.IconSource = BitmapFrame.Create(new Uri("pack://application:,,,/WootAlert;component/Themes/Common/tray.ico"));
                    progressBar1.Visibility = System.Windows.Visibility.Hidden;
                }
                //shows the sold out image if the item is sold out
                if (soldOut)
                {
                    wantOneImage.Visibility = System.Windows.Visibility.Hidden;
                    soldOutImage.Visibility = System.Windows.Visibility.Visible;

                }
                else
                {
                    wantOneImage.Visibility = System.Windows.Visibility.Visible;
                    soldOutImage.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            else { productTitleBlock.Text = "Unable to connect to Woot.com, Retry in 10 min."; }


        }
        //balloon tip shows
        public void ShowBalloonTip()
        {
            string alert = "Woot Alert!!!";
            string text = "New Woot Item!: " + title;


            MyNotifyIcon.ShowBalloonTip(alert, text, forBaloonIcon.Icon);

        }
        //this method themes the windows to the appropriate color scheme.
        public void Themer(ColorScheme scheme)
        {
            switch (scheme)
            {
                case ColorScheme.Woot:
                    minimizeImage.Source = new BitmapImage(wootTheme.MinimizeUri);
                    if (windowIsUp)
                    {
                        bulbImage.Source = new BitmapImage(wootTheme.ContractBulb);
                    }
                    else
                    {
                        bulbImage.Source = new BitmapImage(wootTheme.ExpandBulb);
                    }
                    this.BorderBrush = wootTheme.BorderColor;
                    grid1.Background = wootTheme.BackgroundColor;
                    scrollColor = wootTheme.ScrollBarColor;
                    currentColorScheme = ColorScheme.Woot;
                    webBackColor = wootTheme.WebBackgroundColor;
                    break;
                case ColorScheme.Shirt:
                    minimizeImage.Source = new BitmapImage(shirtTheme.MinimizeUri);
                    if (windowIsUp)
                    {
                        bulbImage.Source = new BitmapImage(shirtTheme.ContractBulb);
                    }
                    else
                    {
                        bulbImage.Source = new BitmapImage(shirtTheme.ExpandBulb);
                    }
                    this.BorderBrush = shirtTheme.BorderColor;
                    grid1.Background = shirtTheme.BackgroundColor;
                    scrollColor = shirtTheme.ScrollBarColor;
                    currentColorScheme = ColorScheme.Shirt;
                    webBackColor = shirtTheme.WebBackgroundColor;
                    break;
                case ColorScheme.Wine:
                    minimizeImage.Source = new BitmapImage(wineTheme.MinimizeUri);
                    if (windowIsUp)
                    {
                        bulbImage.Source = new BitmapImage(wineTheme.ContractBulb);
                    }
                    else
                    {
                        bulbImage.Source = new BitmapImage(wineTheme.ExpandBulb);
                    }
                    this.BorderBrush = wineTheme.BorderColor;
                    grid1.Background = wineTheme.BackgroundColor;
                    scrollColor = wineTheme.ScrollBarColor;
                    currentColorScheme = ColorScheme.Wine;
                    webBackColor = wineTheme.WebBackgroundColor;
                    break;
                case ColorScheme.Kids:
                    minimizeImage.Source = new BitmapImage(KidsTheme.MinimizeUri);
                    if (windowIsUp)
                    {
                        bulbImage.Source = new BitmapImage(KidsTheme.ContractBulb);

                    }
                    else
                    {
                        bulbImage.Source = new BitmapImage(KidsTheme.ExpandBulb);
                    }
                    this.BorderBrush = KidsTheme.BorderColor;
                    grid1.Background = KidsTheme.BackgroundColor;
                    scrollColor = KidsTheme.ScrollBarColor;
                    currentColorScheme = ColorScheme.Kids;
                    webBackColor = KidsTheme.WebBackgroundColor;
                    break;
            }
        }

        #region XmlMethods
        public void WootXml(bool callSetPrevWoot)
        {
            WebClient client = new WebClient();
            try
            {
                byte[] pageBytes = client.DownloadData(new Uri("http://www.woot.com/salerss.aspx"));





                string pageCode = Encoding.UTF8.GetString(pageBytes);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pageCode);
                //XmlNodeList nodes = doc.GetElementsByTagName("item");

                XmlNode item = doc.SelectSingleNode("/rss/channel/item");
                //XmlNodeList itemProperties = nodes[0].ChildNodes;
                title = item.SelectSingleNode("title").InnerText;
                if (callSetPrevWoot)
                {
                    previousWootTitle = title;
                }

                description = item.SelectSingleNode("description").InnerText;
                //title = itemProperties.Item(;
                //if (callSetPrevWoot)
                //{
                //    previousWootTitle = title;
                //}
                //description = itemProperties.Item(1).InnerText;

                purchaseUrl = doc.GetElementsByTagName("woot:purchaseurl")[0].InnerText;
                stdImageUrl = doc.GetElementsByTagName("woot:substandardimage")[0].InnerText;
                soldPercent = decimal.Parse(doc.GetElementsByTagName("woot:soldoutpercentage")[0].InnerText);
                detailImageUrl = doc.GetElementsByTagName("woot:detailimage")[0].InnerText;

                Regex rx = new Regex("lightboxImage.+;\">");
                MatchCollection matches = rx.Matches(description);
                additionalImages.Clear();
                foreach (Match s in matches)
                {
                    additionalImages.Add(description.Substring(s.Index + 15, s.Length - 20));
                }

                price = doc.GetElementsByTagName("woot:price")[0].InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                if (doc.GetElementsByTagName("woot:wootoff")[0].InnerText.ToLower() == "true")
                {
                    wootOff = true;
                }
                else
                {
                    wootOff = false;
                }
                SetTimerInterval();

                // stdImageUrl = doc.GetElementById("woot:standardimage").InnerText;
                //purchaseUrl = itemProperties.Item(12).InnerText;
                //stdImageUrl = itemProperties.Item(15).InnerText;
                //soldPercent = decimal.Parse(itemProperties.Item(21).InnerText);
                //detailImageUrl = itemProperties.Item(16).InnerText;
            }
            catch (WebException)
            {
                if (wootOff)
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 30 sec.";
                }
                else
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 10 min.";
                }
            }
            catch (Exception e)
            {

            }
            finally { }
        }
        public void ShirtXml(bool callSetPrevWoot)
        {
            try
            {
                WebClient client = new WebClient();

                byte[] pageBytes = client.DownloadData(new Uri("http://shirt.woot.com/salerss.aspx"));



                string pageCode = Encoding.UTF8.GetString(pageBytes);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pageCode);
                XmlNode item = doc.SelectSingleNode("/rss/channel/item");
                title = item.SelectSingleNode("title").InnerText;
                if (callSetPrevWoot)
                {
                    previousWootTitle = title;
                }

                description = item.SelectSingleNode("description").InnerText;

                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                purchaseUrl = doc.GetElementsByTagName("woot:purchaseurl")[0].InnerText;
                stdImageUrl = doc.GetElementsByTagName("woot:substandardimage")[0].InnerText;
                soldPercent = decimal.Parse(doc.GetElementsByTagName("woot:soldoutpercentage")[0].InnerText);
                detailImageUrl = doc.GetElementsByTagName("woot:detailimage")[0].InnerText;
                price = doc.GetElementsByTagName("woot:price")[0].InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                if (doc.GetElementsByTagName("woot:wootoff")[0].InnerText.ToLower() == "true")
                {
                    wootOff = true;
                }
                else
                {
                    wootOff = false;
                }
                SetTimerInterval();
            }
            catch (WebException)
            {
                if (wootOff)
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 30 sec.";
                }
                else
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 10 min.";
                }
            }
            catch
            {

            }
            finally { }

        }
        public void WineXml(bool callSetPrevWoot)
        {
            try
            {
                WebClient client = new WebClient();

                byte[] pageBytes = client.DownloadData(new Uri("http://wine.woot.com/salerss.aspx"));



                string pageCode = Encoding.UTF8.GetString(pageBytes);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pageCode);
                XmlNode item = doc.SelectSingleNode("/rss/channel/item");
                title = item.SelectSingleNode("title").InnerText;
                if (callSetPrevWoot)
                {
                    previousWootTitle = title;
                }

                description = item.SelectSingleNode("description").InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;

                purchaseUrl = doc.GetElementsByTagName("woot:purchaseurl")[0].InnerText;
                stdImageUrl = doc.GetElementsByTagName("woot:substandardimage")[0].InnerText;
                soldPercent = decimal.Parse(doc.GetElementsByTagName("woot:soldoutpercentage")[0].InnerText);
                detailImageUrl = doc.GetElementsByTagName("woot:detailimage")[0].InnerText;
                price = doc.GetElementsByTagName("woot:price")[0].InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                if (doc.GetElementsByTagName("woot:wootoff")[0].InnerText.ToLower() == "true")
                {
                    wootOff = true;
                }
                else
                {
                    wootOff = false;
                }
                SetTimerInterval();
            }
            catch (WebException)
            {
                if (wootOff)
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 30 sec.";
                }
                else
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 10 min.";
                }
            }
            catch
            {

            }
            finally { }

        }
        public void KidsXml(bool callSetPrevWoot)
        {
            try
            {
                WebClient client = new WebClient();

                byte[] pageBytes = client.DownloadData(new Uri("http://kids.woot.com/salerss.aspx"));
                                string pageCode = Encoding.UTF8.GetString(pageBytes);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pageCode);
                XmlNode item = doc.SelectSingleNode("/rss/channel/item");
                title = item.SelectSingleNode("title").InnerText;
                if (callSetPrevWoot)
                {
                    previousWootTitle = title;
                }

                description = item.SelectSingleNode("description").InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                purchaseUrl = doc.GetElementsByTagName("woot:purchaseurl")[0].InnerText;
                stdImageUrl = doc.GetElementsByTagName("woot:substandardimage")[0].InnerText;
                soldPercent = decimal.Parse(doc.GetElementsByTagName("woot:soldoutpercentage")[0].InnerText);
                detailImageUrl = doc.GetElementsByTagName("woot:detailimage")[0].InnerText;
                price = doc.GetElementsByTagName("woot:price")[0].InnerText;
                if (doc.GetElementsByTagName("woot:soldout")[0].InnerText.ToLower() == "false")
                {
                    soldOut = false;
                }
                else soldOut = true;
                if (doc.GetElementsByTagName("woot:wootoff")[0].InnerText.ToLower() == "true")
                {
                    wootOff = true;
                }
                else
                {
                    wootOff = false;
                }
                SetTimerInterval();
            }
            catch (WebException)
            {
                if (wootOff)
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 30 sec.";
                }
                else
                {
                    productTitleBlock.Text = "Unable to Connect to Woot, We will try again in 10 min.";
                }
            }
            catch
            {

            }
            finally { }
        }
        #endregion

        private void loadPic()
        {
            // WebClient page = new WebClient();
            //page.DownloadFile(stdImageUrl, "pics0.jpg");
            //byte[] picArray = page.DownloadData(stdImageUrl);

            //productImage.Source = .FromStream(new MemoryStream(picArray));

            productImage.Source = new BitmapImage(new Uri(stdImageUrl));
        }

        private static System.Windows.Controls.Image loadPic(string ImageUrl)
        {
            System.Windows.Controls.Image tempImage = new System.Windows.Controls.Image();
            tempImage.Source = new BitmapImage(new Uri(ImageUrl));
            return tempImage;
        }
        private void ShowWindow(Window previewWindow)
        {
            previewWindow.Owner = this;
            previewWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            previewWindow.Show();
            previewWindow.Left = SystemParameters.WorkArea.Width - this.ActualWidth - previewWindow.ActualWidth;
            previewWindow.Top = SystemParameters.WorkArea.Height - previewWindow.ActualHeight;
            previewWindow.Closed += new EventHandler(previewWindow_Closed);

            switch (currentColorScheme)
            {
                case ColorScheme.Woot:
                    previewWindow.BorderBrush = wootTheme.BorderColor;
                    break;
                case ColorScheme.Wine:
                    previewWindow.BorderBrush = wineTheme.BorderColor;
                    break;
                case ColorScheme.Shirt:
                    previewWindow.BorderBrush = shirtTheme.BorderColor;
                    break;
                case ColorScheme.Kids:
                    previewWindow.BorderBrush = KidsTheme.BorderColor;
                    break;
            }


        }


        //Events -----------------------------------------------------------<>
        #region Events
        private void MainWindow_StateChanged(object sender, System.EventArgs e)
        {

            if (this.WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void MyNotifyIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            Show();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Show();
            this.Activate();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void wantOneImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(purchaseUrl);

        }

        private void bulbImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (currentColorScheme)
            {
                case ColorScheme.Woot:
                    bulbImage.Source = new BitmapImage(wootTheme.ChangeBulb(windowIsUp));
                    break;
                case ColorScheme.Shirt:
                    bulbImage.Source = new BitmapImage(shirtTheme.ChangeBulb(windowIsUp));
                    break;
                case ColorScheme.Wine:
                    bulbImage.Source = new BitmapImage(wineTheme.ChangeBulb(windowIsUp));
                    break;
                case ColorScheme.Kids:
                    bulbImage.Source = new BitmapImage(KidsTheme.ChangeBulb(windowIsUp));
                    break;
            }
            if (windowIsUp)
            {

                grid1.Height = 329;




                webBrowser1.Visibility = System.Windows.Visibility.Hidden;
                this.Top = SystemParameters.WorkArea.Height - (grid1.Height + (this.BorderThickness.Top * 2));
                windowIsUp = false;
            }
            else
            {

                grid1.Height = 700;

                this.InvalidateMeasure();
                this.Top = SystemParameters.WorkArea.Height - (grid1.Height + (this.BorderThickness.Top * 2));
                webBrowser1.Height = 378;
                webBrowser1.Visibility = System.Windows.Visibility.Visible;

                windowIsUp = true;
            }
        }

        private void wantOneImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            wantOneImage.Source = new BitmapImage(new Uri("pack://application:,,,/WootAlert;component/Themes/Common/wantone2.png"));
        }

        private void wantOneImage_MouseLeave(object sender, MouseEventArgs e)
        {
            wantOneImage.Source = new BitmapImage(new Uri("pack://application:,,,/WootAlert;component/Themes/Common/wantone.png"));
        }

        private void shirtWootImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShirtXml(true);
            Themer(ColorScheme.Shirt);
            Update();

            if (ImageWindow != null)
            {
                ImageWindow.Close();
            }

        }

        private void wineWootImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WineXml(true);
            Themer(ColorScheme.Wine);
            Update();

            if (ImageWindow != null)
            {
                ImageWindow.Close();
            }
        }

        private void kidsWootImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            KidsXml(true);
            Themer(ColorScheme.Kids); ;
            Update();

            if (ImageWindow != null)
            {
                ImageWindow.Close();
            }
        }

        private void WootLogo_MouseUp(object sender, MouseButtonEventArgs e)
        {

            WootXml(true);
            Themer(ColorScheme.Woot);
            Update();

            if (ImageWindow != null)
            {
                ImageWindow.Close();
            }

        }

        private void minimizeImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
        // Event to show larger image
        private void productImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsPicWindowOpen == false)
            {
                ImageWindow = new Window1(loadPic(detailImageUrl));
                ShowWindow(ImageWindow);
                IsPicWindowOpen = true;
                // productImage.Width = 400;
                // productImage.Height = 300;
            }
        }
        // Lets the program know the preview window is closed
        void previewWindow_Closed(object sender, EventArgs e)
        {
            IsPicWindowOpen = false;
        }

        private void MenuItem_Click_Restore(object sender, RoutedEventArgs e)
        {
            settings.CommitSettings();
            MyNotifyIcon.Dispose();
            Environment.Exit(0);
        }
        private void MenuItem_Click_Settings(object sender, RoutedEventArgs e)
        {
            SettingsWindow set = new SettingsWindow(settings.PlayAlertSound, settings.WindowsStart, settings.WootRefresh, settings.WootOffRefresh);
            set.Owner = this;

            set.ShowDialog();
            if (set.DialogResult == true)
            {
                settings.PlayAlertSound = set.audioAlerts;
                settings.WindowsStart = set.checkBox2.IsChecked.Value;
                settings.WootOffRefresh = (int)set.slider2.Value;
                settings.WootRefresh = (int)set.slider1.Value;
                SetTimerInterval();//to reset the timer in case you change it in settings
                settings.CommitSettings();
            }
            else
            { }


        }
        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("woot.Alert! is developed and maintained by Jesse Shaw.\r\nVersion = " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\r\nwoot.Alert! is not affiliated with woot.com or its partners.");
        }
        #endregion


    }


    public enum ColorScheme
    {
        Woot,
        Shirt,
        Kids,
        Wine
    }

    public class Theme
    {

        private string bckColor;
        private string webBckColor;
        private string borderColor;
        private string scrlBarColor;
        private Uri expandBulb;
        private Uri contractBulb;
        private Uri minimizeIcon;
        /// <param name="bdrColor">Border Color</param>
        /// /// <param name="webBackColor">This is the color to splice into the woot html style</param>
        /// <param name="scrollColor">Scroll Bar Color</param>
        /// <param name="imagesFolder">Folder where this themes images are located</param>
        public Theme(string backColor, string webBackColor, string bdrColor, string scrollColor, string imagesFolder)
        {
            bckColor = backColor;
            webBckColor = webBackColor;
            borderColor = bdrColor;
            scrlBarColor = scrollColor;
            expandBulb = new Uri("pack://application:,,,/WootAlert;component/Themes/" + imagesFolder + "/expand.png");
            contractBulb = new Uri("pack://application:,,,/WootAlert;component/Themes/" + imagesFolder + "/contract.png");
            minimizeIcon = new Uri("pack://application:,,,/WootAlert;component/Themes/" + imagesFolder + "/minimize.png");
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(bckColor));
            }
        }
        public string WebBackgroundColor
        {
            get
            {
                return webBckColor;
            }
        }
        public SolidColorBrush BorderColor
        {
            get
            {
                return new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(borderColor));
            }
        }
        public string ScrollBarColor
        {
            get
            {
                return scrlBarColor;
            }
        }
        public Uri ChangeBulb(bool windowIsUp)
        {
            if (windowIsUp)
            {
                return expandBulb;
            }
            else
            {
                return contractBulb;
            }
        }
        public Uri MinimizeUri
        {
            get
            {
                return minimizeIcon;
            }

        }
        public Uri ExpandBulb
        {
            get
            {
                return expandBulb;
            }
        }
        public Uri ContractBulb
        {
            get
            {
                return contractBulb;
            }
        }
    }

    public class Settings
    {

        private bool playAlertSound;
        private bool windowsStart;
        private int wootRefresh = 0;
        private int wootOffRefresh = 0;
        string appDataString = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WootAlert";
        public Settings()
        {
            if (System.IO.Directory.Exists(appDataString) == false && System.IO.File.Exists(appDataString + @"\settings.xml") == false)
            {
                System.IO.Directory.CreateDirectory(appDataString);
                StreamWriter xmlfile = new StreamWriter(appDataString + @"\settings.xml", false, Encoding.UTF8);
                xmlfile.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                    <settings>
                                        <audioalert>False</audioalert>
                                        <windowsStart>False</windowsStart>
                                        <wootOffRefresh>30</wootOffRefresh>
                                        <wootRefresh>10</wootRefresh>
                                    </settings>");
                xmlfile.Close();
                LoadXML();
            }
            else { LoadXML(); }
        }

        private void LoadXML()
        {
            XmlDocument settingsDoc = new XmlDocument();
            settingsDoc.Load(appDataString + @"\settings.xml");
            XmlNodeList settingsNodes = settingsDoc.GetElementsByTagName("settings");
            settingsNodes = settingsNodes.Item(0).ChildNodes;
            if (settingsNodes.Item(0).InnerText == "False")
            { playAlertSound = false; }
            else { playAlertSound = true; }
            if (settingsNodes.Item(1).InnerText == "False")
            { windowsStart = false; }
            else { windowsStart = true; }
            wootOffRefresh = int.Parse(settingsNodes.Item(2).InnerText);
            wootRefresh = int.Parse(settingsNodes.Item(3).InnerText);
        }
        public bool PlayAlertSound
        {
            get
            {
                return playAlertSound;
            }
            set
            {
                playAlertSound = value;
            }
        }
        public int WootRefresh
        {
            get
            {
                return wootRefresh;
            }
            set
            {
                wootRefresh = value;
            }

        }
        public int WootOffRefresh
        {
            get
            {
                return wootOffRefresh;
            }
            set
            {
                wootOffRefresh = value;
            }
        }
        public bool WindowsStart
        {
            get
            {
                return windowsStart;
            }
            set
            {
                windowsStart = value;
            }
        }

        public void CommitSettings()
        {

            XmlDocument settingsDoc = new XmlDocument();
            settingsDoc.Load(appDataString + @"\settings.xml");
            XmlNodeList settingsNodes = settingsDoc.GetElementsByTagName("settings");
            settingsNodes = settingsNodes.Item(0).ChildNodes;
            settingsNodes.Item(0).InnerText = playAlertSound.ToString();
            settingsNodes.Item(1).InnerText = windowsStart.ToString();
            settingsNodes.Item(2).InnerText = wootOffRefresh.ToString();
            settingsNodes.Item(3).InnerText = wootRefresh.ToString();
            settingsDoc.Save(appDataString + @"\settings.xml");



            //IWshRuntimeLibrary.IWshShortcut MyShortcut;
            //// Choose the path for the shortcut
            //MyShortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(@"C:\MyShortcut.lnk");
            //// Where the shortcut should point to
            //MyShortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //// Description for the shortcut
            //MyShortcut.Description = "Launch My Application";
            //// Location for the shortcut's icon
            //MyShortcut.IconLocation = System.Reflection.Assembly.GetExecutingAssembly().Location + @"\woot.ico";
            //// Create the shortcut at the given path
            //MyShortcut.Save();


            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("WootApp") == null && windowsStart == true)
            {
                rkApp.SetValue("WootApp", System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            if (rkApp.GetValue("WootApp") != null && windowsStart == false)
            {
                rkApp.DeleteValue("WootApp");
            }


        }


    }
}
