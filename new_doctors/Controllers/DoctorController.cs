using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using new_doctors.DTOs;
using new_doctors.models;
using new_doctors.models.backend.Models;
using WebApplication1.packages;

namespace new_doctors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly pkg_doctors _pkg_doctors;
       public DoctorController(pkg_doctors pkg_doctor)
        {
            _pkg_doctors = pkg_doctor;
        }

        [HttpPost]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            _pkg_doctors.AddDoctor(doctor);

            return Ok(new { message = "Employee added succesfully", doctor });
    }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _pkg_doctors.DeleteDoctor(id);
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetDoctor(int id)
        {
            try
            {
                var doctor = _pkg_doctors.GetDoctor(id);
                if (doctor == null)
                {
                    return NotFound(new { message = "Doctor not found." });
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public IActionResult EditDoctor(int id, [FromBody] Doctor doctor)
        {
            try
            {
                _pkg_doctors.EditDoctor(id, doctor);
                return Ok(new { message = "Doctor updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        public IActionResult GetDoctors() 
        {
              var doctors = _pkg_doctors.GetDoctors();
            return Ok(doctors); 
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginRequest)
        {
            var loginResponse = _pkg_doctors.Login(loginRequest.Email, loginRequest.Password);
            if (loginResponse != null)
            {
                return Ok(loginResponse);
            }
            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpPost("register/user")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            try
            {
                _pkg_doctors.RegisterUser(user);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _pkg_doctors.DeleteUser(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("appointment")]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                _pkg_doctors.CreateAppointment(appointment);
                return Ok(new { message = "Appointment created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("appointment/{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            try
            {
                _pkg_doctors.DeleteAppointment(id);
                return Ok(new { message = "Appointment deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }







}
