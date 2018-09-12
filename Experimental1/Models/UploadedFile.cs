﻿namespace Experimental1.Models
{
    public class UploadedFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
        public string PublishedLocation { get; set; }
        public string Error { get; set; }
    }
    public class DownloadedFile
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string PublishedLocation { get; set; }
        public string Extension { get; set; }
        
    }
}