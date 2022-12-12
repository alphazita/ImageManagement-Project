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
using Aspose.Slides;
using Microsoft.SharePoint.Client;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using Image = System.Drawing.Image;

namespace WindowsFormsApp1
{
    public partial class Form1 : System.Windows.Forms.Form
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
                        string name = Path.GetFileNameWithoutExtension(url) + ".jpg";
                        if (!listImages.Contains(name)){
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
                        
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occurred while downloading the image
                        Console.WriteLine("Error downloading image from URL: " + url);
                        Console.WriteLine(ex.Message);
                        //System.Windows.Forms.Application.Exit();
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
            //if (savedImages.Count==6)
            //{
                buttonstart.Enabled = false;
            //}

            int thumbWidth = 130;
            int thumbHeight = 130;
            int i = 0;
            //Create the thumbnails 
            foreach (string im  in savedImages)
            //Parallel.ForEach(savedImages, image =>
            {
                try
                {
                    if(savedImages.First() == im)
                        i = i + 80;
                    else
                        i = i + 140;
                    Image ima = new Bitmap(im);
                    Image imageThumbnail = ima.GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr());
                    //Placeholder ph = new Placeholder();
                    System.Windows.Forms.Button thumbnailButton = new System.Windows.Forms.Button();
                    thumbnailButton.Image = imageThumbnail;
                    thumbnailButton.Name = im;
                    thumbnailButton.AutoSize = true;
                    thumbnailButton.Location = new System.Drawing.Point(i,120);
                    thumbnailButton.Size = new Size(130, 130);
                    
                    //thumbnailButton.Margin = new Padding(150, 0, 0, 0);
                    

                    // When the user clicks on the thumbnail button, open the full-size image in a scrollable PictureBox
                    thumbnailButton.Click += (sr, er) =>
                    {
                        Panel MyPanel = new Panel();
                        MyPanel.Dock = DockStyle.Fill;
                        PictureBox fullSizeImage = new PictureBox();
                        fullSizeImage.Image = ima;
                        fullSizeImage.SizeMode = PictureBoxSizeMode.AutoSize;
                        MyPanel.Controls.Add(fullSizeImage);
                        MyPanel.AutoScroll = true;
                        this.Controls.Add(MyPanel);
                        fullSizeImage.Show();
                        
                    };
                    this.Controls.Add(thumbnailButton);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

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
