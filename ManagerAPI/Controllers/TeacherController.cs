using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;

namespace ManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        //private readonly ICourseRepository _teacherRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly IMaterialRepository _materialRepository;
        //private readonly IAssignmentRespository _assignmentRespository;
        //private readonly ISubmitAssignmentRespository _submitAssignmentRespository;
        //private string BaseMaterailUrl = "";
        //private readonly IMapper _mapper;

    //    public TeacherController(CourseRepository teacherRepository, UserRepository userRepository, MaterialRepository materialRepository,
    //AssignmentRepository assignmentRespository, SubmitAssignmentRespository submitAssignmentRespository,
    //IMapper mapper)
    //    {
    //        _teacherRepository = teacherRepository;
    //        _userRepository = userRepository;
    //        _materialRepository = materialRepository;
    //        _assignmentRespository = assignmentRespository;
    //        _submitAssignmentRespository = submitAssignmentRespository;
    //        _mapper = mapper;
    //        BaseMaterailUrl = "../ManagerAPI/wwwroot/AllFiles/Materials";
    //    }

        private readonly ICourseRepository _teacherRepository = new CourseRepository();
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly IMaterialRepository _materialRepository = new MaterialRepository();
        private readonly IAssignmentRespository _assignmentRespository = new AssignmentRepository();
        private readonly ISubmitAssignmentRespository _submitAssignmentRespository = new SubmitAssignmentRespository();
        private string BaseMaterailUrl = "";
        private readonly IMapper _mapper;

        public TeacherController(IMapper mapper)
        {
            _mapper = mapper;
            BaseMaterailUrl = "../ProjectApi/wwwroot/AllFiles/Materials";
        }


        [HttpGet("courses/{teacherId}")]
        public IActionResult GetAllCourses(int teacherId)
        {
            List<Course> courses = (List<Course>)_teacherRepository.GetAllCourseByTeacherId(teacherId);
            if (courses == null || courses.Count == 0)
            {
                return NotFound();
            }
            List<CourseDto> courseDtos = new List<CourseDto>();
            foreach (var c in courses)
            {
                courseDtos.Add(_mapper.Map<Course, CourseDto>(c));

            }
            //return Ok(_mapper.Map<List<CourseDto>>(courses));
            return Ok(courseDtos);
        }


        //================================================MATERIAL====================================================
        [HttpGet("{courseId}")]
        public IActionResult GetAllMaterialsByCourse(int courseId)
        {
            List<Material> materials = (List<Material>)_materialRepository.GetMaterialsByCourseId(courseId);
            if (materials == null || materials.Count == 0)
            {
                return NotFound();
            }
            List<MaterialDto> materialDtos = new List<MaterialDto>();
            return Ok(_mapper.Map<List<MaterialDto>>(materials));
        }

        [HttpPost]
        public IActionResult UploadMaterial([FromForm] List<IFormFile> files, [FromForm] int courseId, [FromForm] int uploaderId)
        {
            foreach (var file in files)
            {
                UploadMaterialViewModel uploadMaterialViewModel = new UploadMaterialViewModel();
                uploadMaterialViewModel.Material = file;
                uploadMaterialViewModel.CourseId = courseId;
                uploadMaterialViewModel.UploaderId = uploaderId;
                uploadMaterialViewModel.MaterialPath = BaseMaterailUrl;
                uploadMaterialViewModel.MaterialName = file.FileName;
                _materialRepository.SaveMaterial(uploadMaterialViewModel.Material, uploadMaterialViewModel.MaterialPath,
                    uploadMaterialViewModel.CourseId, uploadMaterialViewModel.UploaderId, uploadMaterialViewModel.MaterialName);
            }
            return Ok();
        }

        [HttpDelete("{materialId}")]
        public IActionResult DeleteMaterial(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            if (System.IO.File.Exists(material.Path + "/" + material.MaterialName))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(material.Path + "/" + material.MaterialName);
            }
            _materialRepository.DeleteMaterial(materialId);
            return Ok();
        }
    }
}
