using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Web.DtoModels.PackageModels;
public class ReassignPackageRequestModel
{
    [Required]
    [StringLength(1)]
    public string PackageId { get; set; }

    [StringLength(1)]
    [MaybeNull]
    public string Assignee { get; set; }

    [MaybeNull]
    public List<string>? Versions { get; set; } = null;
}

public class ReassignPackageResponseModel
{
    [Required]
    [StringLength(1)]
    public string PackageId { get; set; }

    [StringLength(1)]
    public string Assignee { get; set; }
    public long ReassignedCount { get; set; }
}
