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
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net.Http;
using RestSharp;
using ZeroGravity.Shared.Constants;
using System.Diagnostics;
using ZeroGravity.SugarBeat.Algorithms.Models;
using ZeroGravity.SugarBeat.Algorithms;

namespace FileUploadUtility
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

        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=miboko;AccountKey=eH6P6eTfXGmkDl/ESsDo7Eymsebx2sXP58K4CTgN37owAeYVfkbqcQmx6LQQwKD/OJ12Zc4rdJhe+V1svTWfaA==;EndpointSuffix=core.windows.net";
        private static string container = "educational-images";

        private static string apiUrl = "https://miboko-api.azurewebsites.net";
        private static string storageUrl = "https://miboko.blob.core.windows.net";
        private static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEwMDEiLCJuYmYiOjE2MzAzMDU2MjUsImV4cCI6MTYzMDM5MjAyNSwiaWF0IjoxNjMwMzA1NjI1LCJpc3MiOiJNaUJvS28ifQ.A4flWgS-Ks_Oo91cHnqMuv6krXa3hfH8N5iokdswlOc";

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, container);
            blobContainerClient.CreateIfNotExists();
            Console.WriteLine("Listing blobs...");
            // List all blobs in the container
            var blobs = blobContainerClient.GetBlobs();
            foreach (BlobItem blobItem in blobs)
            {
                var res = blobItem.Name.Split("/");
                if (res.Length > 1)
                {
                    var flname = res[0].ToLower();
                    var fn = res[1];
                    switch (flname)
                    {
                        case StorageFolderConstants.Activities:
                            await CreateDataModel(StorageFolderConstants.Activities, fn);
                            break;

                        case StorageFolderConstants.Breakfast:
                            await CreateDataModel(StorageFolderConstants.Breakfast, fn);
                            break;

                        case StorageFolderConstants.CalorieAndDrink:
                            await CreateDataModel(StorageFolderConstants.CalorieAndDrink, fn);
                            break;

                        case StorageFolderConstants.Dinner:
                            await CreateDataModel(StorageFolderConstants.Dinner, fn);
                            break;

                        case StorageFolderConstants.Fasting:
                            await CreateDataModel(StorageFolderConstants.Fasting, fn);
                            break;

                        case StorageFolderConstants.HealthySancks:
                            await CreateDataModel(StorageFolderConstants.HealthySancks, fn);
                            break;

                        case StorageFolderConstants.Lunch:
                            await CreateDataModel(StorageFolderConstants.Lunch, fn);
                            break;

                        case StorageFolderConstants.Meditation:
                            await CreateDataModel(StorageFolderConstants.Meditation, fn);
                            break;

                        case StorageFolderConstants.Sleep:
                            await CreateDataModel(StorageFolderConstants.Sleep, fn);
                            break;

                        case StorageFolderConstants.Stress:
                            await CreateDataModel(StorageFolderConstants.Stress, fn);
                            break;

                        case StorageFolderConstants.UnHealthySancks:
                            await CreateDataModel(StorageFolderConstants.UnHealthySancks, fn);
                            break;

                        case StorageFolderConstants.Water:
                            await CreateDataModel(StorageFolderConstants.Water, fn);
                            break;

                        case StorageFolderConstants.Wellbeing:
                            await CreateDataModel(StorageFolderConstants.Wellbeing, fn);
                            break;

                        default:
                            await CreateDataModel(flname, fn);
                            break;
                    }
                }
            }
        }

        private Task CreateDataModel(string flname, string fn)
        {
            try
            {
                var client = new RestClient(apiUrl);
                var request = new RestRequest("EducationalInfo/create");
                request.AddHeader("Authorization", "Bearer " + token);

                var ed = new EducationalInfo()
                {
                    category = flname,
                    tittle = System.IO.Path.GetFileNameWithoutExtension(fn),
                    imageUrl = System.IO.Path.Combine(storageUrl, container, flname, fn)
                };
                ed.imageUrl = ed.imageUrl.Replace("\\", "/");
                request.AddJsonBody(ed);
                var response2 = client.Post<EducationalInfo>(request);
                Debug.WriteLine(response2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var inputs = new List<GlucoseAlgorithmInputEntry>() {
                         new GlucoseAlgorithmInputEntry(1, 130980.1016,0),
                         new GlucoseAlgorithmInputEntry(2, 77269.70313,0),
                         new GlucoseAlgorithmInputEntry(3, 58352.01172, 0),
                         new GlucoseAlgorithmInputEntry(4, 47778.48438, 0),
                         new GlucoseAlgorithmInputEntry(5, 39343.16797, 0),
                         new GlucoseAlgorithmInputEntry(6, 34337.42188, 0),
                         new GlucoseAlgorithmInputEntry(7, 30075.78125, 0),
                         new GlucoseAlgorithmInputEntry(8, 26501.76563, 0),
                         new GlucoseAlgorithmInputEntry(9, 23833.0957, 0),
                         new GlucoseAlgorithmInputEntry(10, 21557.93359, 0),
                         new GlucoseAlgorithmInputEntry(11, 19621.44531, 0),
                         new GlucoseAlgorithmInputEntry(12, 19029.24609, 0),
                         new GlucoseAlgorithmInputEntry(13, 17837.87695, 0),
                         new GlucoseAlgorithmInputEntry(14, 17399.05469, 0),
                         new GlucoseAlgorithmInputEntry(15, 16607.05859, 0),
                         new GlucoseAlgorithmInputEntry(16, 15858.2002, 0),
                         new GlucoseAlgorithmInputEntry(17, 15820.49707, 0),
                         new GlucoseAlgorithmInputEntry(18, 15403.04395, 0),
                         new GlucoseAlgorithmInputEntry(19, 14842.40625, 0)
            };

            var alg = new GlucoseAlgorithm();
            var output = alg.Run(inputs, null, 17.88343945, 3);
            foreach(var entry in output.Value.glucoseAlgorithmOutputEntries)
            {
                Console.WriteLine(entry.GlucoseValues);
            }
        }
    }

    public class EducationalInfo
    {
        public int id { get; set; }
        public string category { get; set; }
        public string imageUrl { get; set; }
        public string tittle { get; set; }
        public string description { get; set; }
    }
}