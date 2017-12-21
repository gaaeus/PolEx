using SQLite;
using System;

namespace PolEX.Model
{
    /// <summary>
    /// Main settings for the user, including address and contact info
    /// warning_check fields are still undefined
    /// </summary>
    [Serializable]
    [Table("UserSettings")]
    public class UserSettings : BindableBase
    {
        #region Properties

        private UInt64? _user_id;
        private String _user_name;
        private String _user_alias;
        private String _user_email;
        private String _user_phone;
        private String _user_cellphone;
        private String _user_address;
        private String _user_address2;
        private String _user_city;
        private String _user_county;
        private String _user_postal_code;
        private String _observations;
        private String _map_coordinates;
        private Boolean _warning_check1;
        private Boolean _warning_check2;
        private Boolean _warning_check3;

        [Column("user_id")]
        [PrimaryKey, AutoIncrement, Indexed]
        public UInt64? user_id {
            get { return _user_id; }
            set { SetProperty(ref _user_id, value, "user_id"); }
        }

        [Column("user_name")]
        public String UserName {
            get { return _user_name; }
            set { SetProperty(ref _user_name, value, "UserName"); }
        }

        [Column("user_alias")]
        public String UserAlias {
            get { return _user_alias; }
            set { SetProperty(ref _user_alias, value, "UserAlias"); }
        }

        [Column("user_email")]
        public String UserEmail {
            get { return _user_email; }
            set { SetProperty(ref _user_email, value, "UserEmail"); }
        }

        [Column("user_phone")]
        public String UserPhone {
            get { return _user_phone; }
            set { SetProperty(ref _user_phone, value, "UserPhone"); }
        }

        [Column("user_cellphone")]
        public String UserCellPhone
        {
            get { return _user_cellphone; }
            set { SetProperty(ref _user_cellphone, value, "UserCellPhone"); }
        }

        [Column("user_address")]
        public String UserAddress {
            get { return _user_address; }
            set { SetProperty(ref _user_address, value, "UserAddress"); }
        }

        [Column("user_address2")]
        public String UserAddress2 {
            get { return _user_address2; }
            set { SetProperty(ref _user_address2, value, "UserAddress2"); }
        }

        [Column("user_city")]
        public String UserCity
        {
            get { return _user_city; }
            set { SetProperty(ref _user_city, value, "UserCity"); }
        }

        [Column("user_county")]
        public String UserCounty
        {
            get { return _user_county; }
            set { SetProperty(ref _user_county, value, "County"); }
        }

        [Column("user_postal_code")]
        public String PostalCode
        {
            get { return _user_postal_code; }
            set { SetProperty(ref _user_postal_code, value, "PostalCode"); }
        }

        [Column("observations")]
        public String Observations {
            get { return _observations; }
            set { SetProperty(ref _observations, value, "Observations"); }
        }

        [Column("map_coordinates")]
        public String MapCoordinates {
            get { return _map_coordinates; }
            set { SetProperty(ref _map_coordinates, value, "MapCoordinates"); }
        }

        [Column("warning_check1")]
        public Boolean WarningCheck1 {
            get { return _warning_check1; }
            set { SetProperty(ref _warning_check1, value, "WarningCheck1"); }
        }

        [Column("warning_check2")]
        public Boolean WarningCheck2 {
            get { return _warning_check2; }
            set { SetProperty(ref _warning_check2, value, "WarningCheck2"); }
        }

        [Column("warning_check3")]
        public Boolean WarningCheck3 {
            get { return _warning_check3; }
            set { SetProperty(ref _warning_check3, value, "WarningCheck3"); }
        }

        #endregion

        #region Constructor

        public UserSettings() { }

        public UserSettings(UInt64? user_id, String user_name, String user_alias, String user_email, String user_phone, String user_address, String user_address2, String observations, String map_coordinates, Boolean warning_check1, Boolean warning_check2, Boolean warning_check3)
        {
            this._user_id = user_id;
            this._user_name = user_name;
            this._user_alias = user_alias;
            this._user_email = user_email;
            this._user_phone = user_phone;
            this._user_address = user_address;
            this._user_address2 = user_address2;
            this._observations = observations;
            this._map_coordinates = map_coordinates;
            this._warning_check1 = warning_check1;
            this._warning_check2 = warning_check2;
            this._warning_check3 = warning_check3;
        }

        #endregion
    }
}
