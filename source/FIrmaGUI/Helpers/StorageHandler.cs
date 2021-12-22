using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace FirmaGUI.Helpers
{
    internal class StorageHandler
    {
        readonly string LocalStatePath = $"{ApplicationData.Current.LocalFolder.Path}\\";

        public string GetStringFromReswFile(string Id)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                return resourceLoader.GetString(Id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void SaveFile(string path, string value)
        {
            using (StreamWriter F = new StreamWriter(LocalStatePath + path))
            {
                F.WriteLine(value);
                F.Close();
            }
        }
        public string ReadFile(string path)
        {

            string ReadData = null;
            try
            {
                using (StreamReader R = File.OpenText(LocalStatePath + path)) 
                {
                    ReadData = R.ReadToEnd();
                    R.Close();

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return ReadData;
        }
        public void SaveObjectToBinaryFile(object obj, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(LocalStatePath + path, FileMode.Create);
            try
            {
                formatter.Serialize(fs, obj);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        public object ReadObjectFromBinaryFile(string path)
        {
            FileStream fs = new FileStream(LocalStatePath + path, FileMode.Open);
            Object obj = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(fs);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return obj;
        }

        public async void UserSaveFile(string type, string content)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add(GetStringFromReswFile("AppDisplayName"), new List<string>() { type });
            savePicker.SuggestedFileName = GetStringFromReswFile("AppDisplayName") + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public async Task<string> UserReadFile(string type)
        {
            var Folder = ApplicationData.Current.LocalCacheFolder;
            var Items = await Folder.GetItemsAsync();
            if (Items.Count != 0) foreach (var Item in Items)
            {
                await Item.DeleteAsync();

            }
            var Picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            string ReadString = null;
            Picker.FileTypeFilter.Add(type);
            var file = await Picker.PickSingleFileAsync();
            
            if (file != null)
            {
                string Token = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                var CopiedFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(Token);
                try
                {
                    await file.CopyAsync(ApplicationData.Current.LocalCacheFolder);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return null;
                }
                var Path = file.Name;
                var TmpPath = ApplicationData.Current.LocalCacheFolder.Path + @"\" + Path;
                ReadString = System.IO.File.ReadAllText(TmpPath);
                Folder = ApplicationData.Current.LocalCacheFolder;
                StorageFile File = await Folder.GetFileAsync(Path);
                await File.DeleteAsync();
            }
            return ReadString;
        }
        internal class XmlDeserializedTeam
        {
            public object Xml { get; set; }
        }
        public string JsonToXml(string Json)
        {
            XmlDocument XML = JsonConvert.DeserializeXmlNode(Json, "Xml");
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            XML.WriteTo(xmlTextWriter);
            return stringWriter.ToString();
        }
        public string XmlToJson(string SerializedXml)
        {
            XmlDocument Document = new XmlDocument();
            Document.LoadXml(SerializedXml);
            string json = JsonConvert.SerializeXmlNode(Document);
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<XmlDeserializedTeam>(json).Xml);
        }
    }
}
