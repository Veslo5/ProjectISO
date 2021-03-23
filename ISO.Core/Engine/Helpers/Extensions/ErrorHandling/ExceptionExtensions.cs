using System;
using System.Text;

namespace ISO.Core.Engine.Helpers.Extensions.ErrorHandling
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets all extensions and recursively their innerexceptions
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllInnerExceptions(this Exception ex)
        {
            var sb = new StringBuilder();

            sb.Append(ex.Message);

            while (ex.InnerException != null)
            {
                sb.Append(ex.Message);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
