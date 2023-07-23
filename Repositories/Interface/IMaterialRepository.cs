using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;

namespace Repositories.Interface
{
    public interface IMaterialRepository
    {
        IEnumerable<Material> GetMaterialsByCourseId(int courseId);
        void SaveMaterial(IFormFile material, string materialPath, int courseId, int uploaderId, string materialName);
        Material GetMaterialById(int materialId);
        void DeleteMaterial(int materialId);
    }
}
