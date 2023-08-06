using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;  // Windows PowerShell namespace.
using System.Threading;              // Thread pool namespace for posting work.
using System.Diagnostics;            // Diagnostics namespace for retrieving
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.IO.MemoryMappedFiles;
using System.Xml.Linq;
using System.IO;
using MQTTnet.Protocol;

namespace ZendureShell
{
    [Cmdlet(VerbsCommon.New, "ZendureShell")]
    [OutputType(typeof(string))]


    public sealed class ZendureShell : PSCmdlet
    {
        [Parameter()]
        public SwitchParameter AsJob
        {
            get { return asjob; }
            set { asjob = value; }
        }
        private bool asjob;
        private SampleJob job = new SampleJob("New-Zendureshell");
        protected override void BeginProcessing()
        {

            base.BeginProcessing();

        }



        protected override void ProcessRecord()
        {
           // base.ProcessRecord();

            if (asjob)
            {
                JobRepository.Add(job);
                WriteObject(job);
                ThreadPool.QueueUserWorkItem(WorkItem);
            }
            else
            {
                job.ProcessJob();
                foreach (PSObject p in job.Output)
                {
                    WriteObject(p);
                }
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }


        private class SampleJob : Job
        {
            IManagedMqttClient managedMqttClient;
            System.IO.MemoryMappedFiles.MemoryMappedFile publishMemoryMappedFile;
            EventWaitHandle eventWaitHandle;


            internal SampleJob(string command)
                : base(command)
            {
                SetJobState(JobState.NotStarted);
                var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithCredentials("xxxx", "xxxx")
                    .WithTcpServer("mqtt.zen-iot.com", 1883)
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                    .WithCleanSession(true)
                    .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)).Build();
                

                string appKey = "xxxxxxx";

                managedMqttClient = new MqttFactory().CreateManagedMqttClient();
                
                managedMqttClient.StartAsync(options);

                if (!managedMqttClient.IsConnected) while (managedMqttClient.IsConnected == false) { Task.Delay(250); };

                managedMqttClient.SubscribeAsync($"{appKey}/#");
                managedMqttClient.SubscribeAsync("/server/app/xxxxx/#"); 

                managedMqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Output.Add(PSObject.AsPSObject($"{e.ApplicationMessage.Topic} : {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}{Environment.NewLine}{Environment.NewLine}"));
                    return Task.CompletedTask;
                };


                publishMemoryMappedFile = MemoryMappedFile.CreateOrOpen("ZendureShellMemory", Constants.MEMORY_MAPPED_FILESIZE, MemoryMappedFileAccess.ReadWrite);
                

                Task.Factory.StartNew(() =>
                {
                    eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "ZendureShellEventHandler");

                    while (1 == 1)
                    {
                        try
                        {
                            eventWaitHandle.WaitOne();

                            using (MemoryMappedViewStream vs = publishMemoryMappedFile.CreateViewStream(0, Constants.MEMORY_MAPPED_FILESIZE, MemoryMappedFileAccess.Read))
                            {

                                using (System.IO.StreamReader sr = new StreamReader(vs))
                                {
                                    string? command = sr.ReadToEnd().Replace("\0", string.Empty);
                                    string topic = command.Split('|')[0].Replace("[AppKey]", "xxxxxx").Replace("[DeviceId]", "xxxxxxx");
                                    string data = command.Split('|')[1];

                                    var message = new MqttApplicationMessageBuilder()
                                        .WithTopic(topic)
                                        .WithPayload(data)
                                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                        .Build();
                                  
                                    var x = managedMqttClient.InternalClient.PublishAsync(message).Result;
                                    Console.WriteLine("{0} : {1}", x.IsSuccess, x.ReasonString);
                                    Console.WriteLine(topic + " " + data);
                                    Console.WriteLine(command.Length);

                                    Task.Delay(1000); // Wait for 1 second
                                }
                            }
                            using (MemoryMappedViewStream vs = publishMemoryMappedFile.CreateViewStream(0, Constants.MEMORY_MAPPED_FILESIZE, MemoryMappedFileAccess.ReadWrite))
                            {
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(vs))
                                {
                                    {
                                        sw.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                                        sw.BaseStream.Write(new byte[Constants.MEMORY_MAPPED_FILESIZE], 0, Constants.MEMORY_MAPPED_FILESIZE);
                                        sw.Flush();
                                    }
                                }
                            }
                            
                            eventWaitHandle.Reset();
                            
                        }
                        catch (Exception ex)
                        {
                            Output.Add($"Error: {ex.InnerException.Message}");
                        }
                    }
                });
            }
            public override string StatusMessage
            {
                get { throw new NotImplementedException(); }
            }

            public override bool HasMoreData
            {
                get
                {
                    return hasMoreData;
                }
            }
            private bool hasMoreData = true;

            public override string Location
            {
                get { throw new NotImplementedException(); }
            }

            public override void StopJob()
            {
                SetJobState(JobState.Completed); ;
            }

            internal void ProcessJob()
            {
                SetJobState(JobState.Running);
                DoProcessLogic();
                //    SetJobState(JobState.Completed);
            }

            // Retrieve the processes of the local computer.
            void DoProcessLogic()
            {
                while (managedMqttClient != null)
                {
                    Thread.Sleep(2000);
                    hasMoreData = true;
                }

                Output.Complete();
            } 
        }

        void WorkItem(object dummy)
        {
            job.ProcessJob();
        }
    }
}