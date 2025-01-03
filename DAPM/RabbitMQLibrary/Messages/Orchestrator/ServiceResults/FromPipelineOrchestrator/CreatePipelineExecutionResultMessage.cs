﻿using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLibrary.Models;

/// <author>Tamás Drabos</author>

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator
{
    public class CreatePipelineExecutionResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public bool Succeeded { get; set; }

        public Guid PipelineExecutionId { get; set; }

        public PipelineExecution? PipelineExecution { get; set; }
    }
}
