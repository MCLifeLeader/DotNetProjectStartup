﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Startup.Data.Models.Db.dboSchema;


namespace Startup.Data.Models.Db.dboSchema;

public partial class UserToAgency
{
    public string UserId { get; set; }

    public Guid AgencyId { get; set; }

    public virtual Agency Agency { get; set; }

    public virtual AspNetUser User { get; set; }
}