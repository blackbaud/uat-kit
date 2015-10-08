using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using TechTalk.SpecFlow; 

namespace Blackbaud.UAT.SpecFlow.Selenium
{

    // http://www.briangrinstead.com/blog/multipart-form-post-in-c

    // Implements multipart/form-data POST in C# http://www.ietf.org/rfc/rfc2388.txt    
    public static class WebPageTest
    {
        public static object PostResults(string uri, string testId, string Credentials)
        {
            byte[] compressed;

            // Targeting the 4.0 .NET framework means that System.IO.Compression.ZipArchive is not available !!
            // Therefore this uses the 3rd part 'SharpZipLib' nuget package. 
            // When/if the agents are upgraded to 4.5 whe can uncomment and refactor the code below to use ZipArchive.

            //// Read results file data
            //FileStream fs =
            //    new FileStream(
            //        //"1_devtools.json",
            //        quick + "_perf.log",
            //        FileMode.Open, FileAccess.Read);
            //byte[] resultsData = new byte[fs.Length];
            //fs.Read(resultsData, 0, resultsData.Length);
            //fs.Close();

            //// Read picture file data
            //fs =
            //    new FileStream(
            //        //"1_screen.png",
            //        quick + ".png",
            //        FileMode.Open, FileAccess.Read);
            //byte[] picData = new byte[fs.Length];
            //fs.Read(picData, 0, picData.Length);
            //fs.Close();

            //using (var memoryStream = new MemoryStream())
            //{
            //    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            //    {
            //        var jsonFile = archive.CreateEntry("1_devtools.json");

            //        using (var entryStream = jsonFile.Open())
            //        using (var binaryWriter = new BinaryWriter(entryStream))
            //        //using (var streamWriter = new StreamWriter(entryStream))
            //        {
            //            binaryWriter.Write(resultsData);
            //            //streamWriter.Write("Bar!");
            //        }

            //        var picFile = archive.CreateEntry("1_screen.png");

            //        using (var entryStream = picFile.Open())
            //        using (var binaryWriter = new BinaryWriter(entryStream))
            //        //using (var streamWriter = new StreamWriter(entryStream))
            //        {
            //            binaryWriter.Write(picData);
            //            //streamWriter.Write("Bar!");
            //        }
            //    }

            //    using (var fileStream = new FileStream("test.zip", FileMode.Create))
            //    {
            //        memoryStream.Seek(0, SeekOrigin.Begin);
            //        memoryStream.CopyTo(fileStream);

            //        memoryStream.Seek(0, SeekOrigin.Begin);
            //        compressed = memoryStream.ToArray();
            //    }
            //}

            //Console.WriteLine("compressed len is {0}", compressed.Length);

            //https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#anchorMemory                

            using (MemoryStream outputMemStream = new MemoryStream())
            {

                using (ZipOutputStream zipStream = new ZipOutputStream(outputMemStream))
                {

                    zipStream.SetLevel(6); //0-9, 9 being the highest level of compression
                    using (
                        FileStream resStream =
                            File.OpenRead(
                                ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_") +
                                "_perf.log"))
                    {
                        ZipEntry resultsEntry = new ZipEntry("1_devtools.json") { Size = resStream.Length };
                        // Size is the magic incantation for the WebPageTest php zip library compatability !!!!
                        zipStream.PutNextEntry(resultsEntry);
                        StreamUtils.Copy(resStream, zipStream, new byte[4096]);
                        zipStream.CloseEntry();
                    }

                    using (
                        FileStream picStream =
                            File.OpenRead(
                                ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", "_") + ".png")
                        )
                    {
                        ZipEntry picEntry = new ZipEntry("1_screen.png") { Size = picStream.Length };
                        zipStream.PutNextEntry(picEntry);
                        StreamUtils.Copy(picStream, zipStream, new byte[4096]);
                        zipStream.CloseEntry();
                    }
                    zipStream.IsStreamOwner = false; // False stops the Close also Closing the underlying stream.
                }

                compressed = outputMemStream.ToArray();

                //using (var fileStream = new FileStream("test2.zip", FileMode.Create))
                //{
                //    outputMemStream.Seek(0, SeekOrigin.Begin);
                //    outputMemStream.CopyTo(fileStream);                                
                //}
            }
            //Console.WriteLine("compressed len is {0}", compressed.Length);

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>
            {
                {"location", "Test"},
                {"id", testId},
                {"done", "1"},
                {"file", new FileParameter(compressed, "1_results.zip", "application/zip")}
            };

            // Create request and receive response            
            HttpWebResponse webResponse = MultipartFormDataPost(uri, "Bluetest", postParameters, Credentials);

            string fullResponse;
            // Process response
            if (webResponse == null) return null;
            using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
            {
                fullResponse = responseReader.ReadToEnd();
            }
            return fullResponse;
        }

        private static readonly Encoding Encoding = Encoding.UTF8;
        private static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string Credentials)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData, Credentials);
        }

        public static string CreateTest(string uri, string Credentials)
        {

            //http://stackoverflow.com/questions/526711/using-a-self-signed-certificate-with-nets-httpwebrequest-response                                    
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            if (Credentials != null)
            {
                //request.PreAuthenticate = true;
                //request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                request.Headers.Add("Authorization",
                    "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Credentials)));
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("CreateTest response Status Code " + response.StatusCode);
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string Credentials)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.ServicePoint.Expect100Continue = false;
            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.SendChunked = true;

            if (Credentials != null)
            {
                //request.PreAuthenticate = true;
                //request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                request.Headers.Add("Authorization",
                    "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Credentials)));
            }

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new MemoryStream();
            bool needsClrf = false;

            foreach (KeyValuePair<string, object> param in postParameters)
            {
                // Add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsClrf)
                    formDataStream.Write(Encoding.GetBytes("\r\n"), 0, Encoding.GetByteCount("\r\n"));

                needsClrf = true;

                FileParameter value = param.Value as FileParameter;
                if (value != null)
                {
                    FileParameter fileToUpload = value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}{4}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream",
                        fileToUpload.Zippy == null ? "" : "\r\nContent-Transfer-Encoding: binary");

                    formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(Encoding.GetBytes(postData), 0, Encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.GetBytes(footer), 0, Encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public string Zippy { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
                if (ContentType != null && ContentType.Contains("zip"))
                {
                    Zippy = ContentType;
                }
                else
                {
                    Zippy = null;
                }

            }
        }
    }
}
