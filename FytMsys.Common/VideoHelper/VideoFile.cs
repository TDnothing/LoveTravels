using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FytMsys.Common
{
    public class VideoFile
    {
        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }
        public TimeSpan Duration { get; set; }
        public double BitRate { get; set; }
        public string RawAudioFormat { get; set; }
        public string AudioFormat { get; set; }
        public string RawVideoFormat { get; set; }
        public string VideoFormat { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string RawInfo { get; set; }
        public bool infoGathered { get; set; }

        public VideoFile(string path)
        {
            _Path = path;
            Initialize();
        }
        private void Initialize()
        {
            this.infoGathered = false;
            if (string.IsNullOrEmpty(_Path))
            {
                throw new Exception("Video file Path not set or empty.");
            }
            if (!File.Exists(_Path))
            {
                throw new Exception("The video file " + _Path + " does not exist.");
            }
        }
    }
}