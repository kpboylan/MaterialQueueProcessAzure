using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialQueueProcessAzure.Enum
{
    public class MessageQueue
    {
        public enum MessageQueueName
        {
            [Description("AddMaterialQueue")]
            AddMaterialQueue,

            [Description("UpdateMaterialQueue")]
            UpdateMaterialQueue,
        }

        public enum AzureQueueName
        {
            [Description("materialqueue")]
            materialqueue
        }

        public enum MessageHostName
        {
            [Description("Localhost")]
            LocalHost,
        }

        public enum DockerMessageHostName
        {
            [Description("rabbitmq")]
            rabbitmq,
        }
    }
}
