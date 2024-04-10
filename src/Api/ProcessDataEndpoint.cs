using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class ProcessDataEndpoint
    {
        private readonly KostalRestClient _client;

        internal ProcessDataEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns list of all modules with a list of available process-data identifiers
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProcessDataIdentifier>> GetProcessDataIdentifier()
        {
            RestRequest request = new($"/processdata") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<ProcessDataIdentifier>>(request);
        }  

        /// <summary>
        /// Returns list of all available process-data identifiers of a module
        /// </summary>
        /// <param name="module">Module object</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifiers(Module module)
        {
            if (module == null)
                throw new ArgumentException($"{nameof(module)} can't be null.");
           
            return await GetProcessDataIdentifiers(module.Id);
        }

        /// <summary>
        /// Returns list of all available process-data identifiers of a module
        /// </summary>
        /// <param name="moduleId">Module id</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifiers(string moduleId)
        {        
            if (string.IsNullOrEmpty(moduleId))
                throw new ArgumentException($"{nameof(moduleId)} can't be null or empty.");

            RestRequest request = new($"/processdata/{moduleId}") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<ProcessModuleData>>(request);
        }

        /// <summary>
        /// Returns specified process-data value of a module
        /// </summary>
        /// <param name="module">Module object</param>
        /// <param name="processData">Process data object</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifier(Module module, ProcessData processData)
        {
            if (module == null)
                throw new ArgumentException($"{nameof(module)} can't be null.");
            if (processData == null)
                throw new ArgumentException($"{nameof(processData)} can't be null.");

            return await GetProcessDataIdentifier(module.Id, processData.Id);
        }      
        
        /// <summary>
        /// Returns specified process-data value of a module
        /// </summary>
        /// <param name="moduleId">Module id</param>
        /// <param name="processDataId">Process data id</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifier(string moduleId, string processDataId)
        {     
            if (string.IsNullOrEmpty(moduleId))
                throw new ArgumentException($"{nameof(moduleId)} can't be null or empty.");
            
            if (string.IsNullOrEmpty(processDataId))
                throw new ArgumentException($"{nameof(processDataId)} can't be null or empty.");

            _client.CheckAuthentication();

            RestRequest request = new($"/processdata/{moduleId}/{processDataId}") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<ProcessModuleData>>(request);
        }  

        /// <summary>
        /// Returns specified process-data value of a module
        /// </summary>
        /// <param name="module">Module object</param>
        /// <param name="processDatas">List of process data object</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifiers(Module module, IList<ProcessData> processDatas)
        {
            if (module == null)
                throw new ArgumentException($"{nameof(module)} can't be null.");

            if (processDatas == null || !processDatas.Any())
                throw new ArgumentException($"{nameof(processDatas)} can't be null or empty.");

            return await GetProcessDataIdentifiers(module.Id, processDatas.Select(p => p.Id).ToList());
        }
   
        /// <summary>
        /// Returns specified process-data value of a module
        /// </summary>
        /// <param name="moduleId">Module id</param>
        /// <param name="processDatasIds">List of process data ids</param>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifiers(string moduleId, IList<string> processDatasIds)
        {
            if (string.IsNullOrEmpty(moduleId))
                throw new ArgumentException($"{nameof(moduleId)} can't be null or empty.");
            
            if (processDatasIds == null || !processDatasIds.Any() || processDatasIds.Any(string.IsNullOrEmpty))
                throw new ArgumentException($"{nameof(processDatasIds)} can't be null, empty or contains null or empty items.");

            _client.CheckAuthentication();

            RestRequest request = new($"/processdata/{moduleId}/{processDatasIds.Aggregate((i, j) => i + "," + j)}") {RequestFormat = DataFormat.Json};
            RestResponse<List<ProcessModuleData>> response = await _client.ExecuteGetAsync<List<ProcessModuleData>>(request);
            return response.Data;
        }

        /// <summary>
        /// Returns specified process-data value of a module
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProcessModuleData>> GetProcessDataIdentifiersFilter(List<ProcessDataIdentifier> filters)
        {
            if (filters == null || !filters.Any())
                throw new ArgumentException($"{nameof(filters)} can't be null or empty.");

            _client.CheckAuthentication();

            RestRequest request = new($"/processdata") { RequestFormat = DataFormat.Json };
            request.AddJsonBody(filters);
            return await _client.PostAsync<List<ProcessModuleData>>(request);
        }
    }
}