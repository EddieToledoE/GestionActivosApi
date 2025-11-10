using GestionActivos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace GestionActivos.Infrastructure.Services
{
    public class MinioStorageService : IFileStorageService
    {
        private readonly IMinioClient _client;
        private readonly string _bucket;

        public MinioStorageService(IConfiguration config)
        {
            _bucket = config["Minio:BucketName"] ?? "activos-bucket";
            bool useSSL = bool.TryParse(config["Minio:UseSSL"], out var ssl) && ssl;

            var clientBuilder = new MinioClient()
                .WithEndpoint(config["Minio:Endpoint"])
                .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"]);

            if (useSSL)
                clientBuilder = clientBuilder.WithSSL();

            _client = clientBuilder.Build();
        }

        public async Task<string> UploadAsync(byte[] fileBytes, string fileName, string contentType)
        {
            bool bucketExists = await _client.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucket)
            );

            if (!bucketExists)
                await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket));

            using var stream = new MemoryStream(fileBytes);

            await _client.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(contentType)
            );

            return $"{_client.Config.Endpoint}/{_bucket}/{fileName}";
        }

        public async Task DeleteAsync(string fileName)
        {
            await _client.RemoveObjectAsync(
                new RemoveObjectArgs().WithBucket(_bucket).WithObject(fileName)
            );
        }
    }
}
