﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class ExecuteOperatorActionDTO
    {
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }    
    }
}
