using SQLite;
using System;
using System.Collections.Generic;

namespace PolEX.Model
{
    /// <summary>
    /// Request classifications
    /// </summary>
    [Serializable]
    public enum RequestClassification
    {
        Question = 0,
        Information = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        Emergency = 5,
        Other = 6
    }

    /// <summary>
    /// Requests class to be stored and sent by the application
    /// </summary>
    [Serializable]
    [Table("Request")]
    public class Request : BindableBase
    {
        #region Properties

        private UInt64? _request_id;
        private UInt64? _category_id;
        private String _request_title;
        private String _request_content;
        private Tuple<int, int, int> _request_coordinates;
        private DateTime _request_datetime;
        private String _APP_GUID;
        private RequestClassification _request_classification;

        private ICollection<Image> _imagecollection { get; set; }
        private ICollection<Video> _videocollection { get; set; }

        [Column("request_id")]
        [PrimaryKey, AutoIncrement, Indexed]
        public UInt64? request_id
        {
            get { return _request_id; }
            set { SetProperty(ref _category_id, value, "request_id"); }
        }

        [Column("category_id")]
        public UInt64? category_id
        {
            get { return _category_id; }
            set { SetProperty(ref _category_id, value, "category_id"); }
        }

        [Column("request_title")]
        public String RequestTitle
        {
            get { return _request_title; }
            set { SetProperty(ref _request_title, value, "RequestTitle"); }
        }

        [Column("request_content")]
        public String RequestContent
        {
            get { return _request_content; }
            set { SetProperty(ref _request_content, value, "RequestContent"); }
        }

        [Column("request_coordinates")]
        public Tuple<int, int, int> RequestCoordinates
        {
            get { return _request_coordinates; }
            set { SetProperty(ref _request_coordinates, value, "RequestCoordinates"); }
        }

        [Column("request_datetime")]
        public DateTime RequestDatetime
        {
            get { return _request_datetime; }
            set { SetProperty(ref _request_datetime, value, "RequestDatetime"); }
        }

        [Column("APP_GUID")]
        public String APP_GUID
        {
            get { return _APP_GUID; }
            set { SetProperty(ref _APP_GUID, value, "APP_GUID"); }
        }

        [Column("request_classification")]
        public RequestClassification RequestClassification
        {
            get { return _request_classification; }
            set { SetProperty(ref _request_classification, value, "RequestClassification"); }
        }

        public ICollection<Image> ImageCollection {
            get { return _imagecollection; }
            set {
                ICollection<Image> localcollection = _imagecollection;
                SetProperty(ref localcollection, value, "ImageCollection"); 
            }
        }

        public ICollection<Video> VideoCollection {
            get { return _videocollection; }
            set
            {
                ICollection<Video> localcollection = _videocollection;
                SetProperty(ref localcollection, value, "VideoCollection");
            }
        }

        #endregion

        #region Constructor

        public Request() { }

        public Request(UInt64? request_id, UInt64? category_id, String request_title, String request_content, Tuple<int, int, int> request_coordinates, DateTime request_datetime, String APP_GUID, RequestClassification request_classification, ICollection<Image> image_collection, ICollection<Video> video_collection)
        {
            this._request_id = request_id;
            this._category_id = category_id;
            this._request_title = request_title;
            this._request_content = request_content;
            this._request_coordinates = request_coordinates;
            this._request_datetime = request_datetime;
            this._APP_GUID = APP_GUID;
            this._request_classification = request_classification;
            this._imagecollection = image_collection;
            this._videocollection = video_collection;
        }

        #endregion
    }
}
