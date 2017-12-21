using SQLite;
using System;

namespace PolEX.Model
{
    /// <summary>
    /// Class to store image data attacheable to a request
    /// </summary>
    [Serializable]
    [Table("Image")]
    public class Image : BindableBase
    {
        #region Properties

        private UInt64? _image_id;
        private UInt64? _request_id;
        private String _image_no;
        private String _image_path;
        public Tuple<double, double, double> _image_coordinates;
        private DateTime _image_datetime;

        [Column("image_id")]
        [PrimaryKey, AutoIncrement, Indexed]
        public UInt64? image_id
        {
            get { return _image_id; }
            set { SetProperty(ref _image_id, value, "image_id"); }
        }

        [Column("request_id")]
        public UInt64? request_id
        {
            get { return _request_id; }
            set { SetProperty(ref _request_id, value, "request_id"); }
        }

        [Column("image_no")]
        [MaxLength(12)]
        public String ImageNo
        {
            get { return _image_no; }
            set { SetProperty(ref _image_no, value, "ImageNo"); }
        }

        [Column("image_path")]
        [MaxLength(1024)]
        public String ImagePath
        {
            get { return _image_path; }
            set { SetProperty(ref _image_path, value, "ImagePath"); }
        }

        [Column("image_coordinates")]
        public Tuple<double, double, double> ImageCoordinates
        {
            get {return _image_coordinates;}
            set { SetProperty(ref _image_coordinates, value, "ImageCoordinates"); }
        }

        [Column("image_datetime")]
        public DateTime ImageDatetime
        {
            get { return _image_datetime; }
            set { SetProperty(ref _image_datetime, value, "ImageDatetime"); }
        }

        #endregion

        #region Constructor

        public Image() { }

        public Image(UInt64? image_id, UInt64? request_id, Tuple<int, int, int>  image_coordinates, DateTime image_datetime)
        {
            this._image_id = image_id;
            this._request_id = request_id;
            this._image_coordinates = image_coordinates;
            this._image_datetime = image_datetime;
        }

        #endregion
    }
}

// Notes
// Use writeablebitmap for storage?