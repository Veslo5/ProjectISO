using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.ErrorHandling
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

            sb.Append(ex.Message + Environment.NewLine);

            while (ex.InnerException != null)
            {
                sb.Append(ex.Message + Environment.NewLine);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
