using SQLite;
using System;

namespace PolEX.Model
{
    /// <summary>
    /// Class to store video data attacheable to a request
    /// </summary>
    [Serializable]
    [Table("Video")]
    public class Video : BindableBase
    {
        #region Properties

        private UInt64? _video_id;
        private UInt64? _request_id;
        public Tuple<int, int, int> _video_coordinates;
        private DateTime _video_datetime;

        [Column("video_id")]
        [PrimaryKey, AutoIncrement, Indexed]
        public UInt64? video_id
        {
            get { return _video_id; }
            set { SetProperty(ref _video_id, value, "_video_id"); }
        }

        [Column("request_id")]
        public UInt64? request_id
        {
            get { return _request_id; }
            set { SetProperty(ref _request_id, value, "request_id"); }
        }

        [Column("video_coordinates")]
        public Tuple<int, int, int> VideoCoordinates
        {
            get { return _video_coordinates; }
            set { SetProperty(ref _video_coordinates, value, "VideoCoordinates"); }
        }

        [Column("video_datetime")]
        public DateTime VideoDatetime
        {
            get { return _video_datetime; }
            set { SetProperty(ref _video_datetime, value, "VideoDatetime"); }
        }

        #endregion

        #region Constructor

        public Video() { }

        public Video(UInt64? video_id, UInt64? request_id, Tuple<int, int, int>  video_coordinates, DateTime video_datetime)
        {
            this._video_id = video_id;
            this._request_id = request_id;
            this._video_coordinates = video_coordinates;
            this._video_datetime = video_datetime;
        }

        #endregion
    }
}
