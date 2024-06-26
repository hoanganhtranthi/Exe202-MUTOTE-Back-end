﻿
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using MuTote.Application.Services.ISerive;
using Stream = System.IO.Stream;
using Task = System.Threading.Tasks.Task;

namespace MuTote.Application.Service
{
    public class FirebaseStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;
        private String ApiKey;
        private static string Bucket;
        private static string AuthEmail;
        private static string AuthPassword;
        public FirebaseStorageService(IConfiguration config)
        {
            _config = config;
            ApiKey = _config["Firebase:ApiKey"];
            Bucket = _config["Firebase:Bucket"];
            AuthEmail = _config["EmailUserName"];
            AuthPassword = _config["EmailPassword"];
        }

        public async Task<string> UploadFileToDefaultAsync(Stream fileStream, string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                ).Child("images").Child(fileName).PutAsync(fileStream, cancellation.Token);
            try
            {
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
    }
}
