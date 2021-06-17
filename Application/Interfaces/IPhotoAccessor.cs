using System.Threading.Tasks;
using Application.Photos;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPhotoAccessor
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
        Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams);
        Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix);
        Task<DeleteFolderResult> DeleteFolderAsync(string prefix);
    }
}