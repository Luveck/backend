
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class RestService : IRestService
    {
        // Post
        public async Task<T> PostRestServiceAsync<T>(string url, string controller,
           string method, object parameters, IDictionary<string, string> headers)
        {
            var baseUrl = string.Format("{0}{1}/{2}", url, controller, method);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpContent jsonObject = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
                HttpResponseMessage resMesAux = await client.PostAsync(baseUrl, jsonObject);
                if (resMesAux.IsSuccessStatusCode)
                {
                    var data = await resMesAux.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data);
                }
                else
                {
                    if (resMesAux.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        var data = await resMesAux.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<ResponseModelDto<object>>(data);

                        throw new BusinessException(objResponse.Messages);
                    }
                }

                throw new ArgumentException(string.Format("{0},{1},{2},{3}", url, resMesAux.StatusCode, resMesAux.Content, baseUrl));
            }
        }

        // Get
        public async Task<T> GetRestServiceAsync<T>(string url, string controller, string method,
            IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            string baseUrl = string.Format("{0}/{1}/{2}", url, controller, method);

            if (parameters.Count > 0)
                baseUrl = baseUrl + "?" + string.Join("&", parameters.Select(p => p.Key + "=" + p.Value).ToArray());

            using (HttpClient clientAux = new HttpClient())
            {
                clientAux.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        clientAux.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage res = await clientAux.GetAsync(baseUrl);

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data);
                }
                else
                {
                    //if (res.StatusCode.Equals(StatusCodes.Status400BadRequest))
                    if (res.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest))
                    {
                        var data = await res.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<ResponseModelDto<object>>(data);

                        throw new BusinessException(objResponse.Messages);
                    }
                }

                throw new ArgumentException(string.Format("{0},{1},{2},{3}", url, res.StatusCode, res.Content, baseUrl));
            }
        }

        public async Task<T> PostRestServiceStringParametersAsync<T>(string url, string controller, string method, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            var baseUrl = string.Format("{0}/{1}/{2}", url, controller, method);

            if (parameters.Count > 0)
                baseUrl = baseUrl + "?" + string.Join("&", parameters.Select(p => p.Key + "=" + p.Value).ToArray());

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpContent jsonObject = new StringContent(string.Empty);
                HttpResponseMessage res = await client.PostAsync(baseUrl, jsonObject);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data);
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        var data = await res.Content.ReadAsStringAsync();
                        var objResponse = JsonConvert.DeserializeObject<ResponseModelDto<object>>(data);

                        throw new BusinessException(objResponse.Messages);
                    }
                }

                throw new ArgumentException(string.Format("{0},{1},{2},{3}", url, res.StatusCode, res.Content, baseUrl));
            }
        }
    }
}
