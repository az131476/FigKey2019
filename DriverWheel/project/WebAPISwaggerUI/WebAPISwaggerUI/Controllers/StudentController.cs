using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
namespace WebAPISwaggerUI.Controllers
{
    /// <summary>
    /// 学生信息
    /// </summary>
    public class StudentController : ApiController
    {
        /// <summary>
        /// 得到所有的学生信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new List<string>() { "student A", "student B" };
        }
        /// <summary>
        /// 根据学生编号得到学生信息
        /// </summary>
        /// <param name="Id">学生编号</param>
        /// <returns></returns>
        public string Get(int Id)
        {
            return "学号：" + Id;
        }
        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="studentModel">学生实体</param>
        /// <remarks>添加一个新的学生</remarks>
        /// <response code="400">Bad request </response>
        /// <response code="500">Internal Server Error</response>
        public void Post(String studentModel)
        {
        }
        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="Id">学生编号</param>
        /// <param name="studentModel">学生实体</param>
        [ResponseType(typeof(string))]
        [ActionName("UpdateStudentById")]
        public void Put(int Id, string studentModel)
        {
        }
        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="Id">学生编号</param>
        public void Delete(int Id)
        {
        }
    }
}
