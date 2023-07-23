using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Repositories;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.StaticFiles;
using BusinessObjects.ViewModel;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using Microsoft.Win32;

namespace ManagerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StudentController : Controller
    {
        private readonly ICourseRepository _courseRepository = new CourseRepository();
        private readonly IMaterialRepository _materialRepository = new MaterialRepository();
        private readonly IAssignmentRespository _assignmentRespository = new AssignmentRepository();
        private readonly ISubmitAssignmentRespository _submitAssignmentRespository = new SubmitAssignmentRespository();
        private readonly IUserRepository _userRepository = new UserRepository();

        private readonly IMapper _mapper;

        public StudentController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCourse()
        {
            List<Course> courses = _courseRepository.GetAllCourse();
            List<CourseDto> courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                courseDtos.Add(_mapper.Map<Course, CourseDto>(course));
            }
            return Ok(courseDtos);
        }

        [HttpGet("{studentId}")]
        public IActionResult GetAllCourses(int studentId)
        {
            List<Course> courses = (List<Course>)_courseRepository.GetAllCourseByStudentId(studentId);
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

        [HttpGet("{email}")]
        public IActionResult GetStudentByEmail(string email)
        {
            User user = _userRepository.GetUserByEmail(email);
            UserDto userDto = _mapper.Map<User, UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet("{courseId}")]
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

        [HttpGet("{courseId}")]
        public ActionResult<IEnumerable<AssigmentDto>> GetAssignmentsByCourse(int courseId)
        => _assignmentRespository.GetAssignmentsByCourseId(courseId)
            .Select(_mapper.Map<Assignment, AssigmentDto>).ToList();


        [HttpGet("{AssignmentId}")]
        public async Task<IActionResult> DownloadAssignmentByassId(int AssignmentId)
        {
            AssigmentDto assigmentDto = (AssigmentDto)_mapper.Map<AssigmentDto>(_assignmentRespository.GetAssignmentsByAssId(AssignmentId));
            //var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
            var filepath = assigmentDto.Path;
            //để xác định kiểu nội dung (content type) của tệp (file) dựa trên phần mở rộng (extension) của tên tệp
            var provider = new FileExtensionContentTypeProvider();
            // tìm kiếm kiểu nội dung tương ứng
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            // đọc và trả về mảng bytes chứa nội dung của tệp, giá trị này sẽ được gán vào biến bytes.
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            //trả về nội dung của tệp (file) dưới dạng phản hồi HTTP khi một yêu cầu tải xuống được thực hiện đến API
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }


        //IFormFile là một interface trong ASP.NET Core được sử dụng để đại diện cho một tệp (file)
        //được gửi lên từ một yêu cầu HTTP thông qua phương thức POST.
        //[FromForm] cho phép trích xuất dữ liệu từ phần thân của yêu cầu HTTP thông qua
        //phương thức POST, và các tham số được gắn[FromForm] sẽ được đối tượng môi trường
        //ASP.NET Core điền dữ liệu vào dựa trên các tên tham số và các trường dữ liệu trong
        //phần thân yêu cầu.
        [HttpPost]
        public IActionResult submitAssignment(IFormFile file, [FromForm] int assignmentId, [FromForm] int uploaderId)
        {
            SubmitAssignmentViewModel submitAssignmentViewModel = new SubmitAssignmentViewModel();
            submitAssignmentViewModel.SubmitFile = file;
            submitAssignmentViewModel.AssignmentId = assignmentId;
            submitAssignmentViewModel.UploaderId = uploaderId;
            _submitAssignmentRespository.SubmitAssignment(submitAssignmentViewModel);
            return Ok();
        }

        [HttpGet("{materialId}")]
        public IActionResult DowloadMaterial(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            string materialPath = material.Path + "/" + material.MaterialName;
            var fileExtension = Path.GetExtension(material.MaterialName);
            var contentType = Registry.GetValue(@"HKEY_CLASSES_ROOT\" + fileExtension, "Content Type", null) as string;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            Byte[] b = System.IO.File.ReadAllBytes(materialPath);
            return File(b, contentType, material.MaterialName);
        }

    }
}
