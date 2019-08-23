using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace dxniraq2u2018.Extensions.CoreHtmlToImage
{
    /// <summary>
    /// Html Converter. Converts HTML string and URLs to image bytes
    /// </summary>
    public class HtmlConverter
    {
        private const string toolFilename = "wkhtmltoimage.exe";
        private string toolFilepath;
        private readonly string tempFolderName = "ExportImageData";
        private HtmlConverter()
        {
        }
        private static readonly object padlock = new object();
        private static HtmlConverter instance = null;
        public static HtmlConverter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new HtmlConverter();
                        }
                    }
                }
                return instance;
            }
        }
         /// <summary>
        /// Converts HTML string to image
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <param name="width">Output document width</param>
        /// <param name="format">Output image format</param>
        /// <param name="quality">Output image quality 1-100</param>
        /// <returns></returns>
        public byte[] FromHtmlString(IHostingEnvironment environment, string html, int width = 1024, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            var tempHtmlPath = Path.Combine(environment.WebRootPath, tempFolderName);
            if (!Directory.Exists(tempHtmlPath))
                Directory.CreateDirectory(tempHtmlPath);
            var filename = Path.Combine(tempHtmlPath, $"{Guid.NewGuid()}.html");
            File.WriteAllText(filename, html);
            var bytes = FromUrl(environment, filename, width, format, quality);
            File.Delete(filename);
            return bytes;
        }

        /// <summary>
        /// Converts HTML page to image
        /// </summary>
        /// <param name="url">Valid http(s):// URL</param>
        /// <param name="width">Output document width</param>
        /// <param name="format">Output image format</param>
        /// <param name="quality">Output image quality 1-100</param>
        /// <returns></returns>
        public byte[] FromUrl(IHostingEnvironment environment, string url, int width = 1024, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            var directory = environment.WebRootPath;
            toolFilepath = Path.Combine(directory, "CoreHtmlToImage");
            toolFilepath = Path.Combine(toolFilepath, toolFilename);
            var imageFormat = format.ToString().ToLower();
            var tempImagePath = Path.Combine(directory, tempFolderName);
            if (!Directory.Exists(tempImagePath))
                Directory.CreateDirectory(tempImagePath);
            var filename = Path.Combine(tempImagePath, $"{Guid.NewGuid().ToString()}.{imageFormat}");
            Process process = Process.Start(new ProcessStartInfo(toolFilepath, $"--quality {quality} --width {width} -f {imageFormat} {url} {filename}")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = directory,
                RedirectStandardError = true
            });

            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.WaitForExit();

            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                File.Delete(filename);
                return bytes;
            }

            throw new Exception("Something went wrong. Please check input parameters");
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception(e.Data);
        }
    }

    public enum ImageFormat
    {
        Jpg,
        Png
    }
}
