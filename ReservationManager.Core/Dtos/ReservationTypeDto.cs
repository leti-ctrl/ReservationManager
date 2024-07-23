﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ReservationTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public TimeOnly Start {  get; set; }
        public TimeOnly End { get; set; }
    }
}
