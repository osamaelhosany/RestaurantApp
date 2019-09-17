using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json;
using RestaurantApp.Constants;
using RestaurantApp.Models;
using RestaurantApp.Models.Menu;
using RestaurantApp.Models.Offer;
using RestaurantApp.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services.SqliteServices
{
    public static class SqliteServices
    {
        private static MobileServiceClient client;
        private const string offlineDbPath = @"databaseVersion2.db";
        static SqliteServices()
        {
            client = new MobileServiceClient(AppConstants.BaseEndPointOfflineMode);
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<OfferModel>();
            store.DefineTable<UserModel>();
            store.DefineTable<CategoryModel>();
            store.DefineTable<ItemModel>();
            client.SyncContext.InitializeAsync(store);
        }

        /// <summary>
        /// generic method return all rows of T Table 
        /// </summary>
        /// <typeparam name="T"> T Generic</typeparam>
        /// <param name="syncItems"></param>
        /// <returns>return observablecollection of generic object</returns>
        public static async Task<ObservableCollection<T>> GetListAsync<T>(bool syncItems = false) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            try
            {
                if (syncItems)
                {
                    await SyncAsync<T>();
                }
                IEnumerable<T> items = await Table.ToEnumerableAsync();

                return new ObservableCollection<T>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"InvalID sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        /// <summary>
        /// generic method to sync local table with server and vise versa 
        /// </summary>
        /// <typeparam name="T"> T generic</typeparam>
        /// <returns></returns>
        public static async Task SyncAsync<T>() where T : BaseModel
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            try
            {
                await client.SyncContext.PushAsync();
                await Table.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "",
                    Table.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["ID"]);
                }
            }
        }
        /// <summary>
        /// generic method to update object in Table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task UpdateAsync<T>(T item) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            await Table.UpdateAsync(item);
            await SyncAsync<T>();
        }
        /// <summary>
        /// generic method to insert object in Table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task InsertAsync<T>(T item) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            await Table.InsertAsync(item);
            await SyncAsync<T>();
        }
        /// <summary>
        /// generic method to insert or updaye object in Table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task InsertOrUpdateAsync<T>(T item) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            //var existtable = await GetListAsync<T>();
            var existtable = await Table.ToEnumerableAsync();
            var record = existtable.Any(x => x.ID == item.ID);
            //IMobileServiceTableQuery<T> query = Table.Where(x => x.ID == item.ID);
            //List<T> listitems = await query.ToListAsync();
            if (record)
                await Table.UpdateAsync(item);
            else
                await Table.InsertAsync(item);
        }
        /// <summary>
        /// generic method to get rows from table by ID 
        /// </summary>
        /// <typeparam name="T">ID as string</typeparam>
        /// <param name="ID"></param>
        /// <returns>item of generic object</returns>
        public static async Task<List<T>> SelectByID<T>(string ID) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            IMobileServiceTableQuery<T> query = Table.Where(x => x.ID == ID);
            List<T> items = await query.ToListAsync();
            return items;
        }
        /// <summary>
        /// generic method to get one row from table by ID 
        /// </summary>
        /// <typeparam name="T">ID as string</typeparam>
        /// <param name="ID"></param>
        /// <returns>item of generic object</returns>
        public static async Task<T> SelectOneItem<T>(string ID) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            IMobileServiceTableQuery<T> query = Table.Where(x => x.ID == ID);
            List<T> items = await query.ToListAsync();
            return items.FirstOrDefault();
        }
        /// <summary>
        /// generic method to remove item form table by ID or by object
        /// </summary>
        /// <typeparam name="T">ID as string</typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static async Task RemoveASync<T>(string ID) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            IMobileServiceTableQuery<T> query = Table.Where(x => x.ID == ID);
            List<T> items = await query.ToListAsync();
            var item = items.FirstOrDefault();
            if (item != null)
            {
                await Table.DeleteAsync(item);
            }
        }
        /// <summary>
        /// generic method to remove item form table by ID or by object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task RemoveASync<T>(T item) where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            if (item != null)
            {
                await Table.DeleteAsync(item);
            }
        }
        /// <summary>
        /// generic method to delete all rows in table from local storage 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task RemoveAll<T>() where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            //await Table.PurgeAsync(true);
            var table = await GetListAsync<T>(false);
            if (table.Count > 0)
            {
                foreach (var item in table)
                {
                    await Table.DeleteAsync(item);
                }
            }

        }
        /// <summary>
        /// generic mehtod to delete all rows in table from local storage and server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task RemoveAllFromServer<T>() where T : BaseModel
        {
            IMobileServiceSyncTable<T> Table;
            Table = client.GetSyncTable<T>();
            await Table.PullAsync("checkserverdata", Table.CreateQuery());
            var table = await GetListAsync<T>(true);
            if (table.Count >= 0)
            {
                foreach (var item in table)
                {
                    await Table.DeleteAsync(item);
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// generic method to cast List from parent to child and vise versa
        /// T is Target Casting 
        /// y is original Type 
        /// </summary>
        /// <typeparam name="T">object target</typeparam>
        /// <typeparam name="y">object original</typeparam>
        /// <param name="ConvertList"></param>
        /// <returns>return List</returns>
        public static List<T1> CastingList<T1, T2>(List<T2> ConvertList) where T1 : BaseModel where T2 : BaseModel
        {
            var serializedParent = JsonConvert.SerializeObject(ConvertList);
            List<T1> ConvertedList = JsonConvert.DeserializeObject<List<T1>>(serializedParent);
            return ConvertedList;
        }
        /// <summary>
        /// generic method to castObject from parent to child and vise versa
        /// T is Target Casting 
        /// y is original Type 
        /// </summary>
        /// <typeparam name="T">object target</typeparam>
        /// <typeparam name="y">object original</typeparam>
        /// <param name="ConvertOject"></param>
        /// <returns>return object</returns>
        public static T1 CastingObject<T1, T2>(T2 ConvertObject) where T1 : BaseModel where T2 : BaseModel
        {
            var serializedParent = JsonConvert.SerializeObject(ConvertObject);
            T1 Convertedobject = JsonConvert.DeserializeObject<T1>(serializedParent);
            return Convertedobject;
        }
    }
}
