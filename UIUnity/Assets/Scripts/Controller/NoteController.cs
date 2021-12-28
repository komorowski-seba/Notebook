using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Interface;
using Common.Scriptable;
using Model.Note;
using Newtonsoft.Json;
using RestSharp;

namespace Controller
{
    public class NoteController : INoteController
    {
        private readonly LoginInfoModel _loginInfoModel;
        private readonly ILoginController _loginController;
        private int _currentPackPosition = 0;

        public NoteController(LoginInfoModel loginInfoModel, ILoginController loginController)
        {
            _loginInfoModel = loginInfoModel;
            _loginController = loginController;
        }

        public async Task<IList<NoteModel>> GetFirstNotesPackAsync(int size)
        {
            _currentPackPosition = 0;
            var result = await GetNotesPackAsync(size);
            return result;
        }

        public async Task<IList<NoteModel>> GetNotesPackAsync(int size)
        {
            var result = await GetNotesAsync(_currentPackPosition, size);
            if (result == null || result.Count <= 0) 
                return new List<NoteModel>();
            
            ++_currentPackPosition;
            return result;
        }

        private async Task<IList<NoteModel>> GetNotesAsync(int position, int size)
        {
            var client = new RestClient($"{_loginInfoModel.urDomain}Note?position={_currentPackPosition}&size={size}")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            var request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", $"Bearer {_loginController.Token}", ParameterType.HttpHeader);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception($"Connection error to {_loginInfoModel.urDomain}");
            
            var notes = JsonConvert.DeserializeObject<IList<NoteModel>>(response.Content);
            return notes ?? new List<NoteModel>();
        }
        
        public async Task<(string, string)> AddNewNote(NoteModel value)
        {
            var client = new RestClient($"{_loginInfoModel.urDomain}Note")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var request = new RestRequest(Method.POST) {RequestFormat = DataFormat.Json};
            request.AddParameter("Authorization", $"Bearer {_loginController.Token}", ParameterType.HttpHeader);
            request.AddJsonBody(new 
            {
                topic = value.Topic,
                desc = value.Desc
            });
        
            var response = await client.ExecuteAsync(request);
            return (response.IsSuccessful ? response.Content : null, 
                !string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.StatusDescription);
        }
    }
}