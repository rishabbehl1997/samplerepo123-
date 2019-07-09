using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Swashbuckle.Swagger.Annotations;

namespace ApiAppSample.Controllers
{
    public class ApiAppController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();
        string valueSecret;
        string keyVault;
        string secretName;
        // GET api/values
        [SwaggerOperation("GetTestSample")]
        public IEnumerable<string> Get(string val)
        {
            return new string[] { "TEST API SAMPLE COMPLETE" };
        }

        [HttpGet]
        [System.Web.Http.Route("Api/Add")]
        public int Add(int a, int b)
        {
            return a + b;
        }

        // GET api/values/rishabkeyvault
        [SwaggerOperation("GetByKeyVaultName")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("api/KeyVaultsGetCredentials")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> Get()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            keyVault = System.Configuration.ConfigurationManager.AppSettings["keyVault"];
            secretName = System.Configuration.ConfigurationManager.AppSettings["secretName"];
            var secret = await keyVaultClient.GetSecretAsync("https://" + keyVault + ".vault.azure.net/secrets/" + secretName + "").ConfigureAwait(false);
            valueSecret = secret.Value;
            HttpClient client = new HttpClient();
            string url = System.Configuration.ConfigurationManager.AppSettings["url"];
            var response = await client.GetAsync(url+valueSecret);
            return response;
        }

        // GET api/values/rishabkeyvault
        [SwaggerOperation("GetByKeyVaultName")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("api/KeyVaultsGetCredentials1")]
        public async System.Threading.Tasks.Task<String> Get(int a)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            keyVault = System.Configuration.ConfigurationManager.AppSettings["keyVault"];
            secretName = System.Configuration.ConfigurationManager.AppSettings["secretName"];
            var secret = await keyVaultClient.GetSecretAsync("https://" + keyVault + ".vault.azure.net/secrets/" + secretName + "").ConfigureAwait(false);
            valueSecret = secret.Value;
            return valueSecret;
        }
    }
}
