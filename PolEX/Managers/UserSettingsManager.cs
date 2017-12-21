using PolEX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolEX.Managers
{
    public class UserSettingsManager
    {
        #region Properties

        public UserSettings UserSettings {get; set;}
        bool Success { get; set; }

        TaskScheduler taskScheduler;

        #endregion

        #region Constructors

        public UserSettingsManager() {
            taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();        
        }

        #endregion

        #region User Settings CRUD

        /// <summary>
        /// Creates the user settings
        /// </summary>
        public async void CreateUpdateUserSettings()
        {
            bool result = false;

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    using (DBManager dbManager = new DBManager())
                    {
                        result = dbManager.CreateUpdateUserSettings(this.UserSettings);
                    }
                })
                .ContinueWith(_ =>
                {
                    this.Success = result;

                }, taskScheduler);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ReadUserSettings()
        {
        
        }

        public async Task DeleteUserSettings()
        {
        
        }

        #endregion



    }
}
