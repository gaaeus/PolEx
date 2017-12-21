using SQLite;
using System;

namespace PolEX.Model
{
    /// <summary>
    /// Main settings for the application
    /// </summary>
    [Serializable]
    [Table("AppSettings")]
    public class AppSettings
    {
        #region Properties

        [Column("MAX_VIDEO_KB")]
        public UInt64 MAX_VIDEO_KB { get; set; }

        [Column("USE_COORDINATES")]
        public Boolean USE_COORDINATES { get; set; }

        [Column("SEND_ANONYMOUS")]
        public Boolean SEND_ANONYMOUS { get; set; }

        [Column("SERVER_URL")]
        public String SERVER_URL { get; set; }

        [Column("SERVER_PORT")]
        public String SERVER_PORT { get; set; }

        [Column("SERVER_CONNECTION_TIMEOUT")]
        public UInt32 SERVER_CONNECTION_TIMEOUT { get; set; }

        [Column("USE_SSL")]
        public Boolean USE_SSL { get; set; }

        #endregion

        #region Constructor

        public AppSettings() { }

        #endregion
    }
}
