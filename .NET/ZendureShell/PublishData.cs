using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ZendureShell
{
    [Cmdlet(VerbsCommon.Push, "ZendureCommand")]
    [OutputType(typeof(string))]
    public class PublishData : Cmdlet
    {
        private string messageType = string.Empty;


        [Parameter(Mandatory = true)]
        [ValidateSet("SetOutputLimit", "SetInputLimit", "SetInverseMaxPower")]
        public string MessageType { get { return messageType; } set { messageType = value; } }


        public PublishData() : base() { }

        protected override void BeginProcessing()
        {

            base.BeginProcessing();
        }
        protected override void ProcessRecord() 
        { 
            switch(messageType)
            {
                case "SetOutputLimit":
                    SendMemoryMappedMessage("3232", "2323");
                    
                    break;

                default: break;
            }
        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }


        private void SendMemoryMappedMessage(string topic, string data)
        {


            using (MemoryMappedFile publishMemoryMappedFile = MemoryMappedFile.CreateOrOpen("ZendureShellMemory", Constants.MEMORY_MAPPED_FILESIZE, MemoryMappedFileAccess.ReadWrite))
            {
                using (EventWaitHandle eventWaitHandle = EventWaitHandle.OpenExisting("ZendureShellEventHandler"))
                {
                    using (MemoryMappedViewStream vs = publishMemoryMappedFile.CreateViewStream())
                    {
                        using (StreamWriter sw = new StreamWriter(vs))
                        {
                        //    var message = new Mqtt_Template_SetProperty("outputLimit", 200);
                            var message = new Mqtt_Template_SetProperty("outputLimit", 300);
                            sw.Write(string.Format("{0}|{1}", message.Topic, message.Data));
                            sw.Flush();
                        }
                        eventWaitHandle.Set();
                    }
                }
            }
        }

    }






}
