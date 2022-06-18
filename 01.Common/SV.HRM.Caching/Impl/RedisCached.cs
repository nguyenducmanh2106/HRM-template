using SV.HRM.Caching.Common;
using SV.HRM.Caching.Interface;
using StackExchange.Redis;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SV.HRM.Core.Utils;
using SV.HRM.Logging;
using Microsoft.Extensions.Logging;

namespace SV.HRM.Caching.Impl
{
    public class RedisCached : ICached, IQueueAndListCached
    {
        protected ConnectionMultiplexer _redisConnection;
        protected Task<ConnectionMultiplexer> _redisConnectionAsync;
        private CachingConfigModel _configuration;
        private ILogger _logger = ApplicationLogManager.CreateLogger<RedisCached>();

        public RedisCached(CachingConfigModel configuration = null)
        {
            _configuration = configuration;
            var connectionMutiplexer = new ConnectionMultiplexerHelper(configuration);
            _redisConnection = connectionMutiplexer.ConnectionMultipexe;
            _redisConnectionAsync = connectionMutiplexer.ConnectionMultipexeAsync;
        }

        private IDatabase GetDatabaseInstance()
        {
            IDatabase client = null;

            try
            {
                client = _redisConnection.GetDatabase(_configuration.Database);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return client;
        }

        private IDatabase GetDatabaseInstanceForAsync()
        {
            IDatabase client = null;
            try
            {
                client = _redisConnectionAsync.Result.GetDatabase(_configuration.Database);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return client;
        }

        public bool Add<T>(string key, T item, int expireInMinute = 0)
        {
            try
            {
                IDatabase client = GetDatabaseInstance();

                byte[] bytes = ZipToBytes(item, key);

                DateTime currentTime = DateTime.Now;
                TimeSpan expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                if (expireInMinute > 0)
                    client.StringSet(key, bytes, expiry: expired);
                else
                    client.StringSet(key, bytes);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public bool Add(string key, string item, int expireInMinute = 0)
        {
            try
            {
                IDatabase client = GetDatabaseInstance();

                byte[] bytes = ZipToBytes(item, key);

                DateTime currentTime = DateTime.Now;
                TimeSpan expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                if (expireInMinute > 0)
                    client.StringSet(key, bytes, expiry: expired, flags: CommandFlags.FireAndForget);
                else
                    client.StringSet(key, bytes, flags: CommandFlags.FireAndForget);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> AddAsync<T>(string key, T item, int expireInMinute = 0)
        {
            try
            {
                RedisKey redisKey = key;
                RedisValue redisValue = ZipToBytes(item, key);

                DateTime currentTime = DateTime.Now;
                TimeSpan expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                IDatabase client = GetDatabaseInstanceForAsync();
                if (expireInMinute > 0)
                {
                    await client.StringSetAsync(redisKey, redisValue, expiry: expired, flags: CommandFlags.FireAndForget).ConfigureAwait(false);
                }
                else
                    await client.StringSetAsync(redisKey, redisValue, flags: CommandFlags.FireAndForget).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> AddAsync(string key, string item, int expireInMinute = 0)
        {
            try
            {
                RedisKey redisKey = key;
                RedisValue redisValue = ZipToBytes(item, key);

                DateTime currentTime = DateTime.Now;
                TimeSpan expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                IDatabase client = GetDatabaseInstanceForAsync();
                if (expireInMinute > 0)
                {
                    await client.StringSetAsync(redisKey, redisValue, expiry: expired, flags: CommandFlags.FireAndForget).ConfigureAwait(false);
                }
                else
                    await client.StringSetAsync(redisKey, redisValue, flags: CommandFlags.FireAndForget).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public T Get<T>(string key, HttpContext context = null, string refreshKey = null)
        {
            T result = default;
            try
            {
                IDatabase client = GetDatabaseInstance();

                if (CacheHelpers.IsRequestClearCache(context))
                {
                    client.KeyDelete(key);
                }

                byte[] redisValue = client.StringGet(key);

                result = UnZipFromBytes<T>(redisValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }

        public string Get(string key, HttpContext context = null, string refreshKey = null)
        {
            string result = string.Empty;
            try
            {
                IDatabase client = GetDatabaseInstance();

                if (CacheHelpers.IsRequestClearCache(context))
                {
                    client.KeyDelete(key);
                }

                byte[] redisValue = client.StringGet(key);

                result = UnZipFromBytes<string>(redisValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string key, HttpContext context = null, string refreshKey = null)
        {
            T result = default;
            try
            {
                IDatabase client = GetDatabaseInstanceForAsync();

                if (CacheHelpers.IsRequestClearCache(context, refreshKey))
                {
                    client.KeyDelete(key);
                }

                byte[] bytes = await client.StringGetAsync(key);
                result = UnZipFromBytes<T>(bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }

        public async Task<string> GetAsync(string key, HttpContext context = null, string refreshKey = null)
        {
            string result = string.Empty;
            try
            {
                IDatabase client = GetDatabaseInstanceForAsync();

                if (CacheHelpers.IsRequestClearCache(context, refreshKey))
                {
                    client.KeyDelete(key);
                }


                byte[] bytes = await client.StringGetAsync(key);

                result = bytes == null ? null : Unzip(bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }

        public bool Remove(string key)
        {
            bool result = false;

            try
            {
                IDatabase client = GetDatabaseInstance();

                result = client.KeyDelete(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return result;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            bool result = false;

            try
            {
                IDatabase client = GetDatabaseInstanceForAsync();

                result = await client.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return result;
        }



        private byte[] ZipToBytes<T>(T item, string key)
        {
            if (item == null || item.Equals(default(T)))
                return null;

            BinaryFormatter bf = new BinaryFormatter();

            byte[] bytes;

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, item);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        private T UnZipFromBytes<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
                return default;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        private void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[8096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }

        private string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        //Queue
        public string DeQueue(string key)
        {
            try
            {
                IDatabase client = GetDatabaseInstance();

                RedisValue valueFromSortedSet = client.ListRightPop(key);

                return valueFromSortedSet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return string.Empty;
            }
        }

        public T DeQueue<T>(string key)
        {
            try
            {
                string value = DeQueue(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<string> DeQueueAsync(string key)
        {
            try
            {

                IDatabase client = GetDatabaseInstanceForAsync();
                return await client.ListRightPopAsync(key);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return string.Empty;
            }
        }

        public async Task<T> DeQueueAsync<T>(string key)
        {
            try
            {
                string value = await DeQueueAsync(key);

                if (!string.IsNullOrEmpty(value))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public void EnQueue(string key, string item)
        {
            try
            {
                //if (score <= 0) score = Utils.DateTimeToUnixTime(DateTime.Now);

                IDatabase client = GetDatabaseInstance();

                client.ListLeftPush(key, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task EnQueueAsync(string key, string item)
        {
            try
            {
                //if (score <= 0) score = Utils.DateTimeToUnixTime(DateTime.Now);

                string value = NewtonJson.Serialize(item);

                IDatabase client = GetDatabaseInstanceForAsync();

                await client.ListLeftPushAsync(key, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}