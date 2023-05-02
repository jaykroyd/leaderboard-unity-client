using System;
using System.Collections.Generic;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Leaderboards
{
    public class UnityWebRequestAPI
    {
        public struct Config : IUnityWebRequestConfiguration
        {
            public string ApiKey;
            public string BaseUrl;            
            public string Subdomain;           
            public int Version;

            string IUnityWebRequestConfiguration.ApiKey => ApiKey;
            string IUnityWebRequestConfiguration.BaseUrl => BaseUrl;
            string IUnityWebRequestConfiguration.Subdomain => Subdomain;
            int IUnityWebRequestConfiguration.Version => Version;
        }

        protected ILogger logger = default;
        protected Config config = default;

        public UnityWebRequestAPI(ILogger logger, IUnityWebRequestConfiguration config)
        {
            this.logger = logger;
            this.config = new Config 
            {
                ApiKey = config.ApiKey,
                BaseUrl = config.BaseUrl,
                Subdomain = config.Subdomain,
                Version = config.Version,
            };
        }

        public void GET<T2>(string endpoint, UnityAction<T2> onSuccess, UnityAction<string> onFailed) where T2 : IResponse, new()
        {
            var url = BuildURL(endpoint);
            var www = CreateUnityWebGetRequest(url);
            DoRequest(www, onSuccess, onFailed);
        }

        public void GET<T2>(string endpoint, UnityAction<T2> onSuccess, UnityAction<string> onFailed, IDictionary<string, string> query) where T2 : IResponse, new()
        {
            var url = BuildURL(endpoint);
            // add url params
            var www = CreateUnityWebGetRequest(url);
            DoRequest(www, onSuccess, onFailed);
        }

        public void POST<T1, T2>(string endpoint, T1 bodyModel, UnityAction<T2> onSuccess, UnityAction<string> onFailed) where T2 : IResponse, new()
        {
            var url = BuildURL(endpoint);
            UnityWebRequest www = CreateUnityWebPostRequest(bodyModel, url);
            DoRequest(www, onSuccess, onFailed);
        }

        private void DoRequest<T2>(UnityWebRequest www, UnityAction<T2> onSuccess, UnityAction<string> onFailed) where T2 : IResponse, new()
        {
            OnRequest(www.url);
            www.SetRequestHeader("Authorization", BuildAuthorization());
            www.timeout = 10;

            void OnOperationCompleted(AsyncOperation op)
            {
                op.completed -= OnOperationCompleted;
                bool isError = !IsSuccess(www);
                T2 response = BuildResponse<T2>(www);

                if (isError)
                {
                    OnFailed(www.url, www.error);
                    onFailed?.Invoke(www.error + " | " + response.Error);
                }
                else
                {
                    OnSuccess(www.url);
                    onSuccess?.Invoke(response);
                }

                www.Dispose();
            }

            var operation = www.SendWebRequest();
            operation.completed += OnOperationCompleted;
        }

        private UnityWebRequest CreateUnityWebGetRequest(string url)
        {
            return UnityWebRequest.Get(url);
        }

        private UnityWebRequest CreateUnityWebPostRequest<T1>(T1 bodyModel, string url)
        {
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            var body = BuildBody<T1>(bodyModel);
            www.uploadHandler = new UploadHandlerRaw(body);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            return www;
        }

        private string BuildURL(string endpoint)
        {
            return config.BaseUrl + "/api" + $"/v{config.Version}" + endpoint;
        }

        private byte[] BuildBody<T>(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return Encoding.UTF8.GetBytes(json);
        }

        private T BuildResponse<T>(UnityWebRequest request) where T : IResponse, new()
        {
            T resp;
            if (request.downloadedBytes > 0)
            {
                var json = Encoding.UTF8.GetString(request.downloadHandler.data);
                resp = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                resp = new T();
            }

            resp.IsError = !IsSuccess(request);
            return resp;
        }

        private string BuildAuthorization()
        {
            string auth = config.ApiKey + ":" + "";
            auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        private bool IsSuccess(UnityWebRequest request)
        {
#if UNITY_2020_1_OR_NEWER
            return request.result == UnityWebRequest.Result.Success;
#else
            return !request.isHttpError && !request.isNetworkError;
#endif
        }

        #region EVENT_CALLBACKS
        protected virtual void OnRequest(string endpoint)
        {
            logger.LogToUnity($"Attempting to submit http request at {endpoint}");
        }

        protected virtual void OnSuccess(string endpoint)
        {
            logger.LogToUnity($"Http request at {endpoint} was successful");
        }

        protected virtual void OnFailed(string endpoint, string error)
        {
            logger.LogErrorToUnity($"Failed to submit http request at {endpoint}: {error}");
        }
        #endregion
    }
}
