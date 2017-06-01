using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tests.CognitiveServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path to the JPEG image with faces to identify:");
            // var imageFilePath = Console.ReadLine();

            var imageFilePath = @"d:/01.jpg";
            // var pSavedPath = @"E:\10001003\";

            MakeDetectRequest(imageFilePath);

            /*
            var bmp = new Bitmap(Image.FromFile(imageFilePath));

            var rectangleImg = DrawRectangleInPicture(bmp, 56, 246, 46, 46);
            rectangleImg.Save(pSavedPath + "\\rectangle.jpg", ImageFormat.Jpeg);
            */

            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }

        static Bitmap DrawRectangleInPicture(Bitmap bmp, int x, int y, int width, int height)
        {
            if (bmp == null) return null;

            var g = Graphics.FromImage(bmp);

            g.DrawRectangle(Pens.Red, new Rectangle(x, y, width, height));

            g.Dispose();

            return bmp;
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async void MakeDetectRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f56a2c716db64959b90ff0bfa3e38901");

            // Request parameters.
            var queryString = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";

            // NOTE: You must use the same region in your REST call as you used to obtain your subscription keys.
            //   For example, if you obtained your subscription keys from westus, replace "westcentralus" in the 
            //   URI below with "westus".
            var uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            var byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            //A peak at the JSON response.
            Console.WriteLine(responseContent);
        }

    }
}



/**
 * [
    {
        "faceId": "06827541-7afe-4084-b2ee-86e92607dfb7",
        "faceRectangle": {
            "top": 56,
            "left": 246,
            "width": 46,
            "height": 46
        },
        "faceAttributes": {
            "gender": "male",
            "age": 39.2
        }
    }
]
    * https://my.oschina.net/gsbhz/blog/496203?p={{currentPage-1}}
    * https://github.com/Microsoft/ProjectOxford-clientsdk
    * 
    # 人脸识别(Face APIs)介绍
    * 1）人脸检测:检测图片中的人脸，以方框标记脸部位置，识别包括人脸特征点、姿势、性别、年龄等在内的人脸属性。
    * Face API 提供高精度的人脸定位检测，在一张图片里，最多可以检测到64张人脸。 
    * 进行人脸检测，只需上传一整张JPEG图片，或提供网页JPEG图片对应的URL即可。
    * 检测到的人脸会被标记上方框（左侧、顶部、宽度和高度），用像素点标明脸部在图片中的位置。 
    * 根据需要，人脸检测还可以从每张人脸上提取诸如姿势、性别、年龄等一系列与脸部有关的属性。
    * 
    * 
    * https://github.com/microsoft/cognitive-Face-Windows
    * https://github.com/microsoft/cognitive-Face-iOS
    * 
    * https://github.com/Azure-Samples/Cognitive-Speech-STT-iOS
    * 
    * https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp#Detect
    * https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236
    * 
    * https://azure.microsoft.com/zh-cn/try/cognitive-services/my-apis/
    


 */
