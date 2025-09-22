using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

public class UnityCloudSave : MonoBehaviour
{
    public async Task ListAllKeys()
    {
        try
        {
            var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();

            //Debug.Log($"Keys count: {keys.Count}\n" + $"Keys: {String.Join(", ", keys)}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task ListAllCustomKeys(string customId)
    {
        try
        {
            var keys = await CloudSaveService.Instance.Data.Custom.ListAllKeysAsync(customId);

            //Debug.Log($"Keys count for custom ID {customId}: {keys.Count}\n" + $"Keys: {String.Join(", ", keys)}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task ForceSaveSingleData(string key, string value)
    {
        try
        {
            Dictionary<string, object> oneElement = new Dictionary<string, object>();

            // It's a text input field, but let's see if you actually entered a number.
            if (Int32.TryParse(value, out int wholeNumber))
            {
                oneElement.Add(key, wholeNumber);
            }
            else if (Single.TryParse(value, out float fractionalNumber))
            {
                oneElement.Add(key, fractionalNumber);
            }
            else
            {
                oneElement.Add(key, value);
            }

            // Saving the data without write lock validation by passing the data as an object instead of a SaveItem
            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);

            Debug.Log($"Successfully saved {key}:{value} with updated write lock {result[key]}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task<string> ForceSaveObjectData(string key, string json)
    {
        try
        {
            // Although we are only saving a single value here, you can save multiple keys
            // and values in a single batch.
            Dictionary<string, object> oneElement = new Dictionary<string, object>
                {
                    { key, json }
                };

            // Saving data without write lock validation by passing the data as an object instead of a SaveItem
            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);
            string writeLock = result[key];

            Debug.Log($"Successfully saved {key}:{json} with updated write lock {writeLock}");

            return writeLock;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }

    public async Task<string> SaveObjectData(string key, string json, string writeLock)
    {
        try
        {
            // Although we are only saving a single value here, you can save multiple keys
            // and values in a single batch.
            // Use SaveItem to specify a write lock. The request will fail if the provided write lock
            // does not match the one currently saved on the server.
            Dictionary<string, SaveItem> oneElement = new Dictionary<string, SaveItem>
                {
                    { key, new SaveItem(json, writeLock) }
                };

            // Saving data with write lock validation by using a SaveItem with the write lock specified
            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);
            string newWriteLock = result[key];

            Debug.Log($"Successfully saved {key}:{json} with updated write lock {newWriteLock}");

            return newWriteLock;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }

    public async Task<T> RetrieveSpecificData<T>(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

            if (results.TryGetValue(key, out var item))
            {
                return item.Value.GetAs<T>();
            }
            else
            {
                Debug.Log($"There is no such key as {key}!");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return default;
    }

    public async Task RetrieveEverything()
    {
        try
        {
            // If you wish to load only a subset of keys rather than everything, you
            // can call a method LoadAsync and pass a HashSet of keys into it.
            var results = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

            //Debug.Log($"{results.Count} elements loaded!");

            foreach (var result in results)
            {
                Debug.Log($"Key: {result.Key}, Value: {result.Value.Value}");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task RetrieveAllCustomData(string customId)
    {
        try
        {
            // If you wish to load only a subset of keys rather than everything, you
            // can call a method LoadAsync and pass a HashSet of keys into it.
            var results = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(customId);

            //Debug.Log($"{results.Count} elements loaded from custom Id {customId}!");

            foreach (var result in results)
            {
                Debug.Log($"Key: {result.Key}, Value: {result.Value.Value}");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task ForceDeleteSpecificData(string key)
    {
        try
        {
            var deleteOptions = new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions
            {
                WriteLock = null // Set this to the specific write lock if needed, or keep it null to ignore validation
            };

            await CloudSaveService.Instance.Data.Player.DeleteAsync(key, deleteOptions);
            Debug.Log($"Successfully deleted {key}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError($"Validation error: {e}");
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError($"Rate limit error: {e}");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud save error: {e}");
        }
    }

    public async Task DeleteSpecificData(string key, string writeLock)
    {
        try
        {
            var deleteOptions = new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions
            {
                WriteLock = writeLock // Set this to the specific write lock if needed, or keep it null to ignore validation
            };

            await CloudSaveService.Instance.Data.Player.DeleteAsync(key, deleteOptions);
            Debug.Log($"Successfully deleted {key}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError($"Validation error: {e}");
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError($"Rate limit error: {e}");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud save error: {e}");
        }
    }

    public async Task ListAllFiles()
    {
        try
        {
            var results = await CloudSaveService.Instance.Files.Player.ListAllAsync();

            Debug.Log("Metadata loaded for all files!");

            foreach (var element in results)
            {
                Debug.Log($"Key: {element.Key}, File Size: {element.Size}");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task GetFileMetadata(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Files.Player.GetMetadataAsync(key);

            Debug.Log("File metadata loaded!");

            Debug.Log($"Key: {results.Key}, File Size: {results.Size}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task SaveFileBytes(string key, byte[] bytes)
    {
        try
        {
            await CloudSaveService.Instance.Files.Player.SaveAsync(key, bytes);

            Debug.Log("File saved!");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task SaveFileStream(string key, Stream stream)
    {
        try
        {
            await CloudSaveService.Instance.Files.Player.SaveAsync(key, stream);

            Debug.Log("File saved!");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task<byte[]> LoadFileBytes(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Files.Player.LoadBytesAsync(key);

            Debug.Log("File loaded!");

            return results;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }

    public async Task<Stream> LoadFileStream(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Files.Player.LoadStreamAsync(key);

            Debug.Log("File loaded!");

            return results;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }

    public async Task DeleteFile(string key)
    {
        try
        {
            await CloudSaveService.Instance.Files.Player.DeleteAsync(key);

            Debug.Log("File deleted!");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
}
