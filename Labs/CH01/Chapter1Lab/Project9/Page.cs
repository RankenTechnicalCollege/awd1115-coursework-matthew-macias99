﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    public class Page : ITurnable
    {
        public string Turn()
        {
            return "Page - grab any edge of the page and drage it over the to left or right";
        }
    }
}
