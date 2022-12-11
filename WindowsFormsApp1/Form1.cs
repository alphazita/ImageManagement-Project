using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        
        private void button1_Click(object sender, EventArgs e)
        {

            var listImages = Directory.GetFiles(@"C:\\Users\\Anastasiaa\\Desktop\\GenikiTaxidromiki\\", "*.jpg");
            List<string> savedImages = new List<string>();
            //check if we have empty folder
            if (listImages.Length == 0)
            {
                List<string> imageUrls = new List<string>
                {
                "https://www.taxydromiki.com/sites/default/files/images/20160323_pireas.jpg",
                "https://www.taxydromiki.com/sites/default/files/styles/flexslider_full/public/hq_gt.jpg",
                "https://www.taxydromiki.com/sites/default/files/images/eblokotel2_0.jpg",
                "https://www.taxydromiki.com/sites/default/files/styles/flexslider_full/public/slide-2_0.jpg",
                "https://www.taxydromiki.com/sites/default/files/styles/flexslider_full/public/slide-1.jpg",
                "https://www.taxydromiki.com/sites/default/files/styles/flexslider_full/public/slide-3.jpg"
                };

                

                // Create an instance of the WebClient class

                WebClient webClient = new WebClient();
                int classLevelCounter = 0;


                //to download the images in parallel to save time 
                foreach (string url in imageUrls)
                //Parallel.ForEach(imageUrls, url =>
                {
                    try
                    {
                        // Download the image from the URL
                        byte[] imageBytes = webClient.DownloadData(url);
                        // Save the image as a JPG
                        using (var stream = new MemoryStream(imageBytes))
                        using (var image = Image.FromStream(stream))
                        {
                            image.Save("C:\\Users\\Anastasiaa\\Desktop\\GenikiTaxidromiki\\" + Path.GetFileNameWithoutExtension(url) + ".jpg", ImageFormat.Jpeg);
                            savedImages.Add("C:\\Users\\Anastasiaa\\Desktop\\GenikiTaxidromiki\\" + Path.GetFileNameWithoutExtension(url) + ".jpg");
                        }

                        // Set up the download progress event handler
                        webClient.DownloadProgressChanged += (webclient, progressEventArgs) =>
                        {
                            // Update the progress bar with the download progress
                            progressBar1.Value = progressEventArgs.ProgressPercentage;
                        };
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occurred while downloading the image
                        Console.WriteLine("Error downloading image from URL: " + url);
                        Console.WriteLine(ex.Message);
                        System.Windows.Forms.Application.Exit();
                    }
                }//);
            }

            //if is empty upload the saved images
            if (savedImages.Count == 0)
            {
                foreach (string file in Directory.EnumerateFiles("C:\\Users\\Anastasiaa\\Desktop\\GenikiTaxidromiki\\", "*.jpg"))
                {
                    savedImages.Add(file);
                }
            }

            //Disable the button if all images are downloaded
            if (savedImages.Count==6)
            {
                buttonstart.Enabled = false;
            }

            int thumbWidth = 200;
            int thumbHeight = 200;
            //Create the thumbnails 
            Parallel.ForEach(savedImages, image =>
            {
                try
                {
                    Image ima = new Bitmap(image);
                    Image imageThumbnail = ima.GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr());

                    System.Windows.Forms.Button thumbnailButton = new System.Windows.Forms.Button();
                    thumbnailButton.Image = imageThumbnail;

                    // When the user clicks on the thumbnail button, open the full-size image in a scrollable PictureBox
                    thumbnailButton.Click += (sr, er) =>
                    {
                        PictureBox fullSizeImage = new PictureBox();
                        fullSizeImage.Image = ima;
                        fullSizeImage.SizeMode = PictureBoxSizeMode.AutoSize;
                        fullSizeImage.Show();
                    };


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });

        }
        //delete all files from folder
        private void delete_Click_1(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo("C:\\Users\\Anastasiaa\\Desktop\\GenikiTaxidromiki\\");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
