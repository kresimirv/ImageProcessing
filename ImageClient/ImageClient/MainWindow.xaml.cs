using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace ImageClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region "Data members"

        private TcpClientHandler client = null; //communication object
        private Point mouse_offset; //for mouse tracking

        private System.Drawing.Image currInputImage = null; //used for holding input image
        private System.Drawing.Image currOutputImage = null; //used for holding output image
        private Uri noInputImageUri = null; //uri of no input image
        private Uri noOutputImageUri = null; //uri of no output image


        #endregion

        #region "Main events"

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //initial images
                currInputImage = null;
                currOutputImage = null;
                noInputImageUri = new Uri(@"/ImageClient;component/Images/drag_drop.png", UriKind.Relative);
                noOutputImageUri = new Uri(@"/ImageClient;component/Images/convert.png", UriKind.Relative);
                
                picInput.Source = new BitmapImage(noInputImageUri); //set to no image picture
                picOutput.Source = new BitmapImage(noOutputImageUri);  //set to no image picture
                
                //read settings from config file
                string ip = Properties.Settings.Default.ServerIP; //ip address
                int port = Properties.Settings.Default.ServerPort; //port

                //create tcp client object
                client = new TcpClientHandler(ip, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message));
            }
        }

        private void chkRotate_Checked(object sender, RoutedEventArgs e)
        {
            //enable rotate options
            rboRotate90.IsEnabled = true;
            rboRotate180.IsEnabled = true;
            rboRotate270.IsEnabled = true;
        }

        private void chkRotate_Unchecked(object sender, RoutedEventArgs e)
        {
            //disable rotate options
            rboRotate90.IsEnabled = false;
            rboRotate180.IsEnabled = false;
            rboRotate270.IsEnabled = false;
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (currInputImage == null) //no image selected
            {
                MessageBox.Show("Select the image first!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Cursor = Cursors.Wait; //change cursor

            currOutputImage = null; //clear output image

            client.Connect(); //connect to server

            //check if connection is active
            if (client.IsConnected == true)
            {
                //create xml message
                string message = CreateXMLMessage();
                if (message == string.Empty)
                {
                    return;
                }

                //send message to server and get server response
                string returnImageBase64 = client.SendMessageAndReceive(message);
                System.Drawing.Image imgRet = ImageEncoder.Base64ToImage(returnImageBase64);
                if (imgRet != null)
                {
                    picOutput.Source = ImageEncoder.ToBitmapImage(imgRet);
                    currOutputImage = imgRet;
                }

                client.Disconnect(); //close server connection

                this.Cursor = Cursors.Arrow; //change cursor
            }
            else
            {
                this.Cursor = Cursors.Arrow; //change cursor

                //alert user
                MessageBox.Show("Unable to connect to image server!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (currOutputImage == null) //if no image to save
            {
                MessageBox.Show("No output image to save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //show save file dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Image (.png)|.png|Jpeg Image (.jpeg)|.jpeg|Gif Image (.gif)|.gif|Bitmap Image (.bmp)|.bmp";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == true) //if save selected
            {
                string extension = System.IO.Path.GetExtension(sfd.FileName); //get file extension
                switch (extension)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".jpeg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        format = ImageFormat.Gif;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                currOutputImage.Save(sfd.FileName, format); //save image
            }
        }

        #endregion

        #region "Drag & Drop events"

        private void Window_Drop(object sender, DragEventArgs e)
        {
            //get file draged to window
            string[] fileList = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileList.Length == 1) //accept only one file
            {
                //display image file in input
                string file = fileList[0];
                System.Drawing.Image im = null;
                try
                {
                    im = System.Drawing.Image.FromFile(file);
                }
                catch
                { }

                if (im != null) //if valid image
                {
                    //clear images from controls
                    picInput.Source = null;
                    picOutput.Source = null;

                    //set images
                    currInputImage = im;
                    currOutputImage = null;

                    //set current images
                    picInput.Source = ImageEncoder.ToBitmapImage(im); //set it as input image
                    picOutput.Source = new BitmapImage(noOutputImageUri);  //set to no image picture
                }
                else //invalid image
                {
                    //clear images
                    picInput.Source = null;
                    picOutput.Source = null;

                    //set images
                    currInputImage = null;
                    currOutputImage = null;

                    //set current images
                    picInput.Source = new BitmapImage(noInputImageUri); //set to no image picture
                    picOutput.Source = new BitmapImage(noOutputImageUri);  //set to no image picture
                }
            }
        }

        #endregion

        #region "Move, minimize & maximize events"

        private void btnMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //minimize window
        }

        private void btnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close(); //close application
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); //move window
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Create XML message for server
        /// </summary>
        /// <returns>Returns XML message as string</returns>
        private string CreateXMLMessage()
        {
            string result = string.Empty;

            //encode image to base 64
            System.Drawing.Image img = currInputImage;
            string img64 = ImageEncoder.ImageToBase64(img, System.Drawing.Imaging.ImageFormat.Jpeg);

            //create message
            string grayscaleChecked = "0";
            if (chkGrayscale.IsChecked == true)
            {
                grayscaleChecked = "1";
            }

            string flipChecked = "0";
            if (chkFlip.IsChecked == true)
            {
                flipChecked = "1";
            }

            string rotateChecked = "0";
            if (chkRotate.IsChecked == true)
            {
                if (rboRotate90.IsChecked == true)
                {
                    rotateChecked = "90";
                }
                if (rboRotate180.IsChecked == true)
                {
                    rotateChecked = "180";
                }
                if (rboRotate270.IsChecked == true)
                {
                    rotateChecked = "270";
                }
            }

            //create xml message
            result = XmlCreator.MakeXMLMessage(grayscaleChecked, flipChecked, rotateChecked, img64);

            return result;
        }

        #endregion

    }
}
