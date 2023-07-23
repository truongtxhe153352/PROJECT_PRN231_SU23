using BusinessObjects.DTO;
using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        public void DeleteMaterial(int materialId) => MaterialDao.DeleteMaterial(materialId);

        public Material GetMaterialById(int materialId) => MaterialDao.GetMaterialById(materialId);

        public IEnumerable<Material> GetMaterialsByCourseId(int courseId) => MaterialDao.GetMaterialsByCourseId(courseId);

        public void SaveMaterial(IFormFile material, string materialPath, int courseId, int uploaderId, string materialName) 
            => MaterialDao.SaveMaterial(material, materialPath, courseId, uploaderId, materialName);
   
    
    }
}
