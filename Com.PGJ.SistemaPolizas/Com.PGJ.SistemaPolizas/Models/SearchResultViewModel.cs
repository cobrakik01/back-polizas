﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.PGJ.SistemaPolizas.Models
{
    public class SearchResultViewModel
    {
        public IPagedList result { get; set; }
        public int total { get; set; }
    }
}