using Microsoft.OpenApi.Any;
using new_doctors.DTOs;
using new_doctors.models;
using new_doctors.models.backend.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace WebApplication1.packages
{
    public class pkg_doctors : pkg_base
    {
        public pkg_doctors(string connectionString) : base(connectionString)
        {
        }

        public void AddDoctor(Doctor doctor)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.add_doctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = doctor.FirstName;
                        command.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = doctor.LastName;
                        command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = doctor.Email;

                        // Hash the password before storing it
                        string hashedPassword = PasswordHelper.HashPassword(doctor.Password);
                        command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = hashedPassword;

                        command.Parameters.Add("p_private_number", OracleDbType.Varchar2).Value = doctor.PrivateNumber;
                        command.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctor.Category;
                        command.Parameters.Add("p_image_url", OracleDbType.Varchar2).Value = doctor.ImageUrl;
                        command.Parameters.Add("p_cv_url", OracleDbType.Varchar2).Value = doctor.CVUrl;
                        command.Parameters.Add("p_achievments", OracleDbType.Varchar2).Value = doctor.Achievements;

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (OracleException ex)
                        {
                            // Handle Oracle exceptions
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding doctor", ex);
            }
        }


        public void DeleteDoctor(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.delete_doctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public Doctor GetDoctor(int id)
        {
            Doctor doctor = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.get_doctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctor = new Doctor
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                FirstName =  reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                PrivateNumber = reader["PRIVATE_NUMBER"].ToString(),
                                Password =  reader["password"].ToString(),
                                Category =  reader["category"].ToString(),
                                ImageUrl =  reader["image_url"].ToString(),
                                CVUrl =  reader["cv_url"].ToString(),
                                Achievements =  reader["achievments"].ToString(),

                            };
                        }
                    }
                }
            }
            return doctor;
        }
        public void EditDoctor(int id, Doctor doctor)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.edit_doctor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    command.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = doctor.FirstName;
                    command.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = doctor.LastName;
                    command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = doctor.Email;
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = PasswordHelper.HashPassword(doctor.Password);
                    command.Parameters.Add("p_private_number", OracleDbType.Varchar2).Value = doctor.PrivateNumber;
                    command.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctor.Category;
                    command.Parameters.Add("p_image_url", OracleDbType.Varchar2).Value = doctor.ImageUrl;
                    command.Parameters.Add("p_cv_url", OracleDbType.Varchar2).Value = doctor.CVUrl;
                    command.Parameters.Add("p_achievments", OracleDbType.Varchar2).Value = doctor.Achievements;

                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Doctor>  GetDoctors()
        {
            var doctors = new List<Doctor>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.get_all_doctors", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new Doctor
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                FirstName =  reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                PrivateNumber = reader["PRIVATE_NUMBER"].ToString(),
                                Password =  reader["password"].ToString(),
                                Category =  reader["category"].ToString(),
                                ImageUrl =  reader["image_url"].ToString(),
                                CVUrl =  reader["cv_url"].ToString(),
                                Achievements =  reader["achievments"].ToString(),
                               
                            });
                        }
                    }
                }
            }
            return doctors;
        }
        public LoginResponseDto Login(string email, string password)
        {
            LoginResponseDto loginResponse = null;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("login_user", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = password;
                    command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["password"].ToString();
                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                loginResponse = new LoginResponseDto
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    FirstName = reader["first_name"].ToString(),
                                    LastName = reader["last_name"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Role = reader["role"].ToString(),
                                    ImageUrl = reader["image_url"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            return loginResponse;
        }


        public void RegisterUser(User user)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.register_user", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = user.FirstName;
                    command.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = user.LastName;
                    command.Parameters.Add("p_private_number", OracleDbType.Varchar2).Value = user.PrivateNumber;
                    command.Parameters.Add("p_created_at", OracleDbType.Varchar2).Value = user.CreatedAt;
                    command.Parameters.Add("p_reset_code", OracleDbType.Varchar2).Value = user.ResetCode;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.delete_user", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateAppointment(Appointment appointment)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.create_appointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_user_id", OracleDbType.Int32).Value = appointment.UserId;
                    command.Parameters.Add("p_doctor_id", OracleDbType.Int32).Value = appointment.DoctorId;
                    command.Parameters.Add("p_date", OracleDbType.Date).Value = appointment.Date;
                    command.Parameters.Add("p_problem", OracleDbType.Varchar2).Value = appointment.Problem;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAppointment(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand("PGK_SABA_DOCTORS_APP.delete_appointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }
















    }
}
