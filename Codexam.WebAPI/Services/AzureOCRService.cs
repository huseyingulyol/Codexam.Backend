using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Codexam.WebAPI.Services
{


    public class AzureOcrService
    {
        public string subscriptionKey = "<SubscriptionKey>";
        public string endpoint = "<SubscriptionKey>";

        // localImagePath = @"C:\Documents\LocalImage.jpg"
        private const string localImagePath = @"<LocalImage>";

        // Specify the features to return
        public List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        public AzureOcrService(IConfiguration configuration)
        {
            subscriptionKey = configuration["Azure:SubscriptionKey"];
            endpoint = configuration["Azure:Endpoint"];
        }

        public async Task<string> PerformOcrAsync(string imagePath)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
              new ApiKeyServiceClientCredentials("subscriptionKey"),
              new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://api.cognitive.azure.cn";

            Console.WriteLine("Images being analyzed ...");
            await AnalyzeLocalAsync(computerVision, "localImagePath");

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
            return "";
        }


        public async Task AnalyzeLocalAsync(
            ComputerVisionClient computerVision, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, (IList<VisualFeatureTypes?>)features);
                DisplayResults(analysis, imagePath);
            }
        }

        // Display the most relevant caption for the image
        public void DisplayResults(ImageAnalysis analysis, string imageUri)
        {
            Console.WriteLine(imageUri);
            if (analysis.Description.Captions.Count != 0)
            {
                Console.WriteLine(analysis.Description.Captions[0].Text + "\n");
            }
            else
            {
                Console.WriteLine("No description generated.");
            }
        }
    }
}
