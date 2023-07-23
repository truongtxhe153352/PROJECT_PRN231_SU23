using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using BusinessObjects.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Repositories;
using Repositories.Interface;

namespace ManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ICourseRepository _teacherRepository = new CourseRepository();
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly IMaterialRepository _materialRepository = new MaterialRepository();
        private readonly IAssignmentRespository _assignmentRespository = new AssignmentRepository();
        private readonly ISubmitAssignmentRespository _submitAssignmentRespository = new SubmitAssignmentRespository();
        private string BaseMateriallUrl = "";
        private readonly IMapper _mapper;

        public TeacherController(IMapper mapper)
        {
            _mapper = mapper;
            BaseMateriallUrl = "../ManagerAPI/wwwroot/AllFiles/Materials";
        }


        [HttpGet("Courses/{teacherId}")]
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
            return Ok(courseDtos);
        }

        [HttpGet("getByEmail/{email}")]
        public IActionResult GetTeacherByEmail(string email)
        {
            User user = _userRepository.GetUserByEmail(email);
            UserDto userDto = _mapper.Map<User, UserDto>(user);
            return Ok(userDto);
        }

        //================================================MATERIAL====================================================
        [HttpGet("Materials/{courseId}")]
        public IActionResult GetAllMaterialsByCourse(int courseId)
        {
            List<Material> materials = (List<Material>)_materialRepository.GetMaterialsByCourseId(courseId);
            List<MaterialDto> materialDtos = new List<MaterialDto>();
            foreach (var material in materials)
            {
                materialDtos.Add(_mapper.Map<Material, MaterialDto>(material));
            }
            return Ok(materialDtos);
        }

        [HttpPost("Materials")]
        public IActionResult UploadMaterial([FromForm] List<IFormFile> files, [FromForm] int courseId, [FromForm] int uploaderId)
        {
            foreach (var file in files)
            {
                UploadMaterialViewModel uploadMaterialViewModel = new UploadMaterialViewModel();
                uploadMaterialViewModel.Material = file;
                uploadMaterialViewModel.CourseId = courseId;
                uploadMaterialViewModel.UploaderId = uploaderId;
                uploadMaterialViewModel.MaterialPath = BaseMateriallUrl;
                uploadMaterialViewModel.MaterialName = file.FileName;
                _materialRepository.SaveMaterial(uploadMaterialViewModel.Material, uploadMaterialViewModel.MaterialPath,
                    uploadMaterialViewModel.CourseId, uploadMaterialViewModel.UploaderId, uploadMaterialViewModel.MaterialName);
            }
            return Ok();
        }

        [HttpDelete("Materials/{materialId}")]
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



        //=============================================================ASSIGNMENT==============================================================

        [HttpPost]
        [Route("Assignments")]
        public IActionResult UploadAssigmentNewest(IFormFile file, [FromForm] int courseId, [FromForm] int uploaderId)
        {
            UploadAssignmentViewModel uploadAssignmentViewModel = new UploadAssignmentViewModel();
            uploadAssignmentViewModel.Assignment = file;
            uploadAssignmentViewModel.CourseId = courseId;
            uploadAssignmentViewModel.UploaderId = uploaderId;
            string []x = file.FileName.Trim().Split("-");

            uploadAssignmentViewModel.AssignmentName = x[0];
            uploadAssignmentViewModel.RequiredDate = DateTime.Parse(x[1]);
            _assignmentRespository.SaveAssignment(uploadAssignmentViewModel);
            return Ok();
        }

        [HttpGet("Assignments/{teacherId}/{courseId}")]
        public ActionResult<IEnumerable<AssigmentDto>> ListAssignmentByCourse(int teacherId, int courseId)
        => _assignmentRespository.ListAssignmentByTeacherAndCourse(teacherId, courseId).Select(_mapper.Map<Assignment, AssigmentDto>).ToList();


        [HttpGet("Assignments/{assId}/list-submit")]
        public ActionResult<IEnumerable<SubmitAssignmentDto>> ListSubmitAssignmentByCourse(int assId)
        => _submitAssignmentRespository.ListSubmitAssignmentByAssId(assId).Select(_mapper.Map<SubmitAssignment, SubmitAssignmentDto>).ToList();

        [HttpGet("Assignments/download/{id}")]
        public async Task<IActionResult> DownloadSubmitAssignmentById(int id)
        {
            SubmitAssignmentDto assigmentDto = (SubmitAssignmentDto)_mapper.Map<SubmitAssignmentDto>(_submitAssignmentRespository.GetSubmitAssignmentsById(id));
            //var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            if (assigmentDto != null)
            {
                var filepath = assigmentDto.Path;
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filepath, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }
                var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
                return File(bytes, contenttype, Path.GetFileName(filepath));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
