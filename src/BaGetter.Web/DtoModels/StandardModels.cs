using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Web.DtoModels.StandardModels;
internal class StatusMessageModel
{
    public StatusMessageModel(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}
