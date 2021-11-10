using ImageMagick;
using LG_Exam_ImageConverterPipeline.model;
using LG_Exam_ImageConverterPipeline.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LG_Exam_ImageConverterPipeline
{
   

    class Program
    {
        static Stopwatch Watch = new Stopwatch();
        static List<Task> _tasks { get; set; }

        static config_DataModel _config;

        static FileSystemWatcher _watcher;

        static bool _canBuildAsset = false;
        static void Main(string[] args)
        {
            //TODO
            // get files -> process files -> complete
            Initialize();

            Process();

            Complete();
        }

        private static void Initialize()
        {
            Watch.Start();

            WebRequest request = HttpWebRequest.Create(Config.TexConfigUrl);

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responseText = reader.ReadToEnd();

            _config = JsonConvert.DeserializeObject<config_data>(responseText).config;


            _tasks = new List<Task>();

            Start(Config.InputFileDirectory);

        }

        private static void Complete()
        {
            Watch.Stop();

            Console.WriteLine("Complete " + Watch.Elapsed);
        }

        /// <summary>
        /// Process Tasks
        /// </summary>
        public static void Process()
        {
            while (true)
            {
                Task.WhenAll(_tasks).ContinueWith(x =>
                {
                    if(_canBuildAsset)
                    {
                        BuildAssetBundle();
                        _canBuildAsset = false;
                    }
                })
                .Wait();
            }
        }

        /// <summary>
        /// Task for loading file and for generating size variants 
        /// </summary>
        /// <param name="pFile"></param>
        /// <returns></returns>
        public static Task LoadFileThenInsertToQueue(string pFile)
        {
            return Task.Run(() =>
            {
                try
                {
                    _canBuildAsset = true;
                    Console.WriteLine("Loading file " + pFile);

                    var temp = pFile.Split(Path.DirectorySeparatorChar);
                    var filename = Path.GetFileName(pFile);

                    for (int y = 0; y < _config.target_sizes.Length; y++)
                    {
                        //create magickimage object
                        var image = new MagickImage(pFile);

                        //set bit depth
                        image.BitDepth(_config.bit_depth);

                        //set sizes according to config
                        var size = new MagickGeometry(_config.target_sizes[y], _config.target_sizes[y]);
                        size.IgnoreAspectRatio = true;

                        image.Resize(size);

                        Console.WriteLine("creating " + filename + _config.target_sizes[y].ToString());
                        //create a file in the output folder
                        image.Write(Config.OutputFileDirecotry + filename + _config.target_sizes[y].ToString() + "." + _config.format);
                    }
                }
                catch (Exception ex)
                {
                    //TODO throw erro "Error in GetDataAsync()";
                }

            });
        }

        /// <summary>
        /// Building Assetbundle
        /// will call batch script
        /// </summary>
        public static void BuildAssetBundle()
        {
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                proc.StartInfo.FileName = Config.ProjectDir + "buildassetbundle.bat";
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();

                Console.WriteLine("Build Complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }

        #region Callbacks for FileWatch

        public static void Start(string pDirectory)
        {
            _watcher = new FileSystemWatcher(pDirectory);

            _watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            _watcher.Created += OnCreated;

            _watcher.Filter = "*";
            _watcher.IncludeSubdirectories = false; //should be files only???
            _watcher.EnableRaisingEvents = true;
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            if(string.Compare( Path.GetExtension(e.FullPath), ".png") == 0 ||
                string.Compare(Path.GetExtension(e.FullPath), ".jpg") == 0)
            {
                _tasks.Add(LoadFileThenInsertToQueue(e.FullPath));
            }
            else
            {
                Console.WriteLine("File not supported");
            }
        }
        #endregion
    }
}
